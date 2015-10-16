using System;
using System.Collections.Generic;
using Aragas.Core.Data;
using Aragas.Core.Wrappers;
using PokeD.Core;
using PokeD.Core.Data.Structs;
using PokeD.Core.Packets.SCON;
using PokeD.Core.Packets.SCON.Authorization;
using PokeD.Core.Packets.SCON.Chat;
using PokeD.Core.Packets.SCON.Logs;
using PokeD.Core.Packets.SCON.Status;
using Pokemon_3D_Server_Core.Server_Client_Listener.Settings;
using Pokemon_3D_Server_Core.Server_Client_Listener.Packages;
using System.IO;

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
                if (Setting.SCONPassword.Hash == packet.PasswordHash)
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
            // Rewrite
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            //SendPacket(new PlayerInfoListResponsePacket { PlayerInfoList = new PlayerInfoList(_server.GetAllClientsInfo()) });
        }

        private void HandleLogListRequest(LogListRequestPacket packet)
        {
            // Rewrite 
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            List<Log> Logs = new List<Log>();
            for (int i = 0; i < Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\Logger").Length; i++)
            {
                Logs.Add(new Log() { LogFileName = Directory.GetFiles(Core.Setting.ApplicationDirectory + "\\Logger")[i] });
            }

            SendPacket(new LogListResponsePacket { LogList = new LogList(Logs.ToArray()) });
        }
        private void HandleLogFileRequest(LogFileRequestPacket packet)
        {
            // Rewrite
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            //if (FileSystemWrapper.LogFolder.CheckExistsAsync(packet.LogFilename).Result == ExistenceCheckResult.FileExists)
            //{
            //    var logText = FileSystemWrapper.LogFolder.GetFileAsync(packet.LogFilename).Result.ReadAllTextAsync().Result;

            //    SendPacket(new LogFileResponsePacket { LogFilename = packet.LogFilename, LogFile = logText });
            //}
        }

        private void HandleCrashLogListRequest(CrashLogListRequestPacket packet)
        {
            // Rewrite
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            //if (FileSystemWrapper.LogFolder.CheckExistsAsync("Crash").Result == ExistenceCheckResult.FolderExists)
            //{
            //    var list = FileSystemWrapper.LogFolder.GetFolderAsync("Crash").Result.GetFilesAsync().Result;

            //    var crashLogs = new List<Log>();
            //    foreach (var file in list)
            //        crashLogs.Add(new Log { LogFileName = file.Name });

            //    SendPacket(new CrashLogListResponsePacket { CrashLogList = new LogList(crashLogs.ToArray()) });
            //}
        }
        private void HandleCrashLogFileRequest(CrashLogFileRequestPacket packet)
        {
            // Rewrite
            if (!Authorized)
            {
                SendPacket(new AuthorizationDisconnectPacket { Reason = "Not authorized!" });
                return;
            }

            //if (FileSystemWrapper.LogFolder.CheckExistsAsync("Crash").Result == ExistenceCheckResult.FolderExists)
            //    if (FileSystemWrapper.LogFolder.GetFolderAsync("Crash").Result.CheckExistsAsync(packet.CrashLogFilename).Result == ExistenceCheckResult.FileExists)
            //    {
            //        var crashLogText = FileSystemWrapper.LogFolder.GetFolderAsync("Crash").Result.GetFileAsync(packet.CrashLogFilename).Result.ReadAllTextAsync().Result;

            //        SendPacket(new CrashLogFileResponsePacket { CrashLogFilename = packet.CrashLogFilename, CrashLogFile = crashLogText });
            //    }
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

            SendPacket(new PlayerDatabaseListResponsePacket { PlayerDatabaseList = new PlayerDatabaseList() });
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

            SendPacket(new BanListResponsePacket { BanList = new BanList() });
        }
    }
}