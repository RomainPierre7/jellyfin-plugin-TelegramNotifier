using System;
using System.Threading.Tasks;
using MediaBrowser.Controller.Events;
using MediaBrowser.Model.Tasks;

namespace Jellyfin.Plugin.TelegramNotifier.Notifiers;

public class TaskCompletedNotifier : IEventConsumer<TaskCompletionEventArgs>
{
    private readonly NotificationFilter _notificationFilter;

    public TaskCompletedNotifier(NotificationFilter notificationFilter)
    {
        _notificationFilter = notificationFilter;
    }

    public async Task OnEvent(TaskCompletionEventArgs eventArgs)
    {
        if (eventArgs == null)
        {
            throw new ArgumentNullException(nameof(eventArgs));
        }

        string message = $"🧰 Task {eventArgs.Task.Name} completed: {eventArgs.Task.CurrentProgress}%\n" +
                         $"🗒️ ({eventArgs.Task.Category}) {eventArgs.Task.Description}";

        await _notificationFilter.Filter(NotificationFilter.NotificationType.TaskCompleted, message).ConfigureAwait(false);
    }
}