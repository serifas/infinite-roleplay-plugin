using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ImGuiNET;
using ImGuiScene;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using static Dalamud.Interface.Windowing.Window;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.ImGuiFileDialog;
using ImGuiNET;
using ImGuiScene;
using static Lumina.Data.Files.ScdFile;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.Gui;
using Dalamud.Game.ClientState;
using Networking;

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
        public static TargetManager targetManager;
        public static PlayerCharacter playerCharacter;
        private ChatGui ChatGUI;
        public static PlayerCharacter lastTarget;
        private bool _showFileDialogError = false;
        public bool openedProfile = false;
        public bool openedTargetProfile = false;
        public OptionsWindow(Plugin plugin, DalamudPluginInterface Interface, TargetManager targetManager) : base(
       "OPTIONS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(250, 225),
                MaximumSize = new Vector2(250, 225)
            };
            
            this.plugin = plugin;
            this.configuration = plugin.Configuration;
            this.profilesImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/profiles.png");
            this.profilesImage = Interface.UiBuilder.LoadImage(profilesImagePath);
            this.documentImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/friends.png");
            this.documentImage = Interface.UiBuilder.LoadImage(documentImagePath);
            this.groupsImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/groups.png");
            this.groupsImage = Interface.UiBuilder.LoadImage(groupsImagePath);
            this.systemsImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/bookmarks.png");
            this.systemsImage = Interface.UiBuilder.LoadImage(systemsImagePath);

         
        }

        public override void Draw()
        {
            if (ImGui.ImageButton(this.profilesImage.ImGuiHandle, new Vector2(100, 50)))
            {
                LoginWindow.loginRequest = true;
                plugin.ReloadClient();                
                ProfileWindow.ClearUI();
                DataSender.FetchProfile(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name);

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Edit Profile");
            }
            ImGui.SameLine();
            if (ImGui.ImageButton(this.documentImage.ImGuiHandle, new Vector2(100, 50)))
            {
               // plugin.WindowSystem.GetWindow("SHINE RULEBOOK").IsOpen = true;

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Friends");
            }
            if (ImGui.ImageButton(this.systemsImage.ImGuiHandle, new Vector2(100, 50)))
            {
                DataSender.RequestBookmarks(configuration.username);
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Bookmarks");
            }

            ImGui.SameLine();
            if (ImGui.ImageButton(this.groupsImage.ImGuiHandle, new Vector2(100, 50)))
            {

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Groups");
            }
            if (isAdmin == true)
            {
                if (ImGui.Button("Administration", new Vector2(225, 25)))
                {
                    plugin.WindowSystem.GetWindow("ADMINISTRATION").IsOpen = true;
                }
            }
            if (ImGui.Button("Logout", new Vector2(225, 25)))
            {
                plugin.loggedIn = false;
                plugin.DisconnectFromServer();
                plugin.WindowSystem.GetWindow("LOGIN").IsOpen = true;
                plugin.WindowSystem.GetWindow("OPTIONS").IsOpen = false;
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
