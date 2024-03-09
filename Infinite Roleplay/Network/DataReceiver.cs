
using Dalamud.Hooking;
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Common.Math;
using InfiniteRoleplay;
using InfiniteRoleplay.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using InfiniteRoleplay.Helpers;
using ImGuiNET;
using InfiniteRoleplay.Scripts.Misc;
using static FFXIVClientStructs.FFXIV.Client.UI.Misc.GroupPoseModule;
using System.Reflection;
using System.Xml.Linq;
using ImGuiScene;

namespace Networking
{

    //Packets that can be received from the server (Must match server packet number on server)
    public enum ServerPackets
    {
        SWelcomeMessage = 1,
        SRecLoginStatus = 2,
        SRecAccPermissions = 3,
        SRecProfileBio = 4,
        SRecExistingProfile = 5,
        SSendProfile = 20,
        SDoneSending = 21,
        SNoProfileBio = 22,
        SNoProfile = 23,
        SSendProfileHook = 24,
        SSendNoProfileHooks = 25,
        SRecNoTargetHooks = 26,
        SRecNoTargetBio = 27,
        SRecTargetHooks = 28,
        SRecTargetBio = 29,
        SRecTargetProfile = 30,
        SRecNoTargetProfile = 31,
        SRecProfileStory = 32,
        SRecTargetStory = 33,
        SRecBookmarks = 34,
        SRecNoTargetStory = 35,
        SRecNoProfileStory = 36,
        SRecProfileGallery = 37,
        SRecGalleryImageLoaded = 38,
        SRecImageDeletionStatus = 39,
        SRecNoTargetGallery = 40,
        SRecTargetGallery = 41,
        SRecNoProfileGallery = 42,
        CProfileAlreadyReported = 43,
        CProfileReportedSuccessfully = 44,
        SSendProfileNotes = 45,
        SSendNoProfileNotes = 46,
        SSendNoAuthorization = 47,
        SSendVerificationMessage = 48,
    }
    class DataReceiver
    {
        public static string accountStatus = "status...";
        public static bool LoadedSelf = false;
        public static bool ExistingProfileData, ExistingProfileNotes, ExistingBioData, ExistingHooksData, ExistingStoryData, ExistingOOCData, ExistingGalleryData = false;
        public static int BioLoadStatus = -1, HooksLoadStatus = -1, StoryLoadStatus = -1, OOCLoadStatus = -1, GalleryLoadStatus = -1, BookmarkLoadStatus = -1;
        public static int TargetBioLoadStatus = -1, TargetHooksLoadStatus = -1, TargetStoryLoadStatus = -1, TargetOOCLoadStatus = -1, TargetGalleryLoadStatus = -1, TargetNotesLoadStatus = -1;
        public static bool ExistingTargetProfileData, ExistingTargetBioData, ExistingTargetHooksData, ExistingTargetStoryData, ExistingTargetOOCData, ExistingTargetGalleryData = false;
        public static string bookmarks;
        public static byte[] currentAvatar , currentTargetAvatar;
        public static byte[][] ExistingTargetGalleryImageBytes, ExistingTargetGalleryThumbBytes;
        public static int hookEditCount, hookCount;
        public static int targetHookEditCount, ExistingGalleryImageCount, ExistingGalleryThumbCount;
        public static int lawfulGoodEditVal, neutralGoodEditVal, chaoticGoodEditVal, 
                          lawfulNeutralEditVal, trueNeutralEditVal, chaoticNeutralEditVal, 
                          lawfulEvilEditVal, neutralEvilEditVal, chaoticEvilEditVal;
        public static string currentName, currentRace, currentGender,currentAge, currentHeight,currentWeight,currentAfg;

        public static Vector4 accounStatusColor = new Vector4(255, 255, 255, 255);
        public static Plugin plugin;
        public static Dictionary<int, string> characters = new Dictionary<int, string>();
        public static Dictionary<int, string> adminCharacters = new Dictionary<int, string>();
        public static Dictionary<int, byte[]> adminCharacterAvatars = new Dictionary<int, byte[]>();
        public static SortedList<int, string> pages = new SortedList<int, string>();
        public static SortedList<string, string> pagesContent = new SortedList<string, string>();
        public static Dictionary<int, string> profiles = new Dictionary<int, string>();
        public static Dictionary<int, byte[]> characterAvatars = new Dictionary<int, byte[]>();
        public static Dictionary<int, int> characterVerificationStatuses = new Dictionary<int, int>();
        public static string ConnectionMsg;
        public static bool loggedIn;
        public static bool isAdmin;

        // public NSWorld.World world = new NSWorld.World();
        //EXAMPLE PACKET//
        /*
         public static void ExampleRecPacket(byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer(); //create a new buffer (always the same)
            buffer.WriteBytes(data); //write the bytes of the data sent from the packet into the buffer (always the same)
            int packetID = buffer.ReadInt(); //packetID is simply the id of the packet sent from server (always the same)
            //CAN BE ANY BUFFER FROM THE ByteBuffer SCRIPT. MUT MATCH DATA SENT FROM SERVER
            //      types are:
            //      ReadByte
            //      ReadBytes
            //      ReadShort
            //      ReadInt
            //      ReadLong
            //      ReadFloat
            //      ReadBool
            //      ReadString
        
            string msg = buffer.ReadString(); //example buffer data from server.
            buffer.Dispose(); //dispose of our buffer
            Debug.Log(msg); //log our message from server if wanted in the console window or do something else with the data received.   
        }
         
         
         
         */
        public static void RecPermissions(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int permissions = buffer.ReadInt();
            if(permissions == 1)
            {
                isAdmin = true;
            }
            else
            {
                isAdmin = false;
            }
        }
        public static void RecRulebook(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int ruleBookPageID = buffer.ReadInt();
            string ruleBookPageTitle = buffer.ReadString();           
            
            pages.Add(ruleBookPageID, ruleBookPageTitle);
            
            
        }
        public static void RecBookmarks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string bookmarkVals = buffer.ReadString();

            plugin.bookmarksWindow.IsOpen = true;
            
            Regex nameRx = new Regex(@"<bookmarkName>(.*?)</bookmarkName>");
            Regex worldRx = new Regex(@"<bookmarkWorld>(.*?)</bookmarkWorld>");
            string[] bookmarkSplit = bookmarkVals.Replace("|||", "~").Split('~');
            BookmarksWindow.profiles.Clear();
            for (int i = 0; i < bookmarkSplit.Count(); i++)
            {
                string characterName = nameRx.Match(bookmarkSplit[i]).Groups[1].Value;
                string characterWorld = worldRx.Match(bookmarkSplit[i]).Groups[1].Value;

                BookmarksWindow.profiles.Add(characterName, characterWorld);
            }
            BookmarkLoadStatus = 1;
        }
        public static void RecRulebookContent(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int ruleBookPageID = buffer.ReadInt();
            string ruleBookCategoryTitle = buffer.ReadString();
            string ruleBookCategoryContent = buffer.ReadString();

            string ruleBookContentID = ruleBookCategoryTitle + "," + ruleBookPageID;
            string ruleBookCategory = ruleBookCategoryTitle + "," + ruleBookCategoryContent;
           
            if (!pagesContent.ContainsKey(ruleBookContentID))
            {
                pagesContent.Add(ruleBookContentID, ruleBookCategory);
            }
            
        }
        public static void HandleWelcomeMessage(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            var msg = buffer.ReadString();
            ConnectionMsg = msg;
            buffer.Dispose();
          
        }

        public static void HandleVersionRequest(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
          
        }
        public static void ImageLoaded(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int index = buffer.ReadInt();
            buffer.Dispose();
        }
        public static void BadLogin(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            var profiles = buffer.ReadString();
            buffer.Dispose();
        }
        public static void ExistingTargetProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            ExistingTargetProfileData = true;
            plugin.targetWindow.IsOpen = true;
            TargetWindow.ClearUI();
            ReportWindow.reportStatus = "";
        }
        public static void RecProfileReportedSuccessfully(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            ReportWindow.reportStatus = "Profile reported successfully. We are on it!";
        }
        public static void RecProfileAlreadyReported(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            ReportWindow.reportStatus = "Profile has already been reported!";

        }
        public static void NoProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            BioLoadStatus = 0;
            HooksLoadStatus = 0;
            StoryLoadStatus = 0;
            OOCLoadStatus = 0;
            GalleryLoadStatus = 0;
            BookmarkLoadStatus = 0;
            ProfileWindow.addProfile = false;
            ProfileWindow.editProfile = false;
            ExistingProfileData = false;
        }
        public static void NoTargetProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            ExistingTargetProfileData = false;
            ExistingTargetBioData = false;
            ExistingTargetHooksData = false;
            ExistingTargetStoryData = false;
            ExistingTargetOOCData = false;
            ExistingTargetGalleryData = false;
            plugin.targetWindow.IsOpen = true;
            TargetBioLoadStatus = 0;
            TargetHooksLoadStatus = 0;
            TargetStoryLoadStatus = 0;
            TargetOOCLoadStatus = 0;
            TargetGalleryLoadStatus = 0;
            TargetNotesLoadStatus = 0;
            TargetWindow.ClearUI();
            TargetMenu.DisableInput = false;
            BookmarksWindow.DisableBookmarkSelection = false;
            ReportWindow.reportStatus = "";
        }
        public static void NoTargetGallery(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            ExistingTargetGalleryData = false;
            plugin.targetWindow.IsOpen = true;
            BookmarksWindow.DisableBookmarkSelection = false;
            TargetMenu.DisableInput = false;
            TargetGalleryLoadStatus = 0;
        }
        public static void NoTargetStory(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            ExistingTargetStoryData = false;
            plugin.targetWindow.IsOpen = true;
            TargetStoryLoadStatus = 0;
        }
        


        public static void NoProfileBio(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();

            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.name] = "";
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.race] = "";
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.gender] = "";
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.age] = "";
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.height] = "";
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.weight] = "";
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.afg] = "";
            ProfileWindow.currentAlignment = 0;

            ProfileWindow.currentPersonality_1 = 0;
            ProfileWindow.currentPersonality_2 = 0;
            ProfileWindow.currentPersonality_3 = 0;
            loggedIn = true;
            ExistingBioData = false;
            BioLoadStatus = 0;



        }
        public static void NoTargetBio(byte[] data)
        {
            ExistingTargetBioData = false;
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            TargetBioLoadStatus = 0;
        }
       
        public static void NoTargetHooks(byte[] data)
        {
            ExistingTargetHooksData = false;
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            TargetHooksLoadStatus = 0;
        }
        public static void ReceiveProfile(byte[] data)
        {
            ExistingProfileData = true;
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int profileID = buffer.ReadInt();
            string profileName = buffer.ReadString();
            buffer.Dispose();     
            loggedIn = true;
           
        }
        public static void ReceiveVerificationUpdate(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int sheetID = buffer.ReadInt();
            int status = buffer.ReadInt();
            buffer.Dispose();
            if (characters.ContainsKey(sheetID))
            {
                characterVerificationStatuses[sheetID] = status;
            }
        }
        public static void SystemCreated(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
        }
        public static void LoginAuthenticated(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int status = buffer.ReadInt();
            buffer.Dispose();

             if(status == -1)
             {
                plugin.loggedIn = false;
                accounStatusColor = new Vector4(255, 0, 0, 255);
                accountStatus = "Account Banned";
             }
             if(status == 0)
             {
                plugin.loggedIn = false;
                accounStatusColor = new Vector4(255, 255, 0, 255);
                accountStatus = "Inactive Account";
             }
             if (status == 1)
             {
                plugin.loggedIn = true;
                plugin.CloseAllWindows();
                plugin.panelWindow.IsOpen = true;                
                 
             }

        }


        public static void ReceiveTargetProfileGallery(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int profileID = buffer.ReadInt();
            int imageCount = buffer.ReadInt();
            ExistingTargetGalleryData = true;
            for (int i = 0; i < imageCount; i++)
            {
                string url = buffer.ReadString();
                bool nsfw = buffer.ReadBool();
                Imaging.DownloadTargetProfileImage(url, profileID, nsfw, plugin, i);
                //ProfileWindow.ReorderNoSend = true;
            }
            BookmarksWindow.DisableBookmarkSelection = false;
            TargetMenu.DisableInput = false;
            
            TargetGalleryLoadStatus = 1;
            buffer.Dispose();

        }
        public static void ReceiveNoProfileGallery(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            for (int i = 0; i < 30; i++)
            {
                ProfileWindow.galleryImages[i] = ProfileWindow.pictureTab;
                ProfileWindow.imageURLs[i] = string.Empty;
            }
            GalleryLoadStatus = 0;
            ExistingGalleryData = false;
        }
        public static void ReceiveProfileGalleryImage(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int imageCount = buffer.ReadInt();
            int profileID = buffer.ReadInt();
          
            for (int i = 0; i < imageCount; i++)
            {
                string url = buffer.ReadString();
                bool nsfw = buffer.ReadBool();
                Imaging.DownloadProfileImage(url, profileID, nsfw, plugin.PluginInterfacePub, plugin, i);
                ProfileWindow.imageIndex = i + 2;
                ProfileWindow.ImageExists[i] = true;
                plugin.chatGUI.Print(i.ToString());
            }
            ExistingGalleryData = true;
       
            GalleryLoadStatus = 1;
            buffer.Dispose();

        }
        public static void ReceiveTargetBio(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();

            int avatarLen = buffer.ReadInt();
            byte[] avatarBytes = buffer.ReadBytes(avatarLen);
            string name = buffer.ReadString();
            string race = buffer.ReadString();
            string gender = buffer.ReadString();
            int age = buffer.ReadInt();
            string height = buffer.ReadString();
            string weight = buffer.ReadString();
            string atFirstGlance = buffer.ReadString();
            int alignment = buffer.ReadInt();
            int personality_1 = buffer.ReadInt();
            int personality_2 = buffer.ReadInt();
            int personality_3 = buffer.ReadInt();
            TargetWindow.characterEditName = name; TargetWindow.characterEditRace = race; TargetWindow.characterEditGender = gender;
            TargetWindow.characterEditAge = age.ToString(); TargetWindow.characterEditHeight = height; TargetWindow.characterEditWeight = weight.ToString();
            TargetWindow.characterEditAfg = atFirstGlance;
            TargetWindow.alignmentImg = Constants.AlignementIcon(plugin.PluginInterfacePub, alignment);
            TargetWindow.personalityImg1 = Constants.PersonalityIcon(plugin.PluginInterfacePub, personality_1);
            TargetWindow.personalityImg2 = Constants.PersonalityIcon(plugin.PluginInterfacePub, personality_2);
            TargetWindow.personalityImg3 = Constants.PersonalityIcon(plugin.PluginInterfacePub, personality_3);

            var (text, desc) = Constants.AlignmentVals[alignment];
            var (textpers1, descpers1) = Constants.PersonalityValues[personality_1];
            var (textpers2, descpers2) = Constants.PersonalityValues[personality_2];
            var (textpers3, descpers3) = Constants.PersonalityValues[personality_3];
            TargetWindow.alignmentTooltip = text + ": \n" + desc;
            TargetWindow.personality1Tooltip = textpers1 + ": \n" + descpers1;
            TargetWindow.personality2Tooltip = textpers2 + ": \n" + descpers2;
            TargetWindow.personality3Tooltip = textpers3 + ": \n" + descpers3;

            currentTargetAvatar = avatarBytes;
            ExistingTargetBioData = true;
            buffer.Dispose();
            TargetBioLoadStatus = 1;
            plugin.targetWindow.IsOpen = true;
        }
        public static void ReceiveProfileBio(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();

            int avatarLen = buffer.ReadInt();
            byte[] avatarBytes = buffer.ReadBytes(avatarLen);
            string name = buffer.ReadString();
            string race = buffer.ReadString();
            string gender = buffer.ReadString();
            int age = buffer.ReadInt();
            string height = buffer.ReadString();
            string weight = buffer.ReadString();
            string atFirstGlance = buffer.ReadString();
            int alignment = buffer.ReadInt();
            int personality_1 = buffer.ReadInt();
            int personality_2 = buffer.ReadInt();
            int personality_3 = buffer.ReadInt();
            ExistingBioData = true;
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.name] = name.Replace("''", "'");
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.race] = race.Replace("''", "'");
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.gender] = gender.Replace("''", "'");
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.age] = age.ToString();
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.height] = height.Replace("''", "'");
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.weight] = weight.Replace("''", "'");
            ProfileWindow.bioFieldsArr[(int)Constants.BioFieldTypes.afg] = atFirstGlance.Replace("''", "'");
            ProfileWindow.currentAlignment = alignment;

            ProfileWindow.currentPersonality_1 = personality_1;
            ProfileWindow.currentPersonality_2 = personality_2;
            ProfileWindow.currentPersonality_3 = personality_3;
            buffer.Dispose();
            currentAvatar = avatarBytes;
            BioLoadStatus = 1;
        }
        public static void ExistingProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            ExistingProfileData = true;

        }
        public static void ReceiveProfileHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string hooks = buffer.ReadString();
            ExistingHooksData = true;

            Regex hookRx = new Regex(@"<hook>(.*?)</hook>");
            string[] hookSplit = hooks.Replace("|||", "~").Split('~');

            for (int i = 0; i < hookSplit.Count(); i++)
            {
                string hookContent = hookRx.Match(hookSplit[i]).Groups[1].Value;
                ProfileWindow.hookEditCount = i;
                ProfileWindow.resetHooks = true;
                ProfileWindow.HookEditContent[i] = hookContent.Replace("---===---", "\n").Replace("''", "'");

            }
            buffer.Dispose();
            HooksLoadStatus = 1;
        }
      
        public static void ReceiveProfileStory(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string storyTitle = buffer.ReadString();
            string chaptersMsg = buffer.ReadString();
            ExistingStoryData = true;

            Regex chapterTitleRx = new Regex(@"<chapter_title>(.*?)</chapter_title>");
            Regex chapterRx = new Regex(@"<chapter>(.*?)</chapter>");
            string[] chaptersSplit = chaptersMsg.Replace("|||", "~").Split('~');

            ProfileWindow.storyEditTitle = storyTitle;
            for (int i = 0; i < chaptersSplit.Count(); i++)
            {
                string hookContent = chapterRx.Match(chaptersSplit[i]).Groups[1].Value;
                string hookTitle = chapterTitleRx.Match(chaptersSplit[i]).Groups[1].Value;
                ProfileWindow.chapterEditCount = i;
                ProfileWindow.resetStory = true;
                ProfileWindow.ChapterEditTitle[i] = hookTitle;
                ProfileWindow.ChapterEditContent[i] = hookContent.Replace("---===---", "\n").Replace("''", "'");
            }
            buffer.Dispose();
            StoryLoadStatus = 1;
        }

        public static void ReceiveTargetStory(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string storyTitle = buffer.ReadString();
            string chapters = buffer.ReadString();
            ExistingTargetStoryData = true;
            Regex chapterTitleRx = new Regex(@"<chapter_title>(.*?)</chapter_title>");
            Regex chapterRx = new Regex(@"<chapter>(.*?)</chapter>");
            string[] chapterSplit = chapters.Replace("|||", "~").Split('~');
            TargetWindow.storyTitle = storyTitle;
            for (int i = 0; i < chapterSplit.Count(); i++)
            {
                string chapterTitle = chapterTitleRx.Match(chapterSplit[i]).Groups[1].Value;
                string chapterContent = chapterRx.Match(chapterSplit[i]).Groups[1].Value;
                TargetWindow.chapterCount = i;
                TargetWindow.resetStory = true;
                TargetWindow.ChapterTitle[i] = chapterTitle;
                TargetWindow.ChapterContent[i] = chapterContent;
            }
            buffer.Dispose();
            TargetStoryLoadStatus = 1;
        }
        public static void ReceiveTargetHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string hooks = buffer.ReadString();
            ExistingTargetHooksData = true;

            Regex hookRx = new Regex(@"<hook>(.*?)</hook>");
            string[] hookSplit = hooks.Replace("|||", "~").Split('~');

            for (int i = 0; i < hookSplit.Count(); i++)
            {
                string hookContent = hookRx.Match(hookSplit[i]).Groups[1].Value;
                targetHookEditCount = i;
                TargetWindow.HookEditContent[i] = hookContent;
            }
            buffer.Dispose();
            TargetHooksLoadStatus = 1;
        }
        public static void NoProfileHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            ExistingHooksData = false;

            buffer.Dispose();
            HooksLoadStatus = 0;
        }
        public static void NoProfileStory(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            ExistingStoryData = false;
            buffer.Dispose();
            StoryLoadStatus = 0;
        }
        public static void NoProfileNotes(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            TargetWindow.profileNotes = string.Empty;
            ExistingProfileNotes = false;
            TargetNotesLoadStatus = 0;

            buffer.Dispose();
        }
        public static void RecProfileNotes(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string notes = buffer.ReadString();
            ExistingProfileNotes = true;
            TargetWindow.profileNotes = notes;
            buffer.Dispose();
            TargetNotesLoadStatus = 1;
        }

        public static void ReceiveNoAuthorization(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            PanelWindow.statusCol = new Vector4(1, 0, 0, 1);
            PanelWindow.status = "Unauthorized Access to Profile.";
            buffer.Dispose();
        }
        public static void ReceiveVerificationMessage(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            plugin.verificationWindow.IsOpen = true;
            buffer.Dispose();
        }
    }
}
