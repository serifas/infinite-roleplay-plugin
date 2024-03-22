using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using System;
using Dalamud.Interface.GameFonts;
using InfiniteRoleplay.Helpers;

namespace InfiniteRoleplay.Windows
{
    public class OptionsWindow : Window, IDisposable
    {
        private GameFontHandle _nameFont;
        private float _modVersionWidth;
        public static Plugin pg;
        public static bool showTargetOptions;
        public static bool showKofi;
        public static bool showDisc;
        public static bool showWIP;
        public OptionsWindow(Plugin plugin, DalamudPluginInterface Interface) : base(
       "OPTIONS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(300, 180),
                MaximumSize = new Vector2(300, 180)
            };
            pg = plugin;
            this._nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            showTargetOptions = plugin.Configuration.showTargetOptions;
            showWIP = plugin.Configuration.showWIP;
            showKofi = plugin.Configuration.showKofi;
            showDisc = plugin.Configuration.showDisc;
        }
        public override void Draw()
        {
            Misc.SetTitle(pg, false, "Options");
            //okay that's done.
            ImGui.Spacing();
            //now for some simple toggles
            if(ImGui.Checkbox("Show target menu when selecting players.", ref showTargetOptions))
            {
                pg.Configuration.showTargetOptions = showTargetOptions;
                pg.Configuration.Save();
            }
            if (ImGui.Checkbox("Show WIP options (Just extra clutter).", ref showWIP))
            {
                pg.Configuration.showWIP = showWIP;
                pg.Configuration.Save();
            }
            if (ImGui.Checkbox("Show Ko-fi Button", ref showKofi))
            {
                pg.Configuration.showKofi = showKofi;
                pg.Configuration.Save();
            }
            if (ImGui.Checkbox("Show Discord Button.", ref showDisc))
            {
                pg.Configuration.showDisc = showDisc;
                pg.Configuration.Save();
            }
        }
        public void Dispose()
        {

        }
        public override void Update()
        {
            
        }
    }

}
