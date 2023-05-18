
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
        public static void SendSystemStats(string username, int statCount, string[] statNames, string[] statDescriptions, Vector3[] colors)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CSendSystemStats);
            buffer.WriteString(username);
            buffer.WriteInteger(statCount);
            for(int i = 0; i < statCount; i++)
            {
                buffer.WriteString(statNames[i]);
                buffer.WriteString(statDescriptions[i]);
                buffer.WriteInteger(int.Parse(colors[i].X.ToString()));
                buffer.WriteInteger(int.Parse(colors[i].Y.ToString()));
                buffer.WriteInteger(int.Parse(colors[i].Z.ToString()));
            }
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
        public static void FetchProfiles(string username)
        {
            var buffer = new ByteBuffer();
            buffer.WriteInteger((int)ClientPackets.CFetchProfiles);
            buffer.WriteString(username);
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
        public static void CreateProfile(string username, string playername, string charactername, byte[] avatarBytes, int avatarBytesLength, string race, string age, string height, string  weight, int health, int strength, int senses, int hardiness, int intelligence, int nimbleness, int eminence, string ability1Name, string ability2Name, string ability3Name, string ability1Desc, string ability2Desc, string ability3Desc)
        {
            
                var buffer = new ByteBuffer();
                buffer.WriteInteger((int)ClientPackets.CCreateProfile);
                buffer.WriteString(username);
                buffer.WriteString(playername);
                buffer.WriteString(charactername);
                buffer.WriteString(race);
                buffer.WriteString(age);
                buffer.WriteString(height);
                buffer.WriteString(weight);
                buffer.WriteInteger(health);
                buffer.WriteInteger(senses);
                buffer.WriteInteger(strength);
                buffer.WriteInteger(hardiness);
                buffer.WriteInteger(intelligence);
                buffer.WriteInteger(nimbleness);
                buffer.WriteInteger(eminence);
                buffer.WriteString(ability1Name);
                buffer.WriteString(ability2Name);
                buffer.WriteString(ability3Name);
                buffer.WriteString(ability1Desc);
                buffer.WriteString(ability2Desc);
                buffer.WriteString(ability3Desc);
                buffer.WriteInteger(avatarBytesLength);
                buffer.WriteBytes(avatarBytes);
                ClientTCP.SendData(buffer.ToArray());
                buffer.Dispose();
         
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
