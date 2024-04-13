# Jellyfin Telegram Notifier

This project is a [Jellyfin](https://github.com/jellyfin) plugin that sends notifications to a Telegram bot when something happens on the server (like a new media added, a media deleted, etc.).

> **Note:** ⚠️ This project is still in development and is not functional yet. 👷

## Table of contents

- [Jellyfin Telegram Notifier](#jellyfin-telegram-notifier)
  - [Table of contents](#table-of-contents)
  - [Installation](#installation)
    - [Install the plugin](#install-the-plugin)
    - [Install the project (for developers)](#install-the-project-for-developers)

## Install the plugin

To install the plugin on your Jellyfin server, you need to follow these steps:

> **Note:** 👷 This section is still in progress

## Use the plugin

To use the plugin, you need to follow these steps:

> **Note:** 👷 This section is still in progress

## Install the project (for developers)

To install the project, you need to follow these steps:

> **Note:** You need to have the [.NET SDK](https://dotnet.microsoft.com/download) installed on your machine.

1. Clone the repository
2. Install the dependencies
3. Compile the plugin

### 1. Clone the repository

```bash
git clone https://github.com/RomainPierre7/jellyfin_telegram_notifier.git
```

### 2. Install the dependencies

```bash
dotnet add package Jellyfin.Model
dotnet add package Jellyfin.Controller
dotnet add package Telegram.Bot
```

### 3. Compile the plugin

```bash
dotnet build
```