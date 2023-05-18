using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using ImGuiScene;
using System;
using System.IO;
using System.Numerics;
using ImGuiFileDialog;
using System.Collections.Generic;
using Dalamud.Game.ClientState.Objects.Types;
using Windows.UI.Xaml.Controls;
using Dalamud.Interface;
using Windows.ApplicationModel.UserDataTasks;
using Windows.Gaming.Input;
using Dalamud.Plugin;
using Windows.UI;
using OtterGui.Raii;
using OtterGui;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.Style;
using OtterGui.Widgets;
using Windows.Media.Devices;
using InfiniteRoleplay.UI.Classes;
using Dalamud.Interface.Colors;
using System.Linq;
using Dalamud;
using Lumina.Excel.GeneratedSheets;
using static Uno.UI.FeatureConfiguration;
using Dalamud.IoC;
using Windows.UI.Xaml.Shapes;
using Path = System.IO.Path;
using static System.Net.Mime.MediaTypeNames;
using Lumina.Data.Files;
using FileDialog = ImGuiFileDialog.FileDialog;
using System.Runtime.CompilerServices;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using FileDialogManager = Dalamud.Interface.ImGuiFileDialog.FileDialogManager;
using InfiniteRoleplay;
using System.Threading.Tasks;
using System.Diagnostics;
using OtterGui.Filesystem;
using System.Net.Http.Headers;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using Microsoft.UI.Xaml.Controls;
using UpdateTest;

namespace InfiniteRP.Windows;

public class Rulebook : Window, IDisposable
{
    public string message;
    private Plugin Plugin;
    private int pageID = 0;

    private DalamudPluginInterface pg;
    private string TitleVal = string.Empty;
    private string TitleCreateMain = string.Empty;
    private string[] TitleCreate = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
    private string[] ContentCreate = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
    private string[] ContentTitles = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
    private string[] ContentValues = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
    private int key = 0;
    private bool creation = false;
    private float _modVersionWidth;
    private GameFontHandle _nameFont;
    private int sectionCount;
    private GameFontHandle _infoFont;
    private int sheetID;
    private byte[] avatarBytesData;
    public SortedList<int, string> PageIndexes = new SortedList<int, string>();
    public SortedList<string, string> PageContentIndexes = new SortedList<string, string>();
    public Dictionary<string, string> PageSections = new Dictionary<string, string>();
    public string[] titles;
    public string[] contents;
    private int availableReductions;
    public Rulebook(Plugin plugin) : base(
        "SHINE RULEBOOK", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
    {
        this.SizeConstraints = new WindowSizeConstraints
        {
            MinimumSize = new Vector2(750, 950),
            MaximumSize = new Vector2(750, 950)
        };

        this.pg = plugin.PluginInterfacePub;
        this._nameFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
        this._infoFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter16));



      
        this.Plugin = plugin;
        

      //  var penImage = Path.Combine(pg.AssemblyLocation.Directory?.FullName!, "editbtn.png");
        //this.penIcon = pg.UiBuilder.LoadImage(penImage);

    }









    public void Dispose()
    {
        
    }



    public override void Draw()
    {
        Vector4 color = new Vector4(0, 0, 0, 0);
        string connectionLabel = "Disconnected";

        if (ImGui.BeginChild("CONNECTION", new Vector2(100, 45), false))
        {
           
            ImGui.TextColored(color, connectionLabel);

            if (ImGui.Button("Show Settings"))
            {
                this.Plugin.DrawConfigUI();
            }

        }
        ImGui.EndChild();

        if (ImGui.BeginChild("RULEBOOK INDEX", new Vector2(200, 850), true))
        {
            if (ImGui.Button("Add Page", new Vector2(100, 20)))
            {
                creation = true;
            }
            //CHARACTER SELECTION
            foreach(string pageTitle in PageIndexes.Values)
            {                   
                //senses + "," + strength + "," + hardiness + "," + intelligence + "," + nimbleness + "," + eminence + "," + ability_1 + "," + ability_2 + "," + ability_3 + "," + ability_1_description + "," + ability_2_description + "," + ability_3_description;
                

                if (ImGui.Selectable(pageTitle))
                {
                    creation = false;
                    TitleVal = pageTitle;
                    for (int i = 0; i < PageContentIndexes.Count; i++)
                    {

                        int PageID = PageIndexes.Keys[i];
                        string ContentPageID = PageContentIndexes.Keys[i].Split(",")[1];
                        ContentTitles[i] = PageID.ToString();
                        ContentValues[i] = ContentPageID;
                        
                    }
                   
                }
            }
        }
        
        ImGui.EndChild();

        ImGui.SameLine();
        if (creation == true)
        {

            if (ImGui.BeginChild("PAGE CREATION", new Vector2(500, 850), true))
            {
                ImGui.SameLine();

                ImGui.Spacing();
                ImGui.InputTextWithHint("##pagetitle", $"Title", ref TitleCreateMain, 100);
                if (ImGui.Button("Add Section"))
                {
                    sectionCount ++;
                }

                for(int i = 0; i < sectionCount; i++)
                {
                    int index = i + 1;
                    ImGui.InputTextWithHint("##pagename" + i, $"Title " + index, ref TitleCreate[i], 100);
                    ImGui.InputTextMultiline("##pageconent" + i, ref ContentCreate[i], 3000, new Vector2(450, 300));
                }



                ImGui.Separator();
                if(ImGui.Button("Create Page"))
                {
                   // DataSender.SendRulebookPage(Plugin.Configuration.username, TitleCreateMain.Replace(",", ":::"));
                    for(int i = 0; i < sectionCount; i++)
                    {
                   //     DataSender.SendRulebookPageContent(Plugin.Configuration.username, TitleCreate[i].Replace(",", ":::"), ContentCreate[i].Replace(",", ":::"));
                    }
                   
                }
            }
            ImGui.EndChild();
        }
        else
        {
            if (ImGui.BeginChild("RULEBOOK", new Vector2(2000, 3000), true))
            {

                using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
                using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                ImGuiUtil.DrawTextButton(TitleVal, Vector2.Zero, 0);
                
                ImGui.Separator();
                

                for(int i = 0; i < ContentTitles.Length; i++)
                {
                    var title = ContentTitles[i];
                    var content = ContentValues[i];
                    ImGui.Text(title.Replace(":::", ", "));
                    using var defInfFontDen = ImRaii.DefaultFont();
                    ImGui.Text(content.Replace(":::", ", "));
                }

               





               

            }

            ImGui.EndChild();

        }


    }
    public override void Update()
    {
        PageIndexes = DataReceiver.pages;
        PageContentIndexes = DataReceiver.pagesContent;
    }
    public byte[] ImageToByteArray(System.Drawing.Image img)
    {
        MemoryStream ms = new MemoryStream();
        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
        return ms.ToArray();
    }
    public string[] subCategories(string val)
    {
        string[] arr = val.Split(new char[] { '*', '*' }, StringSplitOptions.None);
        return arr;
    }
}

