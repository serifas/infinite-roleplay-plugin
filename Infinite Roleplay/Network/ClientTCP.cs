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
using System.Net.Http;

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
        private static string server = "47.158.180.196";
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
            if (IsConnectedToServer(ClientTCP.clientSocket) == true)
            {
                if (loadCallback == true)
                {
                    ClientConnectionCallback();
                    loadCallback = false;
                    string internalIpString = GetLocalIPAddress();
                    var externalIpTask = GetExternalIpAddress();
                    GetExternalIpAddress().Wait();
                    var externalIpString = externalIpTask.Result ?? IPAddress.Loopback;
                    if (internalIpString == string.Empty)
                    {
                        return;
                    }
                    else
                    {
                        DataSender.SendHelloServer(internalIpString, externalIpString.ToString());
                    }
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
        public static async Task<IPAddress?> GetExternalIpAddress()
        {
            var externalIpString = (await new HttpClient().GetStringAsync("http://icanhazip.com"))
                .Replace("\\r\\n", "").Replace("\\n", "").Trim();
            if (!IPAddress.TryParse(externalIpString, out var ipAddress)) return null;
            return ipAddress;
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



        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return string.Empty;
        }

        public static void PingHost(string _HostURI, int _PortNumber)
        {
            try
            {
                TcpClient client = new TcpClient(_HostURI, _PortNumber);        
            
            
                if(client.Connected == true)
                {
                    client.Close();
                }


            }
            catch
            {

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
