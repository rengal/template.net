using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Resto.Data;

namespace Resto.Net
{
    public class MulticastListener
    {
        private const string MulticastAddress = "225.6.7.8";
        private const int MulticastPort = 8080;

        private readonly HashSet<string> setServers = new HashSet<string>();
        private readonly Thread thread;
        private readonly MulticastOption multicastOption;

        public MulticastListener()
        {
            multicastOption = new MulticastOption(IPAddress.Parse(MulticastAddress));
            thread = new Thread(RunListener) { IsBackground = true };
        }

        public delegate void ErrorEventHandler(string Message);
        public event ErrorEventHandler OnError;
        public delegate void NewServerEventHandler(string protocol, string addr, string subUrl, int port, string desc);
        public event NewServerEventHandler OnNewServer;

        private void RunListener()
        {
            while (true)
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                    {
                        socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                        socket.Bind(new IPEndPoint(IPAddress.Any, MulticastPort));
                        socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, multicastOption);
                        byte[] byteBuffer = new byte[1024];
                        System.Text.RegularExpressions.Regex regex = new Regex(@"^RestoMulticaster: (?<port>\d+) '(?<url>[^']+)' '(?<desc>[^']+)'( '(?<protocolAndHost>[^']+)')?");
                        while (true)
                        {
                            EndPoint fromAddr = new IPEndPoint(IPAddress.Any, 0);
                            int received = socket.ReceiveFrom(byteBuffer, ref fromAddr);
                            if (received >= 1024)
                                throw new Exception("Too big packet received from " + fromAddr);
                            string data = Encoding.GetEncoding("windows-1251").GetString(byteBuffer, 0, received);
                            System.Text.RegularExpressions.Match match = regex.Match(data);


                            string addr;
                            string protocol;
                            string protocolAndHost = match.Result("${protocolAndHost}");
                            if (string.IsNullOrEmpty(protocolAndHost))
                            {
                                protocol = ServerInfo.CommunicationProtocols.Unknown;
                                addr = ((IPEndPoint)fromAddr).Address.ToString();
                            }
                            else
                            {
                                string[] parts = protocolAndHost.Split(new[] { "://" }, StringSplitOptions.None);
                                protocol = parts[0];
                                addr = parts[1];
                            }

                            string fullURL = protocol + "://" + match.Result(((IPEndPoint)fromAddr).Address + ":${port}${url}");
                            int port = Int32.Parse(match.Result("${port}"));
                            string desc = match.Result("${desc}");
                            string subUrl = match.Result("${url}");
                            if (fullURL.EndsWith("/"))
                                fullURL.Remove(fullURL.Length - 1);
                            lock (setServers)
                            {
                                if (!setServers.Contains(fullURL))
                                {
                                    setServers.Add(fullURL);
                                    if (OnNewServer != null)
                                        OnNewServer(protocol, addr, subUrl, port, desc);
                                }
                            }
                        }
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception exception)
                {
                    if (OnError != null)
                        OnError(exception.Message);
                    Thread.Sleep(5000);
                }
            }
        }

        public void StartListener()
        {
            thread.Start();
        }

        public void StopListener()
        {
            StopListener(true);
        }

        public void StopListener(bool join)
        {
            thread.Abort();
            if (join)
                thread.Join();
        }

#if DEBUG
        #region test
        // for testing purposes only
        static int NumberReceived = 0;
        static MulticastListener Listener;
        public static int Main(string[] args)
        {
            Listener = new MulticastListener();
            Listener.OnError += Listener_OnError;
            Listener.OnNewServer += Listener_OnNewServer;
            Listener.StartListener();
            while (NumberReceived < 10)
            {
                //Console.WriteLine("Receiving...");
                Thread.Sleep(1000);
            }
            Listener.StopListener();
            Console.WriteLine("The program has finished.");
            Console.ReadLine();
            return 0;
        }

        private static void Listener_OnNewServer(string protocol, string address, string subUrl, int port, string desc)
        {
            NumberReceived++;
            Console.WriteLine("OnNewServer: " + protocol + @"://" + address + ", port:" + port + subUrl + ", description: " + desc);
        }

        static void Listener_OnError(string message)
        {
            Console.WriteLine("Error occured: " + message);
        }
        #endregion
#endif
    }
}
