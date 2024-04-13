using MediaBrowser.Model.Plugins;

namespace Jellyfin.Plugin.TelegramNotifier.Configuration
{
    // Définir une classe pour contenir les paramètres de configuration de votre plugin
    public class PluginConfiguration : BasePluginConfiguration
    {
        // Vous pouvez ajouter des propriétés ici pour stocker les paramètres de configuration de votre plugin
        // Par exemple :
        public string Parametre1 { get; set; }
        public int Parametre2 { get; set; }
        // Ajoutez d'autres propriétés selon les besoins de votre plugin
    }
}

