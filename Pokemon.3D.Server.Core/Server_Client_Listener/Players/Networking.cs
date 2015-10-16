using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Players
{
    /// <summary>
    /// Class containing Networking
    /// </summary>
    public class Networking : IDisposable
    {
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();
        private List<Timer> TimerCollection { get; set; } = new List<Timer>();

        private int LastHourCheck = 0;

        private static readonly object Lock = new object();
        private static readonly object Lock1 = new object();

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
        /// Get/Set Network IsActive.
        /// </summary>
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// New Networking
        /// </summary>
        /// <param name="Client">Client</param>
        public Networking(TcpClient Client)
        {
            IsActive = true;

            Reader = new StreamReader(Client.GetStream());
            Writer = new StreamWriter(Client.GetStream());

            this.Client = Client;

            LastValidPing = DateTime.Now;
            LastValidMovement = DateTime.Now;

            Timer Timer1 = new Timer(new TimerCallback(ThreadStartPinging), null, 0, 1000);
            TimerCollection.Add(Timer1);

            Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
            Thread.Start();
            ThreadCollection.Add(Thread);
        }

        private void ThreadStartListening()
        {
            do
            {
                try
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(PreHandlePackage), Reader.ReadLine());
                }
                catch (Exception)
                {
                    return;
                }
            } while (IsActive);
        }

        private void PreHandlePackage(object ReturnMessage)
        {
            lock (Lock1)
            {
                if (!string.IsNullOrWhiteSpace((string)ReturnMessage))
                {
                    Package Package = new Package((string)ReturnMessage, Client);
                    Core.Logger.Log("Receive: " + ReturnMessage, Logger.LogTypes.Debug, Client);
                    if (Package.IsValid)
                    {
                        LastValidPing = DateTime.Now;
                        Package.Handle();
                    }
                }
                else if (string.IsNullOrWhiteSpace((string)ReturnMessage) && IsActive)
                {
                    IsActive = false;
                    Core.Player.Remove(Client, "You have left the game.");
                }
            }
        }

        private void ThreadStartPinging(object obj)
        {
            if (IsActive)
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

                if (DateTime.Now >= LoginStartTime.AddHours(LastHourCheck + 1))
                {
                    Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_LOGINTIME", (LastHourCheck + 1).ToString()), Client));
                    LastHourCheck++;
                }
            }
        }

        /// <summary>
        /// Sent the package to the player.
        /// </summary>
        /// <param name="p">Package</param>
        public void SentToPlayer(Package p)
        {
            lock (Lock)
            {
                try
                {
                    Writer.WriteLine(p.ToString());
                    Writer.Flush();
                    Core.Logger.Log("Sent: " + p.ToString(), Logger.LogTypes.Debug, Client);
                }
                catch (Exception)
                {
                    Core.Logger.Log("StreamWriter failed to send package data.", Logger.LogTypes.Debug, Client);
                }
            }
        }

        /// <summary>
        /// Dispose the networking client
        /// </summary>
        public void Dispose()
        {
            IsActive = false;

            for (int i = 0; i < TimerCollection.Count; i++)
            {
                TimerCollection[i].Dispose();
            }
            TimerCollection.RemoveRange(0, TimerCollection.Count);

            Client.Close();
            Reader.Dispose();
            Writer.Dispose();
        }
    }
}