using System;
using System.Collections.Generic;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using PokeD.Core.Packets;
using PokeD.Core.Packets.SCON;
using PokeD.Core.Packets.SCON.Authorization;
using PokeD.Core.Packets.SCON.Chat;
using PokeD.Core.Packets.SCON.Logs;
using PokeD.Core.Packets.SCON.Status;

using Pokemon_3D_Server_Core.SCON_Client_Listener.Servers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.SCON
{
    public partial class SCONClient 
    {

        #region Values

        public string IP => Stream.Host;

        public DateTime ConnectionTime { get; } = DateTime.Now;

        bool EncryptionEnabled => Module.EncryptionEnabled;

        bool IsInitialized { get; set; }

        public bool ChatReceiving { get; set; }

        #endregion Values

        ProtobufStream Stream { get; }

        ModuleSCON Module { get; }

#if DEBUG
        // -- Debug -- //
        List<SCONPacket> Received { get; } = new List<SCONPacket>();
        List<SCONPacket> Sended { get; } = new List<SCONPacket>();
        // -- Debug -- //
#endif

        public SCONClient(ITCPClient clientWrapper, ModuleSCON server)
        {
            Stream = new ProtobufStream(clientWrapper);
            Module = server;

            AuthorizationStatus = (EncryptionEnabled ? AuthorizationStatus.EncryprionEnabled : 0);
        }

        public void Update()
        {
            if (Stream.Connected)
            {
                if (Stream.DataAvailable > 0)
                {
                    var dataLength = Stream.ReadVarInt();
                    if (dataLength != 0)
                    {
                        var data = Stream.ReadByteArray(dataLength);

                        HandleData(data);
                    }
                    else
                    {
                        Core.Logger.Log($"Protobuf Reading Error: Packet Length size is 0. Disconnecting.");
                        Module.RemoveClient(this, "Packet Length size is 0!");
                    }
                }
            }
            else
                Module.RemoveClient(this);
        }

        private void HandleData(byte[] data)
        {
            if (data != null)
            {
                using (PacketDataReader reader = new ProtobufDataReader(data))
                {
                    var id = reader.Read<VarInt>();

                    if (SCONPacketResponses.Packets.Length > id)
                    {
                        if (SCONPacketResponses.Packets[id] != null)
                        {
                            var packet = SCONPacketResponses.Packets[id]().ReadPacket(reader) as SCONPacket;
                            if (packet != null)
                            {
                                HandlePacket(packet);

#if DEBUG
                                Received.Add(packet);
#endif
                            }
                            else
                                Core.Logger.Log($"SCON Reading Error: packet is null. Packet ID {id}"); // TODO: Disconnect?
                        }
                        else
                            Core.Logger.Log($"SCON Reading Error: SCONPacketResponses.Packets[{id}] is null.");
                    }
                    else
                    {
                        Core.Logger.Log($"SCON Reading Error: Packet ID {id} is not correct, Packet Data: {data}. Disconnecting.");
                        Module.RemoveClient(this, $"Packet ID {id} is not correct!");
                    }
                }
            }
            else
                Core.Logger.Log($"SCON Reading Error: Packet Data is null.");
        }
        private void HandlePacket(ProtobufPacket packet)
        {
            switch ((SCONPacketTypes)(int)packet.ID)
            {
                case SCONPacketTypes.AuthorizationRequest:
                    HandleAuthorizationRequest((AuthorizationRequestPacket)packet);
                    break;


                //case SCONPacketTypes.EncryptionResponse:
                //    HandleEncryptionResponse((EncryptionResponsePacket)packet);
                //    break;


                case SCONPacketTypes.AuthorizationPassword:
                    HandleAuthorizationPassword((AuthorizationPasswordPacket)packet);
                    break;


                case SCONPacketTypes.ExecuteCommand:
                    HandleExecuteCommand((ExecuteCommandPacket)packet);
                    break;


                case SCONPacketTypes.StartChatReceiving:
                    HandleStartChatReceiving((StartChatReceivingPacket)packet);
                    break;

                case SCONPacketTypes.StopChatReceiving:
                    HandleStopChatReceiving((StopChatReceivingPacket)packet);
                    break;


                case SCONPacketTypes.PlayerInfoListRequest:
                    HandlePlayerInfoListRequest((PlayerInfoListRequestPacket)packet);
                    break;


                case SCONPacketTypes.LogListRequest:
                    HandleLogListRequest((LogListRequestPacket)packet);
                    break;

                case SCONPacketTypes.LogFileRequest:
                    HandleLogFileRequest((LogFileRequestPacket)packet);
                    break;


                case SCONPacketTypes.CrashLogListRequest:
                    HandleCrashLogListRequest((CrashLogListRequestPacket)packet);
                    break;

                case SCONPacketTypes.CrashLogFileRequest:
                    HandleCrashLogFileRequest((CrashLogFileRequestPacket)packet);
                    break;


                case SCONPacketTypes.PlayerDatabaseListRequest:
                    HandlePlayerDatabaseListRequest((PlayerDatabaseListRequestPacket)packet);
                    break;


                case SCONPacketTypes.BanListRequest:
                    HandleBanListRequest((BanListRequestPacket)packet);
                    break;


                //case SCONPacketTypes.UploadLuaToServer:
                //    HandleUploadLuaToServer((UploadLuaToServerPacket)packet);
                //    break;

                //case SCONPacketTypes.ReloadNPCs:
                //    HandleReloadNPCs((ReloadNPCsPacket)packet);
                //    break;
            }
        }

        public void SendPacket(ProtobufPacket packet, int originID = 0)
        {
            var sconPacket = packet as SCONPacket;
            if (sconPacket == null)
                throw new Exception($"Wrong packet type, {packet.GetType().FullName}");

            Stream.SendPacket(ref packet);

#if DEBUG
            Sended.Add(sconPacket);
#endif
        }


        public void Dispose()
        {
            Stream.Disconnect();
            Stream.Dispose();
        }
    }
}