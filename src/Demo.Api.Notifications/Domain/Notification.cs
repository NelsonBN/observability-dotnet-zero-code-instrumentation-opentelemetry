﻿namespace Api.Notifications.Domain;

public sealed class Notification
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Message { get; private set; } = default!;

    public string? Email { get; private set; }
    public NotificationStatus EmailNotificationStatus { get; private set; }

    public string? Phone { get; private set; }
    public NotificationStatus PhoneNotificationStatus { get; private set; }

    public int Version { get; private set; }

    private Notification() { }


    public void SMSSent()
    {
        PhoneNotificationStatus = NotificationStatus.Sent;
        Version++;
    }

    public void SMSFailed()
    {
        PhoneNotificationStatus = NotificationStatus.Failed;
        Version++;
    }

    public void EmailSent()
    {
        EmailNotificationStatus = NotificationStatus.Sent;
        Version++;
    }

    public void EmailFailed()
    {
        EmailNotificationStatus = NotificationStatus.Failed;
        Version++;
    }

    public static Notification Create(Guid userId, string message, string? email, string? phone)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message, nameof(message));


        if(string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException("Email or phone must be provided");
        }

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Version = 1,
            Message = message
        };


        if(string.IsNullOrWhiteSpace(email))
        {
            notification.EmailNotificationStatus = NotificationStatus.NoSent;
        }
        else
        {
            notification.Email = email;
            notification.EmailNotificationStatus = NotificationStatus.Pending;
        }


        if(string.IsNullOrWhiteSpace(phone))
        {
            notification.PhoneNotificationStatus = NotificationStatus.NoSent;
        }
        else
        {
            notification.Phone = phone;
            notification.PhoneNotificationStatus = NotificationStatus.Pending;
        }

        return notification;
    }
}
