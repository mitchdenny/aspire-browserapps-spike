var builder = DistributedApplication.CreateBuilder(args);

var staticFilesBrowserApp = builder.AddBrowserApp("staticfiles", "../staticfilesapp");

var apiService = builder.AddProject<Projects.BrowserAppsSpike_ApiService>("apiservice");

builder.AddProject<Projects.BrowserAppsSpike_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

var nextjsBrowserApp = builder.AddBrowserApp("nextjs", "../nextjsapp")
                              .WithRunCommand("npm", ["run", "dev"], runCommand => {
                                    runCommand.WithHttpEndpoint(env: "PORT");
                              });

nextjsBrowserApp.WithFowardingTo("/api*", apiService.GetEndpoint("http"));

builder.Build().Run();
