﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
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

        private static object Lock = new object();
        private static object Lock1 = new object();

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
                    string ReturnMessage = Reader.ReadLine();
                    Core.Logger.Log($"Receive: {ReturnMessage}", Logger.LogTypes.Debug, Client);

                    if (!string.IsNullOrWhiteSpace(ReturnMessage))
                    {
                        Package Package = new Package(ReturnMessage, Client);
                        if (Package.IsValid)
                        {
                            LastValidPing = DateTime.Now;
                            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadHandlePackage), Package);
                        }
                    }
                    else
                    {
                        IsActive = false;
                        Core.Player.Remove(Client, "You have left the game.");
                        return;
                    }
                }
                catch (Exception)
                {
                    return;
                }
            } while (IsActive);
        }

        private void ThreadHandlePackage(object obj)
        {
            lock (Lock1)
            {
                try
                {
                    Package Package = (Package)obj;
                    Package.Handle();
                }
                catch (Exception)
                {
                    return;
                }
            }
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
                            IsActive = false;
                            Core.Player.Remove(Client, Core.Setting.Token("SERVER_NOPING"));
                            return;
                        }
                    }

                    if (Core.Setting.AFKKickTime >= 10)
                    {
                        if ((DateTime.Now - LastValidMovement).TotalSeconds >= Core.Setting.AFKKickTime && Core.Player.GetPlayer(Client).BusyType == (int)Player.BusyTypes.Inactive)
                        {
                            IsActive = false;
                            Core.Player.Remove(Client, Core.Setting.Token("SERVER_AFK"));
                            return;
                        }
                    }

                    if (DateTime.Now >= LoginStartTime.AddHours(LastHourCheck + 1))
                    {
                        SentToPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_LOGINTIME", (LastHourCheck + 1).ToString()), Client));
                        LastHourCheck++;
                    }
                }
                catch (Exception)
                {
                    IsActive = false;
                    Core.Player.Remove(Client, Core.Setting.Token("SERVER_AFK"));
                    return;
                }

                sw.Stop();
                if (sw.ElapsedMilliseconds < 1000)
                {
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().Toint());
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
            lock (Lock)
            {
                try
                {
                    Writer.WriteLine(p.ToString());
                    Writer.Flush();
                    Core.Logger.Log($"Sent: {p.ToString()}", Logger.LogTypes.Debug, Client);
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}