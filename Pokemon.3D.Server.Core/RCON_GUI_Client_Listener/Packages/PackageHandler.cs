using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;
using Pokemon_3D_Server_Core.Server_Client_Listener.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Events;

namespace Pokemon_3D_Server_Core.RCON_GUI_Client_Listener.Packages
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
                    case (int)Package.PackageTypes.Ping:
                        Core.Logger.Log("Unable to handle this package as it is not getable.", Logger.LogTypes.Debug, p.Client);
                        break;

                    case (int)Package.PackageTypes.Kick:
                        HandleKick(p);
                        break;

                    case (int)Package.PackageTypes.AddPlayer:
                        HandleAddPlayer(p);
                        break;

                    case (int)Package.PackageTypes.UpdatePlayer:
                        HandleUpdatePlayer(p);
                        break;

                    case (int)Package.PackageTypes.RemovePlayer:
                        HandleRemovePlayer(p);
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
                        Core.RCONGUIDownloadQueue.HandlePackage(p);
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

        private void HandleKick(Package p)
        {
            Core.Logger.Log($"You have been kicked with the following reason: {p.DataItems[0]}", Logger.LogTypes.Info);
            Core.RCONGUIListener.Dispose();
        }

        private void HandleAddPlayer(Package p)
        {
            PlayerEvent.Invoke(PlayerEvent.Types.Add, p.DataItems[0]);
        }

        private void HandleUpdatePlayer(Package p)
        {
            PlayerEvent.Invoke(PlayerEvent.Types.Update, p.DataItems[0]);
        }

        private void HandleRemovePlayer(Package p)
        {
            PlayerEvent.Invoke(PlayerEvent.Types.Remove, p.DataItems[0]);
        }

        private void HandleLogger(Package p)
        {
            LoggerEvent.Invoke(p.DataItems[0]);
        }
    }
}
