using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Shared.jianmingyong;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Threading;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Servers
{
    /// <summary>
    /// Class containing Pokemon 3D Listener
    /// </summary>
    public class Listener : IDisposable
    {
        private TcpListener TcpListener { get; set; }

        private ThreadCollection ThreadCollection { get; set; } = new ThreadCollection();
        private IWorkItemsGroup ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(Environment.ProcessorCount);

        private bool IsActive { get; set; } = false;

        /// <summary>
        /// Starts listening for incoming connection requests.
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
                    TcpListener = new TcpListener(new IPEndPoint(IPAddress.Any, Core.Setting.Port));
                    TcpListener.Start();

                    IsActive = true;

                    // Threading
                    ThreadCollection.Add(new ThreadStart(ThreadStartListening));
                    ThreadCollection.Add(new ThreadStart(Core.World.Update));

                    if (Core.Setting.AutoRestartTime >= 10)
                    {
                        Core.Logger.Log($"The server will restart every {Core.Setting.AutoRestartTime.ToString()} seconds.", Logger.LogTypes.Info);

                        ThreadCollection.Add(new ThreadStart(ThreadAutoRestart));
                    }
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

            ThreadCollection.Dispose();

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
                ThreadCollection.Add(new ThreadStart(ThreadPortCheck));
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
                    ThreadPool.QueueWorkItem(new Action<TcpClient>(ThreadAcceptTcpClient), TcpListener.AcceptTcpClient());
                }
                catch (ThreadAbortException) { return; }
                catch (Exception) { }
            } while (IsActive);
        }

        private void ThreadAcceptTcpClient(TcpClient Client)
        {
            try
            {
                if (Client != null)
                {
                    StreamReader Reader = new StreamReader(Client.GetStream());
                    string ReturnMessage = Reader.ReadLine();

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
            }
            catch (Exception) { }
        }

        private void ThreadPortCheck()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Core.Logger.Log("Port check is now enabled.", Logger.LogTypes.Info);

            do
            {
                if (sw.Elapsed.TotalMinutes >= 15)
                {
                    if (Functions.CheckPortOpen(Core.Setting.Port))
                    {
                        Core.Logger.Log("Port Check cycle completed. Result: True.", Logger.LogTypes.Info);
                        sw.Restart();
                    }
                    else
                    {
                        Core.Logger.Log("Port Check cycle completed. Result: False.", Logger.LogTypes.Info);
                        ClientEvent.Invoke(ClientEvent.Types.Restart);
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }
            } while (IsActive);
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
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().ToInt());
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