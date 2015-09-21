using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Core.Network
{
    /// <summary>
    /// Class containing Server Client
    /// </summary>
    public class ServerClient
    {
        private IPEndPoint IPEndPoint;
        private TcpListener Listener;
        private TcpClient Client;
        private StreamReader Reader;
        private StreamWriter Writer;

        /// <summary>
        /// Start the server
        /// </summary>
        public void Start()
        {
            try
            {
                // Before Running CheckList
                if (!My.Computer.Network.IsAvailable)
                {
                    Core.Logger.Add("ServerClient.cs: Unable to start the server. Check your connection again.", Logger.LogTypes.Warning);
                    return;
                }

                IPEndPoint = new IPEndPoint(IPAddress.Any, Core.Setting.Port);
                Listener = new TcpListener(IPEndPoint);
                Listener.Start();

                // Threading
                Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { Name = "ThreadStartListening", IsBackground = true };
                Thread.Start();
                Core.ThreadCollection.Add(Thread);

                // Timer 1
                Timer Timer1 = new Timer(new TimerCallback(Core.World.Update), null, 0, 1000);
                Core.TimerCollection.Add(Timer1);

                // Timer 2
                Timer Timer2 = new Timer(new TimerCallback(Core.Package.Handle), null, 0, 1);
                Core.TimerCollection.Add(Timer2);

                Core.Logger.Add("ServerClient.cs: Server Client is initalizing.", Logger.LogTypes.Info);
                if (Core.Setting.OfflineMode)
                {
                    Core.Logger.Add("ServerClient.cs: Players with offline profile can join the server.", Logger.LogTypes.Info);
                }

                string GameMode = null;
                for (int i = 0; i < Core.Setting.GameMode.Count; i++)
                {
                    GameMode += Core.Setting.GameMode[i] + ", ";
                }
                GameMode = GameMode.Remove(GameMode.LastIndexOf(","));

                if (Functions.CheckPortOpen())
                {
                    Core.Logger.Add(string.Format(@"ServerClient.cs: Server Started. Players can join using the following address: {0}:{1} (Global), {2}:{3} (Local) and with the following GameMode: {4}.", Core.Setting.IPAddress, Core.Setting.Port, Functions.GetPrivateIP(), Core.Setting.Port, GameMode), Logger.LogTypes.Info);
                }
                else
                {
                    Core.Logger.Add(string.Format(@"ServerClient.cs: The specific Port {0} is not opened. External/Global IP will not accept new players.", Core.Setting.Port), Logger.LogTypes.Info);
                    Core.Logger.Add(string.Format(@"ServerClient.cs: Server Started. Players can join using the following address: {0}:{1} (Local) and with the following GameMode: {2}.", Functions.GetPrivateIP(), Core.Setting.Port, GameMode), Logger.LogTypes.Info);
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void ThreadStartListening()
        {
            do
            {
                try
                {
                    Client = Listener.AcceptTcpClient();
                    Reader = new StreamReader(Client.GetStream());
                    string ReturnMessage = Reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(ReturnMessage))
                    {
                        Package Package = new Package(ReturnMessage, Client);
                        Core.Logger.Add("ServerClient.cs: Receive: " + ReturnMessage, Logger.LogTypes.Debug, Client);
                        if (Package.IsValid)
                        {
                            ThreadPool.QueueUserWorkItem(new WaitCallback(Package.Handle), null);
                        }
                    }
                }
                catch (Exception)
                {
                    Core.Logger.Add("ServerClient.cs: StreamReader failed to receive package data.", Logger.LogTypes.Debug, Client);
                }
            } while (true);
        }

        /// <summary>
        /// Sent Package Data to Player
        /// </summary>
        /// <param name="p">Package</param>
        public void SentToPlayer(Package p)
        {
            if (Core.Player.HasPlayer(p.Client))
            {
                Core.Player.GetPlayer(p.Client).Network.PackageToSend.Enqueue(p);
            }
            else
            {
                try
                {
                    Writer = new StreamWriter(p.Client.GetStream()) { AutoFlush = true };
                    Writer.WriteLine(p.ToString());
                    Writer.Flush();
                    Core.Logger.Add("ServerClient.cs: Sent: " + p.ToString(), Logger.LogTypes.Debug, Client);
                }
                catch (Exception)
                {
                    Core.Logger.Add("ServerClient.cs: StreamWriter failed to send package data.", Logger.LogTypes.Debug, Client);
                }
            }
        }

        /// <summary>
        /// Sent Package Data to All Player
        /// </summary>
        /// <param name="p">Package</param>
        public void SendToAllPlayer(Package p)
        {
            for (int i = 0; i < Core.Player.Count; i++)
            {
                if (p.Client == null || Core.Player[i].Network.Client != p.Client)
                {
                    Core.Player[i].Network.PackageToSend.Enqueue(p);
                }
            }
        }
    }
}
