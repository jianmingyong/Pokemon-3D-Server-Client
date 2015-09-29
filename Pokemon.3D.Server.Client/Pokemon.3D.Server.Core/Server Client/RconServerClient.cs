using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;

namespace Pokemon_3D_Server_Core.Network
{
    /// <summary>
    /// Class containing RCON Stuff.
    /// </summary>
    public class RconServerClient
    {
        private IPEndPoint IPEndPoint;
        private TcpListener Listener;
        private TcpClient Client;
        private StreamReader Reader;
        private StreamWriter Writer;
    }
}
