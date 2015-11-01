using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Downloader
{
    /// <summary>
    /// Class containing DownloadQueue
    /// </summary>
    public class DownloaderQueue : List<DownloadFile>
    {
        /// <summary>
        /// Check if the Download File Queue Exist.
        /// </summary>
        /// <param name="ID">ID</param>
        public bool DownloadFileExist(int ID)
        {
            if (GetDownloadFile(ID) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the Download File Object
        /// </summary>
        /// <param name="ID">ID</param>
        public DownloadFile GetDownloadFile(int ID)
        {
            return (from DownloadFile p in Core.RCONGUIDownloadQueue where p.ID == ID select p).FirstOrDefault();
        }

        /// <summary>
        /// HandlePackage
        /// </summary>
        /// <param name="p">Package Data</param>
        public void HandlePackage(Package p)
        {
            if (p.PackageType == (int)Package.PackageTypes.DownloadContent)
            {
                if (DownloadFileExist(p.DataItems[0].ToInt()))
                {
                    GetDownloadFile(p.DataItems[0].ToInt()).WriteData(p);
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.CreateFile)
            {
                if (p.DataItems[3].ToInt() == (int)DownloadFile.FileType.CrashLog)
                {
                    this.Add(new DownloadFile(p, DownloadFile.FileType.CrashLog));
                }
                else if (p.DataItems[3].ToInt() == (int)DownloadFile.FileType.Logger )
                {
                    this.Add(new DownloadFile(p, DownloadFile.FileType.Logger));
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.EndCreateFile)
            {
                if (DownloadFileExist(p.DataItems[0].ToInt()))
                {
                    GetDownloadFile(p.DataItems[0].ToInt()).Dispose();
                }
            }
        }
    }
}
