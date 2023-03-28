using Budget.API;

var builder = WebApplication.CreateBuilder(args);

var app = builder
    .ConfigureServices()
    .ConfigureApp();

await app.SeedData();

app.Run();