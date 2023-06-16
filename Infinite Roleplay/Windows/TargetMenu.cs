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
    internal class TargetMenu : Window, IDisposable
    {

        private Plugin plugin;
        private string profileViewImagePath;
        private TextureWrap profileViewImage;
        private string requestFriendImagePath;
        private TextureWrap requestFriendImage;
        private string addBookmarkImagePath;
        private TextureWrap addBookmarkImage;
        private string removeBookmarkImagePath;
        private TextureWrap removeBookmarkImage;
        private string groupInviteImagePath;
        private TextureWrap groupInviteImage;
        public static bool isAdmin;
        public Configuration configuration;
        public static bool WindowOpen;
        public string msg;
        private TargetManager targetManager;
        public static PlayerCharacter playerCharacter;
        private ChatGui ChatGUI;
        public static PlayerCharacter lastTarget;
        private bool _showFileDialogError = false;
        public bool openedProfile = false;
        public bool openedTargetProfile = false;
        public TargetMenu(Plugin plugin, DalamudPluginInterface Interface, TargetManager targetManager) : base(
       "TARGET OPTIONS", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(280, 100),
                MaximumSize = new Vector2(280, 100)
            };
            this.plugin = plugin;
            this.configuration = plugin.Configuration;
            this.profileViewImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/profile_view.png");
            this.profileViewImage = Interface.UiBuilder.LoadImage(profileViewImagePath);
            this.requestFriendImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/friend_request.png");
            this.requestFriendImage = Interface.UiBuilder.LoadImage(requestFriendImagePath);
            this.addBookmarkImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/bookmark.png");
            this.addBookmarkImage = Interface.UiBuilder.LoadImage(addBookmarkImagePath);
            this.removeBookmarkImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/remove_bookmark.png");
            this.removeBookmarkImage = Interface.UiBuilder.LoadImage(removeBookmarkImagePath);
            this.groupInviteImagePath = Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/group_invite.png");
            this.groupInviteImage = Interface.UiBuilder.LoadImage(groupInviteImagePath);
            this.targetManager = targetManager;

        }

        public override void Draw()
        {
            if (ImGui.ImageButton(this.profileViewImage.ImGuiHandle, new Vector2(50, 50)))
            {
                LoginWindow.loginRequest = true;
                plugin.ReloadClient();

                var targetPlayer = targetManager.Target as PlayerCharacter;
                if (targetPlayer != null)
                {
                    DataSender.RequestTargetProfile(targetPlayer.Name.ToString(), targetPlayer.HomeWorld.GameData.Name.ToString());
                }
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("View Profile");
            }


            ImGui.SameLine();

            if (ImGui.ImageButton(this.requestFriendImage.ImGuiHandle, new Vector2(50, 50)))
            {
                //plugin.WindowSystem.GetWindow("SHINE RULEBOOK").IsOpen = true;

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Friend Request");
            }


            ImGui.SameLine();

            if (ImGui.ImageButton(this.groupInviteImage.ImGuiHandle, new Vector2(50, 50)))
            {
                
            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Group Invite");
            }


            ImGui.SameLine();

            if (ImGui.ImageButton(this.addBookmarkImage.ImGuiHandle, new Vector2(50, 50)))
            {

            }
            if (ImGui.IsItemHovered())
            {
                ImGui.SetTooltip("Bookmark Profile");
            }


        }


        public void Dispose()
        {
            WindowOpen = false;
        }
        public override void Update()
        { 
        }


    }
}
