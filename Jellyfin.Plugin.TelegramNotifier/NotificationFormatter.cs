using System;
using System.Collections.Generic;
using Jellyfin.Plugin.TelegramNotifier.Configuration;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public class NotificationFormatter
    {
        private readonly Dictionary<NotificationFilter.NotificationType, Func<UserConfiguration, string, string>> _prefixFormatters;
        private readonly Dictionary<NotificationFilter.NotificationType, Func<UserConfiguration, string>> _suffixFormatters;

        public NotificationFormatter()
        {
            _prefixFormatters = new Dictionary<NotificationFilter.NotificationType, Func<UserConfiguration, string, string>>
            {
                [NotificationFilter.NotificationType.ItemAdded] = (user, subtype) => subtype switch
                {
                    "ItemAddedMovie" => user.MessageItemAddedPrefix ?? $"🎬",
                    "ItemAddedSerie" => user.MessageItemAddedPrefix ?? $"📺 [Serie]",
                    "ItemAddedSeason" => user.MessageItemAddedPrefix ?? $"📺",
                    "ItemAddedEpisode" => user.MessageItemAddedPrefix ?? $"📺",
                    _ => user.MessageItemAddedPrefix ?? string.Empty
                },
                // Add more types and their prefixes as needed
            };

            _suffixFormatters = new Dictionary<NotificationFilter.NotificationType, Func<UserConfiguration, string>>
            {
                [NotificationFilter.NotificationType.ItemAdded] = user => user.MessageItemAddedSuffix ?? string.Empty,
                // Add more types and their suffixes as needed
            };
        }

        public string GetPrefix(NotificationFilter.NotificationType type, UserConfiguration user, string subtype)
        {
            if (_prefixFormatters.TryGetValue(type, out var formatter))
            {
                return formatter(user, subtype);
            }

            return user.MessageItemAddedPrefix ?? string.Empty;
        }

        public string GetSuffix(NotificationFilter.NotificationType type, UserConfiguration user)
        {
            if (_suffixFormatters.TryGetValue(type, out var formatter))
            {
                return formatter(user);
            }

            return user.MessageItemAddedSuffix ?? string.Empty;
        }

        public string FormatMessage(NotificationFilter.NotificationType type, string message, UserConfiguration user, string subtype, string overview)
        {
            string prefix = GetPrefix(type, user, subtype);
            string suffix = GetSuffix(type, user);
            string overviewText = user.EnableItemAddedOverview == true && !string.IsNullOrEmpty(overview) ? $"\n\n{overview}" : string.Empty;
            return $"{prefix} {message} {suffix} {overviewText}".Trim();
        }
    }
}