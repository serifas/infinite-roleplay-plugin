using Dalamud.Interface.Colors;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using FFXIVClientStructs.FFXIV.Common.Math;
using ImGuiNET;
using ImGuiScene;
using InfiniteRoleplay;
using OtterGui.Raii;
using OtterGui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dalamud.Interface.GameFonts;
using Dalamud.Game.Gui.Dtr;
using Microsoft.VisualBasic;
using Networking;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Internal;
using System.Reflection.Metadata;

namespace InfiniteRoleplay.Windows
{
    public class OptionsWindow : Window, IDisposable
    {
        private GameFontHandle _nameFont;
        private float _modVersionWidth;
        public static Plugin pg;
        public static bool showTargetOptions;
        public static bool showWIP;
        public OptionsWindow(Plugin plugin, DalamudPluginInterface Interface) : base(
       "OPTIONS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(200, 300),
                MaximumSize = new Vector2(600, 800)
            };
            pg = plugin;
            this._nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));

        }
        public override void Draw()
        {
            //yes all this for a title
            var Name = "Options";
            var NameWidth = Name.Length * 10;
            var decidingWidth = Math.Max(500, ImGui.GetWindowWidth());
            var offsetWidth = (decidingWidth - NameWidth) / 2;
            var offsetVersion = Name.Length > 0
                ? _modVersionWidth + ImGui.GetStyle().ItemSpacing.X + ImGui.GetStyle().WindowPadding.X
                : 0;
            var offset = Math.Max(offsetWidth, offsetVersion);
            if (offset > 0)
            {
                ImGui.SetCursorPosX(offset);
            }


            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
            ImGuiUtil.DrawTextButton(Name, Vector2.Zero, 0);
            //set everything back
            using var defCol = ImRaii.DefaultColors();
            using var defStyle = ImRaii.DefaultStyle();
            using var defFont = ImRaii.DefaultFont();
            //okay that's done.
            ImGui.Spacing();
            //now for some simple toggles
            if(ImGui.Checkbox("Show target menu when selecting players", ref showTargetOptions))
            {
                pg.Configuration.showTargetOptions = showTargetOptions;
                pg.Configuration.Save();
            }
            if(ImGui.Checkbox("Show WIP modules", ref showWIP))
            {
                pg.Configuration.showWIP = showWIP;
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
