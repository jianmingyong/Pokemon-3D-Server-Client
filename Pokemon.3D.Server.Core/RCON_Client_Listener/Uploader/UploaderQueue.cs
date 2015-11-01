using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Packages;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using System.IO;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Uploader
{
    /// <summary>
    /// Class containing UploaderQueue
    /// </summary>
    public class UploaderQueue : List<UploadFile>
    {
        /// <summary>
        /// Check if the Upload File Queue Exist.
        /// </summary>
        /// <param name="ID">ID</param>
        public bool UploadFileExist(int ID)
        {
            if (GetUploadFile(ID) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the Upload File Object
        /// </summary>
        /// <param name="ID">ID</param>
        public UploadFile GetUploadFile(int ID)
        {
            return (from UploadFile p in Core.RCONUploadQueue where p.ID == ID select p).FirstOrDefault();
        }

        /// <summary>
        /// HandlePackage
        /// </summary>
        /// <param name="p">Package Data</param>
        public void HandlePackage(Package p)
        {
            try
            {
                if (p.PackageType == (int)Package.PackageTypes.GetAllCrashLog)
                {
                    if (Directory.Exists(Core.Setting.ApplicationDirectory + "\\CrashLogs"))
                    {
                        List<string> CrashLogs = Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\CrashLogs").ToList();

                        for (int i = 0; i < CrashLogs.Count; i++)
                        {
                            Core.RCONUploadQueue.Add(new UploadFile(GetNextValidID(), Path.GetFileName(CrashLogs[i]), UploadFile.FileType.CrashLog, p.Client));
                        }
                    }
                }
                else if (p.PackageType == (int)Package.PackageTypes.GetAllLogs)
                {
                    if (Directory.Exists(Core.Setting.ApplicationDirectory + "\\Logger"))
                    {
                        List<string> Logger = Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\Logger").ToList();

                        for (int i = 0; i < Logger.Count; i++)
                        {
                            Core.RCONUploadQueue.Add(new UploadFile(GetNextValidID(), Path.GetFileName(Logger[i]), UploadFile.FileType.Logger, p.Client));
                        }
                    }
                }
                else if (p.PackageType == (int)Package.PackageTypes.CreateFile || p.PackageType == (int)Package.PackageTypes.DownloadContent)
                {
                    if (UploadFileExist(p.DataItems[0].ToInt()))
                    {
                        GetUploadFile(p.DataItems[0].ToInt()).UploadData(p);
                    }
                }
                else if (p.PackageType == (int)Package.PackageTypes.EndCreateFile)
                {
                    GetUploadFile(p.DataItems[0].ToInt()).Dispose();
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
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
