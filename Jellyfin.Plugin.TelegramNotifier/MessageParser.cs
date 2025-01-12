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
        private static readonly string[] AlbumNamePath = new[] { "Name" };
        private static readonly string[] AudioNamePath = new[] { "Name" };
        private static readonly string[] EventArgsArgumentDeviceNamePath = new[] { "Argument", "DeviceName" };
        private static readonly string[] EventArgsArgumentUsernamePath = new[] { "Argument", "Username" };
        private static readonly string[] EventArgsArgumentUserDotNamePath = new[] { "Argument", "User", "Name" };
        private static readonly string[] EventArgsArgumentSessionInfoDeviceNamePath = new[] { "Argument", "SessionInfo", "DeviceName" };
        private static readonly string[] EventArgsUsers0UsernamePath = new[] { "Users[0]", "Username" };
        private static readonly string[] EventArgsDeviceNamePath = new[] { "DeviceName" };
        private static readonly string[] EventArgsItemNamePath = new[] { "Item", "Name" };
        private static readonly string[] EventArgsItemSeriesNamePath = new[] { "Item", "Series", "Name" };
        private static readonly string[] EventArgsItemProductionYearPath = new[] { "Item", "ProductionYear" };
        private static readonly string[] EventArgsItemMediaTypePath = new[] { "Item", "MediaType" };
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
            { "{seasonNumber}", GetSeasonNumberSafely(objEventArgs) },
            { "{eSeasonNumber}", GetESeasonNumberSafely(objEventArgs) },
            { "{episodeNumber}", GetEpisodeNumberSafely(objEventArgs) },
            { "{playbackSeasonNumber}", GetPlaybackSeasonNumberSafely(objEventArgs) },
            { "{playbackEpisodeNumber}", GetPlaybackEpisodeNumberSafely(objEventArgs) },
            { "{album.Name}", GetPropertySafely(objEventArgs, AlbumNamePath) },
            { "{audio.Name}", GetPropertySafely(objEventArgs, AudioNamePath) },
            { "{eventArgs.Argument.DeviceName}", GetPropertySafely(objEventArgs, EventArgsArgumentDeviceNamePath) },
            { "{eventArgs.Argument.Username}", GetPropertySafely(objEventArgs, EventArgsArgumentUsernamePath) },
            { "{eventArgs.Argument.User.Name}", GetPropertySafely(objEventArgs, EventArgsArgumentUserDotNamePath) },
            { "{eventArgs.Argument.SessionInfo.DeviceName}", GetPropertySafely(objEventArgs, EventArgsArgumentSessionInfoDeviceNamePath) },
            { "{eventArgs.Users[0].Username}", GetPropertySafely(objEventArgs, EventArgsUsers0UsernamePath) },
            { "{eventArgs.Item.Series.Name}", GetPropertySafely(objEventArgs, EventArgsItemSeriesNamePath) },
            { "{eventArgs.DeviceName}", GetPropertySafely(objEventArgs, EventArgsDeviceNamePath) },
            { "{eventArgs.Item.Name}", GetPropertySafely(objEventArgs, EventArgsItemNamePath) },
            { "{eventArgs.Item.ProductionYear}", GetPropertySafely(objEventArgs, EventArgsItemProductionYearPath) },
            { "{eventArgs.Item.MediaType}", GetPropertySafely(objEventArgs, EventArgsItemMediaTypePath) },
            { "{eventArgs.Item.Genres}", GetGenresSafely(objEventArgs) },
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

        private static string GetGenresSafely(object obj)
        {
            var itemProperty = obj.GetType().GetProperty("Item");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return "No genres available";
            }

            var item = itemProperty.GetValue(obj);

            var genresProperty = item.GetType().GetProperty("Genres");

            if (genresProperty == null || genresProperty.GetValue(item) == null)
            {
                return "No genres available";
            }

            var genresValue = genresProperty.GetValue(item);

            if (genresValue is IEnumerable<object> genres)
            {
                return string.Join(", ", genres);
            }
            else
            {
                return "Invalid genres";
            }
        }

        private static string GetDurationSafely(object obj)
        {
            var itemProperty = obj.GetType().GetProperty("Item");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return "No duration available";
            }

            var item = itemProperty.GetValue(obj);

            var ticksProperty = item.GetType().GetProperty("RunTimeTicks");

            if (ticksProperty == null || ticksProperty.GetValue(item) == null)
            {
                return "No duration available";
            }

            var ticksValue = ticksProperty.GetValue(item);

            if (long.TryParse(ticksValue.ToString(), out long ticks))
            {
                long hours = ticks / (600000000L * 60);
                long minutes = (ticks / 600000000L) % 60;

                string duration;
                if (hours > 0)
                {
                    duration = minutes < 10 ? $"{hours}h 0{minutes}m" : $"{hours}h {minutes}m";
                }
                else
                {
                    duration = minutes > 1 ? $"{minutes} minutes" : $"{minutes} minute";
                }

                return duration;
            }
            else
            {
                return "Invalid duration";
            }
        }

        private static string GetSeasonNumberSafely(object obj)
        {
            // string seasonNumber = season.IndexNumber.HasValue ? season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
            var itemProperty = obj.GetType().GetProperty("IndexNumber");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return " Error ";
            }

            var item = itemProperty.GetValue(obj);

            if (int.TryParse(item.ToString(), out int seasonNumber))
            {
                return seasonNumber.ToString("00", CultureInfo.InvariantCulture);
            }
            else
            {
                return " Error ";
            }
        }

        private static string GetESeasonNumberSafely(object obj)
        {
            // string eSeasonNumber = episode.Season.IndexNumber.HasValue ? episode.Season.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
            var itemProperty = obj.GetType().GetProperty("Season");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return " Error ";
            }

            var item = itemProperty.GetValue(obj);

            var seasonNumberProperty = item.GetType().GetProperty("IndexNumber");

            if (seasonNumberProperty == null || seasonNumberProperty.GetValue(item) == null)
            {
                return " Error ";
            }

            var seasonNumberItem = seasonNumberProperty.GetValue(item);

            if (int.TryParse(seasonNumberItem.ToString(), out int seasonNumber))
            {
                return seasonNumber.ToString("00", CultureInfo.InvariantCulture);
            }
            else
            {
                return " Error ";
            }
        }

        private static string GetEpisodeNumberSafely(object obj)
        {
            // string episodeNumber = episode.IndexNumber.HasValue ? episode.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
            var itemProperty = obj.GetType().GetProperty("IndexNumber");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return " Error ";
            }

            var item = itemProperty.GetValue(obj);

            if (int.TryParse(item.ToString(), out int episodeNumber))
            {
                return episodeNumber.ToString("00", CultureInfo.InvariantCulture);
            }
            else
            {
                return " Error ";
            }
        }

        private static string GetPlaybackSeasonNumberSafely(object obj)
        {
            // string seasonNumber = eventArgs.Item.IndexNumber.HasValue ? eventArgs.Item.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
            var itemProperty = obj.GetType().GetProperty("Item");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return " Error ";
            }

            var item = itemProperty.GetValue(obj);

            var seasonNumberProperty = item.GetType().GetProperty("IndexNumber");

            if (seasonNumberProperty == null || seasonNumberProperty.GetValue(item) == null)
            {
                return " Error ";
            }

            var seasonNumberItem = seasonNumberProperty.GetValue(item);

            if (int.TryParse(seasonNumberItem.ToString(), out int seasonNumber))
            {
                return seasonNumber.ToString("00", CultureInfo.InvariantCulture);
            }
            else
            {
                return " Error ";
            }
        }

        private static string GetPlaybackEpisodeNumberSafely(object obj)
        {
            // string episodeNumber = eventArgs.Item.IndexNumber.HasValue ? eventArgs.Item.IndexNumber.Value.ToString("00", CultureInfo.InvariantCulture) : "00";
            var itemProperty = obj.GetType().GetProperty("Item");

            if (itemProperty == null || itemProperty.GetValue(obj) == null)
            {
                return " Error ";
            }

            var item = itemProperty.GetValue(obj);

            var episodeNumberProperty = item.GetType().GetProperty("IndexNumber");

            if (episodeNumberProperty == null || episodeNumberProperty.GetValue(item) == null)
            {
                return " Error ";
            }

            var episodeNumberItem = episodeNumberProperty.GetValue(item);

            if (int.TryParse(episodeNumberItem.ToString(), out int episodeNumber))
            {
                return episodeNumber.ToString("00", CultureInfo.InvariantCulture);
            }
            else
            {
                return " Error ";
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
                       $"Message:\n{message}\n\n{ex.Message} Check your configuration or metadata.";
            }
        }
    }
}