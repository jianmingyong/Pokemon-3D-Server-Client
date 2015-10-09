using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Event;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Core.Network
{
    /// <summary>
    /// Class containing Server Client
    /// </summary>
    public class ServerClient : IDisposable
    {
        private IPEndPoint IPEndPoint { get; set; }
        private TcpListener Listener { get; set; }
        private TcpClient Client { get; set; }
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }

        private bool IsActive { get; set; }

        /// <summary>
        /// [0] => ThreadStartListening
        /// </summary>
        private static List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        /// <summary>
        /// [0] => Core.World.Update
        /// [1] => Core.Package.Handle
        /// [2] => ThreadAutoRestart
        /// </summary>
        private static List<Timer> TimerCollection { get; set; } = new List<Timer>();

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
                    Core.Logger.Add("ServerClient.cs: Network is not available.", Logger.LogTypes.Warning);
                    Stop(false);
                }
                else
                {
                    IPEndPoint = new IPEndPoint(IPAddress.Any, Core.Setting.Port);
                    Listener = new TcpListener(IPEndPoint);
                    Listener.Start();

                    IsActive = true;

                    // Threading
                    Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { Name = "ThreadStartListening", IsBackground = true };
                    Thread.Start();
                    ThreadCollection.Add(Thread);

                    // Timer 1
                    Timer Timer1 = new Timer(new TimerCallback(Core.World.Update), null, 0, 1000);
                    TimerCollection.Add(Timer1);

                    // Timer 2
                    Timer Timer2 = new Timer(new TimerCallback(Core.Package.Handle), null, 0, 1);
                    TimerCollection.Add(Timer2);

                    // Timer 3
                    if (Core.Setting.AutoRestartTime >= 10)
                    {
                        Core.Logger.Add(string.Format(@"ServerClient.cs: The server will restart every {0} seconds.", Core.Setting.AutoRestartTime), Logger.LogTypes.Info);
                        Timer Timer3 = new Timer(new TimerCallback(ThreadAutoRestart), null, 0, 1000);
                        TimerCollection.Add(Timer3);
                    }

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
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Stop(false);
            }
        }

        public void Stop(bool Dispose)
        {
            IsActive = false;

            if (Client != null) Client.Close();
            if (Reader != null) Reader.Close();
            if (Writer != null) Writer.Close();
            if (Listener != null) Listener.Stop();

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);

            for (int i = 0; i < TimerCollection.Count; i++)
            {
                TimerCollection[i].Dispose();
            }
            TimerCollection.RemoveRange(0, TimerCollection.Count);

            Core.Server.SendToAllPlayer(new Package(Package.PackageTypes.ServerClose, Core.Setting.Token("SERVER_CLOSE"), null));
            Core.Logger.Add("ServerClient.cs: Server stopped.", Logger.LogTypes.Info);

            if (Dispose)
            {
                this.Dispose();
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
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception)
                {
                    Core.Logger.Add("ServerClient.cs: StreamReader failed to receive package data.", Logger.LogTypes.Debug, Client);
                }
            } while (IsActive);
        }

        private void ThreadAutoRestart(object obj = null)
        {
            TimeSpan TimeLeft = Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now;

            if (TimeLeft.TotalSeconds == 300 || TimeLeft.TotalSeconds == 60 || (TimeLeft.TotalSeconds <= 10 && TimeLeft.TotalSeconds > 0))
            {
                SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", Core.Setting.TimeLeft()), null));
            }
            else if (TimeLeft.TotalSeconds < 1)
            {
                ClientEvent.Invoke(ClientEvent.Types.Restart);
            }
        }

        /// <summary>
        /// Sent Package Data to Player
        /// </summary>
        /// <param name="p">Package</param>
        /// <param name="Threaded">Threaded?</param>
        public void SentToPlayer(Package p, bool Threaded = true)
        {
            if (Core.Player.HasPlayer(p.Client))
            {
                if (Core.Player.GetPlayer(p.Client).Network.IsActive && Threaded)
                {
                    Core.Player.GetPlayer(p.Client).Network.PackageToSend.Enqueue(p);
                }
                else
                {
                    Core.Player.GetPlayer(p.Client).Network.StartSending(p);
                }
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
        /// Send Package Data to all Operator.
        /// </summary>
        /// <param name="p">Package</param>
        public void SendToAllOperator(Package p)
        {
            for (int i = 0; i < Core.Player.Count; i++)
            {
                if (p.Client != null && Core.Player[i].Network.Client != p.Client && Core.Player[i].IsOperator())
                {
                    if (Core.Player[i].Network.IsActive)
                    {
                        Core.Player[i].Network.PackageToSend.Enqueue(p);
                    }
                    else
                    {
                        Core.Player[i].Network.StartSending(p);
                    }
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
                    if (Core.Player[i].Network.IsActive)
                    {
                        Core.Player[i].Network.PackageToSend.Enqueue(p);
                    }
                    else
                    {
                        Core.Player[i].Network.StartSending(p);
                    }
                }
            }
        }

        /// <summary>
        /// Dispose ServerClient.
        /// </summary>
        public void Dispose()
        {
            if (Reader != null) Reader.Dispose();
            if (Writer != null) Writer.Dispose();
        }
    }
}
