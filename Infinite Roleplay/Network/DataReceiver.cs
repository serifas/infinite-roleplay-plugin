
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
using System.Resources;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

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
    }
    class DataReceiver
    {
        public static string accountStatus = "status...";
        public static bool LoadedSelf = false;
        public static bool ExistingProfileData = false, ExistingTargetProfileData = false, targetBioData = false, ExistingStory = false, ExistingTargetStory;
        public static string bookmarks;
        public static byte[] currentAvatar , currentTargetAvatar;
        public static int hookEditCount, hookCount;
        public static int targetHookEditCount;
        public static int lawfulGoodEditVal, neutralGoodEditVal, chaoticGoodEditVal, 
                          lawfulNeutralEditVal, trueNeutralEditVal, chaoticNeutralEditVal, 
                          lawfulEvilEditVal, neutralEvilEditVal, chaoticEvilEditVal;
        public static string currentName, currentRace, currentGender,currentAge, currentHeight,currentWeight,currentAfg;

        public static bool ExistingBioData, ExistingHooks = false, ExistingTargetBioData, ExistingTargetHooks;
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
            plugin.WindowSystem.GetWindow("BOOKMARKS").IsOpen = true;

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

            plugin.WindowSystem.GetWindow("TARGET").IsOpen = true;
        }
        public static void ExistingProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            ExistingProfileData = true;
            
            plugin.WindowSystem.GetWindow("PROFILES").IsOpen = true;

        }
        public static void NoProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            ExistingProfileData = false;
            plugin.WindowSystem.GetWindow("PROFILES").IsOpen = true;
        }
        public static void NoTargetProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            ExistingTargetProfileData = false;
            plugin.WindowSystem.GetWindow("TARGET").IsOpen = true;
        }
        public static void NoProfileBio(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            ExistingBioData = false;
            plugin.WindowSystem.GetWindow("PROFILES").IsOpen = true;
        }
        public static void NoTargetBio(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            loggedIn = true;
            targetBioData = false;
        }
        public static void ReceiveProfile(byte[] data)
        {
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

            plugin.loggedIn = true;
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
                 plugin.WindowSystem.GetWindow("LOGIN").IsOpen = false;
                 plugin.WindowSystem.GetWindow("OPTIONS").IsOpen = true;
                 plugin.loggedIn = true;
             }

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
            int lawful_good = buffer.ReadInt();
            int neutral_good = buffer.ReadInt();
            int chaotic_good = buffer.ReadInt();
            int lawful_neutral = buffer.ReadInt();
            int true_neutral = buffer.ReadInt();
            int chaotic_neutral = buffer.ReadInt();
            int lawful_evil = buffer.ReadInt();
            int neutral_evil = buffer.ReadInt();
            int chaotic_evil = buffer.ReadInt();
            TargetWindow.characterEditName = name; TargetWindow.characterEditRace = race; TargetWindow.characterEditGender = gender;
            TargetWindow.characterEditAge = age.ToString(); TargetWindow.characterEditHeight = height; TargetWindow.characterEditWeight = weight.ToString(); 
            TargetWindow.characterEditAfg = atFirstGlance;

            TargetWindow.alignmentEditVals[0] = lawful_good;
            TargetWindow.alignmentEditVals[1] = neutral_good;
            TargetWindow.alignmentEditVals[2] = chaotic_good;
            TargetWindow.alignmentEditVals[3] = lawful_neutral;
            TargetWindow.alignmentEditVals[4] = true_neutral;
            TargetWindow.alignmentEditVals[5] = chaotic_neutral;
            TargetWindow.alignmentEditVals[6] =  lawful_evil;
            TargetWindow.alignmentEditVals[7] = neutral_evil;
            TargetWindow.alignmentEditVals[8] = chaotic_evil;


            currentTargetAvatar = avatarBytes;
            ExistingTargetBioData = true;
            buffer.Dispose();

            plugin.WindowSystem.GetWindow("TARGET").IsOpen = true;
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
            int lawful_good = buffer.ReadInt();
            int neutral_good = buffer.ReadInt();
            int chaotic_good = buffer.ReadInt();
            int lawful_neutral = buffer.ReadInt();
            int true_neutral = buffer.ReadInt();
            int chaotic_neutral = buffer.ReadInt();
            int lawful_evil = buffer.ReadInt();
            int neutral_evil = buffer.ReadInt();
            int chaotic_evil = buffer.ReadInt();
            
            ProfileWindow.characterEditName = name.Replace("''", "'"); ProfileWindow.characterEditRace = race.Replace("''", "'"); ProfileWindow.characterEditGender = gender.Replace("''", "'");
            ProfileWindow.characterEditAge = age.ToString().Replace("''", "'"); ProfileWindow.characterEditHeight = height.Replace("''", "'"); ProfileWindow.characterEditWeight = weight.ToString().Replace("''", "'");
            ProfileWindow.characterEditAfg = atFirstGlance;

            ProfileWindow.alignmentEditVals[0] = lawful_good;
            ProfileWindow.alignmentEditVals[1] = neutral_good;
            ProfileWindow.alignmentEditVals[2] = chaotic_good;
            ProfileWindow.alignmentEditVals[3] = lawful_neutral;
            ProfileWindow.alignmentEditVals[4] = true_neutral;
            ProfileWindow.alignmentEditVals[5] = chaotic_neutral;
            ProfileWindow.alignmentEditVals[6] = lawful_evil;
            ProfileWindow.alignmentEditVals[7] = neutral_evil;
            ProfileWindow.alignmentEditVals[8] = chaotic_evil;

            currentAvatar = avatarBytes;
            ExistingBioData = true;
            buffer.Dispose();

            plugin.WindowSystem.GetWindow("PROFILES").IsOpen = true;
        }
        public static void ReceiveProfileHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string hooks = buffer.ReadString();
            ExistingHooks = true;

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
        }
        public static void ReceiveProfileStory(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string storyTitle = buffer.ReadString();
            string chaptersMsg = buffer.ReadString();
            ExistingStory = true;

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
        }

        public static void ReceiveTargetStory(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string storyTitle = buffer.ReadString();
            string chapters = buffer.ReadString();
            ExistingTargetStory = true;
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
        }
        public static void ReceiveTargetHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            string hooks = buffer.ReadString();
            ExistingTargetHooks = true;

            Regex hookRx = new Regex(@"<hook>(.*?)</hook>");
            string[] hookSplit = hooks.Replace("|||", "~").Split('~');

            for (int i = 0; i < hookSplit.Count(); i++)
            {
                string hookContent = hookRx.Match(hookSplit[i]).Groups[1].Value;
                targetHookEditCount = i;
                TargetWindow.HookEditContent[i] = hookContent;
            }
            buffer.Dispose();
        }
        public static void NoProfileHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            ExistingHooks = false;
            buffer.Dispose();
        }



        public static void NoTargetProfileHooks(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            ExistingTargetHooks = false;
            buffer.Dispose();
        }
    }
}
