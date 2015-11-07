using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Aragas.Core.Data;
using PokeD.Core.Data.Structs;
using PokeD.Core.Packets.SCON;
using PokeD.Core.Packets.SCON.Authorization;
using PokeD.Core.Packets.SCON.Chat;
using PokeD.Core.Packets.SCON.Logs;
using PokeD.Core.Packets.SCON.Status;
using Pokemon_3D_Server_Core.Shared.jianmingyong.Modules;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.SCON
{
    public partial class SCONClient
    {
        AuthorizationStatus AuthorizationStatus { get; set; }

        byte[] VerificationToken { get; set; }
        bool Authorized { get; set; }

        private void HandleAuthorizationRequest(AuthorizationRequestPacket packet)
        {
            if (Authorized)
                return;

            SendPacket(new AuthorizationResponsePacket { AuthorizationStatus = AuthorizationStatus });
        }
        private void HandleAuthorizationPassword(AuthorizationPasswordPacket packet)
        {
            if(Authorized)
                return;

            if (AuthorizationStatus.HasFlag(AuthorizationStatus.RemoteClientEnabled))
            {
                if (Core.Setting.SCONPassword.Hash == packet.PasswordHash)
                {
                    Authorized = true;
                    SendPacket(new AuthorizationCompletePacket());

                    IsInitialized = true;
                }
                else
                    SendPacket(new AuthorizationDisconnectPacket { Reason = "Password not correct!" });
            }
            else
                SendPacket(new AuthorizationDisconnectPacket {Reason = "Remote Client not enabled!"});
        }

        private void HandleExecuteCommand(ExecuteCommandPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            Package Package = new Package(Package.PackageTypes.ChatMessage, packet.Command, null);
            Core.Command.HandleAllCommand(Package);
        }
        
        /// <summary>
        /// Not used right now.
        /// </summary>
        private void HandleStartChatReceiving(StartChatReceivingPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            //ChatReceiving = true;
        }
        /// <summary>
        /// Not used right now.
        /// </summary>
        private void HandleStopChatReceiving(StopChatReceivingPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            //ChatReceiving = false;
        }

        private void HandlePlayerInfoListRequest(PlayerInfoListRequestPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            List<PlayerInfo> Player = new List<PlayerInfo>();
            for (int i = 0; i < Core.Pokemon3DPlayer.Count; i++)
            {
                Player.Add(new PlayerInfo()
                {
                    Name = Core.Pokemon3DPlayer[i].Name,
                    GameJoltID = Core.Pokemon3DPlayer[i].GameJoltID.ToString().ToLong(),
                    IP = ((IPEndPoint)Core.Pokemon3DPlayer[i].Network.Client.Client.RemoteEndPoint).Address.ToString(),
                    LevelFile = Core.Pokemon3DPlayer[i].LevelFile,
                    Position = new Vector3(Core.Pokemon3DPlayer[i].Position_X, Core.Pokemon3DPlayer[i].Position_Y, Core.Pokemon3DPlayer[i].Position_Z),
                    Ping = 0,
                    PlayTime = DateTime.Now - Core.Pokemon3DPlayer[i].Network.LoginStartTime
                });
            }

            SendPacket(new PlayerInfoListResponsePacket { PlayerInfoList = new PlayerInfoList(Player.ToArray()) });
        }

        private void HandleLogListRequest(LogListRequestPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            List<Log> Logs = new List<Log>();
            for (int i = 0; i < Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\Logger").Length; i++)
            {
                Logs.Add(new Log() { LogFileName = Path.GetFileName(Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\Logger")[i]) });
                SendPacket(new LogListResponsePacket { LogList = new LogList(Logs.ToArray()) });
            }
        }
        private void HandleLogFileRequest(LogFileRequestPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            if (File.Exists(Core.Setting.ApplicationDirectory + "\\Logger\\" + packet.LogFilename))
            {
                SendPacket(new LogFileResponsePacket { LogFilename = packet.LogFilename, LogFile = File.ReadAllText(Core.Setting.ApplicationDirectory + "\\Logger\\" + packet.LogFilename) });
            }
        }

        private void HandleCrashLogListRequest(CrashLogListRequestPacket packet)
        {
            // Rewrite
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            List<Log> Logs = new List<Log>();
            for (int i = 0; i < Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\CrashLogs").Length; i++)
            {
                Logs.Add(new Log() { LogFileName = Path.GetFileName(Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\CrashLogs")[i]) });
                SendPacket(new CrashLogListResponsePacket { CrashLogList = new LogList(Logs.ToArray()) });
            }
        }
        private void HandleCrashLogFileRequest(CrashLogFileRequestPacket packet)
        {
            // Rewrite
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            if (File.Exists(Core.Setting.ApplicationDirectory + "\\CrashLogs\\" + packet.CrashLogFilename))
            {
                SendPacket(new CrashLogFileResponsePacket { CrashLogFilename = packet.CrashLogFilename, CrashLogFile = File.ReadAllText(Core.Setting.ApplicationDirectory + "\\CrashLogs\\" + packet.CrashLogFilename) });
            }
        }

        /// <summary>
        /// Delete if you don't have any database
        /// </summary>
        private void HandlePlayerDatabaseListRequest(PlayerDatabaseListRequestPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            // SendPacket(new PlayerDatabaseListResponsePacket { PlayerDatabaseList = new PlayerDatabaseList() });
        }

        /// <summary>
        /// Delete if you don't have any banlist
        /// </summary>
        private void HandleBanListRequest(BanListRequestPacket packet)
        {
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            // SendPacket(new BanListResponsePacket { BanList = new BanList() });
        }
    }
}