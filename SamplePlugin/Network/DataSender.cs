
using Dalamud.Hooking;
using FFXIVClientStructs.FFXIV.Client.Graphics.Scene;
using InfiniteRoleplay;
using Lumina.Excel.GeneratedSheets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace UpdateTest
{
    public enum ClientPackets
    {
        CHelloServer = 1,
        CLogin = 2,
        CCreateProfile = 3,
        CFetchProfiles = 4,
        CSendNewSystem = 5,
        CSendRulebookPageContent = 6,
        CSendRulebookPage = 7,
        CSendSheetVerify = 8,
        CSendSystemStats = 9,
        CCreateProfileBio = 10,
        CBanAccount = 11,
        CStrikeAccount = 12,
        CEditProfileBio = 13,
        CSendHooks = 14,
        SRequestTargetProfile = 15,
        CRegister = 16,
        CDeleteHook = 17,
        CSendStory = 18,
    }
    public class DataSender
    {
        public static int userID;
        // public static LoadCharacter assets = new LoadCharacter();
        public static void SendHelloServer()
        {                     
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CHelloServer);
            buffer.WriteString("===New Connection===");
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void Login(string username, string password)
        {

            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CLogin);
            buffer.WriteString(username);
            buffer.WriteString(password);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void Register(string username, string password)
        {

            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CRegister);
            buffer.WriteString(username);
            buffer.WriteString(password);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void SendSystemStats(string username, string SystemName, string Msg)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendSystemStats);
            buffer.WriteString(username);
            buffer.WriteString(SystemName);
            buffer.WriteString(Msg);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void SendStory(string username, string worldname, string title, string chapters)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendSystemStats);
            buffer.WriteString(username);
            buffer.WriteString(worldname);
            buffer.WriteString(title);
            buffer.WriteString(chapters);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void SendNewSystem(string username, string name, string description, byte[] systemImage, int max_stats, int max_stat_points_per_stat, int max_stat_reduction, int max_stat_reduction_per_stat, int stat_allocation_allowed, int stat_reduction_allowed)
        {

            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendNewSystem);
            buffer.WriteString(username);
            buffer.WriteString(name);
            buffer.WriteString(description);
            buffer.WriteInteger(systemImage.Length);
            buffer.WriteBytes(systemImage);
            buffer.WriteInteger(max_stats);
            buffer.WriteInteger(max_stat_points_per_stat);
            buffer.WriteInteger(max_stat_reduction);
            buffer.WriteInteger(max_stat_reduction_per_stat);
            buffer.WriteInteger(stat_allocation_allowed);
            buffer.WriteInteger(stat_reduction_allowed);
           
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void FetchProfile(string username, string characterName, string world)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CFetchProfiles);
            buffer.WriteString(username);
            buffer.WriteString(characterName);
            buffer.WriteString(world);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void CreateProfile(string username, string playerName, string playerServer)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CCreateProfile);
            buffer.WriteString(username);
            buffer.WriteString(playerName);
            buffer.WriteString(playerServer);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void StrikeAccount(string senderName, string receiverName)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CStrikeAccount);
            buffer.WriteString(senderName);
            buffer.WriteString(receiverName);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void EditProfileBio(string playerName, string playerServer, byte[] avatarBytes, string name, string race, string gender, int age,
                                            string height, string weight, string atFirstGlance,
                                            int lawful_good, int neutral_good, int chaotic_good,
                                            int lawful_neutral, int true_neutral, int chaotic_neutral,
                                            int lawful_evil, int neutral_evil, int chaotic_evil)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CEditProfileBio);
            buffer.WriteString(playerName);
            buffer.WriteString(playerServer);
            buffer.WriteInteger(avatarBytes.Length);
            buffer.WriteBytes(avatarBytes);
            buffer.WriteString(name);
            buffer.WriteString(race);
            buffer.WriteString(gender);
            buffer.WriteInteger(age);
            buffer.WriteString(height);
            buffer.WriteString(weight);
            buffer.WriteString(atFirstGlance);
            buffer.WriteInteger(lawful_good);
            buffer.WriteInteger(neutral_good);
            buffer.WriteInteger(chaotic_good);
            buffer.WriteInteger(lawful_neutral);
            buffer.WriteInteger(true_neutral);
            buffer.WriteInteger(chaotic_neutral);
            buffer.WriteInteger(lawful_evil);
            buffer.WriteInteger(neutral_evil);
            buffer.WriteInteger(chaotic_evil);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void SubmitProfileBio(string playerName, string playerServer, byte[] avatarBytes, string name, string race, string gender, int age, 
                                            string height, string weight, string atFirstGlance, 
                                            int lawful_good, int neutral_good, int chaotic_good, 
                                            int lawful_neutral, int true_neutral, int chaotic_neutral, 
                                            int lawful_evil, int neutral_evil, int chaotic_evil)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CCreateProfileBio);
            buffer.WriteString(playerName);
            buffer.WriteString(playerServer);
            buffer.WriteInteger(avatarBytes.Length);
            buffer.WriteBytes(avatarBytes);
            buffer.WriteString(name);
            buffer.WriteString(race);
            buffer.WriteString(gender);
            buffer.WriteInteger(age);
            buffer.WriteString(height);
            buffer.WriteString(weight);
            buffer.WriteString(atFirstGlance);
            buffer.WriteInteger(lawful_good);
            buffer.WriteInteger(neutral_good);
            buffer.WriteInteger(chaotic_good);
            buffer.WriteInteger(lawful_neutral);
            buffer.WriteInteger(true_neutral);
            buffer.WriteInteger(chaotic_neutral);
            buffer.WriteInteger(lawful_evil);
            buffer.WriteInteger(neutral_evil);
            buffer.WriteInteger(chaotic_evil);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRulebookPage(string username, string title)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendRulebookPage);
            buffer.WriteString(username);
            buffer.WriteString(title);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void SendRulebookPageContent(string username, string title, string content)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendRulebookPageContent);
            buffer.WriteString(username);
            buffer.WriteString(title);
            buffer.WriteString(content);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void RequestTargetProfile(string targetPlayerName, string targetPlayerWorld)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.SRequestTargetProfile);
            buffer.WriteString(targetPlayerName);
            buffer.WriteString(targetPlayerWorld);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void SendHooks(string charactername, string characterworld, string hooks)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendHooks);
            buffer.WriteString(charactername);
            buffer.WriteString(characterworld);
            buffer.WriteString(hooks);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void DeleteHooks(string charactername, string characterworld, string hookMsg)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CDeleteHook);
            buffer.WriteString(charactername);
            buffer.WriteString(characterworld);
            buffer.WriteString(hookMsg);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();

            }
        public static void CreateProfile(string username, string charactername, byte[] avatarBytes, int avatarBytesLength, string race, string age, string height, string weight, string afg, string hooks, string story, string url)
        {

        }

        
        public static void UpdateSheetStatus(int sheetID, int status)
        {

            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendSheetVerify);
            buffer.WriteInteger(sheetID);
            buffer.WriteInteger(status);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
        public static void CreateSystem(string username, string name, string description, byte[] systemImage, int max_stats, int max_stat_points_per_stat, int max_stat_reduction, int max_stat_reduction_per_stat, int stat_allocation_allowed, int stat_reduction_allowed,int statCount)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendNewSystem);
            buffer.WriteString(username);
                buffer.WriteString(name);
                buffer.WriteString(description);
                buffer.WriteInteger(systemImage.Length);
                buffer.WriteBytes(systemImage);
                buffer.WriteInteger(max_stats);
                buffer.WriteInteger(max_stat_points_per_stat);
                buffer.WriteInteger(max_stat_reduction);
                buffer.WriteInteger(max_stat_reduction_per_stat);
                buffer.WriteInteger(stat_allocation_allowed);
                buffer.WriteInteger(stat_reduction_allowed);
                buffer.WriteInteger(statCount);
          
           
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }
      
        /*
         * PACKET STRUCTURE
         * 
        public static void EXAMPLE_PACKET_FUNCTION()
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.PACKETNAME_REFERENCED_IN_CLIENTPACKETS); 
            //buffer.WriteBool(true);
            //buffer.WriteString("Sup");
            //buffer.WriteInteger(1);
            ClientTCP.SendData(buffer.ToArray());
            buffer.Dispose();
        }

        */
    }
}
