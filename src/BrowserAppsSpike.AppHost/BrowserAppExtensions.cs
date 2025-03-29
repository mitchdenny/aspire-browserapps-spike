internal static class BrowserAppExtensions
{
    public static IResourceBuilder<BrowserAppResource> AddBrowserApp(this IDistributedApplicationBuilder builder, string name, string path)
    {
        var resource = new BrowserAppResource(name);
        return builder.AddResource(resource)
                      .WithImage("caddy:latest")
                      .WithBindMount(path, "/usr/share/caddy")
                      .WithHttpEndpoint(targetPort: 80);
    }
}

internal class BrowserAppResource(string name) : ContainerResource(name), IResourceWithEndpoints
{

}