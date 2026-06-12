export default function (view) {

    const TelegramNotifierConfig = {
        pluginUniqueId: 'fd76211d-17e0-4a72-a23f-c6eeb1e48b3a',

        notificationType: {
            primaryTypes: ["ItemAdded", "ItemUpdated", "ItemDeleted"],
            showHiddenTypes: false,

            subtypeDisplayNames: {
                "Movies": "Movies / 电影",
                "Series": "Series / 剧集",
                "Seasons": "Seasons / 季",
                "Episodes": "Episodes / 单集",
                "Albums": "Albums / 专辑",
                "Songs": "Songs / 歌曲",
                "Books": "Books / 书籍"
            },

            values: {
                "ItemAdded": ["Item Added / 新增媒体", "Movies", "Series", "Seasons", "Episodes", "Albums", "Songs", "Books"],
                "ItemUpdated": ["Item Updated / 媒体刷新（更新）", "Movies", "Series", "Seasons", "Episodes", "Albums", "Songs", "Books"],
                "ItemDeleted": ["Item Deleted / 删除媒体", "Movies", "Series", "Seasons", "Episodes", "Albums", "Songs", "Books"],
                "PlaybackStart": ["Playback Start / 开始播放", "Movies", "Episodes"],
                "PlaybackProgress": ["Playback Progress / 播放进度（建议关闭）", "Movies", "Episodes"],
                "PlaybackStop": ["Playback Stop / 停止播放", "Movies", "Episodes"],
                "SubtitleDownloadFailure": "Subtitle Download Failure / 字幕下载失败",
                "AuthenticationFailure": "Authentication Failure / 登录失败",
                "AuthenticationSuccess": "Authentication Success / 登录成功",
                "SessionStart": "Session Start / 会话开始",
                "PendingRestart": "Pending Restart / 等待重启",
                "TaskCompleted": "Task Completed / 任务完成",
                "PluginInstallationCancelled": "Plugin Installation Cancelled / 插件安装已取消",
                "PluginInstallationFailed": "Plugin Installation Failed / 插件安装失败",
                "PluginInstalled": "Plugin Installed / 插件已安装",
                "PluginInstalling": "Plugin Installing / 插件安装中",
                "PluginUninstalled": "Plugin Uninstalled / 插件已卸载",
                "PluginUpdated": "Plugin Updated / 插件已更新",
                "UserCreated": "User Created / 用户已创建",
                "UserDeleted": "User Deleted / 用户已删除",
                "UserLockedOut": "User Locked Out / 用户已锁定",
                "UserPasswordChanged": "User Password Changed / 用户密码已更改",
                "UserUpdated": "User Updated / 用户已更新",
                "UserDataSaved": "User Data Saved / 用户数据已保存"
            },

            defaultMessages: {
                "ItemAddedMovies": "🎬 {item.Name} ({item.ProductionYear})\n      added to library\n\n📽 {item.Overview}",
                "ItemAddedSeries": "📺 [Serie] {serie.Name} ({item.ProductionYear}) added to library\n\n📽 {item.Overview}",
                "ItemAddedSeasons": "📺 {season.Series.Name} ({item.ProductionYear})\n      Season {seasonNumber} added to library\n\n📽 {item.Overview}",
                "ItemAddedEpisodes": "📺 {episode.Series.Name} ({item.ProductionYear})\n      S{eSeasonNumber} - E{episodeNumber}\n      '{item.Name}' added to library\n\n📽 {item.Overview}",
                "ItemAddedAlbums": "🎵 [Album] {album.Name} ({item.ProductionYear}) added to library",
                "ItemAddedSongs": "🎵 [Audio] {audio.Name} ({item.ProductionYear}) added to library",
                "ItemAddedBooks": "📖 [Book] {item.name} added to library\n\n🖋️ {item.Overview}",
                "ItemUpdatedMovies": "🎬 {item.Name} ({item.ProductionYear}) 已刷（更）新\n\n📽 {item.Overview}",
                "ItemUpdatedSeries": "📺 [Serie] {serie.Name} ({item.ProductionYear}) 已刷（更）新\n\n📽 {item.Overview}",
                "ItemUpdatedSeasons": "📺 {season.Series.Name} ({item.ProductionYear})\n      Season {seasonNumber} 已刷（更）新\n\n📽 {item.Overview}",
                "ItemUpdatedEpisodes": "📺 {episode.Series.Name} ({item.ProductionYear})\n      S{eSeasonNumber} - E{episodeNumber}\n      '{item.Name}' 已刷（更）新\n\n📽 {item.Overview}",
                "ItemUpdatedAlbums": "🎵 [Album] {album.Name} ({item.ProductionYear}) 已刷（更）新",
                "ItemUpdatedSongs": "🎵 [Audio] {audio.Name} ({item.ProductionYear}) 已刷（更）新",
                "ItemUpdatedBooks": "📖 [Book] {item.Name} 已刷（更）新\n\n🖋️ {item.Overview}",
                "ItemDeletedMovies": "🗑️🎬 {item.Name} ({item.ProductionYear})\n      removed from library\n\n📽 {item.Overview}",
                "ItemDeletedSeries": "🗑️📺 [Serie] {serie.Name} ({item.ProductionYear}) removed from library\n\n📽 {item.Overview}",
                "ItemDeletedSeasons": "🗑️📺 {season.Series.Name} ({item.ProductionYear})\n      Season {seasonNumber} removed from library\n\n📽 {item.Overview}",
                "ItemDeletedEpisodes": "🗑️📺 {episode.Series.Name} ({item.ProductionYear})\n      S{eSeasonNumber} - E{episodeNumber}\n      '{item.Name}' removed from library\n\n📽 {item.Overview}",
                "ItemDeletedAlbums": "🗑️🎵 [Album] {album.Name} ({item.ProductionYear}) removed from library",
                "ItemDeletedSongs": "🗑️🎵 [Audio] {audio.Name} ({item.ProductionYear}) removed from library",
                "ItemDeletedBooks": "🗑️📖 [Book] {item.name} removed from library\n\n🖋️ {item.Overview}",
                "AuthenticationFailure": "🔒 Authentication failure on {eventArgs.Argument.DeviceName} for user {eventArgs.Argument.Username}",
                "AuthenticationSuccess": "🔓 Authentication success for user {eventArgs.Argument.User.Name} on {eventArgs.Argument.SessionInfo.DeviceName}",
                "PendingRestart": "🔄 Jellyfin is pending a restart.",
                "PlaybackProgressMovies": "👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})",
                "PlaybackProgressEpisodes": "👤 {eventArgs.Users[0].Username} is still watching on {eventArgs.DeviceName}:\n🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})\n      S{playbackSeasonNumber} - E{playbackEpisodeNumber}\n      '{eventArgs.Item.Name}'",
                "PlaybackStartMovies": "👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName} ({eventArgs.Session.PlayState.PlayMethod}):\n🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})\n📺 [{eventArgs.Item.MediaType}] {eventArgs.Item.Genres}\n🕒 {duration}\n📽 {eventArgs.Item.Overview}",
                "PlaybackStartEpisodes": "👤 {eventArgs.Users[0].Username} is watching on {eventArgs.DeviceName} ({eventArgs.Session.PlayState.PlayMethod}):\n🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})\n      S{playbackSeasonNumber} - E{playbackEpisodeNumber}\n      '{eventArgs.Item.Name}'\n📺 [{eventArgs.Item.MediaType}] {eventArgs.Item.Series.Genres}\n🕒 {duration}\n📽 {eventArgs.Item.Overview}",
                "PlaybackStopMovies": "👤 {eventArgs.Users[0].Username} stopped watching:\n🎬 {eventArgs.Item.Name} ({eventArgs.Item.ProductionYear})",
                "PlaybackStopEpisodes": "👤 {eventArgs.Users[0].Username} stopped watching:\n🎬 {eventArgs.Item.Series.Name} ({eventArgs.Item.ProductionYear})\n      S{playbackSeasonNumber} - E{playbackEpisodeNumber}\n      '{eventArgs.Item.Name}'",
                "PluginInstallationCancelled": "🔴 {eventArgs.Argument.Name} plugin installation cancelled (version {eventArgs.Argument.Version}):",
                "PluginInstallationFailed": "🔴 {eventArgs.InstallationInfo} plugin installation failed (version {eventArgs.VersionInfo}):\n{eventArgs.Exception}",
                "PluginInstalled": "🚧 {eventArgs.Argument.Name} plugin installed (version {eventArgs.Argument.Version})\n\nYou may need to restart your server.",
                "PluginInstalling": "🚧 {eventArgs.Argument.Name} plugin is installing (version {eventArgs.Argument.Version})",
                "PluginUninstalled": "🚧 {eventArgs.Argument.Name} plugin uninstalled",
                "PluginUpdated": "🚧 {eventArgs.Argument.Name} plugin updated to version {eventArgs.Argument.Version}:🗒️ {eventArgs.Argument.Changelog}\n\nYou may need to restart Jellyfin to apply the changes.",
                "SessionStart": "👤 {eventArgs.Argument.UserName} has started a session on:\n💻 {eventArgs.Argument.Client} ({eventArgs.Argument.DeviceName})\n",
                "SubtitleDownloadFailure": "🚫 Subtitle download failed for {eventArgs.Item.Name}",
                "TaskCompleted": "🧰 Task {eventArgs.Task.Name} completed: {eventArgs.Task.CurrentProgress}%\n🗒️ ({eventArgs.Task.Category}) {eventArgs.Task.Description}",
                "UserCreated": "👤 User {eventArgs.Argument.Username} created.",
                "UserDeleted": "🗑️ User {eventArgs.Argument.Username} deleted.",
                "UserLockedOut": "👤🔒 User {eventArgs.Argument.Username} locked out",
                "UserPasswordChanged": "👤 User {eventArgs.Argument.Username} changed his password.",
                "UserUpdated": "👤 User {eventArgs.Argument.Username} has been updated",
                "UserDataSaved": "👤 User {eventArgs.Argument.Username} data saved."
            },

            loadNotificationTypes: function (userConfig) {
                const temp = document.querySelector("#template-notification-type");
                const temp_without_textarea = document.querySelector("#template-notification-type-without-textarea");
                const subtemp = document.querySelector("#template-notification-subtype");
                const container = document.querySelector("[data-name=notificationTypeContainer]");
                container.innerHTML = '';
                this.showHiddenTypes = false;

                const notificationTypeKeys = Object.keys(TelegramNotifierConfig.notificationType.values).sort();
                for (const key of notificationTypeKeys) {
                    const typeWrapper = document.createElement('div');
                    typeWrapper.dataset.name = 'notificationTypeGroup';
                    typeWrapper.dataset.value = key;
                    const isPrimaryType = this.primaryTypes.includes(key);
                    const typeEnabled = userConfig !== null && userConfig[key] === true;
                    if (!isPrimaryType && !typeEnabled) {
                        typeWrapper.dataset.hiddenType = 'true';
                    }

                    let template = temp.cloneNode(true).content;
                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        template = temp_without_textarea.cloneNode(true).content;
                    }
                    const name = template.querySelector("[data-name=notificationTypeName]");
                    const value = template.querySelector("[data-name=notificationTypeValue]");

                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        name.innerText = TelegramNotifierConfig.notificationType.values[key][0];
                    } else {
                        name.innerText = TelegramNotifierConfig.notificationType.values[key];
                        const textarea = template.querySelector('[data-name="txtTemplate"]');
                        textarea.value = userConfig === null ? this.defaultMessages[key] : userConfig[key + 'StringMessage'];
                        textarea.dataset.value = key;
                    }
                    value.dataset.value = key;
                    if (userConfig === null) {
                        value.checked = false;
                    } else {
                        value.checked = userConfig[key] === true;
                    }
                    typeWrapper.appendChild(template);

                    // Notification subtypes
                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        for (const subtype of TelegramNotifierConfig.notificationType.values[key].slice(1)) {
                            const subtypeWrapper = document.createElement('div');
                            template = subtemp.cloneNode(true).content;
                            const name = template.querySelector("[data-name=notificationSubtypeName]");
                            const value = template.querySelector("[data-name=notificationSubtypeValue]");
                            const textarea = template.querySelector('[data-name="txtTemplate"]');

                            name.innerText = this.subtypeDisplayNames[subtype] || subtype;
                            const subkey = key + subtype.replace(/\s/g, '');
                            value.dataset.value = subkey;
                            textarea.dataset.value = subkey;
                            if (userConfig === null) {
                                value.checked = false;
                                textarea.value = this.defaultMessages[subkey];
                            } else {
                                value.checked = userConfig[subkey] === true;
                                textarea.value = userConfig[subkey + 'StringMessage'];
                            }
                            subtypeWrapper.dataset.name = 'notificationSubtypeGroup';
                            subtypeWrapper.dataset.value = subkey;
                            if (!value.checked) {
                                subtypeWrapper.dataset.hiddenType = 'true';
                            }

                            subtypeWrapper.appendChild(template);
                            typeWrapper.appendChild(subtypeWrapper);
                        }
                    }

                    container.appendChild(typeWrapper);
                }

                this.updateHiddenTypeVisibility();
            },

            updateHiddenTypeVisibility: function () {
                const hiddenRows = document.querySelectorAll('[data-name=notificationTypeContainer] [data-hidden-type="true"]');
                for (const row of hiddenRows) {
                    const checkbox = row.querySelector('input[type="checkbox"]');
                    const isChecked = checkbox !== null && checkbox.checked === true;
                    row.style.display = this.showHiddenTypes || isChecked ? '' : 'none';
                }

                const button = document.getElementById('toggleHiddenNotificationTypes');
                if (button !== null) {
                    const text = button.querySelector('span');
                    if (text !== null) {
                        text.innerText = this.showHiddenTypes ?
                            'Hide hidden/disabled notification types / 隐藏未启用的通知类型' :
                            'Show hidden/disabled notification types / 显示隐藏或未启用的通知类型';
                    }
                }
            },

            saveNotificationTypes: function (userConfig) {
                const notificationTypeKeys = Object.keys(TelegramNotifierConfig.notificationType.values).sort();
                for (const key of notificationTypeKeys) {
                    userConfig[key] = document.querySelector(`[data-name=notificationTypeValue][data-value=${key}]`).checked;
                    if (typeof TelegramNotifierConfig.notificationType.values[key] === 'string') {
                        userConfig[key + 'StringMessage'] = document.querySelector(`[data-name=txtTemplate][data-value=${key}]`).value;
                    }

                    // Notification subtypes
                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        for (const subtype of TelegramNotifierConfig.notificationType.values[key].slice(1)) {
                            const subkey = key + subtype.replace(/\s/g, '');
                            userConfig[subkey] = document.querySelector(`[data-name=notificationSubtypeValue][data-value=${subkey}]`).checked;
                            userConfig[subkey + 'StringMessage'] = document.querySelector(`[data-name=txtTemplate][data-value=${subkey}]`).value;
                        }
                    }
                }
            }
        },

        user: {
            loadUsers: async function () {
                const users = await window.ApiClient.getUsers();
                const selectElement = document.getElementById("userToConfigure");
                selectElement.innerHTML = '';
                for (const user of users) {
                    const option = document.createElement('option');
                    option.value = user.Id;
                    option.textContent = user.Name;
                    selectElement.appendChild(option);
                }
            },
            getSelectedUserId: function () {
                const userId = document.getElementById("userToConfigure").value;
                return userId;
            }
        },

        init: async function () {
            await this.user.loadUsers();
            this.loadConfig();

            document.getElementById('userToConfigure').addEventListener('change', this.loadConfig);
            document.getElementById('testButton').addEventListener('click', this.testBotConfig);
            document.getElementById('saveButton').addEventListener('click', this.saveConfig);
            document.getElementById('toggleHiddenNotificationTypes').addEventListener('click', function (event) {
                event.preventDefault();
                TelegramNotifierConfig.notificationType.showHiddenTypes = !TelegramNotifierConfig.notificationType.showHiddenTypes;
                TelegramNotifierConfig.notificationType.updateHiddenTypeVisibility();
            });

            document.body.addEventListener('click', function (event) {
                const button = event.target.closest('.edit-template-button');

                if (button) {
                    event.preventDefault();
                    const container = button.closest('div');
                    const resetButton = container.querySelector('.reset-template-button');
                    const textarea = container.querySelector('textarea[data-name="txtTemplate"]');

                    if (resetButton && textarea) {
                        resetButton.style.display = resetButton.style.display === 'none' ? 'block' : 'none';
                        textarea.style.display = textarea.style.display === 'none' ? 'block' : 'none';
                    }
                }
            });

            document.body.addEventListener('click', function (event) {
                const button = event.target.closest('.reset-template-button');

                if (button) {
                    event.preventDefault();
                    const container = button.closest('div');
                    const textarea = container.querySelector('textarea[data-name="txtTemplate"]');
                    const key = textarea.dataset.value;

                    if (textarea) {
                        textarea.value = TelegramNotifierConfig.notificationType.defaultMessages[key];
                    }
                }
            });

        },

        loadConfig: function () {
            Dashboard.showLoadingMsg();
            ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId).then(function (config) {
                document.querySelector('#EnablePlugin').checked = config.EnablePlugin;
                const userConfig = config.UserConfigurations.find(x => x.UserId === TelegramNotifierConfig.user.getSelectedUserId());
                if (userConfig) {
                    document.querySelector('#ServerUrl').value = config.ServerUrl;
                    document.querySelector('#BotToken').value = userConfig.BotToken;
                    document.querySelector('#ChatId').value = userConfig.ChatId;
                    document.querySelector('#ThreadId').value = userConfig.ThreadId;
                    document.querySelector('#EnableUser').checked = userConfig.EnableUser;
                    document.querySelector('#SilentNotification').checked = userConfig.SilentNotification;
                    document.querySelector('#DoNotMentionOwnActivities').checked = userConfig.DoNotMentionOwnActivities;
                    document.querySelector('#KeepSerieImage').checked = userConfig.KeepSerieImage;
                    TelegramNotifierConfig.notificationType.loadNotificationTypes(userConfig);
                } else {
                    document.querySelector('#ServerUrl').value = config.ServerUrl;
                    document.querySelector('#BotToken').value = '';
                    document.querySelector('#ChatId').value = '';
                    document.querySelector('#ThreadId').value = '';
                    document.querySelector('#EnableUser').checked = false;
                    document.querySelector('#SilentNotification').checked = false;
                    document.querySelector('#DoNotMentionOwnActivities').checked = false;
                    document.querySelector('#KeepSerieImage').checked = false;
                    TelegramNotifierConfig.notificationType.loadNotificationTypes(null);
                }
                Dashboard.hideLoadingMsg();
            });
        },

        saveConfig: function (e = null) {
            if (e) {
                e.preventDefault();
            }
            return new Promise((resolve, reject) => {
                Dashboard.showLoadingMsg();
                ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId).then(function (config) {
                    config.EnablePlugin = document.querySelector('#EnablePlugin').checked;
                    const userConfig = config.UserConfigurations.find(x => x.UserId === TelegramNotifierConfig.user.getSelectedUserId());
                    if (userConfig) {
                        config.ServerUrl = document.querySelector('#ServerUrl').value;
                        userConfig.BotToken = document.querySelector('#BotToken').value;
                        userConfig.ChatId = document.querySelector('#ChatId').value;
                        userConfig.ThreadId = document.querySelector('#ThreadId').value;
                        userConfig.EnableUser = document.querySelector('#EnableUser').checked;
                        userConfig.SilentNotification = document.querySelector('#SilentNotification').checked;
                        userConfig.DoNotMentionOwnActivities = document.querySelector('#DoNotMentionOwnActivities').checked;
                        userConfig.KeepSerieImage = document.querySelector('#KeepSerieImage').checked;
                        TelegramNotifierConfig.notificationType.saveNotificationTypes(userConfig);
                    } else {
                        config.ServerUrl = document.querySelector('#ServerUrl').value;
                        config.UserConfigurations.push({
                            UserId: TelegramNotifierConfig.user.getSelectedUserId(),
                            UserName: document.querySelector('#userToConfigure').selectedOptions[0].text,
                            BotToken: document.querySelector('#BotToken').value,
                            ChatId: document.querySelector('#ChatId').value,
                            ThreadId: document.querySelector('#ThreadId').value,
                            EnableUser: document.querySelector('#EnableUser').checked,
                            SilentNotification: document.querySelector('#SilentNotification').checked,
                            DoNotMentionOwnActivities: document.querySelector('#DoNotMentionOwnActivities').checked,
                            KeepSerieImage: document.querySelector('#KeepSerieImage').checked
                        });
                        TelegramNotifierConfig.notificationType.saveNotificationTypes(config.UserConfigurations.find(x => x.UserId === TelegramNotifierConfig.user.getSelectedUserId()));
                    }
                    ApiClient.updatePluginConfiguration(TelegramNotifierConfig.pluginUniqueId, config).then(function (result) {
                        Dashboard.processPluginConfigurationUpdateResult(result);
                        resolve(result);
                    }).catch(reject);
                }).catch(reject);
            });
        },

        testBotConfig: function () {
            var button = this;
            button.disabled = true;
            TelegramNotifierConfig.saveConfig()
                .then(function () {
                    return ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId);
                })
                .then(function (config) {
                    const userConfig = config.UserConfigurations.find(x => x.UserId === TelegramNotifierConfig.user.getSelectedUserId());
                    var threadId = userConfig.ThreadId;
                    if (threadId === '') {
                        threadId = null;
                    }
                    const params = {
                        botToken: userConfig.BotToken,
                        chatId: userConfig.ChatId,
                        threadId: threadId,
                    };
                    const url = new URL('/TelegramNotifierApi/TestNotifier', window.location.origin);
                    Object.keys(params).forEach(key => url.searchParams.append(key, params[key]));
                    return fetch(url);
                })
                .then(function (response) {
                    Dashboard.hideLoadingMsg();
                    if (!response.ok) {
                        throw new Error('Error while sending the test message to the telegram bot');
                    }
                    button.style.backgroundColor = 'green';
                    button.style.color = 'white';
                    button.textContent = 'Test passed / 测试通过';
                })
                .catch(function () {
                    button.style.backgroundColor = 'red';
                    button.style.color = 'white';
                    button.textContent = 'Test failed / 测试失败';
                })
                .finally(function () {
                    setTimeout(function () {
                        button.disabled = false;
                        button.style.color = '';
                        button.style.backgroundColor = '';
                        button.textContent = 'Test bot configuration / 测试机器人配置';
                    }, 2000);
                });
        }
    };

    view.addEventListener('viewshow', async function () {
        await TelegramNotifierConfig.init();
    });
}
