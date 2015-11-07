using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Servers
{
    /// <summary>
    /// Class containing RCON Listener
    /// </summary>
    public class Listener : IDisposable
    {
        private StreamReader Reader { get; set; }
        private StreamWriter Writer { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private IWorkItemsGroup ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(1);
        private IWorkItemsGroup ThreadPool2 = new SmartThreadPool().CreateWorkItemsGroup(1);
        private IWorkItemsGroup ThreadPool3 = new SmartThreadPool().CreateWorkItemsGroup(1);

        /// <summary>
        /// Get/Set Client
        /// </summary>
        public TcpClient Client { get; set; }

        private DateTime LastValidPing { get; set; } = DateTime.Now;
        private DateTime LoginStartTime { get; } = DateTime.Now;

        private string IPAddress { get; set; }
        private string Password { get; set; }
        private int Port { get; set; }

        /// <summary>
        /// Get/Set Is Active Status.
        /// </summary>
        public bool IsActive { get; set; } = false;

        /// <summary>
        /// New Networking
        /// </summary>
        /// <param name="IPAddress">IPAddress</param>
        /// <param name="Password">Password</param>
        /// <param name="Port">Port</param>
        public Listener(string IPAddress, string Password, int Port)
        {
            this.IPAddress = IPAddress;
            this.Password = Password;
            this.Port = Port;

            Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
            Thread.Start();
            ThreadCollection.Add(Thread);
        }

        private void ThreadStartListening()
        {
            try
            {
                Core.Logger.Log($"Connecting to the specified server... Please wait.", Logger.LogTypes.Info);

                Client = new TcpClient();

                if (!Client.ConnectAsync(IPAddress, Port).Wait(5000))
                {
                    Core.Logger.Log(Core.Setting.Token("RCON_CONNECTFAILED"), Logger.LogTypes.Info);
                    return;
                }

                Reader = new StreamReader(Client.GetStream());
                Writer = new StreamWriter(Client.GetStream());

                SentToServer(new Package(Package.PackageTypes.Authentication, new List<string> { Password.Md5HashGenerator(), Password.SHA1HashGenerator(), Password.SHA256HashGenerator() }, null));

                string ReturnMessage = Reader.ReadLine();

                if (string.IsNullOrEmpty(ReturnMessage))
                {
                    Core.Logger.Log(Core.Setting.Token("RCON_CONNECTFAILED"), Logger.LogTypes.Info);
                    return;
                }
                else
                {
                    Package Package = new Package(ReturnMessage, Client);
                    if (Package.IsValid)
                    {
                        Core.Logger.Log($"Receive: {Package.ToString()}", Logger.LogTypes.Debug, Client);

                        if (Package.DataItems[0] == Package.AuthenticationStatus.AccessGranted.ToString())
                        {
                            Core.Logger.Log($"You are now connected to the server.", Logger.LogTypes.Info);
                            IsActive = true;
                        }
                        else if (Package.DataItems[0] == Package.AuthenticationStatus.AccessDenied.ToString())
                        {
                            Core.Logger.Log(Core.Setting.Token("RCON_CONNECTFAILED"), Logger.LogTypes.Info);
                            return;
                        }
                    }
                    else
                    {
                        Core.Logger.Log(Core.Setting.Token("RCON_CONNECTFAILED"), Logger.LogTypes.Info);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Core.Logger.Log(Core.Setting.Token("RCON_CONNECTFAILED"), Logger.LogTypes.Info);
                return;
            }

            Thread Thread2 = new Thread(new ThreadStart(ThreadStartPinging)) { IsBackground = true };
            Thread2.Start();
            ThreadCollection.Add(Thread2);

            do
            {
                try
                {
                    ThreadPool.QueueWorkItem(new WorkItemCallback(ThreadPreHandlePackage), Reader.ReadLine());
                }
                catch (Exception) { }
            } while (IsActive);
        }

        private object ThreadPreHandlePackage(object p)
        {
            if (string.IsNullOrEmpty((string)p))
            {
                if (IsActive)
                {
                    IsActive = false;
                    Core.Logger.Log(Core.Setting.Token("SERVER_CLOSE"), Logger.LogTypes.Info);
                    Dispose();
                }
            }
            else
            {
                Package Package = new Package((string)p, Client);
                if (Package.IsValid)
                {
                    ThreadPool2.QueueWorkItem(new WorkItemCallback(ThreadHandlePackage), Package);
                    Core.Logger.Log($"Receive: {Package.ToString()}", Logger.LogTypes.Debug, Client);
                }
            }

            return null;
        }

        private object ThreadHandlePackage(object obj)
        {
            Package Package = (Package)obj;
            Package.Handle();

            return null;
        }

        private void ThreadStartPinging()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    if ((DateTime.Now - LastValidPing).TotalSeconds >= 10)
                    {
                        SentToServer(new Package(Package.PackageTypes.Ping, "", null));
                        LastValidPing = DateTime.Now;
                    }
                }
                catch (Exception)
                {
                    Dispose();
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
        /// Sent To Server
        /// </summary>
        /// <param name="p">Package to send.</param>
        public void SentToServer(Package p)
        {
            ThreadPool3.QueueWorkItem(new WorkItemCallback(ThreadSentToServer), p);
        }

        private object ThreadSentToServer(object p)
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

        /// <summary>
        /// Dispose all network data.
        /// </summary>
        public void Dispose()
        {
            IsActive = false;

            if (Client != null) Client.Close();
            if (Reader != null) Reader.Dispose();
            if (Writer != null) Writer.Dispose();

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);

            Core.Logger.Log("RCON Listener Disposed.", Logger.LogTypes.Info);
        }
    }
}