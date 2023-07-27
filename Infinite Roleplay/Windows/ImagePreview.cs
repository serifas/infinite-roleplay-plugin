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
    internal class ImagePreview : Window, IDisposable
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
        public static TextureWrap PreviewImage;
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
        public static int width = 0, height = 0;
        public bool openedTargetProfile = false;
        public ImagePreview(Plugin plugin, DalamudPluginInterface Interface, TargetManager targetManager) : base(
       "PREVIEW", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            

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
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(width, height),
                MaximumSize = new Vector2(width, height)
            };
            ImGui.Image(PreviewImage.ImGuiHandle, new Vector2(width, height));
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