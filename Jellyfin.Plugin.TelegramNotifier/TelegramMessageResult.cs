namespace Jellyfin.Plugin.TelegramNotifier;

public sealed class TelegramMessageResult
{
    public TelegramMessageResult(long messageId)
    {
        MessageId = messageId;
    }

    public long MessageId { get; }
}
