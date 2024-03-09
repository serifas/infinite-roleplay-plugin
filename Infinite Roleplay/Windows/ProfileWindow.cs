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
using static InfiniteRoleplay.Defines.ProfileDefines;
using Lumina.Excel.GeneratedSheets2;
using InfiniteRoleplay.Scripts.Misc;
using OtterGui;
using static FFXIVClientStructs.FFXIV.Common.Component.BGCollision.MeshPCB;
using System.Linq.Expressions;

namespace InfiniteRoleplay.Windows
{
    //changed
    public class ProfileWindow : Window, IDisposable
    {
        public static bool AllLoaded;
        public static bool Reorder = false, ReorderNoSend = false;
        private Plugin plugin;
        public static Plugin pluginP;
        public static Misc misc = new Misc();
        public static bool loadedSelf = false;
        public static PlayerCharacter playerCharacter;
        private DalamudPluginInterface pg;
        public static bool addGalleryImageGUI = false;
        private FileDialogManager _fileDialogManager;
        public Configuration configuration;
        public static bool addBio = false;
        public static bool editBio = false;
        public static bool addHooks = false;
        public static bool editHooks = false;
        public static bool addStory = false;
        public static bool galleryTableAdded = false;
        public static Timer timer;
        public static bool resetHooks;

        public static bool[] ImageExists = new bool[30];
      
        public static int imageIndex = 0;
        public static bool resetStory;
    
        public static IDalamudTextureWrap loaderAnimInd, pictureTab;
        public static string[] HookContent = new string[30];
        public static string[] HookEditContent = new string[30];
        public static string[] ChapterContent = new string[30];
        public static string[] ChapterEditContent = new string[30];
        public static string[] ChapterTitle = new string[30];
        public static string[] ChapterEditTitle = new string[30];
        public static bool[] NSFW = new bool[30];
        public static bool editStory, addOOC, editOOC, addGallery, editGallery, addAvatar, editAvatar, addProfile, editProfile, LoadPreview = false;
        public static int hookCount = 0;
        public static int hookEditCount;
        public static int chapterCount = 0;
        public static int chapterEditCount;
        public static byte[] picBytes;
        public static int imageIndexVal = 0;
        public bool ExistingProfile;
        public bool ExistingStory;
        public bool ExistingOOC;
        public bool ExistingGallery;
        public bool ExistingBio;
        public bool ExistingHooks;
        public static byte[] thumbHolder;
        public string storyTitle = "";
        public static string storyEditTitle = "";
        public static int currentAlignment, currentPersonality_1, currentPersonality_2, currentPersonality_3 = 0;
        public byte[] avatarBytes, existingAvatarBytes;
        public static int availablePercentage = 50;
        public int[] flaggedHookIndexes = new int[] { };
        public static bool addImageToGallery = false;
        public static string[] imageURLs = new string[30];
        private float _modVersionWidth;
        private GameFontHandle _Font;
        private IDalamudTextureWrap avatarImg, currentAvatarImg;
        public static List<IDalamudTextureWrap> galleryThumbsList = new List<IDalamudTextureWrap>();
        public static List<IDalamudTextureWrap> galleryImagesList = new List<IDalamudTextureWrap>();
        public static IDalamudTextureWrap[] galleryImages, galleryThumbs;
        public static string[] bioFieldsArr = new string[7];

        public bool reduceChapters = false;
        public bool reduceHooks = false;
        public IDalamudTextureWrap blank;
        public static System.Drawing.Image bl;
        private IDalamudTextureWrap persistAvatarHolder;
        private IDalamudTextureWrap[] otherImages;
        public static int alignmentVal;

        public ProfileWindow(Plugin plugin,
                             DalamudPluginInterface Interface,
                             IChatGui chatGUI,
                             Configuration configuration) : base(
       "PROFILE", ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        {
            this.SizeConstraints = new WindowSizeConstraints
            {

                MinimumSize = new Vector2(600, 400),
                MaximumSize = new Vector2(750, 950)
            };
          
            this.plugin = plugin;
            this.pg = plugin.PluginInterfacePub;
            this.configuration = plugin.Configuration;
            this._fileDialogManager = new FileDialogManager();
            string avatarPath = Path.Combine(pg.AssemblyLocation.Directory?.FullName!, @"UI/common/avatar_holder.png");
            IDalamudTextureWrap avatarHolder = plugin.PluginInterfacePub.UiBuilder.LoadImage(File.ReadAllBytes(avatarPath));
            string pictureTabPath = Path.Combine(pg.AssemblyLocation.Directory?.FullName!, @"UI/common/picturetab.png");
            pictureTab = plugin.PluginInterfacePub.UiBuilder.LoadImage(File.ReadAllBytes(pictureTabPath));
            this.avatarImg = avatarHolder;
            this.persistAvatarHolder = avatarHolder;
            this.configuration = configuration;
            

            timer = new Timer(30);
            timer.Elapsed += misc.OnEventExecution;
            for(int i = 0; i < 30; i++)
            {
                ChapterTitle[i] = string.Empty;
                ChapterEditTitle[i] = string.Empty;
                ChapterContent[i] = string.Empty;
                ChapterEditContent[i] = string.Empty;
                HookContent[i] = string.Empty;
                HookEditContent[i] = string.Empty;
                NSFW[i] = false;
                galleryImagesList.Add(pictureTab);
                galleryThumbsList.Add(pictureTab);
                imageURLs[i] = string.Empty;
            }
            galleryImages = galleryImagesList.ToArray();
            galleryThumbs = galleryThumbsList.ToArray();
            for(int b = 0; b <bioFieldsArr.Length; b++)
            {
                bioFieldsArr[b] = string.Empty;
            }
           
            this.avatarBytes = File.ReadAllBytes(avatarPath) ;
            //alignment icons
            
             this._Font = pg.UiBuilder.GetGameFontHandle(new GameFontStyle(GameFontFamilyAndSize.Jupiter23));
         }
      

        public override void Draw()
        {
            if (AllLoaded == true)
            {

                _fileDialogManager.Draw();

                Misc.RemoveLoader(timer);
               
                if (this.ExistingProfile == true)
                {
                    if (ImGui.Button("Edit Profile", new Vector2(100, 20))) { editProfile = true; }
                }
                if (this.ExistingProfile == false)
                {
                    if (ImGui.Button("Add Profile", new Vector2(100, 20))) { addProfile = true; DataSender.CreateProfile(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString()); }
                }

                
                if (editProfile == true)
                {
                    addProfile = false;
                    ImGui.Spacing();
                    if (this.ExistingBio == true) { if (ImGui.Button("Edit Bio", new Vector2(100, 20))) { ClearUI(); editBio = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your bio."); } } else { if (ImGui.Button("Add Bio", new Vector2(100, 20))) { ClearUI(); editBio = true; } }
                    ImGui.SameLine();
                    if (this.ExistingHooks == true) { if (ImGui.Button("Edit Hooks", new Vector2(100, 20))) { ClearUI(); editHooks = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Hooks."); } } else { if (ImGui.Button("Add Hooks", new Vector2(100, 20))) { ClearUI(); editHooks = true; } }
                    ImGui.SameLine();
                    if (this.ExistingStory == true) { if (ImGui.Button("Edit Story", new Vector2(100, 20))) { ClearUI(); editStory = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Story."); } } else { if (ImGui.Button("Add Story", new Vector2(100, 20))) { ClearUI(); editStory = true; } }
                    ImGui.SameLine();
                    if (this.ExistingOOC == true) { if (ImGui.Button("Edit OOC Info", new Vector2(100, 20))) { ClearUI(); editOOC = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your OOC Info."); } } else { if (ImGui.Button("Add OOC Info", new Vector2(100, 20))) { ClearUI(); addOOC = true; } }
                    ImGui.SameLine();
                    if (this.ExistingGallery == true) { if (ImGui.Button("Edit Gallery", new Vector2(100, 20))) { ClearUI(); addGallery = true; } if (ImGui.IsItemHovered()) { ImGui.SetTooltip("Edit your Gallery."); } } else { if (ImGui.Button("Add Gallery", new Vector2(100, 20))) { ClearUI(); addGallery = true; } }

                }
                bool warning = false;
                bool success = false;
                if (ImGui.BeginChild("PROFILE"))
                {
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
                        for (int i = 0; i < Constants.BioFieldVals.Length; i++)
                        {
                            var BioField = Constants.BioFieldVals[i];
                            if (BioField.Item4 == Constants.InputTypes.single)
                            {
                                ImGui.Text(BioField.Item1);
                                if(BioField.Item1 != "AT FIRST GLANCE:")
                                {
                                    ImGui.SameLine();
                                }                                
                                ImGui.InputTextWithHint(BioField.Item2, BioField.Item3, ref bioFieldsArr[i], 100);
                            }
                            else
                            {
                                ImGui.Text(BioField.Item1);
                                ImGui.InputTextMultiline(BioField.Item2, ref bioFieldsArr[i], 3000, new Vector2(500, 150));
                            }
                        }
                        ImGui.Spacing();
                        ImGui.Spacing();
                        ImGui.TextColored(new Vector4(1, 1, 1, 1), "ALIGNMENT:");                       
                        AddAlignmentSelection();
                        ImGui.Spacing();
                        ImGui.TextColored(new Vector4(1, 1, 1, 1), "PERSONALITY TRAITS:");
                        AddPersonalitySelection_1();
                        AddPersonalitySelection_2();
                        AddPersonalitySelection_3();
                        if (ImGui.Button("Save Bio"))
                        {
                            DataSender.SubmitProfileBio(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(),
                                                    existingAvatarBytes,
                                                    bioFieldsArr[(int)Constants.BioFieldTypes.name].Replace("'", "''"),
                                                    bioFieldsArr[(int)Constants.BioFieldTypes.race].Replace("'", "''"),
                                                    bioFieldsArr[(int)Constants.BioFieldTypes.gender].Replace("'", "''"),
                                                    int.Parse(bioFieldsArr[(int)Constants.BioFieldTypes.age]),
                                                    bioFieldsArr[(int)Constants.BioFieldTypes.height].Replace("'", "''"),
                                                    bioFieldsArr[(int)Constants.BioFieldTypes.weight].Replace("'", "''"),
                                                    bioFieldsArr[(int)Constants.BioFieldTypes.afg].Replace("'", "''"),
                                                    currentAlignment, currentPersonality_1, currentPersonality_2, currentPersonality_3);

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
                            if (imageIndex < 30)
                            {
                                imageIndex++;
                            }
                        }
                        ImGui.SameLine();
                        if(ImGui.Button("Submit Gallery"))
                        {
                            for(int i = 0; i < imageIndex; i++)
                            {
                                DataSender.SendGalleryImage(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(),
                                                  NSFW[i], imageURLs[i], i);
                            }   
                            
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


                   
                    if (addGalleryImageGUI == true)
                    {
                        AddImageToGallery(plugin, imageIndex);
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
                                galleryImages[i] = galleryImages[i + 1];
                                galleryThumbs[i] = galleryThumbs[i + 1];
                               
                            }
                        }

                        imageIndex--;
                        galleryImages[imageIndex] = pictureTab;
                        galleryThumbs[imageIndex] = pictureTab;
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
                                galleryImages[i] = galleryImages[i + 1];
                                galleryThumbs[i] = galleryThumbs[i + 1];

                            }
                        }
                    }

                }
            }
            else
            {

                misc.AddLoader(timer);
            }
        }


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
                    if (ImGui.Checkbox("Yes", ref NSFW[i]))
                    {
                        DataSender.SendGalleryImage(configuration.username, playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(),
                                                    NSFW[i], imageURLs[i], i);
                    }
                    ImGui.InputText("##ImageURL" + i, ref imageURLs[i], 300);
                    try
                    {
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
                            if (ImGui.Button("Remove##" + "gallery_remove" + i))
                            {
                                imageIndexVal = i;
                                ImageExists[i] = false;
                                Reorder = true;
                                DataSender.RemoveGalleryImage(playerCharacter.Name.ToString(), playerCharacter.HomeWorld.GameData.Name.ToString(), i, imageIndex);
                            }
                        }
                        ImGui.EndChild();
                    }
                    catch(Exception ex)
                    {
                        plugin.chatGUI.Print(ex.Message);
                    }
            }


        ImGui.EndChild();

        }






    }
        public void ResetGallery(Plugin plugin)
        {
            Task.Run(async () =>
            {


                for (int g = 0; g < galleryImages.Length; g++)
                {
                    imageIndexVal = 0;
                    imageIndex = 0;
                    Reorder = true;
                }
                for (int i = 0; i < 30; i++)
                {
                    ImageExists[i] = false;
                }
                for(int i = 0; i < galleryImages.Length; i++)
                {
                    // you might normally want to embed resources and load them from the manifest stream
                    galleryImages[i] = pictureTab;
                    galleryThumbs[i] = pictureTab;
                   
                }
                //this.imageTextures.Add(goatImage);
            });
        }
        public void Reset(Plugin plugin)
        {
            ResetBio(plugin);
            ResetGallery(plugin);
            ResetHooks();
            ResetStory();
        }
        public void ResetBio(Plugin plugin)
        {
            System.Drawing.Image image1 = System.Drawing.Image.FromFile(Path.Combine(plugin.PluginInterfacePub.AssemblyLocation.Directory?.FullName!, "UI/common/avatar_holder.png"));
            this.avatarBytes = Imaging.ImageToByteArray(image1);
            DataReceiver.currentAvatar = avatarBytes;
            this.avatarImg = this.persistAvatarHolder;
            this.currentAvatarImg = this.persistAvatarHolder;
           
            
           /* for(int a = 0; a < alignmentWidthVals.Length; a++)
            {
                alignmentWidthVals[a] = 0f;
            }
           

           
            for (int av = 0; av < alignmentVals.Length; av++)
            {
                alignmentVals[av] = 0;
            }
            for (int al = 0; al < alignmentVals.Length; al++)
            {
                alignmentVals[al] = 0;
            }
            availablePercentage = 50;
           */
        }
        public void ResetHooks()
        {
            for (int h = 0; h < hookCount; h++)
            {
                HookContent[h] = string.Empty;
                HookEditContent[h] = string.Empty;
            }
            hookCount = 0;
        }
        public void ResetStory()
        {
            for (int s = 0; s < chapterEditCount; s++)
            {
                ChapterTitle[s] = string.Empty;
                ChapterEditTitle[s] = string.Empty;
                ChapterContent[s] = string.Empty;
                ChapterEditContent[s] = string.Empty;
                chapterCount = 0;
            }



            chapterCount = 0;
            chapterEditCount = 0;
            storyTitle = string.Empty;
            storyEditTitle = string.Empty;
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
        /*  public void UpdateUI()
          {
              int usedAlignmentVals = 0;
              for (int al = 0; al < alignmentVals.Length; al++)
              {
                  usedAlignmentVals += alignmentVals[al];
              }
              availablePercentage = 50 - usedAlignmentVals;
          }*/
        public void AddAlignmentSelection()
        {
            var (text, desc) = Constants.AlignmentVals[currentAlignment];
            using var combo = ImRaii.Combo("##Alignment", text);
            ImGuiUtil.HoverTooltip(desc);
            if (!combo)
                return;

            foreach (var ((newText, newDesc), idx) in Constants.AlignmentVals.WithIndex())
            {
                if (ImGui.Selectable(newText, idx == currentAlignment))
                    currentAlignment = idx;

                ImGuiUtil.SelectableHelpMarker(newDesc);
            }
        }
        public void AddPersonalitySelection_1()
        {
            var (text, desc) = Constants.PersonalityValues[currentPersonality_1];
            using var combo = ImRaii.Combo("##Personality Feature #1", text);
            ImGuiUtil.HoverTooltip(desc);
            if (!combo)
                return;

            foreach (var ((newText, newDesc), idx) in Constants.PersonalityValues.WithIndex())
            {
                if (ImGui.Selectable(newText, idx == currentPersonality_1))
                    currentPersonality_1 = idx;

                ImGuiUtil.SelectableHelpMarker(newDesc);
            }
        }
        public void AddPersonalitySelection_2()
        {
            var (text, desc) = Constants.PersonalityValues[currentPersonality_2];
            using var combo = ImRaii.Combo("##Personality Feature #2", text);
            ImGuiUtil.HoverTooltip(desc);
            if (!combo)
                return;

            foreach (var ((newText, newDesc), idx) in Constants.PersonalityValues.WithIndex())
            {
                if (ImGui.Selectable(newText, idx == currentPersonality_2))
                    currentPersonality_2 = idx;

                ImGuiUtil.SelectableHelpMarker(newDesc);
            }
        }
        public void AddPersonalitySelection_3()
        {
            var (text, desc) = Constants.PersonalityValues[currentPersonality_3];
            using var combo = ImRaii.Combo("##Personality Feature #3", text);
            ImGuiUtil.HoverTooltip(desc);
            if (!combo)
                return;

            foreach (var ((newText, newDesc), idx) in Constants.PersonalityValues.WithIndex())
            {
                if (ImGui.Selectable(newText, idx == currentPersonality_3))
                    currentPersonality_3 = idx;

                ImGuiUtil.SelectableHelpMarker(newDesc);
            }
        }
        public override void Update()
        {
            ExistingProfile = DataReceiver.ExistingProfileData;
            ExistingStory = DataReceiver.ExistingStoryData;
            ExistingBio = DataReceiver.ExistingBioData;
            ExistingHooks = DataReceiver.ExistingHooksData;
            ExistingGallery = DataReceiver.ExistingGalleryData;
            existingAvatarBytes = DataReceiver.currentAvatar;
            loadedSelf = DataReceiver.LoadedSelf;
            

          
           

           /* if (editBio == true)
            {
                for (int i = 0; i < alignmentWidthVals.Length; i++)
                {
                    if (alignmentWidthVals[i] < alignmentVals[i]) { alignmentWidthVals[i] += 0.1f; }
                    if (alignmentWidthVals[i] > alignmentVals[i]) { alignmentWidthVals[i] -= 0.1f; }
                }
            }*/

            if (DataReceiver.StoryLoadStatus != -1 &&
               DataReceiver.HooksLoadStatus != -1 &&
               DataReceiver.BioLoadStatus != -1 &&
               DataReceiver.GalleryLoadStatus != -1
               )
            {

                AllLoaded = true;
            }
            else
            {
                AllLoaded = false;
            }



        }
        
      
        public int AvailablePercentageLeft(int lawful_good, int neutral_good, int chaotic_good, int lawful_neutral, int true_neutral, int chaotic_neutral, int lawful_evil, int neutral_evil, int chaotic_evil)
        {
            int PercentageUsed = lawful_good + neutral_good + chaotic_good + lawful_neutral + true_neutral + chaotic_neutral + lawful_evil + neutral_evil + chaotic_evil;
            int percentageLeft = availablePercentage - PercentageUsed;
            return percentageLeft;
        }
       /* public void ModAlignment(string alignmentName, bool add) 
        {
            for(int i =0; i < alignmentNames.Length; i++)
            {
                if (alignmentNames[i] == alignmentName)
                {
                    if(ExistingBio == true)
                    {
                        if (add)
                        {
                            if(availablePercentage > 0 && alignmentVals[i] < 10)
                            {
                                availablePercentage -= 1;
                                alignmentVals[i] += 1;
                            }
                        }
                        else
                        {
                            if (alignmentVals[i] > 0)
                            {
                                availablePercentage +=1;
                                alignmentVals[i] -= 1;
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


        }*/
      
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
                   

                });
            }, 0, null, this.configuration.AlwaysOpenDefaultImport);
        }

       
    }
}
