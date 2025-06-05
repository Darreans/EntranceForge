using BepInEx;
// using BepInEx.Logging; // No longer strictly needed if LogInstance and LoggingHelper are removed
using BepInEx.Unity.IL2CPP;
using Bloodstone.API;
using HarmonyLib;

namespace EntranceForge
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [Reloadable] 
    public class Plugin : BasePlugin
    {
        private Harmony _harmony;

        public override void Load()
        {
          

            _harmony = new Harmony(PluginInfo.PLUGIN_GUID);
            _harmony.PatchAll(typeof(Plugin).Assembly);

        }

        public override bool Unload()
        {
            if (_harmony != null)
            {
                _harmony.UnpatchSelf();
            }
            return true;
        }
    }

    public static class PluginInfo
    {
        public const string PLUGIN_GUID = "Entranceforge";
        public const string PLUGIN_NAME = "EntranceForge";
        public const string PLUGIN_VERSION = "1.0.0";
    }
}