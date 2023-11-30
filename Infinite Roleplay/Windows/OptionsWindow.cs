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
using Dalamud.Game.Config;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;

namespace InfiniteRoleplay.Windows
{
    public class OptionsWindow : Window, IDisposable
    {

        private Plugin plugin;
        private string profilesImagePath;
        private IDalamudTextureWrap profilesImage;
        private string documentImagePath;
        private IDalamudTextureWrap documentImage;
        private string systemsImagePath;
        private IDalamudTextureWrap systemsImage;
        private string groupsImagePath;
        private IDalamudTextureWrap groupsImage;
        public static bool isAdmin;
        public Configuration configuration;
        public static bool WindowOpen;
        public string msg;
        public static ITargetManager targetManager;
        public static PlayerCharacter playerCharacter;
        private IChatGui ChatGUI;
        public static PlayerCharacter lastTarget;
        private bool _showFileDialogError = false;
        public bool openedProfile = false;
        public bool openedTargetProfile = false;
        public static bool DisableInput = false;
        public OptionsWindow(Plugin plugin, DalamudPluginInterface Interface, ITargetManager targetManager) : base(
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
                plugin.profileWindow.IsOpen = true;
                DisableInput = true;
                ProfileWindow.ClearUI();
                DataSender.Login(configuration.username, configuration.password, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString()) ;
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Profile");
            }
            ImGui.SameLine();
            if (ImGui.ImageButton(this.documentImage.ImGuiHandle, new Vector2(100, 50)))
            {
               // plugin.WindowSystem.GetWindow("SHINE RULEBOOK").IsOpen = true;

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Connections - Comming Soon");
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
                ImGui.SetTooltip("Events - Comming Soon");
            }
            if (isAdmin == true)
            {
                if (ImGui.Button("Administration", new Vector2(225, 25)))
                {
                   //plugin.window("ADMINISTRATION").IsOpen = true;
                }
            }
            if (ImGui.Button("Logout", new Vector2(225, 25)))
            {
                plugin.loggedIn = false;
                plugin.DisconnectFromServer();
                plugin.loginWindow.IsOpen = true;
                plugin.optionsWindow.IsOpen = false;
            }


        }
       

        public void Dispose()
        {
            WindowOpen = false;
            profilesImage.Dispose();
            documentImage.Dispose();
            groupsImage.Dispose();
            systemsImage.Dispose();
        }
        public override void Update()
        {

            isAdmin = DataReceiver.isAdmin;
            msg = DataReceiver.ConnectionMsg;

        }


    }
}
