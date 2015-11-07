using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Amib.Threading;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Servers
{
    /// <summary>
    /// Class containing RCON Listener
    /// </summary>
    public class Listener : IDisposable
    {
        private IPEndPoint IPEndPoint { get; set; }
        private TcpListener TcpListener { get; set; }

        private TcpClient Client { get; set; }

        private StreamReader Reader { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private IWorkItemsGroup ThreadPool = new SmartThreadPool().CreateWorkItemsGroup(1);

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
                    IPEndPoint = new IPEndPoint(IPAddress.Any, Core.Setting.RCONPort);
                    TcpListener = new TcpListener(IPEndPoint);
                    TcpListener.Start();

                    IsActive = true;

                    // Threading
                    Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
                    Thread.Start();
                    ThreadCollection.Add(Thread);
                }
            }
            catch (Exception)
            {
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

            Core.Logger.Log("RCON Listener Disposed.", Logger.LogTypes.Info);
        }

        private void ThreadStartListening()
        {
            if (Functions.CheckPortOpen(Core.Setting.RCONPort))
            {
                Core.Logger.Log($"RCON Server started. Players can join using the following address: {Core.Setting.IPAddress}:{Core.Setting.RCONPort.ToString()} (Global), {Functions.GetPrivateIP()}:{Core.Setting.RCONPort.ToString()} (Local).", Logger.LogTypes.Info);
            }
            else
            {
                Core.Logger.Log($"The specific port {Core.Setting.RCONPort.ToString()} is not opened. External/Global IP will not accept new RCON players.", Logger.LogTypes.Info);
                Core.Logger.Log($"RCON Server started. Players can join using the following address: {Functions.GetPrivateIP()}:{Core.Setting.RCONPort.ToString()} (Local).", Logger.LogTypes.Info);
            }

            Core.Logger.Log("RCON Listener initialized.", Logger.LogTypes.Info);

            do
            {
                try
                {
                    Client = TcpListener.AcceptTcpClient();
                    Reader = new StreamReader(Client.GetStream());

                    ThreadPool.QueueWorkItem(new WorkItemCallback(ThreadHandlePackage), Reader.ReadLine());
                }
                catch (ThreadAbortException) { return; }
                catch (Exception) { }
            } while (IsActive);
        }

        private object ThreadHandlePackage(object obj)
        {
            try
            {
                string ReturnMessage = (string)obj;

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
            catch (Exception) { }

            return null;
        }
    }
}