using System;
using System.Collections.Generic;
using System.Threading;
using Aragas.Core.Wrappers;
using Pokemon_3D_Server_Core.SCON_Client_Listener.SCON;
using Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.Servers
{
    /// <summary>
    /// Class containing Pokemon 3D Listener
    /// </summary>
    public class SCONListener : IDisposable
    {
        private INetworkTCPServer Listener { get; set; }

        private bool IsActive { get; set; } = false;

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private List<SCONClient> SCONClients { get; set; } = new List<SCONClient>();

        public SCONListener()
        {
            AppDomainWrapper.Instance = new AppDomainWrapperInstance();
            NetworkTCPServerWrapper.Instance = new NetworkTCPServerWrapperInstance();
        }

        /// <summary>
        /// Start the Listener.
        /// </summary>
        public void Start()
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
                    // Threading
                    Thread Thread = new Thread(new ThreadStart(ThreadStartListening)) { IsBackground = true };
                    Thread.Start();
                    ThreadCollection.Add(Thread);

                    var UpdateCycle = new Thread(Update);
                    UpdateCycle.Start();
                    ThreadCollection.Add(UpdateCycle);

                    Listener = NetworkTCPServerWrapper.NewInstance(Core.Setting.SCONPort);
                    Listener.Start();

                    IsActive = true;
                    
                    Core.Logger.Log("SCON Listener initializing...", Logger.LogTypes.Info);
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

            if (Listener != null) Listener.Stop();

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);

            Core.Logger.Log("SCON stopped.", Logger.LogTypes.Info);
        }

        /// <summary>
        /// Dispose the Listener.
        /// </summary>
        public void Dispose()
        {
            IsActive = false;

            if (Listener != null) Listener.Stop();

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);
        }

        private void ThreadStartListening()
        {
            if (Functions.CheckPortOpen(Core.Setting.SCONPort.ToString().Toint()))
            {
                Core.Logger.Log(string.Format(@"SCON Started. SCON clients can join using the following address: {0}:{1} (Global), {2}:{3} (Local).", Core.Setting.IPAddress, Core.Setting.SCONPort.ToString(), Functions.GetPrivateIP(), Core.Setting.SCONPort.ToString()), Logger.LogTypes.Info);
            }
            else
            {
                Core.Logger.Log(string.Format(@"The specific port {0} is not opened. External/Global IP will not accept new SCON clients.", Core.Setting.SCONPort.ToString()), Logger.LogTypes.Info);
                Core.Logger.Log(string.Format(@"SCON started. SCON clients can join using the following address: {0}:{1} (Local).", Functions.GetPrivateIP(), Core.Setting.SCONPort.ToString()), Logger.LogTypes.Info);
            }

            Core.Logger.Log("SCON Listener initialized.", Logger.LogTypes.Info);

            do
            {
                if (Listener.AvailableClients)
                {
                    SCONClients.Add(new SCONClient(Listener.AcceptNetworkTCPClient(), this));
                }
                Thread.Sleep(100);
            } while (IsActive);
        }

        private void Update()
        {
            do
            {
                for (var i = 0; i < SCONClients.Count; i++)
                {
                    SCONClient client = SCONClients[i];

                    if (client != null)
                    {
                        client.Update();
                    }
                }
                Thread.Sleep(100);
            } while (IsActive);
        }

        public void RemovePlayer(SCONClient sconClient)
        {
            SCONClients.Remove(sconClient);
        }
    }
}