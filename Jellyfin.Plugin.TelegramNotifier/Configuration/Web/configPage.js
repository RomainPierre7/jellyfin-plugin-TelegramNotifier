export default function (view) {

    const TelegramNotifierConfig = {
        pluginUniqueId: 'fd76211d-17e0-4a72-a23f-c6eeb1e48b3a',

        notificationType: {
            values: {
                "ItemAdded": ["Item Added", "Movies", "Series", "Seasons", "Episodes", "Albums", "Songs"],
                "PlaybackStart": "Playback Start",
                "PlaybackProgress": "Playback Progress (recommended: disabled)",
                "PlaybackStop": "Playback Stop",
                "SubtitleDownloadFailure": "Subtitle Download Failure",
                "AuthenticationFailure": "Authentication Failure",
                "AuthenticationSuccess": "Authentication Success",
                "SessionStart": "Session Start",
                "PendingRestart": "Pending Restart",
                "TaskCompleted": "Task Completed",
                "PluginInstallationCancelled": "Plugin Installation Cancelled",
                "PluginInstallationFailed": "Plugin Installation Failed",
                "PluginInstalled": "Plugin Installed",
                "PluginInstalling": "Plugin Installing",
                "PluginUninstalled": "Plugin Uninstalled",
                "PluginUpdated": "Plugin Updated",
                "UserCreated": "User Created",
                "UserDeleted": "User Deleted",
                "UserLockedOut": "User Locked Out",
                "UserPasswordChanged": "User Password Changed",
                "UserUpdated": "User Updated",
                "UserDataSaved": "User Data Saved"
            },

            loadNotificationTypes: function (userConfig) {
                const temp = document.querySelector("#template-notification-type");
                const subtemp = document.querySelector("#template-notification-subtype");
                const container = document.querySelector("[data-name=notificationTypeContainer]");
                container.innerHTML = '';

                const notificationTypeKeys = Object.keys(TelegramNotifierConfig.notificationType.values).sort();
                for (const key of notificationTypeKeys) {
                    const template = temp.cloneNode(true).content;
                    const name = template.querySelector("[data-name=notificationTypeName]");
                    const value = template.querySelector("[data-name=notificationTypeValue]");

                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        name.innerText = TelegramNotifierConfig.notificationType.values[key][0];
                    } else {
                        name.innerText = TelegramNotifierConfig.notificationType.values[key];
                    }
                    value.dataset.value = key;
                    if (userConfig === null) {
                        value.checked = false;
                    } else {
                        value.checked = userConfig[key] === true;
                    }
                    container.appendChild(template);

                    // Notification subtypes
                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        for (const subtype of TelegramNotifierConfig.notificationType.values[key].slice(1)) {
                            const template = subtemp.cloneNode(true).content;
                            const name = template.querySelector("[data-name=notificationSubtypeName]");
                            const value = template.querySelector("[data-name=notificationSubtypeValue]");

                            name.innerText = subtype;
                            const subkey = key + subtype.replace(/\s/g, '');
                            value.dataset.value = subkey;
                            if (userConfig === null) {
                                value.checked = false;
                            } else {
                                value.checked = userConfig[subkey] === true;
                            }
                            container.appendChild(template);
                        }
                    }
                }
            },

            saveNotificationTypes: function (userConfig) {
                const notificationTypeKeys = Object.keys(TelegramNotifierConfig.notificationType.values).sort();
                for (const key of notificationTypeKeys) {
                    userConfig[key] = document.querySelector(`[data-name=notificationTypeValue][data-value=${key}]`).checked;

                    // Notification subtypes
                    if (typeof TelegramNotifierConfig.notificationType.values[key] !== 'string') {
                        for (const subtype of TelegramNotifierConfig.notificationType.values[key].slice(1)) {
                            const subkey = key + subtype.replace(/\s/g, '');
                            userConfig[subkey] = document.querySelector(`[data-name=notificationSubtypeValue][data-value=${subkey}]`).checked;
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

            document.getElementById("userToConfigure").addEventListener('change', this.loadConfig);
            document.getElementById('testButton').addEventListener('click', this.testBotConfig);
            document.querySelector('#TelegramNotifierConfigForm').addEventListener('submit', this.saveConfig);
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
                    TelegramNotifierConfig.notificationType.loadNotificationTypes(userConfig);
                } else {
                    document.querySelector('#ServerUrl').value = config.ServerUrl;
                    document.querySelector('#BotToken').value = '';
                    document.querySelector('#ChatId').value = '';
                    document.querySelector('#ThreadId').value = '';
                    document.querySelector('#EnableUser').checked = false;
                    document.querySelector('#SilentNotification').checked = false;
                    document.querySelector('#DoNotMentionOwnActivities').checked = false;
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
                            DoNotMentionOwnActivities: document.querySelector('#DoNotMentionOwnActivities').checked
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
                    button.textContent = 'Test passed';
                })
                .catch(function () {
                    button.style.backgroundColor = 'red';
                    button.style.color = 'white';
                    button.textContent = 'Test failed';
                })
                .finally(function () {
                    setTimeout(function () {
                        button.disabled = false;
                        button.style.color = '';
                        button.style.backgroundColor = '';
                        button.textContent = 'Test bot configuration';
                    }, 2000);
                });
        }
    };

    view.addEventListener('viewshow', async function () {
        await TelegramNotifierConfig.init();
    });
}