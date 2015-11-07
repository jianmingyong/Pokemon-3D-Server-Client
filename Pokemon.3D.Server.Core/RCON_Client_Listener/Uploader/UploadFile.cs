using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Uploader
{
    /// <summary>
    /// Class containing File to Upload.
    /// </summary>
    public class UploadFile : IDisposable
    {
        /// <summary>
        /// Get/Set Upload ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Get/Set Upload File ID
        /// </summary>
        public int FileID { get; set; }

        /// <summary>
        /// Get/Set File Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/Set File Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Get/Set TcpClient
        /// </summary>
        public TcpClient Client { get; set; }

        /// <summary>
        /// File Types
        /// </summary>
        public enum FileType
        {
            /// <summary>
            /// File Types: Crash Log
            /// </summary>
            CrashLog,

            /// <summary>
            /// File Types: Logger
            /// </summary>
            Logger,
        }

        private FileStream Stream { get; set; }
        private StreamReader Reader { get; set; }

        private long CurrentLineID { get; set; } = 0;

        /// <summary>
        /// New Upload File
        /// </summary>
        /// <param name="ID">Upload ID</param>
        /// <param name="FileID">File ID</param>
        /// <param name="Name">File Name</param>
        /// <param name="FileType">File Type</param>
        /// <param name="Client">TcpClient.</param>
        public UploadFile(int ID, int FileID, string Name, FileType FileType, TcpClient Client)
        {
            this.ID = ID;
            this.FileID = FileID;
            this.Name = Name;
            this.Type = FileType.ToString();
            this.Client = Client;

            if (FileType == FileType.CrashLog)
            {
                Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\CrashLogs\\" + Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                Stream.Seek(0, SeekOrigin.Begin);
            }
            else if (FileType == FileType.Logger)
            {
                Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Logger\\" + Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                Stream.Seek(0, SeekOrigin.Begin);
            }

            Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { FileID.ToString(), Name, Stream.Length.ToString() }, Client));
        }

        /// <summary>
        /// Handle Package
        /// </summary>
        /// <param name="p">Package</param>
        public void HandlePackage(Package p)
        {
            if (p.PackageType == (int)Package.PackageTypes.BeginCreateFile)
            {
                if (p.DataItems[1] == Package.BeginCreateFileStatus.FileCreated.ToString())
                {
                    Reader = new StreamReader(Stream);
                    Upload();
                }
                else if (p.DataItems[1] == Package.BeginCreateFileStatus.FileExisted.ToString())
                {
                    Stream.Dispose();

                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndCreateFile, FileID.ToString(), Client));
                }
                else if (p.DataItems[1] == Package.BeginCreateFileStatus.Failed.ToString())
                {
                    Stream.Dispose();

                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndCreateFile, FileID.ToString(), Client));
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.BeginDownloadFile)
            {
                if (p.DataItems[1] == Package.BeginDownloadFileStatus.RequestNextLine.ToString())
                {
                    Upload();
                }
                else if (p.DataItems[1] == Package.BeginDownloadFileStatus.Pause.ToString())
                {
                    // Do nothing, just pause here.
                }
                else if (p.DataItems[1] == Package.BeginDownloadFileStatus.Cancel.ToString())
                {
                    Reader.Dispose();

                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndDownloadFile, FileID.ToString(), Client));
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.EndDownloadFile)
            {
                if (p.DataItems[1] == Package.EndDownloadFileStatus.DownloadStreamDisposed.ToString())
                {
                    Stream.Dispose();

                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndCreateFile, FileID.ToString(), Client));
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.EndCreateFile)
            {
                if (p.DataItems[1] == Package.EndCreateFileStatus.FileStreamDisposed.ToString())
                {
                    Core.RCONUploadQueue.Remove(this);
                }
            }
        }

        private void Upload()
        {
            List<string> ContentToWrite = new List<string> { FileID.ToString() };
            string TempReader;
            long ContentBuffer = 0;

            do
            {
                if (Reader.Peek() > -1)
                {
                    CurrentLineID += 1;
                    TempReader = Reader.ReadLine();
                    ContentBuffer += System.Text.Encoding.UTF8.GetByteCount(TempReader.ToCharArray());
                    ContentToWrite.AddRange(new List<string> { CurrentLineID.ToString(), TempReader });
                }
                else
                {
                    break;
                }
            } while (ContentBuffer < 8192);

            if (ContentBuffer > 0)
            {
                Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.BeginDownloadFile, ContentToWrite, Client));
            }
            else
            {
                Reader.Dispose();
                Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.EndDownloadFile, FileID.ToString(), Client));
            }
        }

        /// <summary>
        /// Dispose if this object is still there.
        /// </summary>
        public void Dispose()
        {
            if (Reader != null) Reader.Dispose();
            if (Stream != null) Stream.Dispose();

            Core.RCONUploadQueue.Remove(this);
        }
    }
}
