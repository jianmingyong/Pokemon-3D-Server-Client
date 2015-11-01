using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.RCON_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;

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

                    case (int)Package.PackageTypes.Logger:

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
            if (p.DataItems[0] == Core.Setting.RCONPassword.Md5HashGenerator() && p.DataItems[1] == Core.Setting.RCONPassword.SHA1HashGenerator() && p.DataItems[2] == Core.Setting.RCONPassword.SHA256HashGenerator())
            {
                Core.RCONPlayer.Add(p);
            }
            else
            {
                Core.RCONPlayer.SentToPlayer(new Package(Package.PackageTypes.Authentication, "0", p.Client));
            }
        }

        private void HandlePing(Package p)
        {
            Player Player = Core.RCONPlayer.GetPlayer(p.Client);
            Player.Network.LastValidPing = DateTime.Now;
        }

        private void HandleLogger(Package p)
        {
            Core.Command.HandleAllCommand(new Server_Client_Listener.Packages.Package(Server_Client_Listener.Packages.Package.PackageTypes.ChatMessage, p.DataItems[0], null));
        }
    }
}
