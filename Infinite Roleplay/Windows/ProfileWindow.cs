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
using Networking;
using System.Net.Http;
using System.Net;
using System.Drawing.Imaging;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using System.Diagnostics;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Diagnostics.Metrics;
using InfiniteRoleplay.Helpers;
using OtterGui.Table;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Aspose.Imaging;
using Aspose.Imaging.ImageFilters.FilterOptions;
using System.Timers;
using Dalamud.Interface.Internal;
using Dalamud.Plugin.Services;
using InfiniteRoleplay.Windows.Defines;
using static InfiniteRoleplay.Windows.Defines.ProfileDefines;
using Lumina.Excel.GeneratedSheets2;

namespace InfiniteRoleplay.Windows
{
    public class ProfileWindow : Window, IDisposable
    {
        public static Timer timer = new Timer(1000);
        public static bool resetGalleryTimer = true;
        public static bool Reorder = false, Reordered = false, ReorderNoSend = false;
        private Plugin plugin;
        public static bool loadedSelf = false;
        public static PlayerCharacter playerCharacter;
        private IChatGui chatGui;
        private DalamudPluginInterface pg;
        public static bool GalleryUpdateAvailable = false;
        public static bool addGalleryImageGUI = false;
        public static SortedList<int, int> reordered = new SortedList<int, int>();
        private FileDialogManager _fileDialogManager;
        public Configuration configuration;
        public static bool addBio = false;
        public static bool editBio = false;
        public static bool addHooks = false;
        public static bool editHooks = false;
        public static bool addStory = false;
        public static int imageCount = 0;
        
        public static bool resetHooks;

        public static bool[] ImageExists = new bool[30] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public static Vector4[] Cols = new Vector4[30] { new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255), new Vector4(255, 255, 255, 255) };
        public static string[] galleryStatusVals = new string[30] { "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission", "Pending Submission" };
        public static int galleryUpdates = 0;

        public static string[] galleryUpdateTxts = new string[30] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static Vector4[] ImageUpdateColors = new Vector4[30] { Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero, Vector4.Zero };
        public static int imageIndex = 0;
        public static bool[] nsfwImages = new bool[30] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public static bool[] galleryImageAdded = new bool[30] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public static bool[] nsfwImagesCheck = new bool[30] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public static bool[] nsfwImagesUncheck = new bool[30] { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
        public static bool[] NSFW = new bool[30] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
        public static int[] nsfw = new int[30] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] removalIndexes = new int[30] { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        public static int[] galleryThumbWidths = new int[30] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] galleryThumbHeights = new int[30] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] galleryButtonWidths = new int[30] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static int[] galleryButtonHeights = new int[30] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        public static string CombinedGalleryString = "";
        public static bool resetStory;
        public static string galleryUploadStatus = "";
        public static byte[][] galleryImageBytes = new byte[30][] { new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };
        public static byte[][] galleryThumbBytes = new byte[30][] { new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };
        public static byte[][] galleryEditImageBytes = new byte[30][] { new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };
        public static byte[][] galleryEditThumbBytes = new byte[30][] { new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0], new byte[0] };
        public static IDalamudTextureWrap galleryImg1, galleryImg2, galleryImg3, galleryImg4, galleryImg5, galleryImg6, galleryImg7, galleryImg8, galleryImg9, galleryImg10, galleryImg11, galleryImg12, galleryImg13, galleryImg14, galleryImg15, galleryImg16, galleryImg17, galleryImg18, galleryImg19, galleryImg20, galleryImg21, galleryImg22, galleryImg23, galleryImg24, galleryImg25, galleryImg26, galleryImg27, galleryImg28, galleryImg29, galleryImg30;
        public static IDalamudTextureWrap[] galleryImages, galleryThumbs;
        public static IDalamudTextureWrap galleryEditThm1, galleryEditThm2, galleryEditThm3, galleryEditThm4, galleryEditThm5, galleryEditThm6, galleryEditThm7, galleryEditThm8, galleryEditThm9, galleryEditThm10, galleryEditThm11, galleryEditThm12, galleryEditThm13, galleryEditThm14, galleryEditThm15, galleryEditThm16, galleryEditThm17, galleryEditThm18, galleryEditThm19, galleryEditThm20, galleryEditThm21, galleryEditThm22, galleryEditThm23, galleryEditThm24, galleryEditThm25, galleryEditThm26, galleryEditThm27, galleryEditThm28, galleryEditThm29, galleryEditThm30;
        public static string[] HookContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] HookEditContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] ChapterContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] ChapterEditContent = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] ChapterTitle = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static string[] ChapterEditTitle = new string[20] { string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty };
        public static bool editStory = false;
        public static bool addOOC = false;
        public static bool editOOC = false;
        public static bool addGallery = false;
        public static bool editGallery = false;
        public static bool addAvatar = false;
        public static bool editAvatar = false;
        public static bool addProfile = false;
        public static bool editProfile = false;
        public static bool LoadPreview = false;
        public bool ExistingBio;
        public static bool ExistingHooks;
        public static int hookCount = 0;
        public static int hookEditCount;
        public static int galleryImageCount = 0;
        public static int editImageIndexVal = 0;
        public static int chapterCount = 0;
        public static int ExistingGalleryImageCount = 0;
        public static int chapterEditCount;
        public bool ExistingStory;
        public bool ExistingOOC;
        public static byte[] picBytes;
        public bool ExistingGallery;
        public static int imageIndexVal = 0;
        public bool ExistingProfile;
        public static byte[] imageHolder, thumbHolder;
        public string storyTitle = "";
        public static string storyEditTitle = "";
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
        public static int availablePercentage = 50;
        public int[] flaggedHookIndexes = new int[] { };
        public static bool addImageToGallery = false;
        //Font Vars
      
        private GameFontHandle _Font;
        //BIO VARS
        private IDalamudTextureWrap avatarImg, currentAvatarImg;
        public static string characterAddName = string.Empty,
                                characterAddRace = string.Empty,
                                characterAddGender = string.Empty,
                                characterAddAge = string.Empty,
                                characterAddAfg = string.Empty,
                                characterAddHeight = string.Empty,
                                characterAddWeight = string.Empty,
                                characterEditName = string.Empty,
                                characterEditRace = string.Empty,
                                characterEditGender = string.Empty,
                                characterEditAge = string.Empty,
                                characterEditAfg = string.Empty,
                                characterEditHeight = string.Empty,
                                characterEditWeight = string.Empty;

        private IDalamudTextureWrap lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil;
        private float lawfulGoodWidth = 0, neutralGoodWidth = 0, chaoticGoodWidth = 0, lawfulNeutralWidth = 0, trueNeutralWidth = 0, chaoticNeutralWidth = 0, lawfulEvilWidth = 0, neutralEvilWidth = 0, chaoticEvilWidth = 0;
        private int lawfulGoodWidthVal = 0, neutralGoodWidthVal = 0, chaoticGoodWidthVal = 0, lawfulNeutralWidthVal = 0, trueNeutralWidthVal = 0, chaoticNeutralWidthVal = 0, lawfulEvilWidthVal = 0, neutralEvilWidthVal = 0, chaoticEvilWidthVal = 0;
        private int lawfulGoodVal = 0, neutralGoodVal = 0, chaoticGoodVal = 0, lawfulNeutralVal = 0, trueNeutralVal = 0, chaoticNeutralVal = 0, lawfulEvilVal = 0, neutralEvilVal = 0, chaoticEvilVal = 0;
        private IDalamudTextureWrap lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar;
        private int currentLawfulGood, currentNeutralGood, currentChaoticGood, currentLawfulNeutral, currentTrueNeutral, currentChaoticNeutral, currentLawfulEvil, currentNeutralEvil, currentChaoticEvil;
        public bool reduceChapters = false;
        public bool reduceHooks = false;
        public IDalamudTextureWrap blank;
        public static System.Drawing.Image bl;
        private IDalamudTextureWrap persistAvatarHolder;
        private IDalamudTextureWrap[] otherImages;
        public ProfileWindow(Plugin plugin, DalamudPluginInterface Interface, Configuration configuration, IDalamudTextureWrap avatarHolder,
                             //alignment icon
                             IDalamudTextureWrap lawfulgood, IDalamudTextureWrap neutralgood, IDalamudTextureWrap chaoticgood,
                             IDalamudTextureWrap lawfulneutral, IDalamudTextureWrap trueneutral, IDalamudTextureWrap chaoticneutral,
                             IDalamudTextureWrap lawfulevil, IDalamudTextureWrap neutralevil, IDalamudTextureWrap chaoticevil,

                             //bars

                             IDalamudTextureWrap lawfulgoodBar, IDalamudTextureWrap neutralgoodBar, IDalamudTextureWrap chaoticgoodBar,
                             IDalamudTextureWrap lawfulneutralBar, IDalamudTextureWrap trueneutralBar, IDalamudTextureWrap chaoticneutralBar,
                             IDalamudTextureWrap lawfulevilBar, IDalamudTextureWrap neutralevilBar, IDalamudTextureWrap chaoticevilBar,
                             System.Drawing.Image pictureTab, System.Drawing.Image blank_holder

                            ) : base(
       "PROFILE", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {
                MinimumSize = new Vector2(400, 400),
                MaximumSize = new Vector2(750, 950)
                
            };
            this.Size = new Vector2(750, 950);
            bl = blank_holder;
            this.plugin = plugin;
            this.pg = plugin.PluginInterfacePub;
            this.configuration = plugin.Configuration;
            this._fileDialogManager = new FileDialogManager();
            this.avatarImg = avatarHolder;
            this.persistAvatarHolder = avatarHolder;
            this.otherImages = new IDalamudTextureWrap[21] { blank, avatarImg, currentAvatarImg, lawfulGood, neutralGood, chaoticGood, lawfulNeutral, trueNeutral, chaoticNeutral, lawfulEvil, neutralEvil, chaoticEvil, lawfulGoodBar, neutralGoodBar, chaoticGoodBar, lawfulNeutralBar, trueNeutralBar, chaoticNeutralBar, lawfulEvilBar, neutralEvilBar, chaoticEvilBar };
            galleryImages = new IDalamudTextureWrap[30] { galleryImg1, galleryImg2, galleryImg3, galleryImg4, galleryImg5, galleryImg6, galleryImg7, galleryImg8, galleryImg9, galleryImg10, galleryImg11, galleryImg12, galleryImg13, galleryImg14, galleryImg15, galleryImg16, galleryImg17, galleryImg18, galleryImg19, galleryImg20, galleryImg21, galleryImg22, galleryImg23, galleryImg24, galleryImg25, galleryImg26, galleryImg27, galleryImg28, galleryImg29, galleryImg30 };
            galleryThumbs = new IDalamudTextureWrap[30] { galleryEditThm1, galleryEditThm2, galleryEditThm3, galleryEditThm4, galleryEditThm5, galleryEditThm6, galleryEditThm7, galleryEditThm8, galleryEditThm9, galleryEditThm10, galleryEditThm11, galleryEditThm12, galleryEditThm13, galleryEditThm14, galleryEditThm15, galleryEditThm16, galleryEditThm17, galleryEditThm18, galleryEditThm19, galleryEditThm20, galleryEditThm21, galleryEditThm22, galleryEditThm23, galleryEditThm24, galleryEditThm25, galleryEditThm26, galleryEditThm27, galleryEditThm28, galleryEditThm29, galleryEditThm30 };
            this.configuration = configuration;
            
            picBytes = Imaging.ImageToByteArray(pictureTab);

            //timer.Elapsed += OnEventExecution;
            imageHolder = Imaging.ImageToByteArray(pictureTab);
            thumbHolder = Imaging.ImageToByteArray(Imaging.byteArrayToImage(Imaging.ScaleImageBytes(picBytes, 150, 150)));
            
            for (int tb = 0; tb < galleryThumbBytes.Length; tb++)
            {
                galleryThumbBytes[tb] = Imaging.ScaleImageBytes(picBytes, 150, 150);   
            }
            for (int ib = 0; ib < galleryImageBytes.Length; ib++)
            {
                galleryImageBytes[ib]= Imaging.ImageToByteArray(pictureTab);               
            }
            System.Drawing.Image image1 = System.Drawing.Image.FromFile(Path.Combine(Interface.AssemblyLocation.Directory?.FullName!, "UI/common/avatar_holder.png"));
            this.avatarBytes = Imaging.ImageToByteArray(image1);
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
            alignmentVals = new int[9] { lawfulGoodVal, neutralGoodVal, chaoticGoodVal, lawfulNeutralVal, trueNeutralVal, chaoticNeutralVal, lawfulEvilVal, neutralEvilVal, chaoticEvilVal };
            alignmentEditVals = new int[9] { lawfulGoodEditVal, neutralGoodEditVal, chaoticGoodEditVal, lawfulNeutralEditVal, trueNeutralEditVal, chaoticNeutralEditVal, lawfulEvilEditVal, neutralEvilEditVal, chaoticEvilEditVal };
            this.alignmentWidthVals = new float[9] { lawfulGoodWidthVal, neutralGoodWidthVal, chaoticGoodWidthVal, lawfulNeutralWidthVal, trueNeutralWidthVal, chaoticNeutralWidthVal, lawfulEvilWidthVal, neutralEvilWidthVal, chaoticEvilWidthVal };
            this.alignmentNames = new string[9] { "lawfulgood", "neutralgood", "chaoticgood", "lawfulneutral", "trueneutral", "chaoticneutral", "lawfulevil", "neutralevil", "chaoticevil" };
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
                if (ExistingGallery == true) { if (ImGui.Button("Edit Gallery", new Vector2(100, 20))) { ClearUI(); addGallery = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Gallery."); } } else { if (ImGui.Button("Add Gallery", new Vector2(100, 20))) { ClearUI(); addGallery = true; } }

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
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(255,0,0,255), "All fields are required including the avatar... If you wish to have a text field empty, just put a blank space in the field.");
                    ImGui.Spacing();

                        //Gather input values and add them
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
                    ImGui.InputTextWithHint("##gender", $"The IC gender of your character", ref characterAddGender, 100);
                    //age input
                    ImGui.Text("Age:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##age", $"The IC age of your character. If your character is not 18+, do not put nsfw images in your gallery", ref characterAddAge, 100, ImGuiInputTextFlags.CharsHexadecimal);
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
                    ImGui.Text(availablePercentage + " Points Available:");

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
                    if (ImGui.Button("+##lawfulgoodplus", new Vector2(20, 20))) { ModAlignment("lawfulgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##lawfulgoodminus", new Vector2(20, 20))) { ModAlignment("lawfulgood", false); }
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
                    if (ImGui.Button("+##neutralgoodplus", new Vector2(20, 20))) { ModAlignment("neutralgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##neutralgoodminus", new Vector2(20, 20))) { ModAlignment("neutralgood", false); }
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
                    if (ImGui.Button("+##chaoticgoodplus", new Vector2(20, 20))) { ModAlignment("chaoticgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##chaoticgoodminus", new Vector2(20, 20))) { ModAlignment("chaoticgood", false); }
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
                    if (ImGui.Button("+##lawfulneutralplus", new Vector2(20, 20))) { ModAlignment("lawfulneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##lawfulneutralminus", new Vector2(20, 20))) { ModAlignment("lawfulneutral", false); }
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
                    if (ImGui.Button("+##trueneutralplus", new Vector2(20, 20))) { ModAlignment("trueneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##trueneutralminus", new Vector2(20, 20))) { ModAlignment("trueneutral", false); }
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
                    if (ImGui.Button("+##chaoticneutralplus", new Vector2(20, 20))) { ModAlignment("chaoticneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##chaoticneutralminus", new Vector2(20, 20))) { ModAlignment("chaoticneutral", false); }
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
                    if (ImGui.Button("+##lawfulevilplus", new Vector2(20, 20))) { ModAlignment("lawfulevil", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##lawfulevilminus", new Vector2(20, 20))) { ModAlignment("lawfulevil", false); }
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
                    if (ImGui.Button("+##neutralevilplus", new Vector2(20, 20))) { ModAlignment("neutralevil", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##neutralevilminus", new Vector2(20, 20))) { ModAlignment("neutralevil", false); }
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
                    if (ImGui.Button("+##chaoticevilplus", new Vector2(20, 20))) { ModAlignment("chaoticevil", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##chaoticevilminus", new Vector2(20, 20))) { ModAlignment("chaoticevil", false); }
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
                            chatGui.PrintError("Please fill out all text fields. If you want a field to be empty please put a space in the text field to submit the form.");
                        }
                        else
                        {
                            DataSender.SubmitProfileBio(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), this.avatarBytes, characterAddName.Replace("'", "''"),
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
                    ImGui.SameLine();
                    ImGui.TextColored(new Vector4(255, 0, 0, 255), "All fields are required including the avatar... If you wish to have a text field empty, just put a blank space in the field.");
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
                    ImGui.InputTextWithHint("##gender", $"The IC Gender of your character", ref characterEditGender, 100);
                    //age input
                    ImGui.Text("Age:    ");
                    ImGui.SameLine();
                    ImGui.InputTextWithHint("##age", $"If your character's age is not 18+ do not put nsfw pictures in your gallery (numbers only)", ref characterEditAge, 100, ImGuiInputTextFlags.CharsHexadecimal);
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
                    if (ImGui.Button("+##editlawfulgoodplus", new Vector2(20, 20))) { ModAlignment("lawfulgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editlawfulgoodminus", new Vector2(20, 20))) { ModAlignment("lawfulgood", false); }
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
                    if (ImGui.Button("+##editneutralgoodplus", new Vector2(20, 20))) { ModAlignment("neutralgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editneutralgoodminus", new Vector2(20, 20))) { ModAlignment("neutralgood", false); }
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
                    if (ImGui.Button("+##editchaoticgoodplus", new Vector2(20, 20))) { ModAlignment("chaoticgood", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editchaoticgoodminus", new Vector2(20, 20))) { ModAlignment("chaoticgood", false); }
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
                    if (ImGui.Button("+##editlawfulneutralplus", new Vector2(20, 20))) { ModAlignment("lawfulneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editlawfulneutralminus", new Vector2(20, 20))) { ModAlignment("lawfulneutral", false); }
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
                    if (ImGui.Button("+##edittrueneutralplus", new Vector2(20, 20))) { ModAlignment("trueneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##edittrueneutralminus", new Vector2(20, 20))) { ModAlignment("trueneutral", false); }
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
                    if (ImGui.Button("+##editchaoticneutralplus", new Vector2(20, 20))) { ModAlignment("chaoticneutral", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editchaoticneutralminus", new Vector2(20, 20))) { ModAlignment("chaoticneutral", false); }
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
                    if (ImGui.Button("+##editlawfulevilplus", new Vector2(20, 20))) { ModAlignment("lawfulevil", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editlawfulevilminus", new Vector2(20, 20))) { ModAlignment("lawfulevil", false); }
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
                    if (ImGui.Button("+##editneutralevilplus", new Vector2(20, 20))) { ModAlignment("neutralevil", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editneutralevilminus", new Vector2(20, 20))) { ModAlignment("neutralevil", false); }
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
                    if (ImGui.Button("+##editchaoticevilplus", new Vector2(20, 20))) { ModAlignment("chaoticevil", true); }
                    ImGui.SameLine();
                    if (ImGui.Button("-##editchaoticevilminus", new Vector2(20, 20))) { ModAlignment("chaoticevil", false); }
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
                    if (ImGui.Button("+", new Vector2(30, 30)))
                    {
                        chapterCount++;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("-", new Vector2(30, 30)))
                    {
                        chapterCount--;
                        if (chapterCount < 1)
                        {
                            chapterCount = 0;
                        }
                    }
                    for (int i = 0; i < chapterCount; i++)
                    {
                        int index = i + 1;
                        ImGui.InputTextMultiline("##content" + i, ref HookContent[i], 3000, new Vector2(450, 100));
                        hookMsg += "<hook>" + index + "," + HookContent[i].Replace("\n", "---===---") + "</hook>|||";
                    }
                    if (ImGui.Button("Submit Hooks"))
                    {
                        DataSender.SendHooks(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name, hookMsg);
                    }
                }
                if (editHooks == true)
                {
                    if (resetHooks == true)
                    {
                        hookCount = 0;
                        resetHooks = false;
                    }
                    string hookMsg = "";
                    if (ImGui.Button("+##addhook", new Vector2(30, 30)))
                    {
                        hookCount++;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("-##removehook", new Vector2(30, 30)))
                    {
                        reduceHooks = true;
                        if (hookCount > 0)
                        {
                            hookCount--;
                            reduceHooks = false;
                        }
                        if (hookCount == 0 && reduceHooks == true)
                        {
                            if (hookEditCount > 0)
                            {
                                hookEditCount--;
                            }
                        }
                    }
                    for (int i = 0; i < hookCount; i++)
                    {
                        int index = hookCount + hookEditCount;
                        hookMsg += "<hook>" + HookContent[i].Replace("\n", "---===---") + "</hook>|||";
                        ImGui.InputTextMultiline("##hook" + i, ref HookContent[i], 3000, new Vector2(450, 100));
                    }
                    for (int h = 0; h < hookEditCount; h++)
                    {
                        int index = hookCount + hookEditCount;
                        ImGui.InputTextMultiline("##hookedit" + h, ref HookEditContent[h], 3000, new Vector2(450, 100));
                        hookMsg += "<hook>" + HookEditContent[h].Replace("\n", "---===---") + "</hook>|||";
                    }
                    if (ImGui.Button("Submit Hooks"))
                    {
                        DataSender.SendHooks(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), hookMsg);
                    }
                }
                if (addStory == true)
                {
                    string storyMsg = "";
                    ImGui.Text("Story Title:");
                    ImGui.SameLine();
                    ImGui.InputText("##storytitle", ref storyTitle, 100);
                    if (ImGui.Button("Add Chapter"))
                    {
                        chapterCount++;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Remove Chapter"))
                    {
                        chapterCount--;
                        if (chapterCount < 1)
                        {
                            chapterCount = 0;
                        }
                    }
                    for (int i = 0; i < chapterCount; i++)
                    {
                        int index = i + 1;
                        ImGui.Text("Chapter Title:");
                        ImGui.SameLine();
                        ImGui.InputText("##chaptertitle" + i, ref ChapterTitle[i], 100);

                        ImGui.Text("Chapter:");
                        ImGui.SameLine();
                        ImGui.InputTextMultiline("##content" + i, ref ChapterContent[i], 3000, new Vector2(450, 100));
                        storyMsg += "<chapter><chapterTitle>" + ChapterTitle[i] + "</chapterTitle><chapterContent>" + ChapterContent[i].Replace("\n", "---===---") + "</chapterContent></chapter>|||";
                    }
                    if (ImGui.Button("Submit Story"))
                    {
                        DataSender.SendStory(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name, storyTitle, storyMsg);
                    }
                }
                if (editStory == true)
                {
                    if (resetStory == true)
                    {
                        chapterCount = 0;
                        resetStory = false;
                    }
                    ImGui.Text("Story Title:");
                    ImGui.SameLine();
                    ImGui.InputText("##story_edit_title", ref storyEditTitle, 100);
                    string chapterMsg = "";
                    string ChapterEdit = "";
                    if (ImGui.Button("Add Chapter"))
                    {
                        chapterCount++;
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Remove Chapter"))
                    {
                        reduceChapters = true;
                        if (chapterCount > 0)
                        {
                            chapterCount--;
                            reduceChapters = false;
                        }
                        if (chapterCount == 0 && reduceChapters == true)
                        {
                            if (chapterEditCount > 0)
                            {
                                chapterEditCount--;
                            }
                        }
                    }
                    for (int h = 0; h < chapterEditCount; h++)
                    {
                        ImGui.Text("Chapter Title:");
                        ImGui.SameLine();
                        ImGui.InputText("##editchaptertitle" + h, ref ChapterEditTitle[h], 100);

                        ImGui.Text("Chapter:");
                        ImGui.SameLine();
                        ImGui.InputTextMultiline("##chapteredit" + h, ref ChapterEditContent[h], 3000, new Vector2(450, 100));
                        ChapterEdit += "<chapter><chapterTitle>" + ChapterEditTitle[h] + "</chapterTitle><chapterContent>" + ChapterEditContent[h].Replace("\n", "---===---") + "</chapterContent></chapter>|||";
                    }
                    for (int i = 0; i < chapterCount; i++)
                    {
                        ImGui.Text("Chapter Title:");
                        ImGui.SameLine();
                        ImGui.InputText("##chaptertitle" + i, ref ChapterTitle[i], 100);
                        chapterMsg += "<chapter><chapterTitle>" + ChapterTitle[i] + "</chapterTitle><chapterContent>" + ChapterContent[i].Replace("\n", "---===---") + "</chapterContent></chapter>|||";
                        ImGui.Text("Chapter:");
                        ImGui.SameLine();
                        ImGui.InputTextMultiline("##chapter" + i, ref ChapterContent[i], 3000, new Vector2(450, 100));
                    }
                    string chapterMessage = ChapterEdit + chapterMsg;
                    if (ImGui.Button("Update Story"))
                    {
                        DataSender.SendStory(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), storyEditTitle, chapterMessage);
                    }
                }
                if (addGallery == true)
                {
                    if (ImGui.Button("Add Image"))
                    {
                        if(imageIndex < 30)
                        {
                            imageIndex++;
                        }
                    }
                    if (resetGalleryTimer == true)
                    {
                       // timer.Stop();
                      //  EnableGallerySubmition();
                    }
                    else
                    {
                        ImGui.Text(galleryUploadStatus);
                    }
                    ImGui.NewLine();
                    addGalleryImageGUI = true;
                    ImageExists[imageIndex] = true;
                }

                if (addImageToGallery == true)
                {
                    addImageToGallery = false;
                    AddImage(false, imageIndexVal);
                }
                

                if (addAvatar == true)
                {
                    addAvatar = false;
                    AddImage(true, 0);
                }
                if (editAvatar == true)
                {
                    editAvatar = false;
                    EditImage(true, 0);
                }
                if (addGalleryImageGUI == true)
                {
                    AddImageToGallery(plugin, imageIndex);
                }
                if (Reorder == true)
                {                   
                    Reorder = false; 
                    bool nextExists = ImageExists[NextAvailableImageIndex() + 1];
                    int firstOpen = NextAvailableImageIndex();
                    ImageExists[firstOpen] = true;
                    if (nextExists)
                    {                       
                        for (int i = firstOpen; i < imageIndex; i++)
                        {                            
                            galleryImageBytes[i] = galleryImageBytes[i + 1];
                            galleryThumbBytes[i] = galleryThumbBytes[i + 1];
                            DrawImage(i, plugin);
                        }
                    }

                    imageIndex--;
                    galleryImageBytes[imageIndex] = picBytes;
                    galleryThumbBytes[imageIndex] = Imaging.ScaleImageBytes(picBytes, 150, 150);
                    ImageExists[imageIndex] = false;

                }
                if (ReorderNoSend == true)
                {
                    ReorderNoSend = false;
                    bool nextExists = ImageExists[NextAvailableImageIndex() + 1];
                    int firstOpen = NextAvailableImageIndex();
                    for (int i = firstOpen; i < imageIndex; i++)
                    {
                        ImageExists[firstOpen] = true;
                        if (nextExists)
                        {
                            galleryImageBytes[i] = galleryImageBytes[i + 1];
                            galleryThumbBytes[i] = galleryThumbBytes[i + 1];

                        }
                    }
                }
               
            }
        }
        /*public static void OnEventExecution(System.Object? sender, ElapsedEventArgs e)
        {
            int time = 60 - e.SignalTime.Second;
            galleryUploadStatus = "Please wait " + time + " seconds to submit gallery...";
            if(time == 60)
            {
                resetGalleryTimer = true;            
            }
        }
        public void EnableGallerySubmition()
        {
            if (ImGui.Button("Submit Gallery"))
            {
                resetGalleryTimer = false;
                timer.Start();
            }
        }*/
        public void AddImageToGallery(Plugin plugin, int imageIndex)
        {
            if(addGallery == true)
            {                
                if (ImGui.BeginTable("##GalleryTable", 4))
                {

                    for (int i = 0; i < imageIndex; i++)
                    {                       
                        if (i % 4 == 0)
                        {
                            ImGui.TableNextRow();
                            ImGui.TableNextColumn();
                            DrawGalleryImage(i, plugin);
                        }
                        else
                        {
                            ImGui.TableNextColumn();
                            DrawGalleryImage(i, plugin);
                        }
                    }
                    ImGui.EndTable();
                }
           


            }

        }
        
        public static int NextAvailableImageIndex()
        {
            bool load = true;
            int index = 0;
            for(int i = 0; i < ImageExists.Length; i++)
            {
                if (ImageExists[i] == false && load == true)
                {
                    load = false;
                    index = i;
                    return index;
                }
            }
            return index;
        }


        public void DrawGalleryImage(int i, Plugin plugin)
        {
            if (ImageExists[i] == true)
            {
               
                if (ImGui.BeginChild("##GalleryImage" + i, new Vector2(150, 240)))
                {
                    ImGui.Text("Will this image be 18+ ?");
                    if (ImGui.Checkbox("Yes", ref nsfwImagesCheck[i]))
                    {
                        nsfwImagesUncheck[i] = false;
                        nsfw[i] = 1;
                        NSFW[i] = true;
                        DataSender.SendGalleryImage(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(),
                                                    NSFW[i], galleryImageBytes[i], i);
                    }
                    ImGui.SameLine();
                    if (ImGui.Checkbox("No", ref nsfwImagesUncheck[i]))
                    {
                        nsfwImagesCheck[i] = false;
                        nsfw[i] = -1;
                        NSFW[i] = false;
                        DataSender.SendGalleryImage(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(),
                                                    NSFW[i], galleryImageBytes[i], i);


                    }

                    if (nsfwImagesCheck[i] == false && nsfwImagesUncheck[i] == false)
                    {
                        nsfw[i] = 0;
                        nsfwImagesUncheck[i] = true;
                    }
                    else
                    {
                        ImageExists[i] = true;
                    }
                    

                    
                    if (galleryImageAdded[i] == false && galleryThumbBytes[i].Length > 0 && galleryImageBytes[i].Length > 0)
                    {
                        DrawImage(i, plugin);
                        galleryImageAdded[i] = true;
                    }
                    ImGui.Image(galleryThumbs[i].ImGuiHandle, new Vector2(galleryThumbs[i].Width, galleryThumbs[i].Height));
                    if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Click to enlarge"); }
                    if (ImGui.IsItemClicked())
                    {
                        ImagePreview.width = galleryImages[i].Width;
                        ImagePreview.height = galleryImages[i].Height;
                        ImagePreview.PreviewImage = galleryImages[i];
                        plugin.loadPreview = true;
                    }
                    if (ImGui.BeginChild("##GalleryImageControls" + i))
                    {
                        if (ImGui.Button("Upload##" + "gallery_add" + i))
                        {
                            addImageToGallery = true;
                            imageIndexVal = i;                             
                        }
                        ImGui.SameLine();
                        if (ImGui.Button("Remove##" + "gallery_remove" + i))
                        {
                            imageIndexVal = i;
                            ImageExists[i] = false;
                            Reorder = true;
                            galleryImageAdded[i] = false;
                            removalIndexes[i] = 1;
                            DataSender.RemoveGalleryImage(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), i, imageIndex);
                        }
                    }
                    ImGui.EndChild();
                }


        ImGui.EndChild();

        }






    }
        public void DrawImage(int i, Plugin plugin)
        {
            Task.Run(async () =>
            {
                // you might normally want to embed resources and load them from the manifest stream
                galleryImages[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(galleryImageBytes[i]);
                galleryThumbs[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(galleryThumbBytes[i]);
                //this.imageTextures.Add(goatImage);
            });
        }
        public void ResetImages(Plugin plugin)
        {
            Task.Run(async () =>
            {
                System.Drawing.Image image1 = System.Drawing.Image.FromFile(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/picturetab.png"));
                for(int i = 0; i < galleryImages.Length; i++)
                {
                    // you might normally want to embed resources and load them from the manifest stream
                    galleryImages[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(Imaging.ScaleImageBytes(Imaging.ImageToByteArray(image1), 150, 150));
                    galleryThumbs[i] = plugin.PluginInterfacePub.UiBuilder.LoadImage(Imaging.ScaleImageBytes(Imaging.ImageToByteArray(image1), 150, 150));
                   
                }
                for (int tb = 0; tb < galleryThumbBytes.Length; tb++)
                {
                    galleryThumbBytes[tb] = Imaging.ScaleImageBytes(Imaging.ImageToByteArray(image1), 150, 150);
                }
                for (int ib = 0; ib < galleryImageBytes.Length; ib++)
                {
                    galleryImageBytes[ib] = Imaging.ImageToByteArray(image1);
                }
                //this.imageTextures.Add(goatImage);
            });
        }
        
        public void ResetUI(Plugin plugin)
        {
            ResetImages(plugin);

            System.Drawing.Image image1 = System.Drawing.Image.FromFile(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/avatar_holder.png"));
            this.avatarBytes = Imaging.ImageToByteArray(image1);
            this.avatarImg = this.persistAvatarHolder;
            this.currentAvatarImg = this.persistAvatarHolder;
            characterAddName = string.Empty;
            characterAddRace = string.Empty;
            characterAddGender = string.Empty;
            characterAddAge = string.Empty;
            characterAddAfg = string.Empty;
            characterAddHeight = string.Empty;
            characterAddWeight = string.Empty;
            characterEditName = string.Empty;
            characterEditRace = string.Empty;
            characterEditGender = string.Empty;
            characterEditAge = string.Empty;
            characterEditAfg = string.Empty;
            characterEditHeight = string.Empty;
            characterEditWeight = string.Empty;
            for(int h = 0; h < hookCount; h++)
            {
                HookContent[h] = string.Empty;
                HookEditContent[h] = string.Empty;
            }
            hookCount = 0;
            
            for(int a = 0; a < alignmentWidthVals.Length; a++)
            {
                alignmentWidthVals[a] = 0f;
            }
            for(int g = 0; g < galleryImages.Length; g++)
            {
                ImageExists[g] = false;
                imageIndexVal = 0;
                imageIndex = 0;
                Reorder = true;
                galleryImageAdded[g] = false;
                removalIndexes[g] = 1;
            }

            for(int s = 0; s < chapterEditCount; s++)
            {
                ChapterTitle[s] = string.Empty;
                ChapterEditTitle[s] = string.Empty;
                ChapterContent[s] = string.Empty;
                ChapterEditContent[s] = string.Empty;
                chapterCount = 0;
            }

            for(int i = 0; i < 30; i++)
            {
                ImageExists[i] = false;
            }
           

            chapterCount = 0;
            chapterEditCount = 0;
            storyTitle = string.Empty;
            storyEditTitle = string.Empty;
            for(int al = 0; al < alignmentEditVals.Length; al++)
            {
                alignmentEditVals[al] = 0;
            }
            availablePercentage = 50;
           
        }
        public static void UpdateUploadStatus(int index)
        {
            ImageUpdateColors[index] = new Vector4(0, 255, 0, 255);
            galleryUpdateTxts[index] = "Completed";
            galleryUpdates = index;
            
            if(index == galleryEditImageBytes.Length)
            {
                DataSender.SendImagesReceived(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString());
            }
        }
        public void SET_VAL(string mytype, string myvalue)
        {
            this.GetType().GetField(mytype).SetValue(this, myvalue);
            
        }
        public object FindValByName(string PropName)
        {
            PropName = PropName.ToLower();
            var props = this.GetType().GetProperties();

            foreach (var item in props)
            {
                if (item.Name.ToLower() == PropName)
                {
                    return item.GetValue(this);
                }
            }
            return null;
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
            foreach (IDalamudTextureWrap ti in galleryImages)
            {
                ti.Dispose();
                Array.Clear(galleryImages);
            }
            foreach (IDalamudTextureWrap gt in galleryThumbs)
            {
                gt.Dispose();
                Array.Clear(galleryThumbs);
            }
            for(int o = 0; o < otherImages.Length; o++)
            {
                otherImages[o].Dispose();
                Array.Clear(otherImages);
            }
           // timer.Dispose();
            storyTitle = "";
        }
        public void UpdateUI()
        {
            int usedAlignmentVals = 0;
            for (int al = 0; al < alignmentEditVals.Length; al++)
            {
                usedAlignmentVals += alignmentEditVals[al];
            }
            availablePercentage = 50 - usedAlignmentVals;
        }
        public override void Update()
        {
            ExistingProfile = DataReceiver.ExistingProfileData;
            ExistingStory = DataReceiver.ExistingStoryData;
            ExistingBio = DataReceiver.ExistingBioData;
            ExistingHooks = DataReceiver.ExistingHooksData;
            ExistingGallery = DataReceiver.ExistingGalleryData;
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
      
        public void EditImage(bool avatar, int imageIndex)
        {
            _fileDialogManager.OpenFileDialog("Select Image", "Image{.png,.jpg}", (s, f) =>
            {
                if (!s)
                    return;
                string imagePath = f[0].ToString();
                var image = Path.GetFullPath(imagePath);
                byte[] imageBytes = File.ReadAllBytes(image);
                if (avatar == true)
                {
                    this.existingAvatarBytes = File.ReadAllBytes(imagePath);
                    DataReceiver.currentAvatar = this.existingAvatarBytes;
                    this.currentAvatarImg = this.plugin.PluginInterfacePub.UiBuilder.LoadImage(image);
                }
                else
                {
                    galleryThumbBytes[imageIndex] = Imaging.ScaleImageBytes(imageBytes, 150, 150);
                    galleryImageBytes[imageIndex] = imageBytes;
                    galleryThumbWidths[imageIndex] = Imaging.byteArrayToImage(galleryThumbBytes[imageIndex]).Width;
                    galleryThumbHeights[imageIndex] = Imaging.byteArrayToImage(galleryThumbBytes[imageIndex]).Height;
                }


            }, 0, null, this.configuration.AlwaysOpenDefaultImport);
        }


       
        public void AddImage(bool avatar, int i)
        {
            _fileDialogManager.OpenFileDialog("Select Image", "Image{.png,.jpg, .gif}", (s, f) =>
            {
                if (!s)
                    return;
                _ = Task.Run(async () =>
                {
                    if (avatar == true)
                    {
                        string AvatarPath = f[0].ToString();
                        var avatarImage = Path.GetFullPath(AvatarPath);

                        this.avatarImg = this.plugin.PluginInterfacePub.UiBuilder.LoadImage(avatarImage);

                        this.avatarBytes = File.ReadAllBytes(AvatarPath);
                    }
                    else
                    {
                        string imagePath = f[0].ToString();
                        var image = Path.GetFullPath(imagePath);

                        byte[] imgBytes = File.ReadAllBytes(imagePath);

                        byte[] scaledImageBytes = Imaging.ScaleImageBytes(imgBytes, 150, 150);
                        System.Drawing.Image scaledImage = Imaging.byteArrayToImage(scaledImageBytes);

                        galleryImageBytes[i] = Imaging.ScaleImageBytes(imgBytes, 900, 900);

                        galleryThumbBytes[i] = scaledImageBytes;

                        SortedList<SortedList<int, bool>, SortedList<int, byte[]>> newImageList = new SortedList<SortedList<int, bool>, SortedList<int, byte[]>>();
                        DataSender.SendGalleryImage(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(),
                                                    NSFW[i], galleryImageBytes[i], i);
                        DrawImage(i, plugin);
                    }

                });
            }, 0, null, this.configuration.AlwaysOpenDefaultImport);
        }


       

    }
}
