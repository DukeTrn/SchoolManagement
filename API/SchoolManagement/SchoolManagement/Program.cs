
var builder = WebApplication.CreateBuilder(args).SetupBuilder().Build();

var app = builder.SetupApp();

await app.RunAsync();

