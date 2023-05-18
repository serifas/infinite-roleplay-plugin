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

using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Dalamud.Interface.ImGuiFileDialog;
using UpdateTest;

namespace InfiniteRoleplay
{ 
    public sealed class Plugin : IDalamudPlugin
    {
        public bool loggedIn;
        public string socketStatus;
        public DalamudPluginInterface PluginInterfacePub;
        public string Name => "Infinite Plugin";
        private const string CommandName = "/infinite";
        private DalamudPluginInterface PluginInterface { get; init; }
        private CommandManager CommandManager { get; init; }
        public Configuration Configuration { get; init; }
        public WindowSystem WindowSystem = new("InfinitePlugin");
        public bool status = false;
        public Dictionary<int, string> characters = new Dictionary<int, string>();
        public Plugin(
           
            [RequiredVersion("1.0")] DalamudPluginInterface pluginInterface,
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
            this.Configuration = this.PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            this.Configuration.Initialize(this.PluginInterface);


          


            var plusEminence = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/emn+.png");
            var plusHardiness = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/hrd+.png");
            var plusIntelligence = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/int+.png");
            var plusNimbleness = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/nim+.png");
            var plusSenses = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/sen+.png");
            var plusStrength = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/str+.png");
            var minusEminence = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/emn-.png");
            var minusHardiness = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/hrd-.png");
            var minusIntelligence = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/int-.png");
            var minusNimbleness = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/nim-.png");
            var minusSenses = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/sen-.png");
            var minusStrength = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/str-.png");
            var health = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/health.png");
            var eminence = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/eminence.png");
            var hardiness = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/hardiness.png");
            var intelligence = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/intelligence.png");
            var nimbleness = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/nimbleness.png");
            var senses = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/senses.png");
            var strength = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/strength.png");
            var hpBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/hpBar.png");
            var emnBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/emnBar.png");
            var hrdBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/hrdBar.png");
            var intBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/intBar.png");
            var nimBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/nimBar.png");
            var senBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/senBar.png");
            var strBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/strBar.png");
            var ngBar = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "stats/ngBar.png");
            var avatarHolder = Path.Combine(PluginInterface.AssemblyLocation.Directory?.FullName!, "profile_avis/avatar_holder.png");
            var PlusEminence = this.PluginInterface.UiBuilder.LoadImage(plusEminence);
            var PlusHardiness = this.PluginInterface.UiBuilder.LoadImage(plusHardiness);
            var PlusIntelligence = this.PluginInterface.UiBuilder.LoadImage(plusIntelligence);
            var PlusNimbleness = this.PluginInterface.UiBuilder.LoadImage(plusNimbleness);
            var PlusSenses = this.PluginInterface.UiBuilder.LoadImage(plusSenses);
            var PlusStrength = this.PluginInterface.UiBuilder.LoadImage(plusStrength);
            var MinusEminence = this.PluginInterface.UiBuilder.LoadImage(minusEminence);
            var MinusHardiness = this.PluginInterface.UiBuilder.LoadImage(minusHardiness);
            var MinusIntelligence = this.PluginInterface.UiBuilder.LoadImage(minusIntelligence);
            var MinusNimbleness = this.PluginInterface.UiBuilder.LoadImage(minusNimbleness);
            var MinusSenses = this.PluginInterface.UiBuilder.LoadImage(minusSenses);
            var MinusStrength = this.PluginInterface.UiBuilder.LoadImage(minusStrength);
            var Health = this.PluginInterface.UiBuilder.LoadImage(health);
            var Eminence = this.PluginInterface.UiBuilder.LoadImage(eminence);
            var Hardiness = this.PluginInterface.UiBuilder.LoadImage(hardiness);
            var Intelligence = this.PluginInterface.UiBuilder.LoadImage(intelligence);
            var Nimbleness = this.PluginInterface.UiBuilder.LoadImage(nimbleness);
            var Senses = this.PluginInterface.UiBuilder.LoadImage(senses);
            var Strength = this.PluginInterface.UiBuilder.LoadImage(strength);
            var HpBar = this.PluginInterface.UiBuilder.LoadImage(hpBar);
            var EmnBar = this.PluginInterface.UiBuilder.LoadImage(emnBar);
            var HrdBar = this.PluginInterface.UiBuilder.LoadImage(hrdBar);
            var IntBar = this.PluginInterface.UiBuilder.LoadImage(intBar);
            var NimBar = this.PluginInterface.UiBuilder.LoadImage(nimBar);
            var SenBar = this.PluginInterface.UiBuilder.LoadImage(senBar);
            var StrBar = this.PluginInterface.UiBuilder.LoadImage(strBar);
            var NgBar = this.PluginInterface.UiBuilder.LoadImage(ngBar);
            var AvatarHolder = this.PluginInterface.UiBuilder.LoadImage(avatarHolder);
          




            string name = "";
            this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(new Rulebook(this));
            this.WindowSystem.AddWindow(new LoginWindow(this));
            this.WindowSystem.AddWindow(new InfiniteSheet(this,PlusEminence, MinusEminence,
                                                               PlusHardiness, MinusHardiness,
                                                               PlusIntelligence, MinusIntelligence,
                                                               PlusNimbleness, MinusNimbleness,
                                                               PlusSenses, MinusSenses,
                                                               PlusStrength, MinusStrength,
                                                               Health, Eminence, Hardiness, Intelligence, Nimbleness, Senses, Strength,
                                                               HpBar, EmnBar, HrdBar, IntBar, NimBar, SenBar, StrBar, NgBar,
                                                               AvatarHolder
                                                               ));
            //this.WindowSystem.AddWindow(new SystemsWindow(this));
            this.WindowSystem.AddWindow(new OptionsWindow(this, this.PluginInterface));
            this.WindowSystem.AddWindow(new MessageBox(this));
            this.WindowSystem.AddWindow(new AdminWindow(this, Health, Eminence, Hardiness, Intelligence, Nimbleness, Senses, Strength,
                                                               HpBar, EmnBar, HrdBar, IntBar, NimBar, SenBar, StrBar, NgBar,
                                                               AvatarHolder, this.PluginInterface));
            this.CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "to open the plugin window"
            });
            this.PluginInterface.UiBuilder.Draw += DrawUI;
            this.PluginInterface.UiBuilder.OpenConfigUi += DrawConfigUI;
            DataReceiver.plugin = this;
            ClientHandleData.InitializePackets(true);
            ClientTCP.InitializingNetworking(true);
            
            if (this.Configuration.username.Length > 0 && this.Configuration.password.Length > 0)
            {
                //DataSender.Login(this.Configuration.username, this.Configuration.password);
                
            }
        }

        public void Dispose()
        {
            this.WindowSystem.RemoveAllWindows();
            this.CommandManager.RemoveHandler(CommandName);

            ClientHandleData.InitializePackets(false);
            ClientTCP.InitializingNetworking(false);
        }



        private void OnCommand(string command, string args)
        {
            DrawConfigUI();
            // in response to the slash command, just display our main ui          
        }
       
        private void DrawUI()
        {
            this.WindowSystem.Draw();
        }


       

        public void DrawConfigUI()
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
        
        public void LoadNameTakenMessage()
        {
            LoadAlert("Banned Account", "This account has been banned by the admins.", new Vector4(1, 0, 0, 1), new Vector4(1, 1, 1, 1));
        }
        public void LoadBannedMessage()
        {
            LoadAlert("Banned Account", "This account has been banned by the admins.", new Vector4(1, 0, 0, 1), new Vector4(1, 1, 1, 1));
        }
        public void LoadAlert(string title, string description, Vector4 titleColor, Vector4 descColor)
        {
           
           

        }
        
    }
}
