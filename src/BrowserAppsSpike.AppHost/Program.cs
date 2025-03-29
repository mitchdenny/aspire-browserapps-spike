var builder = DistributedApplication.CreateBuilder(args);

var staticFilesBrowserApp = builder.AddBrowserApp("staticfiles", "../staticfilesapp");

var nextjsBrowserApp = builder.AddBrowserApp("nextjs", "../nextjsapp");

var apiService = builder.AddProject<Projects.BrowserAppsSpike_ApiService>("apiservice");

builder.AddProject<Projects.BrowserAppsSpike_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
