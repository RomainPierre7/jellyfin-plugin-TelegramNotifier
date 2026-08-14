using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public sealed class ItemNotificationStore : IDisposable
{
    private static readonly TimeSpan RecordLifetime = TimeSpan.FromDays(7);
    private readonly ILogger<ItemNotificationStore> _logger;
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly string _storePath;
    private List<ItemNotificationRecord> _records;

    public ItemNotificationStore(ILogger<ItemNotificationStore> logger)
    {
        _logger = logger;
        _storePath = Path.Combine(Plugin.Instance!.DataFolderPath, "item-notifications.json");
        _records = Load();
    }

    public async Task<IReadOnlyList<ItemNotificationRecord>> GetAsync(Guid itemId)
    {
        await _lock.WaitAsync().ConfigureAwait(false);
        try
        {
            return _records.Where(record => record.ItemId == itemId).ToArray();
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AddAsync(IEnumerable<ItemNotificationRecord> records)
    {
        await _lock.WaitAsync().ConfigureAwait(false);
        try
        {
            foreach (ItemNotificationRecord record in records)
            {
                _records.RemoveAll(existing =>
                    existing.ItemId == record.ItemId &&
                    existing.ChatId == record.ChatId &&
                    existing.ThreadId == record.ThreadId &&
                    existing.MessageId == record.MessageId);
                _records.Add(record);
            }

            RemoveExpiredRecords();
            await SaveAsync().ConfigureAwait(false);
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateAsync(ItemNotificationRecord record)
    {
        await _lock.WaitAsync().ConfigureAwait(false);
        try
        {
            ItemNotificationRecord? existing = _records.FirstOrDefault(candidate =>
                candidate.ItemId == record.ItemId &&
                candidate.ChatId == record.ChatId &&
                candidate.ThreadId == record.ThreadId &&
                candidate.MessageId == record.MessageId);

            if (existing is not null)
            {
                existing.RenderedMessage = record.RenderedMessage;
                await SaveAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task PurgeExpiredAsync()
    {
        await _lock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (RemoveExpiredRecords())
            {
                await SaveAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public void Dispose()
    {
        _lock.Dispose();
    }

    private List<ItemNotificationRecord> Load()
    {
        try
        {
            if (!File.Exists(_storePath))
            {
                return new List<ItemNotificationRecord>();
            }

            string json = File.ReadAllText(_storePath);
            return JsonSerializer.Deserialize<List<ItemNotificationRecord>>(json) ?? new List<ItemNotificationRecord>();
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "Could not load tracked Telegram item notifications");
            return new List<ItemNotificationRecord>();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Could not parse tracked Telegram item notifications");
            return new List<ItemNotificationRecord>();
        }
    }

    private bool RemoveExpiredRecords()
    {
        DateTime cutoff = DateTime.UtcNow.Subtract(RecordLifetime);
        return _records.RemoveAll(record => record.SentAtUtc < cutoff) > 0;
    }

    private async Task SaveAsync()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_storePath)!);
        string json = JsonSerializer.Serialize(_records);
        await File.WriteAllTextAsync(_storePath, json).ConfigureAwait(false);
    }
}
