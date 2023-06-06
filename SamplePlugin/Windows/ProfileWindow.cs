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
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Dalamud.Game.ClientState.Objects.SubKinds;
using System.Drawing;
using Dalamud.Game;
using Dalamud.Game.Gui;
using Image = SixLabors.ImageSharp.Image;
using System.Xml.Linq;
using Dalamud.Game.ClientState.Objects;

namespace InfiniteRoleplay.Windows
{
    internal class ProfileWindow : Window, IDisposable
    {
        private readonly ConcurrentDictionary<string, string> _startPaths = new();
        private Plugin plugin;
        public static bool loadedSelf = false;
        public static PlayerCharacter playerCharacter;
        private ChatGui chatGui;
        private DalamudPluginInterface pg;
        private FileDialogManager _fileDialogManager;
#pragma warning disable CS0169 // The field 'ProfileWindow.profilesImage' is never used
        private TextureWrap profilesImage;

#pragma warning restore CS0169 // The field 'ProfileWindow.profilesImage' is never used
        public Configuration configuration;
        public static bool WindowOpen;
        public static bool addBio = false;
        public static bool editBio = false;
        public static bool addHooks = false;
        public static bool editHooks = false;
        public static bool addStory = false;
        public static bool resetHooks;
        public static string[] HookContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] HookEditContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };

        public static bool editStory = false;
        public static bool addOOC = false;
        public static bool editOOC = false;
        public static bool addGallery = false;
        public static bool editGallery = false;
        public static bool addAvatar = false;
        public static bool editAvatar = false;
        public static bool addProfile = false;
        public static bool editProfile = false;
        public bool ExistingBio;
        public static bool ExistingHooks;
        public static int hookCount = 0;
        public static int hookEditCount;
        public static string[] hooks;
        public bool ExistingStory;
        public bool ExistingOOC;
        public bool ExistingGallery;
        public bool ExistingProfile;
        public static int lawfulGoodEditVal,
                          neutralGoodEditVal,
                          chaoticGoodEditVal,
                          lawfulNeutralEditVal,
                          trueNeutralEditVal,
                          chaoticNeutralEditVal,
                          lawfulEvilEditVal,
                          neutralEvilEditVal,
                          chaoticEvilEditVal;
        public static int[] alignmentVals, alignmentEditVals = new int[] { };
        private float[] alignmentWidthVals = new float[] { };
        private string[] alignmentNames = new string[]{};
        public byte[] avatarBytes, existingAvatarBytes;
        public int availablePercentage = 50;
        public int[] flaggedHookIndexes = new int[] { };
        //Font Vars
        private GameFontHandle _Font;
        //BIO VARS
        private TextureWrap avatarImg, currentAvatarImg;
        public static string characterAddName = "",
                                characterAddRace = "",
                                characterAddGender = "",
                                characterAddAge = "",
                                characterAddAfg = "",
                                characterAddHeight = "",
                                characterAddWeight = "",
                                characterEditName = "",
                                characterEditRace = "",
                                characterEditGender = "",
                                characterEditAge = "",
                                characterEditAfg = "",
                                characterEditHeight = "",
                                characterEditWeight = "";
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
        private int currentLawfulGood, currentNeutralGood, currentChaoticGood, currentLawfulNeutral, currentTrueNeutral, currentChaoticNeutral, currentLawfulEvil, currentNeutralEvil, currentChaoticEvil;



        public ProfileWindow(Plugin plugin, ChatGui chatGui, DalamudPluginInterface Interface, TextureWrap avatarHolder,
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
            
            System.Drawing.Image image1 = System.Drawing.Image.FromFile(Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "profile_avis/avatar_holder.png"));
            this.avatarBytes = ImageToByteArray(image1);
            //alignment icons
            this.lawfulGood = lawfulgood; this.neutralGood = neutralgood; this.chaoticGood = chaoticgood;
            this.lawfulNeutral = lawfulneutral; this.trueNeutral = trueneutral; this.chaoticNeutral = chaoticneutral;
            this.lawfulEvil = lawfulevil; this.neutralEvil = neutralevil; this.chaoticEvil = chaoticevil;
            this.chatGui = chatGui;
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

            alignmentVals = new int[9] { lawfulGoodVal, neutralGoodVal, chaoticGoodVal, lawfulNeutralVal, trueNeutralVal, chaoticNeutralVal, lawfulEvilVal, neutralEvilVal, chaoticEvilVal };
            alignmentEditVals = new int[9] { lawfulGoodEditVal, neutralGoodEditVal, chaoticGoodEditVal, lawfulNeutralEditVal, trueNeutralEditVal, chaoticNeutralEditVal, lawfulEvilEditVal, neutralEvilEditVal, chaoticEvilEditVal };
            this.alignmentWidthVals = new float[9] { lawfulGoodWidthVal, neutralGoodWidthVal, chaoticGoodWidthVal, lawfulNeutralWidthVal, trueNeutralWidthVal, chaoticNeutralWidthVal, lawfulEvilWidthVal, neutralEvilWidthVal, chaoticEvilWidthVal };
            this.alignmentNames = new string[9] { "lawfulgood", "neutralgood", "chaoticgood", "lawfulneutral", "trueneutral", "chaoticneutral", "lawfulevil", "neutralevil", "chaoticevil" };
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public override void Draw()
        {
            _fileDialogManager.Draw();
            //LoadFileSelection();

            //Vector2 addProfileBtnScale = new Vector2(playerCharacter.Name.ToString().Length * 20, 20);
            if (this.ExistingProfile == true)
            {
                if (ImGui.Button("Edit Profile", new Vector2(100, 20))) { editProfile = true; }
            }
            if (this.ExistingProfile == false)
            {
                if (ImGui.Button("Add Profile", new Vector2(100, 20))) { addProfile = true; DataSender.CreateProfile(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString()); }
            }

            if (addProfile == true)
            {
                editProfile = false;
                ImGui.Spacing();
                if (ImGui.Button("Add Bio", new Vector2(100, 20))) { ClearUI(); addBio = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a bio section to your profile."); }
                ImGui.SameLine();
                if (ImGui.Button("Add Hooks", new Vector2(100, 20))) { ClearUI(); addHooks = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a hooks section to your profile."); }
                ImGui.SameLine();
                if (ImGui.Button("Add Story", new Vector2(100, 20))) { ClearUI(); addStory = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a story section to your profile."); }
                ImGui.SameLine();
                if (ImGui.Button("Add OOC Info", new Vector2(100, 20))) { ClearUI(); addOOC = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add an OOC section to your profile."); }
                ImGui.SameLine();
                if (ImGui.Button("Add Gallery", new Vector2(100, 20))) { ClearUI(); addGallery = true; }
                if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Add a gallery section to your profile."); }

            }
            if (editProfile == true)
            {
                addProfile = false;
                
                ImGui.Spacing();
                if (ExistingBio == true) { if (ImGui.Button("Edit Bio", new Vector2(100, 20))) { ClearUI();  editBio = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your bio."); } } else {  if (ImGui.Button("Add Bio", new Vector2(100, 20))) { ClearUI(); addBio = true; } }
                ImGui.SameLine();
                if (ExistingHooks == true) { if (ImGui.Button("Edit Hooks", new Vector2(100, 20))) { ClearUI(); editHooks = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Hooks."); } } else { if (ImGui.Button("Add Hooks", new Vector2(100, 20))) { ClearUI(); addHooks = true; } }
                ImGui.SameLine();
                if (ExistingStory == true) { if (ImGui.Button("Edit Story", new Vector2(100, 20))) { ClearUI(); editStory = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Story."); } } else { if (ImGui.Button("Add Story", new Vector2(100, 20))) { ClearUI(); addStory = true; } }
                ImGui.SameLine();
                if (ExistingOOC == true) { if (ImGui.Button("Edit OOC Info", new Vector2(100, 20))) { ClearUI(); editOOC = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your OOC Info."); } } else { if (ImGui.Button("Add OOC Info", new Vector2(100, 20))) { ClearUI(); addOOC = true; } }
                ImGui.SameLine();
                if (ExistingGallery == true) { if (ImGui.Button("Edit Gallery", new Vector2(100, 20))) { ClearUI(); editGallery = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Gallery."); } } else { if (ImGui.Button("Add Gallery", new Vector2(100, 20))) { ClearUI(); addGallery = true; } }

            }
            bool warning = false;
            bool success = false;
            int currentAvailablePointsLeft = AvailablePercentageLeft(currentLawfulGood, currentNeutralGood, currentChaoticGood, currentLawfulNeutral, currentTrueNeutral, currentChaoticNeutral, currentLawfulEvil, currentNeutralEvil, currentChaoticEvil);
            if (ImGui.BeginChild("PROFILE"))
            {
                if (addBio == true)
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
                    ImGui.InputTextWithHint("##playername", $"Character Name (The name or nickname of the character you are currently playing as)", ref characterAddName, 100);
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
                    ImGui.InputTextWithHint("##age", $"If your character's age is not 18+ you may not make your profile nsfw (numbers only)", ref characterAddAge, 100, ImGuiInputTextFlags.CharsHexadecimal);
                    //age input
                    ImGui.Text("Height:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##height", $"Height in Fulms (numbers only)", ref characterAddHeight, 100);
                    //age input
                    ImGui.Text("Weight:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##weight", $"Weight in Ponze (numbers only)", ref characterAddWeight, 100);
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
                    if (ImGui.ImageButton(this.lawfulGoodPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulgood", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.lawfulGoodMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulgood", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.lawfulGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[0] * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[0].ToString());
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
                    ImGui.Image(this.neutralGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[1] * 30, 20));
                    ImGui.SameLine();
                    int formattedNeutralGoodVal = neutralGoodVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[1].ToString());
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
                    ImGui.Image(this.chaoticGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[2] * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticGoodVal = chaoticGoodVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[2].ToString());
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
                    ImGui.Image(this.lawfulNeutralBar.ImGuiHandle, new Vector2(alignmentWidthVals[3] * 30, 20));
                    ImGui.SameLine();
                    int formattedLawfulNeutralVal = lawfulNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[3].ToString());
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
                    ImGui.Image(this.trueNeutralBar.ImGuiHandle, new Vector2(alignmentWidthVals[4] * 30, 20));
                    ImGui.SameLine();
                    int formattedTrueNeutralVal = trueNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[4].ToString());
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
                    ImGui.Image(this.chaoticNeutralBar.ImGuiHandle, new Vector2(alignmentWidthVals[5] * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticNeutralVal = chaoticNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[5].ToString());
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
                    ImGui.Image(this.lawfulEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[6] * 30, 20));
                    ImGui.SameLine();
                    int formattedLawfulEvilVal = lawfulEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[6].ToString());
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
                    ImGui.Image(this.neutralEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[7] * 30, 20));
                    ImGui.SameLine();
                    int formattedNeutralEvilVal = neutralEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[7].ToString());
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
                    ImGui.Image(this.chaoticEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[8] * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticEvilVal = chaoticEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentVals[8].ToString());
                    #endregion

                    #endregion
                    if (ImGui.Button("Save Bio"))
                    {                      
                        if (characterAddName == string.Empty || characterAddRace == string.Empty || characterAddGender == string.Empty || characterAddAge == string.Empty ||
                            characterAddHeight == string.Empty || characterAddWeight == string.Empty || characterAddAfg == string.Empty)
                        {
                            chatGui.PrintError("Please fill out all text fields. If you want a field to be empty please put a space in the text field to submit the application.");
                        }
                        else
                        {
                            DataSender.SubmitProfileBio(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), avatarBytes, characterAddName.Replace("'", "''"),
                                                   characterAddRace.Replace("'", "''"), characterAddGender.Replace("'", "''"), int.Parse(characterAddAge), characterAddHeight.Replace("'", "''"), characterAddWeight.Replace("'", "''"), characterAddAfg.Replace("'", "''"),
                                                   alignmentVals[0], alignmentVals[1], alignmentVals[2], alignmentVals[3], alignmentVals[4], alignmentVals[5], alignmentVals[6], alignmentVals[7], alignmentVals[8]);
                        
                        }


                    }


                }
                if (editBio == true)
                {
                    this.currentAvatarImg = pg.UiBuilder.LoadImage(existingAvatarBytes);
                    ImGui.Image(this.currentAvatarImg.ImGuiHandle, new Vector2(100, 100));

                    if (ImGui.Button("Edit Avatar"))
                    {
                        editAvatar = true;
                    }
                    ImGui.Spacing();
                    //name input
                    ImGui.Text("Name:   ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##playername", $"Character Name (The name or nickname of the character you are currently playing as)", ref characterEditName, 100);
                    //race input
                    ImGui.Text("Race:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##race", $"The IC Race of your character", ref characterEditRace, 100);
                    //gender input
                    ImGui.Text("Gender: ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##gender", $"Specifying an insult or terms such as trap or futa will get you banned. These are not genders.", ref characterEditGender, 100);
                    //age input
                    ImGui.Text("Age:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##age", $"If your character's age is not 18+ you may not make your profile nsfw (numbers only)", ref characterEditAge, 100, ImGuiInputTextFlags.CharsHexadecimal);
                    //age input
                    ImGui.Text("Height:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##height", $"Height in Fulms (numbers only)", ref characterEditHeight, 100);
                    //age input
                    ImGui.Text("Weight:");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##weight", $"Weight in Ponze (numbers only)", ref characterEditWeight, 100);
                    //at first glance input
                    ImGui.Text("At First Glance:");
                    ImGui.SameLine();
                    ImGui.InputTextMultiline("##afg", ref characterEditAfg, 500, new Vector2(400, 100));

                    ImGui.TextColored(new Vector4(1, 1, 0, 1), "ALIGNMENT:");
                    ImGui.SameLine();
                    ImGui.Text(currentAvailablePointsLeft + " Points Available:");
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
                    if (ImGui.ImageButton(this.lawfulGoodPlus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulgood", true); }
                    ImGui.SameLine();
                    if (ImGui.ImageButton(this.lawfulGoodMinus.ImGuiHandle, new Vector2(20, 20))) { ModAlignment("lawfulgood", false); }
                    ImGui.SameLine();
                    ImGui.Image(this.lawfulGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[0] * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[0].ToString());
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
                    ImGui.Image(this.neutralGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[1] * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[1].ToString());
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
                    ImGui.Image(this.chaoticGoodBar.ImGuiHandle, new Vector2(alignmentWidthVals[2] * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticGoodVal = chaoticGoodVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[2].ToString());
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
                    ImGui.Image(this.lawfulNeutralBar.ImGuiHandle, new Vector2(alignmentWidthVals[3] * 30, 20));
                    ImGui.SameLine();
                    int formattedLawfulNeutralVal = lawfulNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[3].ToString());
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
                    ImGui.Image(this.trueNeutralBar.ImGuiHandle, new Vector2(alignmentWidthVals[4] * 30, 20));
                    ImGui.SameLine();
                    int formattedTrueNeutralVal = trueNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[4].ToString());
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
                    ImGui.Image(this.chaoticNeutralBar.ImGuiHandle, new Vector2(alignmentWidthVals[5] * 30, 20));
                    ImGui.SameLine();
                    int formattedChaoticNeutralVal = chaoticNeutralVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[5].ToString());
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
                    ImGui.Image(this.lawfulEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[6] * 30, 20));
                    ImGui.SameLine();
                    int formattedLawfulEvilVal = lawfulEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[6].ToString());
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
                    ImGui.Image(this.neutralEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[7] * 30, 20));
                    ImGui.SameLine();
                    int formattedNeutralEvilVal = neutralEvilVal / 10;
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[7].ToString());
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
                    ImGui.Image(this.chaoticEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[8] * 30, 20));
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[8].ToString());
                    #endregion

                    #endregion
                    if (ImGui.Button("Update Bio"))
                    {
                        if (characterEditGender.ToLower().Contains("futa") || characterEditGender.ToLower().Contains("trap"))
                        {
                            chatGui.PrintError("Your account " + "'" + configuration.username + "' received a strike, if you continue to post offensive conent you may be banned.");
                            DataSender.StrikeAccount(configuration.username, configuration.username);
                        }
                        else if (characterEditName == string.Empty || characterEditRace == string.Empty || characterEditGender == string.Empty || characterEditAge == string.Empty ||
                            characterEditHeight == string.Empty || characterEditWeight == string.Empty || characterEditAfg == string.Empty)
                        {
                            chatGui.PrintError("Please fill out all text fields. If you want a field to be empty please put a space in the text field to submit the application.");
                        }
                        else
                        {
                            DataSender.SubmitProfileBio(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), existingAvatarBytes, characterEditName.Replace("'", "''"),
                                                   characterEditRace.Replace("'", "''"), characterEditGender.Replace("'", "''"), int.Parse(characterEditAge), characterEditHeight.Replace("'", "''"), characterEditWeight.Replace("'", "''"), characterEditAfg.Replace("'", "''"),
                                                   alignmentEditVals[0], alignmentEditVals[1], alignmentEditVals[2], alignmentEditVals[3], alignmentEditVals[4], alignmentEditVals[5], alignmentEditVals[6], alignmentEditVals[7], alignmentEditVals[8]);

                        }


                    }


                }
                if (addHooks == true)
                {
                    string hookMsg = "";
                    if (ImGui.Button("+", new Vector2(30,30)))
                    {
                        hookCount++;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("-", new Vector2(30, 30)))
                    {
                        hookCount--;
                        if(hookCount < 1)
                        {
                            hookCount = 0;
                        }
                    }

                    for (int i = 0; i < hookCount; i++)
                    {
                        int index = i + 1;
                        ImGui.InputTextMultiline("##content" + i, ref HookContent[i], 3000, new Vector2(450, 100));
                        hookMsg += "<hook>" + index + "," + HookContent[i] + "</hook>|||";
                    }
                    if (ImGui.Button("Submit Hooks"))
                    {
                        ClearUI();
                        editHooks = true;
                        addHooks = false;
                        DataSender.SendHooks(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name, hookMsg);                        

                    }
                }
                if (editHooks == true)
                {
                    if(resetHooks == true)
                    {
                        hookCount = 0;
                        resetHooks = false;
                    }
                    string hookMsg = "";
                    if (ImGui.Button("+", new Vector2(30, 30)))
                    {
                        hookCount++;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("-", new Vector2(30, 30)))
                    {
                        hookCount--;
                        if (hookCount < 1)
                        {
                            hookCount = 0;
                            hookEditCount--;
                            if (hookEditCount < 1)
                            {
                                hookEditCount = 0;
                            }
                        }
                    }
                    for (int h = 0; h < hookEditCount; h++)
                    {
                        int index = hookCount + hookEditCount;
                        string HookEdit = HookEditContent[h].Replace("---===---", "\n").Replace("''","'");
                        ImGui.InputTextMultiline("##hookedit" + h, ref HookEdit, 3000, new Vector2(450, 100));
                        hookMsg += "<hook>" +index + "," + HookEditContent[h].Replace("\n", "---===---") + "</hook>|||";
                    }
                    for(int i = 0; i < hookCount; i++)
                    {
                        int index = hookCount + hookEditCount;
                        hookMsg += "<hook>" + index + "," + HookContent[i].Replace("\n", "---===---") + "</hook>|||";
                        ImGui.InputTextMultiline("##hook" + i, ref HookContent[i], 3000, new Vector2(450, 100));
                    }
                    if (ImGui.Button("Submit Hooks"))
                    {
                         DataSender.SendHooks(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), hookMsg);
                    }
                }
                if (addAvatar == true)
                {
                    addAvatar = false;
                    AddAvatar();
                }
                if (editAvatar == true)
                {
                    editAvatar = false;
                    EditAvatar();
                }

            }
        }
        public static void ClearUI()
        {
            addBio = false;
            editBio = false;
            addHooks = false; 
            editHooks = false;
            addStory = false;
            editStory = false;
            addOOC = false;
            editOOC = false;
            addGallery = false;
            editGallery = false;
        }
        public void Dispose()
        {
            WindowOpen = false;
        }
        public override void Update()
        {
            ExistingProfile = DataReceiver.ExistingProfileData;
            ExistingBio = DataReceiver.ExistingBioData;
            ExistingHooks = DataReceiver.ExistingHooks;
            existingAvatarBytes = DataReceiver.currentAvatar;
            lawfulGoodEditVal = DataReceiver.lawfulGoodEditVal;
            loadedSelf = DataReceiver.LoadedSelf;
            
            if (editBio == true)
            {
                for (int i = 0; i < alignmentWidthVals.Length; i++)
                {
                    if (alignmentWidthVals[i] < alignmentEditVals[i]) { alignmentWidthVals[i] += 0.1f; }
                    if (alignmentWidthVals[i] > alignmentEditVals[i]) { alignmentWidthVals[i] -= 0.1f; }
                }
            }
            if (addBio == true) {
                for (int i = 0; i < alignmentWidthVals.Length; i++)
                {
                    if (alignmentWidthVals[i] < alignmentVals[i]) { alignmentWidthVals[i] += 0.1f; }
                    if (alignmentWidthVals[i] > alignmentVals[i]) { alignmentWidthVals[i] -= 0.1f; }
                }
            }

        }
        public int AvailablePercentageLeft(int lawful_good, int neutral_good, int chaotic_good, int lawful_neutral, int true_neutral, int chaotic_neutral, int lawful_evil, int neutral_evil, int chaotic_evil)
        {
            int PercentageUsed = lawful_good + neutral_good + chaotic_good + lawful_neutral + true_neutral + chaotic_neutral + lawful_evil + neutral_evil + chaotic_evil;
            int percentageLeft = availablePercentage - PercentageUsed;
            return percentageLeft;
        }
        public void ModAlignment(string alignmentName, bool add) 
        {
            for(int i =0; i < alignmentNames.Length; i++)
            {
                if (alignmentNames[i] == alignmentName)
                {
                    if(ExistingBio == true)
                    {
                        if (add)
                        {
                            if(availablePercentage > 0 && alignmentEditVals[i] < 10)
                            {
                                availablePercentage -= 1;
                                alignmentEditVals[i] += 1;
                            }
                        }
                        else
                        {
                            if (alignmentEditVals[i] > 0)
                            {
                                availablePercentage +=1;
                                alignmentEditVals[i] -= 1;
                            }
                        }

                    }
                    else
                    {
                        if (add)
                        {
                            if (availablePercentage > 0 && alignmentVals[i] < 10)
                            {
                                availablePercentage -= 1;
                                alignmentVals[i] += 1;
                            }
                        }
                        else
                        {
                            if (alignmentVals[i] > 0)
                            {
                                availablePercentage += 1;
                                alignmentVals[i] -= 1;
                            }
                        }
                    }
                }
            }


        }
        public void EditAvatar()
        {
            _fileDialogManager.OpenFileDialog("Select Avatar", "Image{.png,.jpg}", (s, f) =>
            {
                if (!s)
                    return;
                string AvatarPath = f[0].ToString();
                var avatarImage = Path.GetFullPath(AvatarPath);

                this.existingAvatarBytes = File.ReadAllBytes(AvatarPath);
                DataReceiver.currentAvatar = this.existingAvatarBytes;
                this.currentAvatarImg = this.plugin.PluginInterfacePub.UiBuilder.LoadImage(avatarImage);


            }, 0, null, this.configuration.AlwaysOpenDefaultImport);
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
