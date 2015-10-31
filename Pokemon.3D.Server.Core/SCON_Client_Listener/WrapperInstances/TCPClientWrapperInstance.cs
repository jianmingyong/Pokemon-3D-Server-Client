using System.IO;
using System.Net;
using System.Net.Sockets;
using Aragas.Core.Wrappers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances
{
    /// <summary>
    /// Class containing TCP Client Implementation.
    /// </summary>
    public class TCPClientImplementation : ITCPClient
    {
        /// <summary>
        /// Get/Set Refresh Connection Info Time.
        /// </summary>
        public int RefreshConnectionInfoTime { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the endpoint.
        /// </summary>
        public string IP => !IsDisposed && Client != null ? (Client.RemoteEndPoint as IPEndPoint)?.Address.ToString() : "";

        /// <summary>
        /// Gets a value that indicates whether a <see cref="Socket"/> is connected
        /// to a remote host as of the last Overload:System.Net.Sockets.Socket.Send or Overload:System.Net.Sockets.Socket.Receive
        /// operation.
        /// </summary>
        public bool Connected => !IsDisposed && Client != null && Client.Connected;

        /// <summary>
        /// Gets the amount of data that has been received from the network and is available to be read.
        /// </summary>
        public int DataAvailable => !IsDisposed && Client != null ? Client.Available : 0;

        private Socket Client { get; }
        private Stream Stream { get; set; }

        private bool IsDisposed { get; set; }

        /// <summary>
        /// New TCPClientImplementation.
        /// </summary>
        public TCPClientImplementation()
        {
            Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Client.NoDelay = true;
        }
        internal TCPClientImplementation(Socket socket)
        {
            Client = socket;
            Stream = new NetworkStream(Client);

        }

        /// <summary>
        /// Establishes a connection to a remote host. The host is specified by a host name and a port number.
        /// </summary>
        /// <param name="ip">The name of the remote host.</param>
        /// <param name="port">The port number of the remote host.</param>
        public ITCPClient Connect(string ip, ushort port)
        {
            if (Connected)
                Disconnect();

            Client.Connect(ip, port);
            Stream = new NetworkStream(Client);

            return this;
        }

        /// <summary>
        /// Closes the socket connection and allows reuse of the socket.
        /// </summary>
        public ITCPClient Disconnect()
        {
            if (Connected)
                Client.Disconnect(false);

            return this;
        }

        /// <summary>
        /// Sends the specified number of bytes of data to a connected <see cref="Socket"/>,
        /// starting at the specified offset, and using the specified <see cref="SocketFlags"/>.
        /// </summary>
        /// <param name="array">An array of type <see cref="byte"/> that contains the data to be sent.</param>
        public void WriteByteArray(byte[] array)
        {
            if (IsDisposed)
                return;

            try
            {
                var length = array.Length;

                var bytesSend = 0;
                while (bytesSend < length)
                    bytesSend += Client.Send(array, bytesSend, length - bytesSend, 0);
            }
            catch (IOException) { Dispose(); }
            catch (SocketException) { Dispose(); }
        }

        /// <summary>
        /// Receives the specified number of bytes from a bound <see cref="Socket"/>
        /// into the specified offset position of the receive buffer, using the specified
        /// <see cref="SocketFlags"/>.
        /// </summary>
        /// <param name="length">The number of bytes to receive.</param>
        public byte[] ReadByteArray(int length)
        {
            if (IsDisposed)
                return new byte[0];

            try
            {
                var array = new byte[length];

                var bytesReceive = 0;
                while (bytesReceive < length)
                    bytesReceive += Client.Receive(array, bytesReceive, length - bytesReceive, 0);

                return array;
            }
            catch (IOException) { Dispose(); return new byte[0]; }
            catch (SocketException) { Dispose(); return new byte[0]; }
        }

        /// <summary>
        /// Get the Stream from the Client.
        /// </summary>
        public Stream GetStream() { return Stream; }

        /// <summary>
        /// Dispose the instances.
        /// </summary>
        public void Dispose()
        {
            if (Connected)
                Disconnect();

            IsDisposed = true;

            Client?.Dispose();
            Stream?.Dispose();
        }
    }

    /// <summary>
    /// Class containing TCP Client Wrapper Instance.
    /// </summary>
    public class TCPClientWrapperInstance : ITCPClientWrapper
    {
        /// <summary>
        /// Create New TCP Client Instance.
        /// </summary>
        public ITCPClient CreateTCPClient() { return new TCPClientImplementation(); }
    }
}
