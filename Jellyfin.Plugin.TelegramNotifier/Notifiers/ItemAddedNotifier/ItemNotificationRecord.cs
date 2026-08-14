using System;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public sealed class ItemNotificationRecord
{
    public Guid ItemId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public string ChatId { get; set; } = string.Empty;

    public string ThreadId { get; set; } = string.Empty;

    public long MessageId { get; set; }

    public bool HasPhoto { get; set; }

    public string RenderedMessage { get; set; } = string.Empty;

    public DateTime SentAtUtc { get; set; }
}
