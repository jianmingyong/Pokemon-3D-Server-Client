using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Core.Network
{
    /// <summary>
    /// Class containing Networking
    /// </summary>
    public class Networking : IDisposable
    {
        /// <summary>
        /// Get/Set SteamReader
        /// </summary>
        public StreamReader Reader { get; set; }

        /// <summary>
        /// Get/Set StreamWriter
        /// </summary>
        public StreamWriter Writer { get; set; }

        /// <summary>
        /// Get/Set Client
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// Get/Set ThreadCollection
        /// </summary>
        public List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        /// <summary>
        /// Get/Set TimerCollection
        /// </summary>
        public List<Timer> TimerCollection { get; set; } = new List<Timer>();

        /// <summary>
        /// Get/Set Player Last Valid Ping
        /// </summary>
        public DateTime LastValidPing { get; set; }

        /// <summary>
        /// Get/Set Player Last Valid Movement
        /// </summary>
        public DateTime LastValidMovement { get; set; }

        /// <summary>
        /// Get/Set Player Login StartTime
        /// </summary>
        public DateTime LoginStartTime { get; set; }

        /// <summary>
        /// Get/Set Network IsActive.
        /// </summary>
        public bool IsActive { get; set; }

        private int LastHourCheck = 0;

        /// <summary>
        /// Get/Set Player Queue for sending package.
        /// </summary>
        public ConcurrentQueue<Package> PackageToSend { get; set; } = new ConcurrentQueue<Package>();

        /// <summary>
        /// New Networking
        /// </summary>
        /// <param name="Client">Client</param>
        public Networking(TcpClient Client)
        {
            Reader = new StreamReader(Client.GetStream());
            Writer = new StreamWriter(Client.GetStream()) { AutoFlush = true };
            this.Client = Client;
            LastValidPing = DateTime.Now;
            LastValidMovement = DateTime.Now;
            LoginStartTime = DateTime.Now;

            IsActive = true;

            // Timer
            Timer Timer = new Timer(new TimerCallback(ThreadStartSending), null, 0, 1);
            TimerCollection.Add(Timer);

            // Timer
            Timer Timer1 = new Timer(new TimerCallback(ThreadStartPinging), null, 0, 1000);
            TimerCollection.Add(Timer1);

            Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { Name = "ThreadStartListening", IsBackground = true };
            Thread.Start();
            ThreadCollection.Add(Thread);
        }

        private void ThreadStartListening()
        {
            do
            {
                try
                {
                    string ReturnMessage = Reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(ReturnMessage))
                    {
                        Package Package = new Package(ReturnMessage, Client);
                        Core.Logger.Add("Networking.cs: Receive: " + ReturnMessage, Logger.LogTypes.Debug, Client);

                        if (Package.IsValid)
                        {
                            LastValidPing = DateTime.Now;
                            Package.Handle(null);
                        }
                    }
                    else
                    {
                        Core.Player.Remove(Core.Player.GetPlayer(Client).ID, "You have left the game.");
                        return;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            } while (true);
        }

        private void ThreadStartPinging(object obj = null)
        {
            if (IsActive)
            {
                if (Core.Setting.NoPingKickTime >= 10)
                {
                    if ((DateTime.Now - LastValidPing).TotalSeconds >= Core.Setting.NoPingKickTime)
                    {
                        Core.Player.Remove(Core.Player.GetPlayer(Client).ID, Core.Setting.Token("SERVER_NOPING"));
                        return;
                    }
                }

                if (Core.Setting.AFKKickTime >= 10)
                {
                    if ((DateTime.Now - LastValidMovement).TotalSeconds >= Core.Setting.AFKKickTime)
                    {
                        Core.Player.Remove(Core.Player.GetPlayer(Client).ID, Core.Setting.Token("SERVER_AFK"));
                        return;
                    }
                }

                if (DateTime.Now >= LoginStartTime.AddHours(LastHourCheck + 1))
                {
                    Core.Server.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_LOGINTIME"), Client));
                }
            }
        }

        /// <summary>
        /// Non-Threaded Sending - Only used on disposing method.
        /// </summary>
        /// <param name="p"></param>
        public void StartSending(Package p)
        {
            try
            {
                Writer.WriteLine(p.ToString());
                Writer.Flush();
                Core.Logger.Add("Networking.cs: Sent: " + p.ToString(), Logger.LogTypes.Debug, Client);
            }
            catch (Exception)
            {
                return;
            }
        }

        private void ThreadStartSending(object obj = null)
        {
            try
            {
                Package p = null;
                if (PackageToSend.Count > 0 && PackageToSend.TryDequeue(out p))
                {
                    Writer.WriteLine(p.ToString());
                    Writer.Flush();
                    Core.Logger.Add("Networking.cs: Sent: " + p.ToString(), Logger.LogTypes.Debug, Client);
                }
            }
            catch (Exception)
            {
                return;
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
            Client.Close();
            Reader.Dispose();
            Writer.Dispose();
        }
    }
}
