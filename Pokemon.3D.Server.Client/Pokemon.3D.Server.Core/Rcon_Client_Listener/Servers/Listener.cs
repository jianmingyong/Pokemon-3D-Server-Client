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

namespace Pokemon_3D_Server_Core.Rcon_Client_Listener.Servers
{
    /// <summary>
    /// Class containing Pokemon 3D Listener
    /// </summary>
    public class Listener : IDisposable
    {
        private TcpClient Client { get; set; }

        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }

        private bool IsActive { get; set; } = false;

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();
        private List<Timer> TimerCollection { get; set; } = new List<Timer>();

        private static readonly object Lock = new object();

        /// <summary>
        /// Start the Listener.
        /// </summary>
        public void Start(string IPAddress)
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
                    Client = new TcpClient(IPAddress, Core.Setting.Port);

                    // Threading
                    Thread Thread = new Thread(new ParameterizedThreadStart(ThreadStartListening)) { IsBackground = true };
                    Thread.Start(IPAddress);
                    ThreadCollection.Add(Thread);

                    Core.Logger.Log("Pokemon 3D Rcon Listener initialized.", Logger.LogTypes.Info);
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

            Core.Logger.Log("Rcon connection closed.", Logger.LogTypes.Info);
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

            Core.Logger.Log("Rcon connection closed.", Logger.LogTypes.Info);
        }

        private void ThreadStartListening(object IPAddress)
        {
            try
            {
                Core.Logger.Log("Connecting to the Rcon Server...", Logger.LogTypes.Info);
                if (Client.ConnectAsync((string)IPAddress, Core.Setting.Port).Wait(5000))
                {
                    IsActive = true;

                    do
                    {
                        try
                        {
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
                else
                {
                    IsActive = false;
                    Core.Logger.Log("Unable to connect to the Rcon Server. Please try again later.", Logger.LogTypes.Info);
                    Stop();
                }
            }
            catch (Exception ex)
            {
                IsActive = false;
                ex.CatchError();
                Stop();
            }
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
    }
}