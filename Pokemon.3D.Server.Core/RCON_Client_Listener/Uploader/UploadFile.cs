using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Uploader
{
    /// <summary>
    /// Class containing File to Upload.
    /// </summary>
    public class UploadFile
    {
        /// <summary>
        /// Get/Set File ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set File Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File Type
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// Logger
            /// </summary>
            Logger,

            /// <summary>
            /// Crash Log
            /// </summary>
            CrashLog,
        }

        private FileStream Stream { get; set; }
        private StreamReader Reader { get; set; }
        private TcpClient Client { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        /// <summary>
        /// New Upload File
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="Name">File Name</param>
        /// <param name="Type">File Type</param>
        public UploadFile(int ID, string Name, FileType Type, TcpClient Client)
        {
            this.ID = ID;
            this.Name = Name;
            this.Client = Client;

            try
            {
                if (Type == FileType.CrashLog)
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\CrashLogs\\" + Name, FileMode.Open, FileAccess.ReadWrite);
                    Stream.Seek(0, SeekOrigin.Begin);
                    Reader = new StreamReader(Stream);

                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), Name, Stream.Length.ToString(), ((int)FileType.CrashLog).ToString()}, Client));
                }
                else if (Type == FileType.Logger)
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Logger\\" + Name, FileMode.Open, FileAccess.ReadWrite);
                    Stream.Seek(0, SeekOrigin.Begin);
                    Reader = new StreamReader(Stream);

                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), Name, Stream.Length.ToString(), ((int)FileType.Logger).ToString() }, Client));
                }
            }
            catch (Exception)
            {
                Dispose();
            }
        }

        /// <summary>
        /// Write the Data into the RCON.
        /// </summary>
        /// <param name="p">Package Data</param>
        public void UploadData(Package p)
        {
            if (p.PackageType == (int)Package.PackageTypes.CreateFile)
            {
                if (p.DataItems[1] == "1")
                {
                    string ReturnMessage = Reader.ReadLine();
                    
                    if (string.IsNullOrWhiteSpace(ReturnMessage))
                    {
                        Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndCreateFile, ID.ToString(), Client));
                    }
                    else
                    {
                        Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.DownloadContent, new List<string> { ID.ToString(), ReturnMessage }, Client));
                    }
                }
                else
                {
                    Dispose();
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.DownloadContent)
            {
                string ReturnMessage = Reader.ReadLine();
                if (string.IsNullOrWhiteSpace(ReturnMessage))
                {
                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndCreateFile, ID.ToString(), Client));
                }
                else
                {
                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.DownloadContent, new List<string> { ID.ToString(), ReturnMessage }, Client));
                }
            }
        }

        /// <summary>
        /// Upload completed
        /// </summary>
        public void Dispose()
        {
            if (Reader != null) Reader.Dispose();
            if (Stream != null) Stream.Dispose();

            Core.RCONUploadQueue.Remove(this);
        }
    }
}
