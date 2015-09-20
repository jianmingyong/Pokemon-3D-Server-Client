using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;

namespace Pokemon_3D_Server_Core.Packages
{
    /// <summary>
    /// Class containing Package Handler
    /// </summary>
    public class PackageHandler
    {
        /// <summary>
        /// A collection of Package Data to process.
        /// </summary>
        public ConcurrentQueue<Package> PackageData { get; set; } = new ConcurrentQueue<Package>();

        /// <summary>
        /// Handle PackageData
        /// </summary>
        /// <param name="obj">Null</param>
        public void Handle(object obj = null)
        {
            try
            {
                Package p;
                if (PackageData.TryDequeue(out p))
                {
                    switch (p.PackageType)
                    {
                        case (int)Package.PackageTypes.Unknown:
                            Core.Logger.Add("PackageHandler.cs: Unable to handle the package due to unknown type.", Logger.LogTypes.Debug, p.Client);
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
                            HandleServerDataRequest(p);
                            break;
                        default:
                            Core.Logger.Add("PackageHandler.cs: Unable to handle the package due to unknown type.", Logger.LogTypes.Debug, p.Client);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.CatchError();
            }
        }

        private static void HandleGameData(Package p)
        {

        }

        private static void HandlePrivateMessage(Package p)
        {

        }

        private static void HandleChatMessage(Package p)
        {

        }

        private static void HandlePing(Package p)
        {

        }

        private static void HandleGamestateMessage(Package p)
        {

        }

        private static void HandleTradeRequest(Package p)
        {

        }

        private static void HandleTradeJoin(Package p)
        {

        }

        private static void HandleTradeQuit(Package p)
        {

        }

        private static void HandleTradeOffer(Package p)
        {

        }

        private static void HandleTradeStart(Package p)
        {

        }

        private static void HandleBattleRequest(Package p)
        {

        }

        private static void HandleBattleJoin(Package p)
        {

        }

        private static void HandleBattleQuit(Package p)
        {

        }

        private static void HandleBattleOffer(Package p)
        {

        }

        private static void HandleBattleStart(Package p)
        {

        }

        private static void HandleBattleClientData(Package p)
        {

        }

        private static void HandleBattleHostData(Package p)
        {

        }

        private static void HandleBattlePokemonData(Package p)
        {

        }

        private static void HandleServerDataRequest(Package p)
        {
            List<string> DataItems = new List<string>
            {
                Core.Player.Count.ToString(),
                Core.Setting.MaxPlayers == -1 ? int.MaxValue.ToString() : Core.Setting.MaxPlayers.ToString(),
                Core.Setting.ServerName,
                string.IsNullOrWhiteSpace(Core.Setting.ServerMessage) ? "" : Core.Setting.ServerMessage
            };

            if (Core.Player.Count > 0)
            {
                for (int i = 0; i < Core.Player.Count; i++)
                {
                    DataItems.Add(Core.Player[i].isGameJoltPlayer ? string.Format("{0} ({1})", Core.Player[i].Name, Core.Player[i].GameJoltID.ToString()) : Core.Player[i].Name);
                }
            }

            Core.Server.SentToPlayer(new Package(Package.PackageTypes.ServerInfoData, DataItems, p.Client));
        }
    }
}
