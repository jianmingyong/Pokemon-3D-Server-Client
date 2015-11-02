using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using System.Diagnostics;

namespace Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Downloader
{
    /// <summary>
    /// Class containing File to Download.
    /// </summary>
    public class DownloadFile
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
        /// Get/Set File Type
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Get/Set Current Bytes
        /// </summary>
        public long CurrentBytes { get; set; } = 0;

        /// <summary>
        /// Get/Set Total Bytes
        /// </summary>
        public long TotalBytes { get; set; } = 0;

        public long Speed { get; set; } = 0;

        /// <summary>
        /// Get/Set Status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Download Status
        /// </summary>
        public enum DownloadStatus
        {
            /// <summary>
            /// Download Status: Initializing
            /// </summary>
            Initializing,

            /// <summary>
            /// Download Status: Downloading
            /// </summary>
            Downloading,

            /// <summary>
            /// Download Status: Completed
            /// </summary>
            Completed,

            /// <summary>
            /// Download Status: Paused
            /// </summary>
            Paused,

            /// <summary>
            /// Download Status: Skipped
            /// </summary>
            Skipped,

            /// <summary>
            /// Download Status: Canceled
            /// </summary>
            Canceled,
        }

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
        private StreamWriter Writer { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private long LastDownloadSize { get; set; } = 0;
        private long CurrentLineID { get; set; } = 0;

        /// <summary>
        /// New Download File
        /// </summary>
        /// <param name="p">Package</param>
        /// <param name="Type">File Type</param>
        public DownloadFile(Package p, FileType Type)
        {
            ID = p.DataItems[0].ToInt();
            Name = p.DataItems[1];
            TotalBytes = p.DataItems[2].ToLong();

            Status = DownloadStatus.Initializing.ToString();

            if (Type == FileType.CrashLog)
            {
                if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs"))
                {
                    Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs");
                }

                if (File.Exists(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name))
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    if (Stream.Length == TotalBytes)
                    {
                        Status = DownloadStatus.Skipped.ToString();

                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { ID.ToString(), Package.BeginCreateFileStatus.FileExisted.ToString() }, null));
                    }
                    else
                    {
                        Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { ID.ToString(), Package.BeginCreateFileStatus.FileCreated.ToString() }, null));
                    }
                }
                else
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { ID.ToString(), Package.BeginCreateFileStatus.FileCreated.ToString() }, null));
                }
            }
            else if (Type == FileType.Logger)
            {
                if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Download\\Logger"))
                {
                    Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Download\\Logger");
                }

                if (File.Exists(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name))
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);

                    if (Stream.Length == TotalBytes)
                    {
                        Status = DownloadStatus.Skipped.ToString();

                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { ID.ToString(), Package.BeginCreateFileStatus.FileExisted.ToString() }, null));
                    }
                    else
                    {
                        Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { ID.ToString(), Package.BeginCreateFileStatus.FileCreated.ToString() }, null));
                    }
                }
                else
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);

                    Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginCreateFile, new List<string> { ID.ToString(), Package.BeginCreateFileStatus.FileCreated.ToString() }, null));
                }
            }
        }

        /// <summary>
        /// Handle Package
        /// </summary>
        /// <param name="p">Package</param>
        public void HandlePackage(Package p)
        {
            if (p.PackageType == (int)Package.PackageTypes.BeginDownloadFile)
            {
                if (p.DataItems[1].ToInt() == 1)
                {
                    Writer = new StreamWriter(Stream);

                    Thread Thread = new Thread(new ThreadStart(GetDownloadSpeed)) { IsBackground = true };
                    Thread.Start();
                    ThreadCollection.Add(Thread);
                }

                Download(p);
            }
            else if (p.PackageType == (int)Package.PackageTypes.EndDownloadFile)
            {
                if (CurrentBytes == TotalBytes)
                {
                    Status = DownloadStatus.Completed.ToString();
                }
                else
                {
                    Status = DownloadStatus.Canceled.ToString();
                }

                Writer.Dispose();

                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.EndDownloadFile, new List<string> { ID.ToString(), Package.EndDownloadFileStatus.DownloadStreamDisposed.ToString() }, null));
            }
            else if (p.PackageType == (int)Package.PackageTypes.EndCreateFile)
            {
                Stream.Dispose();

                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.EndCreateFile, new List<string> { ID.ToString(), Package.EndCreateFileStatus.FileStreamDisposed.ToString() }, null));
            }
        }

        private void Download(Package p)
        {
            CurrentLineID += 1;

            if (Status == DownloadStatus.Initializing.ToString())
            {
                Status = DownloadStatus.Downloading.ToString();
            }

            if (p.DataItems[1].ToInt() == CurrentLineID)
            {
                CurrentBytes += System.Text.ASCIIEncoding.UTF8.GetByteCount((p.DataItems[2] + Environment.NewLine).ToCharArray());

                Writer.WriteLine(p.DataItems[2]);
                Writer.Flush();
            }
            else
            {
                Status = DownloadStatus.Canceled.ToString();
            }

            if (Status == DownloadStatus.Canceled.ToString())
            {
                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginDownloadFile, new List<string> { ID.ToString(), Package.BeginDownloadFileStatus.Cancel.ToString() }, null));
            }
            else if (Status == DownloadStatus.Downloading.ToString())
            {
                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginDownloadFile, new List<string> { ID.ToString(), Package.BeginDownloadFileStatus.RequestNextLine.ToString() }, null));
            }
            else if (Status == DownloadStatus.Paused.ToString())
            {
                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.BeginDownloadFile, new List<string> { ID.ToString(), Package.BeginDownloadFileStatus.Pause.ToString() }, null));
            }
        }

        /// <summary>
        /// Get Download Speed
        /// </summary>
        private void GetDownloadSpeed()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            do
            {
                try
                {
                    long CurrentDownloadSize = CurrentBytes;

                    Speed = CurrentDownloadSize - LastDownloadSize;
                    LastDownloadSize = CurrentDownloadSize;
                }
                catch (Exception)
                {
                    Speed = 0;
                }

                sw.Stop();
                if (sw.ElapsedMilliseconds < 1000)
                {
                    Thread.Sleep(1000 - sw.ElapsedMilliseconds.ToString().ToInt());
                }
                sw.Restart();
            } while (Status == DownloadStatus.Downloading.ToString());
        }

        /// <summary>
        /// Download completed
        /// </summary>
        public void Dispose()
        {
            if (Writer != null) Writer.Dispose();
            if (Stream != null) Stream.Dispose();

            Status = DownloadStatus.Completed.ToString();
            Speed = 0;
        }
    }
}
