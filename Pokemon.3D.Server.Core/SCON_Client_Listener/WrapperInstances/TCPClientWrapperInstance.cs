using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Aragas.Core.Wrappers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.WrapperInstances
{
    public class TCPClientClass : ITCPClient
    {
        public int RefreshConnectionInfoTime { get; set; }

        public string IP => !IsDisposed && Client != null ? ((IPEndPoint) Client.Client.RemoteEndPoint).Address.ToString() : "";
        public bool Connected => !IsDisposed && Client != null && Client.Client.Connected;
        public int DataAvailable => !IsDisposed && Client != null ? Client.Available : 0;

        private TcpClient Client { get; set; }
        private Stream Stream { get; set; }

        private bool IsDisposed { get; set; }


        public TCPClientClass() { }
        public TCPClientClass(TcpClient tcpClient)
        {
            Client = tcpClient;
            Client.SendTimeout = 5;
            Client.ReceiveTimeout = 5;
            Client.NoDelay = false;
            Stream = Client.GetStream();

        }

        public ITCPClient Connect(string ip, ushort port)
        {
            if (Connected)
                Disconnect();

            Client = new TcpClient(ip, port) { SendTimeout = 5, ReceiveTimeout = 5, NoDelay = false };
            Stream = Client.GetStream();

            return this;
        }
        public ITCPClient Disconnect()
        {
            if (Connected)
                Client.Client.Disconnect(false);

            return this;
        }

        public void WriteByteArray(byte[] array)
        {
            if (IsDisposed)
                return;

            try
            {
                var length = array.Length;
                var buffer = length < Client.SendBufferSize ?
                    new byte[length] :
                    new byte[Client.ReceiveBufferSize];

                var totalWritedLength = 0;
                using (var data = new MemoryStream(array))
                {
                    do
                    {
                        var writedLength = data.Read(buffer, 0, buffer.Length);
                        Stream.Write(buffer, 0, buffer.Length);
                        totalWritedLength += writedLength;
                    } while (totalWritedLength < length);
                }
            }
            catch (IOException) { Dispose(); }
            catch (SocketException) { Dispose(); }
        }
        public byte[] ReadByteArray(int length)
        {
            if (IsDisposed)
                return new byte[0];

            try
            {
                var buffer = length < Client.ReceiveBufferSize ?
                    new byte[length] :
                    new byte[Client.ReceiveBufferSize];

                var totalNumberOfBytesRead = 0;
                using (var receivedData = new MemoryStream())
                {
                    do
                    {
                        var numberOfBytesRead = Stream.Read(buffer, 0, buffer.Length);
                        if (numberOfBytesRead == 0)
                            while (DataAvailable <= 0)
                            {
                                Thread.Sleep(1);
                            }

                        receivedData.Write(buffer, 0, buffer.Length); //Write to memory stream
                        totalNumberOfBytesRead += numberOfBytesRead;
                    } while (totalNumberOfBytesRead < length);

                    return receivedData.ToArray();
                }
            }
            catch (IOException) { Dispose(); return new byte[0]; }
            catch (SocketException) { Dispose(); return new byte[0]; }
        }

        public Stream GetStream() { return Stream; }

        public void Dispose()
        {
            if (Connected)
                Disconnect();

            IsDisposed = true;

            Client?.Close();
            Stream?.Dispose();
        }
    }

    public class TCPClientWrapperInstance : ITCPClientWrapper
    {
        public ITCPClient CreateTCPClient() { return new TCPClientClass(); }
    }
}
