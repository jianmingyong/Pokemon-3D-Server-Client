using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Global
{
    /// <summary>
    /// Class containing Package Handler
    /// </summary>
    public class PackageHandler
    {
        /// <summary>
        /// A collection of Package Data to process.
        /// </summary>
        public static ConcurrentQueue<Package> PackageData { get; set; } = new ConcurrentQueue<Package>();

        /// <summary>
        /// Handle PackageData
        /// </summary>
        public static void Handle()
        {
            try
            {
                Package p;
                if (PackageData.TryDequeue(out p))
                {
                    switch (p.PackageType)
                    {
                        case (int)Package.PackageTypes.Unknown:
                            QueueMessage.Add("PackageHandler.cs: Unable to handle the package due to unknown type.", MessageEventArgs.LogType.Debug, p.Client);
                            break;

                        case (int)Package.PackageTypes.GameData:
                            break;

                        case (int)Package.PackageTypes.PrivateMessage:
                            break;

                        case (int)Package.PackageTypes.ChatMessage:
                            break;

                        case (int)Package.PackageTypes.Ping:
                            break;

                        case (int)Package.PackageTypes.GamestateMessage:
                            break;

                        case (int)Package.PackageTypes.TradeRequest:
                            break;

                        case (int)Package.PackageTypes.TradeJoin:
                            break;

                        case (int)Package.PackageTypes.TradeQuit:
                            break;

                        case (int)Package.PackageTypes.TradeOffer:
                            break;

                        case (int)Package.PackageTypes.TradeStart:
                            break;

                        case (int)Package.PackageTypes.BattleRequest:
                            break;

                        case (int)Package.PackageTypes.BattleJoin:
                            break;

                        case (int)Package.PackageTypes.BattleQuit:
                            break;

                        case (int)Package.PackageTypes.BattleOffer:
                            break;

                        case (int)Package.PackageTypes.BattleStart:
                            break;

                        case (int)Package.PackageTypes.BattleClientData:
                            break;

                        case (int)Package.PackageTypes.BattleHostData:
                            break;

                        case (int)Package.PackageTypes.BattlePokemonData:
                            break;

                        case (int)Package.PackageTypes.ServerDataRequest:
                            break;

                        default:
                            QueueMessage.Add("PackageHandler.cs: Unable to handle the package due to unknown type.", MessageEventArgs.LogType.Debug, p.Client);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }


    }
}
