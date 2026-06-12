using System;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class QueuedItemContainer
{
    public QueuedItemContainer(Guid id, NotificationFilter.NotificationType notificationType)
    {
        ItemId = id;
        NotificationType = notificationType;
        RetryCount = 0;
    }

    public int RetryCount { get; set; }

    public Guid ItemId { get; set; }

    public NotificationFilter.NotificationType NotificationType { get; set; }
}
