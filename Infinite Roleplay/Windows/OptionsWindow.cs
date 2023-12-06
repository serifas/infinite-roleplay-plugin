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

namespace InfiniteRoleplay.Windows
{
    public class OptionsWindow : Window, IDisposable
    {

        public OptionsWindow(Plugin plugin, DalamudPluginInterface Interface) : base(
       "OPTIONS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(200, 300),
                MaximumSize = new Vector2(600, 800)
            };
          
        }
        public override void Draw()
        {
            ImGui.Text("Placeholder");
        }
        public void Dispose()
        {

        }
        public override void Update()
        {
            
        }
    }

}
