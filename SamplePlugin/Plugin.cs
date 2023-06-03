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
using Windows.UI.Xaml;
using System.Net.Sockets;
using Windows.Networking.Sockets;
using System.Collections.Generic;
using Window = Dalamud.Interface.Windowing.Window;
using System.Numerics;
using InfiniteRP.Windows;
using InfiniteRoleplay;

using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using UpdateTest;
using ImGuiScene;
using Dalamud.Game.ClientState;
using Dalamud.Game.Gui;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.SubKinds;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using Uno.UI.DataBinding;
using Windows.Storage.Pickers.Provider;
using Dalamud.Game;
using System.Runtime;
using Dalamud.Game.DutyState;
using FFXIVClientStructs.FFXIV.Client.Game.Event;

namespace InfiniteRoleplay
{
    public sealed class Plugin : IDalamudPlugin
    {
        public bool toggleconnection = true;
        public bool loggedIn;
        public bool targeted = false;
        public bool loadCallback = true;
        public string socketStatus;
        public DalamudPluginInterface PluginInterfacePub; 
        public string Name => "Infinite Plugin";
        private const string CommandName = "/infinite";
        private DalamudPluginInterface PluginInterface { get; init; }
        public bool loadNetworking = true;
        public TargetManager targetManager { get; init; }
        private ClientState clientState { get; init; }
        private DutyState dutyState { get; init; }
        private SortedList<string, string> targetedPlayers = new SortedList<string, string>();
        private ChatGui chatgui { get; init; }
        private Framework framework { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("InfinitePlugin");
        public bool status = false;
        public Dictionary<int, string> characters = new Dictionary<int, string>();
        public Plugin(

            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
            [RequiredVersion("1.0")] ClientState ClientState,
            [RequiredVersion("1.0")] DutyState DutyState,
            [RequiredVersion("1.0")] ChatGui chatgui,
            [RequiredVersion("1.0")] Framework framework,
            [RequiredVersion("1.0")] TargetManager targetManager,

            [RequiredVersion("1.0")] CommandManager commandManager)
        {

            try
            {
                Dalamud.Initialize(pluginInterface);
            }
            catch
            {
               
            }
            this.PluginInterface = pluginInterface;
            this.CommandManager = commandManager;
            this.PluginInterfacePub = pluginInterface;
            this.clientState = ClientState;
            this.targetManager = targetManager;
            this.chatgui = chatgui;
            this.framework = framework;
            this.dutyState = DutyState;
            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.framework.Update +=UpdateData;
            this.Configuration.Initialize(this.PluginInterface);           
            TextureWrap AvatarHolder = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "profile_avis/avatar_holder.png"));
            TextureWrap lawfulGoodMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap lawfulGoodPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap neutralGoodMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap neutralGoodPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap chaoticGoodMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap chaoticGoodPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap lawfulNeutralMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap lawfulNeutralPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap trueNeutralMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap trueNeutralPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap chaoticNeutralMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap chaoticNeutralPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap lawfulEvilMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap lawfulEvilPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap neutralEvilMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap neutralEvilPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));
            TextureWrap chaoticEvilMinus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/-.png"));
            TextureWrap chaoticEvilPlus = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/+.png"));

            //Icons

            TextureWrap lawfulGood = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/lawful_good.png"));
            TextureWrap neutralGood = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/neutral_good.png"));
            TextureWrap chaoticGood = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/chaotic_good.png"));
            TextureWrap lawfulNeutral = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/lawful_neutral.png"));
            TextureWrap trueNeutral = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/true_neutral.png"));
            TextureWrap chaoticNeutral = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/chaotic_neutral.png"));
            TextureWrap lawfulEvil = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/lawful_evil.png"));
            TextureWrap neutralEvil = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/neutral_evil.png"));
            TextureWrap chaoticEvil = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/chaotic_evil.png"));

            //bars

            TextureWrap lawfulGoodBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/lawful_good_bar.png"));
            TextureWrap neutralGoodBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/neutral_good_bar.png"));
            TextureWrap chaoticGoodBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/chaotic_good_bar.png"));
            TextureWrap lawfulNeutralBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/lawful_neutral_bar.png"));
            TextureWrap trueNeutralBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/true_neutral_bar.png"));
            TextureWrap chaoticNeutralBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/chaotic_neutral_bar.png"));
            TextureWrap lawfulEvilBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/lawful_evil_bar.png"));
            TextureWrap neutralEvilBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/neutral_evil_bar.png"));
            TextureWrap chaoticEvilBar = this.PluginInterface.UiBuilder.LoadImage(Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "alignments/chaotic_evil_bar.png"));





            



            string name = "";

            Window systemWindow = new SystemsWindow(this);



            this.WindowSystem.AddWindow(systemWindow);
            this.WindowSystem.AddWindow(new ProfileWindow(this, chatgui, clientState.LocalPlayer, this.PluginInterface, AvatarHolder, 
                                                                lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil, 
                                                                lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar,
                                                                lawfulGoodPlus, neutralGoodPlus, chaoticGoodPlus, lawfulNeutralPlus, trueNeutralPlus, chaoticNeutralPlus, lawfulEvilPlus, neutralEvilPlus, chaoticEvilPlus,
                                                                lawfulGoodMinus, neutralGoodMinus, chaoticGoodMinus, lawfulNeutralMinus, trueNeutralMinus, chaoticNeutralMinus, lawfulEvilMinus, neutralEvilMinus, chaoticEvilMinus));
            this.WindowSystem.AddWindow(new Rulebook(this));
            this.WindowSystem.AddWindow(new LoginWindow(this));
            //this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(new OptionsWindow(this, this.PluginInterface, clientState.LocalPlayer, targetManager, chatgui));
            this.WindowSystem.AddWindow(new MessageBox(this));
            this.WindowSystem.AddWindow(new AdminWindow(this, this.PluginInterface));
            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "to open the plugin window"
            });
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawLoginUI;
            DataReceiver.plugin = this;
            this.WindowSystem.AddWindow(new TargetWindow(this, chatgui, this.PluginInterface, AvatarHolder,
                                                                lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil,
                                                                lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar,
                                                                lawfulGoodPlus, neutralGoodPlus, chaoticGoodPlus, lawfulNeutralPlus, trueNeutralPlus, chaoticNeutralPlus, lawfulEvilPlus, neutralEvilPlus, chaoticEvilPlus,
                                                                lawfulGoodMinus, neutralGoodMinus, chaoticGoodMinus, lawfulNeutralMinus, trueNeutralMinus, chaoticNeutralMinus, lawfulEvilMinus, neutralEvilMinus, chaoticEvilMinus));


        }

        

        public void Dispose()
        {
            this.framework.Update -= UpdateData;
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
            ClientHandleData.InitializePackets(false);
            ClientTCP.InitializingNetworking(false);
        }
        public void UpdateData(Framework framework)
        {
            if (IsConnectedToServer())
            {
                toggleconnection = false;
            }
            else
            {
                toggleconnection = true;
            }
            if (IsConnectedToServer() == false && IsLoggedIn() == true && toggleconnection == true)
            {
                ConnectToServer();
            }
            if (IsConnectedToServer() == false && IsLoggedIn() == false)
            {
                DisconnectFromServer();
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
            if(ClientTCP.clientSocket.Connected == true)
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
            if (clientState.IsLoggedIn && clientState.LocalPlayer != null && clientState.LocalPlayer.IsValid())
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
            ClientTCP.ClientConnectionCallback();
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
