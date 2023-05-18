
using FFXIVClientStructs.FFXIV.Common.Math;
using InfiniteRoleplay;
using InfiniteRoleplay.Utils;
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
using static System.Net.Mime.MediaTypeNames;

namespace UpdateTest
{

    //Packets that can be received from the server (Must match server packet number on server)
    public enum ServerPackets
    {
        SWelcomeMessage = 1,
        SRecLoginStatus = 2,
        SRecAccPermissions = 3,
    }
    class DataReceiver
    {
        public static string accountStatus = "status...";
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
          
        }
        public static void NoProfile(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
          
            loggedIn = true;
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
        
        public static void AccountInfo(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int sheetID = buffer.ReadInt();
            string player_name = buffer.ReadString();
            string character_name = buffer.ReadString();
            int avLength = buffer.ReadInt();
            byte[] avatarBytes = buffer.ReadBytes(avLength);
            string race = buffer.ReadString();
            string age = buffer.ReadString();
            string height = buffer.ReadString();
            string weight = buffer.ReadString();
            int health = buffer.ReadInt();
            int senses = buffer.ReadInt();
            int strength = buffer.ReadInt();
            int hardiness = buffer.ReadInt();
            int intelligence = buffer.ReadInt();
            int nimbleness = buffer.ReadInt();
            int eminence = buffer.ReadInt();
            string ability_1 = buffer.ReadString();
            string ability_2 = buffer.ReadString();
            string ability_3 = buffer.ReadString();
            string ability_1_description = buffer.ReadString();
            string ability_2_description = buffer.ReadString();
            string ability_3_description = buffer.ReadString();
            int verification_status = buffer.ReadInt();
            buffer.Dispose();
            if (sheetID != 0)
            {

                string character = sheetID + "," + player_name + "," + character_name + "," + race + "," + age + "," + height + "," + weight + "," + health + "," + senses + "," + strength + "," + hardiness + "," + intelligence + "," + nimbleness + "," + eminence + "," + ability_1 + "," + ability_2 + "," + ability_3 + "," + ability_1_description + "," + ability_2_description + "," + ability_3_description;
                if (!characters.ContainsKey(sheetID))
                {
                    characters.Add(sheetID, character);
                    characterAvatars.Add(sheetID, avatarBytes);
                    characterVerificationStatuses.Add(sheetID, verification_status);
                }
            }
      
        }

        public static void UserProfileDataAdmin(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            int sheetID = buffer.ReadInt();
            string player_name = buffer.ReadString();
            string character_name = buffer.ReadString();
            int avLength = buffer.ReadInt();
            byte[] avatarBytes = buffer.ReadBytes(avLength);
            string race = buffer.ReadString();
            string age = buffer.ReadString();
            string height = buffer.ReadString();
            string weight = buffer.ReadString();
            int health = buffer.ReadInt();
            int senses = buffer.ReadInt();
            int strength = buffer.ReadInt();
            int hardiness = buffer.ReadInt();
            int intelligence = buffer.ReadInt();
            int nimbleness = buffer.ReadInt();
            int eminence = buffer.ReadInt();
            string ability_1 = buffer.ReadString();
            string ability_2 = buffer.ReadString();
            string ability_3 = buffer.ReadString();
            string ability_1_description = buffer.ReadString();
            string ability_2_description = buffer.ReadString();
            string ability_3_description = buffer.ReadString();
            int verification_status = buffer.ReadInt();
            buffer.Dispose();
            if (sheetID != 0)
            {
                string character = sheetID + "," + player_name + "," + character_name + "," + race + "," + age + "," + height + "," + weight + "," + health + "," + senses + "," + strength + "," + hardiness + "," + intelligence + "," + nimbleness + "," + eminence + "," + ability_1 + "," + ability_2 + "," + ability_3 + "," + ability_1_description + "," + ability_2_description + "," + ability_3_description + "," + verification_status;
                if (!adminCharacters.ContainsKey(sheetID))
                {
                    adminCharacters.Add(sheetID, character);
                    adminCharacterAvatars.Add(sheetID, avatarBytes);
                }
                else
                {
                    adminCharacters[sheetID] = character;
                }
            }
        }

    }
}
