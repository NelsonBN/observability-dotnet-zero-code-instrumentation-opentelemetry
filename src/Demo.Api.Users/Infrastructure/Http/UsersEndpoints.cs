using Api.Users.Domain;
using Api.Users.DTOs;
using Api.Users.UseCases;
using MediatR;

namespace Api.Users.Infrastructure.Http;

public static class UsersEndpoints
{
    public static void MapUsersEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/users")
            .WithOpenApi();


        group.MapGet("", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            try
            {
                var response = await mediator.Send(GetUsersQuery.Instance);

                return Results.Ok(response);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][GET]");

                throw;
            }
        });


        group.MapGet("{id:guid}", async (IMediator mediator, ILoggerFactory loggerFactory, Guid id) =>
        {
            try
            {
                var response = await mediator.Send(new GetUserQuery(id));
                return Results.Ok(response);
            }
            catch(UserNotFoundException exception)
            {
                return Results.NotFound(exception.Message);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][GET][BY USER ID]");

                throw;
            }
        }).WithName("GetUser");


        group.MapPost("", async (IMediator mediator, ILoggerFactory loggerFactory, UserRequest request) =>
        {
            try
            {
                var id = await mediator.Send(new CreateUserCommand(request));

                return Results.CreatedAtRoute(
                    "GetUser",
                    new { id },
                    id);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][POST]");
                throw;
            }
        });


        group.MapPut("{id:guid}", async (IMediator mediator, ILoggerFactory loggerFactory, Guid id, UserRequest request) =>
        {
            try
            {
                await mediator.Send(new UpdateUserCommand(id, request));
            }
            catch(UserNotFoundException exception)
            {
                return Results.NotFound(exception.Message);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][PUT]");
                throw;
            }

            return Results.NoContent();
        });


        group.MapDelete("{id:guid}", async (IMediator mediator, ILoggerFactory loggerFactory, Guid id) =>
        {
            try
            {
                await mediator.Send(new DeleteUserCommand(id));
            }
            catch(UserNotFoundException exception)
            {
                return Results.NotFound(exception.Message);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][DELETE]");
                throw;
            }

            return Results.NoContent();
        });
    }
}
