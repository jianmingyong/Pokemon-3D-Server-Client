using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Pokemon_3D_Server_Core.Server_Client_Listener.Players;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings.Data;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;

namespace Pokemon_3D_Server_Core.Server_Client_Listener.Modules
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
            return (from MapFileList p in Core.Setting.MapFileListData where p.Path == Player.LevelFile && p.GameMode == Player.GameMode select p).FirstOrDefault();
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
            if (PlayerList == null)
            {
                // Take Global List Instead.
                return Player.isGameJoltPlayer ?
                    (from MuteList p in Core.Setting.MuteListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() :
                    (from MuteList p in Core.Setting.MuteListData where p.Name == Player.Name && p.GameJoltID == -1 select p).FirstOrDefault();
            }
            else
            {
                // Take the MuteList from PlayerList.
                return PlayerList.isGameJoltPlayer ?
                    Player.isGameJoltPlayer ?
                    (from MuteList p in PlayerList.GetOnlineSetting().MuteListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() :
                    (from MuteList p in PlayerList.GetOnlineSetting().MuteListData where p.Name == Player.Name && p.GameJoltID == -1 select p).FirstOrDefault() :
                    null;
            }
        }

        /// <summary>
        /// Return if the player is muted.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        /// <param name="PlayerList">Player List to check. Null == Global.</param>
        public static bool IsMuteListed(this Player Player, Player PlayerList = null)
        {
            if (PlayerList == null)
            {
                // Use Global List
                if (Core.Setting.MuteList)
                {
                    MuteList MuteList = Player.GetMuteList();
                    if (MuteList == null)
                    {
                        return false;
                    }
                    else
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
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Use PlayerList
                if (Core.Setting.OnlineSettingList)
                {
                    MuteList MuteList = Player.GetMuteList(PlayerList);
                    if (MuteList == null)
                    {
                        return false;
                    }
                    else
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
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Add the following Player to the MuteList.
        /// </summary>
        /// <param name="Player">Player to mute.</param>
        /// <param name="Duration">How long?</param>
        /// <param name="Reason">Reason.</param>
        /// <param name="PlayerList">Player in which is trying to mute. Null == Global.</param>
        public static void AddMuteList(this Player Player, int Duration, string Reason, Player PlayerList = null)
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
        /// <summary>
        /// Have you sweared?
        /// </summary>
        /// <param name="Message">The dirty message.</param>
        public static bool HaveSweared(this string Message)
        {
            if (Core.Setting.SwearInfractionList)
            {
                for (int i = 0; i < Core.Setting.SwearInfractionFilterListData.Count; i++)
                {
                    if (Regex.IsMatch(Message, Core.Setting.SwearInfractionFilterListData[i].Regex))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Swear Word.
        /// </summary>
        /// <param name="Message">The dirty message.</param>
        public static string SwearWord(this string Message)
        {
            for (int i = 0; i < Core.Setting.SwearInfractionFilterListData.Count; i++)
            {
                if (Regex.IsMatch(Message, Core.Setting.SwearInfractionFilterListData[i].Regex))
                {
                    return Regex.Match(Message, Core.Setting.SwearInfractionFilterListData[i].Regex).Groups[1].Value;
                }
            }
            return null;
        }
        #endregion SwearInfractionFilterList

        #region SwearInfractionList
        /// <summary>
        /// Get SwearInfractionList Data
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static SwearInfractionList GetSwearInfractionList(this Player Player)
        {
            return Player.isGameJoltPlayer ?
                (from SwearInfractionList p in Core.Setting.SwearInfractionListData where p.GameJoltID == Player.GameJoltID select p).FirstOrDefault() :
                (from SwearInfractionList p in Core.Setting.SwearInfractionListData where p.Name == Player.Name && p.GameJoltID == -1 select p).FirstOrDefault();
        }

        /// <summary>
        /// Return if the Player have been infracted before. No Calculation done here.
        /// </summary>
        /// <param name="Player">Player to check.</param>
        public static bool IsSwearInfracted(this Player Player)
        {
            if (Core.Setting.SwearInfractionList)
            {
                if (Player.GetSwearInfractionList() != null)
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
        /// Infract the Player with the amount of points.
        /// </summary>
        /// <param name="Player">Player to infract.</param>
        /// <param name="Points">Amount of points to infract.</param>
        public static void AddInfractionCount(this Player Player, int Points)
        {
            if (Player.IsSwearInfracted())
            {
                SwearInfractionList SwearInfractionList = Player.GetSwearInfractionList();
                SwearInfractionList.Name = Player.Name;
                if ((DateTime.Now - SwearInfractionList.StartTime).TotalDays >= Core.Setting.SwearInfractionReset)
                {
                    SwearInfractionList.Points = Points;
                }
                else
                {
                    if (SwearInfractionList.Points >= Core.Setting.SwearInfractionCap)
                    {
                        SwearInfractionList.Points = 0;
                    }
                    SwearInfractionList.Points += Points;
                }
                SwearInfractionList.StartTime = DateTime.Now;

                if (SwearInfractionList.Points >= Core.Setting.SwearInfractionCap)
                {
                    SwearInfractionList.Points = Core.Setting.SwearInfractionCap;
                    SwearInfractionList.Muted += 1;
                }

                Core.Setting.Save();
            }
            else
            {
                Core.Setting.SwearInfractionListData.Add(new SwearInfractionList(Player.Name, Player.GameJoltID, Points, 0, DateTime.Now));
                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Defract the Player with the amount of points.
        /// </summary>
        /// <param name="Player">Player to defract.</param>
        /// <param name="Points">Amount of points to defract.</param>
        public static void RemoveInfractionCount(this Player Player, int Points)
        {
            if (Player.IsSwearInfracted())
            {
                SwearInfractionList SwearInfractionList = Player.GetSwearInfractionList();
                SwearInfractionList.Name = Player.Name;
                if ((DateTime.Now - SwearInfractionList.StartTime).TotalDays >= Core.Setting.SwearInfractionReset)
                {
                    SwearInfractionList.Points = 0;
                }
                else
                {
                    SwearInfractionList.Points -= Points;
                    SwearInfractionList.Points = SwearInfractionList.Points.Clamp(0, int.MaxValue);
                }
                SwearInfractionList.StartTime = DateTime.Now;

                Core.Setting.Save();
            }
        }

        /// <summary>
        /// Reset the Player infraction.
        /// </summary>
        /// <param name="Player">Player to reset.</param>
        public static void ResetInfractionCount(this Player Player)
        {
            if (Player.IsSwearInfracted())
            {
                Core.Setting.SwearInfractionListData.Remove(Player.GetSwearInfractionList());
                Core.Setting.Save();
            }
        }
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