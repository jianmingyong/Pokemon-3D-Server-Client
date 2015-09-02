Imports System.Net.Sockets
Imports System.IO

Public Class Player

    ' Base Player Package Data
    Public Property GameMode As String
    Public Property isGameJoltPlayer As Boolean
    Public Property GameJoltID As Integer
    Public Property DecimalSeparator As String
    Public Property Name As String
    Public Property LevelFile As String
    Public Property Position As String
    Public Property Position_X As Double
        Set(value As Double)
            Dim Position_Y As String = Position.GetSplit(1)
            Dim Position_Z As String = Position.GetSplit(2)
            _Position = MathHelper.ToString(value, DecimalSeparator) & "|" & Position_Y & "|" & Position_Z
        End Set
        Get
            Return Position.GetSplit(0).ToDouble
        End Get
    End Property
    Public Property Position_Y As Double
        Set(value As Double)
            Dim Position_X As String = Position.GetSplit(0)
            Dim Position_Z As String = Position.GetSplit(2)
            _Position = Position_X & "|" & MathHelper.ToString(value, DecimalSeparator) & "|" & Position_Z
        End Set
        Get
            Return Position.GetSplit(1).ToDouble
        End Get
    End Property
    Public Property Position_Z As Double
        Set(value As Double)
            Dim Position_X As String = Position.GetSplit(0)
            Dim Position_Y As String = Position.GetSplit(1)
            _Position = Position_X & "|" & Position_Y & "|" & MathHelper.ToString(value, DecimalSeparator)
        End Set
        Get
            Return Position.GetSplit(2).ToDouble
        End Get
    End Property
    Public Property Facing As Integer
    Public Property Moving As Boolean
    Public Property Skin As String
    Public Property BusyType As Integer
    Public Property PokemonVisible As Boolean
    Public Property PokemonPosition As String
    Public Property PokemonPosition_X As Double
        Set(value As Double)
            Dim Position_Y As String = PokemonPosition.GetSplit(1)
            Dim Position_Z As String = PokemonPosition.GetSplit(2)
            _Position = MathHelper.ToString(value, DecimalSeparator) & "|" & Position_Y & "|" & Position_Z
        End Set
        Get
            Return PokemonPosition.GetSplit(0).ToDouble
        End Get
    End Property
    Public Property PokemonPosition_Y As Double
        Set(value As Double)
            Dim Position_X As String = PokemonPosition.GetSplit(0)
            Dim Position_Z As String = PokemonPosition.GetSplit(2)
            _Position = Position_X & "|" & MathHelper.ToString(value, DecimalSeparator) & "|" & Position_Z
        End Set
        Get
            Return PokemonPosition.GetSplit(1).ToDouble
        End Get
    End Property
    Public Property PokemonPosition_Z As Double
        Set(value As Double)
            Dim Position_X As String = PokemonPosition.GetSplit(0)
            Dim Position_Y As String = PokemonPosition.GetSplit(1)
            _Position = Position_X & "|" & Position_Y & "|" & MathHelper.ToString(value, DecimalSeparator)
        End Set
        Get
            Return PokemonPosition.GetSplit(2).ToDouble
        End Get
    End Property
    Public Property PokemonSkin As String
    Public Property PokemonFacing As Integer

    ' Additional Player Package Data
    Public Property PlayerID As Integer
    Public Property PlayerClient As TcpClient
    Public Property PlayerLastValidPing As Date
    Public Property PlayerLastValidMovement As Date
    Public Property PlayerLoginStartTime As Date
    Public Property LastValidPackage As List(Of String)

    ' Spam Detection
    Public Property PlayerLastChat As String
    Public Property PlayerLastChatTime As Date

    Public ThreadCollection As New List(Of Threading.Thread)
    Public TimerCollection As New List(Of Timer)

    Private StreamReader As StreamReader

    Public Enum BusyTypes
        NotBusy
        Battling
        Chatting
        Inactive
    End Enum

    Public Enum OperatorPermission
        Player
        ChatModerator
        ServerModerator
        GlobalModerator
        Administrator
    End Enum

    Public Sub New(ByVal p As Package)
        Update(p, True, False)
    End Sub

    Public Sub New(ByVal p As Package, ByVal id As Integer)
        PlayerID = id
        PlayerClient = p.Client
        PlayerLastValidPing = Date.Now
        PlayerLastValidMovement = Date.Now
        PlayerLoginStartTime = Date.Now
        Update(p, True, False)
        Main.Player.Add(Me)

        Main.ServerClient.SendData(New Package(Package.PackageTypes.ID, id.ToString, p.Client))

        Dim NewPlayers As List(Of Player) = (From a As Player In Main.Player Select a Where Not a.PlayerID = id).ToList

        If NewPlayers IsNot Nothing AndAlso NewPlayers.Count > 0 Then
            For Each Player As Player In NewPlayers
                Main.ServerClient.SendData(New Package(Package.PackageTypes.CreatePlayer, Player.PlayerID.ToString, p.Client))
                Main.ServerClient.SendData(New Package(Package.PackageTypes.GameData, Player.PlayerID, Player.GenerateGameData(True), p.Client))
            Next
        End If

        Main.ServerClient.SendAllData(New Package(Package.PackageTypes.CreatePlayer, id.ToString, p.Client))
        Main.ServerClient.SendAllData(New Package(Package.PackageTypes.GameData, GenerateGameData(True), p.Client))

        Main.ServerClient.SendData(New Package(Package.PackageTypes.WorldData, Main.World.GenerateWorld(Me), p.Client))

        If Not String.IsNullOrWhiteSpace(Main.Setting.WelcomeMessage) Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.WelcomeMessage, p.Client))
        End If

        If isGameJoltPlayer Then
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", Name, GameJoltID.ToString, "join the game!"), p.Client))
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Name, GameJoltID.ToString, "join the game!"), Main.LogType.Info)
        Else
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", Name, "join the game!"), p.Client))
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Name, "join the game!"), Main.LogType.Info)
        End If

        If Main.RestartServerTimeLeft IsNot Nothing Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.RestartServerTimeLeft, p.Client))
        End If

        StartListening()
    End Sub

    Public Sub Update(ByVal p As Package, ByVal FullPackage As Boolean, Optional ByVal SentToServer As Boolean = True)
        PlayerLastValidMovement = Date.Now
        Dim List As New List(Of String)
        Dim Positions As New List(Of String)

        If FullPackage Then
            GameMode = p.DataItems(0)
            If p.DataItems(1) = "0" Then
                isGameJoltPlayer = False
            ElseIf p.DataItems(1) = "1"
                isGameJoltPlayer = True
            End If
            If isGameJoltPlayer Then
                GameJoltID = CInt(p.DataItems(2))
            Else
                GameJoltID = -1
            End If
            DecimalSeparator = p.DataItems(3)
            Name = p.DataItems(4)
            LevelFile = p.DataItems(5)
            Position = p.DataItems(6)
            Facing = CInt(p.DataItems(7))
            If p.DataItems(8) = "0" Then
                Moving = False
            ElseIf p.DataItems(8) = "1"
                Moving = True
            End If
            Skin = p.DataItems(9)
            BusyType = CInt(p.DataItems(10))
            If p.DataItems(11) = "0" Then
                PokemonVisible = False
            ElseIf p.DataItems(11) = "1"
                PokemonVisible = True
            End If
            PokemonPosition = p.DataItems(12)
            PokemonSkin = p.DataItems(13)
            PokemonFacing = CInt(p.DataItems(14))

            List.Add(LevelFile)
            List.Add(Position)
            List.Add(Facing.ToString)
            List.Add(Moving.ToString)
            List.Add(Skin)
            List.Add(BusyType.ToString)
            List.Add(PokemonVisible.ToString)
            List.Add(PokemonPosition)
            List.Add(PokemonSkin)
            List.Add(PokemonFacing.ToString)

            LastValidPackage = List
        Else
            List.Add(LevelFile)
            List.Add(Position)
            List.Add(Facing.ToString)
            List.Add(Moving.ToString)
            List.Add(Skin)
            List.Add(BusyType.ToString)
            List.Add(PokemonVisible.ToString)
            List.Add(PokemonPosition)
            List.Add(PokemonSkin)
            List.Add(PokemonFacing.ToString)
            LastValidPackage = List

            If Not String.IsNullOrWhiteSpace(p.DataItems(5)) AndAlso p.DataItems(5).SplitCount = 1 Then
                LevelFile = p.DataItems(5)
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(6)) AndAlso p.DataItems(6).SplitCount = 3 Then
                Position = p.DataItems(6)
                Positions = CatchUp(List(1))
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(7)) AndAlso p.DataItems(7).SplitCount = 1 Then
                Facing = CInt(p.DataItems(7))
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(8)) AndAlso p.DataItems(8).SplitCount = 1 Then
                If p.DataItems(8) = "0" Then
                    Moving = False
                ElseIf p.DataItems(8) = "1"
                    Moving = True
                End If
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(9)) AndAlso (p.DataItems(9).SplitCount = 1 OrElse p.DataItems(9).SplitCount = 2) Then
                Skin = p.DataItems(9)
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(10)) AndAlso p.DataItems(10).SplitCount = 1 Then
                BusyType = CInt(p.DataItems(10))
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(11)) AndAlso p.DataItems(11).SplitCount = 1 Then
                If p.DataItems(11) = "0" Then
                    PokemonVisible = False
                ElseIf p.DataItems(11) = "1"
                    PokemonVisible = True
                End If
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(12)) AndAlso p.DataItems(12).SplitCount = 3 Then
                PokemonPosition = p.DataItems(12)
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(13)) AndAlso p.DataItems(13).SplitCount = 1 Then
                PokemonSkin = p.DataItems(13)
            End If
            If Not String.IsNullOrWhiteSpace(p.DataItems(14)) AndAlso p.DataItems(14).SplitCount = 1 Then
                PokemonFacing = CInt(p.DataItems(14))
            End If
            Main.Main.UpdatePlayerList(Main.Operation.Update, Me)
        End If

        Try
            If SentToServer Then
                If FullPackage Then
                    Main.ServerClient.SendAllData(New Package(Package.PackageTypes.GameData, PlayerID, GenerateGameData(True), PlayerClient))
                Else
                    Dim TempPosition As List(Of String) = Positions
                    If TempPosition.Count > 0 Then
                        Dim PackageData As List(Of String) = GenerateGameData(False)
                        For Each Data As String In TempPosition
                            PackageData(6) = Data.GetSplit(0, ":")
                            PackageData(7) = Data.GetSplit(1, ":")
                            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.GameData, PlayerID, PackageData, PlayerClient))
                        Next
                    Else
                        Main.ServerClient.SendAllData(New Package(Package.PackageTypes.GameData, PlayerID, GenerateGameData(False), PlayerClient))
                    End If
                End If
            End If
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Sub Remove(ByVal Reason As String)
        If isGameJoltPlayer Then
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_GAMEJOLT", Name, GameJoltID.ToString, "left the server."), PlayerClient))
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_GAMEJOLT", Name, GameJoltID.ToString, "left the server with the following reason: " & Reason), Main.LogType.Info)
        Else
            Main.ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_NOGAMEJOLT", Name, "left the server."), PlayerClient))
            Main.Main.QueueMessage(Main.Setting.Token("SERVER_NOGAMEJOLT", Name, "left the server with the following reason: " & Reason), Main.LogType.Info)
        End If

        Main.ServerClient.SendAllData(New Package(Package.PackageTypes.DestroyPlayer, PlayerID.ToString, PlayerClient))
        If Not Reason = Main.Setting.Token("SERVER_PLAYERLEFT") Then
            Main.ServerClient.SendData(New Package(Package.PackageTypes.Kicked, Reason, PlayerClient))
        End If
    End Sub

    Public Function GetPlayerBusyType() As String
        Select Case BusyType
            Case BusyTypes.Battling
                Return " - Battling"
            Case BusyTypes.Chatting
                Return " - Chatting"
            Case BusyTypes.Inactive
                Return " - Inactive"
            Case BusyTypes.NotBusy
                Return ""
            Case Else
                Return Nothing
        End Select
    End Function

    Private Function GenerateGameData(ByVal FullPackageData As Boolean) As List(Of String)
        Dim List As New List(Of String)
        If FullPackageData Then
            List.Add(GameMode)
            If isGameJoltPlayer Then
                List.Add("1")
                List.Add(GameJoltID.ToString)
            Else
                List.AddRange({"0", ""})
            End If
            List.Add(DecimalSeparator)
            List.Add(Name)
            List.Add(LevelFile)
            List.Add(Position.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator))
            List.Add(Facing.ToString)
            If Moving Then
                List.Add("1")
            Else
                List.Add("0")
            End If
            List.Add(Skin)
            List.Add(BusyType.ToString)
            If PokemonVisible Then
                List.Add("1")
            Else
                List.Add("0")
            End If
            List.Add(PokemonPosition.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator))
            List.Add(PokemonSkin)
            List.Add(PokemonFacing.ToString)
        Else
            List.AddRange({"", "", "", "", Name})
            If LastValidPackage(0) = LevelFile Then
                List.Add("")
            Else
                List.Add(LevelFile)
            End If
            If LastValidPackage(1) = Position Then
                List.Add("")
            Else
                List.Add(Position.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator))
            End If
            If LastValidPackage(2) = Facing.ToString Then
                List.Add("")
            Else
                List.Add(Facing.ToString)
            End If
            If LastValidPackage(3) = Moving.ToString Then
                List.Add("")
            Else
                If Moving Then
                    List.Add("1")
                Else
                    List.Add("0")
                End If
            End If
            If LastValidPackage(4) = Skin Then
                List.Add("")
            Else
                List.Add(Skin)
            End If
            If LastValidPackage(5) = BusyType.ToString Then
                List.Add("")
            Else
                List.Add(BusyType.ToString)
            End If
            If LastValidPackage(6) = PokemonVisible.ToString Then
                List.Add("")
            Else
                If PokemonVisible Then
                    List.Add("1")
                Else
                    List.Add("0")
                End If
            End If
            If LastValidPackage(7) = PokemonPosition Then
                List.Add("")
            Else
                List.Add(PokemonPosition.Replace(".", DecimalSeparator).Replace(",", DecimalSeparator))
            End If
            If LastValidPackage(8) = PokemonSkin Then
                List.Add("")
            Else
                List.Add(PokemonSkin)
            End If
            If LastValidPackage(9) = PokemonFacing.ToString Then
                List.Add("")
            Else
                List.Add(PokemonFacing.ToString)
            End If
        End If
        Return List
    End Function

    Private Sub StartListening()
        Dim StartListeningThread As New Threading.Thread(AddressOf ThreadStartListening)
        Dim StartPingThread As New Threading.Thread(AddressOf ThreadStartPing)

        StartListeningThread.IsBackground = True
        StartListeningThread.Start()

        StartPingThread.IsBackground = True
        StartPingThread.Start()

        ThreadCollection.AddRange({StartListeningThread, StartPingThread})
    End Sub

    Private Sub ThreadStartListening()
        Do
            Try
                StreamReader = New StreamReader(PlayerClient.GetStream)
                If StreamReader.Peek > -1 Then
                    Dim ReturnMessage As String = StreamReader.ReadLine
                    Dim Package As New Package(ReturnMessage, PlayerClient)
                    Main.Main.QueueMessage("Player.vb: Receive: " & ReturnMessage, Main.LogType.Debug, PlayerClient)
                    If Package.IsValid Then
                        Threading.ThreadPool.QueueUserWorkItem(AddressOf Package.Handle)
                        PlayerLastValidPing = Date.Now
                    End If
                Else
                    If Main.Player.HasPlayer(PlayerID) Then
                        Main.Player.Remove(PlayerID, Main.Setting.Token("SERVER_PLAYERLEFT"))
                    End If
                    Exit Sub
                End If
            Catch ex As Exception
                If Main.Player.HasPlayer(PlayerID) Then
                    Main.Player.Remove(PlayerID, ex.Message)
                End If
                ex.CatchError
                Exit Sub
            End Try
        Loop
    End Sub

    Private Sub ThreadStartPing()
        Dim LastRecallTime As Integer = 0
        Do
            If Main.Player.HasPlayer(PlayerID) Then
                If Main.Setting.NoPingKickTime > 0 Then
                    If (Date.Now - PlayerLastValidPing).TotalSeconds >= Main.Setting.NoPingKickTime Then
                        Main.Player.Remove(PlayerID, Main.Setting.Token("SERVER_NOPING"))
                        Exit Sub
                    End If
                End If

                If Main.Setting.AFKKickTime > 0 AndAlso BusyType = BusyTypes.Inactive Then
                    Dim AFKTime As TimeSpan = Date.Now - PlayerLastValidMovement
                    Dim AFKTimeString As String = Nothing
                    If AFKTime.Hours = 1 Then
                        AFKTimeString &= AFKTime.Hours & " hour "
                    ElseIf AFKTime.Hours > 1
                        AFKTimeString &= AFKTime.Hours & " hours "
                    End If
                    If AFKTime.Minutes = 1 Then
                        AFKTimeString &= AFKTime.Minutes & " minute "
                    ElseIf AFKTime.Minutes > 1
                        AFKTimeString &= AFKTime.Minutes & " minutes "
                    End If
                    If AFKTime.Seconds <= 1 Then
                        AFKTimeString &= AFKTime.Seconds & " second "
                    ElseIf AFKTime.Seconds > 1
                        AFKTimeString &= AFKTime.Seconds & " seconds "
                    End If

                    If Main.Setting.AFKKickTime - AFKTime.TotalSeconds <= 0 Then
                        Main.Player.Remove(PlayerID, Main.Setting.Token("SERVER_AFKKICKED", AFKTimeString))
                        Exit Sub
                    ElseIf Main.Setting.AFKKickTime - AFKTime.TotalSeconds = 300 AndAlso Main.Setting.AFKKickTime >= 360 Then
                        Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_AFKWARNING", AFKTimeString, "5 minutes"), PlayerClient))
                    ElseIf Main.Setting.AFKKickTime - AFKTime.TotalSeconds = 60 AndAlso Main.Setting.AFKKickTime >= 120 Then
                        Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_AFKWARNING", AFKTimeString, "1 minute"), PlayerClient))
                    ElseIf Main.Setting.AFKKickTime - AFKTime.TotalSeconds <= 10 Then
                        Dim TimeRemaining As Integer = Main.Setting.AFKKickTime - CInt(AFKTime.TotalSeconds)
                        If TimeRemaining > 1 Then
                            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_AFKWARNING", AFKTimeString, TimeRemaining & " seconds"), PlayerClient))
                        ElseIf TimeRemaining = 1
                            Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_AFKWARNING", AFKTimeString, TimeRemaining & " second"), PlayerClient))
                        End If
                    End If
                End If

                If (Date.Now - PlayerLoginStartTime).TotalHours >= LastRecallTime + 1 Then
                    Main.ServerClient.SendData(New Package(Package.PackageTypes.ChatMessage, Main.Setting.Token("SERVER_LOGINREMINDER", (Date.Now - PlayerLoginStartTime).TotalHours.Floor.ToString & " hours"), PlayerClient))
                    LastRecallTime += 1
                End If
            Else
                Exit Sub
            End If

            Threading.Thread.Sleep(1000)
        Loop
    End Sub

    Private Function CatchUp(ByVal LastPosition As String) As List(Of String)
        Dim LastPositionX As Double = LastPosition.GetSplit(0).ToDouble.Round(2)
        Dim LastPositionY As Double = LastPosition.GetSplit(1).ToDouble.Round(2)
        Dim LastPositionZ As Double = LastPosition.GetSplit(2).ToDouble.Round(2)

        Dim Positions As New List(Of String)

        If Position_X.Round(2) > LastPositionX Then
            ' Going Right
            Do While LastPositionX < Position_X.Round(2)
                LastPositionX += "0.01".ToDouble
                Positions.Add(MathHelper.ToString(LastPositionX, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionY, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionZ, DecimalSeparator) & ":3")
            Loop
        ElseIf Position_X.Round(2) < LastPositionX
            ' Going Left
            Do While LastPositionX > Position_X.Round(2)
                LastPositionX -= "0.01".ToDouble
                Positions.Add(MathHelper.ToString(LastPositionX, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionY, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionZ, DecimalSeparator) & ":1")
            Loop
        ElseIf Position_Y.Round(2) > LastPositionY
            ' Going Up
            Do While LastPositionY < Position_Y.Round(2)
                LastPositionY += "0.01".ToDouble
                Positions.Add(MathHelper.ToString(LastPositionX, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionY, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionZ, DecimalSeparator) & ":" & Facing)
            Loop
        ElseIf Position_Y.Round(2) < LastPositionY
            ' Going Down
            Do While LastPositionY > Position_Y.Round(2)
                LastPositionY -= "0.01".ToDouble
                Positions.Add(MathHelper.ToString(LastPositionX, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionY, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionZ, DecimalSeparator) & ":" & Facing)
            Loop
        ElseIf Position_Z.Round(2) > LastPositionZ
            ' Going Back
            Do While LastPositionZ < Position_Z.Round(2)
                LastPositionZ += "0.01".ToDouble
                Positions.Add(MathHelper.ToString(LastPositionX, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionY, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionZ, DecimalSeparator) & ":2")
            Loop
        ElseIf Position_Z.Round(2) < LastPositionZ
            ' Going Forward
            Do While LastPositionZ > Position_Z.Round(2)
                LastPositionZ -= "0.01".ToDouble
                Positions.Add(MathHelper.ToString(LastPositionX, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionY, DecimalSeparator) & "|" & MathHelper.ToString(LastPositionZ, DecimalSeparator) & ":0")
            Loop
        End If
        Return Positions
    End Function
End Class