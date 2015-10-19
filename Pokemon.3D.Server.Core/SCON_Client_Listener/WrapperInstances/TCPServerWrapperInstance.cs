using System.Net;
using System.Net.Sockets;
using Aragas.Core.Wrappers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances
{
    public class TCPListenerClass : ITCPListener
    {
        public ushort Port { get; }
        public bool AvailableClients => Listener.Pending();

        private TcpListener Listener { get; }


        internal TCPListenerClass(ushort port) { Port = port; Listener = new TcpListener(new IPEndPoint(IPAddress.Any, Port)); }

        public void Start() { Listener.Start(); }
        public void Stop() { Listener.Stop(); }

        public ITCPClient AcceptNetworkTCPClient() { return new TCPClientClass(Listener.AcceptTcpClient()); }

        public void Dispose() { Listener?.Stop(); }
    }

    public class TCPServerWrapperInstance : ITCPListenerWrapper
    {
        public ITCPListener CreateTCPListener(ushort port) { return new TCPListenerClass(port); }
    }
}
