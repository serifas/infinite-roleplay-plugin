using Dalamud.Game.Command;
using Dalamud.IoC;
using Dalamud.Plugin;
using System.IO;
using System.Reflection;
using Dalamud.Interface.Windowing;
using InfiniteRoleplay.Windows;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Numerics;
using InfiniteRoleplay;
using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using Dalamud.Game;
using System.Runtime;
using Dalamud.Game.DutyState;
using FFXIVClientStructs.FFXIV.Client.Game.Event;
using Lumina.Excel.GeneratedSheets;
using Networking;
using InfiniteRP.Windows;

namespace InfiniteRoleplay
{
    public sealed class Plugin : IDalamudPlugin
    {
      

        public bool loggedIn;
        public bool toggleconnection;
        public bool firstload = true;
        public bool targeted = false;
        public bool loadCallback = false;
        public string socketStatus;
        public DalamudPluginInterface PluginInterfacePub;
        public string Name => "Infinite Roleplay";
        private const string CommandName = "/infinite";
        private DalamudPluginInterface pluginInterface { get; init; }
       
        public TargetManager targetManager { get; init; }
        public ClientState clientState { get; init; }
        public static ClientState _clientState;
        private Framework framework { get; init; }
        private DutyState dutyState { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("InfiniteRoleplay");
        public Plugin([RequiredVersion("1.0")] ClientState ClientState,
                      [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
                      [RequiredVersion("1.0")] Framework framework,
                      [RequiredVersion("1.0")] TargetManager targetManager,
                      [RequiredVersion("1.0")] DutyState dutyState,
                      [RequiredVersion("1.0")] CommandManager commandManager)
        {

            
            this.pluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            this.PluginInterfacePub = pluginInterface;
            this.clientState = ClientState;
            _clientState = ClientState;
            this.targetManager = targetManager;
            this.framework = framework;
            this.dutyState = dutyState;
            this.Configuration = pluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(pluginInterface);
        


            string name = "";




             this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "to open the plugin window"
            });
            this.pluginInterface.UiBuilder.Draw += DrawUI;
            this.pluginInterface.UiBuilder.OpenConfigUi += DrawLoginUI;
            DataReceiver.plugin = this;
            ConnectToServer();
            ReloadClient();
            this.framework.Update += Update;

        }

        public void ReloadClient()
        {
            ProfileWindow.playerCharacter = this.clientState.LocalPlayer;
            OptionsWindow.playerCharacter = this.clientState.LocalPlayer;
            OptionsWindow.targetManager = this.targetManager;
        }
        public void LoadUI()
        {
            var AvatarHolder = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/common/avatar_holder.png"));
            //Icons

            var lawfulGood = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_good.png"));
            var neutralGood = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_good.png"));
            var chaoticGood = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_good.png"));
            var lawfulNeutral = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_neutral.png"));
            var trueNeutral = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/true_neutral.png"));
            var chaoticNeutral = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_neutral.png"));
            var lawfulEvil = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_evil.png"));
            var neutralEvil = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_evil.png"));
            var chaoticEvil = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_evil.png"));

            //bars

            var lawfulGoodBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_good_bar.png"));
            var neutralGoodBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_good_bar.png"));
            var chaoticGoodBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_good_bar.png"));
            var lawfulNeutralBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_neutral_bar.png"));
            var trueNeutralBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/true_neutral_bar.png"));
            var chaoticNeutralBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_neutral_bar.png"));
            var lawfulEvilBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/lawful_evil_bar.png"));
            var neutralEvilBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/neutral_evil_bar.png"));
            var chaoticEvilBar = pluginInterface.UiBuilder.LoadImage(Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/alignments/chaotic_evil_bar.png"));

            this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(new ProfileWindow(this, this.pluginInterface, AvatarHolder,
                                                                lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil,
                                                                lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar));
            this.WindowSystem.AddWindow(new Rulebook(this));
            this.WindowSystem.AddWindow(new LoginWindow(this));
            //this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(new OptionsWindow(this, this.pluginInterface, targetManager));
            this.WindowSystem.AddWindow(new MessageBox(this));
            this.WindowSystem.AddWindow(new AdminWindow(this, this.pluginInterface));
            this.WindowSystem.AddWindow(new TargetWindow(this, this.pluginInterface, AvatarHolder,
                                                                lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil,
                                                                lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar));
            this.WindowSystem.AddWindow(new TargetMenu(this, this.pluginInterface, targetManager));
            this.WindowSystem.AddWindow(new BookmarksWindow(this, this.pluginInterface));


        }
        public void Dispose()
        {
            this.framework.Update -= Update;
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
            if (IsConnectedToServer() == true)
            {
                DisconnectFromServer();
            }
           
            
        }

        public void RefreshConnection()
        {
            if(IsConnectedToServer() != true)
            {
                ConnectToServer();
                ReloadClient();

            }
               
         
        }

        public void Update(Framework framework)
        {
            if (IsConnectedToServer() == true)
            {
                toggleconnection = false; 
                if (IsLoggedIn() == false)
                {                   
                    DisconnectFromServer();
                }
                if (loadCallback == true)
                {
                    ClientTCP.ClientConnectionCallback();
                    loadCallback = false;
                }
            }
            else
            {
                toggleconnection = true;
            }
            if (IsLoggedIn() == true && toggleconnection == true)
            {
                ConnectToServer();
                ReloadClient();
            }
            if(firstload == true && IsConnectedToServer() == true)
            {
                firstload = false;
                LoadUI();
            }
            var targetPlayer = targetManager.Target as PlayerCharacter;
            if(loggedIn == true)
            {
                if (targetPlayer != null && dutyState.IsDutyStarted == false)
                {
                    WindowSystem.GetWindow("TARGET OPTIONS").IsOpen = true;
                }
                else
                {
                    WindowSystem.GetWindow("TARGET OPTIONS").IsOpen = false;
                }
            }
            
        }

        private void OnCommand(string command, string args)
        {
            DrawLoginUI();
            // in response to the slash command, just display our main ui          
        }
        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }
        public static bool IsConnectedToServer()
        {
            if (ClientTCP.clientSocket.Connected == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsLoggedIn()
        {
            if (clientState.IsLoggedIn == true && clientState.LocalPlayer != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void ConnectToServer()
        {
            ClientHandleData.InitializePackets(true);
            ClientTCP.InitializingNetworking(true);

            loadCallback = true;
        }
        public void DisconnectFromServer()
        {
            ClientHandleData.InitializePackets(false);
            ClientTCP.InitializingNetworking(false);
        }
        public void DrawLoginUI()
        {
            
            if (loggedIn == true)
            {
                this.WindowSystem.GetWindow("OPTIONS").IsOpen = true;
                this.WindowSystem.GetWindow("LOGIN").IsOpen = false;
            }
            else
            {
                this.WindowSystem.GetWindow("LOGIN").IsOpen = true;
            }
        }



    }
}
