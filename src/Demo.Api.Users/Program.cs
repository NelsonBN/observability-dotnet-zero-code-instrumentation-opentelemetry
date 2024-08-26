using Api.Users.Infrastructure.Database;
using Api.Users.Infrastructure.Http;


var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services
    .AddDatabase();

builder.Services.AddHttp();

var app = builder.Build();

app.UseHttp();

await app.RunAsync();
