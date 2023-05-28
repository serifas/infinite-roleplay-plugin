using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdateTest
{ 
    static class ClientHandleData
    {
        private static ByteBuffer playerBuffer;
        public static DataReceiver dr = new DataReceiver();
        public delegate void Packet(byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        //add our packets so we don't need to load them on the go.
        //should be added to start of client loading up
        public static void InitializePackets(bool start)
        {
            if(start == true)
            {
                packets.Add((int)ServerPackets.SWelcomeMessage, DataReceiver.HandleWelcomeMessage);
                packets.Add((int)ServerPackets.SRecLoginStatus, DataReceiver.LoginAuthenticated);
                packets.Add((int)ServerPackets.SRecAccPermissions, DataReceiver.RecPermissions);
                packets.Add((int)ServerPackets.SRecProfileBio, DataReceiver.ReceiveProfileBio);
                packets.Add((int)ServerPackets.SNoProfileBio, DataReceiver.NoProfileBio);
                packets.Add((int)ServerPackets.SNoProfile, DataReceiver.NoProfile);
                packets.Add((int)ServerPackets.SRecExistingProfile, DataReceiver.ExistingProfile);
                packets.Add((int)ServerPackets.SSendProfileHook, DataReceiver.ReceiveProfileHooks);
                packets.Add((int)ServerPackets.SSendNoProfileHooks, DataReceiver.NoProfileHooks);

            }
            else
            {
                packets.Clear();
            }
            //simple message back from server, simply for verification that the user is connected
        }

        //LITERALLY NOT COMMENTING THIS, AS IT WASNT EVEN EXPLAINED TO ME
        public static void HandleData(byte[] data)
        {
            var buffer = (byte[])data.Clone();
            var pLength = 0;

            if (playerBuffer == null)
            {
                playerBuffer = new ByteBuffer();
            }
            playerBuffer.WriteBytes(buffer);
            if (playerBuffer.Count() == 0)
            {
                playerBuffer.Clear();
                return;
            }
            if (playerBuffer.Length() > 4)
            {
                pLength = playerBuffer.ReadInt(false);
                if (pLength <= 0)
                {
                    playerBuffer.Clear();
                    return;
                }
            }
            while (pLength > 0 & pLength <= playerBuffer.Length() - 4)
            {
                if (pLength <= playerBuffer.Length() - 4)
                {
                    playerBuffer.ReadInt();
                    data = playerBuffer.ReadBytes(pLength);
                    HandleDataPackets(data);

                }

                pLength = 0;
                if (playerBuffer.Length() > 4)
                {
                    pLength = playerBuffer.ReadInt(false);
                    if (pLength <= 0)
                    {
                        playerBuffer.Clear();
                        return;
                    }
                }
            }
            if (pLength <= 1)
            {
                playerBuffer.Clear();
            }
        }
        private static void HandleDataPackets(byte[] data)
        {
            var buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            var packetID = buffer.ReadInt();
            buffer.Dispose();
            if (packets.TryGetValue(packetID, out var packet))
            {
                packet.Invoke(data);
            }
        }
    }
}
