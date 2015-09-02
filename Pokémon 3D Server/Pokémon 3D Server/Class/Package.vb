Imports System.Net.Sockets
Imports System.Text.RegularExpressions

#If RELEASEVERSION Then
Public Class Package
    'Package data:
    'ProtcolVersion|PackageType|Origin|DataItemsCount|Offset1|Offset2|Offset3...|Data1Data2Data3
    'The package contains:
    '    - Its protocol version.
    '    - The PackageType, defining the type of the package
    '    - The Origin, indicating which computer sent this package.
    '    - The DataItemsCount tells the package how many data items it contains.
    '    - A list of offsets that separate the data.
    '    - A list of data items, that aren't separated.

    Public Enum PackageTypes As Integer
        Unknown = -1
        GameData = 0

        PrivateMessage = 2
        ChatMessage = 3
        Kicked = 4
        ID = 7
        CreatePlayer = 8
        DestroyPlayer = 9
        ServerClose = 10
        ServerMessage = 11
        WorldData = 12
        Ping = 13
        GamestateMessage = 14

        TradeRequest = 30
        TradeJoin = 31
        TradeQuit = 32

        TradeOffer = 33
        TradeStart = 34

        BattleRequest = 50
        BattleJoin = 51
        BattleQuit = 52

        BattleOffer = 53
        BattleStart = 54

        BattleClientData = 55
        BattleHostData = 56
        BattlePokemonData = 57

        ServerInfoData = 98
        ServerDataRequest = 99
    End Enum

#Region "Public Field"
    Public Property ProtocolVersion As String = Nothing
    Public Property PackageType As Integer = PackageTypes.Unknown
    Public Property Origin As Integer = -1
    Public Property DataItemsCount As Integer = 0
    Public Property DataItems As New List(Of String)
    Public Property IsValid As Boolean = False
    Public Property Client As TcpClient

#End Region

    ' Empty Server Class
    Public Sub New()
        PackageType = PackageTypes.Unknown
        Origin = -1
        DataItemsCount = 0
        DataItems = Nothing
        IsValid = False
    End Sub

    ' Full Package
    Public Sub New(ByVal FullData As String, ByVal Client As TcpClient)
        Try
            Me.Client = Client

            If FullData.Contains("|") = False Then
                Main.Main.QueueMessage("Package.vb: Package does not contains pipelines.", Main.LogType.Debug)
                IsValid = False
                Exit Sub
            End If

            Dim bits As List(Of String) = FullData.Split(CChar("|")).ToList()

            If bits.Count >= 5 Then
                'Get first part, set the protocol version:
                If String.Equals(Main.Setting.ProtocolVersion, bits(0), StringComparison.OrdinalIgnoreCase) Then
                    ProtocolVersion = bits(0)
                Else
                    Main.Main.QueueMessage("Package.vb: ProtocolVersion do not match.", Main.LogType.Debug)
                    IsValid = False
                    Exit Sub
                End If

                'Get second part, set PackageType:
                If IsNumeric(bits(1)) = True Then
                    PackageType = CInt(bits(1))
                Else
                    Main.Main.QueueMessage("Package.vb: PackageType is not numeric.", Main.LogType.Debug)
                    IsValid = False
                    Exit Sub
                End If

                'Get third part, set Origin:
                If IsNumeric(bits(2)) = True Then
                    Origin = CInt(bits(2))
                Else
                    Main.Main.QueueMessage("Package.vb: Origin is not numeric.", Main.LogType.Debug)
                    IsValid = False
                    Exit Sub
                End If

                'Get data items count:
                If IsNumeric(bits(3)) = True Then
                    DataItemsCount = CInt(bits(3))
                Else
                    Main.Main.QueueMessage("Package.vb: Data Item count is not numeric.", Main.LogType.Debug)
                    IsValid = False
                    Exit Sub
                End If

                Dim OffsetList As New List(Of Integer)

                'Count from 4th item to second last item. Those are the offsets.
                For i = 4 To DataItemsCount - 1 + 4
                    If IsNumeric(bits(i)) Then
                        OffsetList.Add(CInt(bits(i)))
                    Else
                        Main.Main.QueueMessage("Package.vb: Offset is not numeric.", Main.LogType.Debug)
                        IsValid = False
                        Exit Sub
                    End If
                Next

                'Set the datastring, its the last item in the list. If it contained any separators, they will get readded here:
                Dim dataString As String = ""
                For i = DataItemsCount + 4 To bits.Count - 1
                    If i > DataItemsCount + 4 Then
                        dataString &= "|"
                    End If
                    dataString &= bits(i)
                Next

                'Cutting the data:
                For i = 0 To OffsetList.Count - 1
                    Dim cOffset As Integer = OffsetList(i)
                    Dim length As Integer = dataString.Length - cOffset
                    If i < OffsetList.Count - 1 Then
                        length = OffsetList(i + 1) - cOffset
                    End If

                    DataItems.Add(dataString.Substring(cOffset, length))
                Next

                IsValid = True
            Else
                Main.Main.QueueMessage("Package.vb: Incompleted Package.", Main.LogType.Debug)
                IsValid = False
            End If
        Catch ex As Exception
            Main.Main.QueueMessage("Package.vb: " & ex.Message, Main.LogType.Debug)
            IsValid = False
        End Try
    End Sub

    ' Create a new Package
    Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal DataItems As List(Of String), ByVal Client As TcpClient)
        ProtocolVersion = Main.Setting.ProtocolVersion
        Me.PackageType = PackageType
        Me.Origin = Origin
        DataItemsCount = DataItems.Count
        Me.DataItems = DataItems
        IsValid = True
        Me.Client = Client
    End Sub

    Public Sub New(ByVal PackageType As PackageTypes, ByVal DataItems As List(Of String), ByVal Client As TcpClient)
        ProtocolVersion = Main.Setting.ProtocolVersion
        Me.PackageType = PackageType
        Me.Origin = -1
        DataItemsCount = DataItems.Count
        Me.DataItems = DataItems
        IsValid = True
        Me.Client = Client
    End Sub

    Public Sub New(ByVal PackageType As PackageTypes, ByVal Origin As Integer, ByVal DataItems As String, ByVal Client As TcpClient)
        ProtocolVersion = Main.Setting.ProtocolVersion
        Me.PackageType = PackageType
        Me.Origin = Origin
        DataItemsCount = 1
        Me.DataItems = {DataItems}.ToList
        IsValid = True
        Me.Client = Client
    End Sub

    Public Sub New(ByVal PackageType As PackageTypes, ByVal DataItems As String, ByVal Client As TcpClient)
        ProtocolVersion = Main.Setting.ProtocolVersion
        Me.PackageType = PackageType
        Me.Origin = -1
        DataItemsCount = 1
        Me.DataItems = {DataItems}.ToList
        IsValid = True
        Me.Client = Client
    End Sub

    Public Sub Handle(Optional ByVal state As Object = Nothing)
        Main.PackageHandler.Handle(Me)
    End Sub

    Public Function IsFullPackageData() As Boolean
        If String.IsNullOrWhiteSpace(DataItems(0)) Then
            Return False
        End If
        Return True
    End Function

    Public Overrides Function ToString() As String
        Dim outputStr As String = Main.Setting.ProtocolVersion & "|" & PackageType.ToString & "|" & Origin.ToString & "|" & DataItems.Count

        Dim currentIndex As Integer = 0
        Dim data As String = ""
        For Each dataItem As String In DataItems
            outputStr &= "|" & currentIndex.ToString
            data &= dataItem
            currentIndex += dataItem.Length
        Next

        outputStr &= "|" & data

        Return outputStr
    End Function
End Class

Public Class PackageHandler
    Public Sub Handle(ByVal p As Package)
        Select Case p.PackageType
            Case Package.PackageTypes.Unknown
                Main.Main.QueueMessage("PackageHandler.vb: Unable to handle the package due to unknown type.", Main.LogType.Info, p.Client)
            Case Package.PackageTypes.GameData
                HandleGameData(p)
            Case Package.PackageTypes.PrivateMessage
                HandlePrivateMessage(p)
            Case Package.PackageTypes.ChatMessage
                HandleChatMessage(p)
            Case Package.PackageTypes.Ping
                Main.Player.GetPlayer(p.Client).PlayerLastValidPing = Date.Now
            Case Package.PackageTypes.GamestateMessage
                HandleGamestateMessage(p)
            Case Package.PackageTypes.TradeRequest
                HandleTradeRequest(p)
            Case Package.PackageTypes.TradeJoin
                HandleTradeJoin(p)
            Case Package.PackageTypes.TradeQuit
                HandleTradeQuit(p)
            Case Package.PackageTypes.TradeOffer
                HandleTradeOffer(p)
            Case Package.PackageTypes.TradeStart
                HandleTradeStart(p)
            Case Package.PackageTypes.BattleRequest
                HandleBattleRequest(p)
            Case Package.PackageTypes.BattleJoin
                HandleBattleJoin(p)
            Case Package.PackageTypes.BattleQuit
                HandleBattleQuit(p)
            Case Package.PackageTypes.BattleOffer
                HandleBattleOffer(p)
            Case Package.PackageTypes.BattleStart
                HandleBattleStart(p)
            Case Package.PackageTypes.BattleClientData
                HandleBattleClientData(p)
            Case Package.PackageTypes.BattleHostData
                HandleBattleHostData(p)
            Case Package.PackageTypes.BattlePokemonData
                HandleBattlePokemonData(p)
            Case Package.PackageTypes.ServerDataRequest
                HandleServerDataRequest(p)
            Case Else
                Main.Main.QueueMessage("PackageHandler.vb: Unable to handle the package due to unknown type.", Main.LogType.Info, p.Client)
        End Select
    End Sub

    Private Sub HandleGameData(ByVal p As Package)
        If Not p.IsFullPackageData AndAlso Main.Player.HasPlayer(p.Client) Then
            Main.Player.GetPlayer(p.Client).Update(p, False)
        ElseIf p.IsFullPackageData AndAlso Main.Player.HasPlayer(p.Client) Then
            Main.Player.GetPlayer(p.Client).Update(p, True)
        ElseIf p.IsFullPackageData AndAlso Not Main.Player.HasPlayer(p.Client)
            Dim isGameJoltPlayer As Boolean = False

            If p.DataItems(1) = "1" Then
                isGameJoltPlayer = True
            End If

            ' Check for Max Player
            If Not Main.Setting.MaxPlayers < 0 AndAlso Main.Player.Count >= Main.Setting.MaxPlayers Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_FULL"), p.Client))
                If isGameJoltPlayer Then
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the server is full of players."), Main.LogType.Info, p.Client)
                Else
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the server is full of players."), Main.LogType.Info, p.Client)
                End If
                Exit Sub
            End If

            ' Check for GameMode
            If Main.Setting.GameMode.Contains(","c) Then
                Dim Valid As Boolean = False
                For Each gamemode As String In Main.Setting.GameMode.Split(","c)
                    If String.Equals(gamemode, p.DataItems(0), StringComparison.OrdinalIgnoreCase) Then
                        Valid = True
                    End If
                Next
                If Not Valid Then
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_CONFIGNOTMATCH", Main.Setting.GameMode & " gamemode"), p.Client))
                    If isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is not playing the correct gamemode."), Main.LogType.Info, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is not playing the correct gamemode."), Main.LogType.Info, p.Client)
                    End If
                    Exit Sub
                End If
            Else
                If Not String.Equals(Main.Setting.GameMode, p.DataItems(0), StringComparison.OrdinalIgnoreCase) Then
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_CONFIGNOTMATCH", Main.Setting.GameMode & " gamemode"), p.Client))
                    If isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is not playing the correct gamemode."), Main.LogType.Info, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is not playing the correct gamemode."), Main.LogType.Info, p.Client)
                    End If
                    Exit Sub
                End If
            End If

            ' Check for Online Mode Server
            If Not Main.Setting.OfflineMode AndAlso Not isGameJoltPlayer Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_CONFIGNOTMATCH", "online profile"), p.Client))
                If isGameJoltPlayer Then
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is playing offline profile."), Main.LogType.Info, p.Client)
                Else
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is playing offline profile."), Main.LogType.Info, p.Client)
                End If
                Exit Sub
            End If

            ' Check for BlackList
            If Main.Setting.BlackList AndAlso Main.Setting.BlackListData.Count > 0 Then
                If Main.Setting.IsBlackListed(New Player(p)) Then
                    Dim BlackListData As BlackList = Main.Setting.GetBlackListData(New Player(p))
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_BLACKLISTED", BlackListData.BanReason & " | Remaining time to expire: " & BlackListData.BanRemainingTime), p.Client))
                    If isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is banned."), Main.LogType.Info, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is banned."), Main.LogType.Info, p.Client)
                    End If
                    Exit Sub
                End If
            End If

            ' Check for IPBlackList
            If Main.Setting.IPBlackList AndAlso Main.Setting.IPBlackListData.Count > 0 Then
                If Main.Setting.IsIPBlackListed(New Player(p)) Then
                    Dim BlackListData As IPBlackList = Main.Setting.GetIPBlackListData(New Player(p))
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_BLACKLISTED", BlackListData.BanReason & " | Remaining time to expire: " & BlackListData.BanRemainingTime), p.Client))
                    If isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is ip banned."), Main.LogType.Info, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is ip banned."), Main.LogType.Info, p.Client)
                    End If
                    Exit Sub
                End If
            End If

            ' Check for WhiteList
            If Main.Setting.WhiteList AndAlso Main.Setting.WhiteListData.Count > 0 Then
                If Main.Setting.IsWhiteListed(New Player(p)) Then
                    Dim WhiteListData As WhiteList = Main.Setting.GetWhiteListData(New Player(p))
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_WHITELIST"), p.Client))
                    If isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is not whitelisted."), Main.LogType.Info, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is not whitelisted."), Main.LogType.Info, p.Client)
                    End If
                    Exit Sub
                End If
            End If

            ' Check if you are a clone.
            If Main.Player.HasPlayer(p.DataItems(4)) Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Main.Setting.Token("SERVER_PLAYERDUPLICATE"), p.Client))
                If isGameJoltPlayer Then
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", p.DataItems(4), p.DataItems(2), "is unable to join the server as the player is still on the server."), Main.LogType.Info, p.Client)
                Else
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", p.DataItems(4), "is unable to join the server as the player is still on the server."), Main.LogType.Info, p.Client)
                End If
                Exit Sub
            End If

            ' Otherwise, let the player join :)
            Main.Player.Add(p)
        End If
    End Sub

    Private Sub HandlePrivateMessage(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)

        If Not Main.Player.HasPlayer(p.DataItems(0)) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERNOTFOUND"), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a PM as the player does not exist."), Main.LogType.PM, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a PM as the player does not exist."), Main.LogType.PM, p.Client)
            End If
            Exit Sub
        End If

        If Main.Setting.IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED", Main.Setting.GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a PM due to being muted in the server."), Main.LogType.PM, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a PM due to being muted in the server."), Main.LogType.PM, p.Client)
            End If
            Exit Sub
        End If

        If Main.Setting.GetSetting(Main.Player.GetPlayer(p.DataItems(0))) IsNot Nothing AndAlso Main.Setting.GetSetting(Main.Player.GetPlayer(p.DataItems(0))).IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED2", Main.Setting.GetSetting(Main.Player.GetPlayer(p.DataItems(0))).GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetSetting(Main.Player.GetPlayer(p.DataItems(0))).GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a PM due to being muted by the player."), Main.LogType.PM, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a PM due to being muted by the player."), Main.LogType.PM, p.Client)
            End If
            Exit Sub
        End If

        If Main.Setting.SpamResetDuration > 0 Then
            If Not String.IsNullOrWhiteSpace(Player.PlayerLastChat) AndAlso Player.PlayerLastChat = p.DataItems(1) Then
                If Date.Now < Player.PlayerLastChatTime.AddSeconds(Main.Setting.SpamResetDuration) Then

                    Dim SpamResetTimeLeft As TimeSpan
                    Dim SpamResetTimeLeftText As String = Nothing

                    SpamResetTimeLeft = Player.PlayerLastChatTime.AddSeconds(Main.Setting.SpamResetDuration) - Date.Now
                    If SpamResetTimeLeft.Minutes = 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Minutes & " minute "
                    ElseIf SpamResetTimeLeft.Minutes > 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Minutes & " minutes "
                    End If

                    If SpamResetTimeLeft.Seconds = 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Seconds & " second "
                    ElseIf SpamResetTimeLeft.Seconds > 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Seconds & " seconds "
                    End If

                    Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_SPAMDETECTION", SpamResetTimeLeftText), p.Client))
                    If Player.isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have triggered the spam detection."), Main.LogType.PM, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have triggered the spam detection."), Main.LogType.PM, p.Client)
                    End If
                    Exit Sub
                End If
            End If
        End If

        If Main.Setting.PlayerSweared(p.DataItems(1)) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERSWEARED", Main.Setting.SwearedWord(p.DataItems(1))), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have sweared in the server. Triggered word: " & Main.Setting.SwearedWord(p.DataItems(1))), Main.LogType.PM, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sweared in the server. Triggered word: " & Main.Setting.SwearedWord(p.DataItems(1))), Main.LogType.PM, p.Client)
            End If
            Main.Setting.AddSwearInfracted(Player, 1)
        End If

        Main.ServerClient.SendData(New Package(Package.PackageTypes.PrivateMessage, Player.PlayerID, p.DataItems(1), Main.Player.GetPlayer(p.DataItems(0)).PlayerClient))
        Main.ServerClient.SendData(New Package(Package.PackageTypes.PrivateMessage, Player.PlayerID, p.DataItems, p.Client))

        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have sent a private message to " & p.DataItems(0)), Main.LogType.PM, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sent a private message to " & p.DataItems(0)), Main.LogType.PM, p.Client)
        End If

        Player.PlayerLastChat = p.DataItems(1)
        Player.PlayerLastChatTime = Date.Now
    End Sub

    Private Sub HandleChatMessage(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)

        If Main.Setting.IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED", Main.Setting.GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a PM due to being muted in the server."), Main.LogType.PM, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a PM due to being muted in the server."), Main.LogType.PM, p.Client)
            End If
            Exit Sub
        End If

        If Main.Setting.SpamResetDuration > 0 Then
            If Not String.IsNullOrWhiteSpace(Player.PlayerLastChat) AndAlso Player.PlayerLastChat = p.DataItems(0) Then
                If Date.Now < Player.PlayerLastChatTime.AddSeconds(Main.Setting.SpamResetDuration) Then

                    Dim SpamResetTimeLeft As TimeSpan
                    Dim SpamResetTimeLeftText As String = Nothing

                    SpamResetTimeLeft = Player.PlayerLastChatTime.AddSeconds(Main.Setting.SpamResetDuration) - Date.Now
                    If SpamResetTimeLeft.Minutes = 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Minutes & " minute "
                    ElseIf SpamResetTimeLeft.Minutes > 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Minutes & " minutes "
                    End If

                    If SpamResetTimeLeft.Seconds = 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Seconds & " second "
                    ElseIf SpamResetTimeLeft.Seconds > 1 Then
                        SpamResetTimeLeftText &= SpamResetTimeLeft.Seconds & " seconds "
                    End If

                    Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_SPAMDETECTION", SpamResetTimeLeftText), p.Client))
                    If Player.isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have triggered the spam detection."), Main.LogType.Chat, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have triggered the spam detection."), Main.LogType.Chat, p.Client)
                    End If
                    Exit Sub
                End If
            End If
        End If

        If p.DataItems(0).StartsWith("/") Then
            HandleChatCommand(p)
            Exit Sub
        End If

        If Main.Setting.PlayerSweared(p.DataItems(0)) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERSWEARED", Main.Setting.SwearedWord(p.DataItems(0))), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have sweared in the server. Triggered word: " & Main.Setting.SwearedWord(p.DataItems(0))), Main.LogType.PM, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sweared in the server. Triggered word: " & Main.Setting.SwearedWord(p.DataItems(0))), Main.LogType.PM, p.Client)
            End If
            Main.Setting.AddSwearInfracted(Player, 1)
        End If

        Dim Temp As List(Of Player) = Main.Player
        For Each Players As Player In Temp
            If Main.Setting.GetSetting(Players) IsNot Nothing AndAlso Not Main.Setting.GetSetting(Players).IsMuted(Player) Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Player.PlayerID, p.DataItems(0), Players.PlayerClient))
            ElseIf Main.Setting.GetSetting(Players) Is Nothing
                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Player.PlayerID, p.DataItems(0), Players.PlayerClient))
            End If
        Next

        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_CHATGAMEJOLT", Player.Name, Player.GameJoltID.ToString, p.DataItems(0)), Main.LogType.Chat, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_CHATNOGAMEJOLT", Player.Name, p.DataItems(0)), Main.LogType.Chat, p.Client)
        End If

        Player.PlayerLastChat = p.DataItems(0)
        Player.PlayerLastChatTime = Date.Now
    End Sub

    Private Sub HandleGamestateMessage(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        If Player.isGameJoltPlayer Then
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, "The player " & Player.Name & " (" & Player.GameJoltID.ToString & ") " & p.DataItems(0), Nothing))
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, p.DataItems(0)), Main.LogType.Server, p.Client)
        Else
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, "The player " & Player.Name & " " & p.DataItems(0), Nothing))
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, p.DataItems(0)), Main.LogType.Server, p.Client)
        End If
    End Sub

    Private Sub HandleTradeRequest(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        If Main.Setting.AutoRestartTime > 0 Then
            Dim TimeLeft As TimeSpan = (Main.LastRestartTime.AddSeconds(Main.Setting.AutoRestartTime) - Date.Now)
            Dim ReturnText As String = Nothing

            If TimeLeft.Minutes = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minute "
            ElseIf TimeLeft.Minutes > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minutes "
            End If

            If TimeLeft.Seconds = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " second "
            ElseIf TimeLeft.Seconds > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " seconds "
            End If

            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_RESTARTWARNING", ReturnText), p.Client))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeQuit, Player.PlayerID, "", p.Client))
            Exit Sub
        End If

        If Main.Setting.IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED", Main.Setting.GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a trade request due to being muted in the server."), Main.LogType.Trade, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a trade request due to being muted in the server."), Main.LogType.Trade, p.Client)
            End If
            Exit Sub
        End If

        If Main.Setting.GetSetting(TradePlayer) IsNot Nothing AndAlso Main.Setting.GetSetting(TradePlayer).IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED2", Main.Setting.GetSetting(TradePlayer).GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetSetting(TradePlayer).GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a trade request due to being muted by the player."), Main.LogType.Trade, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a trade request due to being muted by the player."), Main.LogType.Trade, p.Client)
            End If
            Exit Sub
        End If

        Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeRequest, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have sent a trade request to " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sent a trade request to " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        End If
    End Sub

    Private Sub HandleTradeJoin(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        If Main.Setting.AutoRestartTime > 0 Then
            Dim TimeLeft As TimeSpan = (Main.LastRestartTime.AddSeconds(Main.Setting.AutoRestartTime) - Date.Now)
            Dim ReturnText As String = Nothing

            If TimeLeft.Minutes = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minute "
            ElseIf TimeLeft.Minutes > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minutes "
            End If

            If TimeLeft.Seconds = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " second "
            ElseIf TimeLeft.Seconds > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " seconds "
            End If

            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_RESTARTWARNING", ReturnText), p.Client))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_RESTARTWARNING", ReturnText), TradePlayer.PlayerClient))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeQuit, TradePlayer.PlayerID, "", p.Client))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeQuit, Player.PlayerID, "", TradePlayer.PlayerClient))
            Exit Sub
        End If

        Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeJoin, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have join a trade request from " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have join a trade request from " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        End If
    End Sub

    Private Sub HandleTradeQuit(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeQuit, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have left a trade request from " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have left a trade request from " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        End If
    End Sub

    Private Sub HandleTradeOffer(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeOffer, Player.PlayerID, p.DataItems(1), TradePlayer.PlayerClient))
    End Sub

    Private Sub HandleTradeStart(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeStart, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have started a trade from " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have started a trade from " & TradePlayer.Name), Main.LogType.Trade, p.Client)
        End If
    End Sub

    Private Sub HandleBattleRequest(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        If Main.Setting.AutoRestartTime > 0 Then
            Dim TimeLeft As TimeSpan = (Main.LastRestartTime.AddSeconds(Main.Setting.AutoRestartTime) - Date.Now)
            Dim ReturnText As String = Nothing

            If TimeLeft.Minutes = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minute "
            ElseIf TimeLeft.Minutes > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minutes "
            End If

            If TimeLeft.Seconds = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " second "
            ElseIf TimeLeft.Seconds > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " seconds "
            End If

            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_RESTARTWARNING", ReturnText), p.Client))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.TradeQuit, Player.PlayerID, "", p.Client))
            Exit Sub
        End If

        If Main.Setting.IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED", Main.Setting.GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a battle request due to being muted in the server."), Main.LogType.PvP, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a battle request due to being muted in the server."), Main.LogType.PvP, p.Client)
            End If
            Exit Sub
        End If

        If Main.Setting.GetSetting(TradePlayer) IsNot Nothing AndAlso Main.Setting.GetSetting(TradePlayer).IsMuted(Player) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_PLAYERMUTED2", Main.Setting.GetSetting(TradePlayer).GetMuteListData(Player).MuteReason & " | Remaining time to expire: " & Main.Setting.GetSetting(TradePlayer).GetMuteListData(Player).BanRemainingTime), p.Client))
            If Player.isGameJoltPlayer Then
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to send a battle request due to being muted by the player."), Main.LogType.PvP, p.Client)
            Else
                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to send a battle request due to being muted by the player."), Main.LogType.PvP, p.Client)
            End If
            Exit Sub
        End If

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleRequest, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have sent a battle request to " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have sent a battle request to " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        End If
    End Sub

    Private Sub HandleBattleJoin(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        If Main.Setting.AutoRestartTime > 0 Then
            Dim TimeLeft As TimeSpan = (Main.LastRestartTime.AddSeconds(Main.Setting.AutoRestartTime) - Date.Now)
            Dim ReturnText As String = Nothing

            If TimeLeft.Minutes = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minute "
            ElseIf TimeLeft.Minutes > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Minutes & " minutes "
            End If

            If TimeLeft.Seconds = 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " second "
            ElseIf TimeLeft.Seconds > 1 AndAlso TimeLeft.Days = 0 Then
                ReturnText &= TimeLeft.Seconds & " seconds "
            End If

            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_RESTARTWARNING", ReturnText), p.Client))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_RESTARTWARNING", ReturnText), TradePlayer.PlayerClient))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleQuit, TradePlayer.PlayerID, "", p.Client))
            Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleQuit, Player.PlayerID, "", TradePlayer.PlayerClient))
            Exit Sub
        End If

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleJoin, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have join a battle request from " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have join a battle request from " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        End If
    End Sub

    Private Sub HandleBattleQuit(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleQuit, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have left a battle request from " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have left a battle request from " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        End If
    End Sub

    Private Sub HandleBattleOffer(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleOffer, Player.PlayerID, p.DataItems(1), TradePlayer.PlayerClient))
    End Sub

    Private Sub HandleBattleStart(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleStart, Player.PlayerID, "", TradePlayer.PlayerClient))
        If Player.isGameJoltPlayer Then
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have started a battle from " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        Else
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have started a battle from " & TradePlayer.Name), Main.LogType.PvP, p.Client)
        End If
    End Sub

    Private Sub HandleBattleClientData(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleClientData, Player.PlayerID, p.DataItems(1), TradePlayer.PlayerClient))
    End Sub

    Private Sub HandleBattleHostData(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattleHostData, Player.PlayerID, p.DataItems(1), TradePlayer.PlayerClient))
    End Sub

    Private Sub HandleBattlePokemonData(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim TradePlayer As Player = Main.Player.GetPlayer(CInt(p.DataItems(0)))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.BattlePokemonData, Player.PlayerID, p.DataItems(1), TradePlayer.PlayerClient))
    End Sub

    Private Sub HandleServerDataRequest(ByVal p As Package)
        Dim DataItems As New List(Of String)

        DataItems.Add(Main.Player.Count.ToString)
        If Main.Setting.MaxPlayers = -1 Then
            DataItems.Add(Integer.MaxValue.ToString)
        Else
            DataItems.Add(Main.Setting.MaxPlayers.ToString)
        End If
        DataItems.Add(Main.Setting.ServerName)
        If Main.Setting.ServerMessage Is Nothing Then
            DataItems.Add("")
        Else
            DataItems.Add(Main.Setting.ServerMessage)
        End If

        If Main.Player.Count > 0 Then
            For Each a As Player In Main.Player
                DataItems.Add(a.Name)
            Next
        End If

        Main.ServerClient.SendData(New Package(Package.PackageTypes.ServerInfoData, DataItems, p.Client))
    End Sub

    Public Sub HandleChatCommand(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)

#Region "BlackList"

        ' BlackList Feature
        ' /blacklist <Boolean>
        ' /list blacklist <Boolean>
#Region "BlackList Feature"
        'If Not client Then
        '    If Regex.IsMatch(p.DataItems(0), "\/blacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase) Then
        '        If Main.Setting.OperatorPermission(Player) >= Player.OperatorPermission.ServerModerator Then
        '            Dim Group1 As String = Regex.Match(p.DataItems(0), "\/blacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value

        '            If String.Equals(Group1, "True", StringComparison.OrdinalIgnoreCase) Then
        '                Main.Setting.BlackList = True

        '                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "You have turned on the blacklist feature.", p.Client))
        '                If Player.isGameJoltPlayer Then
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have turn on the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "have turn on the blacklist feature."), Main.LogType.Command, p.Client)
        '                Else
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn on the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn on the blacklist feature."), Main.LogType.Command, p.Client)
        '                End If
        '            ElseIf String.Equals(Group1, "False", StringComparison.OrdinalIgnoreCase)
        '                Main.Setting.BlackList = False

        '                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "You have turned off the blacklist feature.", p.Client))
        '                If Player.isGameJoltPlayer Then
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have turn off the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "have turn off the blacklist feature."), Main.LogType.Command, p.Client)
        '                Else
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn off the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn off the blacklist feature."), Main.LogType.Command, p.Client)
        '                End If
        '            Else
        '                HandleChatCommandList(New Package(Package.PackageTypes.ChatMessage, "/help blacklist", p.Client), False)
        '            End If
        '        Else
        '            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_COMMANDPERMISSION"), p.Client))
        '            If Player.isGameJoltPlayer Then
        '                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "is unable to use ""/blacklist"" due to insufficient permission."), Main.LogType.Command, p.Client)
        '            Else
        '                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "is unable to use ""/blacklist"" due to insufficient permission."), Main.LogType.Command, p.Client)
        '            End If
        '        End If
        '    ElseIf Regex.IsMatch(p.DataItems(0), "\/list\sblacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase)
        '        If Main.Setting.OperatorPermission(Player) >= Player.OperatorPermission.ServerModerator Then
        '            Dim Group1 As String = Regex.Match(p.DataItems(0), "\/list\sblacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value

        '            If String.Equals(Group1, "True", StringComparison.OrdinalIgnoreCase) Then
        '                Main.Setting.BlackList = True

        '                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "You have turned on the blacklist feature.", p.Client))
        '                If Player.isGameJoltPlayer Then
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have turn on the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "have turn on the blacklist feature."), Main.LogType.Command, p.Client)
        '                Else
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn on the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn on the blacklist feature."), Main.LogType.Command, p.Client)
        '                End If
        '            ElseIf String.Equals(Group1, "False", StringComparison.OrdinalIgnoreCase)
        '                Main.Setting.BlackList = False

        '                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "You have turned off the blacklist feature.", p.Client))
        '                If Player.isGameJoltPlayer Then
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have turn off the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "have turn off the blacklist feature."), Main.LogType.Command, p.Client)
        '                Else
        '                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn off the blacklist feature."), Player.PlayerClient))
        '                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "have turn off the blacklist feature."), Main.LogType.Command, p.Client)
        '                End If
        '            Else
        '                HandleChatCommandList(New Package(Package.PackageTypes.ChatMessage, "/help list blacklist", p.Client), False)
        '            End If
        '        Else
        '            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_COMMANDPERMISSION"), p.Client))
        '            If Player.isGameJoltPlayer Then
        '                Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "is unable to use ""/list blacklist"" due to insufficient permission."), Main.LogType.Command, p.Client)
        '            Else
        '                Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "is unable to use ""/list blacklist"" due to insufficient permission."), Main.LogType.Command, p.Client)
        '            End If
        '        End If
        '    End If
        'Else
        '    If Regex.IsMatch(p.DataItems(0), "\/blacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase) Then
        '        Dim Group1 As String = Regex.Match(p.DataItems(0), "\/blacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value

        '        If String.Equals(Group1, "True", StringComparison.OrdinalIgnoreCase) Then
        '            Main.Setting.BlackList = True
        '            Main.Main.QueueMessage("Package.HandleChatCommand.vb: Turning on blacklist feature.", Main.LogType.Command)
        '        ElseIf String.Equals(Group1, "False", StringComparison.OrdinalIgnoreCase)
        '            Main.Setting.BlackList = False
        '            Main.Main.QueueMessage("Package.HandleChatCommand.vb: Turning off blacklist feature.", Main.LogType.Command)
        '        Else
        '            HandleChatCommandList(New Package(Package.PackageTypes.ChatMessage, "/help blacklist", Nothing), True)
        '        End If
        '    ElseIf Regex.IsMatch(p.DataItems(0), "\/list\sblacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase)
        '        Dim Group1 As String = Regex.Match(p.DataItems(0), "\/list\sblacklist\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value

        '        If String.Equals(Group1, "True", StringComparison.OrdinalIgnoreCase) Then
        '            Main.Setting.BlackList = True
        '            Main.Main.QueueMessage("Package.HandleChatCommand.vb: Turning on blacklist feature.", Main.LogType.Command)
        '        ElseIf String.Equals(Group1, "False", StringComparison.OrdinalIgnoreCase)
        '            Main.Setting.BlackList = False
        '            Main.Main.QueueMessage("Package.HandleChatCommand.vb: Turning off blacklist feature.", Main.LogType.Command)
        '        Else
        '            HandleChatCommandList(New Package(Package.PackageTypes.ChatMessage, "/help list blacklist", Nothing), True)
        '        End If
        '    End If
        'End If
#End Region

        ' BlackList.Add
        ' /ban <PlayerName> <Duration> <Reason>
        ' /ban <PlayerName> <Duration>
        ' /ban <PlayerName>
        ' /list add blacklist <PlayerName>

        'If Regex.IsMatch(p.DataItems(0), "\/ban\s+([^\s]+)\s+([^\s]+)\s+([^\s]+)\s*", RegexOptions.IgnoreCase) Then

        '    Dim Group1 As String = Regex.Match(p.DataItems(0), "\/ban\s+([^\s]+)\s+([^\s]+)\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value
        '    Dim Group2 As String = Regex.Match(p.DataItems(0), "\/ban\s+([^\s]+)\s+([^\s]+)\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(2).Value
        '    Dim Group3 As String = Regex.Match(p.DataItems(0), "\/ban\s+([^\s]+)\s+([^\s]+)\s+([^\s]+)\s*", RegexOptions.IgnoreCase).Groups.Item(3).Value

        '    If IsNumeric(Group2) AndAlso CInt(Group2) < 0 Then
        '        Group2 = "-1"
        '    End If

        '    If Main.Player.HasPlayer(Group1) Then
        '        Main.Setting.AddBlackList(Main.Player.GetPlayer(Group1), CInt(Group2), Group3)
        '    End If

        'End If

        ' BlackList.Remove
        ' /unban <PlayerName>
        ' /list remove blacklist <PlayerName>

        ' BlackList.Toggle
        ' /toggleban <PlayerName>
        ' /list toggle blacklist <PlayerName>

        ' BlackList.Check
        ' /checkban <PlayerName>
        ' /list check blacklist <PlayerName>
#End Region

#Region "Say"
        '/say <Message>
        If Command(p, Player.OperatorPermission.ChatModerator, "say", CommandParamType.Any) Then
            Dim Group1 As String = Regex.Match(p.DataItems(0), "\/say\s+(.+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Group1, p.Client))

            If p.Client IsNot Nothing Then
                If Player.isGameJoltPlayer Then
                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have send a server chat."), p.Client))
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", "have send a server chat."), Main.LogType.Command, p.Client)
                Else
                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have send a server chat."), p.Client))
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", "have send a server chat."), Main.LogType.Command, p.Client)
                End If
            Else
                Main.Main.QueueMessage("Package.HandleChatCommand.vb: [SERVER] " & Group1, Main.LogType.Command)
            End If
        End If
#End Region

#Region "Weather"
        '/weather <weatherid>
        If Command(p, Player.OperatorPermission.ServerModerator, "weather", CommandParamType.Integer) Then
            Dim Group1 As String = Regex.Match(p.DataItems(0), "\/weather\s+(\d+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value

            If CInt(Group1) <= -4 Then
                Group1 = "-4"
            ElseIf CInt(Group1) >= 9
                Group1 = "9"
            End If

            If Main.World.CanUpdate Then
                Main.World.CanUpdate = False
            End If

            If CInt(Group1) = -4 Then
                Main.World.Weather = Main.World.GenerateWeather(Main.Setting.Weather, Main.World.Season)
            Else
                Main.World.Weather = Main.World.GenerateWeather(CInt(Group1), Main.World.Season)
            End If

            Dim CurrentPlayer As List(Of Player) = Main.Player
            For Each Players As Player In CurrentPlayer
                Main.ServerClient.SendData(New Package(Package.PackageTypes.WorldData, Main.World.GenerateWorld(Players), Players.PlayerClient))
            Next

            If p.Client IsNot Nothing Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, p.Client))
                If Player.isGameJoltPlayer Then
                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have changed the global weather."), p.Client))
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have changed the global weather."), Main.LogType.Command, p.Client)
                Else
                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have changed the global weather."), p.Client))
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have changed the global weather."), Main.LogType.Command, p.Client)
                End If
                Main.Main.QueueMessage("World.vb: Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, Main.LogType.Info)
            Else
                Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, "Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, Nothing))
                Main.Main.QueueMessage("World.vb: Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, Main.LogType.Info)
            End If
        End If
#End Region

#Region "Season"
        '/season <seasonid>
        If Command(p, Player.OperatorPermission.ServerModerator, "season", CommandParamType.Integer) Then
            Dim Group1 As String = Regex.Match(p.DataItems(0), "\/season\s+(\d+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value

            If CInt(Group1) <= -4 Then
                Group1 = "-4"
            ElseIf CInt(Group1) >= 3
                Group1 = "3"
            End If

            If Main.World.CanUpdate Then
                Main.World.CanUpdate = False
            End If

            If CInt(Group1) = -4 Then
                Main.World.Season = Main.World.GenerateSeason(Main.Setting.Season)
            Else
                Main.World.Season = Main.World.GenerateSeason(CInt(Group1))
            End If

            Dim CurrentPlayer As List(Of Player) = Main.Player
            For Each Players As Player In CurrentPlayer
                Main.ServerClient.SendData(New Package(Package.PackageTypes.WorldData, Main.World.GenerateWorld(Players), Players.PlayerClient))
            Next

            If p.Client IsNot Nothing Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, p.Client))
                If Player.isGameJoltPlayer Then
                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", "have changed the global season."), p.Client))
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "have changed the global season."), Main.LogType.Command, p.Client)
                Else
                    Main.ServerClient.SendOperatorData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", "have changed the global season."), p.Client))
                    Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "have changed the global season."), Main.LogType.Command, p.Client)
                End If
                Main.Main.QueueMessage("World.vb: Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, Main.LogType.Info)
            Else
                Main.Main.QueueMessage("World.vb: Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, Main.LogType.Info)
            End If
        End If
#End Region

#Region "World"
        ' /world
        If Command(p, Player.OperatorPermission.Player, "world", CommandParamType.Nothing) Then
            If p.Client IsNot Nothing Then
                Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, "Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, p.Client))
            Else
                Main.Main.QueueMessage("World.vb: Current Season: " & Main.World.GetSeasonName(Main.World.Season) & " | Current Weather: " & Main.World.GetWeatherName(Main.World.Weather) & " | Current Time: " & Main.World._CurrentTime.AddSeconds(Main.World.TimeOffset).ToString, Main.LogType.Info)
            End If
        End If
#End Region

    End Sub

    Public Sub HandleChatCommandList(ByVal p As Package)
        Dim Player As Player = Main.Player.GetPlayer(p.Client)
        Dim ListType As String = Nothing
        Dim ListValue As String = Nothing
        Dim Items As New List(Of String)

        ' Help Libary
        ' /help <Index>
        ' /help <Name>
        ' /help
        If Regex.IsMatch(p.DataItems(0), "\/Help\s+(\d+)\s*", RegexOptions.IgnoreCase) Then
            Dim HelpPage As Integer = CInt(Regex.Match(p.DataItems(0), "\/Help\s+(\d+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value)

            If HelpPage <= 0 Then
                HelpPage = 1
            ElseIf HelpPage > 1
                HelpPage = 1
            End If

            ListValue = HelpPage.ToString
            ListType = "General"
        ElseIf Regex.IsMatch(p.DataItems(0), "\/help\s+(\w+)\s*", RegexOptions.IgnoreCase)
            ListValue = Regex.Match(p.DataItems(0), "\/help\s+(\w+)\s*", RegexOptions.IgnoreCase).Groups.Item(1).Value
            ListType = "InDepth"
        ElseIf Regex.IsMatch(p.DataItems(0), "\/help\s*", RegexOptions.IgnoreCase)
            ListValue = "0"
            ListType = "General"
        End If

#Region "General"
        If ListType = "General" Then
            ' Each Page can only have 14.
            Select Case ListValue
                Case "1"
                    Items.Add("---------- Help: Index (1/1) ----------")
                    Items.Add("Use /help [Name/Index] to get page index of help.")
                    Items.Add("---------- Category: BlackList ----------")
                    ' BlackList Feature
                    Items.Add("/blacklist - To turn on or off the blacklist feature.")
                    ' BlackList.Add
                    Items.Add("/ban - To ban a player in the server.")
                    ' BlackList.Remove
                    Items.Add("/unban - To unban a player in the server.")
                    ' BlackList.Check
                    Items.Add("/checkban - To check the ban status of a player in the server.")
                    Items.Add("---------- Category: IPBlackList ----------")
                    ' IPBlackList Feature
                    Items.Add("/ipblacklist - To turn on or off the ipblacklist feature.")
                    ' IPBlackList.Add
                    Items.Add("/ipban - To ipban a player in the server.")
                    ' IPBlackList.Remove
                    Items.Add("/unipban - To unipban a player in the server.")
                    ' IPBlackList.Check
                    Items.Add("/checkipban - To check the ipban status of a player in the server.")
                Case "2"
                    Items.Add("---------- Help: Index (2/1) ----------")
                    Items.Add("Use /help [Name/Index] to get page index of help.")
                    Items.Add("---------- Category: MuteList ----------")
                    ' MuteList Feature
                    Items.Add("/mutelist - To turn on or off the mutelist feature.")
                    ' MuteList.Add
                    Items.Add("/mute - To mute a player in the server.")
                    ' MuteList.Remove
                    Items.Add("/unmute - To unmute a player in the server.")
                    ' MuteList.Check
                    Items.Add("/checkmute - To check the mute status of a player in the server.")
                    Items.Add("---------- Category: Operators ----------")
                    ' OperatorList Feature
                    Items.Add("/operatorlist - To turn on or off the operatorlist feature.")
                    ' OperatorList.Add
                    Items.Add("/op - To add an operator in the server.")
                    ' OperatorList.Remove
                    Items.Add("/deop - To remove an operator in the server.")
                    ' OperatorList.Check
                    Items.Add("/checkop - To check the status of a player in the server.")
                Case "3"
                    Items.Add("---------- Help: Index (3/1) ----------")
                    Items.Add("Use /help [Name/Index] to get page index of help.")
                    Items.Add("---------- Category: Whitelist ----------")
                    ' Whitelist Feature
                    Items.Add("/whitelist - To turn on or off the whitelist feature.")
                    ' Whitelist.Add
                    Items.Add("/allow - To allow a player to join the server.")
                    ' Whitelist.Remove
                    Items.Add("/disallow - To disallow a player to join in the server.")
                    ' Whitelist.Check
                    Items.Add("/checkallow - To check the status of a player in the server.")
                    Items.Add("---------- Category: Swear Detection system ----------")
                    ' SwearInfraction Feature
                    Items.Add("/swearinfractionlist - To turn on or off the swearinfractionlist feature.")
                    ' SwearInfraction.Add
                    Items.Add("/swearadd - To infract a player in the server.")
                    ' SwearInfraction.Remove
                    Items.Add("/swearremove - To reset the infraction of a player in the server.")
                    ' SwearInfraction.Check
                    Items.Add("/swearcheck - To check the status of a player in the server.")

                Case "4"
                    Items.Add("---------- Help: Index (4/1) ----------")
                    Items.Add("Use /help [Name/Index] to get page index of help.")
                    Items.Add("---------- Category: Online Mode Setting ----------")
                    ' OnlineSetting Feature
                    Items.Add("/onlinesettinglist - To turn on or off the onlinesettinglist feature.")
                    ' OnlineSetting.Weather.Change
                    Items.Add("/onlinesetting.weather - To change the setting of the weather locally.")
                    ' OnlineSetting.Season.Change
                    Items.Add("/onlinesetting.season - To change the setting of the season locally.")
                    ' OnlineSetting.World.Check
                    Items.Add("/onlinesetting.checkworld - To check the current world setting locally.")
                    ' OnlineSetting.MuteList.Add
                    Items.Add("/onlinesetting.mute - To mute a player in the server locally.")
                    ' OnlineSetting.MuteList.Remove
                    Items.Add("/onlinesetting.unmute - To unmute a player in the server locally.")
                    ' OnlineSetting.MuteList.Check
                    Items.Add("/onlinesetting.checkmute - To check the mute status of a player in the server locally.")
                Case "5"
                    Items.Add("---------- Help: Index (5/1) ----------")
                    Items.Add("Use /help [Name/Index] to get page index of help.")
                    Items.Add("---------- Category: Others ----------")
                    ' PM
                    Items.Add("/pm - To talk privately to a player.")
                    ' Say
                    Items.Add("/say - To talk as a server chat.")
                    ' Find
                    Items.Add("/find - To find a player in the server. (GameMode are not compatible yet.)")
                    ' Weather
                    Items.Add("/weather - To change the global weather.")
                    ' Season
                    Items.Add("/season - To change the global season.")
                    ' World
                    Items.Add("/world - To check the current world status.")
                    ' Restart
                    Items.Add("/restart - To restart the server.")
                    ' Stop
                    Items.Add("/stop - To stop the server.")
            End Select
        End If
#End Region

#Region "InDepth"
        If ListType = "InDepth" Then
            ' Each Page can only have 14.
            Select Case ListValue
                Case ""

            End Select
        End If
#End Region
    End Sub

    ' Command Extra Helper Functions
    Private Enum CommandParamType
        [String]
        [Integer]
        [Any]
        [Nothing]
    End Enum

    Private Function Command(ByVal p As Package, ByVal RequiredPermission As Integer, ByVal Name As String, ParamArray ByVal ParamType() As CommandParamType) As Boolean
        Dim RegexFilter As String = "\/" & Name
        For Each Param As CommandParamType In ParamType
            If Param = CommandParamType.Any Then
                RegexFilter &= "\s+(.+)"
            ElseIf Param = CommandParamType.String
                RegexFilter &= "\s+(\w+)"
            ElseIf Param = CommandParamType.Integer
                RegexFilter &= "\s+(\d+)"
            End If
        Next
        RegexFilter &= "\s*"

        If p.Client Is Nothing Then
            If p.DataItems(0).StartsWith("/" & Name) Then
                If Regex.IsMatch(p.DataItems(0), RegexFilter, RegexOptions.IgnoreCase) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Dim Player As Player = Main.Player.GetPlayer(p.Client)
            If p.DataItems(0).StartsWith("/" & Name) Then
                If Main.Setting.OperatorPermission(Player) >= RequiredPermission Then
                    If Regex.IsMatch(p.DataItems(0), RegexFilter, RegexOptions.IgnoreCase) Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_COMMANDPERMISSION"), p.Client))
                    If Player.isGameJoltPlayer Then
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Player.Name, Player.GameJoltID.ToString, "is unable to use ""/" & Name & """ due to insufficient permission."), Main.LogType.Command, p.Client)
                    Else
                        Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Player.Name, "is unable to use ""/" & Name & """ due to insufficient permission."), Main.LogType.Command, p.Client)
                    End If
                    Return False
                End If
            Else
                Return False
            End If
        End If
    End Function
End Class
#Else
Public Class OldPackage

    'Release => SViper / Official
#Const Release = "SViper"

    Public Enum PackageTypes
        Unknown = -1
        GameData = 0
        PlayData = 1
        PrivateMessage = 2
        ChatMessage = 3
        Kicked = 4
        ID = 7
        CreatePlayer = 8
        DestroyPlayer = 9
        ServerClose = 10
        ServerMessage = 11
        WorldData = 12
        Ping = 13
        GamestateMessage = 14
        TradeRequest = 30
        TradeJoin = 31
        TradeQuit = 32
        TradeOffer = 33
        TradeStart = 34
        BattleRequest = 50
        BattleJoin = 51
        BattleQuit = 52
        BattleOffer = 53
        BattleStart = 54
        BattleClientData = 55
        BattleHostData = 56
        BattlePokemonData = 57
        ServerInfoData = 98
        ServerDataRequest = 99
    End Enum

    Public Shared Function Check(ByVal Data As String) As Boolean
        If String.IsNullOrEmpty(Data) Or String.IsNullOrWhiteSpace(Data) Then
            Return False
        End If

        If Not Data.Contains("|") Then
            Return False
        Else
            If Not Functions.GetSplit(Data, 0, "|") = OldSetting.ProtocolVersion Then
                Return False
            End If
        End If

        ' Valid Protocol Version / Valid Data
        ' 0.5|13|0|0|
        ' 0.5|0|0|15|0|10|11|17|18|30|52|61|62|63|65|66|67|74|88|pokemon 3d1116016.jianmingyongmainmenu\mainmenu0.dat13|1.9|1400740013|2|14[POKEMON|S]1570
        Dim DataCount As Integer = 0

        If Not Integer.TryParse(Functions.GetSplit(Data, 3, "|"), DataCount) Then
            Return False
        ElseIf DataCount < 0 Then
            Return False
        End If

        Dim LastIndexCount As Integer = 0

        If Not Integer.TryParse(Functions.GetSplit(Data, 3 + DataCount, "|"), LastIndexCount) Then
            Return False
        ElseIf LastIndexCount < 0 Then
            Return False
        End If

        Dim FullDataString As String = Nothing

        If Data.IndexOf(LastIndexCount.ToString) + 3 < Data.Length Then
            FullDataString = Data.Substring(Data.IndexOf(LastIndexCount.ToString) + 3)
        Else
            Return False
        End If

        If FullDataString.Count >= LastIndexCount Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function Type(ByVal Data As String) As Integer
        If Check(Data) Then
            Return CInt(Functions.GetSplit(Data, 1, "|"))
        Else
            Return PackageTypes.Unknown
        End If
    End Function

    Public Shared Function Origin(ByVal Data As String) As Integer
        If Check(Data) Then
            Return CInt(Functions.GetSplit(Data, 2, "|"))
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function DataItemsCount(ByVal Data As String) As Integer
        If Check(Data) Then
            Return CInt(Functions.GetSplit(Data, 3, "|"))
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function DataItem(ByVal data As String) As List(Of String)
        If Check(data) Then
            Dim DataItemsCount As Integer = OldPackage.DataItemsCount(data)
            Dim FullDataItems As String = Nothing
            Dim DataItems As New List(Of String)
            Dim PreviousIndex As Integer = 0

            If DataItemsCount = 1 Then
                FullDataItems = data.Substring(OldSetting.ProtocolVersion.Length + 1 + Type(data).ToString.Length + 1 + Origin(data).ToString.Length + 1 + DataItemsCount.ToString.Length + 1 + 2)
                DataItems.Add(FullDataItems)
            Else
                FullDataItems = data.Substring(OldSetting.ProtocolVersion.Length + 1 + Type(data).ToString.Length + 1 + Origin(data).ToString.Length + 1 + DataItemsCount.ToString.Length + 1)
                For i As Integer = 1 To DataItemsCount Step +1
                    FullDataItems = FullDataItems.Substring(FullDataItems.IndexOf("|") + 1)
                Next
                For i As Integer = 4 To 4 + DataItemsCount Step +1
                    ' 0.5|0|0|15|0|0|0|0|0|0|12|17|18|18|18|18|18|18|18|yourroom.dat1|0|31
                    If i = 4 Then
                        PreviousIndex = 0
                    ElseIf i > 4 And i < 4 + DataItemsCount Then
                        If CInt(Functions.GetSplit(data, i, "|")) = PreviousIndex Then
                            DataItems.Add(Nothing)
                        Else
                            If CInt(Functions.GetSplit(data, i, "|")) >= FullDataItems.Length Then
                                DataItems.Add(FullDataItems.Remove(0, PreviousIndex))
                            Else
                                DataItems.Add(FullDataItems.Remove(CInt(Functions.GetSplit(data, i, "|"))).Remove(0, PreviousIndex))
                            End If
                            PreviousIndex = CInt(Functions.GetSplit(data, i, "|"))
                        End If
                    Else
                        DataItems.Add(FullDataItems.Remove(0, PreviousIndex))
                    End If
                Next
            End If

            Return DataItems
        Else
            Return Nothing
        End If
    End Function

    Public Shared Function CreateData(ByVal Type As PackageTypes, ByVal Origin As Integer, ByVal DataItems As String) As String
        Dim PreviousIndex As Integer = 0
        Dim FullDataItems As String = Nothing
        Dim ReturnString As String = OldSetting.ProtocolVersion + "|" + CInt(Type).ToString + "|" + Origin.ToString + "|1|0|"

        If String.IsNullOrEmpty(DataItems) Or String.IsNullOrWhiteSpace(DataItems) Then
            FullDataItems = " "
        Else
            FullDataItems = DataItems
        End If

        Return ReturnString + FullDataItems
    End Function

    Public Shared Function CreateData(ByVal Type As PackageTypes, ByVal Origin As Integer, ByVal DataItems As List(Of String)) As String
        Dim PreviousIndex As Integer = 0
        Dim FullDataItems As String = Nothing
        Dim ReturnString As String = OldSetting.ProtocolVersion + "|" + CInt(Type).ToString + "|" + Origin.ToString + "|" + DataItems.Count.ToString + "|0|"

        If DataItems.Count > 1 Then
            For i As Integer = 0 To DataItems.Count - 1 Step +1
                If i = 0 Then
                    PreviousIndex = 0
                ElseIf i > 0 Then
                    FullDataItems += (DataItems(i - 1).Length + PreviousIndex).ToString + "|"
                    PreviousIndex += DataItems(i - 1).Length
                End If
            Next
            For Each Str As String In DataItems
                FullDataItems += Str
            Next
        Else
            If String.IsNullOrEmpty(DataItems(0)) Or String.IsNullOrWhiteSpace(DataItems(0)) Then
                FullDataItems = " "
            Else
                FullDataItems = DataItems(0)
            End If
        End If

        Return ReturnString + FullDataItems
    End Function

    Public Shared Function IsFullPackageData(ByVal Data As String) As Boolean
        If Check(Data) Then
            Dim DataItem As List(Of String) = OldPackage.DataItem(Data)
            If String.Equals(DataItem(0), "1", StringComparison.OrdinalIgnoreCase) Then
                For Each DataItems As String In DataItem
                    If String.IsNullOrEmpty(DataItems) Or String.IsNullOrWhiteSpace(DataItems) Then
                        Return False
                    End If
                Next
            Else
                For i As Integer = 0 To 14 Step +1
                    If (String.IsNullOrEmpty(DataItem(i)) Or String.IsNullOrWhiteSpace(DataItem(i))) And Not i = 2 Then
                        Return False
                    End If
                Next
            End If

            Return True
        Else
            Return Nothing
        End If
    End Function

    Public Shared Sub Handle(ByVal Data As String, ByVal Client As TcpClient)
        Dim Type As Integer = OldPackage.Type(Data)

        Select Case Type
            Case PackageTypes.Unknown
                OldServerClient.QueueMessage("Invalid Data have been received from " + CType(Client.Client.RemoteEndPoint, IPEndPoint).Address.ToString, Main.LogType.Debug)
            Case PackageTypes.GameData
                HandleGameData(Data, Client)
            Case PackageTypes.PrivateMessage
                HandlePrivateMessage(Data, Client)
            Case PackageTypes.ChatMessage
                HandleChatMessage(Data, Client)
            Case PackageTypes.Ping
                OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
            Case PackageTypes.GamestateMessage
                HandleGamestateMessage(Data, Client)
            Case PackageTypes.TradeRequest
                HandleTradeRequest(Data, Client)
            Case PackageTypes.TradeJoin
                HandleTradeJoin(Data, Client)
            Case PackageTypes.TradeQuit
                HandleTradeQuit(Data, Client)
            Case PackageTypes.TradeOffer
                HandleTradeOffer(Data, Client)
            Case PackageTypes.TradeStart
                HandleTradeStart(Data, Client)
            Case PackageTypes.BattleRequest
                HandleBattleRequest(Data, Client)
            Case PackageTypes.BattleJoin
                HandleBattleJoin(Data, Client)
            Case PackageTypes.BattleQuit
                HandleBattleQuit(Data, Client)
            Case PackageTypes.BattleOffer
                HandleBattleOffer(Data, Client)
            Case PackageTypes.BattleStart
                HandleBattleStart(Data, Client)
            Case PackageTypes.BattleClientData
                HandleBattleClientData(Data, Client)
            Case PackageTypes.BattleHostData
                HandleBattleHostData(Data, Client)
            Case PackageTypes.BattlePokemonData
                HandleBattlePokemonData(Data, Client)
            Case PackageTypes.ServerDataRequest
                HandleServerDataRequest(Data, Client)
        End Select

    End Sub

    Public Shared Sub HandleGameData(ByVal Data As String, ByVal Client As TcpClient)
        If Not IsFullPackageData(Data) And OldPlayer.PlayerClient.Contains(Client) Then
            OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
            If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
                OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
            End If
            OldPlayer.Update(Data, Client)
            OldServerClient.PlayersToUpdate.Add(Client)
        Else
            If OldPlayer.PlayerClient.Contains(Client) Then
                OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
                If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
                    OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
                End If
                OldPlayer.Update(Data, Client)
                OldServerClient.PlayersToUpdate.Add(Client)
                Exit Sub
            End If

            Dim DataItems As List(Of String) = OldPackage.DataItem(Data)

            ' Check for Max Player
            If OldPlayer.PlayerID.Count = CInt(OldSetting.MaxPlayers) Then
                OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join is currently full of players. Please try again later."), Client)
                OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join is currently full of players. Please try again later.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                Exit Sub
            End If

            ' Check for GameMode
            If Not String.Equals(OldSetting.GameMode, DataItems(0), StringComparison.OrdinalIgnoreCase) Then
                OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join required " + OldSetting.GameMode + " GameMode. Your current GameMode is " + DataItems(0) + "."), Client)
                OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join required " + OldSetting.GameMode + " GameMode. Your current GameMode is " + DataItems(0) + ".", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                Exit Sub
            End If

            ' Check for Online Mode Server
            If String.Equals(OldSetting.OfflineMode, "False", StringComparison.OrdinalIgnoreCase) Then
                If String.Equals(DataItems(1), "0", StringComparison.OrdinalIgnoreCase) Then
                    OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join required GameJolt profile."), Client)
                    OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join required GameJolt profile.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Exit Sub
                End If
            End If

            ' Check for BlackList
            If String.Equals(OldSetting.BlackList, "True", StringComparison.OrdinalIgnoreCase) Then
                If OldSetting.BlackListItem.Count > 0 Then
                    For Each Data1 As String In OldSetting.BlackListItem
                        ' Name | GameJolt ID | Ban Reason | Ban Start Time | Ban Time
                        If DataItems(1) = "0" Then
                            ' If Not GameJolt ID
                            If String.Equals(Functions.GetSplit(Data1, 0, "|"), DataItems(4), StringComparison.Ordinal) And String.Equals(Functions.GetSplit(Data1, 1, "|"), "Nothing", StringComparison.OrdinalIgnoreCase) Then
                                ' You are ban but check ban time
                                ' If End Ban Time is > the current time
                                If Date.Compare(CType(Functions.GetSplit(Data1, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data1, 4, "|"))), Date.Now) > 0 Then
                                    Dim RemainBanTime As TimeSpan = (CType(Functions.GetSplit(Data1, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data1, 4, "|"))) - Date.Now)
                                    OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join banned you due to the following reason: " + Functions.GetSplit(Data1, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds."), Client)
                                    OldServerClient.QueueMessage(DataItems(4) + " failed to join the server due to the following reason: The server you are trying to join banned you due to the following reason: " + Functions.GetSplit(Data1, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                                    Exit Sub
                                Else
                                    OldSetting.BlackListItem.Remove(Data1)
                                    Exit For
                                End If
                            End If
                        Else
                            ' If GameJolt ID
                            If String.Equals(Functions.GetSplit(Data1, 1, "|"), DataItems(2), StringComparison.OrdinalIgnoreCase) Then
                                ' You are ban but check ban time
                                ' If End Ban Time is > the current time
                                If Date.Compare(CType(Functions.GetSplit(Data1, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data1, 4, "|"))), Date.Now) > 0 Then
                                    Dim RemainBanTime As TimeSpan = (CType(Functions.GetSplit(Data1, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data1, 4, "|"))) - Date.Now)
                                    OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join banned you due to the following reason: " + Functions.GetSplit(Data1, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds."), Client)
                                    OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join banned you due to the following reason: " + Functions.GetSplit(Data1, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                                    Exit Sub
                                Else
                                    OldSetting.BlackListItem.Remove(Data1)
                                    Exit For
                                End If
                            End If
                        End If
                    Next
                End If
            End If

            ' Check for IPBlackList
            If String.Equals(OldSetting.BlackList, "True", StringComparison.OrdinalIgnoreCase) Then
                If OldSetting.IPBlackListItem.Count > 0 Then
                    For Each Data1 As String In OldSetting.BlackListItem
                        ' IP | Ban Reason | Ban Start Time | Ban Time
                        If String.Equals(Functions.GetSplit(Data1, 0, "|"), CType(Client.Client.RemoteEndPoint, IPEndPoint).Address.ToString, StringComparison.OrdinalIgnoreCase) Then
                            If Date.Compare(CType(Functions.GetSplit(Data1, 2, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data1, 3, "|"))), Date.Now) > 0 Then
                                Dim RemainBanTime As TimeSpan = (CType(Functions.GetSplit(Data1, 2, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data1, 3, "|"))) - Date.Now)
                                OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join IP banned you due to the following reason: " + Functions.GetSplit(Data1, 1, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds."), Client)
                                If CInt(DataItems(2)) = 1 Then
                                    OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join IP banned you due to the following reason: " + Functions.GetSplit(Data1, 1, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                                Else
                                    OldServerClient.QueueMessage(DataItems(4) + " failed to join the server due to the following reason: The server you are trying to join IP banned you due to the following reason: " + Functions.GetSplit(Data1, 1, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                                End If
                                Exit Sub
                            Else
                                OldSetting.IPBlackListItem.Remove(Data1)
                                Exit For
                            End If
                        End If
                    Next
                End If
            End If

            ' Check for WhiteList
            Dim AllowJoin As Boolean = False
            If String.Equals(OldSetting.WhiteList, "True", StringComparison.OrdinalIgnoreCase) Then
                If OldSetting.WhiteListItem.Count > 0 Then
                    For Each Data1 As String In OldSetting.WhiteListItem
                        ' Name | GameJolt ID | WhiteList Reason
                        If DataItems(1) = "0" Then
                            ' If Not GameJolt ID
                            If String.Equals(Functions.GetSplit(Data1, 0, "|"), DataItems(4), StringComparison.Ordinal) And String.Equals(Functions.GetSplit(Data1, 1, "|"), "Nothing", StringComparison.OrdinalIgnoreCase) Then
                                AllowJoin = True
                                Exit For
                            Else
                                AllowJoin = False
                            End If
                        Else
                            ' If GameJolt ID
                            If String.Equals(Functions.GetSplit(Data1, 1, "|"), DataItems(2), StringComparison.OrdinalIgnoreCase) Then
                                AllowJoin = True
                                Exit For
                            Else
                                AllowJoin = False
                            End If
                        End If
                    Next
                End If
            Else
                AllowJoin = True
            End If

            If Not AllowJoin Then
                OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join required administrator permission to join."), Client)
                If CInt(DataItems(2)) = 1 Then
                    OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join required administrator permission to join.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                Else
                    OldServerClient.QueueMessage(DataItems(4) + " failed to join the server due to the following reason: The server you are trying to join required administrator permission to join.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                End If
                Exit Sub
            End If

            ' Check if you are a clone.
            For Each Players As String In OldPlayer.Name
                If String.Equals(DataItems(4), Players, StringComparison.Ordinal) Then
                    AllowJoin = False
                    Exit For
                Else
                    AllowJoin = True
                End If
            Next

            If Not AllowJoin Then
                OldPlayer.SendData(CreateData(PackageTypes.Kicked, -1, "The server you are trying to join contains the same player name. Please try again later."), Client)
                If CInt(DataItems(2)) = 1 Then
                    OldServerClient.QueueMessage(DataItems(4) + " (" + DataItems(2) + ") failed to join the server due to the following reason: The server you are trying to join contains the same player name. Please try again later.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                Else
                    OldServerClient.QueueMessage(DataItems(4) + " failed to join the server due to the following reason: The server you are trying to join contains the same player name. Please try again later.", Main.LogType.Info, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                End If
                Exit Sub
            End If

            OldPlayer.Add(Data, Client)
        End If
    End Sub

    Public Shared Sub HandlePrivateMessage(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim PMPlayerIndex As Integer = OldPlayer.Name.IndexOf(DataItems(0))

        For Each line As String In OldSetting.SwearFilterList
            Dim Message2 As System.Text.RegularExpressions.MatchCollection = Regex.Matches(DataItems(1), "\S+")
            Dim Infracted As Boolean = False
            For i As Integer = 0 To Message2.Count - 1 Step +1
                If String.Equals(Message2.Item(i).Value, line, StringComparison.OrdinalIgnoreCase) Then
                    If Not OldPlayer.isMuted(CurrentPlayerIndex) Then
                        OldPlayer.AddInfractionCount(CurrentPlayerIndex)

                        If OldPlayer.InfractionCount(CurrentPlayerIndex) >= CInt(OldSetting.SwearInfractionCap) Then
                            OldPlayer.SubtractInfractionCount(CurrentPlayerIndex, CInt(OldSetting.SwearInfractionCap))
                            OldPlayer.AddInfractionMuteCount(CurrentPlayerIndex)

                            If OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 1 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 60, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 2 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 3600, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 3 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 86400, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 4 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 604800, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 5 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 2419200, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) >= 6 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, Integer.MaxValue, "You have sweared too much.", True)
                            End If
                        Else
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server detected that your message contains swearing words. Please avoid swearing in the server. | You have " + OldPlayer.InfractionCount(CurrentPlayerIndex).ToString + " infraction points."), Client)
                        End If

                        Dim Message1 As String = Nothing
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            Message1 += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")"
                        Else
                            Message1 += OldPlayer.Name(CurrentPlayerIndex) + ""
                        End If
                        OldServerClient.QueueMessage("The player " + Message1 + " have triggered the swear filter. The swear word is: " + line, Main.LogType.Server)
                        Infracted = True
                    End If
                    Exit For
                End If
            Next
            If Infracted Then
                Exit For
            End If
        Next

        ' Check if Player is muted
        If OldPlayer.isMuted(CurrentPlayerIndex, PMPlayerIndex) Then
            Dim MuteData As String = OldPlayer.GetMuteReason(CurrentPlayerIndex, PMPlayerIndex)
            ' Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time
            Dim RemainBanTime As TimeSpan = (CType(Functions.GetSplit(MuteData, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(MuteData, 4, "|"))) - Date.Now)
            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds."), Client)
            OldServerClient.QueueMessage("The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        Else
            OldPlayer.SendData(CreateData(PackageTypes.PrivateMessage, OldPlayer.PlayerID(CurrentPlayerIndex), DataItems(1)), Client)
            Dim Message As String = Nothing
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") have sent a private message to "
            Else
                Message += OldPlayer.Name(CurrentPlayerIndex) + " have sent a private message to "
            End If

#If Release = "SViper" Then
            ' SViper Server Client
            If OldPlayer.isGameJoltPlayer(PMPlayerIndex) Then
                Message += OldPlayer.Name(PMPlayerIndex) + " (" + OldPlayer.GameJoltID(PMPlayerIndex).ToString + ") with the following content: "
            Else
                Message += OldPlayer.Name(PMPlayerIndex) + " with the following content: "
            End If
            OldServerClient.QueueMessage(Message + DataItems(1), Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
#ElseIf Release = "Official" Then
            ' Official Release
            If Player.isGameJoltPlayer(PMPlayerIndex) Then
                Message += Player.Name(PMPlayerIndex) + " (" + Player.GameJoltID(PMPlayerIndex).ToString + ")"
            Else
                Message += Player.Name(PMPlayerIndex)
            End If
            ServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
#End If
        End If
    End Sub

    Public Shared Sub HandleChatMessage(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim Message As String = DataItems(0)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)

        For Each line As String In OldSetting.SwearFilterList
            Dim Message2 As System.Text.RegularExpressions.MatchCollection = Regex.Matches(Message, "\S+")
            Dim Infracted As Boolean = False
            For i As Integer = 0 To Message2.Count - 1 Step +1
                If String.Equals(Message2.Item(i).Value, line, StringComparison.OrdinalIgnoreCase) Then
                    If Not OldPlayer.isMuted(CurrentPlayerIndex) Then
                        OldPlayer.AddInfractionCount(CurrentPlayerIndex)

                        If OldPlayer.InfractionCount(CurrentPlayerIndex) >= CInt(OldSetting.SwearInfractionCap) Then
                            OldPlayer.SubtractInfractionCount(CurrentPlayerIndex, CInt(OldSetting.SwearInfractionCap))
                            OldPlayer.AddInfractionMuteCount(CurrentPlayerIndex)

                            If OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 1 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 60, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 2 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 3600, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 3 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 86400, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 4 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 604800, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) = 5 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, 2419200, "You have sweared too much. Take a break please.", True)
                            ElseIf OldPlayer.InfractionMuteCount(CurrentPlayerIndex) >= 6 Then
                                OldPlayer.AddMute(CurrentPlayerIndex, Nothing, Integer.MaxValue, "You have sweared too much.", True)
                            End If
                        Else
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server detected that your message contains swearing words. Please avoid swearing in the server. | You have " + OldPlayer.InfractionCount(CurrentPlayerIndex).ToString + " infraction points."), Client)
                        End If

                        Dim Message1 As String = Nothing
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            Message1 += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")"
                        Else
                            Message1 += OldPlayer.Name(CurrentPlayerIndex) + ""
                        End If
                        OldServerClient.QueueMessage("The player " + Message1 + " have triggered the swear filter system. The swear word is: " + line, Main.LogType.Server)
                        Infracted = True
                    End If
                    Exit For
                End If
            Next
            If Infracted Then
                Exit For
            End If
        Next

        If Message.StartsWith("/") Then
            HandleChatMessageCommand(Message, Client)
            Exit Sub
        End If

        If OldPlayer.isMuted(CurrentPlayerIndex) Then
            Dim MuteData As String = OldPlayer.GetMuteReason(CurrentPlayerIndex)
            ' Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time
            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, MuteData), Client)
            Dim Message1 As String = Nothing
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                Message1 += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")"
            Else
                Message1 += OldPlayer.Name(CurrentPlayerIndex) + ""
            End If
            OldServerClient.QueueMessage("The player " + Message1 + " is unable to chat due to the following reason: " + MuteData, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        Else
            OldPlayer.SendAllData(CreateData(PackageTypes.ChatMessage, OldPlayer.PlayerID(CurrentPlayerIndex), Message), Client)
            Dim Message1 As String = Nothing
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                Message1 += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + "): "
            Else
                Message1 += OldPlayer.Name(CurrentPlayerIndex) + ": "
            End If
            OldServerClient.QueueMessage(Message1 + Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        End If
    End Sub

    Public Shared Sub HandleChatMessageCommand(ByVal Message As String, Optional ByVal Client As TcpClient = Nothing, Optional ByVal IsHost As Boolean = False)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        If Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s*") Then
            ' /ban <Playername> [Duration] [Reason]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                        ' /ban <Playername> [Duration] [Reason]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                        Dim Reason As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddBan(PlayerIndex, Client, CInt(Duration), Reason)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*") Then
                        ' /ban <Playername> [Duration]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddBan(PlayerIndex, Client, CInt(Duration))
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s*") Then
                        ' /ban <Playername>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s*").Groups(1).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddBan(PlayerIndex, Client)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help ban", Client)
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use ban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use ban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                    ' /ban <Playername> [Duration] [Reason]
                    Dim PlayerName As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                    Dim Duration As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                    Dim Reason As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.AddBan(PlayerIndex, Nothing, CInt(Duration), Reason)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*") Then
                    ' /ban <Playername> [Duration]
                    Dim PlayerName As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                    Dim Duration As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.AddBan(PlayerIndex, Nothing, CInt(Duration))
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s*") Then
                    ' /ban <Playername>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Bb][Aa][Nn]\s+(\w+)\s*").Groups(1).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.AddBan(PlayerIndex, Nothing)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help ban")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Cc][Hh][Ee][Cc][Kk][Bb][Aa][Nn]\s*") Then
            ' /checkban [Index/Name]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    Dim ReturnMessage As List(Of String) = OldPlayer.ListBanData(Message)
                    If ReturnMessage IsNot Nothing Then
                        For Each Data As String In ReturnMessage
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, Data), Client)
                        Next
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use checkban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use checkban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                Dim ReturnMessage As List(Of String) = OldPlayer.ListBanData(Message)
                If ReturnMessage IsNot Nothing Then
                    For Each Data As String In ReturnMessage
                        OldServerClient.QueueMessage(Data, Main.LogType.Info)
                    Next
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Cc][Hh][Ee][Cc][Kk][Ii][Pp][Bb][Aa][Nn]\s*") Then
            ' /checkipban [Index/ip]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    Dim ReturnMessage As List(Of String) = OldPlayer.ListIPBanData(Message)
                    If ReturnMessage IsNot Nothing Then
                        For Each Data As String In ReturnMessage
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, Data), Client)
                        Next
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use checkipban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use checkipban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                Dim ReturnMessage As List(Of String) = OldPlayer.ListIPBanData(Message)
                If ReturnMessage IsNot Nothing Then
                    For Each Data As String In ReturnMessage
                        OldServerClient.QueueMessage(Data, Main.LogType.Info)
                    Next
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Cc][Hh][Ee][Cc][Kk][Mm][Uu][Tt][Ee]\s*") Then
            ' /checkmute [Index/Name]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ChatModerator) Then
                    Dim ReturnMessage As List(Of String) = OldPlayer.ListMuteData(Message, True, Client)
                    If ReturnMessage IsNot Nothing Then
                        For Each Data As String In ReturnMessage
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, Data), Client)
                        Next
                    End If
                Else
                    Dim ReturnMessage As List(Of String) = OldPlayer.ListMuteData(Message, False, Client)
                    If ReturnMessage IsNot Nothing Then
                        For Each Data As String In ReturnMessage
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, Data), Client)
                        Next
                    End If
                End If
            Else
                Dim ReturnMessage As List(Of String) = OldPlayer.ListMuteData(Message, True, Client)
                If ReturnMessage IsNot Nothing Then
                    For Each Data As String In ReturnMessage
                        OldServerClient.QueueMessage(Data, Main.LogType.Info)
                    Next
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Cc][Hh][Ee][Cc][Kk][Oo][Pp]\s*") Then
            ' /checkop [Index/Name]
            If Not IsHost Then
                Dim ReturnMessage As List(Of String) = OldPlayer.ListOperatorData(Message)
                If ReturnMessage IsNot Nothing Then
                    For Each Data As String In ReturnMessage
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, Data), Client)
                    Next
                End If
            Else
                Dim ReturnMessage As List(Of String) = OldPlayer.ListOperatorData(Message)
                If ReturnMessage IsNot Nothing Then
                    For Each Data As String In ReturnMessage
                        OldServerClient.QueueMessage(Data, Main.LogType.Info)
                    Next
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Dd][Ee][Oo][Pp]\s*") Then
            ' /deop <PlayerName> <Scope>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.GlobalModerator) Then
                    If Regex.IsMatch(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s+(\w+)\s*") Then
                        ' /unban <PlayerName> <Scope>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                        Dim Scope As String = Regex.Match(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                        Dim ReturnMessage As String = Nothing
                        If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveOperator(PlayerName, True, True)
                        ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveOperator(PlayerName, False, True)
                        Else
                            ReturnMessage = "The server is unable to remove the player from the operator list as the scope is not properly defined."
                        End If
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s*") Then
                        ' /unban <PlayerName>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s*").Groups(1).Value
                        Dim ReturnMessage As String = OldPlayer.RemoveOperator(PlayerName, False, False)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help deop")
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Global Moderator and above permission."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use deop command on the server due to the following reason: This command require you to have Global Moderator and above permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use deop command on the server due to the following reason: This command require you to have Global Moderator and above permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s+(\w+)\s*") Then
                    ' /deop <PlayerName> <Scope>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                    Dim Scope As String = Regex.Match(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                    Dim ReturnMessage As String = Nothing
                    If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                        ReturnMessage = OldPlayer.RemoveOperator(PlayerName, True, True)
                    ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                        ReturnMessage = OldPlayer.RemoveOperator(PlayerName, False, True)
                    Else
                        ReturnMessage = "The server is unable to remove the player from the operator list as the scope is not properly defined."
                    End If
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s*") Then
                    ' /deop <PlayerName>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Dd][Ee][Oo][Pp]\s+(\w+)\s*").Groups(1).Value
                    Dim ReturnMessage As String = OldPlayer.RemoveOperator(PlayerName, False, False)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help deop")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Ff][Ii][Nn][Dd]\s*") Then
            ' /find [PlayerName]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Ff][Ii][Nn][Dd]\s+(\w+)\s*") Then
                        ' /find [PlayerName]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Ff][Ii][Nn][Dd]\s+(\w+)\s*").Groups(1).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.Find(PlayerIndex)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help find")
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use find command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use find command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Ff][Ii][Nn][Dd]\s+(\w+)\s*") Then
                    ' /find [PlayerName]
                    Dim PlayerName As String = Regex.Match(Message, "\/[Ff][Ii][Nn][Dd]\s+(\w+)\s*").Groups(1).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.Find(PlayerIndex)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help find")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Hh][Ee][Ll][Pp]\s*") Then
            ' /help
            If Not IsHost Then
                HandleChatMessageCommandHelp(Message, Client)
            Else
                HandleChatMessageCommandHelp(Message)
            End If
        ElseIf Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s*") Then
            ' /ipban <Playername> [Duration] [Reason]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                        ' /ipban <Playername> [Duration] [Reason]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                        Dim Reason As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddIPBan(PlayerIndex, Client, CInt(Duration), Reason)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*") Then
                        ' /ipban <Playername> [Duration]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddIPBan(PlayerIndex, Client, CInt(Duration))
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s*") Then
                        ' /ipban <Playername>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s*").Groups(1).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddIPBan(PlayerIndex, Client)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help ipban", Client)
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use ipban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use ipban command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                    ' /ipban <Playername> [Duration] [Reason]
                    Dim PlayerName As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                    Dim Duration As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                    Dim Reason As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.AddIPBan(PlayerIndex, Nothing, CInt(Duration), Reason)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*") Then
                    ' /ipban <Playername> [Duration]
                    Dim PlayerName As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                    Dim Duration As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.AddIPBan(PlayerIndex, Nothing, CInt(Duration))
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s*") Then
                    ' /ipban <Playername>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Ii][Pp][Bb][Aa][Nn]\s+(\w+)\s*").Groups(1).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.AddIPBan(PlayerIndex, Nothing)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help ipban")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Kk][Ii][Cc][Kk]\s*") Then
            ' /kick <PlayerName> [Reason]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s+(.+)\s*") Then
                        ' /kick <PlayerName> <Reason>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s+(.+)\s*").Groups(1).Value
                        Dim Reason As String = Regex.Match(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s+(.+)\s*").Groups(2).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.Kick(PlayerIndex, Reason, Client)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s*") Then
                        ' /kick <PlayerName>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s*").Groups(1).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.Kick(PlayerIndex, Nothing, Client)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help kick", Client)
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use kick command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use kick command on the server due to the following reason: This command require you to have Server Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s+(.+)\s*") Then
                    ' /kick <PlayerName> <Reason>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s+(.+)\s*").Groups(1).Value
                    Dim Reason As String = Regex.Match(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s+(.+)\s*").Groups(2).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.Kick(PlayerIndex, Reason)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s*") Then
                    ' /kick <PlayerName>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Kk][Ii][Cc][Kk]\s+(\w+)\s*").Groups(1).Value
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                    Dim ReturnMessage As String = OldPlayer.Kick(PlayerIndex)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help kick")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s*") Then
            ' /mute <Playername> [Duration] [Reason]
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ChatModerator) Then
                    If Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                        ' /mute <Playername> [Duration] [Reason]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                        Dim Reason As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Client, CInt(Duration), Reason, True)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*") Then
                        ' /mute <Playername> [Duration]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Client, CInt(Duration), "No reason.", True)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s*") Then
                        ' /mute <Playername>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s*").Groups(1).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Client, Integer.MaxValue, "No reason.", True)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help mute", Client)
                    End If
                Else
                    If Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                        ' /mute <Playername> [Duration] [Reason]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                        Dim Reason As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Client, CInt(Duration), Reason, False)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*") Then
                        ' /mute <Playername> [Duration]
                        Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                        Dim Duration As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Client, CInt(Duration), "No reason.", False)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s*") Then
                        ' /mute <Playername>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s*").Groups(1).Value
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                        Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Client, Integer.MaxValue, "No reason.", False)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help mute", Client)
                    End If
                End If
            ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*") Then
                ' /mute <Playername> [Duration] [Reason]
                Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(1).Value
                Dim Duration As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(2).Value
                Dim Reason As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s+(.+)\s*").Groups(3).Value
                Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Nothing, CInt(Duration), Reason, True)
                OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
            ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*") Then
                ' /mute <Playername> [Duration]
                Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*").Groups(1).Value
                Dim Duration As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s+(\d+)\s*").Groups(2).Value
                Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Nothing, CInt(Duration), "No reason.", True)
                OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
            ElseIf Regex.IsMatch(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s*") Then
                ' /mute <Playername>
                Dim PlayerName As String = Regex.Match(Message, "\/[Mm][Uu][Tt][Ee]\s+(\w+)\s*").Groups(1).Value
                Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)
                Dim ReturnMessage As String = OldPlayer.AddMute(PlayerIndex, Nothing, Integer.MaxValue, "No reason.", True)
                OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
            Else
                HandleChatMessageCommandHelp("/help mute")
            End If
        ElseIf Regex.IsMatch(Message, "\/[Oo][Pp]\s*") Then
            ' /op <PlayerName> <Permission>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.GlobalModerator) Then
                    If Regex.IsMatch(Message, "\/[Oo][Pp]\s+(\w+)\s+(\d)\s*") Then
                        Dim PlayerName As String = Regex.Match(Message, "\/[Oo][Pp]\s+(\w+)\s+(\d)\s*").Groups(1).Value
                        Dim Permission As Integer = CInt(Regex.Match(Message, "\/[Oo][Pp]\s+(\w+)\s+(\d)\s*").Groups(2).Value)
                        Dim Reason As String = Nothing
                        Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)

                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            Reason = "The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") added this player as an operator."
                        Else
                            Reason = "The player " + OldPlayer.Name(CurrentPlayerIndex) + " added this player as an operator."
                        End If

                        Dim ReturnMessage As String = OldPlayer.AddOperator(PlayerIndex, Permission, Reason)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help op", Client)
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Global Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use op command on the server due to the following reason: This command require you to have Global Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use op command on the server due to the following reason: This command require you to have Global Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Oo][Pp]\s+(\w+)\s+(\d)\s*") Then
                    Dim PlayerName As String = Regex.Match(Message, "\/[Oo][Pp]\s+(\w+)\s+(\d)\s*").Groups(1).Value
                    Dim Permission As Integer = CInt(Regex.Match(Message, "\/[Oo][Pp]\s+(\w+)\s+(\d)\s*").Groups(2).Value)
                    Dim Reason As String = Nothing
                    Dim PlayerIndex As Integer = OldPlayer.Name.IndexOf(PlayerName)

                    Dim ReturnMessage As String = OldPlayer.AddOperator(PlayerIndex, Permission)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help op")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Pp][Mm]\s*") Then
            ' /pm
            If IsHost Then
                OldServerClient.QueueMessage("Unable to use this command.", Main.LogType.Info)
            End If
        ElseIf Regex.IsMatch(Message, "\/[Rr][Ee][Ss][Tt][Aa][Rr][Tt]\s*") Then
            ' /restart
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.Administrator) Then
                    Application.Restart()
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Administrator permission."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use restart command on the server due to the following reason: This command require you to have Administrator permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use restart command on the server due to the following reason: This command require you to have Administrator permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                Application.Restart()
            End If
        ElseIf Regex.IsMatch(Message, "\/[Ss][Aa][Yy]\s*") Then
            ' /say
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ChatModerator) Then
                    If Not OldPlayer.isMuted(CurrentPlayerIndex) Then
                        If Regex.IsMatch(Message, "\/[Ss][Aa][Yy]\s+(.+)") Then
                            Dim Text As String = Regex.Match(Message, "\/[Ss][Aa][Yy]\s+(.+)").Groups(1).Value
                            OldPlayer.SendAllData(CreateData(PackageTypes.ChatMessage, -1, Text), Client)
                            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                                OldServerClient.QueueMessage("[Server Chat]" + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + "): " + Text, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                            Else
                                OldServerClient.QueueMessage("[Server Chat]" + OldPlayer.Name(CurrentPlayerIndex) + ": " + Text, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                            End If
                        Else
                            HandleChatMessageCommandHelp("/help say", Client)
                        End If
                    Else
                        Dim MuteData As String = OldPlayer.GetMuteReason(OldPlayer.PlayerClient.IndexOf(Client))
                        ' Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time
                        Dim RemainBanTime As TimeSpan = (CType(Functions.GetSplit(MuteData, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(MuteData, 4, "|"))) - Date.Now)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds."), Client)
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to chat on the server due to the following reason: The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        Else
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to chat on the server due to the following reason: The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        End If
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Chat Moderator permission or above."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use say command on the server due to the following reason: This command require you to have Chat Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use say command on the server due to the following reason: This command require you to have Chat Moderator permission or above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Ss][Aa][Yy]\s+(.+)") Then
                    Dim Text As String = Regex.Match(Message, "\/[Ss][Aa][Yy]\s+(.+)").Groups(1).Value
                    OldPlayer.SendAllData(CreateData(PackageTypes.ChatMessage, -1, Text))
                    OldServerClient.QueueMessage(Text, Main.LogType.Server)
                Else
                    HandleChatMessageCommandHelp("/help say")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s*") Then
            ' /season <id>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s+(-{0,1}\d+)\s*") Then
                        ' /season <id>
                        Dim ID As String = Regex.Match(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s+(-{0,1}\d+)\s*").Groups(1).Value
                        OldWorld.Change(CInt(ID), -3, 0)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "You have changed the season to " + OldWorld.GetSeasonName), Client)
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") changed the season.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        Else
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " changed the season.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        End If
                    Else
                        HandleChatMessageCommandHelp("/help season", Client)
                    End If
                Else
                    If Regex.IsMatch(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s+(-{0,1}\d+)\s*") Then
                        ' /season <id>
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            Dim ID As String = Regex.Match(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s+(-{0,1}\d+)\s*").Groups(1).Value
                            If OldOnlineSetting.HaveSettingFile(OldPlayer.GameJoltID(CurrentPlayerIndex)) Then
                                OldOnlineSetting.Load(OldPlayer.GameJoltID(CurrentPlayerIndex))
                                OldWorld.Change(CInt(ID), CInt(OldOnlineSetting.Weather), 0, Client, "Season")
                            Else
                                OldWorld.Change(CInt(ID), -3, 0, Client, "Season")
                            End If
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") changed the season for himself.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        Else
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Online Profile or Server Moderator and above."), Client)
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use season command on the server due to the following reason: This command require you to have Online Profile or Server Moderator and above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        End If
                    Else
                        HandleChatMessageCommandHelp("/help season", Client)
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s+(-{0,1}\d+)\s*") Then
                    ' /season <id>
                    Dim ID As String = Regex.Match(Message, "\/[Ss][Ee][Aa][Ss][Oo][Nn]\s+(-{0,1}\d+)\s*").Groups(1).Value
                    OldWorld.Change(CInt(ID), -3, 0)
                Else
                    HandleChatMessageCommandHelp("/help season")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Ss][Tt][Oo][Pp]\s*") Then
            '/stop
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.Administrator) Then
                    Application.Exit()
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Administrator permission."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use stop command on the server due to the following reason: This command require you to have Administrator permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use stop command on the server due to the following reason: This command require you to have Administrator permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                Application.Exit()
            End If
        ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s*") Then
            ' /unban <PlayerName> <Scope>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s+(\w+)\s*") Then
                        ' /unban <PlayerName> <Scope>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                        Dim Scope As String = Regex.Match(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                        Dim ReturnMessage As String = Nothing
                        If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveBan(PlayerName, True, True)
                        ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveBan(PlayerName, False, True)
                        Else
                            ReturnMessage = "The server is unable to remove the player from the ban list as the scope is not properly defined."
                        End If
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s*") Then
                        ' /unban <PlayerName>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s*").Groups(1).Value
                        Dim ReturnMessage As String = OldPlayer.RemoveBan(PlayerName, False, False)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help unban")
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator and above permission."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use unban command on the server due to the following reason: This command require you to have Server Moderator and above permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use unban command on the server due to the following reason: This command require you to have Server Moderator and above permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s+(\w+)\s*") Then
                    ' /unban <PlayerName> <Scope>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                    Dim Scope As String = Regex.Match(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                    Dim ReturnMessage As String = Nothing
                    If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                        ReturnMessage = OldPlayer.RemoveBan(PlayerName, True, True)
                    ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                        ReturnMessage = OldPlayer.RemoveBan(PlayerName, False, True)
                    Else
                        ReturnMessage = "The server is unable to remove the player from the ban list as the scope is not properly defined."
                    End If
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s*") Then
                    ' /unban <PlayerName>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Bb][Aa][Nn]\s+(\w+)\s*").Groups(1).Value
                    Dim ReturnMessage As String = OldPlayer.RemoveBan(PlayerName, False, False)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help unban")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Ii][Pp][Bb][Aa][Nn]\s*") Then
            ' /unipban <ip>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ChatModerator) Then
                    If Regex.IsMatch(Message, "\/[Uu][Nn][Ii][Pp][Bb][Aa][Nn]\s+((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\s*$") Then
                        ' /unipban <ip>
                        Dim PlayerIP As String = Regex.Match(Message, "\/[Uu][Nn][Ii][Pp][Bb][Aa][Nn]\s+((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\s*$").Groups(1).Value
                        Dim ReturnMessage As String = OldPlayer.RemoveIPBan(PlayerIP)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help unipban")
                    End If
                Else
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Server Moderator and above permission."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to use unipban command on the server due to the following reason: This command require you to have Server Moderator and above permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use unipban command on the server due to the following reason: This command require you to have Server Moderator and above permission.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Uu][Nn][Ii][Pp][Bb][Aa][Nn]\s+((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\s*$") Then
                    ' /unipban <ip>
                    Dim PlayerIP As String = Regex.Match(Message, "\/[Uu][Nn][Ii][Pp][Bb][Aa][Nn]\s+((?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?))\s*$").Groups(1).Value
                    Dim ReturnMessage As String = OldPlayer.RemoveIPBan(PlayerIP)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help unipban")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s*") Then
            ' /unmute <PlayerName> <Scope>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ChatModerator) Then
                    If Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*") Then
                        ' /unmute <PlayerName> <Scope>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                        Dim Scope As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                        Dim ReturnMessage As String = Nothing
                        If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveMute(PlayerName, True, True, True, Client)
                        ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveMute(PlayerName, False, True, True, Client)
                        Else
                            ReturnMessage = "The server is unable to remove the player from the mute list as the scope is not properly defined."
                        End If
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s*") Then
                        ' /unmute <PlayerName>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s*").Groups(1).Value
                        Dim ReturnMessage As String = OldPlayer.RemoveMute(PlayerName, False, False, True, Client)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help unmute")
                    End If
                Else
                    If Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*") Then
                        ' /unmute <PlayerName> <Scope>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                        Dim Scope As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                        Dim ReturnMessage As String = Nothing
                        If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveMute(PlayerName, True, True, False, Client)
                        ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                            ReturnMessage = OldPlayer.RemoveMute(PlayerName, False, True, True, Client)
                        Else
                            ReturnMessage = "The server is unable to remove the player from the mute list as the scope is not properly defined."
                        End If
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s*") Then
                        ' /unmute <PlayerName>
                        Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s*").Groups(1).Value
                        Dim ReturnMessage As String = OldPlayer.RemoveMute(PlayerName, False, False, False, Client)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, ReturnMessage), Client)
                        OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        HandleChatMessageCommandHelp("/help unmute")
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*") Then
                    ' /unmute <PlayerName> <Scope>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*").Groups(1).Value
                    Dim Scope As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s+(\w+)\s*").Groups(2).Value
                    Dim ReturnMessage As String = Nothing
                    If String.Equals(Scope, "True", StringComparison.OrdinalIgnoreCase) Then
                        ReturnMessage = OldPlayer.RemoveMute(PlayerName, True, True, True, Client)
                    ElseIf String.Equals(Scope, "False", StringComparison.OrdinalIgnoreCase) Then
                        ReturnMessage = OldPlayer.RemoveMute(PlayerName, False, True, True, Client)
                    Else
                        ReturnMessage = "The server is unable to remove the player from the mute list as the scope is not properly defined."
                    End If
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                ElseIf Regex.IsMatch(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s*") Then
                    ' /unmute <PlayerName>
                    Dim PlayerName As String = Regex.Match(Message, "\/[Uu][Nn][Mm][Uu][Tt][Ee]\s+(\w+)\s*").Groups(1).Value
                    Dim ReturnMessage As String = OldPlayer.RemoveMute(PlayerName, False, False, True, Client)
                    OldServerClient.QueueMessage(ReturnMessage, Main.LogType.Info)
                Else
                    HandleChatMessageCommandHelp("/help unmute")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s*") Then
            ' /weather <id>
            If Not IsHost Then
                If OldPlayer.CheckPermission(CurrentPlayerIndex, OldPlayer.OPPermissionLevel.ServerModerator) Then
                    If Regex.IsMatch(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s+(-{0,1}\d+)\s*") Then
                        ' /weather <id>
                        Dim ID As String = Regex.Match(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s+(-{0,1}\d+)\s*").Groups(1).Value
                        OldWorld.Change(-3, CInt(ID), 0)
                        OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "You have changed the weather to " + OldWorld.GetWeatherName), Client)
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") changed the weather.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        Else
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " changed the weather.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        End If
                    Else
                        HandleChatMessageCommandHelp("/help weather", Client)
                    End If
                Else
                    If Regex.IsMatch(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s+(-{0,1}\d+)\s*") Then
                        ' /weather <id>
                        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                            Dim ID As String = Regex.Match(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s+(-{0,1}\d+)\s*").Groups(1).Value
                            If OldOnlineSetting.HaveSettingFile(OldPlayer.GameJoltID(CurrentPlayerIndex)) Then
                                OldOnlineSetting.Load(OldPlayer.GameJoltID(CurrentPlayerIndex))
                                OldWorld.Change(CInt(OldOnlineSetting.Season), CInt(ID), 0, Client, "Weather")
                            Else
                                OldWorld.Change(-3, CInt(ID), 0, Client, "Weather")
                            End If
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") changed the weather for himself.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        Else
                            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "This command require you to have Online Profile or Server Moderator and above."), Client)
                            OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to use weather command on the server due to the following reason: This command require you to have Online Profile or Server Moderator and above.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                        End If
                    Else
                        HandleChatMessageCommandHelp("/help weather", Client)
                    End If
                End If
            Else
                If Regex.IsMatch(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s+(-{0,1}\d+)\s*") Then
                    ' /weather <id>
                    Dim ID As String = Regex.Match(Message, "\/[Ww][Ee][Aa][Tt][Hh][Ee][Rr]\s+(-{0,1}\d+)\s*").Groups(1).Value
                    OldWorld.Change(-3, CInt(ID), 0)
                Else
                    HandleChatMessageCommandHelp("/help weather")
                End If
            End If
        ElseIf Regex.IsMatch(Message, "\/[RR][Ee][Ff][Rr][Ee][Ss][Hh]\s*") Then
            ' /refresh
            If Not IsHost Then
                OldSetting.Load()
                OldServerClient.QueueMessage("Setting have been refreshed.", Main.LogType.Info)
            End If
        Else
            ' No Command or not valid
            If Not IsHost Then
                If OldPlayer.isMuted(CurrentPlayerIndex) Then
                    Dim MuteData As String = OldPlayer.GetMuteReason(CurrentPlayerIndex)
                    ' Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time
                    Dim RemainBanTime As TimeSpan = (CType(Functions.GetSplit(MuteData, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(MuteData, 4, "|"))) - Date.Now)
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds."), Client)
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to chat on the server due to the following reason: The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    Else
                        OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to chat on the server due to the following reason: The server you are trying to chat on banned you due to the following reason: " + Functions.GetSplit(MuteData, 2, "|") + " | Your remaining Ban Time: " + RemainBanTime.Days.ToString + " days " + RemainBanTime.Hours.ToString + " hours " + RemainBanTime.Minutes.ToString + " minutes " + RemainBanTime.Seconds.ToString + " seconds.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                    End If
                Else
                    OldPlayer.SendAllData(CreateData(PackageTypes.ChatMessage, OldPlayer.PlayerID(CurrentPlayerIndex), Message), Client)
                    Dim Message1 As String = Nothing
                    If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                        Message1 += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + "): "
                    Else
                        Message1 += OldPlayer.Name(CurrentPlayerIndex) + ": "
                    End If
                    OldServerClient.QueueMessage(Message1 + Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
                End If
            Else
                HandleChatMessageCommandHelp("/help")
            End If
        End If
    End Sub

    Public Shared Sub HandleChatMessageCommandHelp(ByVal Message As String, Optional ByVal Client As TcpClient = Nothing)
        Dim HelpPage As Integer = 0
        Dim Items As New List(Of String)
        If Regex.IsMatch(Message, "\/[Hh][Ee][Ll][Pp]\s+(\d+)\s*") Or Regex.IsMatch(Message, "\/[Hh][Ee][Ll][Pp]\s*$") Then
            ' It is a Page Index.
            If Regex.IsMatch(Message, "\/[Hh][Ee][Ll][Pp]\s*$") Then
                HelpPage = 1
            ElseIf CInt(Regex.Match(Message, "\/[Hh][Ee][Ll][Pp]\s+(\d+)\s*").Groups(1).Value) < 0 Then
                ' It is first page.
                HelpPage = 1
            ElseIf CInt(Regex.Match(Message, "\/[Hh][Ee][Ll][Pp]\s+(\d+)\s*").Groups(1).Value) > 0 Then
                HelpPage = CInt(Regex.Match(Message, "\/[Hh][Ee][Ll][Pp]\s+(\d+)\s*").Groups(1).Value)
            End If

            Select Case HelpPage
                ' Each Page can only have 14.
                Case Is <= 1
                    Items.Add("---------- Help: Index (1/5) ----------")
                    Items.Add("Use /help [Command/Index] to get page index of help.")
                    Items.Add("---------- Permission: Everyone ----------")
                    Items.Add("/checkmute - To return a list of players who were muted. (Online Profile Only)")
                    Items.Add("/checkop - To return a list of players who were operator.")
                    Items.Add("/mute - To mute a player in the server. (Online Profile Only)")
                    Items.Add("/pm - To chat privately with your friends.")
                    Items.Add("/season - To change the season in the server globally. (Online Profile Only)")
                    Items.Add("/unmute - To unmute a player in the server. (Online Profile Only)")
                    Items.Add("/weather - To change the weather in the server globally. (Online Profile Only)")
                Case 2
                    Items.Add("---------- Help: Index (2/5) ----------")
                    Items.Add("Use /help [Command/Index] to get page index of help.")
                    Items.Add("---------- Permission: Chat Moderator and above ----------")
                    Items.Add("/checkmute - To return a list of players who were muted.")
                    Items.Add("/mute - To mute a player in the server.")
                    Items.Add("/say - To make a server announcement.")
                    Items.Add("/unmute - To unmute a player in the server.")
                Case 3
                    Items.Add("---------- Help: Index (3/5) ----------")
                    Items.Add("Use /help [Command/Index] to get page index of help.")
                    Items.Add("---------- Permission: Server Moderator and above ----------")
                    Items.Add("/ban - To ban a player in the server.")
                    Items.Add("/checkban - To return a list of players who were banned.")
                    Items.Add("/checkipban - To return a list of players ip who were banned.")
                    Items.Add("/find - To find a player in the server.")
                    Items.Add("/ipban - To ip ban a player in the server.")
                    Items.Add("/kick - To remove a player in the server.")
                    Items.Add("/unban - To unban a player in the server.")
                    Items.Add("/unipban - To unban a player in the server.")
                Case 4
                    Items.Add("---------- Help: Index (4/5) ----------")
                    Items.Add("Use /help [Command/Index] to get page index of help.")
                    Items.Add("---------- Permission: Global Moderator and above ----------")
                    Items.Add("/deop - To remove a player as operator in the server.")
                    Items.Add("/op - To add players as operator in the server.")
                Case Is >= 5
                    Items.Add("---------- Help: Index (5/5) ----------")
                    Items.Add("Use /help [Command/Index] to get page index of help.")
                    Items.Add("---------- Permission: Administrator ----------")
                    Items.Add("/restart - To restart the server.")
                    Items.Add("/stop - To close the server.")
            End Select

        ElseIf Regex.IsMatch(Message, "\/help\s+(\w+)\s*") Then
            ' It is a command.
            If String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "ban", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Ban ----------")
                Items.Add("Usage: /ban [Name] [Optional:Duration in seconds] [Optional:Reason]")
                Items.Add("Duration: 1 Minute = 60, 1 Hour = 3600, 1 Day = 86400, 1 Week = 604800, 1 Month = 2419200, Permanent = 2147483647")
                Items.Add("Description: To ban a player in the server.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "checkban", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Checkban ----------")
                Items.Add("Usage: /checkban [Index or Name]")
                Items.Add("Description: To return a list of players who were banned.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "checkipban", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Checkipban ----------")
                Items.Add("Usage: /checkipban [Index or IP]")
                Items.Add("Description: To return a list of players ip who were banned.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "checkmute", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Checkmute ----------")
                Items.Add("Usage: /checkmute [Index or Name]")
                Items.Add("Description: To return a list of players who were muted.")
                Items.Add("Permission Level: Online Profile Players, Chat Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "checkop", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Checkop ----------")
                Items.Add("Usage: /checkop [Index or Name]")
                Items.Add("Description: To return a list of players who were operator.")
                Items.Add("Permission Level: Everyone.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "deop", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Deop ----------")
                Items.Add("Usage: /deop [Name] [Optional:Scope]")
                Items.Add("Scope: False for offline search only, True for online search only.")
                Items.Add("Description: To remove players as operator in the server.")
                Items.Add("Permission Level: Global Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "find", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Find ----------")
                Items.Add("Usage: /find [Name]")
                Items.Add("Description: To find a player in the server.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "ipban", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: IP Ban ----------")
                Items.Add("Usage: /ipban [Name]")
                Items.Add("Description: To ip ban a player in the server.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "kick", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Kick ----------")
                Items.Add("Usage: /kick [Name] [Optional:Reason]")
                Items.Add("Description: To remove a player in the server.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "mute", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Mute ----------")
                Items.Add("Usage: /mute [Name] [Optional:Duration in seconds] [Optional:Reason]")
                Items.Add("Duration: 1 Minute = 60, 1 Hour = 3600, 1 Day = 86400, 1 Week = 604800, 1 Month = 2419200, Permanent = 2147483647")
                Items.Add("Description: To mute a player in the server.")
                Items.Add("Permission Level: Online Profile Players, Chat Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "op", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: OP ----------")
                Items.Add("Usage: /op [Name] [PermissionIndex]")
                Items.Add("PermissionIndex: 1 = Chat Moderator, 2 = Server Moderator, 3 = Global Moderator, 4 = Administrator")
                Items.Add("Description: To add players as operator in the server.")
                Items.Add("Permission Level: Global Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "pm", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: PM ----------")
                Items.Add("Usage: /pm [Name] [Message]")
                Items.Add("Description: To chat privately with your friends.")
                Items.Add("Permission Level: Everyone can use this command except server client.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "restart", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Restart ----------")
                Items.Add("Usage: /restart")
                Items.Add("Description: To restart the server.")
                Items.Add("Permission Level: Administrator.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "say", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Say ----------")
                Items.Add("Usage: /say [Message]")
                Items.Add("Description: To make a server announcement.")
                Items.Add("Permission Level: Chat Moderator or above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "season", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Season ----------")
                Items.Add("Usage: /season [ID]")
                Items.Add("ID: 0 = Winter, 1 = Spring, 2 = Summer, 3 = Fall, -1 = Random, -2 = P3D Default, -3 = Server Default")
                Items.Add("Description: To change the season in the server globally.")
                Items.Add("Permission Level: Online Profile Players, Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "stop", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Stop ----------")
                Items.Add("Usage: /stop")
                Items.Add("Description: To close the server.")
                Items.Add("Permission Level: Administrator.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "unban", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Unban ----------")
                Items.Add("Usage: /unban [Name] [Optional:Scope]")
                Items.Add("Scope: False for offline search only, True for online search only.")
                Items.Add("Description: To unban a player in the server.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "unipban", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Unipban ----------")
                Items.Add("Usage: /unipban [IP]")
                Items.Add("Description: To unban a player ip in the server.")
                Items.Add("Permission Level: Server Moderator and above.")
            ElseIf String.Equals(Regex.Match(Message, "\/help\s+(\w+)\s*").Groups(1).Value, "weather", StringComparison.OrdinalIgnoreCase) Then
                Items.Add("---------- Help: Weather ----------")
                Items.Add("Usage: /weather [ID]")
                Items.Add("ID: 0 = Clear, 1 = Rain, 2 = Snow, 3 = Underwater, 4 = Sunny, 5 = Fog, 6 = Thunderstorm, 7 = Sandstorm, 8 = Ash, 9 = Blizzard, -1 = Random, -2 = P3D Default, -3 = Server Default")
                Items.Add("Description: To change the weather in the server globally.")
                Items.Add("Permission Level: Online Profile Players, Server Moderator and above.")
            End If
        End If

        If Items.Count > 0 Then
            For Each Item As String In Items
                If Client IsNot Nothing Then
                    OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, Item), Client)
                Else
                    OldServerClient.QueueMessage(Item, Main.LogType.Info)
                End If
            Next
        End If
    End Sub

    Public Shared Sub HandleGamestateMessage(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)

        If OldPlayer.isGameJoltPlayer(OldPlayer.PlayerClient.IndexOf(Client)) Then
            OldPlayer.SendAllData(CreateData(PackageTypes.ChatMessage, -1, "The player " + OldPlayer.Name(OldPlayer.PlayerClient.IndexOf(Client)) + " (" + OldPlayer.GameJoltID(OldPlayer.PlayerClient.IndexOf(Client)).ToString + ") " + DataItems(0)), Client)
            OldServerClient.QueueMessage("The player " + OldPlayer.Name(OldPlayer.PlayerClient.IndexOf(Client)) + " (" + OldPlayer.GameJoltID(OldPlayer.PlayerClient.IndexOf(Client)).ToString + ") " + DataItems(0), Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        Else
            OldPlayer.SendAllData(CreateData(PackageTypes.ChatMessage, -1, "The player " + OldPlayer.Name(OldPlayer.PlayerClient.IndexOf(Client)) + " " + DataItems(0)), Client)
            OldServerClient.QueueMessage("The player " + OldPlayer.Name(OldPlayer.PlayerClient.IndexOf(Client)) + " " + DataItems(0), Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        End If
    End Sub

    Public Shared Sub HandleTradeRequest(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))
        Dim TimeLeft As TimeSpan = (CType(OldSetting.StartTime, Date).AddSeconds(CDbl(OldSetting.AutoRestartTime)) - Date.Now)

        If Not String.Equals(OldSetting.AutoRestartTime, "0", StringComparison.OrdinalIgnoreCase) And CInt(TimeLeft.TotalSeconds) <= 300 Then
            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server you are trying to trade on is disabled due to an incoming server restart in less than 5 minutes."), Client)
            OldPlayer.SendData(CreateData(PackageTypes.TradeQuit, OldPlayer.PlayerID(CurrentPlayerIndex), " "), Client)
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to send a trade request due to the following reason: The server you are trying to trade on is disabled due to an incoming server restart in less than 5 minutes.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
            Else
                OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to send a trade request due to the following reason: The server you are trying to trade on is disabled due to an incoming server restart in less than 5 minutes.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
            End If
        Else
            OldPlayer.SendData(CreateData(PackageTypes.TradeRequest, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))
            Dim Message As String = Nothing
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                Message += "The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") have send a trade request to "
            Else
                Message += "The player " + OldPlayer.Name(CurrentPlayerIndex) + " have send a trade request to "
            End If
            If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
                Message += OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ")."
            Else
                Message += OldPlayer.Name(TradePlayerIndex) + "."
            End If
            OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        End If
    End Sub

    Public Shared Sub HandleTradeJoin(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.TradeJoin, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have joined the trade with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have joined the trade with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + "."
        End If
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
    End Sub

    Public Shared Sub HandleTradeQuit(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.TradeQuit, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have left the trade with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have left the trade with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + "."
        End If
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
    End Sub

    Public Shared Sub HandleTradeOffer(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.TradeOffer, OldPlayer.PlayerID(CurrentPlayerIndex), DataItems(1)), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have offer the trade with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have offer the trade with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") with the following Data: " + DataItems(1) + "."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + " with the following Data: " + DataItems(1) + "."
        End If

#If Release = "SViper" Then
        ' SViper Server Client
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
#ElseIf Release = "Official" Then
            ' Official Release
            'ServerClient.QueueMessage(Message, Main.LogType.Hidden, CType(Client.Client.RemoteEndPoint, IPEndPoint))
#End If
    End Sub

    Public Shared Sub HandleTradeStart(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.TradeStart, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have accepted the trade with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have accepted the trade with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + "."
        End If
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
    End Sub

    Public Shared Sub HandleBattleRequest(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))
        Dim TimeLeft As TimeSpan = (CType(OldSetting.StartTime, Date).AddSeconds(CDbl(OldSetting.AutoRestartTime)) - Date.Now)

        If Not String.Equals(OldSetting.AutoRestartTime, "0", StringComparison.OrdinalIgnoreCase) And CInt(TimeLeft.TotalSeconds) <= 300 Then
            OldPlayer.SendData(CreateData(PackageTypes.ChatMessage, -1, "The server you are trying to battle on is disabled due to an incoming server restart in less than 5 minutes."), Client)
            OldPlayer.SendData(CreateData(PackageTypes.BattleQuit, OldPlayer.PlayerID(CurrentPlayerIndex), " "), Client)
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") is unable to send a battle request due to the following reason: The server you are trying to battle on is disabled due to an incoming server restart in less than 5 minutes.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
            Else
                OldServerClient.QueueMessage("The player " + OldPlayer.Name(CurrentPlayerIndex) + " is unable to send a battle request due to the following reason: The server you are trying to battle on is disabled due to an incoming server restart in less than 5 minutes.", Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
            End If
        Else
            OldPlayer.SendData(CreateData(PackageTypes.BattleRequest, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))
            Dim Message As String = Nothing
            If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
                Message += "The player " + OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") have send a battle request to "
            Else
                Message += "The player " + OldPlayer.Name(CurrentPlayerIndex) + " have send a battle request to "
            End If
            If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
                Message += OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ")."
            Else
                Message += OldPlayer.Name(TradePlayerIndex) + "."
            End If
            OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
        End If
    End Sub

    Public Shared Sub HandleBattleJoin(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattleJoin, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have joined the battle with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have joined the battle with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + "."
        End If
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
    End Sub

    Public Shared Sub HandleBattleQuit(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattleQuit, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have left the battle with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have left the battle with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + "."
        End If
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
    End Sub

    Public Shared Sub HandleBattleOffer(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattleOffer, OldPlayer.PlayerID(CurrentPlayerIndex), DataItems(1)), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have offer the battle with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have offer the battle with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ") with the following Data: "
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + " with the following Data: "
        End If

#If Release = "SViper" Then
        ' SViper Server Client
        OldServerClient.QueueMessage(Message + DataItems(1), Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
#ElseIf Release = "Official" Then
            ' Official Release
            ' ServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
#End If
    End Sub

    Public Shared Sub HandleBattleStart(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattleStart, OldPlayer.PlayerID(CurrentPlayerIndex), " "), OldPlayer.PlayerClient(TradePlayerIndex))

        Dim Message As String = Nothing
        If OldPlayer.isGameJoltPlayer(TradePlayerIndex) Then
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " (" + OldPlayer.GameJoltID(TradePlayerIndex).ToString + ") have accepted the battle with "
        Else
            Message += "The player " + OldPlayer.Name(TradePlayerIndex) + " have accepted the battle with "
        End If
        If OldPlayer.isGameJoltPlayer(CurrentPlayerIndex) Then
            Message += OldPlayer.Name(CurrentPlayerIndex) + " (" + OldPlayer.GameJoltID(CurrentPlayerIndex).ToString + ")."
        Else
            Message += OldPlayer.Name(CurrentPlayerIndex) + "."
        End If
        OldServerClient.QueueMessage(Message, Main.LogType.Server, CType(Client.Client.RemoteEndPoint, IPEndPoint))
    End Sub

    Public Shared Sub HandleBattleClientData(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattleClientData, OldPlayer.PlayerID(CurrentPlayerIndex), DataItems(1)), OldPlayer.PlayerClient(TradePlayerIndex))
    End Sub

    Public Shared Sub HandleBattleHostData(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattleHostData, OldPlayer.PlayerID(CurrentPlayerIndex), DataItems(1)), OldPlayer.PlayerClient(TradePlayerIndex))
    End Sub

    Public Shared Sub HandleBattlePokemonData(ByVal Data As String, ByVal Client As TcpClient)
        OldPlayer.PlayerLastPing(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        If Not OldPlayer.BusyType(OldPlayer.PlayerClient.IndexOf(Client)) = OldPlayer.BusyTypes.Inactive Then
            OldPlayer.PlayerLastAFK(OldPlayer.PlayerClient.IndexOf(Client)) = Date.Now
        End If

        Dim DataItems As List(Of String) = DataItem(Data)
        Dim CurrentPlayerIndex As Integer = OldPlayer.PlayerClient.IndexOf(Client)
        Dim TradePlayerIndex As Integer = OldPlayer.PlayerID.IndexOf(CInt(DataItems(0)))

        OldPlayer.SendData(CreateData(PackageTypes.BattlePokemonData, OldPlayer.PlayerID(CurrentPlayerIndex), DataItems(1)), OldPlayer.PlayerClient(TradePlayerIndex))
    End Sub

    Public Shared Sub HandleServerDataRequest(ByVal Data As String, ByVal Client As TcpClient)
        Dim DataItems As New List(Of String)

        DataItems.Add(OldPlayer.PlayerID.Count.ToString)
        DataItems.Add(OldSetting.MaxPlayers.ToString)
        DataItems.Add(OldSetting.ServerName.ToString)
        DataItems.Add(OldSetting.ServerMessage.ToString)

        If OldPlayer.PlayerID.Count > 0 Then
            For Each Players As String In OldPlayer.Name
                DataItems.Add(Players)
            Next
        End If

        OldPlayer.SendData(CreateData(PackageTypes.ServerInfoData, -1, DataItems), Client)
    End Sub
End Class
#End If