using System.Net;
using System.Net.Sockets;
using Aragas.Core.Wrappers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances
{
    /// <summary>
    /// Class containing TCP Listener Implementation
    /// </summary>
    public class TCPListenerImplementation : ITCPListener
    {
        /// <summary>
        /// Get the Port for the listener.
        /// </summary>
        public ushort Port { get; }

        /// <summary>
        /// Determines the status of the <see cref="Socket"/>.
        /// </summary>
        public bool AvailableClients => Listener.Poll(0, SelectMode.SelectRead);

        private bool IsDisposed { get; set; }

        private Socket Listener { get; }


        internal TCPListenerImplementation(ushort port)
        {
            Port = port;

            Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Listener.NoDelay = true;

            Listener.Bind(new IPEndPoint(IPAddress.Any, Port));
        }

        /// <summary>
        /// Places a <see cref="Socket"/> in a listening state.
        /// </summary>
        public void Start()
        {
            if (IsDisposed)
                return;

            Listener.Listen(1000);
        }

        /// <summary>
        /// Closes the <see cref="Socket"/> connection and releases all associated resources.
        /// </summary>
        public void Stop()
        {
            if (IsDisposed)
                return;

            Listener.Close();
        }

        /// <summary>
        /// Creates a new <see cref="Socket"/> for a newly created connection.
        /// </summary>
        public ITCPClient AcceptTCPClient()
        {
            if (IsDisposed)
                return null;

            return new TCPClientImplementation(Listener.Accept());
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="Socket"/> class.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;

            Listener?.Dispose();
        }
    }

    /// <summary>
    /// Class containing TCP Server Wrapper Instance
    /// </summary>
    public class TCPServerWrapperInstance : ITCPListenerWrapper
    {
        /// <summary>
        /// Create New TCP Listener.
        /// </summary>
        /// <param name="port">Port to Listen.</param>
        public ITCPListener CreateTCPListener(ushort port) { return new TCPListenerImplementation(port); }
    }
}
