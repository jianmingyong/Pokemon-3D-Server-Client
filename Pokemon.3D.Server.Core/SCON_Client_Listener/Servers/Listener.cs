using System;
using System.Collections.Generic;
using System.Threading;
using Aragas.Core.Wrappers;
using Pokemon_3D_Server_Core.SCON_Client_Listener.SCON;
using Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using System.Diagnostics;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.Servers
{
    /// <summary>
    /// Class containing Pokemon 3D Listener
    /// </summary>
    public class SCONListener : IDisposable
    {
        private ITCPListener Listener { get; set; }

        private bool IsActive { get; set; } = false;

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private List<SCONClient> SCONClients { get; set; } = new List<SCONClient>();

        public SCONListener()
        {
            AppDomainWrapper.Instance = new AppDomainWrapperInstance();
            TCPListenerWrapper.Instance = new TCPServerWrapperInstance();
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
                    Dispose();
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

                    Listener = TCPListenerWrapper.CreateTCPListener(Core.Setting.SCONPort);
                    Listener.Start();

                    IsActive = true;
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Dispose();
            }
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

            Core.Logger.Log("SCON Listener Disposed.", Logger.LogTypes.Info);
        }

        private void ThreadStartListening()
        {
            if (Functions.CheckPortOpen(Core.Setting.SCONPort.ToString().Toint()))
            {
                Core.Logger.Log($"SCON started. SCON clients can join using the following address: {Core.Setting.IPAddress}:{Core.Setting.SCONPort.ToString()} (Global), {Functions.GetPrivateIP()}:{Core.Setting.SCONPort.ToString()} (Local).", Logger.LogTypes.Info);
            }
            else
            {
                Core.Logger.Log($"The specific port {Core.Setting.SCONPort.ToString()} is not opened. External/Global IP will not accept new SCON clients.", Logger.LogTypes.Info);
                Core.Logger.Log($"SCON started. SCON clients can join using the following address: {Functions.GetPrivateIP()}:{Core.Setting.SCONPort.ToString()} (Local).", Logger.LogTypes.Info);
            }

            Core.Logger.Log("SCON Listener initialized.", Logger.LogTypes.Info);

            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    if (Listener.AvailableClients)
                    {
                        SCONClients.Add(new SCONClient(Listener.AcceptNetworkTCPClient(), this));
                        Core.Logger.Log("New SCON Player Added.", Logger.LogTypes.Debug);
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
            } while (IsActive);
        }

        private void Update()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    for (var i = 0; i < SCONClients.Count; i++)
                    {
                        SCONClient client = SCONClients[i];

                        if (client != null)
                        {
                            client.Update();
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
                if (sw.ElapsedMilliseconds < 10)
                {
                    Thread.Sleep(10 - sw.ElapsedMilliseconds.ToString().Toint());
                }
                sw.Restart();
            } while (IsActive);
        }

        public void RemovePlayer(SCONClient sconClient)
        {
            SCONClients.Remove(sconClient);
            Core.Logger.Log("SCON Player Removed.", Logger.LogTypes.Debug);
        }
    }
}