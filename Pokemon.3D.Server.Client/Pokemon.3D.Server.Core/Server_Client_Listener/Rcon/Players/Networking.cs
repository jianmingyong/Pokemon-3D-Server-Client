using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Rcon.Players
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
        /// Get/Set Rcon Player Last Valid Ping
        /// </summary>
        public DateTime LastValidPing { get; set; }

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
                    Core.RconPlayer.Remove(Client, "You have closed the connection to the server.");
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
                        Core.RconPlayer.Remove(Client, Core.Setting.Token("SERVER_NOPING"));
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Sent the package to the rcon player.
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