using Api.Notifications.Infrastructure.Database;
using Api.Notifications.Infrastructure.Http;
using Api.Notifications.Infrastructure.UsersApi;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services
    .AddDatabase()
    .AddUsersApi();

builder.Services.AddHttp();

var app = builder.Build();


app.UseHttp();

await app.RunAsync();
