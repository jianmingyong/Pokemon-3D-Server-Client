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
        public List<Thread> ThreadCollection { get; set; }

        /// <summary>
        /// Get/Set TimerCollection
        /// </summary>
        public List<Timer> TimerCollection { get; set; }

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
        }

        private void StartListening()
        {

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
                            Package.Handle();
                            LastValidPing = DateTime.Now;
                        }
                    }
                }
                catch (SocketException)
                {
                    return;
                }
                catch (IOException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    ex.CatchError();
                    return;
                }
            } while (true);
        }

        private void ThreadStartPinging()
        {

        }

        private void ThreadStartSending()
        {
            try
            {
                Package p;
                if (PackageToSend.Count > 0 && PackageToSend.TryDequeue(out p))
                {
                    if (Client.IsConnected())
                    {
                        Writer.WriteLine(p.ToString());
                        Writer.Flush();
                        Core.Logger.Add("Networking.cs: Sent: " + p.ToString(), Logger.LogTypes.Debug, Client);
                    }
                }
            }
            catch (SocketException)
            {
                return;
            }
            catch (IOException)
            {
                return;
            }
            catch (Exception ex)
            {
                ex.CatchError();
                return;
            }
        }

        /// <summary>
        /// Dispose the networking client
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            Reader.Dispose();
            Writer.Dispose();
            Client.Close();
        }
    }
}
