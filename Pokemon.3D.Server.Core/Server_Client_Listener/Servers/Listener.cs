using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Servers
{
    /// <summary>
    /// Class containing Pokemon 3D Listener
    /// </summary>
    public class Listener : IDisposable
    {
        private IPEndPoint IPEndPoint { get; set; }

        private TcpListener TcpListener { get; set; }
        private TcpClient Client { get; set; }

        private StreamReader Reader { get; set; }

        private bool IsActive { get; set; } = false;

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();
        private List<Timer> TimerCollection { get; set; } = new List<Timer>();

        private static readonly object Lock = new object();

        /// <summary>
        /// Start the Listener.
        /// </summary>
        public void Start()
        {
            try
            {
                // Before Running CheckList
                if (!My.Computer.Network.IsAvailable)
                {
                    Core.Logger.Log("Network is not available.", Logger.LogTypes.Warning);
                    Stop();
                }
                else
                {
                    IPEndPoint = new IPEndPoint(IPAddress.Any, Core.Setting.Port);
                    TcpListener = new TcpListener(IPEndPoint);
                    TcpListener.Start();

                    IsActive = true;

                    // Threading
                    Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
                    Thread.Start();
                    ThreadCollection.Add(Thread);

                    //// Timer 1
                    Timer Timer1 = new Timer(new TimerCallback(Core.World.Update), null, 0, 1000);
                    TimerCollection.Add(Timer1);

                    // Timer 3
                    if (Core.Setting.AutoRestartTime >= 10)
                    {
                        Core.Logger.Log(string.Format(@"The server will restart every {0} seconds.", Core.Setting.AutoRestartTime), Logger.LogTypes.Info);
                        Timer Timer3 = new Timer(new TimerCallback(ThreadAutoRestart), null, 0, 1000);
                        TimerCollection.Add(Timer3);
                    }

                    Core.Logger.Log("Pokemon 3D Listener initializing...", Logger.LogTypes.Info);
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Stop();
            }  
        }

        /// <summary>
        /// Stop the Listener.
        /// </summary>
        public void Stop()
        {
            IsActive = false;

            if (Client != null)
            {
                Client.Close();
            }

            if (Client != null) Client.Close();
            if (Reader != null) Reader.Close();
            if (TcpListener != null) TcpListener.Stop();

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

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ServerClose, Core.Setting.Token("SERVER_CLOSE"), null));
            Core.Logger.Log("Server stopped.", Logger.LogTypes.Info);
        }

        /// <summary>
        /// Dispose the Listener.
        /// </summary>
        public void Dispose()
        {
            IsActive = false;

            if (Client != null)
            {
                Client.Close();
            }

            if (Client != null) Client.Close();
            if (Reader != null) Reader.Dispose();
            if (TcpListener != null) TcpListener.Stop();

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

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ServerClose, Core.Setting.Token("SERVER_CLOSE"), null));
        }

        private void ThreadStartListening()
        {
            if (Core.Setting.OfflineMode)
            {
                Core.Logger.Log("Players with offline profile can join the server.", Logger.LogTypes.Info);
            }

            string GameMode = null;
            for (int i = 0; i < Core.Setting.GameMode.Count; i++)
            {
                GameMode += Core.Setting.GameMode[i] + ", ";
            }
            GameMode = GameMode.Remove(GameMode.LastIndexOf(","));

            if (Functions.CheckPortOpen(Core.Setting.Port))
            {
                Core.Logger.Log(string.Format(@"Server Started. Players can join using the following address: {0}:{1} (Global), {2}:{3} (Local) and with the following GameMode: {4}.", Core.Setting.IPAddress, Core.Setting.Port, Functions.GetPrivateIP(), Core.Setting.Port, GameMode), Logger.LogTypes.Info);
            }
            else
            {
                Core.Logger.Log(string.Format(@"The specific port {0} is not opened. External/Global IP will not accept new players.", Core.Setting.Port), Logger.LogTypes.Info);
                Core.Logger.Log(string.Format(@"Server started. Players can join using the following address: {0}:{1} (Local) and with the following GameMode: {2}.", Functions.GetPrivateIP(), Core.Setting.Port, GameMode), Logger.LogTypes.Info);
            }

            Core.Logger.Log("Pokemon 3D Listener initialized.", Logger.LogTypes.Info);

            do
            {
                try
                {
                    Client = TcpListener.AcceptTcpClient();
                    Reader = new StreamReader(Client.GetStream());
                    ThreadPool.QueueUserWorkItem(new WaitCallback(PreHandlePackage), Reader.ReadLine());
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception)
                {
                    Core.Logger.Log("StreamReader failed to receive package data.", Logger.LogTypes.Debug, Client);
                }
            } while (IsActive);
        }

        private void PreHandlePackage(object ReturnMessage)
        {
            lock (Lock)
            {
                if (!string.IsNullOrWhiteSpace((string)ReturnMessage))
                {
                    Package Package = new Package((string)ReturnMessage, Client);
                    Core.Logger.Log("Receive: " + ReturnMessage, Logger.LogTypes.Debug, Client);
                    if (Package.IsValid)
                    {
                        Package.Handle();
                    }
                }
            }
        }

        private void ThreadAutoRestart(object obj)
        {
            TimeSpan TimeLeft = Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now;

            if (TimeLeft.TotalSeconds == 300 || TimeLeft.TotalSeconds == 60 || (TimeLeft.TotalSeconds <= 10 && TimeLeft.TotalSeconds > 0))
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", this.TimeLeft()), null));
            }
            else if (TimeLeft.TotalSeconds < 1)
            {
                ClientEvent.Invoke(ClientEvent.Types.Restart,null);
            }
        }

        /// <summary>
        /// Server Time Left
        /// </summary>
        public string TimeLeft()
        {
            if (Core.Setting.AutoRestartTime >= 10)
            {
                TimeSpan TimeLeft = Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now;
                string ReturnString = null;

                if (TimeLeft.Days > 1)
                {
                    ReturnString += TimeLeft.Days.ToString() + " days";
                }
                else if (TimeLeft.Days == 1)
                {
                    ReturnString += "1 day";
                }
                else
                {
                    if (TimeLeft.Hours > 1)
                    {
                        ReturnString += TimeLeft.Hours.ToString() + " hours ";
                    }
                    else if (TimeLeft.Hours == 1)
                    {
                        ReturnString += "1 hour ";
                    }

                    if (TimeLeft.Minutes > 1)
                    {
                        ReturnString += TimeLeft.Minutes.ToString() + " minutes ";
                    }
                    else if (TimeLeft.Minutes == 1)
                    {
                        ReturnString += "1 minute ";
                    }
                    else if (TimeLeft.Minutes == 0 && TimeLeft.TotalSeconds > 60)
                    {
                        ReturnString += "0 minute ";
                    }

                    if (TimeLeft.Seconds > 1)
                    {
                        ReturnString += TimeLeft.Seconds.ToString() + " seconds";
                    }
                    else if (TimeLeft.Seconds == 1)
                    {
                        ReturnString += "1 second";
                    }
                    else if (TimeLeft.Seconds == 0)
                    {
                        ReturnString += "0 second";
                    }
                }

                return ReturnString;
            }
            else
            {
                return null;
            }
        }
    }
}