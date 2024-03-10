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
    public class RestorationWindow : Window, IDisposable
    {
        private GameFontHandle _nameFont;
        private float _modVersionWidth;
        public static Plugin pg;
        public static string restorationKey = string.Empty;
        public static string restorationPass = string.Empty;
        public static string restorationPassConfirm = string.Empty;
        public static string restorationEmail = string.Empty;
        public static string restorationUsername = string.Empty;
        public RestorationWindow(Plugin plugin, DalamudPluginInterface Interface) : base(
       "RESTORATION", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(350, 400),
                MaximumSize = new Vector2(350,400)
            };
            pg = plugin;
            this._nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
          
        }
        public override void Draw()
        {
            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
            ImGuiUtil.DrawTextButton("Account Restoration", Vector2.Zero, 0);
            //set everything back
            using var defCol = ImRaii.DefaultColors();
            using var defStyle = ImRaii.DefaultStyle();
            using var defFont = ImRaii.DefaultFont();
            //okay that's done.
            ImGui.Text("We sent a restoration key to the email address provided. \nPlease enter the key with a new username and password below.");
            ImGui.Spacing();
            //now for some simple toggles
            ImGui.InputText("Restoration Key", ref restorationKey, 10);
            ImGui.InputText("New Username", ref restorationUsername, 25);
            ImGui.InputText("New Password", ref restorationPass, 30, ImGuiInputTextFlags.Password);
            ImGui.InputText("New Password Confirmation", ref restorationPassConfirm, 30, ImGuiInputTextFlags.Password);
            if (ImGui.Button("Submit"))
            {
                if(restorationKey != string.Empty && restorationUsername != string.Empty && restorationPass != string.Empty && restorationPassConfirm != string.Empty)
                {
                    if (restorationPass == restorationPassConfirm)
                    {
                        DataSender.SendRestoration(restorationUsername, restorationEmail, restorationPass, restorationKey);
                    }
                   

                }
               
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
