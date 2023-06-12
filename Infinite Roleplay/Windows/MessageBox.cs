using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using Dalamud.Interface;
using Dalamud.Interface.Windowing;
using Dalamud.Utility;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using ImGuiNET;
using ImGuiScene;

namespace InfiniteRoleplay.Windows;

public class MessageBox : Window, IDisposable
{
    public static int SELECTED_SHEET_ID;
    public string title;
    public static string TITLE;
    public string description;
    public static string DESCRIPTION;
    public Plugin Plugin;
    public Vector4 titleColor;
    public static Vector4 TITLE_COLOR;
    public Vector4 descColor;
    public static Vector4 DESC_COLOR;
    public messageType messageTyp;
    public static messageType MESSAGE_TYPE;
    public static string MESSAGE_CONTENT = "";
    public string messageContent = "";
    public enum messageType { 
        SHEET_VERIFY = 1,
        SHEET_REVISION_REQUEST = 2,
        SHEET_DECLINE = 3,
        SHEET_CREATION_SUCCESS = 4,
        SHEET_CREATION_FAILURE = 5,
    }; 



    public MessageBox(Plugin plugin) : base(
        "Message" , ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(375, 100),
            MaximumSize = new Vector2(float.MaxValue, float.MaxValue)
        };

        
        Plugin = plugin;
    }
    public void Dispose()
    {

    }

    public override void Draw()
    {
        ImGui.Spacing();
        ImGui.TextColored(titleColor, title);
        ImGui.Spacing();
        ImGui.TextColored(descColor, description);
        
        if (ImGui.Button("Confirm", new Vector2(225, 25)))
        {
            if (this.messageTyp == messageType.SHEET_VERIFY) 
            {
               // DataSender.UpdateSheetStatus(SELECTED_SHEET_ID, 1);
                this.IsOpen =false;
            }
            if(this.messageTyp == messageType.SHEET_DECLINE)
            {
              //  DataSender.UpdateSheetStatus(SELECTED_SHEET_ID, -2);
                this.IsOpen = false;
            }
            if(this.messageTyp == messageType.SHEET_REVISION_REQUEST) 
            {
              //  DataSender.UpdateSheetStatus(SELECTED_SHEET_ID, -1);
                this.IsOpen = false;
            }
        }

    }
    public override void Update()
    {
        this.title = TITLE;
        this.description = DESCRIPTION;
        this.titleColor = TITLE_COLOR;
        this.descColor = DESC_COLOR;
        this.messageTyp = MESSAGE_TYPE;
        this.messageContent = MESSAGE_CONTENT;
    }
}
