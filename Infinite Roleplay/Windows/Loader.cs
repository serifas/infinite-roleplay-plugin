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
using System.Timers;
using Dalamud.Interface.Internal;

namespace InfiniteRoleplay.Windows
{
    internal class Loader : Window, IDisposable
    {
        public static int loaderIndex = 0;
        public static IDalamudTextureWrap PreviewImage;
        public static bool isAdmin;
        public Configuration configuration;
        public static bool WindowOpen;
        public string msg;
        public static ITargetManager targetManager;
        public static PlayerCharacter playerCharacter;
        public static PlayerCharacter lastTarget;
        public bool openedProfile = false;
        public static int width = 0, height = 0;
        public bool openedTargetProfile = false;
        public static DalamudPluginInterface pg;
        public static IDalamudTextureWrap loaderAnimInd;
        public static Window window;
        public static int contentCount, currentCount;
        public Loader(DalamudPluginInterface pluginInterface, Plugin plugin) : base(
       "LOADING", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            pg = pluginInterface; 
            var timer = new Timer(30);
            timer.Elapsed += OnEventExecution;
            timer.Start();
           
        }

        public override void Draw()
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(360, 200),
                MaximumSize = new Vector2(360, 200)
            };
            ImGui.Image(loaderAnimInd.ImGuiHandle, new Vector2(340, 180));

        }

        public static void OnEventExecution(Object? sender, ElapsedEventArgs eventArgs)
        {
            loaderIndex++;
            if(loaderIndex >= 59)
            {
                loaderIndex = 1;
            }
            loaderAnimInd = pg.UiBuilder.LoadImage(Path.Combine(pg.AssemblyLocation.Directory?.FullName!, "UI/common/loader/loader (" + loaderIndex + ").gif"));
        }
        public void Dispose()
        {
            loaderAnimInd.Dispose();
            WindowOpen = false;
        }
        public override void Update()
        {
            if(currentCount >= contentCount)
            {
                Dispose();
                window.IsOpen = true;
            }
        }


    }
}
