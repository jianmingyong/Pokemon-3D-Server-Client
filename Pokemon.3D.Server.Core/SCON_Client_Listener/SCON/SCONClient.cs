using System.Collections.Generic;
using Aragas.Core.Data;
using Aragas.Core.Interfaces;
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
using Pokemon_3D_Server_Core.Server_Client_Listener.Loggers;

namespace Pokemon_3D_Server_Core.SCON_Client_Listener.SCON
{
    public partial class SCONClient
    {
        public string IP => Client.IP;

        bool IsInitialized { get; set; }
        bool IsDisposed { get; set; }

        public INetworkTCPClient Client { get; }
        ProtobufStream Stream { get; }

        private readonly SCONListener _listener;

#if DEBUG
        // -- Debug -- //
        List<ProtobufPacket> Received { get; } = new List<ProtobufPacket>();
        List<ProtobufPacket> Sended { get; } = new List<ProtobufPacket>();
        public bool ChatReceiving { get; set; }

        // -- Debug -- //
#endif

        public SCONClient(INetworkTCPClient client, SCONListener sconListener)
        {
            Client = client;
            Stream = new ProtobufStream(Client);

            _listener = sconListener;

            AuthorizationStatus = AuthorizationStatus.RemoteClientEnabled;
        }

        public void Update()
        {
            if (Stream.Connected)
            {
                if (Stream.Connected && Stream.DataAvailable > 0)
                {
                    var dataLength = Stream.ReadVarInt();
                    if (dataLength == 0)
                    {
                        Core.Logger.Log($"Protobuf Reading Error: Packet Length size is 0. Disconnecting.", Logger.LogTypes.Warning);
                        SendPacket(new AuthorizationDisconnectPacket { Reason = "Packet Length size is 0!" });
                        _listener.RemovePlayer(this);
                        return;
                    }

                    var data = Stream.ReadByteArray(dataLength);

                    HandleData(data);
                }
            }
            else
                _listener.RemovePlayer(this);
        }

        private void HandleData(byte[] data)
        {
            if (data != null)
            {
                using (IPacketDataReader reader = new ProtobufDataReader(data))
                {
                    var id = reader.Read<VarInt>();
                    var origin = reader.Read<VarInt>();

                    if (SCONPacketResponses.Packets.Length > id)
                    {
                        if (SCONPacketResponses.Packets[id] != null)
                        {
                            var packet = SCONPacketResponses.Packets[id]().ReadPacket(reader);
                            packet.Origin = origin;

                            HandlePacket(packet);

#if DEBUG
                            Received.Add(packet);
                            
#endif
                        }
                        else
                            Core.Logger.Log($"SCON Reading Error: SCONPacketResponses.Packets[{id}] is null.", Logger.LogTypes.Warning);
                    }
                    else
                    {
                        Core.Logger.Log($"SCON Reading Error: Packet ID {id} is not correct, Packet Data: {data}. Disconnecting.", Logger.LogTypes.Warning);
                        SendPacket(new AuthorizationDisconnectPacket { Reason = $"Packet ID {id} is not correct!" });
                        _listener.RemovePlayer(this);
                    }
                }
            }
            else
                Core.Logger.Log($"SCON Reading Error: Packet Data is null.", Logger.LogTypes.Warning);
        }
        private void HandlePacket(ProtobufPacket packet)
        {
            switch ((SCONPacketTypes)(int)packet.ID)
            {
                case SCONPacketTypes.AuthorizationRequest:
                    HandleAuthorizationRequest((AuthorizationRequestPacket)packet);
                    break;


                case SCONPacketTypes.EncryptionResponse:
                    break;


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
            }
        }


        private void SendPacket(ProtobufPacket packet, int originID = 0)
        {
            if (Stream.Connected)
            {
                Stream.SendPacket(ref packet);

#if DEBUG
                Sended.Add(packet);
#endif
            }
        }


        private void DisconnectAndDispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;


            Stream.Dispose();
        }
        public void Dispose()
        {
            if (IsDisposed)
                return;

            IsDisposed = true;


            DisconnectAndDispose();
        }
    }
}