
using Dalamud.Interface.Windowing;
using FFXIVClientStructs.FFXIV.Common.Math;
using InfiniteRoleplay;
using InfiniteRoleplay.Utils;
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
using System.Threading.Tasks;
using UpdateTest;
using Windows.Devices.HumanInterfaceDevice;
using static System.Net.Mime.MediaTypeNames;

namespace UpdateTest
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
    }
    class DataReceiver
    {
        public static string accountStatus = "status...";
        public static bool ExistingProfileData = false;
        public static byte[] currentAvatar;
        public static int lawfulGoodEditVal, neutralGoodEditVal, chaoticGoodEditVal, 
                          lawfulNeutralEditVal, trueNeutralEditVal, chaoticNeutralEditVal, 
                          lawfulEvilEditVal, neutralEvilEditVal, chaoticEvilEditVal;
        public static string currentName, currentRace, currentGender,currentAge, currentHeight,currentWeight,currentAfg;

        public static bool ExistingBioData = false;
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
        public static void BadLogin(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            var profiles = buffer.ReadString();
            buffer.Dispose();
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
            ProfileWindow.characterEditName = name; ProfileWindow.characterEditRace = race; ProfileWindow.characterEditGender = gender;
            ProfileWindow.characterEditAge = age.ToString(); ProfileWindow.characterEditHeight = height; ProfileWindow.characterEditWeight = weight.ToString(); 
            ProfileWindow.characterEditAfg = atFirstGlance;

            ProfileWindow.lawfulGoodEditVal = lawful_good;
            ProfileWindow.neutralGoodEditVal = neutral_good;
            ProfileWindow.chaoticGoodEditVal = chaotic_good;
            ProfileWindow.lawfulNeutralEditVal = lawful_neutral;
            ProfileWindow.trueNeutralEditVal = true_neutral;
            ProfileWindow.chaoticNeutralEditVal = chaotic_neutral;
            ProfileWindow.lawfulEvilEditVal =  lawful_evil;
            ProfileWindow.neutralEvilEditVal = neutral_evil;
            ProfileWindow.chaoticEvilEditVal = chaotic_evil;


            currentAvatar = avatarBytes;
            ExistingBioData = true;
            buffer.Dispose();

        }
        
        

        

    }
}
