using System;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class QueuedItemContainer
{
    public QueuedItemContainer(Guid id)
    {
        ItemId = id;
        RetryCount = 0;
    }

    public int RetryCount { get; set; }

    public Guid ItemId { get; set; }
}
