using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Pokemon_3D_Server_Core.Loggers;
using Pokemon_3D_Server_Core.Modules;
using Pokemon_3D_Server_Core.Packages;
using Pokemon_3D_Server_Core.Settings.Data;

namespace Pokemon_3D_Server_Core.Players
{
    /// <summary>
    /// Class containing a collection of players.
    /// </summary>
    public class PlayerCollection : List<Player>
    {
        private static readonly object Lock = new object();

        /// <summary>
        /// Add New Player to the collection.
        /// </summary>
        /// <param name="p">Package Data</param>
        public void Add(Package p)
        {
            if (p.IsValid && p.IsFullPackageData())
            {
                int ID = GetNextValidID();
                Player Player = new Player(p, ID);
            }
        }

        /// <summary>
        /// Remove Player by ID
        /// </summary>
        /// <param name="ID">Player ID</param>
        /// <param name="Reason">Reason</param>
        public void Remove(int ID, string Reason)
        {
            Player Player = GetPlayer(ID);
            Player.Network.IsActive = false;

            if (Player.isGameJoltPlayer)
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server."), null));
                Core.Logger.Log(Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server with the following reason: " + Reason), Logger.LogTypes.Info);

                OnlineSetting OnlineSetting = (from OnlineSetting p in Core.Setting.OnlineSettingListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault();
                OnlineSetting.Save();
                Core.Setting.OnlineSettingListData.Remove(OnlineSetting);
            }
            else
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server."), null));
                Core.Logger.Log(Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server with the following reason: " + Reason), Logger.LogTypes.Info);
            }

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.DestroyPlayer, Player.ID.ToString(), null));

            if (Reason != Core.Setting.Token("SERVER_PLAYERLEFT"))
            {
                Core.Player.SentToPlayer(new Package(Package.PackageTypes.Kicked, Reason, Player.Network.Client));
            }

            Player.Dispose();
            Core.Player.Remove(Player);
        }

        /// <summary>
        /// Remove Player by Name
        /// </summary>
        /// <param name="Name">Player Name</param>
        /// <param name="Reason">Reason</param>
        public void Remove(string Name, string Reason)
        {
            Player Player = GetPlayer(Name);
            Player.Network.IsActive = false;

            if (Player.isGameJoltPlayer)
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server."), null));
                Core.Logger.Log(Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server with the following reason: " + Reason), Logger.LogTypes.Info);

                OnlineSetting OnlineSetting = (from OnlineSetting p in Core.Setting.OnlineSettingListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault();
                OnlineSetting.Save();
                Core.Setting.OnlineSettingListData.Remove(OnlineSetting);
            }
            else
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server."), null));
                Core.Logger.Log(Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server with the following reason: " + Reason), Logger.LogTypes.Info);
            }

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.DestroyPlayer, Player.ID.ToString(), null));

            if (Reason != Core.Setting.Token("SERVER_PLAYERLEFT"))
            {
                Core.Player.SentToPlayer(new Package(Package.PackageTypes.Kicked, Reason, Player.Network.Client));
            }

            Player.Dispose();
            Core.Player.Remove(Player);
        }

        /// <summary>
        /// Remove Player by Client
        /// </summary>
        /// <param name="Client">Client</param>
        /// <param name="Reason">Reason</param>
        public void Remove(TcpClient Client, string Reason)
        {
            Player Player = GetPlayer(Client);
            Player.Network.IsActive = false;

            if (Player.isGameJoltPlayer)
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server."), null));
                Core.Logger.Log(Core.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString(), "left the server with the following reason: " + Reason), Logger.LogTypes.Info);

                OnlineSetting OnlineSetting = (from OnlineSetting p in Core.Setting.OnlineSettingListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault();
                OnlineSetting.Save();
                Core.Setting.OnlineSettingListData.Remove(OnlineSetting);
            }
            else
            {
                Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.ChatMessage, Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server."), null));
                Core.Logger.Log(Core.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "left the server with the following reason: " + Reason), Logger.LogTypes.Info);
            }

            Core.Player.SendToAllPlayer(new Package(Package.PackageTypes.DestroyPlayer, Player.ID.ToString(), null));

            if (Reason != Core.Setting.Token("SERVER_PLAYERLEFT"))
            {
                Core.Player.SentToPlayer(new Package(Package.PackageTypes.Kicked, Reason, Player.Network.Client));
            }

            Player.Dispose();
            Core.Player.Remove(Player);
        }

        /// <summary>
        /// Check if the player is in the collection.
        /// </summary>
        /// <param name="ID">ID of the player.</param>
        public bool HasPlayer(int ID)
        {
            if ((from Player p in Core.Player where p.ID == ID select p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the player is in the collection.
        /// </summary>
        /// <param name="Name">Name of the player.</param>
        public bool HasPlayer(string Name)
        {
            if ((from Player p in Core.Player where p.Name == Name select p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Check if the player is in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the player.</param>
        public bool HasPlayer(TcpClient Client)
        {
            if ((from Player p in Core.Player where p.Network.Client == Client select p).Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="ID">ID of the player.</param>
        public Player GetPlayer(int ID)
        {
            return (from Player p in Core.Player where p.ID == ID select p).FirstOrDefault();
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="Name">Name of the player.</param>
        public Player GetPlayer(string Name)
        {
            return (from Player p in Core.Player where p.Name == Name select p).FirstOrDefault();
        }

        /// <summary>
        /// Get the player in the collection.
        /// </summary>
        /// <param name="Client">TcpClient of the player.</param>
        public Player GetPlayer(TcpClient Client)
        {
            return (from Player p in Core.Player where p.Network.Client == Client select p).FirstOrDefault();
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
                List<Player> ListOfPlayer = (from Player p in Core.Player orderby p.ID ascending select p).ToList();

                for (int i = 0; i < ListOfPlayer.Count; i++)
                {
                    if (ValidID == ListOfPlayer[i].ID)
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

        /// <summary>
        /// Sent Package Data to Player
        /// </summary>
        /// <param name="p">Package</param>
        public void SentToPlayer(Package p)
        {
            lock (Lock)
            {
                if (Core.Player.HasPlayer(p.Client))
                {
                    GetPlayer(p.Client).Network.SentToPlayer(p);
                }
                else
                {
                    try
                    {
                        StreamWriter Writer = new StreamWriter(p.Client.GetStream());
                        Writer.WriteLine(p.ToString());
                        Writer.Flush();
                        Core.Logger.Log("Sent: " + p.ToString(), Logger.LogTypes.Debug, p.Client);
                    }
                    catch (Exception)
                    {
                        Core.Logger.Log("StreamWriter failed to send package data.", Logger.LogTypes.Debug, p.Client);
                    }
                }
            }
        }

        /// <summary>
        /// Send Package Data to all Operator.
        /// </summary>
        /// <param name="p">Package</param>
        public void SendToAllOperator(Package p)
        {
            for (int i = 0; i < Count; i++)
            {
                if (p.Client != null && Core.Player[i].Network.Client != p.Client && (Core.Player[i].IsOperator() || Core.Player[i].GameJoltID == 116016 || Core.Player[i].GameJoltID == 222452))
                {
                    Package NewPackage = p;
                    NewPackage.Client = Core.Player[i].Network.Client;
                    Core.Player.SentToPlayer(NewPackage);
                }
            }
        }

        /// <summary>
        /// Sent Package Data to All Player
        /// </summary>
        /// <param name="p">Package</param>
        public void SendToAllPlayer(Package p)
        {
            for (int i = 0; i < Count; i++)
            {
                if (p.Client == null || Core.Player[i].Network.Client != p.Client)
                {
                    Package NewPackage = p;
                    NewPackage.Client = Core.Player[i].Network.Client;
                    Core.Player.SentToPlayer(NewPackage);
                }
            }
        }
    }
}