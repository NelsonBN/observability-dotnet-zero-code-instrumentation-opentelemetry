using Api.Notifications.Domain;
using Api.Notifications.DTOs;
using Api.Notifications.UseCases;
using MediatR;

namespace Api.Notifications.Infrastructure.Http;

public static class NotificationsEndpoints
{
    public static void MapNotificationsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/notifications")
            .WithOpenApi();


        group.MapGet("", async (IMediator mediator, ILoggerFactory loggerFactory) =>
        {
            try
            {
                var response = await mediator.Send(GetNotificationsQuery.Instance);

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
                var response = await mediator.Send(new GetNotificationQuery(id));
                return Results.Ok(response);
            }
            catch(NotificationNotFoundException exception)
            {
                return Results.NotFound(exception.Message);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][GET][BY ID]");
                throw;
            }
        }).WithName("GetNotification");



        group.MapPost("", async (IMediator mediator, ILoggerFactory loggerFactory, NotificationRequest request) =>
        {
            try
            {
                var id = await mediator.Send(new SendNotificationCommand(request));

                return Results.AcceptedAtRoute(
                    "GetNotification",
                    new { id },
                    id);
            }
            catch(UserNotFoundException exception)
            {
                return Results.NotFound(exception.Message);
            }
            catch(Exception exception)
            {
                loggerFactory
                    .CreateLogger("NotificationsEndpoints")
                    .LogError(exception, "[WEB API][POST]");
                throw;
            }
        });
    }
}
