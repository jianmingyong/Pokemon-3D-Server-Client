Public Class ApplicationSetting

    Public ListOfSetting As New List(Of NewSetting)

    Public Sub LoadSetting()
        AddNewSetting("CheckForUpdate", Main.Setting.CheckForUpdate.ToString, "To allow application to check for update upon launch.", "Syntax: Boolean: true, false.")
        AddNewSetting("GeneratePublicIP", Main.Setting.GeneratePublicIP.ToString, "To allow application to update IP address upon launch.", "Syntax: Boolean: true, false.")
        AddNewSetting("IPAddress", Main.Setting.IPAddress, "Public IP address of your server.", "Syntax: Valid IPv4 address.")
        AddNewSetting("Port", Main.Setting.Port.ToString, "The port to use on your server.", "Syntax: Integer: Between 0 to 65535 inclusive.")
        AddNewSetting("ServerName", Main.Setting.ServerName, "The server name to display to public.", "Syntax: String.")
        AddNewSetting("ServerMessage", Main.Setting.ServerMessage, "The server message to display when a player select a server.", "Syntax: String.")
        AddNewSetting("WelcomeMessage", Main.Setting.WelcomeMessage, "The server message to display when a player joins a server.", "Syntax: String.")
        AddNewSetting("GameMode", Main.Setting.GameMode, "The GameMode that player should play in order to join the server.", "You may allow multiple gamemode to join the game by adding a comma for each gamemode.", "Syntax: String.")
        AddNewSetting("MaxPlayers", Main.Setting.MaxPlayers.ToString, "The maximum amount of player in the server that can join.", "Syntax: Integer: -1: Unlimited.")
        AddNewSetting("OfflineMode", Main.Setting.OfflineMode.ToString, "The ability for offline profile player to join the server.", "Syntax: Boolean: true, false.")

        AddNewSetting("Season", Main.Setting.Season.ToString, "To set server default season.", "Syntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3")
        AddNewSetting("Weather", Main.Setting.Weather.ToString, "To set server default weather.", "Syntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3")
        AddNewSetting("DoDayCycle", Main.Setting.DoDayCycle.ToString, "To allow the server to update day cycle.", "Syntax: Boolean: true, false.")
        AddNewSetting("SeasonMonth", Main.Setting.SeasonMonth.SeasonData, "To set the season based on local date. Must set Season = -3", "Syntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2", "You may insert more than one season by separating it with a comma.")
        AddNewSetting("WeatherSeason", Main.Setting.WeatherSeason.WeatherData, "To set the weather based on server season. Must set Weather = -3", "Syntax: See Weather ^", "You may insert more than one weather by separating it with a comma.")

        AddNewSetting("NoPingKickTime", Main.Setting.NoPingKickTime.ToString, "To kick player out if there are no valid ping for n amount of seconds.", "Syntax: Integer: -1 to disable it.")
        AddNewSetting("AFKKickTime", Main.Setting.AFKKickTime.ToString, "To kick player out if there are no valid activity for n amount of seconds.", "Syntax: Integer: -1 to disable it.")
        AddNewSetting("AutoRestartTime", Main.Setting.AutoRestartTime.ToString, "To automatically restart the server after n seconds. Disable PvP and trade features for the last 5 minutes of the countdown.", "Syntax: Integer: -1 to disable it.")

        AddNewSetting("BlackList", Main.Setting.BlackList.ToString, "To allow using Blacklist feature.", "Syntax: Boolean: true, false.")
        AddNewSetting("IPBlackList", Main.Setting.IPBlackList.ToString, "To allow using IPBlackList feature.", "Syntax: Boolean: true, false.")
        AddNewSetting("WhiteList", Main.Setting.WhiteList.ToString, "To allow using WhiteList feature.", "Syntax: Boolean: true, false.")
        AddNewSetting("OperatorList", Main.Setting.OperatorList.ToString, "To allow using Operator feature.", "Syntax: Boolean: true, false.")
        AddNewSetting("MuteList", Main.Setting.MuteList.ToString, "To allow using Mute feature.", "Syntax: Boolean: true, false.")
        AddNewSetting("OnlineSettingList", Main.Setting.OnlineSettingList.ToString, "To allow using Online Setting feature.", "Syntax: Boolean: true, false.")
        AddNewSetting("SwearInfractionList", Main.Setting.SwearInfractionList.ToString, "To allow using Swear Infraction feature.", "Syntax: Boolean: true, false.")

        AddNewSetting("SwearInfractionCap", Main.Setting.SwearInfractionCap.ToString, "Amount of infraction points before the first mute.", "Syntax: Integer: -1 to disable.")
        AddNewSetting("SwearInfractionReset", Main.Setting.SwearInfractionReset.ToString, "Amount of days before it expire the infraction count.", "Syntax: Integer: -1 to disable.")

        AddNewSetting("SpamResetDuration", Main.Setting.SpamResetDuration.ToString, "Amount of seconds for the user to send the same word again.", "Syntax: Integer: -1 to disable.")

        AddNewSetting("LoggerInfo", Main.Setting.LoggerInfo.ToString, "To log server information.", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerWarning", Main.Setting.LoggerWarning.ToString, "To log server warning including ex exception.", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerDebug", Main.Setting.LoggerDebug.ToString, "To log server package data (Lag might happen if turn on).", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerChat", Main.Setting.LoggerChat.ToString, "To log server chat message.", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerPM", Main.Setting.LoggerPM.ToString, "To log server private chat message. (Actual Private Message content is not logged)", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerServer", Main.Setting.LoggerServer.ToString, "To log server message.", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerTrade", Main.Setting.LoggerTrade.ToString, "To log trade request. (Actual Trade Request content is not logged)", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerPvP", Main.Setting.LoggerPvP.ToString, "To log pvp request. (Actual PvP Request content is not logged)", "Syntax: Boolean: true, false.")
        AddNewSetting("LoggerCommand", Main.Setting.LoggerCommand.ToString, "To log server command usage. (Debug Commands are not logged)", "Syntax: Boolean: true, false.")

        ObjectListView1.AddObjects(ListOfSetting)
    End Sub

    Private Sub AddNewSetting(ByVal [Property] As String, ByVal Value As String, ParamArray ByVal Description() As String)
        Dim Descriptions As String = Nothing

        For Each line As String In Description
            Descriptions &= line & vbNewLine
        Next

        ListOfSetting.Add(New NewSetting([Property], Value, Descriptions))
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        For i As Integer = 0 To ListOfSetting.Count - 1
            Select Case CType(ObjectListView1.GetModelObject(i), NewSetting).Setting
                Case "CheckForUpdate"
                    Try
                        Main.Setting.CheckForUpdate = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "GeneratePublicIP"
                    Try
                        Main.Setting.GeneratePublicIP = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "IPAddress"
                    Try
                        Main.Setting.IPAddress = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "Port"
                    Try
                        Main.Setting.Port = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "ServerName"
                    Try
                        Main.Setting.ServerName = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "ServerMessage"
                    Try
                        Main.Setting.ServerMessage = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "WelcomeMessage"
                    Try
                        Main.Setting.WelcomeMessage = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "GameMode"
                    Try
                        Main.Setting.GameMode = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "MaxPlayers"
                    Try
                        Main.Setting.MaxPlayers = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "OfflineMode"
                    Try
                        Main.Setting.OfflineMode = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "Season"
                    Try
                        Main.Setting.Season = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "Weather"
                    Try
                        Main.Setting.Weather = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "DoDayCycle"
                    Try
                        Main.Setting.DoDayCycle = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "SeasonMonth"
                    Try
                        Main.Setting.SeasonMonth.SeasonData = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "WeatherSeason"
                    Try
                        Main.Setting.WeatherSeason.WeatherData = CType(ObjectListView1.GetModelObject(i), NewSetting).Value
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "NoPingKickTime"
                    Try
                        Main.Setting.NoPingKickTime = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "AFKKickTime"
                    Try
                        Main.Setting.AFKKickTime = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "AutoRestartTime"
                    Try
                        Main.Setting.AutoRestartTime = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "BlackList"
                    Try
                        Main.Setting.BlackList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "IPBlackList"
                    Try
                        Main.Setting.IPBlackList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "WhiteList"
                    Try
                        Main.Setting.WhiteList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "OperatorList"
                    Try
                        Main.Setting.OperatorList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "MuteList"
                    Try
                        Main.Setting.MuteList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "OnlineSettingList"
                    Try
                        Main.Setting.OnlineSettingList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "SwearInfractionList"
                    Try
                        Main.Setting.SwearInfractionList = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "SwearInfractionCap"
                    Try
                        Main.Setting.SwearInfractionCap = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "SwearInfractionReset"
                    Try
                        Main.Setting.SwearInfractionReset = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "SpamResetDuration"
                    Try
                        Main.Setting.SpamResetDuration = CInt(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerInfo"
                    Try
                        Main.Setting.LoggerInfo = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerWarning"
                    Try
                        Main.Setting.LoggerWarning = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerDebug"
                    Try
                        Main.Setting.LoggerDebug = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerChat"
                    Try
                        Main.Setting.LoggerChat = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerPM"
                    Try
                        Main.Setting.LoggerPM = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerServer"
                    Try
                        Main.Setting.LoggerServer = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerTrade"
                    Try
                        Main.Setting.LoggerTrade = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerPvP"
                    Try
                        Main.Setting.LoggerPvP = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
                Case "LoggerCommand"
                    Try
                        Main.Setting.LoggerCommand = CBool(CType(ObjectListView1.GetModelObject(i), NewSetting).Value)
                    Catch ex As Exception
                        ex.CatchError
                    End Try
            End Select
        Next
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        Close()
    End Sub

    Private Sub ApplicationSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadSetting()
    End Sub
End Class

Public Class NewSetting
    Public Property Setting As String
    Public Property Value As String
    Public Property Remark As String

    Public Sub New(ByVal Setting As String, ByVal Value As String, ByVal Remark As String)
        Me.Setting = Setting
        Me.Value = Value
        Me.Remark = Remark
    End Sub
End Class