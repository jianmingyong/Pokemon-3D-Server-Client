Imports System.Net
Imports System.Net.Sockets
Imports System.IO

Public Class Main

#If RELEASEVERSION Then
    Public Shared Main As Main
    Public Shared Setting As New Setting
    Public Shared World As New World
    Public Shared Player As New PlayerCollection
    Public Shared ServerClient As New ServerClient
    Public Shared PackageHandler As New PackageHandler

    Public ThreadCollection As New List(Of Threading.Thread)
    Public TimerCollection As New List(Of Timer)

    Public Delegate Sub AddLogSafe(ByVal Message As String, ByVal Type As LogType, ByVal Client As TcpClient)
    Public Delegate Sub UpdatePlayerListSafe(ByVal Operation As Operation, ByVal p As Player)

    Public LastRestartTime As Date = Date.Now
    Private StopGUIUpdate As Boolean = False

    Public Enum Operation
        Add
        Remove
        Update
    End Enum

    Public Enum LogType
        Info
        Warning
        Debug
        Chat
        PM
        Server
        Trade
        PvP
        Command
    End Enum

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Main = Me
            If Not My.Computer.Network.IsAvailable Then
                ReturnMessage("No internet connection found. Please try again later.")
                Application.Exit()
                Exit Sub
            End If

            QueueMessage("Main.vb: Application started", LogType.Info)
            If Setting.HaveSettingFile("application_settings.json") Then
                Setting.Load()
            Else
                Application.Exit()
                Exit Sub
            End If

            If Setting.MaxPlayers > 0 Then
                Main_PlayerOnline.Text = "Player Online (0 / " & Setting.MaxPlayers.ToString & ")"
            Else
                Main_PlayerOnline.Text = "Player Online (0" & ")"
            End If

            ServerClient = New ServerClient
            ServerClient.Port(Setting.Port)
            ServerClient.Start()

            If Setting.AutoRestartTime > 0 AndAlso Setting.AutoRestartTime < 10 Then
                QueueMessage("Main.vb: The server client will not auto restart to prevent infinite restart loop. Please increase the timer to be at least 10 seconds.", LogType.Info)
            ElseIf Setting.AutoRestartTime > 0
                QueueMessage("Main.vb: Server is schedule to restart every " & Setting.AutoRestartTime.ToString & " seconds.", LogType.Info)
            End If

            Dim Timer As New Timer
            Dim Timer1 As New Timer

            AddHandler Timer.Tick, AddressOf RestartServerTimer
            AddHandler Timer1.Tick, AddressOf UpdateMenuBar

            Timer.Interval = 1000
            Timer.Start()

            Timer1.Interval = 1
            Timer1.Start()

            TimerCollection.AddRange({Timer, Timer1})
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Private Sub Main_FormClosing(sender As Object, e As EventArgs) Handles MyBase.FormClosing
        If Player.Count > 0 Then
            QueueMessage("Main.vb: Server is closing. Removing all players.", LogType.Info)
            ServerClient.SendAllData(New Package(Package.PackageTypes.ServerClose, Setting.Token("SERVER_CLOSE"), Nothing))
        End If
        Setting.Save()
        QueueMessage("Main.vb: Application closed successfully!", LogType.Info)
    End Sub

    Public Sub QueueMessage(ByVal Message As String, ByVal Type As LogType, Optional ByVal Client As TcpClient = Nothing)
        Try
            If InvokeRequired Then
                If Client Is Nothing Then
                    BeginInvoke(New AddLogSafe(AddressOf QueueMessage), Message, Type, Nothing)
                Else
                    BeginInvoke(New AddLogSafe(AddressOf QueueMessage), Message, Type, Client)
                End If
                Exit Sub
            End If

            Dim Logger As String = Nothing
            If Client Is Nothing Then
                Logger = Message
            Else
                Logger = CType(Client.Client.RemoteEndPoint, IPEndPoint).Address.ToString & ": " & Message
            End If

            Select Case Type
                Case LogType.Info
                    If Not Setting.LoggerInfo Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Info] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Info] " + Logger)
                    End If
                Case LogType.Warning
                    If Not Setting.LoggerWarning Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Warning] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Warning] " + Logger)
                    End If
                Case LogType.Debug
                    If Not Setting.LoggerDebug Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Debug] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Debug] " + Logger)
                    End If
                Case LogType.Chat
                    If Not Setting.LoggerChat Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Chat] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Chat] " + Logger)
                    End If
                Case LogType.PM
                    If Not Setting.LoggerPM Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [PM] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [PM] " + Logger)
                    End If
                Case LogType.Server
                    If Not Setting.LoggerServer Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Server] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Server] " + Logger)
                    End If
                Case LogType.Trade
                    If Not Setting.LoggerTrade Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Trade] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Trade] " + Logger)
                    End If
                Case LogType.PvP
                    If Not Setting.LoggerPvP Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [PvP] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [PvP] " + Logger)
                    End If
                Case LogType.Command
                    If Not Setting.LoggerCommand Then Exit Sub
                    If Main_Logger.Lines.Length = 0 Then
                        Main_Logger.Text = Date.Now.ToString & " [Command] " & Logger
                    Else
                        Main_Logger.AppendText(vbNewLine & Date.Now.ToString & " [Command] " + Logger)
                    End If
            End Select

            If Main_Logger.Lines.Length >= 1000 Then
                Main_Logger.Lines = (From line In Main_Logger.Lines Skip 1).ToArray
            End If

            If Not Directory.Exists(Setting.ApplicationDirectory & " \ Logger") Then
                Directory.CreateDirectory(Setting.ApplicationDirectory & "\Logger")
            End If

            File.AppendAllText(Setting.ApplicationDirectory & "\Logger\Logger_" & Setting.StartTime.Day.ToString & "-" & Setting.StartTime.Month.ToString & "-" & Setting.StartTime.Year.ToString & "_" & Setting.StartTime.Hour.ToString & "." & Setting.StartTime.Minute.ToString & "." & Setting.StartTime.Second.ToString & ".dat", Main_Logger.Lines(Main_Logger.Lines.GetUpperBound(0)) & vbNewLine)
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Sub UpdatePlayerList(ByVal Operation As Operation, ByVal p As Player)
        Try
            If InvokeRequired Then
                BeginInvoke(New UpdatePlayerListSafe(AddressOf UpdatePlayerList), Operation, p)
                Exit Sub
            End If

            Select Case Operation
                Case Operation.Add
                    If p.isGameJoltPlayer Then
                        Main_CurrentPlayerOnline.Items.Add("ID: " & p.PlayerID.ToString & " " & p.Name & " (" & p.GameJoltID.ToString & ")")
                    Else
                        Main_CurrentPlayerOnline.Items.Add("ID: " & p.PlayerID.ToString & " " & p.Name)
                    End If
                    If Setting.MaxPlayers > 0 Then
                        Main_PlayerOnline.Text = "Player Online (" & Player.Count.ToString & " / " & Setting.MaxPlayers.ToString & ")"
                    Else
                        Main_PlayerOnline.Text = "Player Online (" & Player.Count.ToString & ")"
                    End If
                Case Operation.Remove
                    If Player.IndexOf(p) >= 0 Then
                        Main_CurrentPlayerOnline.Items.RemoveAt(Player.IndexOf(p))
                        Player.RemoveAt(Player.IndexOf(p))
                    End If
                    If Setting.MaxPlayers > 0 Then
                        Main_PlayerOnline.Text = "Player Online (" & Player.Count.ToString & " / " & Setting.MaxPlayers.ToString & ")"
                    Else
                        Main_PlayerOnline.Text = "Player Online (" & Player.Count.ToString & ")"
                    End If
                Case Operation.Update
                    If p.isGameJoltPlayer Then
                        Main_CurrentPlayerOnline.Items.Item(Player.IndexOf(p)) = "ID: " & p.PlayerID.ToString & " " & p.Name & " (" & p.GameJoltID.ToString & ")" & p.GetPlayerBusyType
                    Else
                        Main_CurrentPlayerOnline.Items.Item(Player.IndexOf(p)) = "ID: " & p.PlayerID.ToString & " " & p.Name & p.GetPlayerBusyType
                    End If
            End Select
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Private Sub RestartServerTimer(ByVal sender As Object, ByVal e As EventArgs)
        If Setting.AutoRestartTime > 10 Then
            Dim TimeLeft As TimeSpan = (LastRestartTime.AddSeconds(CDbl(Setting.AutoRestartTime)) - Date.Now)
            If CInt(TimeLeft.TotalSeconds) = 300 Then
                ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Setting.Token("SERVER_RESTARTWARNING", "5 minutes"), Nothing))
                QueueMessage("This server is schedule for a restart in 5 minutes. For your personal safety, starting a new trade and PvP during this period is disabled. Save your game now to prevent data lost.", LogType.Info)
            ElseIf CInt(TimeLeft.TotalSeconds) = 60 Then
                ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Setting.Token("SERVER_RESTARTWARNING", "1 minute"), Nothing))
                QueueMessage("This server is schedule for a restart in 1 minute. For your personal safety, starting a new trade and PvP during this period is disabled. Save your game now to prevent data lost.", LogType.Info)
            ElseIf CInt(TimeLeft.TotalSeconds) <= 10 AndAlso CInt(TimeLeft.TotalSeconds) > 1 Then
                ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Setting.Token("SERVER_RESTARTWARNING", CInt(TimeLeft.TotalSeconds).ToString & " seconds"), Nothing))
                QueueMessage("This server is schedule for a restart in " & TimeLeft.Seconds.ToString & " seconds. For your personal safety, starting a new trade and PvP during this period is disabled. Save your game now to prevent data lost.", LogType.Info)
            ElseIf CInt(TimeLeft.TotalSeconds) = 1
                ServerClient.SendAllData(New Package(Package.PackageTypes.ChatMessage, Setting.Token("SERVER_RESTARTWARNING", "1 second"), Nothing))
                QueueMessage("This server is schedule for a restart in 1 second. For your personal safety, starting a new trade and PvP during this period is disabled. Save your game now to prevent data lost.", LogType.Info)
            End If

            If (LastRestartTime.AddSeconds(CDbl(Setting.AutoRestartTime)) - Date.Now).TotalSeconds <= 0 Then
                Application.Restart()
            End If
        End If
    End Sub

    Private Sub UpdateMenuBar(ByVal sender As Object, ByVal e As EventArgs)
        If ServerClient.ServerClientStatus = ServerClient.Status.Started Then
            StartToolStripMenuItem.Enabled = False
            StopToolStripMenuItem.Enabled = True
            ApplicationSettingsToolStripMenuItem.Enabled = False
        ElseIf ServerClient.ServerClientStatus = ServerClient.Status.Stopped
            StartToolStripMenuItem.Enabled = True
            StopToolStripMenuItem.Enabled = False
            ApplicationSettingsToolStripMenuItem.Enabled = True
        End If

        If StopGUIUpdate And Not Main_Logger.Focused Then
            StopGUIUpdate = False
        End If
    End Sub

    Public Function RestartServerTimeLeft() As String
        If Setting.AutoRestartTime > 10 Then
            Dim TimeLeft As TimeSpan = (LastRestartTime.AddSeconds(CDbl(Setting.AutoRestartTime)) - Date.Now)
            Dim TimeRemainingText As String = "This server is schedule for a restart in "

            If TimeLeft.TotalSeconds > 0 Then
                If TimeLeft.Days = 1 Then
                    TimeRemainingText &= TimeLeft.Days & " day "
                ElseIf TimeLeft.Days > 1
                    TimeRemainingText &= TimeLeft.Days & " days "
                End If

                If TimeLeft.Hours = 1 AndAlso TimeLeft.Days = 0 Then
                    TimeRemainingText &= TimeLeft.Hours & " hour "
                ElseIf TimeLeft.Hours > 1 AndAlso TimeLeft.Days = 0 Then
                    TimeRemainingText &= TimeLeft.Hours & " hours "
                End If

                If TimeLeft.Minutes = 1 AndAlso TimeLeft.Days = 0 Then
                    TimeRemainingText &= TimeLeft.Minutes & " minute "
                ElseIf TimeLeft.Minutes > 1 AndAlso TimeLeft.Days = 0 Then
                    TimeRemainingText &= TimeLeft.Minutes & " minutes "
                End If

                If TimeLeft.Seconds = 1 AndAlso TimeLeft.Days = 0 Then
                    TimeRemainingText &= TimeLeft.Seconds & " second "
                ElseIf TimeLeft.Seconds > 1 AndAlso TimeLeft.Days = 0 Then
                    TimeRemainingText &= TimeLeft.Seconds & " seconds "
                End If

                If TimeLeft.TotalSeconds <= 300 Then
                    TimeRemainingText = TimeRemainingText.Remove(TimeRemainingText.Length - 1)
                    TimeRemainingText &= ". For your personal safety, starting a new trade and PvP during this period is disabled."
                End If

                Return TimeRemainingText
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function

    Private Sub Main_Command_TextChanged(sender As Object, e As KeyEventArgs) Handles Main_Command.KeyDown
        If e.KeyData = Keys.Enter Then
            PackageHandler.HandleChatCommand(New Package(Package.PackageTypes.ChatMessage, Main_Command.Text, Nothing))
            Main_Logger.SelectionStart = Main_Logger.TextLength
            Main_Command.Clear()
        End If
    End Sub

#Region "Menu Bar"
#Region "Server"
    Private Sub StartToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartToolStripMenuItem.Click
        LastRestartTime = Date.Now

        ' Restart Timer
        If Setting.AutoRestartTime > 0 AndAlso Setting.AutoRestartTime < 10 Then
            QueueMessage("Main.vb: The server client will not auto restart to prevent infinite restart loop. Please increase the timer to be at least 10 seconds.", LogType.Info)
        ElseIf Setting.AutoRestartTime > 0
            QueueMessage("Main.vb: Server is schedule to restart every " & Setting.AutoRestartTime.ToString & " seconds.", LogType.Info)
            TimerCollection(0).Start()
        End If

        ' Resume ServerClient
        ServerClient.Resume(Setting.Port)
    End Sub

    Private Sub StopToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StopToolStripMenuItem.Click
        If Player.Count > 0 Then
            QueueMessage("Main.vb: Removing all players.", LogType.Info)
            ServerClient.SendAllData(New Package(Package.PackageTypes.ServerClose, Setting.Token("SERVER_CLOSE"), Nothing))
        End If

        Setting.Save()

        ' Abort Restart Timer
        TimerCollection(0).Stop()

        ' Abort ServerClient
        ServerClient.Stop()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Application.Exit()
    End Sub
#End Region

#Region "Settings"
    Private Sub ApplicationSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ApplicationSettingsToolStripMenuItem.Click
        ApplicationSetting.Show()
    End Sub
#End Region

#Region "About"
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        AboutBox.Show()
    End Sub
#End Region
#End Region

#Region "Main_Logger"
    Private Sub Main_Logger_VScroll(sender As Object, e As EventArgs) Handles Main_Logger.VScroll
        StopGUIUpdate = True
    End Sub

    Private Sub Main_Logger_SelectionChanged(sender As Object, e As EventArgs) Handles Main_Logger.SelectionChanged
        StopGUIUpdate = True
    End Sub

    Private Sub Main_Logger_TextChanged(sender As Object, e As EventArgs) Handles Main_Logger.TextChanged
        If Not StopGUIUpdate Then
            Main_Logger.SelectionStart = Main_Logger.TextLength
            Main_Logger.ScrollToCaret()
        End If
    End Sub
#End Region

#Region "Main_LoggerRC"
    Private Sub CopyToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyToolStripMenuItem.Click
        Main_Logger.Copy()
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SelectAllToolStripMenuItem.Click
        Main_Logger.SelectAll()
    End Sub

    Private Sub CopyAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyAllToolStripMenuItem.Click
        Main_Logger.SelectAll()
        Main_Logger.Copy()
    End Sub
#End Region

    Private Sub Main_CurrentPlayerOnlineRC_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Main_CurrentPlayerOnlineRC.Opening
        If Main_CurrentPlayerOnline.Items.Count >= 0 Then
            e.Cancel = True
        End If
    End Sub
#Else
    Private Sub Main_CurrentPlayerOnline_SelectedIndexChanged(sender As Object, e As MouseEventArgs) Handles Main_CurrentPlayerOnline.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If Main_CurrentPlayerOnline.IndexFromPoint(e.X, e.Y) < 0 Then
                Main_CurrentPlayerOnline.SelectedIndex = Main_CurrentPlayerOnline.Items.Count - 1
            Else
                Main_CurrentPlayerOnline.SelectedIndex = Main_CurrentPlayerOnline.IndexFromPoint(e.X, e.Y)
            End If
        End If
    End Sub

#Region "Main_CurrentPlayerOnlineRC"
    Private Sub Main_CurrentPlayerOnlineRC_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles Main_CurrentPlayerOnlineRC.Opening
        If Main_CurrentPlayerOnline.Items.Count = 0 Then
            e.Cancel = True
        End If
    End Sub

    ' Ban
    Private Sub MinuteToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MinuteToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Text = "60"
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    Private Sub HourToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HourToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Text = "3600"
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    Private Sub DayToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles DayToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Text = "86400"
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    Private Sub WeekToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles WeekToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Text = "604800"
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    Private Sub MonthToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles MonthToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Text = "2419200"
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    Private Sub PermanentToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles PermanentToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Text = Integer.MaxValue.ToString
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    Private Sub CustomToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CustomToolStripMenuItem1.Click
        Ban.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Ban.DurationText.Clear()
        Ban.ReasonText.Clear()
        Ban.Show()
    End Sub

    ' IP Ban
    Private Sub MinuteToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles MinuteToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Text = "60"
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    Private Sub HourToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles HourToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Text = "3600"
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    Private Sub DayToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles DayToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Text = "86400"
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    Private Sub WeekToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles WeekToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Text = "604800"
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    Private Sub MonthToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles MonthToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Text = "2419200"
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    Private Sub PermanentToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles PermanentToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Text = Integer.MaxValue.ToString
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    Private Sub CustomToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles CustomToolStripMenuItem2.Click
        IPBan.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        IPBan.DurationText.Clear()
        IPBan.ReasonText.Clear()
        IPBan.Show()
    End Sub

    ' Find
    Private Sub FindToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles FindToolStripMenuItem.Click
        OldPackage.HandleChatMessageCommand("/find " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value, Nothing, True)
    End Sub

    ' Kick
    Private Sub KickToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles KickToolStripMenuItem.Click
        Kick.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Kick.Reason.Clear()
        Kick.Show()
    End Sub

    ' Mute
    Private Sub MinuteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MinuteToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Text = "60"
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub HourToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HourToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Text = "3600"
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub DayToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DayToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Text = "86400"
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub WeekToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles WeekToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Text = "604800"
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub MonthToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MonthToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Text = "2419200"
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub PermanentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PermanentToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Text = Integer.MaxValue.ToString
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub CustomToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CustomToolStripMenuItem.Click
        Mute.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        Mute.DurationText.Clear()
        Mute.ReasonText.Clear()
        Mute.Show()
    End Sub

    Private Sub CheckToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CheckToolStripMenuItem1.Click
        OldPackage.HandleChatMessageCommand("/checkmute " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value, Nothing, True)
    End Sub

    Private Sub RemoveToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RemoveToolStripMenuItem1.Click
        If OldPlayer.isGameJoltPlayer(OldPlayer.Name.IndexOf(Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value)) Then
            OldPackage.HandleChatMessageCommand("/unmute " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " True", Nothing, True)
        Else
            OldPackage.HandleChatMessageCommand("/unmute " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " False", Nothing, True)
        End If
    End Sub

    ' OP
    Private Sub ChatModeratorToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles ChatModeratorToolStripMenuItem.Click
        OldPackage.HandleChatMessageCommand("/op " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " 1", Nothing, True)
    End Sub

    Private Sub ServerModeratorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ServerModeratorToolStripMenuItem.Click
        OldPackage.HandleChatMessageCommand("/op " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " 2", Nothing, True)
    End Sub

    Private Sub GlobalModeratorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GlobalModeratorToolStripMenuItem.Click
        OldPackage.HandleChatMessageCommand("/op " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " 3", Nothing, True)
    End Sub

    Private Sub AdministratorToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AdministratorToolStripMenuItem.Click
        OldPackage.HandleChatMessageCommand("/op " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " 4", Nothing, True)
    End Sub

    Private Sub RemoveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RemoveToolStripMenuItem.Click
        If OldPlayer.isGameJoltPlayer(OldPlayer.Name.IndexOf(Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value)) Then
            OldPackage.HandleChatMessageCommand("/deop " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " True", Nothing, True)
        Else
            OldPackage.HandleChatMessageCommand("/deop " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value + " False", Nothing, True)
        End If
    End Sub

    Private Sub CheckToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CheckToolStripMenuItem.Click
        OldPackage.HandleChatMessageCommand("/checkop " + Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value, Nothing, True)
    End Sub

    ' PM
    Private Sub PMToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PMToolStripMenuItem.Click
        PM.PlayerName = Regex.Match(Main_CurrentPlayerOnline.SelectedItem.ToString, "(\w+)").Groups(1).Value
        PM.Reason.Clear()
        PM.OK_Button.Enabled = False
        PM.Show()
    End Sub

#End Region
#End If

End Class