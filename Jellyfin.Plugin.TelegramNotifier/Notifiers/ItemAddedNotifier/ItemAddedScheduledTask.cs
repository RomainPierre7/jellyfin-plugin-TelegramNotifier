using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediaBrowser.Model.Globalization;
using MediaBrowser.Model.Tasks;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers.ItemAddedNotifier;

public class ItemAddedScheduledTask : IScheduledTask, IConfigurableScheduledTask
{
    private const int RecheckIntervalSec = 60;
    private readonly IItemAddedManager _itemAddedManager;
    private readonly ILocalizationManager _localizationManager;

    public ItemAddedScheduledTask(
        IItemAddedManager itemAddedManager,
        ILocalizationManager localizationManager)
    {
        _itemAddedManager = itemAddedManager;
        _localizationManager = localizationManager;
    }

    public string Name => "TelegramNotifier Item Added Notifier";

    public string Key => "TelegramNotifier";

    public string Description => "Processes item added queue";

    public string Category => _localizationManager.GetLocalizedString("TasksLibraryCategory");

    public bool IsHidden => false;

    public bool IsEnabled => true;

    public bool IsLogged => false;

    public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        return _itemAddedManager.ProcessItemsAsync();
    }

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        return new[]
        {
            new TaskTriggerInfo
            {
                Type = TaskTriggerInfo.TriggerInterval,
                IntervalTicks = TimeSpan.FromSeconds(RecheckIntervalSec).Ticks
            }
        };
    }
}
