using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Players
{
    /// <summary>
    /// Class containing Networking
    /// </summary>
    public class Networking
    {
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private IWorkItemsGroup ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(1);
        private IWorkItemsGroup ThreadPool2 = new SmartThreadPool().CreateWorkItemsGroup(1);

        /// <summary>
        /// Get Sending WorkPool.
        /// </summary>
        public IWorkItemsGroup ThreadPool3 = new SmartThreadPool().CreateWorkItemsGroup(1);

        private int LastHourCheck = 0;

        /// <summary>
        /// Get/Set Client
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Get/Set Player Last Valid Ping
        /// </summary>
        public DateTime LastValidPing { get; set; }

        /// <summary>
        /// Get/Set Player Last Valid Movement
        /// </summary>
        public DateTime LastValidMovement { get; set; }

        /// <summary>
        /// Get Player Login StartTime
        /// </summary>
        public DateTime LoginStartTime { get; } = DateTime.Now;

        /// <summary>
        /// Get/Set Is Active Status.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// New Networking
        /// </summary>
        /// <param name="Client">Client</param>
        public Networking(TcpClient Client)
        {
            // Set Client Property.
            this.Client = Client;
            IsActive = true;

            Reader = new StreamReader(Client.GetStream());
            Writer = new StreamWriter(Client.GetStream());

            LastValidPing = DateTime.Now;
            LastValidMovement = DateTime.Now;

            Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
            Thread.Start();
            ThreadCollection.Add(Thread);

            Thread Thread2 = new Thread(new ThreadStart(ThreadStartPinging)) { IsBackground = true };
            Thread2.Start();
            ThreadCollection.Add(Thread2);
        }

        private void ThreadStartListening()
        {
            do
            {
                try
                {
                    if (Client.Available > -1)
                    {
                        ThreadPool.QueueWorkItem(new Action<string>(ThreadPreHandlePackage), Reader.ReadLine());
                    }
                }
                catch (Exception)
                {
                    if (IsActive)
                    {
                        IsActive = false;
                        Core.Player.Remove(Client, "You have left the game.");
                    }
                }
            } while (IsActive);
        }

        private void ThreadPreHandlePackage(string p)
        {
            Package Package = new Package(p, Client);
            if (Package.IsValid)
            {
                LastValidPing = DateTime.Now;
                ThreadPool2.QueueWorkItem(new Action<Package>(ThreadHandlePackage), Package);
                Core.Logger.Log($"Receive: {Package.ToString()}", Logger.LogTypes.Debug, Client);
            }
        }

        private void ThreadHandlePackage(Package p)
        {
            p.Handle();
        }

        private void ThreadStartPinging()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    if (Core.Setting.NoPingKickTime >= 10)
                    {
                        if ((DateTime.Now - LastValidPing).TotalSeconds >= Core.Setting.NoPingKickTime)
                        {
                            Core.Player.Remove(Client, Core.Setting.Token("SERVER_NOPING"));
                            return;
                        }
                    }

                    if (Core.Setting.AFKKickTime >= 10)
                    {
                        if ((DateTime.Now - LastValidMovement).TotalSeconds >= Core.Setting.AFKKickTime && Core.Player.GetPlayer(Client).BusyType == (int)Player.BusyTypes.Inactive)
                        {
                            Core.Player.Remove(Client, Core.Setting.Token("SERVER_AFK"));
                            return;
                        }
                    }

                    /*
                    if (DateTime.Now >= LoginStartTime.AddHours(LastHourCheck + 1))
                    {
                        SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_LOGINTIME", (LastHourCheck + 1).ToString()), Client));
                        LastHourCheck++;
                    }
                    */
                }
                catch (Exception ex)
                {
                    Core.Player.Remove(Client, ex.Message);
                    return;
                }

                sw.Stop();
                if (sw.ElapsedMilliseconds < 1000)
                {
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().ToInt());
                }
                sw.Restart();
            } while (IsActive);
        }

        /// <summary>
        /// Sent To Player
        /// </summary>
        /// <param name="p">Package to send.</param>
        public void SentToPlayer(Package p)
        {
            ThreadPool3.QueueWorkItem(new WorkItemCallback(ThreadSentToPlayer), p);
        }

        private object ThreadSentToPlayer(object p)
        {
            try
            {
                Writer.WriteLine(((Package)p).ToString());
                Writer.Flush();
                Core.Logger.Log($"Sent: {((Package)p).ToString()}", Logger.LogTypes.Debug, Client);
            }
            catch (Exception) { }

            return null;
        }
    }
}