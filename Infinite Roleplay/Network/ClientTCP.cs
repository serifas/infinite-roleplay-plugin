using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using System.Runtime.CompilerServices;
using System.Timers;
using InfiniteRoleplay.Windows.Functions;
using InfiniteRoleplay;
using System.IO;
using InfiniteRoleplay.Windows;

namespace Networking
{
    public class ClientTCP
    {
        public static bool loadCallback;
        public float targetTime = 60.0f;
        public static bool Connected;
        public static TcpClient clientSocket;
        private static NetworkStream myStream;
        private static byte[] recBuffer;
        private static string server = "185.28.22.232";
        private static int port = 25565;
        public static Timer timer;
        public static Plugin plugin;
        public static void LoadConnectionTimer()
        {
            timer = new Timer(3000);
            timer.Elapsed += OnTick;
            timer.Start();
        }
        public static bool IsConnectedToServer(TcpClient _tcpClient)
        {

            try
            {
                if (_tcpClient != null && _tcpClient.Client != null && _tcpClient.Client.Connected)
                {
                    /* pear to the documentation on Poll:
                     * When passing SelectMode.SelectRead as a parameter to the Poll method it will return 
                     * -either- true if Socket.Listen(Int32) has been called and a connection is pending;
                     * -or- true if data is available for reading; 
                     * -or- true if the connection has been closed, reset, or terminated; 
                     * otherwise, returns false
                     */

                    // Detect if client disconnected
                    if (_tcpClient.Client.Poll(0, SelectMode.SelectRead))
                    {
                        byte[] buff = new byte[1];
                        if (_tcpClient.Client.Receive(buff, SocketFlags.Peek) == 0)
                        {
                            // Client disconnected
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public static void CheckStatus()
        {
            if (PingHost(server, port) == true)
            {
                if (IsConnectedToServer(clientSocket) == true)
                {
                    if (loadCallback == true)
                    {
                        ClientConnectionCallback();
                        loadCallback = false;
                        plugin.chatGUI.PrintError("Callback Loaded");
                    }
                    if (plugin.uiLoaded == false)
                    {
                        plugin.LoadUI();
                    }
                }
                else
                {
                    ConnectToServer();
                }

            }

        }

        public static void OnTick(System.Object? sender, ElapsedEventArgs eventArgs)
        {
            CheckStatus();
        }
        public static void ConnectToServer()
        {
            ClientHandleData.InitializePackets(true);
            InitializingNetworking(true);
            loadCallback = true;
            CheckStatus();
            plugin.chatGUI.PrintError("We connected and are checking status");
        }

        public static void InitializingNetworking(bool start)
        {

            if (start == true)
            {
                EstablishConnection();
            }
            else
            {
                if (clientSocket.Connected == true)
                {
                    Disconnect();
                }
            }
        }



        public static bool PingHost(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new TcpClient(hostUri, portNumber))
                    return true;
            }
            catch (SocketException ex)
            {
                return false;
            }
        }


        public static void EstablishConnection()
        {
            try
            {
                clientSocket = new TcpClient();
                clientSocket.ReceiveBufferSize = 65535;
                clientSocket.SendBufferSize = 65535;
                recBuffer = new byte[65535 * 2];
                clientSocket.Connect(server, port);
            }
            catch
            {

            }

        }

        public static void ClientConnectionCallback()
        {
            Connected = true;
            clientSocket.NoDelay = true;
            myStream = clientSocket.GetStream();
            myStream.BeginRead(recBuffer, 0, 4096 * 2, ReceiveCallback, null);

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
            Connected = false; //simply used for scripts to tell if the server is connected
            clientSocket.Close(); //close the socket on disconnect
        }

    }
}
