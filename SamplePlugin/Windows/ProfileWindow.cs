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
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System.Threading.Channels;

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
        public int availablePercentage = 50;
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
                    ImGui.Text("Name:   ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##playername", $"Character Name (The name of the character you are currently playing as)", ref characterAddName, 100);
                    //race input
                    ImGui.Text("Race:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##race", $"The IC Race of your character", ref characterAddRace, 100);
                    //gender input
                    ImGui.Text("Gender: ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##gender", $"Specifying an insult or terms such as trap or futa will get you banned. These are not genders.", ref characterAddGender, 100);
                    //age input
                    ImGui.Text("Age:    ");
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
                #region ALIGNMENTS
                    #region LAWFUL GOOD
                    //LAWFUL GOOD
                    ImGui.Image(this.lawfulGood.ImGuiHandle, new Vector2(32, 32));
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
                    int formattedLawfulGoodVal = lawfulGoodVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), lawfulGoodVal.ToString() );
                    #endregion

                    #region NEUTRAL GOOD
                    // NEUTRAL GOOD
                    ImGui.Image(this.neutralGood.ImGuiHandle, new Vector2(32, 32));
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
                    int formattedNeutralGoodVal = neutralGoodVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), neutralGoodVal.ToString());
                    #endregion

                    #region CHAOTIC GOOD
                    // CHAOTIC GOOD
                    ImGui.Image(this.chaoticGood.ImGuiHandle, new Vector2(32, 32));
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
                    int formattedChaoticGoodVal = chaoticGoodVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), chaoticGoodVal.ToString());
                    #endregion

                    #region LAWFUL NEUTRAL
                    // LAWFUL NEUTRAL
                    ImGui.Image(this.lawfulNeutral.ImGuiHandle, new Vector2(32, 32));
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
                    int formattedLawfulNeutralVal = lawfulNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), lawfulNeutralVal.ToString());
                    #endregion

                    #region TRUE NEUTRAL
                    // CHAOTIC GOOD
                    ImGui.Image(this.trueNeutral.ImGuiHandle, new Vector2(32, 32));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("TRUE NEUTRAL:\n" +
                                        "    True Neutral characters don’t like to take sides.\n" +
                                        "    They are pragmatic rather than emotional in their actions, \n" +
                                        "    choosing the response which makes the most sense for them in each situation. " +
                                        "\n" +
                                        "    Neutral characters don’t believe in upholding the rules and laws of society, but nor \n" +
                                        "    do they feel the need to rebel against them. There will be times when a Neutral character \n" +
                                        "    has to make a choice between siding with Good or Evil, perhaps casting the deciding vote \n" +
                                        "    in a party. They will make a choice in these situations, usually siding with whichever causes \n" +
                                        "    them the least hassle, or they stand to gain the most from. ");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.trueNeutralPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("trueneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.trueNeutralMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("trueneutral", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.trueNeutralBar.ImGuiHandle, new Vector2(trueNeutralWidth * 30, 20));
                    ImGui.SameLine();
                    int formattedTrueNeutralVal = trueNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), trueNeutralVal.ToString());
                    #endregion

                    #region CHAOTIC NEUTRAL
                    // CHAOTIC GOOD
                    ImGui.Image(this.chaoticNeutral.ImGuiHandle, new Vector2(32, 32));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("CHAOTIC NEUTRAL:\n" +
                                        "    Chaotic Neutral characters are free spirits. \n" +
                                        "    They do what they want but don’t seek to disrupt the usual norms and laws of society. \n" +
                                        "    These individuals don’t like being told what to do, following traditions, \n" +
                                        "    or being controlled. That said, they will not work to change these restrictions,\n" +
                                        "    instead, they will just try to avoid them in the first place.\n" +
                                        "    Their need to be free is the most important thing. ");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.chaoticNeutralPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("chaoticneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.chaoticNeutralMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("chaoticneutral", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.chaoticNeutralBar.ImGuiHandle, new Vector2(chaoticNeutralWidth * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticNeutralVal = chaoticNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), chaoticNeutralVal.ToString());
                    #endregion

                    #region LAWFUL EVIL
                    // CHAOTIC GOOD
                    ImGui.Image(this.lawfulEvil.ImGuiHandle, new Vector2(32, 32));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("LAWFUL EVIL:\n" +
                                        "    Lawful Evil characters operate within a strict code of laws and traditions.\n" +
                                        "    Upholding these values and living by these is more important than anything, \n" +
                                        "    even the lives of others. They may not consider themselves to be Evil, \n" +
                                        "    they may believe what they are doing is right. \n" +

                                        "    These characters enforce their system of control through force.\n" +
                                        "    Anyone who doesn’t follow their code or acts out of line will face consequences. \n" +
                                        "    Lawful Evil characters feel no guilt or remorse for causing harm to others in this way.");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.lawfulEvilPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulevil", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.lawfulEvilMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulevil", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.lawfulEvilBar.ImGuiHandle, new Vector2(lawfulEvilWidth * 30, 20));
                    ImGui.SameLine();
                    int formattedLawfulEvilVal = lawfulEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), lawfulEvilVal.ToString());
                    #endregion

                    #region NEUTRAL EVIL
                    // CHAOTIC GOOD
                    ImGui.Image(this.neutralEvil.ImGuiHandle, new Vector2(32, 32));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("NEUTRAL EVIL:\n" +
                                        "    Neutral Evil characters are selfish. Their actions are driven by their own wants \n" +
                                        "    whether that’s power, greed, attention, or something else. \n" +
                                        "    They will follow laws if they happen to align with their ambitions, but they will not \n" +
                                        "    hesitate to break them if they don’t.They don’t believe that following laws \n" +
                                        "    and traditions makes anyone a better person. \n" +
                                        "    Instead, they use other people’s beliefs in codes and loyalty against them, using it \n" +
                                        "    as a tool to influence their behaviour. ");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.neutralEvilPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("neutralevil", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.neutralEvilMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("neutralevil", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.neutralEvilBar.ImGuiHandle, new Vector2(neutralEvilWidth * 30, 20));
                    ImGui.SameLine();
                    int formattedNeutralEvilVal = neutralEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), neutralEvilVal.ToString());
                    #endregion

                    #region CHAOTIC EVIL
                    // CHAOTIC GOOD
                    ImGui.Image(this.chaoticEvil.ImGuiHandle, new Vector2(32, 32));
                    if (ImGui.IsItemHovered())
                    {
                        ImGui.SetTooltip("CHAOTIC EVIL:\n" +
                                        "    Chaotic Evil characters care only for themselves with a complete disregard \n" +
                                        "    for all law and order and for the welfare and freedom of others. \n" +
                                        "    They harm others out of anger or just for fun.\n" +
                                        "    Characters aligned with Chaotic Evil usually operate alone \n" +
                                        "    because they do not work well with others.");
                    }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.chaoticEvilPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("chaoticevil", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.chaoticEvilMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("chaoticevil", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.chaoticEvilBar.ImGuiHandle, new Vector2(chaoticEvilWidth * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticEvilVal = chaoticEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), chaoticEvilVal.ToString());
                    #endregion

                    #endregion
                    if (ImGui.Button("Save Bio"))
                    {

                    }
                }








            }
            if (addAvatar == true)
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
            if (trueNeutralWidth < trueNeutralWidthVal) { trueNeutralWidth += 0.1f; }
            if (trueNeutralWidth > trueNeutralWidthVal) { trueNeutralWidth -= 0.1f; }
            if (chaoticNeutralWidth < chaoticNeutralWidthVal) { chaoticNeutralWidth += 0.1f; }
            if (chaoticNeutralWidth > chaoticNeutralWidthVal) { chaoticNeutralWidth -= 0.1f; }
            if (lawfulEvilWidth < lawfulEvilWidthVal) { lawfulEvilWidth += 0.1f; }
            if (lawfulEvilWidth > lawfulEvilWidthVal) { lawfulEvilWidth -= 0.1f; }
            if (neutralEvilWidth < neutralEvilWidthVal) { neutralEvilWidth += 0.1f; }
            if (neutralEvilWidth > neutralEvilWidthVal) { neutralEvilWidth -= 0.1f; }
            if (chaoticEvilWidth < chaoticEvilWidthVal) { chaoticEvilWidth += 0.1f; }
            if (chaoticEvilWidth > chaoticEvilWidthVal) { chaoticEvilWidth -= 0.1f; }
        }
        public void ModAlignment(string alignmentName, bool add) 
        {
            
            if (alignmentName == "lawfulgood")
            {
                if(add){ if(availablePercentage > 0 && lawfulGoodVal < 10){ availablePercentage -= 1; lawfulGoodWidthVal += 1; lawfulGoodVal += 1;}}
                else{ if(lawfulGoodWidthVal > 0) { availablePercentage += 1; lawfulGoodWidthVal -= 1; lawfulGoodVal -= 1;}}
            }
            if (alignmentName == "neutralgood")
            {
                if (add) { if (availablePercentage > 0 && neutralGoodVal < 10) { availablePercentage -= 1; neutralGoodWidthVal += 1; neutralGoodVal += 1; } }
                else { if (neutralGoodWidthVal > 0) { availablePercentage += 1; neutralGoodWidthVal -= 1; neutralGoodVal -= 1; } }
            }
            if (alignmentName == "chaoticgood")
            {
                if (add) { if (availablePercentage > 0 && chaoticGoodVal < 10) { availablePercentage -= 1; chaoticGoodWidthVal += 1; chaoticGoodVal += 1; } }
                else { if (chaoticGoodWidthVal > 0) { availablePercentage += 1; chaoticGoodWidthVal -= 1; chaoticGoodVal -= 1; } }
            }
            if (alignmentName == "lawfulneutral")
            {
                if (add) { if (availablePercentage > 0 && lawfulNeutralVal < 10) { availablePercentage -= 1; lawfulNeutralWidthVal += 1; lawfulNeutralVal += 1; } }
                else { if (lawfulNeutralWidthVal > 0) { availablePercentage += 1; lawfulNeutralWidthVal -= 1; lawfulNeutralVal -= 1; } }
            }
            if (alignmentName == "trueneutral")
            {
                if (add) { if (availablePercentage > 0 && trueNeutralVal < 10) { availablePercentage -= 1; trueNeutralWidthVal += 1; trueNeutralVal += 1; } }
                else { if (trueNeutralWidthVal > 0) { availablePercentage += 1; trueNeutralWidthVal -= 1; trueNeutralVal -= 1; } }
            }
            if (alignmentName == "chaoticneutral")
            {
                if (add) { if (availablePercentage > 0 && chaoticNeutralVal < 10) { availablePercentage -= 1; chaoticNeutralWidthVal += 1; chaoticNeutralVal += 1; } }
                else { if (chaoticNeutralWidthVal > 0) { availablePercentage += 1; chaoticNeutralWidthVal -= 1; chaoticNeutralVal -= 1; } }
            }
            if (alignmentName == "lawfulevil")
            {
                if (add) { if (availablePercentage > 0 && lawfulEvilVal < 10) { availablePercentage -= 1; lawfulEvilWidthVal += 1; lawfulEvilVal += 1; } }
                else { if (lawfulEvilWidthVal > 0) { availablePercentage += 1; lawfulEvilWidthVal -= 1; lawfulEvilVal -= 1; } }
            }
            if (alignmentName == "neutralevil")
            {
                if (add) { if (availablePercentage > 0 && neutralEvilVal < 10) { availablePercentage -= 1; neutralEvilWidthVal += 1; neutralEvilVal += 1; } }
                else { if (neutralEvilWidthVal > 0) { availablePercentage += 1; neutralEvilWidthVal -= 1; neutralEvilVal -= 1; } }
            }
            if (alignmentName == "chaoticevil")
            {
                if (add) { if (availablePercentage > 0 && chaoticEvilVal < 10) { availablePercentage -= 1; chaoticEvilWidthVal += 1; chaoticEvilVal += 1; } }
                else { if (chaoticEvilWidthVal > 0) { availablePercentage += 1; chaoticEvilWidthVal -= 1; chaoticEvilVal -= 1; } }
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
