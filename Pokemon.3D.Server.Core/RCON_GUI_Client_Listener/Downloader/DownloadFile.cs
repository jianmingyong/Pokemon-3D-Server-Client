using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

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
        public string CurrentBytes { get; set; }

        /// <summary>
        /// Get/Set Current Bytes
        /// </summary>
        public double CurrentBytes_L { get; set; } = 0;

        /// <summary>
        /// Get/Set Total Bytes
        /// </summary>
        public string TotalBytes { get; set; }

        /// <summary>
        /// Get/Set Total Bytes
        /// </summary>
        public double TotalBytes_L { get; set; } = 0;

        /// <summary>
        /// Get/Set Speed.
        /// </summary>
        public string Speed { get; set; }

        /// <summary>
        /// Get/Set Speed.
        /// </summary>
        public double Speed_L { get; set; } = 0;

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

        private double LastDownloadSize { get; set; } = 0;
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
            TotalBytes_L = p.DataItems[2].ToLong();

            if (TotalBytes_L < 1024)
            {
                TotalBytes = TotalBytes_L.ToString("F2") + " B";
            }
            else if (TotalBytes_L < 1024 * 1024)
            {
                TotalBytes = Shared.jianmingyong.Modules.Math.Round(TotalBytes_L / 1024, 2).ToString("F2") + " KB";
            }
            else if (TotalBytes_L < 1024 * 1024 * 1024)
            {
                TotalBytes = Shared.jianmingyong.Modules.Math.Round(TotalBytes_L / 1024 / 1024,2).ToString("F2") + " MB";
            }

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

                    if (Stream.Length == TotalBytes_L || Stream.Length - Encoding.UTF8.GetByteCount(Environment.NewLine.ToCharArray()) == TotalBytes_L)
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

                    if (Stream.Length == TotalBytes_L || Stream.Length - Encoding.UTF8.GetByteCount(Environment.NewLine.ToCharArray()) == TotalBytes_L)
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
                if (CurrentBytes_L == TotalBytes_L)
                {
                    Status = DownloadStatus.Completed.ToString();
                    Speed = "0 B/s";
                }
                else if (CurrentBytes_L - Encoding.UTF8.GetByteCount(Environment.NewLine.ToCharArray()) == TotalBytes_L)
                {
                    Status = DownloadStatus.Completed.ToString();
                    Speed = "0 B/s";
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
            if (Status == DownloadStatus.Initializing.ToString())
            {
                Status = DownloadStatus.Downloading.ToString();
            }

            for (int i = 1; i < p.DataItemsCount; i += 2)
            {
                CurrentLineID += 1;

                if (p.DataItems[i].ToInt() == CurrentLineID)
                {
                    CurrentBytes_L += Encoding.UTF8.GetByteCount((p.DataItems[i + 1] + Environment.NewLine).ToCharArray());

                    if (CurrentBytes_L < 1024)
                    {
                        CurrentBytes = CurrentBytes_L.ToString("F2") + " B";
                    }
                    else if (CurrentBytes_L < 1024 * 1024)
                    {
                        CurrentBytes = Shared.jianmingyong.Modules.Math.Round(CurrentBytes_L / 1024, 2).ToString("F2") + " KB";
                    }
                    else if (CurrentBytes_L < 1024 * 1024 * 1024)
                    {
                        CurrentBytes = Shared.jianmingyong.Modules.Math.Round(CurrentBytes_L / 1024 / 1024,2).ToString("F2") + " MB";
                    }

                    Writer.WriteLine(p.DataItems[i + 1]);
                    Writer.Flush();
                }
                else
                {
                    Status = DownloadStatus.Canceled.ToString();
                    break;
                }
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
                    double CurrentDownloadSize = CurrentBytes_L;

                    Speed_L = CurrentDownloadSize - LastDownloadSize;
                    LastDownloadSize = CurrentDownloadSize;
                }
                catch (Exception)
                {
                    Speed_L = 0;
                }

                if (Speed_L < 1024)
                {
                    Speed = Speed_L.ToString("F2") + " B/s";
                }
                else if (Speed_L < 1024 * 1024)
                {
                    Speed = Shared.jianmingyong.Modules.Math.Round(Speed_L / 1024, 2).ToString("F2") + " KB/s";
                }
                else if (Speed_L < 1024 * 1024 * 1024)
                {
                    Speed = Shared.jianmingyong.Modules.Math.Round(Speed_L / 1024 / 1024,2).ToString("f2") + " MB/s";
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

            Core.RCONGUIDownloadQueue.Remove(this);
        }
    }
}
