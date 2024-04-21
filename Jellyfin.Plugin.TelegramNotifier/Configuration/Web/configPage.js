export default function (view) {

    const TelegramNotifierConfig = {
        pluginUniqueId: 'fd76211d-17e0-4a72-a23f-c6eeb1e48b3a',

        notificationType: {
            "Test": "Test",
            "ItemAdded": "Item Added",
            "Generic": "Generic",
            "PlaybackStart": "Playback Start",
            "PlaybackProgress": "Playback Progress",
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
                    document.querySelector('#BotToken').value = userConfig.BotToken;
                    document.querySelector('#ChatId').value = userConfig.ChatId;
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
                        userConfig.BotToken = document.querySelector('#BotToken').value;
                        userConfig.ChatId = document.querySelector('#ChatId').value;
                    } else {
                        config.UserConfigurations.push({
                            UserId: TelegramNotifierConfig.user.getSelectedUserId(),
                            UserName: document.querySelector('#userToConfigure').selectedOptions[0].text,
                            BotToken: document.querySelector('#BotToken').value,
                            ChatId: document.querySelector('#ChatId').value
                        });
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
                    const params = {
                        botToken: userConfig.BotToken,
                        chatId: userConfig.ChatId
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