var builder = DistributedApplication.CreateBuilder(args);

var staticFilesBrowserApp = builder.AddBrowserApp("staticfiles", "../staticfilesapp");

var apiService = builder.AddProject<Projects.BrowserAppsSpike_ApiService>("apiservice");

var nextJs = builder.AddNextJsApp("nextjs", "../nextjsapp")
                    .WithFowardingTo("/api*/*", apiService.GetEndpoint("http"));

var viteapp = builder.AddViteApp("viteapp", "../viteapp")
                     .WithFowardingTo("/api*/*", apiService.GetEndpoint("http"));

builder.Build().Run();
