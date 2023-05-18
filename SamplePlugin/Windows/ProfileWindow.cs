using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ImGuiNET;
using ImGuiScene;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using UpdateTest;
using static Dalamud.Interface.Windowing.Window;
using Dalamud.Interface;
using Dalamud.Interface.Colors;
using Dalamud.Interface.GameFonts;
using Dalamud.Interface.ImGuiFileDialog;
using ImGuiNET;
using ImGuiScene;
using static Lumina.Data.Files.ScdFile;
using Lumina.Excel.GeneratedSheets;
using FFXIVClientStructs.FFXIV.Client.Graphics.Render;
using System.Collections.Concurrent;
using Dalamud.Utility;
using System.Reflection;
using SixLabors.ImageSharp;
using Penumbra.UI;
using System.Security.Policy;
using OtterGui.Raii;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using Windows.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Controls;
using static Uno.CompositionConfiguration;
using System.Windows.Markup;

namespace InfiniteRoleplay.Windows
{
    internal class ProfileWindow : Window, IDisposable
    {
        private readonly ConcurrentDictionary<string, string> _startPaths = new();
        private Plugin plugin;

        private DalamudPluginInterface pg;
        private FileDialogManager _fileDialogManager;
        private FileDialogService _fileDialogService;
#pragma warning disable CS0169 // The field 'ProfileWindow.profilesImage' is never used
        private TextureWrap profilesImage;
#pragma warning restore CS0169 // The field 'ProfileWindow.profilesImage' is never used
        public Configuration configuration;
        public static bool WindowOpen;
        public static bool addBio = false;
        public static bool addHooks = false;
        public static bool addStory = false;
        public static bool addOOC = false;
        public static bool addGallery = false;
        public static bool addAvatar = false;
        public static bool addProfile = false;
        public byte[] avatarBytes;
        public int availablePercentage = 100;
        //Font Vars
        private GameFontHandle _Font;
        //BIO VARS
        private TextureWrap avatarImg;
        public static string characterAddName = "";
        public static string characterAddRace = "";
        public static string characterAddGender = "";
        public static string characterAddAge = "";
        public static string characterAddAfg = "";
        public static string characterAddHeight = "";
        public static string characterAddWeight = "";
        public static string fileName = "";
        private readonly FileDialogManager _manager;
        private bool _isOpen;
        private bool _showFileDialogError = false;
        private TextureWrap lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil;
        private float lawfulGoodWidth = 0, neutralGoodWidth = 0, chaoticGoodWidth = 0, lawfulNeutralWidth = 0, trueNeutralWidth = 0, chaoticNeutralWidth = 0, lawfulEvilWidth = 0, neutralEvilWidth = 0, chaoticEvilWidth = 0;
        private int lawfulGoodWidthVal = 0, neutralGoodWidthVal = 0, chaoticGoodWidthVal = 0, lawfulNeutralWidthVal = 0, trueNeutralWidthVal = 0, chaoticNeutralWidthVal = 0, lawfulEvilWidthVal = 0, neutralEvilWidthVal = 0, chaoticEvilWidthVal = 0;
        private int lawfulGoodVal = 0, neutralGoodVal = 0, chaoticGoodVal = 0, lawfulNeutralVal = 0, trueNeutralVal = 0, chaoticNeutralVal = 0, lawfulEvilVal = 0, neutralEvilVal = 0, chaoticEvilVal = 0;
        private TextureWrap lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar;
        private TextureWrap lawfulGoodPlus, neutralGoodPlus, chaoticGoodPlus, lawfulNeutralPlus, trueNeutralPlus, chaoticNeutralPlus, lawfulEvilPlus, neutralEvilPlus, chaoticEvilPlus;
        private TextureWrap lawfulGoodMinus, neutralGoodMinus, chaoticGoodMinus, lawfulNeutralMinus, trueNeutralMinus, chaoticNeutralMinus, lawfulEvilMinus, neutralEvilMinus, chaoticEvilMinus;
        
        
        
        public ProfileWindow(Plugin plugin, DalamudPluginInterface Interface, TextureWrap avatarHolder,
                             //alignment icon
                             TextureWrap lawfulgood, TextureWrap neutralgood, TextureWrap chaoticgood,
                             TextureWrap lawfulneutral, TextureWrap trueneutral, TextureWrap chaoticneutral,
                             TextureWrap lawfulevil, TextureWrap neutralevil, TextureWrap chaoticevil,

                             //bars

                             TextureWrap lawfulgoodBar, TextureWrap neutralgoodBar, TextureWrap chaoticgoodBar,
                             TextureWrap lawfulneutralBar, TextureWrap trueneutralBar, TextureWrap chaoticneutralBar,
                             TextureWrap lawfulevilBar, TextureWrap neutralevilBar, TextureWrap chaoticevilBar,

                             //add plus buttons

                             TextureWrap lawfulgoodPlus, TextureWrap neutralgoodPlus, TextureWrap chaoticgoodPlus,
                             TextureWrap lawfulneutralPlus, TextureWrap trueneutralPlus, TextureWrap chaoticneutralPlus,
                             TextureWrap lawfulevilPlus, TextureWrap neutralevilPlus, TextureWrap chaoticevilPlus,
                             TextureWrap lawfulgoodMinus, TextureWrap neutralgoodMinus, TextureWrap chaoticgoodMinus,
                             TextureWrap lawfulneutralMinus, TextureWrap trueneutralMinus, TextureWrap chaoticneutralMinus,
                             TextureWrap lawfulevilMinus, TextureWrap neutralevilMinus, TextureWrap chaoticevilMinus
                            ) : base(
       "PROFILES", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(750, 950),
                MaximumSize = new Vector2(750, 950)
            };
            this.plugin = plugin;
            this.pg = plugin.PluginInterfacePub;
            this.configuration = plugin.Configuration;
            this._fileDialogManager = new FileDialogManager();
            this.avatarImg = avatarHolder;
            //alignment icons
            this.lawfulGood = lawfulgood; this.neutralGood = neutralgood; this.chaoticGood = chaoticgood;
            this.lawfulNeutral = lawfulneutral; this.trueNeutral = trueneutral; this.chaoticNeutral = chaoticneutral;
            this.lawfulEvil = lawfulevil; this.neutralEvil = neutralevil; this.chaoticEvil = chaoticevil;

            //bars
            this.lawfulGoodBar = lawfulgoodBar; this.neutralGoodBar = neutralgoodBar; this.chaoticGoodBar = chaoticgoodBar;
            this.lawfulNeutralBar = lawfulneutralBar; this.trueNeutralBar = trueneutralBar; this.chaoticNeutralBar = chaoticneutralBar;
            this.lawfulEvilBar = lawfulevilBar; this.neutralEvilBar = neutralevilBar; this.chaoticEvilBar = chaoticevilBar;
            this._Font = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            //plus and minus
            this.lawfulGoodPlus = lawfulgoodPlus; this.neutralGoodPlus = neutralgoodPlus; this.chaoticGoodPlus = chaoticgoodPlus;
            this.lawfulNeutralPlus = lawfulneutralPlus; this.trueNeutralPlus = trueneutralPlus; this.chaoticNeutralPlus = chaoticneutralPlus;
            this.lawfulEvilPlus = lawfulevilPlus; this.neutralEvilPlus = neutralevilPlus; this.chaoticEvilPlus = chaoticevilPlus;
            this.lawfulGoodMinus = lawfulgoodMinus; this.neutralGoodMinus = neutralgoodMinus; this.chaoticGoodMinus = chaoticgoodMinus;
            this.lawfulNeutralMinus = lawfulneutralMinus; this.trueNeutralMinus = trueneutralMinus; this.chaoticNeutralMinus = chaoticneutralMinus;
            this.lawfulEvilMinus = lawfulevilMinus; this.neutralEvilMinus = neutralevilMinus; this.chaoticEvilMinus = chaoticevilMinus;

        }
        public override void Draw()
        {
            _fileDialogManager.Draw();
            //LoadFileSelection();
            if (ImGui.Button("Add Profile", new Vector2(100,20))) { addProfile = true; }
           
            if (addProfile == true)
            {
                ImGui.Spacing();
                if (ImGui.Button("Add Bio", new Vector2(100, 20))) { addBio = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a bio section to your profile"); }
                ImGui.SameLine();
                if (ImGui.Button("Add Hooks", new Vector2(100, 20))) { addHooks = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a hooks section to your profile"); }
                ImGui.SameLine();
                if (ImGui.Button("Add Story", new Vector2(100, 20))) { addStory = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a story section to your profile"); }
                ImGui.SameLine();
                if (ImGui.Button("Add OOC Info", new Vector2(100, 20))) { addOOC = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add an OOC section to your profile"); }
                ImGui.SameLine();
                if (ImGui.Button("Add Gallery", new Vector2(100, 20))) { addGallery = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a gallery section to your profile"); }

            }
            

            if (ImGui.BeginChild("PROFILE"))
            {
                if(addBio== true)
                {
                    ImGui.Image(this.avatarImg.ImGuiHandle, new Vector2(100, 100));

                    if (ImGui.Button("Add Avatar"))
                    {
                        addAvatar = true;
                    }
                    ImGui.Spacing();
                    //name input
                    ImGui.Text("Name:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##playername", $"Character Name (The name of the character you are currently playing as)", ref characterAddName, 100);
                    //race input
                    ImGui.Text("Race:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##race", $"The IC Race of your character", ref characterAddRace, 100);
                    //gender input
                    ImGui.Text("Gender:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##gender", $"Specifying an insult or terms such as trap or futa will get you banned. These are not genders.", ref characterAddGender, 100);
                    //age input
                    ImGui.Text("Age:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##age", $"If your character's age is not 18+ you will be banned for making your profile nsfw", ref characterAddAge, 100);
                    //age input
                    ImGui.Text("Height:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##height", $"Height in Fulms", ref characterAddHeight, 100);
                    //age input
                    ImGui.Text("Weight:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##weight", $"Weight in Ponze", ref characterAddWeight, 100);
                    //at first glance input
                    ImGui.Text("At First Glance:");
                    ImGui.SameLine();
                    ImGui.InputTextMultiline("##afg", ref characterAddAfg, 500, new Vector2(400, 100));

                    ImGui.TextColored(new Vector4(1, 1, 0, 1), "ALIGNMENT:");
                    ImGui.SameLine();
                    ImGui.Text(this.availablePercentage + " Points Available:");

                    //LAWFUL GOOD
                    ImGui.Image(this.lawfulGood.ImGuiHandle, new Vector2(50, 50));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("LAWFUL GOOD:\n" +
                                        "    These characters always do the right thing as expected by society.\n" +
                                        "    They always follow the rules, tell the truth and help people out.\n" +
                                        "    They like order, trust and believe in people with social authority, \n" +
                                        "    and they aim to be an upstanding citizen.");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.lawfulGoodPlus.ImGuiHandle, new Vector2(20, 20))){ ModAlignment("lawfulgood", true);}
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.lawfulGoodMinus.ImGuiHandle, new Vector2(20, 20))){ ModAlignment("lawfulgood", false);}
                    ImGui.SameLine();
                    ImGui.Image(this.lawfulGoodBar.ImGuiHandle, new Vector2(lawfulGoodWidth * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), lawfulGoodVal.ToString() );

                    // NEUTRAL GOOD
                    ImGui.Image(this.neutralGood.ImGuiHandle, new Vector2(50, 50));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("NEUTRAL GOOD:\n" +
                                        "    These characters do their best to help others, \n" +
                                        "    but they do it because they want to, not because they have \n" +
                                        "    been told to by a person in authority or by society’s laws.\n" +
                                        "    A Neutral Good person will break the rules if they are doing it \n" +
                                        "    for good reasons and they will feel confident \n" +
                                        "    and justified in their actions.  ");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.neutralGoodPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("neutralgood", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.neutralGoodMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("neutralgood", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.neutralGoodBar.ImGuiHandle, new Vector2(neutralGoodWidth * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), neutralGoodVal.ToString());

                    // CHAOTIC GOOD
                    ImGui.Image(this.chaoticGood.ImGuiHandle, new Vector2(50, 50));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("CHAOTIC GOOD:\n" +
                                        "    Chaotic Good characters do what their conscience tells \n" +
                                        "    them to for the greater good. They do not care about following society’s rules, \n" +
                                        "    they care about doing what’s right. \n" +
                                        "\n" +
                                        "    A Chaotic Good character will speak up for and help, those who are being needlessly \n" +
                                        "    held back because of arbitrary rules and laws. They do not like seeing people \n" +
                                        "    being told what to do for nonsensical reasons. ");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.chaoticGoodPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("chaoticgood", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.chaoticGoodMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("chaoticgood", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.chaoticGoodBar.ImGuiHandle, new Vector2(chaoticGoodWidth * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), chaoticGoodVal.ToString());

                    // LAWFUL NEUTRAL
                    ImGui.Image(this.lawfulNeutral.ImGuiHandle, new Vector2(50, 50));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("LAWFUL NEUTRAL:\n" +
                                        "    A Lawful Neutral character behaves in a way that matches \n" +
                                        "    the organization, authority or tradition they follow. \n" +
                                        "    They live by this code and uphold it above all else, taking actions \n" +
                                        "    that are sometimes considered Good and sometimes considered Evil by others.\n" +
                                        "    The Lawful Neutral character does not care about what others think of \n" +
                                        "    their actions, they only care about their actions being correct according \n" +
                                        "    to their code.But they do not preach their code to others and try to convert them. ");
                    }
                    ImGui.SameLine();
                        if (ImGui.ImageButton(this.lawfulNeutralPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulneutral", true); }
                        ImGui.SameLine();
                        if (ImGui.ImageButton(this.lawfulNeutralMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulneutral", false); }
                        ImGui.SameLine();
                        ImGui.Image(this.lawfulNeutralBar.ImGuiHandle, new Vector2(lawfulNeutralWidth * 30, 20));
                        ImGui.SameLine();
                        ImGui.TextColored(new Vector4(1, 1, 1, 1), lawfulNeutralVal.ToString());
                    }
                }
                ImGui.SameLine();
                if(addAvatar == true)
                {
                    addAvatar = false;
                AddAvatar();
            }

        }
        public void Dispose()
        {
            WindowOpen = false;
        }
        public override void Update()
        {
            if (lawfulGoodWidth < lawfulGoodWidthVal) { lawfulGoodWidth += 0.1f; }
            if (lawfulGoodWidth > lawfulGoodWidthVal) { lawfulGoodWidth -= 0.1f; }
            if (neutralGoodWidth < neutralGoodWidthVal) { neutralGoodWidth += 0.1f; }
            if (neutralGoodWidth > neutralGoodWidthVal) { neutralGoodWidth -= 0.1f; }
            if (chaoticGoodWidth < chaoticGoodWidthVal) { chaoticGoodWidth += 0.1f; }
            if (chaoticGoodWidth > chaoticGoodWidthVal) { chaoticGoodWidth -= 0.1f; }
            if (lawfulNeutralWidth < lawfulNeutralWidthVal) { lawfulNeutralWidth += 0.1f; }
            if (lawfulNeutralWidth > lawfulNeutralWidthVal) { lawfulNeutralWidth -= 0.1f; }
        }
        public void ModAlignment(string alignmentName, bool add) 
        {
            
            if (alignmentName == "lawfulgood")
            {
                if(add){ if(availablePercentage > 0){ availablePercentage -= 10; lawfulGoodWidthVal += 1; lawfulGoodVal += 10;}}
                else{ if(lawfulGoodWidthVal > 0) { availablePercentage += 10; lawfulGoodWidthVal -= 1; lawfulGoodVal -= 10;}}
            }
            if (alignmentName == "neutralgood")
            {
                if (add) { if (availablePercentage > 0) { availablePercentage -= 10; neutralGoodWidthVal += 1; neutralGoodVal += 10; } }
                else { if (neutralGoodWidthVal > 0) { availablePercentage += 10; neutralGoodWidthVal -= 1; neutralGoodVal -= 10; } }
            }
            if (alignmentName == "chaoticgood")
            {
                if (add) { if (availablePercentage > 0) { availablePercentage -= 10; chaoticGoodWidthVal += 1; chaoticGoodVal += 10; } }
                else { if (chaoticGoodWidthVal > 0) { availablePercentage += 10; chaoticGoodWidthVal -= 1; chaoticGoodVal -= 10; } }
            }
            if (alignmentName == "lawfulneutral")
            {
                if (add) { if (availablePercentage > 0) { availablePercentage -= 10; lawfulNeutralWidthVal += 1; lawfulNeutralVal += 10; } }
                else { if (chaoticGoodWidthVal > 0) { availablePercentage += 10; lawfulNeutralWidthVal -= 1; lawfulNeutralVal -= 10; } }
            }

        }
        public void AddAvatar()
        {
            _fileDialogManager.OpenFileDialog("Select Avatar", "Image{.png,.jpg}", (s, f) =>
            {
                if (!s)
                    return;
                    string AvatarPath = f[0].ToString();
                    var avatarImage = Path.GetFullPath(AvatarPath);

                    this.avatarImg = this.plugin.PluginInterfacePub.UiBuilder.LoadImage(avatarImage);

                    this.avatarBytes = File.ReadAllBytes(AvatarPath);

            }, 0, null, this.configuration.AlwaysOpenDefaultImport);
        }
     
    }
}
