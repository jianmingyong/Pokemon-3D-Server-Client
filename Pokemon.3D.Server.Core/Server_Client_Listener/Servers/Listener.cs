using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;

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

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private IWorkItemsGroup ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(1);

        private bool IsActive { get; set; } = false;

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
                    Dispose();
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

                    if (Core.Setting.AutoRestartTime >= 10)
                    {
                        Core.Logger.Log($"The server will restart every {Core.Setting.AutoRestartTime.ToString()} seconds.", Logger.LogTypes.Info);

                        Thread Thread2 = new Thread(new ThreadStart(ThreadAutoRestart)) { IsBackground = true };
                        Thread2.Start();
                        ThreadCollection.Add(Thread2);
                    }

                    Thread Thread3 = new Thread(new ThreadStart(Core.World.Update)) { IsBackground = true };
                    Thread3.Start();
                    ThreadCollection.Add(Thread3);
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Dispose();
            }
        }

        /// <summary>
        /// Dispose the Listener.
        /// </summary>
        public void Dispose()
        {
            IsActive = false;

            if (TcpListener != null) TcpListener.Stop();
            if (Client != null) Client.Close();
            if (Reader != null) Reader.Dispose();

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);

            Core.Logger.Log("Pokemon 3D Listener Disposed.", Logger.LogTypes.Info);
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
                Core.Logger.Log($"Server started. Players can join using the following address: {Core.Setting.IPAddress}:{Core.Setting.Port.ToString()} (Global), {Functions.GetPrivateIP()}:{Core.Setting.Port.ToString()} (Local) and with the following GameMode: {GameMode}.", Logger.LogTypes.Info);
            }
            else
            {
                Core.Logger.Log($"The specific port {Core.Setting.Port.ToString()} is not opened. External/Global IP will not accept new players.", Logger.LogTypes.Info);
                Core.Logger.Log($"Server started. Players can join using the following address: {Functions.GetPrivateIP()}:{Core.Setting.Port.ToString()} (Local) and with the following GameMode: {GameMode}.", Logger.LogTypes.Info);
            }

            Core.Logger.Log("Pokémon 3D Listener initialized.", Logger.LogTypes.Info);

            do
            {
                try
                {
                    Client = TcpListener.AcceptTcpClient();
                    Reader = new StreamReader(Client.GetStream());

                    ThreadPool.QueueWorkItem(new WorkItemCallback(ThreadHandlePackage), Reader.ReadLine());
                }
                catch (ThreadAbortException) { return; }
                catch (Exception) { }
            } while (IsActive);
        }

        private object ThreadHandlePackage(object obj)
        {
            try
            {
                string ReturnMessage = (string)obj;

                if (!string.IsNullOrWhiteSpace(ReturnMessage))
                {
                    Package Package = new Package(ReturnMessage, Client);
                    Core.Logger.Log($"Receive: {ReturnMessage}", Logger.LogTypes.Debug, Client);

                    if (Package.IsValid)
                    {
                        Package.Handle();
                    }
                }
            }
            catch (Exception) { }

            return null;
        }

        private void ThreadAutoRestart()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    TimeSpan TimeLeft = Core.Setting.StartTime.AddSeconds(Core.Setting.AutoRestartTime) - DateTime.Now;

                    if (TimeLeft.TotalSeconds == 300 || TimeLeft.TotalSeconds == 60 || (TimeLeft.TotalSeconds <= 10 && TimeLeft.TotalSeconds > 0))
                    {
                        Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_TRADEPVPFAIL", this.TimeLeft()), null));
                    }
                    else if (TimeLeft.TotalSeconds < 1)
                    {
                        ClientEvent.Invoke(ClientEvent.Types.Restart);
                    }
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                }

                sw.Stop();
                if (sw.ElapsedMilliseconds < 1000)
                {
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().Toint());
                }
                sw.Restart();
            }
            while (IsActive);
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