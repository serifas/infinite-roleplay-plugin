using System;
using System.Numerics;
using Dalamud.Interface;
using System.Security.Cryptography;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using Dalamud.Interface.ImGuiFileDialog;
using UpdateTest;
using Windows.Devices.HumanInterfaceDevice;

namespace InfiniteRoleplay.Windows;

public class LoginWindow : Window, IDisposable
{
    private Configuration Configuration;
    public string username = string.Empty;
    public string password = string.Empty;
    public bool attemptedLogin = false;
    public bool updateWindow = false;
    public string status = "status...";
    public Vector4 statusColor = new Vector4(255,255,255,255);
    public Plugin plugin;
    public LoginWindow(Plugin plugin) : base(
        "LOGIN",
        ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar |
        ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.Size = new Vector2(232, 180);
        this.SizeCondition = ImGuiCond.Always;

        this.Configuration = plugin.Configuration;
        this.username = this.Configuration.username;
        this.password = this.Configuration.password;
    }

    public void Dispose() 
    {
        updateWindow = false;
    }

    public override void Draw()
    {
        updateWindow = true;
        // can't ref a property, so use a local copy
        var connectionValue = this.Configuration.StayOnline;
        var usernamevalue = this.Configuration.username;
        var passwordvalue = this.Configuration.password;
        ImGui.InputTextWithHint("##username", $"Username", ref this.username, 100);
        ImGui.InputTextWithHint("##password", $"Password", ref this.password, 100, ImGuiInputTextFlags.Password);

        if (ImGui.Button("Login"))
        {            
            this.Configuration.username = this.username;
            this.Configuration.password = this.password;
            this.Configuration.Save();           
            DataSender.Login(this.username, this.password);           
        }
        ImGui.TextColored(this.statusColor, this.status);
    }
    public static FileDialogManager SetupFileManager()
    {
        var fileManager = new FileDialogManager
        {
            AddedWindowFlags = ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking,
        };

        return fileManager;
    }
    public override void Update()
    {
        this.status = DataReceiver.accountStatus;
        this.statusColor = DataReceiver.accounStatusColor;
    }


}
