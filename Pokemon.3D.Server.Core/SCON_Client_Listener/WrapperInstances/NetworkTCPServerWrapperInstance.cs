﻿using System.Net;
using System.Net.Sockets;
using Aragas.Core.Wrappers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances
{
    public class NetworkTCPServerWrapperInstance : INetworkTCPServer
    {
        public ushort Port { get; }
        public bool AvailableClients => Listener.Pending();


        TcpListener Listener { get; }


        public NetworkTCPServerWrapperInstance() { }
        private NetworkTCPServerWrapperInstance(ushort port)
        {
            Port = port;
            Listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port));
        }


        public void Start()
        {
            Listener.Start();
        }
        public void Stop()
        {
            Listener.Stop();
        }


        public INetworkTCPClient AcceptNetworkTCPClient()
        {
            return new NetworkTCPClientWrapperInstance(Listener.AcceptTcpClient());
        }


        public INetworkTCPServer NewInstance(ushort port)
        {
            return new NetworkTCPServerWrapperInstance(port);
        }


        public void Dispose()
        {
            Listener?.Stop();
        }
    }
}