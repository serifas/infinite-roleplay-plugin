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
using InfiniteRoleplay.Helpers;
using Dalamud.Plugin.Services;
using Dalamud.Interface.Internal.Windows;

namespace InfiniteRoleplay
{
    public sealed class Plugin : IDalamudPlugin
    {
      

        public bool loggedIn;
        public bool toggleconnection;
        public bool firstload = true;
        public bool targeted = false;
        public bool loadCallback = false;
        public bool loadPreview = false;
        public string socketStatus;
        public DalamudPluginInterface PluginInterfacePub;
        public TargetWindow targetWindow;
        public TargetMenu targetMenu;
        public ImagePreview imagePreview;
        public BookmarksWindow bookmarksWindow;
        public OptionsWindow optionsWindow;
        public LoginWindow loginWindow;
        public ProfileWindow profileWindow;
        public string Name => "Infinite Roleplay";
        private const string CommandName = "/infinite";
        private DalamudPluginInterface pluginInterface { get; init; }
       
        public ITargetManager targetManager { get; init; }
        public IClientState clientState { get; init; }
        public static IClientState _clientState;
        private IFramework framework { get; init; }
        private IDutyState dutyState { get; init; }
        private ICommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("InfiniteRoleplay");
        public Plugin([RequiredVersion("1.0")] IClientState ClientState,
                      [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
                      [RequiredVersion("1.0")] IFramework framework,
                      [RequiredVersion("1.0")] ITargetManager targetManager,
                      [RequiredVersion("1.0")] IDutyState dutyState,
                      [RequiredVersion("1.0")] ICommandManager commandManager)
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
            var pictureTab = Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/common/picturetab.png");
            var blank_holder = Path.Combine(pluginInterface.AssemblyLocation.Directory?.FullName!, "UI/common/blank.png");

            byte[] imgBytes = File.ReadAllBytes(pictureTab);

            byte[] picTabBytes = Imaging.ScaleImageBytes(imgBytes, 300, 300);
            System.Drawing.Image picTab = Imaging.byteArrayToImage(picTabBytes);
            byte[] emptyByteImage = File.ReadAllBytes(blank_holder);

            byte[] blankTabBytes = Imaging.ScaleImageBytes(emptyByteImage, 300, 300);
            System.Drawing.Image blankTab = Imaging.byteArrayToImage(blankTabBytes);


            targetWindow = new TargetWindow(this, this.pluginInterface, AvatarHolder,
                                                             lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil,
                                                             lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar);
            targetMenu = new TargetMenu(this, this.pluginInterface, targetManager);

            imagePreview = new ImagePreview(this, this.pluginInterface, targetManager);

            bookmarksWindow = new BookmarksWindow(this, this.pluginInterface);

            optionsWindow = new OptionsWindow(this, this.pluginInterface, targetManager);

            loginWindow = new LoginWindow(this, this.clientState.LocalPlayer);
            profileWindow = new ProfileWindow(this, this.pluginInterface, this.Configuration, AvatarHolder,
                                                                lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil,
                                                                lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar, picTab, blankTab);
            // this.WindowSystem.AddWindow(new Loader(this.pluginInterface, this));
            // this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(profileWindow);

            //  this.WindowSystem.AddWindow(new Rulebook(this));
            this.WindowSystem.AddWindow(loginWindow);
            //this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(optionsWindow);
            //   this.WindowSystem.AddWindow(new MessageBox(this));
            //  this.WindowSystem.AddWindow(new AdminWindow(this, this.pluginInterface));
            this.WindowSystem.AddWindow(targetWindow);
            this.WindowSystem.AddWindow(targetMenu);
            this.WindowSystem.AddWindow(bookmarksWindow);
            this.WindowSystem.AddWindow(imagePreview);

        }
        public void Dispose()
        {
            this.framework.Update -= Update;
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);
            if (IsConnectedToServer(ClientTCP.clientSocket) == true)
            {
                DisconnectFromServer();
            }
           
            
        }

        public void RefreshConnection()
        {
            if(IsConnectedToServer(ClientTCP.clientSocket) != true)
            {
                ConnectToServer();
                ReloadClient();

            }
               
         
        }

        public void Update(IFramework framework)
        {
            if (IsConnectedToServer(ClientTCP.clientSocket) == true)
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
            if(firstload == true && IsConnectedToServer(ClientTCP.clientSocket) == true)
            {
                firstload = false;
                LoadUI();
            }
            var targetPlayer = targetManager.Target as PlayerCharacter;
            if(loggedIn == true)
            {
                if (targetPlayer != null && dutyState.IsDutyStarted == false)
                {
                   targetMenu.IsOpen = true;
                }
                else
                {
                    targetMenu.IsOpen = false;
                }
            }
            if(loadPreview == true)
            {
                imagePreview.IsOpen = true;
                loadPreview = false;
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
        public bool IsConnectedToServer(TcpClient _tcpClient)
        {
           
                try
                {
                    if (_tcpClient != null && _tcpClient.Client != null && _tcpClient.Client.Connected)
                    {
                        /* pear to the documentation on Poll:
                         * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                         * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                         * -or- true if data is available for reading; 
                         * -or- true if the connection has been closed, reset, or terminated; 
                         * otherwise, returns false
                         */

                        // Detect if client disconnected
                        if (_tcpClient.Client.Poll(0, SelectMode.SelectRead))
                        {
                            byte[] buff = new byte[1];
                            if (_tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                            {
                                // Client disconnected
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }

                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
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
                optionsWindow.IsOpen = true;
                loginWindow.IsOpen = false;
            }
            else
            {
                loginWindow.IsOpen = true;
            }
        }
        



    }
}
