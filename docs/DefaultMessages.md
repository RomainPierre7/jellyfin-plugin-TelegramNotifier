# Default Messages

This document provides a list of default messages of the plugin. You can use these messages as a reference to create your own custom messages.

> **Note:** If you want to use dynamic values in your messages, you must respect the placeholders used in the default messages `{}`. The plugin will replace these placeholders with the actual values.
>> The available placeholders depends on the event type.

---

## **Extended Item Placeholders** (Item Added, Item Deleted)

| Placeholder | Description |
|-------------|-------------|
| `{item.OfficialRating}` | Content rating (PG-13, R, TV-MA) |
| `{item.Tagline}` | Item tagline |
| `{item.PremiereDate}` | Formatted release date |
| `{item.CumulativeRunTime}` | Total runtime (for series) |
| `{item.SeasonCount}` | Number of seasons (for series) |
| `{item.Tags}` | Comma-separated tags |
| `{item.ImdbId}` | IMDb ID |
| `{item.TmdbId}` | TMDB ID |
| `{item.ImdbUrl}` | Full IMDb URL |
| `{item.TmdbUrl}` | Full TMDB URL |

**Directors fallback:** `{item.Directors}` shows Directors → Producers → Writers → Creators → Actors (first non-empty).

**Conditional sections:** Use `{?placeholder}content{/placeholder}` to hide blocks when placeholder is empty. Example: `{?item.Directors}🎥 Director: {item.Directors}{/item.Directors}`

---

## **Library Additions**
- **Movies**
  ```
  🎬 {item.Name} ({item.ProductionYear})
        added to library

  📽 {item.Overview}
  ```
- **Series**
  ```
  📺 [Serie] {serie.Name} ({item.ProductionYear}) added to library

  📽 {item.Overview}
  ```
- **Seasons**
  ```
  📺 {season.Series.Name} ({item.ProductionYear})
        Season {seasonNumber} added to library

  📽 {item.Overview}
  ```
- **Episodes**
  ```
  📺 {episode.Series.Name} ({item.ProductionYear})
        S{eSeasonNumber} - E{episodeNumber}
        '{item.Name}' added to library

  📽 {item.Overview}
  ```
- **Albums**
  ```
  🎵 [Album] {album.Name} ({item.ProductionYear}) added to library
  ```
- **Songs**
  ```
  🎵 [Audio] {audio.Name} ({item.ProductionYear}) added to library
  ```
- **Books**
  ```
  📖 [Book] {item.Name} added to library

  🖋️ {item.Overview}";
  ```

---

## **Library Removals**
- **Movies**
  ```
  🗑️🎬 {item.Name} ({item.ProductionYear})
        removed from library

  📽 {item.Overview}
  ```
- **Series**
  ```
  🗑️📺 [Serie] {serie.Name} ({item.ProductionYear}) removed from library

  📽 {item.Overview}
  ```
- **Seasons**
  ```
  🗑️📺 {season.Series.Name} ({item.ProductionYear})
        Season {seasonNumber} removed from library

  📽 {item.Overview}
  ```
- **Episodes**
  ```
  🗑️📺 {episode.Series.Name} ({item.ProductionYear})
        S{eSeasonNumber} - E{episodeNumber}
        '{item.Name}' removed from library

  📽 {item.Overview}
  ```
- **Albums**
  ```
  🗑️🎵 [Album] {album.Name} ({item.ProductionYear}) removed from library
  ```
- **Songs**
  ```
  🗑️🎵 [Audio] {audio.Name} ({item.ProductionYear}) removed from library
  ```
- **Books**
  ```
  🗑️📖 [Book] {item.Name} removed from library

  🖋️ {item.Overview}";
  ```

---

## **Authentication**
- **Failure**
  ```
  🔒 Authentication failure on {eventArgs.Argument.DeviceName} for user {eventArgs.Argument.Username}
  ```
- **Success**
  ```
  🔓 Authentication success for user {eventArgs.Argument.User.Name} on {eventArgs.Argument.SessionInfo.DeviceName}
  ```

---

## **System Events**
- **Pending Restart**
  ```
  🔄 Jellyfin is pending a restart.
  ```

---

## **Playback Progress**
- **Movies**
  ```
  👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:
  🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})
  ```
- **Episodes**
  ```
  👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:
  🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})
        S{playbackSeasonNumber} - E{playbackEpisodeNumber}
        '{eventArgs.Item.Name}'
  ```

---

## **Playback Start**
- **Movies**
  ```
  👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName} ({eventArgs.Session.PlayState.PlayMethod}):
  🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})
  📺 [{eventArgs.Item.MediaType}] {eventArgs.Item.Genres}
  🕒 {duration}
  📽 {eventArgs.Item.Overview}
  ```
- **Episodes**
  ```
  👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName} ({eventArgs.Session.PlayState.PlayMethod}):
  🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})
        S{playbackSeasonNumber} - E{playbackEpisodeNumber}
        '{eventArgs.Item.Name}'
  📺 [{eventArgs.Item.MediaType}] {eventArgs.Item.Series.Genres}
  🕒 {duration}
  📽 {eventArgs.Item.Overview}
  ```

---

## **Playback Stop**
- **Movies**
  ```
  👤 {eventArgs.Users[0].Username} stopped watching:
  🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})
  ```
- **Episodes**
  ```
  👤 {eventArgs.Users[0].Username} stopped watching:
  🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})
        S{playbackSeasonNumber} - E{playbackEpisodeNumber}
        '{eventArgs.Item.Name}'
  ```

---

## **Plugin Management**
- **Installation Cancelled**
  ```
  🔴 {eventArgs.Argument.Name} plugin installation cancelled (version {eventArgs.Argument.Version}):
  ```
- **Installation Failed**
  ```
  🔴 {eventArgs.InstallationInfo} plugin installation failed (version {eventArgs.VersionInfo}):
  {eventArgs.Exception}
  ```
- **Installed**
  ```
  🚧 {eventArgs.Argument.Name} plugin installed (version {eventArgs.Argument.Version})

  You may need to restart your server.
  ```
- **Installing**
  ```
  🚧 {eventArgs.Argument.Name} plugin is installing (version {eventArgs.Argument.Version})
  ```
- **Uninstalled**
  ```
  🚧 {eventArgs.Argument.Name} plugin uninstalled
  ```
- **Updated**
  ```
  🚧 {eventArgs.Argument.Name} plugin updated to version {eventArgs.Argument.Version}:
  🗒️ {eventArgs.Argument.Changelog}

  You may need to restart Jellyfin to apply the changes.
  ```

---

## **Session Events**
- **Start**
  ```
  👤 {eventArgs.Argument.UserName} has started a session on:
  💻 {eventArgs.Argument.Client} ({eventArgs.Argument.DeviceName})
  ```

---

## **Subtitle Download**
- **Failure**
  ```
  🚫 Subtitle download failed for {eventArgs.Item.Name}
  ```

---

## **Tasks**
- **Completed**
  ```
  🧰 Task {eventArgs.Task.Name} completed: {eventArgs.Task.CurrentProgress}%
  🗒️ ({eventArgs.Task.Category}) {eventArgs.Task.Description}
  ```

---

## **User Management**
- **Created**
  ```
  👤 User {eventArgs.Argument.Username} created.
  ```
- **Deleted**
  ```
  🗑️ User {eventArgs.Argument.Username} deleted.
  ```
- **Locked Out**
  ```
  👤🔒 User {eventArgs.Argument.Username} locked out
  ```
- **Password Changed**
  ```
  👤 User {eventArgs.Argument.Username} changed his password.
  ```
- **Updated**
  ```
  👤 User {eventArgs.Argument.Username} has been updated
  ```
- **Data Saved**
  ```
  👤 User {eventArgs.Argument.Username} data saved.
  ```
