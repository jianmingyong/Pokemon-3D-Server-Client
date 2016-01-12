using System;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Threading;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Threading;

namespace Pokemon_3D_Server_Core.Shared.jianmingyong
{
    /// <summary>
    /// Class containing Updater.
    /// </summary>
    public class Updater : IDisposable
    {
        private string ExpectVersion { get; set; }
        private string ExpectMD5Checksum { get; set; }
        private string ExpectFileURL { get; set; }

        private ThreadCollection ThreadCollection { get; set; } = new ThreadCollection();

        /// <summary>
        /// Start checking for update.
        /// </summary>
        public void Update()
        {
            if (My.Computer.Network.IsAvailable)
            {
                ThreadCollection.Add(new ThreadStart(ThreadUpdate));
            }
            else
            {
                Core.Logger.Log("Unable to check for update.", Logger.LogTypes.Info);
            }
        }

        private void ThreadUpdate()
        {
            try
            {
                Core.Logger.Log("Checking for update.", Logger.LogTypes.Info);

                WebRequest Request = WebRequest.Create("https://github.com/jianmingyong/Pokemon-3D-Server-Client/raw/master/Pokemon.3D.Server.Core/Resource/Update.dat");
                Request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                Request.Timeout = 10000;

                using (WebResponse Response = Request.GetResponse())
                {
                    using (StreamReader Reader = new StreamReader(Response.GetResponseStream()))
                    {
                        string Result = Reader.ReadToEnd();

                        ExpectVersion = Result.GetSplit(0);
                        ExpectMD5Checksum = Result.GetSplit(1);
                        ExpectFileURL = Result.GetSplit(2);
                    }
                }

                if (Core.Setting.ApplicationVersion != ExpectVersion)
                {
                    
                    Core.Logger.Log($"Update found: Expect Version: {ExpectVersion}, Current Version: {Core.Setting.ApplicationVersion}.", Logger.LogTypes.Info);
                    Core.Logger.Log($"Downloading update.", Logger.LogTypes.Info);

                    Request = WebRequest.Create("https://github.com/jianmingyong/Pokemon-3D-Server-Client/raw/master/Pokemon.3D.Server.Client.Updater/bin/Release/Pokemon.3D.Server.Client.Updater.exe");
                    Request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    Request.Timeout = 10000;

                    using (WebResponse Response = Request.GetResponse())
                    {
                        using (Stream Reader = Response.GetResponseStream())
                        {
                            byte[] Buffer = new byte[65536];

                            using (FileStream Writer = new FileStream(Core.Setting.ApplicationDirectory + "\\Pokemon.3D.Server.Client.Updater.exe", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                int ReadLength = 0;
                                do
                                {
                                    ReadLength = Reader.Read(Buffer, 0, Buffer.Length);
                                    Writer.Write(Buffer, 0, ReadLength);
                                } while (ReadLength > 0);
                            }
                        }
                    }

                    Request = WebRequest.Create("https://github.com/jianmingyong/Pokemon-3D-Server-Client/raw/master/Pokemon.3D.Server.Client.Updater/bin/Release/SharpCompress.dll");
                    Request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    Request.Timeout = 10000;

                    using (WebResponse Response = Request.GetResponse())
                    {
                        using (Stream Reader = Response.GetResponseStream())
                        {
                            byte[] Buffer = new byte[65536];

                            using (FileStream Writer = new FileStream(Core.Setting.ApplicationDirectory + "\\SharpCompress.dll", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                int ReadLength = 0;
                                do
                                {
                                    ReadLength = Reader.Read(Buffer, 0, Buffer.Length);
                                    Writer.Write(Buffer, 0, ReadLength);
                                } while (ReadLength > 0);
                            }
                        }
                    }

                    Request = WebRequest.Create(ExpectFileURL);
                    Request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    Request.Timeout = 10000;

                    using (WebResponse Response = Request.GetResponse())
                    {
                        using (Stream Reader = Response.GetResponseStream())
                        {
                            byte[] Buffer = new byte[65536];

                            using (FileStream Writer = new FileStream(Core.Setting.ApplicationDirectory + "\\Pokemon.3D.Server.Client.GUI.zip", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                            {
                                int ReadLength = 0;
                                do
                                {
                                    ReadLength = Reader.Read(Buffer, 0, Buffer.Length);
                                    Writer.Write(Buffer, 0, ReadLength);
                                } while (ReadLength > 0);
                            }
                        }
                    }

                    Core.Logger.Log($"Download completed.", Logger.LogTypes.Info);
                    Core.Logger.Log($"Checking Md5 checksum.", Logger.LogTypes.Info);

                    string CurrentMD5Checksum = Functions.Md5HashGenerator(Core.Setting.ApplicationDirectory + "\\Pokemon.3D.Server.Client.GUI.zip", true);

                    if (string.Equals(CurrentMD5Checksum, ExpectMD5Checksum, StringComparison.OrdinalIgnoreCase))
                    {
                        ClientEvent.Invoke(ClientEvent.Types.Update);
                    }
                    else
                    {
                        Core.Logger.Log($"MD5 does not match. Update failed.", Logger.LogTypes.Info);
                    }
                }
                else
                {
                    Core.Logger.Log($"No update found. You are using the latest version.", Logger.LogTypes.Info);
                }
            }
            catch (Exception)
            {
                Core.Logger.Log("Update failed. Please try again later.", Logger.LogTypes.Info);
            }
            finally
            {
                Dispose();
            }
        }

        /// <summary>
        /// Dispose Updater
        /// </summary>
        public void Dispose()
        {
            ThreadCollection.Dispose();
        }
    }
}
