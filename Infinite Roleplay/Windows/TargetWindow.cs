using Dalamud.Interface.ImGuiFileDialog;
using Dalamud.Interface.Windowing;
using Dalamud.Plugin;
using ImGuiNET;
using ImGuiScene;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
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
using System.Security.Policy;
using OtterGui.Raii;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Windows.Markup;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using System.Threading.Channels;
using FFXIVClientStructs.FFXIV.Client.Game.Character;
using Dalamud.Game.ClientState.Objects.SubKinds;
using System.Drawing;
using Dalamud.Game;
using Dalamud.Game.Gui;
using System.Xml.Linq;
using Dalamud.Game.ClientState.Objects;
using OtterGui;
using Networking;

namespace InfiniteRoleplay.Windows
{
    internal class TargetWindow : Window, IDisposable
    {
        private readonly ConcurrentDictionary<string, string> _startPaths = new();
        private Plugin plugin;


        private float _modVersionWidth;
        private PlayerCharacter playerCharacter;
        private ChatGui chatGui;
        private DalamudPluginInterface pg;
#pragma warning disable CS0169 // The field 'ProfileWindow.profilesImage' is never used
        private TextureWrap profilesImage;

#pragma warning restore CS0169 // The field 'ProfileWindow.profilesImage' is never used
        public Configuration configuration;
        public static bool WindowOpen;
        public static byte[][] existingGalleryImgBytes = new byte[30][] { new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };
        public static byte[][] existingGalleryThumbBytes = new byte[30][] { new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };


        public static string[] StoryContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] ChapterContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] ChapterTitle = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] HookContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] HookEditContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static int chapterCount;
        public static int chapterEditCount;
        public static bool viewBio = false;
        public static bool viewHooks = false;
        public static bool viewStory = false;
        public static bool viewOOC = false;
        public static bool viewGallery = false;
        public static bool resetStory = false;
        public static bool[] galleryExists = new bool[30] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public bool ExistingBio;
        public static bool ExistingHooks;
        public static int hookCount;
        public static int hookEditCount, existingGalleryImageCount;
        private GameFontHandle _nameFont;
        private GameFontHandle _secionFont;
        public static string[] hooks;
        public bool ExistingStory;
        public bool ExistingOOC;
        public static bool ExistingGallery;
        public bool ExistingProfile;
        public static string storyTitle = "";
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
        //Font Vars
        private GameFontHandle _Font;
        //BIO VARS
        public static TextureWrap galleryImg1, galleryImg2, galleryImg3, galleryImg4, galleryImg5, galleryImg6, galleryImg7, galleryImg8, galleryImg9, galleryImg10, galleryImg11, galleryImg12, galleryImg13, galleryImg14, galleryImg15, galleryImg16, galleryImg17, galleryImg18, galleryImg19, galleryImg20, galleryImg21, galleryImg22, galleryImg23, galleryImg24, galleryImg25, galleryImg26, galleryImg27, galleryImg28, galleryImg29, galleryImg30;
        public static TextureWrap[] galleryImages, galleryThumbs;
        public static TextureWrap galleryEditThm1, galleryEditThm2, galleryEditThm3, galleryEditThm4, galleryEditThm5, galleryEditThm6, galleryEditThm7, galleryEditThm8, galleryEditThm9, galleryEditThm10, galleryEditThm11, galleryEditThm12, galleryEditThm13, galleryEditThm14, galleryEditThm15, galleryEditThm16, galleryEditThm17, galleryEditThm18, galleryEditThm19, galleryEditThm20, galleryEditThm21, galleryEditThm22, galleryEditThm23, galleryEditThm24, galleryEditThm25, galleryEditThm26, galleryEditThm27, galleryEditThm28, galleryEditThm29, galleryEditThm30;
        
        
        private TextureWrap avatarImg, currentAvatarImg;
        public static string    characterEditName = "",
                                characterEditRace = "",
                                characterEditGender = "",
                                characterEditAge = "",
                                characterEditAfg = "",
                                characterEditHeight = "",
                                characterEditWeight = "";
        public static string fileName = "";
        private readonly FileDialogManager _manager;
        private bool _isOpen;
        private TextureWrap[] otherImages;
        
        private bool _showFileDialogError = false;
        private TextureWrap lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil;
        private int lawfulGoodWidthVal = 0, neutralGoodWidthVal = 0, chaoticGoodWidthVal = 0, lawfulNeutralWidthVal = 0, trueNeutralWidthVal = 0, chaoticNeutralWidthVal = 0, lawfulEvilWidthVal = 0, neutralEvilWidthVal = 0, chaoticEvilWidthVal = 0;
        private int lawfulGoodVal = 0, neutralGoodVal = 0, chaoticGoodVal = 0, lawfulNeutralVal = 0, trueNeutralVal = 0, chaoticNeutralVal = 0, lawfulEvilVal = 0, neutralEvilVal = 0, chaoticEvilVal = 0;
        private TextureWrap lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar;
     

        public TargetWindow(Plugin plugin, DalamudPluginInterface Interface, TextureWrap avatarHolder,
                             //alignment icon
                             TextureWrap lawfulgood, TextureWrap neutralgood, TextureWrap chaoticgood,
                             TextureWrap lawfulneutral, TextureWrap trueneutral, TextureWrap chaoticneutral,
                             TextureWrap lawfulevil, TextureWrap neutralevil, TextureWrap chaoticevil,
                             
                             //bars

                             TextureWrap lawfulgoodBar, TextureWrap neutralgoodBar, TextureWrap chaoticgoodBar,
                             TextureWrap lawfulneutralBar, TextureWrap trueneutralBar, TextureWrap chaoticneutralBar,
                             TextureWrap lawfulevilBar, TextureWrap neutralevilBar, TextureWrap chaoticevilBar
                            ) : base(
       "TARGET", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(750, 950),
                MaximumSize = new Vector2(750, 950)
            };
            this.plugin = plugin;
            this.pg = plugin.PluginInterfacePub;
            this.configuration = plugin.Configuration;
            this.avatarImg = avatarHolder;
            
            this._nameFont = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
            System.Drawing.Image image1 = System.Drawing.Image.FromFile(Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/avatar_holder.png"));
            this.avatarBytes = ImageToByteArray(image1);
            //alignment icons
            this.lawfulGood = lawfulgood; this.neutralGood = neutralgood; this.chaoticGood = chaoticgood;
            this.lawfulNeutral = lawfulneutral; this.trueNeutral = trueneutral; this.chaoticNeutral = chaoticneutral;
            this.lawfulEvil = lawfulevil; this.neutralEvil = neutralevil; this.chaoticEvil = chaoticevil;
            this.chatGui = chatGui;
            this.otherImages = new TextureWrap[19] { this.currentAvatarImg, this.lawfulGood, this.neutralGood, this.chaoticGood, this.lawfulNeutral, this.trueNeutral, this.chaoticNeutral, this.lawfulEvil, this.neutralEvil, this.chaoticEvil, this.lawfulGoodBar, this.neutralGoodBar, this.chaoticGoodBar, this.lawfulNeutralBar, this.trueNeutralBar, this.chaoticNeutralBar, this.lawfulEvilBar, this.neutralEvilBar, this.chaoticEvilBar };
            galleryImages = new TextureWrap[30] { galleryImg1, galleryImg2, galleryImg3, galleryImg4, galleryImg5, galleryImg6, galleryImg7, galleryImg8, galleryImg9, galleryImg10, galleryImg11, galleryImg12, galleryImg13, galleryImg14, galleryImg15, galleryImg16, galleryImg17, galleryImg18, galleryImg19, galleryImg20, galleryImg21, galleryImg22, galleryImg23, galleryImg24, galleryImg25, galleryImg26, galleryImg27, galleryImg28, galleryImg29, galleryImg30 };
            galleryThumbs = new TextureWrap[30] { galleryEditThm1, galleryEditThm2, galleryEditThm3, galleryEditThm4, galleryEditThm5, galleryEditThm6, galleryEditThm7, galleryEditThm8, galleryEditThm9, galleryEditThm10, galleryEditThm11, galleryEditThm12, galleryEditThm13, galleryEditThm14, galleryEditThm15, galleryEditThm16, galleryEditThm17, galleryEditThm18, galleryEditThm19, galleryEditThm20, galleryEditThm21, galleryEditThm22, galleryEditThm23, galleryEditThm24, galleryEditThm25, galleryEditThm26, galleryEditThm27, galleryEditThm28, galleryEditThm29, galleryEditThm30 };

            //bars
            this.lawfulGoodBar = lawfulgoodBar; this.neutralGoodBar = neutralgoodBar; this.chaoticGoodBar = chaoticgoodBar;
            this.lawfulNeutralBar = lawfulneutralBar; this.trueNeutralBar = trueneutralBar; this.chaoticNeutralBar = chaoticneutralBar;
            this.lawfulEvilBar = lawfulevilBar; this.neutralEvilBar = neutralevilBar; this.chaoticEvilBar = chaoticevilBar;
            this._Font = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
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
            //LoadFileSelection();

            //Vector2 addProfileBtnScale = new Vector2(playerCharacter.Name.ToString().Length * 20, 20);
            if(ExistingProfile == true)
            {
                    if (ExistingBio == true)
                    {
                        if (ImGui.Button("Bio", new Vector2(100, 20))) { ClearUI(); viewBio = true; }
                        if (ImGui.IsItemHovered()) { ImGui.SetTooltip("View bio section of this profile."); }
                    }
                    if (ExistingHooks == true)
                    {
                        ImGui.SameLine();
                        if (ImGui.Button("Hooks", new Vector2(100, 20))) { ClearUI(); viewHooks = true; }
                        if (ImGui.IsItemHovered()) { ImGui.SetTooltip("View hooks section of this profile."); }
                    }
                    if (ExistingStory == true)
                    {
                        ImGui.SameLine();
                        if (ImGui.Button("Story", new Vector2(100, 20))) { ClearUI(); viewStory = true; }
                        if (ImGui.IsItemHovered()) { ImGui.SetTooltip("View story section to your profile."); }
                    }
                    if (ExistingGallery == true)
                    {
                        ImGui.SameLine();
                        if (ImGui.Button("Gallery", new Vector2(100, 20))) { ClearUI(); viewGallery = true; }
                        if (ImGui.IsItemHovered()) { ImGui.SetTooltip("View gallery section of this profile."); }
                    }
                    if (ExistingOOC == true)
                    {
                        ImGui.SameLine();
                        if (ImGui.Button("OOC Info", new Vector2(100, 20))) { ClearUI(); viewOOC = true; }
                        if (ImGui.IsItemHovered()) { ImGui.SetTooltip("ViewOOC section of this profile."); }
                    }
                }
               
              

         
            bool warning = false;
            bool success = false;
            if (ImGui.BeginChild("PROFILE"))
            {


                if (ExistingBio == false && ExistingHooks == false && ExistingStory == false && ExistingOOC == false && ExistingOOC == false)
                {
                    ImGui.Text("No Profile Data Available");
                }
                else
                {
                    if (viewBio == true)
                    {
                        this.currentAvatarImg = pg.UiBuilder.LoadImage(existingAvatarBytes);
                        ImGui.Image(this.currentAvatarImg.ImGuiHandle, new Vector2(100, 100));


                        ImGui.Spacing();
                        ImGui.Text("Name:   " + characterEditName);
                        ImGui.Text("Race:    " + characterEditRace);
                        ImGui.Text("Gender: " + characterEditGender);
                        ImGui.Text("Age:    " + characterEditAge);
                        ImGui.Text("Height:" + characterEditHeight);
                        ImGui.Text("Weight:" + characterEditWeight);
                        ImGui.Text("At First Glance:" + characterEditAfg);
                        ImGui.TextColored(new Vector4(1, 1, 0, 1), "ALIGNMENT:");

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
                        ImGui.Image(this.chaoticEvilBar.ImGuiHandle, new Vector2(alignmentWidthVals[8] * 30, 20));
                        ImGui.SameLine();
                        ImGui.TextColored(new Vector4(1, 1, 1, 1), alignmentEditVals[8].ToString());
                        #endregion

                        #endregion



                    }


                }

                if (viewHooks == true)
                {

                    for (int h = 0; h < hookEditCount; h++)
                    {
                        ImGui.Text(HookEditContent[h].Replace("---===---", "\n").Replace("''", "'"));
                    }

                }

                if (viewStory == true)
                {

                    int NameWidth = storyTitle.Length * 10;
                    var decidingWidth = Math.Max(500, ImGui.GetWindowWidth());
                    var offsetWidth = (decidingWidth - NameWidth) / 2;
                    var offsetVersion = storyTitle.Length > 0
                        ? _modVersionWidth + ImGui.GetStyle().ItemSpacing.X + ImGui.GetStyle().WindowPadding.X
                        : 0;
                    var offset = Math.Max(offsetWidth, offsetVersion);
                    if (offset > 0)
                    {
                        ImGui.SetCursorPosX(offset);
                    }


                    using var col = ImRaii.PushColor(ImGuiCol.Border, ImGuiColors.DalamudViolet);
                    using var style = ImRaii.PushStyle(ImGuiStyleVar.FrameBorderSize, 2 * ImGuiHelpers.GlobalScale);
                    using var font = ImRaii.PushFont(_nameFont.ImFont, _nameFont.Available);
                    ImGuiUtil.DrawTextButton(storyTitle, Vector2.Zero, 0);
                    string chapterMsg = "";


                    for (int h = 0; h < chapterCount; h++)
                    {
                        string Chapter = ChapterContent[h].Replace("---===---", "\n").Replace("''", "'");

                        ImGui.Text(ChapterTitle[h]);
                        using var defInfFontDen = ImRaii.DefaultFont();
                        ImGui.Text(Chapter);
                    }


                }
                if(viewGallery == true)
                {

                    if (ImGui.BeginTable("##GalleryTargetTable", 4))
                    {
                        for (int i = 0; i < existingGalleryImageCount; i++)
                        {
                            if (i % 4 == 0)
                                {
                                    ImGui.TableNextRow();
                                    ImGui.TableNextColumn();
                                    galleryImages[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(existingGalleryImgBytes[i]);
                                    galleryThumbs[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(existingGalleryThumbBytes[i]);
                                    ImGui.Image(galleryThumbs[i].ImGuiHandle, new Vector2(galleryThumbs[i].Width, galleryThumbs[i].Height));
                                    if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Click to enlarge"); }
                                    if (ImGui.IsItemClicked())
                                    {
                                        ImagePreview.width = galleryImages[i].Width;
                                        ImagePreview.height = galleryImages[i].Height;
                                        ImagePreview.PreviewImage = galleryImages[i];
                                        plugin.loadPreview = true;
                                    }
                                }
                                else
                                {
                                    ImGui.TableNextColumn();
                                    galleryImages[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(existingGalleryImgBytes[i]);
                                    galleryThumbs[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(existingGalleryThumbBytes[i]);
                                    ImGui.Image(galleryThumbs[i].ImGuiHandle, new Vector2(galleryThumbs[i].Width, galleryThumbs[i].Height));
                                    if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Click to enlarge"); }
                                    if (ImGui.IsItemClicked())
                                    {
                                        ImagePreview.width = galleryImages[i].Width;
                                        ImagePreview.height = galleryImages[i].Height;
                                        ImagePreview.PreviewImage = galleryImages[i];
                                        plugin.loadPreview = true;
                                    }
                                }

                        }
                        ImGui.EndTable();





                    }
                }
            }
                
            else
            {
                ImGui.Text("No Profile Available");
            }
                
               





        }
        public static void ClearUI()
        {
            viewBio = false;
            viewHooks = false; 
            viewStory = false;
            viewOOC= false;
            viewGallery = false;
        }
        public void Dispose()
        {
            WindowOpen = false;

            this.currentAvatarImg.Dispose();
            for(int o = 0; o < otherImages.Length; o++)
            {
                otherImages[o].Dispose();
            }
            for(int i = 0; i < galleryImages.Length; i++)
            {
                galleryImages[i].Dispose();                
            }
            for(int t = 0; t < galleryThumbs.Length; t++)
            {
                galleryThumbs[t].Dispose();
            }
        }
        public override void Update()
        {
            ExistingProfile = DataReceiver.ExistingTargetProfileData;
            ExistingBio = DataReceiver.ExistingTargetBioData;
            ExistingStory = DataReceiver.ExistingTargetStoryData;
            ExistingHooks = DataReceiver.ExistingTargetHooksData;
            ExistingOOC = DataReceiver.ExistingTargetOOCData;
            existingAvatarBytes = DataReceiver.currentTargetAvatar;
            ExistingGallery = DataReceiver.ExistingTargetGalleryData;
            hookEditCount = DataReceiver.targetHookEditCount;

            if (viewBio == true)
            {
                for (int i = 0; i < alignmentWidthVals.Length; i++)
                {
                    if (alignmentWidthVals[i] < alignmentEditVals[i]) { alignmentWidthVals[i] += 0.1f; }
                    if (alignmentWidthVals[i] > alignmentEditVals[i]) { alignmentWidthVals[i] -= 0.1f; }
                }
            }


        }
       
       
    }
}
