using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Jellyfin.Plugin.TelegramNotifier
{
    public static class MessageParser
    {
        private static readonly string[] ItemNamePath = new[] { "Name" };
        private static readonly string[] ItemProductionYearPath = new[] { "ProductionYear" };
        private static readonly string[] SerieNamePath = new[] { "Name" };
        private static readonly string[] SeasonSeriesNamePath = new[] { "Series", "Name" };
        private static readonly string[] SeasonNumberPath = new[] { "seasonNumber" };
        private static readonly string[] EpisodeSeriesNamePath = new[] { "Series", "Name" };
        private static readonly string[] ESeasonNumberPath = new[] { "eSeasonNumber" };
        private static readonly string[] EpisodeNumberPath = new[] { "episodeNumber" };
        private static readonly string[] AlbumNamePath = new[] { "Name" };
        private static readonly string[] AudioNamePath = new[] { "Name" };
        private static readonly string[] EventArgsArgumentDeviceNamePath = new[] { "Argument", "DeviceName" };
        private static readonly string[] EventArgsArgumentUsernamePath = new[] { "Argument", "Username" };
        private static readonly string[] EventArgsArgumentUserDotNamePath = new[] { "Argument", "User", "Name" };
        private static readonly string[] EventArgsArgumentSessionInfoDeviceNamePath = new[] { "Argument", "SessionInfo", "DeviceName" };
        private static readonly string[] EventArgsUsers0UsernamePath = new[] { "Users[0]", "Username" };
        private static readonly string[] EventArgsDeviceNamePath = new[] { "DeviceName" };
        private static readonly string[] EventArgsItemNamePath = new[] { "Item", "Name" };
        private static readonly string[] EventArgsItemProductionYearPath = new[] { "Item", "ProductionYear" };
        private static readonly string[] EventArgsItemMediaTypePath = new[] { "Item", "MediaType" };
        private static readonly string[] EventArgsItemGenresPath = new[] { "Item", "Genres" };
        private static readonly string[] EventArgsItemOverviewPath = new[] { "Item", "Overview" };
        private static readonly string[] EventArgsInstallationInfoPath = new[] { "InstallationInfo" };
        private static readonly string[] EventArgsVersionInfoPath = new[] { "VersionInfo" };
        private static readonly string[] EventArgsExceptionPath = new[] { "Exception" };
        private static readonly string[] EventArgsArgumentNamePath = new[] { "Argument", "Name" };
        private static readonly string[] EventArgsArgumentVersionPath = new[] { "Argument", "Version" };
        private static readonly string[] EventArgsArgumentChangelogPath = new[] { "Argument", "Changelog" };
        private static readonly string[] EventArgsArgumentUserNamePath = new[] { "Argument", "UserName" };
        private static readonly string[] EventArgsArgumentClientPath = new[] { "Argument", "Client" };
        private static readonly string[] EventArgsTaskNamePath = new[] { "Task", "Name" };
        private static readonly string[] EventArgsTaskCurrentProgressPath = new[] { "Task", "CurrentProgress" };
        private static readonly string[] EventArgsTaskCategoryPath = new[] { "Task", "Category" };
        private static readonly string[] EventArgsTaskDescriptionPath = new[] { "Task", "Description" };

        private static Dictionary<string, string?> GetReplacements(dynamic eventArgs)
        {
            try
            {
                object objEventArgs = eventArgs;
                return new Dictionary<string, string?>
        {
            { "{item.Name}", GetPropertySafely(objEventArgs, ItemNamePath) },
            { "{item.ProductionYear}", GetPropertySafely(objEventArgs, ItemProductionYearPath) },
            { "{serie.Name}", GetPropertySafely(objEventArgs, SerieNamePath) },
            { "{season.Series.Name}", GetPropertySafely(objEventArgs, SeasonSeriesNamePath) },
            { "{seasonNumber}", GetPropertySafely(objEventArgs, SeasonNumberPath) },
            { "{episode.Series.Name}", GetPropertySafely(objEventArgs, EpisodeSeriesNamePath) },
            { "{eSeasonNumber}", GetPropertySafely(objEventArgs, ESeasonNumberPath) },
            { "{episodeNumber}", GetPropertySafely(objEventArgs, EpisodeNumberPath) },
            { "{album.Name}", GetPropertySafely(objEventArgs, AlbumNamePath) },
            { "{audio.Name}", GetPropertySafely(objEventArgs, AudioNamePath) },
            { "{eventArgs.Argument.DeviceName}", GetPropertySafely(objEventArgs, EventArgsArgumentDeviceNamePath) },
            { "{eventArgs.Argument.Username}", GetPropertySafely(objEventArgs, EventArgsArgumentUsernamePath) },
            { "{eventArgs.Argument.User.Name}", GetPropertySafely(objEventArgs, EventArgsArgumentUserDotNamePath) },
            { "{eventArgs.Argument.SessionInfo.DeviceName}", GetPropertySafely(objEventArgs, EventArgsArgumentSessionInfoDeviceNamePath) },
            { "{eventArgs.Users[0].Username}", GetPropertySafely(objEventArgs, EventArgsUsers0UsernamePath) },
            { "{eventArgs.DeviceName}", GetPropertySafely(objEventArgs, EventArgsDeviceNamePath) },
            { "{eventArgs.Item.Name}", GetPropertySafely(objEventArgs, EventArgsItemNamePath) },
            { "{eventArgs.Item.ProductionYear}", GetPropertySafely(objEventArgs, EventArgsItemProductionYearPath) },
            { "{eventArgs.Item.MediaType}", GetPropertySafely(objEventArgs, EventArgsItemMediaTypePath) },
            { "{string.Join(\", \", eventArgs.Item.Genres)}", GetPropertySafely(objEventArgs, EventArgsItemGenresPath) },
            { "{duration}", GetDurationSafely(objEventArgs) },
            { "{eventArgs.Item.Overview}", GetPropertySafely(objEventArgs, EventArgsItemOverviewPath) },
            { "{eventArgs.InstallationInfo}", GetPropertySafely(objEventArgs, EventArgsInstallationInfoPath) },
            { "{eventArgs.VersionInfo}", GetPropertySafely(objEventArgs, EventArgsVersionInfoPath) },
            { "{eventArgs.Exception}", GetPropertySafely(objEventArgs, EventArgsExceptionPath) },
            { "{eventArgs.Argument.Name}", GetPropertySafely(objEventArgs, EventArgsArgumentNamePath) },
            { "{eventArgs.Argument.Version}", GetPropertySafely(objEventArgs, EventArgsArgumentVersionPath) },
            { "{eventArgs.Argument.Changelog}", GetPropertySafely(objEventArgs, EventArgsArgumentChangelogPath) },
            { "{eventArgs.Argument.UserName}", GetPropertySafely(objEventArgs, EventArgsArgumentUserNamePath) },
            { "{eventArgs.Argument.Client}", GetPropertySafely(objEventArgs, EventArgsArgumentClientPath) },
            { "{eventArgs.Task.Name}", GetPropertySafely(objEventArgs, EventArgsTaskNamePath) },
            { "{eventArgs.Task.CurrentProgress}", GetPropertySafely(objEventArgs, EventArgsTaskCurrentProgressPath) },
            { "{eventArgs.Task.Category}", GetPropertySafely(objEventArgs, EventArgsTaskCategoryPath) },
            { "{eventArgs.Task.Description}", GetPropertySafely(objEventArgs, EventArgsTaskDescriptionPath) }
        };
            }
            catch (Exception ex)
            {
                throw new Exception("Error while building replacements: " + ex.Message);
            }
        }

        private static string? GetPropertySafely(object obj, string[] propertyPath)
        {
            try
            {
                foreach (var property in propertyPath)
                {
                    if (obj == null)
                    {
                        return null;
                    }

                    var match = System.Text.RegularExpressions.Regex.Match(property, @"(.*)\[(\d+)\]");
                    if (match.Success)
                    {
                        var propertyName = match.Groups[1].Value;
                        var index = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                        var propertyInfo = obj.GetType().GetProperty(propertyName);
                        if (propertyInfo == null)
                        {
                            return null;
                        }

                        obj = propertyInfo.GetValue(obj);
                        if (obj is IEnumerable<object> collection && collection.Count() > index)
                        {
                            obj = collection.ElementAt(index);
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        var propertyInfo = obj.GetType().GetProperty(property);
                        if (propertyInfo == null)
                        {
                            return null;
                        }

                        obj = propertyInfo.GetValue(obj);
                    }
                }

                return obj?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static string GetDurationSafely(object obj)
        {
            var itemProperty = obj.GetType().GetProperty("Item");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return "Durée non disponible";
            }

            var item = itemProperty.GetValue(obj);

            var ticksProperty = item.GetType().GetProperty("RunTimeTicks");

            if (ticksProperty == null || ticksProperty.GetValue(item) == null)
            {
                return "Durée non disponible";
            }

            var ticksValue = ticksProperty.GetValue(item);

            if (long.TryParse(ticksValue.ToString(), out long ticks))
            {
                long hours = ticks / (600000000L * 60);
                long minutes = (ticks / 600000000L) % 60;

                string duration;
                if (hours > 0)
                {
                    duration = minutes < 10 ? $"{hours}h {minutes}m" : $"{hours}h 0{minutes}m";
                }
                else
                {
                    duration = minutes > 1 ? $"{minutes} minutes" : $"{minutes} minute";
                }

                return duration;
            }
            else
            {
                return "Durée invalide";
            }
        }

        public static string ParseMessage(string message, dynamic eventArgs)
        {
            try
            {
                var replacements = GetReplacements(eventArgs);

                foreach (var pair in replacements)
                {
                    if (message.Contains(pair.Key))
                    {
                        if (pair.Value == null)
                        {
                            throw new Exception($"The value for the key '{pair.Key}' is null, but the key exists in the message.");
                        }

                        message = Regex.Replace(message, Regex.Escape(pair.Key), pair.Value, RegexOptions.IgnoreCase);
                    }
                }

                return message;
            }
            catch (Exception ex)
            {
                return $"Error: Wrong message configuration for event {eventArgs?.GetType()?.Name ?? "Unknown"}.\n" +
                       "One or more keys are invalid or do not exist.\n\n" +
                       $"Message:\n{message}\n\n{ex.Message}";
            }
        }
    }
}