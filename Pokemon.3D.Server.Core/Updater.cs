using System;
using System.Net;
using System.Net.Cache;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;

namespace Pokemon_3D_Server_Core
{
    /// <summary>
    /// Class containing Updater features.
    /// </summary>
    public class Updater
    {
        private string UpdateURL { get; set; }

        private string ExpectVersion { get; set; }
        private string ExpectMD5Checksum { get; set; }
        private string ExpectFileURL { get; set; }

        private WebClient Client { get; } = new WebClient() { CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache) };

        /// <summary>
        /// Start checking for update.
        /// </summary>
        public void Update()
        {
            UpdateURL = "https://github.com/jianmingyong/Pokemon-3D-Server-Client/raw/master/Pokemon.3D.Server.Core/Update.dat";

            Core.Logger.Log("Checking for update...", Logger.LogTypes.Info);

            Client.DownloadStringAsync(new Uri(UpdateURL));
            Client.DownloadStringCompleted += Client_DownloadStringCompleted;
        }

        private void Client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                ExpectVersion = e.Result.GetSplit(0);
                ExpectMD5Checksum = e.Result.GetSplit(1);
                ExpectFileURL = e.Result.GetSplit(2);

                if (Core.Setting.ApplicationVersion != ExpectVersion)
                {
                    Core.Logger.Log($"Update found: Expect Version: {ExpectVersion}, Current Version: {Core.Setting.ApplicationVersion}.", Logger.LogTypes.Info);
                    Core.Logger.Log($"Downloading update.", Logger.LogTypes.Info);

                    Client.DownloadFileAsync(new Uri(ExpectFileURL), Core.Setting.ApplicationDirectory + "\\Release.zip");
                    Client.DownloadFileCompleted += Client_DownloadFileCompleted;
                }
            }
            else if (e.Error != null)
            {
                e.Error.CatchError();
            }

            Client.DownloadStringCompleted -= Client_DownloadStringCompleted;
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            Core.Logger.Log($"Download completed.", Logger.LogTypes.Info);

            if (!e.Cancelled && e.Error == null)
            {
                Core.Logger.Log($"Checking Md5 checksum.", Logger.LogTypes.Info);

                string CurrentMD5Checksum = Functions.Md5HashGenerator(Core.Setting.ApplicationDirectory + "\\Release.zip", true);

                if (string.Equals(CurrentMD5Checksum, ExpectMD5Checksum, StringComparison.OrdinalIgnoreCase))
                {
                    ClientEvent.Invoke(ClientEvent.Types.Update, null);
                }
            }
            else if (e.Error != null)
            {
                e.Error.CatchError();
            }

            Client.DownloadFileCompleted -= Client_DownloadFileCompleted;
        }
    }
}
