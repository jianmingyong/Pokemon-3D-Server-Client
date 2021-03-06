﻿using System;
using System.Collections.Generic;
using System.Threading;

using Aragas.Core.Wrappers;
using PokeD.Core.Packets.SCON.Authorization;
using Pokemon_3D_Server_Core.SCON_Client_Listener.SCON;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Shared.Aragas.WrapperInstances;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.Servers
{
    public class ModuleSCON
    {
        private ITCPListener Listener { get; set; }
        private List<SCONClient> Clients { get; } = new List<SCONClient>();

        private List<Thread> ThreadCollection { get; } = new List<Thread>();
        
        public bool EncryptionEnabled { get; private set; } = false;
        
        private bool IsActive { get; set; } = false;

        public ModuleSCON()
        {
            TCPClientWrapper.Instance = new TCPClientWrapperInstance();
            TCPListenerWrapper.Instance = new TCPServerWrapperInstance();
        }

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
                    Thread Thread = new Thread(new System.Threading.ThreadStart(CheckListener)) {IsBackground = true};
                    Thread.Start();
                    ThreadCollection.Add(Thread);

                    StartListen();

                    var UpdateCycle = new Thread(Update);
                    UpdateCycle.Start();
                    ThreadCollection.Add(UpdateCycle);

                    IsActive = true;
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
                Dispose();
            }
        }

        private void StartListen()
        {
            Listener = TCPListenerWrapper.CreateTCPListener(Core.Setting.SCONPort);
            Listener.Start();
        }
        private void CheckListener()
        {
            if (Listener != null && Listener.AvailableClients)
                if (Listener.AvailableClients)
                    AddClient(new SCONClient(Listener.AcceptTCPClient(), this));
        }


        public void AddClient(SCONClient client) { Clients.Add(client); }

        public void RemoveClient(SCONClient client, string reason = "")
        {
            Clients.Remove(client);

            if(!string.IsNullOrEmpty(reason))
                client.SendPacket(new AuthorizationDisconnectPacket { Reason = reason });

            client.Dispose();
        }


        private void Update()
        {
            for (var i = 0; i < Clients.Count; i++)
                Clients[i]?.Update();
        }


        public void Dispose()
        {
            IsActive = false;

            Listener?.Stop();

            for (int i = 0; i < ThreadCollection.Count; i++)
            {
                if (ThreadCollection[i].IsAlive)
                {
                    ThreadCollection[i].Abort();
                }
            }
            ThreadCollection.RemoveRange(0, ThreadCollection.Count);

            for (int i = 0; i < Clients.Count; i++)
                Clients[i].Dispose();

            Clients.Clear();

            Core.Logger.Log("SCON Listener Disposed.",Logger.LogTypes.Info);
        }
    }
}