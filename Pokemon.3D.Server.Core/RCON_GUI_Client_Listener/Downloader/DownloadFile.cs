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
            /// Before Downloading Package
            /// </summary>
            Initializing,

            /// <summary>
            /// Retriving the File Content
            /// </summary>
            Downloading,

            /// <summary>
            /// Download is completed.
            /// </summary>
            Completed,

            /// <summary>
            /// Download failed.
            /// </summary>
            Error,

            /// <summary>
            /// File is skipped.
            /// </summary>
            Skipped,
        }

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
        private StreamWriter Writer { get; set; }

        private List<Thread> ThreadCollection { get; set; } = new List<Thread>();

        private long LastDownloadSize { get; set; } = 0;

        /// <summary>
        /// New Download File
        /// </summary>
        /// <param name="p">Package</param>
        /// <param name="Type">File Type</param>
        public DownloadFile(Package p, FileType Type)
        {
            // DataItems[0] = File ID, DataItems[1] = File Name, DataItems[2] = Expect File Size in bytes.

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

                if (File.Exists(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\"+ Name))
                {
                    if (File.ReadAllText(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name).Length * 2 == TotalBytes)
                    {
                        Status = DownloadStatus.Skipped.ToString();
                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), "0" }, null));
                    }
                    else
                    {
                        File.Delete(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name);

                        Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name, FileMode.Create, FileAccess.ReadWrite);
                        Writer = new StreamWriter(Stream);

                        Status = DownloadStatus.Downloading.ToString();
                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), "1" }, null));
                    }
                }
                else
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\CrashLogs\\" + Name, FileMode.Create, FileAccess.ReadWrite);
                    Writer = new StreamWriter(Stream);

                    Status = DownloadStatus.Downloading.ToString();
                    Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), "1" }, null));
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
                    if (File.ReadAllText(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name).Length * 2 == TotalBytes)
                    {
                        Status = DownloadStatus.Skipped.ToString();
                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), "0" }, null));
                    }
                    else
                    {
                        File.Delete(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name);

                        Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name, FileMode.Create, FileAccess.ReadWrite);
                        Writer = new StreamWriter(Stream);

                        Status = DownloadStatus.Downloading.ToString();
                        Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), "1" }, null));
                    }
                }
                else
                {
                    Stream = new FileStream(Core.Setting.ApplicationDirectory + "\\Download\\Logger\\" + Name, FileMode.Create, FileAccess.ReadWrite);
                    Writer = new StreamWriter(Stream);

                    Status = DownloadStatus.Downloading.ToString();
                    Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.CreateFile, new List<string> { ID.ToString(), "1" }, null));
                }
            }

            if (Status == DownloadStatus.Downloading.ToString())
            {
                Thread Thread = new Thread(new ThreadStart(GetDownloadSpeed)) { IsBackground = true };
                Thread.Start();
                ThreadCollection.Add(Thread);
            }
        }

        /// <summary>
        /// Write the Data into the file.
        /// </summary>
        /// <param name="p">Package Data</param>
        public void WriteData(Package p)
        {
            if (p.IsValid)
            {
                string TextToAdd = p.DataItems[1] + Environment.NewLine;
                CurrentBytes += System.Text.ASCIIEncoding.UTF8.GetByteCount(TextToAdd.ToCharArray());

                Writer.WriteLine(p.DataItems[1]);
                Writer.Flush();

                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.DownloadContent, new List<string> { ID.ToString(), "1" }, null));
            }
            else
            {
                Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.DownloadContent, new List<string> { ID.ToString(), "0" }, null));
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
            Writer.Dispose();
            Stream.Dispose();

            Status = DownloadStatus.Completed.ToString();
            Speed = 0;

            Core.RCONGUIListener.SentToServer(new Package(Package.PackageTypes.EndCreateFile, new List<string> { ID.ToString(), "1" }, null));
        }
    }
}
