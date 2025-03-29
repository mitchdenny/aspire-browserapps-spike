using System.IO;
using Aspire.Hosting.Eventing;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

internal static class BrowserAppExtensions
{
    public static IResourceBuilder<BrowserAppResource> AddBrowserApp(this IDistributedApplicationBuilder builder, string name, string path)
    {
        var resource = new BrowserAppResource(name, path);

        var caddyConfigPath = Path.GetTempFileName();

        builder.Eventing.Subscribe<AfterEndpointsAllocatedEvent>(async (e, ct) => {

            if (!resource.TryGetEndpoints(out var endpoints))
            {
                throw new DistributedApplicationException("No endpoints");
            }

            var httpEndpoint = endpoints.OfType<EndpointAnnotation>().Single(e => e.Transport == "http");

            using var writer = new StreamWriter(caddyConfigPath);
            await writer.WriteLineAsync($"{httpEndpoint.TargetHost}:{httpEndpoint.TargetPort}");
            
            var matcherEvent = new AdvertiseCaddyMatcherEvent(resource);
            await builder.Eventing.PublishAsync(matcherEvent, ct);
            
            foreach (var matcher in matcherEvent.Matchers)
            {
                await writer.WriteLineAsync($"reverse_proxy {matcher.Matcher} host.docker.internal:{matcher.Endpoint.Port}");
            }
        });

        return builder.AddResource(resource)
                      .WithImage("caddy:latest")
                      .WithBindMount(path, "/usr/share/caddy")
                      .WithBindMount(caddyConfigPath, "/etc/caddy/Caddyfile")
                      .WithHttpEndpoint(targetPort: 80);
    }

    public static IResourceBuilder<T> WithRunCommand<T>(this IResourceBuilder<T> builder, string command, string[]? args = null, Action<IResourceBuilder<ExecutableResource>>? configureExecutable = null) where T: BrowserAppResource
    {
        var runCommand = builder.ApplicationBuilder.AddExecutable($"{builder.Resource.Name}-run-command", command, builder.Resource.Path, args)
                                                   .WithParentRelationship(builder.Resource);

        builder.ApplicationBuilder.Eventing.Subscribe<AdvertiseCaddyMatcherEvent>(builder.Resource, (e, ct) => {
            e.Matchers.Add(("/*", runCommand.GetEndpoint("http")));
            return Task.CompletedTask;
        });

        configureExecutable?.Invoke(runCommand);

        builder.WaitFor(runCommand);
        return builder;
    }

    public static IResourceBuilder<T> WithFowardingTo<T>(this IResourceBuilder<T> builder, string matcher, EndpointReference endpoint) where T: BrowserAppResource
    {
        builder.ApplicationBuilder.Eventing.Subscribe<AdvertiseCaddyMatcherEvent>(builder.Resource, (e, ct) => {
            e.Matchers.Add((matcher, endpoint));
            return Task.CompletedTask;
        });

        return builder;
    }
}

internal class BrowserAppResource(string name, string path) : ContainerResource(name), IResourceWithEndpoints
{
    public string Path { get; } = path;
}

internal class AdvertiseCaddyMatcherEvent(IResource resource) : IDistributedApplicationResourceEvent
{
    public IResource Resource => resource;
    public List<(string Matcher, EndpointReference Endpoint)> Matchers { get; } = new();
}