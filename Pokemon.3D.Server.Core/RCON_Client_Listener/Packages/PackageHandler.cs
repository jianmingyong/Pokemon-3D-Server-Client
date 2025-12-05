using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.RCON_Client_Listener.Packages
{
    /// <summary>
    /// Class containing Package Handler.
    /// </summary>
    public class PackageHandler
    {
        /// <summary>
        /// Handle PackageData
        /// </summary>
        /// <param name="p">Package</param>
        public void Handle(Package p)
        {
            try
            {
                switch (p.PackageType)
                {
                    case (int)Package.PackageTypes.Unknown:
                        Core.Logger.Log("Unable to handle the package due to unknown type.", Logger.LogTypes.Debug, p.Client);
                        break;

                    case (int)Package.PackageTypes.Authentication:
                        HandleAuthentication(p);
                        break;

                    case (int)Package.PackageTypes.Ping:
                        HandlePing(p);
                        break;

                    case (int)Package.PackageTypes.Kick:
                    case (int)Package.PackageTypes.AddPlayer:
                    case (int)Package.PackageTypes.UpdatePlayer:
                    case (int)Package.PackageTypes.RemovePlayer:
                        Core.Logger.Log("Unable to handle this package as it is not getable.", Logger.LogTypes.Debug, p.Client);
                        break;

                    case (int)Package.PackageTypes.Logger:
                        HandleLogger(p);
                        break;

                    case (int)Package.PackageTypes.GetAllCrashLogs:
                    case (int)Package.PackageTypes.GetAllLogs:
                    case (int)Package.PackageTypes.BeginCreateFile:
                    case (int)Package.PackageTypes.BeginDownloadFile:
                    case (int)Package.PackageTypes.EndDownloadFile:
                    case (int)Package.PackageTypes.EndCreateFile:
                        Core.RCONUploadQueue.HandlePackage(p);
                        break;

                    default:
                        Core.Logger.Log("Unable to handle the package due to unknown type.", Logger.LogTypes.Debug, p.Client);
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private void HandleAuthentication(Package p)
        {
            if (string.Equals(p.DataItems[0], Core.Setting.RCONPassword.Md5HashGenerator(), StringComparison.OrdinalIgnoreCase) && string.Equals(p.DataItems[1], Core.Setting.RCONPassword.SHA1HashGenerator(), StringComparison.OrdinalIgnoreCase) && string.Equals(p.DataItems[2], Core.Setting.RCONPassword.SHA256HashGenerator(), StringComparison.OrdinalIgnoreCase))
            {
                Core.RCONPlayer.Add(p);
            }
            else
            {
                Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.Authentication, Package.AuthenticationStatus.AccessDenied.ToString(), p.Client));
            }
        }

        private void HandlePing(Package p)
        {
            var Player = Core.RCONPlayer.GetPlayer(p.Client);
            Player.Network.LastValidPing = DateTime.Now;
        }

        private void HandleLogger(Package p)
        {
            Core.Command.HandleAllCommand(new Server_Client_Listener.Packages.Package(Server_Client_Listener.Packages.Package.PackageTypes.ChatMessage, p.DataItems[0], null));
        }
    }
}
