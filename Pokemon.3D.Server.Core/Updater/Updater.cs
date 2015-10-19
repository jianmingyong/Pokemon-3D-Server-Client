using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

namespace Pokemon_3D_Server_Core.Updater
{
    /// <summary>
    /// Class containing Remote Update.
    /// </summary>
    public class Updater
    {
        private string DownloadString { get; set; }

        /// <summary>
        /// Check for update.
        /// </summary>
        public bool CheckForUpdate()
        {
            using (WebClient Client = new WebClient())
            {
                try
                {
                    // Bypass all Cache. Make sure we get the latest one as quick as possible.
                    Client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    DownloadString = Client.DownloadString("https://github.com/jianmingyong/Pokemon-3D-Server-Client/blob/master/Update.dat");

                    if (Core.Setting.ApplicationVersion == DownloadString.GetSplit(0))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        public void Update()
        {
            if (!Directory.Exists(Core.Setting.ApplicationDirectory + "\\Downloads"))
            {
                Directory.CreateDirectory(Core.Setting.ApplicationDirectory + "\\Downloads");
            }

            using (WebClient Client = new WebClient())
            {
                try
                {
                    // Bypass all Cache. Make sure we get the latest one as quick as possible.
                    Client.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    Client.DownloadFile(DownloadString.GetSplit(1), Core.Setting.ApplicationDirectory + "\\Downloads\\Release.zip");

                    if (Functions.Md5HashGenerator(Core.Setting.ApplicationDirectory + "\\Downloads\\Release.zip", true) == DownloadString.GetSplit(2))
                    {
                        ClientEvent.Invoke(ClientEvent.Types.Update, null);
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}
