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

        #region MuteList
        /// <summary>
        /// Get MuteList Data.
        /// </summary>
        /// <param name="Player">Player.</param>
        /// <param name="PlayerList">Player List.</param>
        public static MuteList GetMuteList(this Player Player, Player PlayerList = null)
        {
            if (Player.isGameJoltPlayer)
            {
                if (PlayerList == null)
                {
                    return (from MuteList p in Core.Setting.MuteListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault();
                }
                else
                {
                    return (from MuteList p in PlayerList.GetOnlineSetting().MuteListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault();
                }
            }
            else
            {
                return (from MuteList p in Core.Setting.MuteListData where p.Name == Player.Name && p.GameJoltID == -1 select p).FirstOrDefault();
            }
        }

        /// <summary>
        /// Return if the player is muted.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        /// <param name="PlayerList">Player List to check. Null == Global.</param>
        public static bool IsMuteListed(this Player Player, Player PlayerList = null)
        {
            if (Core.Setting.MuteList)
            {
                if (PlayerList != null && Core.Setting.OnlineSettingList)
                {
                    MuteList MuteList = Player.GetMuteList(PlayerList);

                    if (MuteList != null)
                    {
                        if (MuteList.Duration == -1 || DateTime.Now < MuteList.StartTime.AddSeconds(MuteList.Duration))
                        {
                            return true;
                        }
                        else
                        {
                            OnlineSetting OnlineSetting = PlayerList.GetOnlineSetting();
                            OnlineSetting.MuteListData.Remove(MuteList);
                            OnlineSetting.Save();
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (PlayerList == null)
                {
                    MuteList MuteList = Player.GetMuteList();

                    if (MuteList != null)
                    {
                        if (MuteList.Duration == -1 || DateTime.Now < MuteList.StartTime.AddSeconds(MuteList.Duration))
                        {
                            return true;
                        }
                        else
                        {
                            Core.Setting.MuteListData.Remove(MuteList);
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
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Add the following Player to the MuteList.
        /// </summary>
        /// <param name="Player">Player to mute.</param>
        /// <param name="Duration">How long?</param>
        /// <param name="Reason">Reason.</param>
        /// <param name="PlayerList">Player in which is trying to mute. Null == Global.</param>
        public static void AddMuteList(this Player Player,int Duration,string Reason, Player PlayerList = null)
        {
            if (PlayerList == null)
            {
                if (Player.IsMuteListed())
                {
                    MuteList MuteList = Player.GetMuteList();
                    MuteList.Name = Player.Name;
                    MuteList.StartTime = DateTime.Now;
                    MuteList.Duration = Duration;
                    MuteList.Reason = Reason;
                    Core.Setting.Save();
                }
                else
                {
                    Core.Setting.MuteListData.Add(new MuteList(Player.Name, Player.GameJoltID, Reason, DateTime.Now, Duration));
                    Core.Setting.Save();
                }
            }
            else
            {
                if (Player.IsMuteListed(PlayerList))
                {
                    MuteList MuteList = Player.GetMuteList(PlayerList);
                    MuteList.Name = Player.Name;
                    MuteList.StartTime = DateTime.Now;
                    MuteList.Duration = Duration;
                    MuteList.Reason = Reason;
                    PlayerList.GetOnlineSetting().Save();
                }
                else
                {
                    PlayerList.GetOnlineSetting().MuteListData.Add(new MuteList(Player.Name, Player.GameJoltID, Reason, DateTime.Now, Duration));
                    PlayerList.GetOnlineSetting().Save();
                }
            }
        }

        /// <summary>
        /// Remove the following Player in the MuteList.
        /// </summary>
        /// <param name="Player">Player to unmute.</param>
        /// <param name="PlayerList">Player in which muted the client. Null == Global.</param>
        public static void RemoveMuteList(this Player Player, Player PlayerList = null)
        {
            if (PlayerList == null)
            {
                if (Player.IsMuteListed())
                {
                    Core.Setting.MuteListData.Remove(Player.GetMuteList());
                    Core.Setting.Save();
                }
            }
            else
            {
                if (Player.IsMuteListed(PlayerList))
                {
                    PlayerList.GetOnlineSetting().MuteListData.Remove(Player.GetMuteList(PlayerList));
                    PlayerList.GetOnlineSetting().Save();
                }
            }
        }
        #endregion MuteList

        #region OnlineSetting
        /// <summary>
        /// Get OnlineSetting Data.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static OnlineSetting GetOnlineSetting(this Player Player)
        {
            return Player.isGameJoltPlayer ?
                (from OnlineSetting p in Core.Setting.OnlineSettingListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() : null;
        }
        #endregion OnlineSetting

        #region OperatorList
        /// <summary>
        /// Get OperatorList Data
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static OperatorList GetOperatorList(this Player Player)
        {
            return Player.isGameJoltPlayer ?
                (from OperatorList p in Core.Setting.OperatorListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() :
                (from OperatorList p in Core.Setting.OperatorListData where p.Name == Player.Name && p.GameJoltID == -1 select p).FirstOrDefault();
        }

        /// <summary>
        /// Return if the Player is an operator.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static bool IsOperator(this Player Player)
        {
            if (Core.Setting.OperatorList)
            {
                if (Player.GetOperatorList() != null)
                {
                    return true;
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
        /// Add the following Player to the Operator List.
        /// </summary>
        /// <param name="Player">Player to op.</param>
        /// <param name="Reason">Reason.</param>
        /// <param name="OperatorLevel">OperatorLevel</param>
        public static void AddOperator(this Player Player, string Reason, int OperatorLevel)
        {
            if (Player.IsOperator())
            {
                OperatorList OperatorList = Player.GetOperatorList();
                OperatorList.Name = Player.Name;
                OperatorList.Reason = Reason;
                OperatorList.OperatorLevel = OperatorLevel;
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.OperatorListData.Add(new OperatorList(Player.Name, Player.GameJoltID, Reason, OperatorLevel));
                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Remove the following Player in the Operator List.
        /// </summary>
        /// <param name="Player">Player to deop.</param>
        public static void RemoveOperator(this Player Player)
        {
            if (Player.IsOperator())
            {
                Core.Setting.OperatorListData.Remove(Player.GetOperatorList());
                Core.Setting.Save();
            }
        }
        #endregion OperatorList

        #region SwearInfractionFilterList

        #endregion SwearInfractionFilterList

        #region SwearInfractionList

        #endregion SwearInfractionList

        #region WhiteList
        /// <summary>
        /// Get WhiteList Data
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static WhiteList GetWhiteList(this Player Player)
        {
            return Player.isGameJoltPlayer ?
                (from WhiteList p in Core.Setting.WhiteListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() :
                (from WhiteList p in Core.Setting.WhiteListData where p.Name == Player.Name && p.GameJoltID == -1 select p).FirstOrDefault();
        }

        /// <summary>
        /// Return if you are WhiteListed on the server.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static bool IsWhiteListed(this Player Player)
        {
            if (Core.Setting.WhiteList)
            {
                if (Player.GetWhiteList() != null)
                {
                    return true;
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
        /// Add the following Player to the WhiteList.
        /// </summary>
        /// <param name="Player">Player to whitelist.</param>
        /// <param name="Reason">Reason.</param>
        public static void AddWhiteList(this Player Player, string Reason)
        {
            if (Player.IsWhiteListed())
            {
                WhiteList WhiteList = Player.GetWhiteList();
                WhiteList.Name = Player.Name;
                WhiteList.Reason = Reason;
                Core.Setting.Save();
            }
            else
            {
                Core.Setting.WhiteListData.Add(new WhiteList(Player.Name, Player.GameJoltID, Reason));
                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Remove the following Player in the WhiteList.
        /// </summary>
        /// <param name="Player">Player to remove.</param>
        public static void RemoveWhiteList(this Player Player)
        {
            if (Player.IsWhiteListed())
            {
                Core.Setting.WhiteListData.Remove(Player.GetWhiteList());
                Core.Setting.Save();
            }
        }
        #endregion WhiteList
    }
}