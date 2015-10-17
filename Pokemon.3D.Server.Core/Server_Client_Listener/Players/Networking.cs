using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using System.Collections.Concurrent;
using System.Diagnostics;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Players
{
    /// <summary>
    /// Class containing Networking
    /// </summary>
    public class Networking : IDisposable
    {
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }

        private ConcurrentQueue<string> PackageToReceive = new ConcurrentQueue<string>();
        /// <summary>
        /// Package to send.
        /// </summary>
        public ConcurrentQueue<Package> PackageToSend = new ConcurrentQueue<Package>();

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();
        private List<Timer> TimerCollection { get; set; } = new List<Timer>();

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

            Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
            Thread.Start();
            ThreadCollection.Add(Thread);

            Thread Thread1 = new Thread(new ThreadStart(ThreadHandlePackage)) { IsBackground = true };
            Thread1.Start();
            ThreadCollection.Add(Thread1);

            Thread Thread2 = new Thread(new ThreadStart(ThreadStartPinging)) { IsBackground = true };
            Thread2.Start();
            ThreadCollection.Add(Thread2);

            Thread Thread3 = new Thread(new ThreadStart(ThreadSentToPlayer)) { IsBackground = true };
            Thread3.Start();
            ThreadCollection.Add(Thread3);
        }

        private void ThreadStartListening()
        {
            do
            {
                try
                {
                    PackageToReceive.Enqueue(Reader.ReadLine());
                }
                catch (ThreadAbortException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                }
            } while (IsActive);
        }

        private void ThreadHandlePackage()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    string NewData;
                    if (PackageToReceive.TryDequeue(out NewData))
                    {
                        if (!string.IsNullOrWhiteSpace(NewData))
                        {
                            Package Package = new Package(NewData, Client);
                            Core.Logger.Log($"Receive: {NewData}", Logger.LogTypes.Debug, Client);

                            if (Package.IsValid)
                            {
                                LastValidPing = DateTime.Now;
                                Package.Handle();
                            }
                        }
                        else if (string.IsNullOrWhiteSpace(NewData) && IsActive)
                        {
                            IsActive = false;
                            Core.Player.Remove(Client, "You have left the game.");
                        }
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
                if (sw.ElapsedMilliseconds < 100)
                {
                    Thread.Sleep(100 - sw.ElapsedMilliseconds.ToString().Toint());
                }
                sw.Restart();
            } while (IsActive);
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

                    if (DateTime.Now >= LoginStartTime.AddHours(LastHourCheck + 1))
                    {
                        Core.Player.SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_LOGINTIME", (LastHourCheck + 1).ToString()), Client));
                        LastHourCheck++;
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
            } while (IsActive);
        }

        private void ThreadSentToPlayer()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    Package Package;
                    if (PackageToSend.TryDequeue(out Package))
                    {
                        Writer.WriteLine(Package.ToString());
                        Writer.Flush();
                        Core.Logger.Log($"Sent: {Package.ToString()}", Logger.LogTypes.Debug, Client);
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
                if (sw.ElapsedMilliseconds < 10)
                {
                    Thread.Sleep(10 - sw.ElapsedMilliseconds.ToString().Toint());
                }
                sw.Restart();
            } while (true);
        }

        /// <summary>
        /// Dispose the networking client
        /// </summary>
        public void Dispose()
        {
            IsActive = false;

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);

            Client.Close();
            Reader.Dispose();
            Writer.Dispose();
        }
    }
}