using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ImGuiNET;
using ImGuiScene;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using UpdateTest;
using static Dalamud.Interface.Windowing.Window;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.ImGuiFileDialog;
using ImGuiNET;
using ImGuiScene;
using static Lumina.Data.Files.ScdFile;

namespace InfiniteRoleplay.Windows
{
    internal class OptionsWindow : Window, IDisposable
    {

        private Plugin plugin;
        private string profilesImagePath;
        private TextureWrap profilesImage;
        private string documentImagePath;
        private TextureWrap documentImage;
        private string systemsImagePath;
        private TextureWrap systemsImage;
        private string groupsImagePath;
        private TextureWrap groupsImage;
        public static bool isAdmin;
        public Configuration configuration;
        public static bool WindowOpen;
        public string msg;

        private bool _showFileDialogError = false;
        public OptionsWindow(Plugin plugin, DalamudPluginInterface Interface) : base(
       "OPTIONS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(250, 225),
                MaximumSize = new Vector2(250, 225)
            };
            this.plugin = plugin;
            this.configuration = plugin.Configuration;
            this.profilesImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "common/profiles.png");
            this.profilesImage = Interface.UiBuilder.LoadImage(profilesImagePath);
            this.documentImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "common/friends.png");
            this.documentImage = Interface.UiBuilder.LoadImage(documentImagePath);
            this.groupsImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "common/groups.png");
            this.groupsImage = Interface.UiBuilder.LoadImage(groupsImagePath);
            this.systemsImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "common/systems.png");
            this.systemsImage = Interface.UiBuilder.LoadImage(systemsImagePath);


        }
        public override void Draw()
        {
            if (ImGui.ImageButton(this.profilesImage.ImGuiHandle, new Vector2(100, 50)))
            {
                plugin.WindowSystem.GetWindow("PROFILES").IsOpen = true;
                //  DataSender.FetchProfiles(this.plugin.Configuration.username);
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Profile");
            }
            ImGui.SameLine();
            if (ImGui.ImageButton(this.documentImage.ImGuiHandle, new Vector2(100, 50)))
            {
                plugin.WindowSystem.GetWindow("SHINE RULEBOOK").IsOpen = true;

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Rule Book");
            }
            if (ImGui.ImageButton(this.systemsImage.ImGuiHandle, new Vector2(100, 50)))
            {
                plugin.WindowSystem.GetWindow("SYSTEMS").IsOpen = true;
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Systems");
            }

            ImGui.SameLine();
            if (ImGui.ImageButton(this.groupsImage.ImGuiHandle, new Vector2(100, 50)))
            {

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Events");
            }
            if (isAdmin == true)
            {
                if (ImGui.Button("Administration", new Vector2(225, 25)))
                {
                     plugin.WindowSystem.GetWindow("ADMINISTRATION").IsOpen = true;
                 }
             }



        }
        public void Dispose()
        {
            WindowOpen = false;
        }
        public override void Update()
        {
            isAdmin = DataReceiver.isAdmin;
            msg = DataReceiver.ConnectionMsg;


        }

    }
}
