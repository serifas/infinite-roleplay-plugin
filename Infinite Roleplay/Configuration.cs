using Dalamud.Configuration;
using Dalamud.Plugin;
using System;

namespace InfiniteRoleplay
{
    [Serializable]
    public class Configuration : IPluginConfiguration
    {
        public int Version { get; set; } = 0;
        public bool AlwaysOpenDefaultImport { get; set; } = false;
        public bool StayOnline { get; set; }
        public string username { get; set; } = "";
        public string password { get; set; } = "";
        public bool SomePropertyToBeSavedAndWithADefault { get; set; } = true;

        // the below exist just to make saving less cumbersome
        [NonSerialized]
        private DalamudPluginInterface? PluginInterface;

        public void Initialize(DalamudPluginInterface pluginInterface)
        {
            this.PluginInterface = pluginInterface;
        }

        public void Save()
        {
            this.PluginInterface!.SavePluginConfig(this);
        }
    }
}