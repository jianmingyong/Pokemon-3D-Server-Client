using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_3D_Server_Core.Players;
using Pokemon_3D_Server_Core.Settings;
using System.Net.Sockets;
using System.Net;

namespace Pokemon_3D_Server_Core.Modules
{
    /// <summary>
    /// Class containing common player operations.
    /// </summary>
    public static class PlayerStatus
    {
        #region BlackList
        /// <summary>
        /// Get BlackList Data.
        /// </summary>
        /// <param name="Player">Player to get.</param>
        public static BlackList GetBlackList(this Player Player)
        {
            return Player.isGameJoltPlayer ?
                (from BlackList p in Core.Setting.BlackListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() :
                (from BlackList p in Core.Setting.BlackListData where string.Equals(p.Name, Player.Name, StringComparison.Ordinal) && p.GameJoltID == -1 select p).FirstOrDefault();
        }

        /// <summary>
        /// Return if the player is blacklisted.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static bool IsBlackListed(this Player Player)
        {
            if (Core.Setting.BlackList)
            {
                BlackList BlackList = GetBlackList(Player);

                if (BlackList != null)
                {
                    if (BlackList.Duration == -1 || DateTime.Now < BlackList.StartTime.AddSeconds(BlackList.Duration))
                    {
                        return true;
                    }
                    else
                    {
                        Core.Setting.BlackListData.Remove(BlackList);
                        Core.Setting.Save();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add the following Player to the Blacklist.
        /// </summary>
        /// <param name="Player">Player to ban.</param>
        /// <param name="Duration">How Long?</param>
        /// <param name="Reason">Reason</param>
        public static void AddBlackList(this Player Player, int Duration, string Reason)
        {
            if (Player.IsBlackListed())
            {
                BlackList BlackList = Player.GetBlackList();
                BlackList.Name = Player.Name;
                BlackList.StartTime = DateTime.Now;
                BlackList.Duration = Duration;
                BlackList.Reason = Reason;
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.BlackListData.Add(new BlackList(Player.Name, Player.GameJoltID, Reason, DateTime.Now, Duration));
                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Remove the following Player in the BlackList.
        /// </summary>
        /// <param name="Player">Player to unban.</param>
        public static void RemoveBlackList(this Player Player)
        {
            if (Player.IsBlackListed())
            {
                Core.Setting.BlackListData.Remove(Player.GetBlackList());
                Core.Setting.Save();
            }
        }
        #endregion BlackList

        #region IPBlackList
        /// <summary>
        /// Get IP BlackList Data.
        /// </summary>
        /// <param name="Player">Player to get.</param>
        public static IPBlackList GetIPBlackList(this Player Player)
        {
            return (from IPBlackList p in Core.Setting.IPBlackListData where p.IPAddress == ((IPEndPoint)Player.Network.Client.Client.RemoteEndPoint).Address.ToString() select p).FirstOrDefault();
        }

        /// <summary>
        /// Get IP BlackList Data
        /// </summary>
        /// <param name="IPAddress">IP Address</param>
        public static IPBlackList GetIPBlackList(this string IPAddress)
        {
            return (from IPBlackList p in Core.Setting.IPBlackListData where p.IPAddress == IPAddress select p).FirstOrDefault();
        }

        /// <summary>
        /// Return if the Player is ip blacklisted.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static bool IsIPBlackListed(this Player Player)
        {
            if (Core.Setting.IPBlackList)
            {
                IPBlackList IPBlackList = GetIPBlackList(Player);

                if (IPBlackList != null)
                {
                    if (IPBlackList.Duration == -1 || DateTime.Now < IPBlackList.StartTime.AddSeconds(IPBlackList.Duration))
                    {
                        return true;
                    }
                    else
                    {
                        Core.Setting.IPBlackListData.Remove(IPBlackList);
                        Core.Setting.Save();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Return if the Player is ip blacklisted.
        /// </summary>
        /// <param name="IPAddress">IPAddress to check.</param>
        /// <returns></returns>
        public static bool IsIPBlackListed(this string IPAddress)
        {
            if (Core.Setting.IPBlackList)
            {
                IPBlackList IPBlackList = IPAddress.GetIPBlackList();

                if (IPBlackList != null)
                {
                    if (IPBlackList.Duration == -1 || DateTime.Now < IPBlackList.StartTime.AddSeconds(IPBlackList.Duration))
                    {
                        return true;
                    }
                    else
                    {
                        Core.Setting.IPBlackListData.Remove(IPBlackList);
                        Core.Setting.Save();
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add the following Player to the IPBlackList
        /// </summary>
        /// <param name="Player">Player to ban.</param>
        /// <param name="Duration">How long?</param>
        /// <param name="Reason">Reason.</param>
        public static void AddIPBlackList(this Player Player, int Duration, string Reason)
        {
            if (Player.IsIPBlackListed())
            {
                IPBlackList IPBlackList = Player.GetIPBlackList();
                IPBlackList.StartTime = DateTime.Now;
                IPBlackList.Duration = Duration;
                IPBlackList.Reason = Reason;
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.IPBlackListData.Add(new IPBlackList(((IPEndPoint)Player.Network.Client.Client.RemoteEndPoint).Address.ToString(), Reason, DateTime.Now, Duration));
                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Add the following Player to the IPBlackList.
        /// </summary>
        /// <param name="IPAddress">IPAddress</param>
        /// <param name="Duration">How long?</param>
        /// <param name="Reason">Reason</param>
        public static void AddIPBlackList(this string IPAddress, int Duration, string Reason)
        {
            if (IPAddress.IsIPBlackListed())
            {
                IPBlackList IPBlackList = IPAddress.GetIPBlackList();
                IPBlackList.StartTime = DateTime.Now;
                IPBlackList.Duration = Duration;
                IPBlackList.Reason = Reason;
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.IPBlackListData.Add(new IPBlackList(IPAddress, Reason, DateTime.Now, Duration));
                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Remove the following player in the IPBlackList.
        /// </summary>
        /// <param name="IPAddress">IPAddress</param>
        public static void RemoveIPBlackList(this string IPAddress)
        {
            if (IPAddress.IsIPBlackListed())
            {
                Core.Setting.IPBlackListData.Remove(IPAddress.GetIPBlackList());
                Core.Setting.Save();
            }
        }
        #endregion IPBlackList

        #region MapFile
        /// <summary>
        /// Get MapFileList
        /// </summary>
        /// <param name="Player">Player to get.</param>
        public static MapFileList GetMapFileList(this Player Player)
        {
            List<MapFileList> MapFileList = (from MapFileList p in Core.Setting.MapFileListData where p.Path == Player.LevelFile select p).ToList();

            for (int i = 0; i < MapFileList.Count; i++)
            {
                List<string> SupportedGameMode = MapFileList[i].GameMode;
                for (int a = 0; a < SupportedGameMode.Count; a++)
                {
                    if (string.Equals(SupportedGameMode[a].Trim(), Player.GameMode, StringComparison.OrdinalIgnoreCase))
                    {
                        return MapFileList[i];
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get the Map Name
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static string GetMapName(this Player Player)
        {
            if (Player.GetMapFileList() != null)
            {
                return Player.GetMapFileList().Name;
            }
            else
            {
                return null;
            }
        }
        #endregion MapFile
    }
}
