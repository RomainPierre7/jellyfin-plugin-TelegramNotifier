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

        init: async function () {
            this.loadConfig();

            document.getElementById('testButton').addEventListener('click', this.testBotConfig);
            document.querySelector('#TelegramNotifierConfigForm').addEventListener('submit', this.saveConfig);
        },

        loadConfig: function () {
            Dashboard.showLoadingMsg();
            ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId).then(function (config) {
                document.querySelector('#EnablePlugin').checked = config.EnablePlugin;
                document.querySelector('#BotToken').value = config.BotToken;
                document.querySelector('#ChatId').value = config.ChatId;
                Dashboard.hideLoadingMsg();
            });
        },

        saveConfig: function () {
            Dashboard.showLoadingMsg();
            ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId).then(function (config) {
                config.EnablePlugin = document.querySelector('#EnablePlugin').checked;
                config.BotToken = document.querySelector('#BotToken').value;
                config.ChatId = document.querySelector('#ChatId').value;
                config.UserConfigurations = [
                    {
                        UBotToken: "Test",
                        UChatId: "Test"
                    }
                ];
                ApiClient.updatePluginConfiguration(TelegramNotifierConfig.pluginUniqueId, config).then(function (result) {
                    Dashboard.processPluginConfigurationUpdateResult(result);
                });
            });
            e.preventDefault();
            return false;
        },

        testBotConfig: function () {
            var button = this;
            button.disabled = true;
            ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId)
                .then(function (config) {
                    Dashboard.showLoadingMsg();
                    config.BotToken = document.querySelector('#BotToken').value;
                    config.ChatId = document.querySelector('#ChatId').value;
                    return ApiClient.updatePluginConfiguration(TelegramNotifierConfig.pluginUniqueId, config);
                })
                .then(function () {
                    return ApiClient.getPluginConfiguration(TelegramNotifierConfig.pluginUniqueId);
                })
                .then(function (config) {
                    const params = {
                        botToken: config.BotToken,
                        chatId: config.ChatId
                    };
                    console.log(params);
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