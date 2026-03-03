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
        private static readonly string[] ItemOverviewPath = new[] { "Overview" };
        private static readonly string[] ItemCommunityRatingPath = new[] { "CommunityRating" };
        private static readonly string[] ItemStudiosPath = new[] { "Studios" };
        private static readonly string[] ItemProductionLocationsPath = new[] { "ProductionLocations" };
        private static readonly string[] ItemIdPath = new[] { "Id" };
        private static readonly string[] SerieNamePath = new[] { "Name" };
        private static readonly string[] SeasonSeriesNamePath = new[] { "Series", "Name" };
        private static readonly string[] EpisodeSeriesNamePath = new[] { "Series", "Name" };
        private static readonly string[] AlbumNamePath = new[] { "Name" };
        private static readonly string[] AudioNamePath = new[] { "Name" };
        private static readonly string[] EventArgsSessionPlayStatePlayMethodPath = new[] { "Session", "PlayState", "PlayMethod" };
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

        private static readonly Regex OverviewTruncationRegex = new(
            @"\{item\.Overview:(\d+)\}",
            RegexOptions.Compiled);

        private static object GetEffectiveItem(object obj)
        {
            if (obj == null) return null;
            var itemProperty = obj.GetType().GetProperty("Item");
            if (itemProperty != null && itemProperty.GetValue(obj) != null)
                return itemProperty.GetValue(obj);
            return obj;
        }

        private static Dictionary<string, string?> GetReplacements(dynamic eventArgs)
        {
            try
            {
                object objEventArgs = eventArgs;
                object effectiveItem = GetEffectiveItem(objEventArgs);

                string serverUrl = "http://localhost:8096";
                if (Plugin.Instance?.Configuration != null && !string.IsNullOrEmpty(Plugin.Instance.Configuration.ServerUrl))
                {
                    var url = Plugin.Instance.Configuration.ServerUrl.Trim();
                    serverUrl = url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? url : "http://" + url;
                }

                var replacements = new Dictionary<string, string?>
                {
                    { "{item.Name}", GetPropertySafely(objEventArgs, ItemNamePath) },
                    { "{item.ProductionYear}", GetPropertySafely(objEventArgs, ItemProductionYearPath) },
                    { "{item.Overview}", GetPropertySafely(objEventArgs, ItemOverviewPath) },
                    { "{item.CommunityRating}", GetItemCommunityRatingSafely(effectiveItem) },
                    { "{item.RunTime}", GetItemRunTimeSafely(effectiveItem) },
                    { "{item.Genres}", GetItemGenresSafely(effectiveItem) },
                    { "{item.Directors}", GetItemDirectorsSafely(effectiveItem) },
                    { "{item.Studios}", GetItemStudiosSafely(effectiveItem) },
                    { "{item.ProductionLocations}", GetItemProductionLocationsSafely(effectiveItem) },
                    { "{item.Id}", GetPropertySafely(objEventArgs, ItemIdPath) ?? GetPropertySafely(effectiveItem, new[] { "Id" }) },
                    { "{item.LibraryName}", GetItemLibraryNameSafely(effectiveItem) },
                    { "{item.VideoResolution}", GetItemVideoResolutionSafely(effectiveItem) },
                    { "{item.VideoCodec}", GetItemVideoCodecSafely(effectiveItem) },
                    { "{item.AudioCodec}", GetItemAudioCodecSafely(effectiveItem) },
                    { "{server.Url}", serverUrl },
                    { "{serie.Name}", GetPropertySafely(objEventArgs, SerieNamePath) },
                    { "{season.Series.Name}", GetPropertySafely(objEventArgs, SeasonSeriesNamePath) },
                    { "{episode.Series.Name}", GetPropertySafely(objEventArgs, EpisodeSeriesNamePath) },
                    { "{seasonNumber}", GetSeasonNumberSafely(objEventArgs) },
                    { "{eSeasonNumber}", GetESeasonNumberSafely(objEventArgs) },
                    { "{episodeNumber}", GetEpisodeNumberSafely(objEventArgs) },
                    { "{playbackSeasonNumber}", GetPlaybackSeasonNumberSafely(objEventArgs) },
                    { "{playbackEpisodeNumber}", GetPlaybackEpisodeNumberSafely(objEventArgs) },
                    { "{album.Name}", GetPropertySafely(objEventArgs, AlbumNamePath) },
                    { "{audio.Name}", GetPropertySafely(objEventArgs, AudioNamePath) },
                    { "{eventArgs.Session.PlayState.PlayMethod}", GetPropertySafely(objEventArgs, EventArgsSessionPlayStatePlayMethodPath) },
                    { "{eventArgs.Argument.DeviceName}", GetPropertySafely(objEventArgs, EventArgsArgumentDeviceNamePath) },
                    { "{eventArgs.Argument.Username}", GetPropertySafely(objEventArgs, EventArgsArgumentUsernamePath) },
                    { "{eventArgs.Argument.User.Name}", GetPropertySafely(objEventArgs, EventArgsArgumentUserDotNamePath) },
                    { "{eventArgs.Argument.SessionInfo.DeviceName}", GetPropertySafely(objEventArgs, EventArgsArgumentSessionInfoDeviceNamePath) },
                    { "{eventArgs.Users[0].Username}", GetPropertySafely(objEventArgs, EventArgsUsers0UsernamePath) },
                    { "{eventArgs.Item.Series.Name}", GetPropertySafely(objEventArgs, EventArgsItemSeriesNamePath) },
                    { "{eventArgs.Item.Series.Genres}", GetSerieGenresSafely(objEventArgs) },
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

                return replacements;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while building replacements: " + ex.Message);
            }
        }

        private static string GetItemCommunityRatingSafely(object item)
        {
            if (item == null) return string.Empty;
            var val = GetPropertySafely(item, ItemCommunityRatingPath);
            if (string.IsNullOrEmpty(val)) return string.Empty;
            return float.TryParse(val, CultureInfo.InvariantCulture, out float rating) ? rating.ToString("F1", CultureInfo.InvariantCulture) : val;
        }

        private static string GetItemRunTimeSafely(object item)
        {
            if (item == null) return string.Empty;
            var ticksProperty = item.GetType().GetProperty("RunTimeTicks");
            if (ticksProperty?.GetValue(item) == null) return string.Empty;
            var ticksValue = ticksProperty.GetValue(item);
            if (!long.TryParse(ticksValue?.ToString(), out long ticks) || ticks == 0) return string.Empty;
            long hours = ticks / (600000000L * 60);
            long minutes = (ticks / 600000000L) % 60;
            return hours > 0
                ? (minutes < 10 ? $"{hours}h 0{minutes}m" : $"{hours}h {minutes}m")
                : (minutes > 1 ? $"{minutes} min" : $"{minutes} min");
        }

        private static string GetItemGenresSafely(object item)
        {
            if (item == null) return string.Empty;
            var genresValue = item.GetType().GetProperty("Genres")?.GetValue(item);
            if (genresValue is IEnumerable<object> genres)
                return string.Join(", ", genres.Select(g => g?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            if (genresValue is System.Collections.IEnumerable enumGenres)
                return string.Join(", ", enumGenres.Cast<object>().Select(g => g?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            return string.Empty;
        }

        private static string GetItemDirectorsSafely(object item)
        {
            if (item == null) return string.Empty;
            try
            {
                var peopleProp = item.GetType().GetProperty("People");
                if (peopleProp?.GetValue(item) is System.Collections.IEnumerable people)
                {
                    var directors = new List<string>();
                    foreach (var p in people)
                    {
                        if (p == null) continue;
                        var typeProp = p.GetType().GetProperty("Type");
                        var nameProp = p.GetType().GetProperty("Name");
                        if (typeProp?.GetValue(p)?.ToString()?.Equals("Director", StringComparison.OrdinalIgnoreCase) == true
                            && nameProp?.GetValue(p) is string name && !string.IsNullOrEmpty(name))
                            directors.Add(name);
                    }
                    return string.Join(", ", directors);
                }
            }
            catch { }
            return string.Empty;
        }

        private static string GetItemStudiosSafely(object item)
        {
            if (item == null) return string.Empty;
            var studiosValue = item.GetType().GetProperty("Studios")?.GetValue(item);
            if (studiosValue is IEnumerable<object> studios)
                return string.Join(", ", studios.Select(s => s?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            if (studiosValue is System.Collections.IEnumerable enumStudios)
                return string.Join(", ", enumStudios.Cast<object>().Select(s => s?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            return string.Empty;
        }

        private static string GetItemProductionLocationsSafely(object item)
        {
            if (item == null) return string.Empty;
            var locsValue = item.GetType().GetProperty("ProductionLocations")?.GetValue(item);
            if (locsValue is IEnumerable<object> locs)
                return string.Join(", ", locs.Select(l => l?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            if (locsValue is System.Collections.IEnumerable enumLocs)
                return string.Join(", ", enumLocs.Cast<object>().Select(l => l?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            return string.Empty;
        }

        private static string GetItemLibraryNameSafely(object item)
        {
            if (item == null) return string.Empty;
            try
            {
                var parentProp = item.GetType().GetProperty("Parent");
                var parent = parentProp?.GetValue(item);
                while (parent != null)
                {
                    var nameProp = parent.GetType().GetProperty("Name");
                    var name = nameProp?.GetValue(parent)?.ToString();
                    if (!string.IsNullOrEmpty(name)) return name;
                    parent = parent.GetType().GetProperty("Parent")?.GetValue(parent);
                }
            }
            catch { }
            return string.Empty;
        }

        private static string GetItemVideoResolutionSafely(object item)
        {
            if (item == null) return string.Empty;
            try
            {
                var widthProp = item.GetType().GetProperty("Width");
                var heightProp = item.GetType().GetProperty("Height");
                var w = widthProp?.GetValue(item);
                var h = heightProp?.GetValue(item);
                if (w != null && h != null && int.TryParse(w.ToString(), out int width) && int.TryParse(h.ToString(), out int height) && width > 0 && height > 0)
                    return $"{height}p";
            }
            catch { }
            return string.Empty;
        }

        private static string GetItemVideoCodecSafely(object item)
        {
            if (item == null) return string.Empty;
            try
            {
                var videoType = item.GetType();
                if (videoType.GetMethod("GetDefaultVideoStream")?.Invoke(item, null) is { } stream)
                {
                    var codecProp = stream.GetType().GetProperty("Codec");
                    return codecProp?.GetValue(stream)?.ToString() ?? string.Empty;
                }
            }
            catch { }
            return string.Empty;
        }

        private static string GetItemAudioCodecSafely(object item)
        {
            return string.Empty;
        }

        private static string? GetPropertySafely(object obj, string[] propertyPath)
        {
            try
            {
                foreach (var property in propertyPath)
                {
                    if (obj == null) return null;
                    var match = Regex.Match(property, @"(.*)\[(\d+)\]");
                    if (match.Success)
                    {
                        var propertyName = match.Groups[1].Value;
                        var index = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                        var propertyInfo = obj.GetType().GetProperty(propertyName);
                        if (propertyInfo == null) return null;
                        obj = propertyInfo.GetValue(obj);
                        if (obj is System.Collections.IEnumerable collection)
                        {
                            var list = collection.Cast<object>().ToList();
                            obj = list.Count > index ? list[index] : null;
                        }
                        else return null;
                    }
                    else
                    {
                        var propertyInfo = obj.GetType().GetProperty(property);
                        if (propertyInfo == null) return null;
                        obj = propertyInfo.GetValue(obj);
                    }
                }
                return obj?.ToString();
            }
            catch { return null; }
        }

        private static string GetGenresSafely(object obj)
        {
            var item = GetEffectiveItem(obj);
            if (item == null) return "No genres available";
            var result = GetItemGenresSafely(item);
            return string.IsNullOrEmpty(result) ? "No genres available" : result;
        }

        private static string GetSerieGenresSafely(object obj)
        {
            var itemProperty = obj?.GetType().GetProperty("Item");
            if (itemProperty == null || itemProperty.GetValue(obj) == null) return "No genres available";
            var item = itemProperty.GetValue(obj);
            var seriesProperty = item?.GetType().GetProperty("Series");
            if (seriesProperty == null || seriesProperty.GetValue(item) == null) return "No genres available";
            var series = seriesProperty.GetValue(item);
            var genresProperty = series?.GetType().GetProperty("Genres");
            if (genresProperty == null || genresProperty.GetValue(series) == null) return "No genres available";
            var genresValue = genresProperty.GetValue(series);
            if (genresValue is IEnumerable<object> genres)
                return string.Join(", ", genres.Select(g => g?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            if (genresValue is System.Collections.IEnumerable enumGenres)
                return string.Join(", ", enumGenres.Cast<object>().Select(g => g?.ToString() ?? string.Empty).Where(s => !string.IsNullOrEmpty(s)));
            return "No genres available";
        }

        private static string GetDurationSafely(object obj)
        {
            var item = GetEffectiveItem(obj);
            if (item == null) return "No duration available";
            var runTime = GetItemRunTimeSafely(item);
            return string.IsNullOrEmpty(runTime) ? "No duration available" : runTime;
        }

        private static string? GetSeasonNumberSafely(object obj)
        {
            var itemProperty = obj?.GetType().GetProperty("IndexNumber");
            if (itemProperty == null || itemProperty.GetValue(obj) == null) return null;
            var item = itemProperty.GetValue(obj);
            return int.TryParse(item?.ToString(), out int n) ? n.ToString("00", CultureInfo.InvariantCulture) : null;
        }

        private static string? GetESeasonNumberSafely(object obj)
        {
            var seasonProperty = obj?.GetType().GetProperty("Season");
            if (seasonProperty == null || seasonProperty.GetValue(obj) == null) return null;
            var season = seasonProperty.GetValue(obj);
            var idxProp = season?.GetType().GetProperty("IndexNumber");
            if (idxProp == null || idxProp.GetValue(season) == null) return null;
            var val = idxProp.GetValue(season);
            return int.TryParse(val?.ToString(), out int n) ? n.ToString("00", CultureInfo.InvariantCulture) : null;
        }

        private static string? GetEpisodeNumberSafely(object obj)
        {
            var itemProperty = obj?.GetType().GetProperty("IndexNumber");
            if (itemProperty == null || itemProperty.GetValue(obj) == null) return null;
            var item = itemProperty.GetValue(obj);
            return int.TryParse(item?.ToString(), out int n) ? n.ToString("00", CultureInfo.InvariantCulture) : null;
        }

        private static string? GetPlaybackSeasonNumberSafely(object obj)
        {
            var itemProperty = obj?.GetType().GetProperty("Item");
            if (itemProperty == null || itemProperty.GetValue(obj) == null) return null;
            var item = itemProperty.GetValue(obj);
            var seasonProperty = item?.GetType().GetProperty("Season");
            if (seasonProperty == null || seasonProperty.GetValue(item) == null) return null;
            var season = seasonProperty.GetValue(item);
            var idxProp = season?.GetType().GetProperty("IndexNumber");
            if (idxProp == null || idxProp.GetValue(season) == null) return null;
            var val = idxProp.GetValue(season);
            return int.TryParse(val?.ToString(), out int n) ? n.ToString("00", CultureInfo.InvariantCulture) : null;
        }

        private static string? GetPlaybackEpisodeNumberSafely(object obj)
        {
            var itemProperty = obj?.GetType().GetProperty("Item");
            if (itemProperty == null || itemProperty.GetValue(obj) == null) return null;
            var item = itemProperty.GetValue(obj);
            var idxProp = item?.GetType().GetProperty("IndexNumber");
            if (idxProp == null || idxProp.GetValue(item) == null) return null;
            var val = idxProp.GetValue(item);
            return int.TryParse(val?.ToString(), out int n) ? n.ToString("00", CultureInfo.InvariantCulture) : null;
        }

        public static string ParseMessage(string message, dynamic eventArgs)
        {
            var replacements = GetReplacements(eventArgs);

            message = OverviewTruncationRegex.Replace(message, m =>
            {
                if (!int.TryParse(m.Groups[1].Value, out int maxLen) || maxLen <= 0) return m.Value;
                var overview = GetPropertySafely(GetEffectiveItem(eventArgs), ItemOverviewPath) ?? string.Empty;
                if (string.IsNullOrEmpty(overview)) return string.Empty;
                if (overview.Length <= maxLen) return overview;
                return overview.Substring(0, maxLen).TrimEnd() + "...";
            });

            foreach (var pair in replacements)
            {
                if (message.Contains(pair.Key))
                {
                    var value = string.IsNullOrEmpty(pair.Value) ? "..." : pair.Value;
                    message = Regex.Replace(message, Regex.Escape(pair.Key), value);
                }
            }

            return message;
        }
    }
}
