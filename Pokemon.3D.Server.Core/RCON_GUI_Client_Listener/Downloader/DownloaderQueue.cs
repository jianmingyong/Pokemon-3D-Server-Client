using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Downloader
{
    /// <summary>
    /// Class containing DownloadQueue
    /// </summary>
    public class DownloaderQueue : List<DownloadFile>
    {
        /// <summary>
        /// Get/Set Download Type
        /// </summary>
        public DownloadFile.FileType DownloadType { get; set; }

        /// <summary>
        /// HandlePackage
        /// </summary>
        /// <param name="p">Package Data</param>
        public void HandlePackage(Package p)
        {
            if (p.PackageType == (int)Package.PackageTypes.GetAllCrashLogs || p.PackageType == (int)Package.PackageTypes.GetAllLogs)
            {
                if (p.DataItems[0] == Package.FileRequestStatus.Success.ToString())
                {

                }
                else if (p.DataItems[0] == Package.FileRequestStatus.Failed.ToString())
                {

                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.BeginCreateFile)
            {
                Add(new DownloadFile(p, DownloadType));
            }
            else if (p.PackageType == (int)Package.PackageTypes.BeginDownloadFile || p.PackageType == (int)Package.PackageTypes.EndDownloadFile || p.PackageType == (int)Package.PackageTypes.EndCreateFile)
            {
                DownloadFile File = (from DownloadFile u in Core.RCONGUIDownloadQueue where p.DataItems[0].ToInt() == u.ID select u).FirstOrDefault();

                if (File != null)
                {
                    File.HandlePackage(p);
                }
            }
        }

        /// <summary>
        /// Dispose Download Queue.
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < Count; i++)
            {
                this[i].Dispose();
            }

            RemoveRange(0, Count);
        }
    }
}
