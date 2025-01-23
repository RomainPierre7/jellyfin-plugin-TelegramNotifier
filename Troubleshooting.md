# Troubleshooting guide

If you're having trouble with the plugin, you can follow these steps to troubleshoot the issue:

1. **Check the logs**: The plugin logs are stored in the Jellyfin logs. You can access them by going to the Jellyfin dashboard, then to the **Dashboard** section, and finally to the **Logs** tab. Look for any error messages related to the plugin.

> **Note:** Use ctrl+f to search for the plugin name in the logs.

2. **Check the plugin settings**: Make sure that the plugin settings are correctly configured. You can access the plugin settings by going to the Jellyfin dashboard, then to the **Plugins** section, and finally to the **Installed** tab. Click on the **Telegram Notifier** plugin and check the settings.

3. **Check the plugin version**: Make sure that you are using the latest version of the plugin. You can check the plugin version by going to the Jellyfin dashboard, then to the **Plugins** section, and finally to the **Installed** tab. Look for the **Telegram Notifier** plugin and check the version.

4. **Try restarting the Jellyfin server**: Sometimes, restarting the Jellyfin server can resolve issues with the plugin. You can restart the server by going to the Jellyfin dashboard, then to the **Dashboard** section, and finally to the **Restart** tab.

5. **Try reinstalling the plugin**: If none of the above steps work, you can try reinstalling the plugin. You can reinstall the plugin by going to the Jellyfin dashboard, then to the **Plugins** section, and finally to the **Installed** tab. Look for the **Telegram Notifier** plugin and click on the **Uninstall** button. Then, follow the steps to reinstall the plugin.

> **Note:** To do a fresh install, you may have to delete the plugin files from the Jellyfin server manually. You can find the plugin files in the Jellyfin installation directory under the **plugins** folder (the location may vary depending on your system). Delete the `Telegram Notifier` folders and the corresponding `.xml` file in the `configurations` folder.

Path examples:

- **Linux**: ```/var/lib/jellyfin/plugins```
- **Windows**: ```C:\ProgramData\Jellyfin\Server\plugins```
- **Docker**: ```/config/plugins``` or ```/config/data/plugins```

6. **Submit an issue**: If you are still facing issues with the plugin, you can submit an issue [here](https://github.com/RomainPierre7/jellyfin-plugin-TelegramNotifier/issues).