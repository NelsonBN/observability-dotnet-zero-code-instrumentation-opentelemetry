using Api.Notifications.Domain;
using Api.Notifications.DTOs;
using MediatR;

namespace Api.Notifications.UseCases;

public sealed record SendNotificationCommand(NotificationRequest Request) : IRequest<Guid>
{
    internal sealed class Handler(INotificationsRepository repository, IUsersService service) : IRequestHandler<SendNotificationCommand, Guid>
    {
        private readonly INotificationsRepository _repository = repository;
        private readonly IUsersService _service = service;

        public async Task<Guid> Handle(SendNotificationCommand command, CancellationToken cancellationToken)
        {
            var user = await _service.GetUserAsync(command.Request.UserId, cancellationToken);
            if(user is null)
            {
                throw new UserNotFoundException(command.Request.UserId);
            }

            var notification = Notification.Create(
                command.Request.UserId,
                command.Request.Message,
                user.Email,
                user.Phone);

            await _repository.AddAsync(notification, cancellationToken);

            return notification.Id;
        }
    }
}
