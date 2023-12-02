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
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using FFXIVClientStructs.Havok;
using System.Text.RegularExpressions;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Interface.Utility;

namespace InfiniteRoleplay.Windows
{
    public class BookmarksWindow : Window, IDisposable
    {
        private Plugin plugin;
        private GameFontHandle _nameFont;
        private GameFontHandle _infoFont;
        private float _modVersionWidth;
        public static SortedList<string, string> profiles = new SortedList<string, string>();
        private DalamudPluginInterface pg;
        private TargetWindow TargetWindow;
        public static bool DisableBookmarkSelection = false;
        public BookmarksWindow(Plugin plugin, DalamudPluginInterface Interface, TargetWindow targetWindow) : base(
       "BOOKMARKS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(300, 500),
                MaximumSize = new Vector2(500, 800)
            };
            this.plugin = plugin;
            this.pg = Interface;
            this._nameFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            this._infoFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter16));
            this.TargetWindow = targetWindow;
        }
        public override void Draw()
        {

            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
            var _nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
            ImGuiUtil.DrawTextButton("Profiles", Vector2.Zero, 0);
            using var defInfFontDen = ImRaii.DefaultFont();
            using var DefaultColor = ImRaii.DefaultColors();

            if (ImGui.BeginChild("Profiles", new Vector2(290, 380), true))
            {
                for (int i = 1; i < profiles.Count; i++)
                {
                    if (DisableBookmarkSelection == true)
                    {
                        ImGui.BeginDisabled();
                    }
                    if (ImGui.Button(profiles.Keys[i] + " @ " + profiles.Values[i]))
                    {
                        plugin.ReloadTarget();
                        LoginWindow.loginRequest = true;
                        DisableBookmarkSelection = true;
                        plugin.targetWindow.IsOpen = true;
                        DataSender.RequestTargetProfile(profiles.Keys[i], profiles.Values[i]);

                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Remove##Removal" + i))
                    {
                        LoginWindow.loginRequest = true;
                        plugin.ReloadClient();
                        DataSender.RemoveBookmarkedPlayer(plugin.Configuration.username.ToString(), profiles.Keys[i], profiles.Values[i]);
                    }
                    if (DisableBookmarkSelection == true)
                    {
                        ImGui.EndDisabled();
                    }

                }

            }
            ImGui.EndChild();

        }

        public void Dispose()
        {

        }
        public override void Update()
        {

        }
    }
}
