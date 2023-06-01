using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using System.Runtime.CompilerServices;

namespace UpdateTest
{
    public class ClientTCP
    {
        public float targetTime = 60.0f;
        private static bool connected;
        public static TcpClient clientSocket;
        private static NetworkStream myStream;
        private static byte[] recBuffer;
        private static string server = "77.83.199.90";
        private static int port = 80;
        public static void InitializingNetworking(bool start)
        {
          
            if(start == true)
            {
                if(PingHost(server))
                {
                    try
                    {
                        clientSocket = new TcpClient();
                        clientSocket.ReceiveBufferSize = 65535;
                        clientSocket.SendBufferSize = 65535;
                        recBuffer = new byte[65535 * 2];
                        clientSocket.BeginConnect(server, port, new AsyncCallback(ClientConnectionCallback), clientSocket);

                    }
                    catch
                    {

                    }
                }





            }
            else
            {
                if (clientSocket.Connected == true)
                {
                    Disconnect();
                }

            }
        }



        public static bool PingHost(string server)
        {
            bool connection = false;
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(server, 1000);
                if (reply != null)
                {                    
                    connection = true;
                }
            }
            catch
            {
                connection = false;
            }
            return connection;
            Console.ReadKey();
        }
    
        /*public static bool PingHost(string _HostURI, int _PortNumber)
        {
            try
            {
                TcpClient client = new TcpClient(_HostURI, _PortNumber);
                client.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }*/

        private static void ClientConnectionCallback(IAsyncResult result)
        {
            clientSocket.EndConnect(result);
            if (clientSocket.Connected == false)
            {
                return;
            }
            else
            {
                connected = true;
                clientSocket.NoDelay = true;
                myStream = clientSocket.GetStream();
                myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);
            }
        }
        private static void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                var length = myStream.EndRead(result);
                if (length <= 0)
                {
                    return;
                }
                var newBytes = new byte[length];
                Array.Copy(recBuffer, newBytes, length);
                ClientHandleData.HandleData(newBytes);
                myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);

            }
            catch (Exception)
            {
                return;
            }
        }
        public static void SendData(byte[] data)
        {
            try
            {
                var buffer = new ByteBuffer();
                buffer.WriteInteger(data.GetUpperBound(0) - data.GetLowerBound(0) + 1);
                buffer.WriteBytes(data);
                myStream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
                buffer.Dispose();
            }
            catch
            {

            }
                
           

        }
        public static void Disconnect()
        {
            connected = false; //simply used for scripts to tell if the server is connected
            clientSocket.Close(); //close the socket on disconnect
        }

    }
}
