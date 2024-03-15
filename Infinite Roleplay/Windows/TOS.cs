using Dalamud.Interface.Colors;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.Utility;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using FFXIVClientStructs.Havok;
using ImGuiNET;
using Networking;
using OtterGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Text;
using static Dalamud.Interface.Windowing.Window;

namespace InfiniteRoleplay.Windows
{
    public class TOS : Window, IDisposable
    {
        private GameFontHandle _nameFont;
        private float _modVersionWidth;
        public static Plugin pg;
        public static string verificationKey = string.Empty;
        public static string verificationStatus = string.Empty;
        public static Vector4 verificationCol = new Vector4(1, 1, 1, 1);
        public static string ToS1, ToS2;
        public static bool load;
        public TOS(Plugin plugin, DalamudPluginInterface Interface) : base(
        "TERMS OF SERVICE")
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(1200, 200),
                MaximumSize = new Vector2(1200, 1000)
            };
            pg = plugin;
            this._nameFont = plugin.PluginInterfacePub.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));

            load = true; 
            ToS1 = ReadTOS("https://raw.githubusercontent.com/serifas/infinite-roleplay-plugin/main/TOS1.txt");
            ToS2 = ReadTOS("https://raw.githubusercontent.com/serifas/infinite-roleplay-plugin/main/TOS2.txt");
        }
        public override async void Draw()
        {
           
            using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
            using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
            using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
            ImGuiUtil.DrawTextButton("Terms of Service", Vector2.Zero, 0);
            //set everything back
            using var defCol = ImRaii.DefaultColors();
            using var defStyle = ImRaii.DefaultStyle();
            using var defFont = ImRaii.DefaultFont();
        //okay that's done.
            ImGui.Text(ToS1);
            ImGui.Text(ToS2);
            //now for some simple toggles
    }

        public void Dispose()
        {

        }
       
        static string ReadTOS(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                return result;
            }
        }
    }
    
}
