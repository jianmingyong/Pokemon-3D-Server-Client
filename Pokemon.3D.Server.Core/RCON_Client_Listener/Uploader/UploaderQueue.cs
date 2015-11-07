using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Uploader
{
    /// <summary>
    /// Class containing UploaderQueue
    /// </summary>
    public class UploaderQueue : List<UploadFile>
    {
        /// <summary>
        /// HandlePackage
        /// </summary>
        /// <param name="p">Package Data</param>
        public void HandlePackage(Package p)
        {
            if (p.PackageType == (int)Package.PackageTypes.GetAllCrashLogs)
            {
                int NumberofFiles = 0;

                foreach (string File in Directory.EnumerateFiles(Core.Setting.ApplicationDirectory + "\\CrashLogs"))
                {
                    Add(new UploadFile(GetNextValidID(), NumberofFiles, Path.GetFileName(File), UploadFile.FileType.CrashLog, p.Client));
                    NumberofFiles += 1;
                }

                if (NumberofFiles == 0)
                {
                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.GetAllCrashLogs, Package.FileRequestStatus.Failed.ToString(), p.Client));
                }
                else
                {
                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.GetAllCrashLogs, Package.FileRequestStatus.Success.ToString(), p.Client));
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.GetAllLogs)
            {
                int NumberofFiles = 0;

                foreach (string File in Directory.EnumerateFiles(Core.Setting.ApplicationDirectory + "\\Logger"))
                {
                    Add(new UploadFile(GetNextValidID(), NumberofFiles, Path.GetFileName(File), UploadFile.FileType.Logger, p.Client));
                    NumberofFiles += 1;
                }

                if (NumberofFiles == 0)
                {
                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.GetAllLogs, Package.FileRequestStatus.Failed.ToString(), p.Client));
                }
                else
                {
                    Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.GetAllLogs, Package.FileRequestStatus.Success.ToString(), p.Client));
                }
            }
            else if (p.PackageType == (int)Package.PackageTypes.BeginCreateFile || p.PackageType == (int)Package.PackageTypes.BeginDownloadFile || p.PackageType == (int)Package.PackageTypes.EndDownloadFile || p.PackageType == (int)Package.PackageTypes.EndCreateFile)
            {
                UploadFile File = (from UploadFile u in Core.RCONUploadQueue where p.Client == u.Client && p.DataItems[0].ToInt() == u.FileID select u).FirstOrDefault();
                if (File != null)
                {
                    File.HandlePackage(p);
                }
            }
        }

        private int GetNextValidID()
        {
            if (Count == 0)
            {
                return 0;
            }
            else
            {
                int ValidID = 0;
                List<UploadFile> ListOfFiles = (from UploadFile p in Core.RCONUploadQueue orderby p.ID ascending select p).ToList();

                for (int i = 0; i < ListOfFiles.Count; i++)
                {
                    if (ValidID == ListOfFiles[i].ID)
                    {
                        ValidID++;
                    }
                    else
                    {
                        return ValidID;
                    }
                }
                return ValidID;
            }
        }
    }
}
