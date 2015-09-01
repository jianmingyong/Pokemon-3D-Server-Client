Imports System.IO
Imports System.Net

Public Class Settings

    '<!-- Main -->
    Public Shared ApplicationDirectory As String

    '<!-- Pokémon 3D Server Client Setting File -->
    Public Shared StartTime As String
    Public Shared ApplicationVersion As String
    Public Shared ProtocalVersion As String

    Public Shared ApplicationCheckForUpdate As String
    Public Shared GeneratePublicIP As String
    Public Shared Debugger As String

    '<!-- Main Server Property -->
    Public Shared IPAddress As String
    Public Shared Port As String

    Public Shared ServerName As String
    Public Shared ServerMessage As String
    Public Shared WelcomeMessage As String
    Public Shared GameMode As String
    Public Shared MaxPlayers As String
    Public Shared OfflineMode As String

    '<!-- Advanced Server Property -->
    Public Shared BlackList As String
    Public Shared WhiteList As String
    Public Shared AllowOP As String

    Public Shared Weather As String
    Public Shared Season As String
    Public Shared DoDayCycle As String

    Public Shared NoPingKickTime As String
    Public Shared AFKKickTime As String
    Public Shared AutoRestartTime As String

    '<!-- Advance World Property -->
    Public Shared SeasonMonth As String
    Public Shared WeatherSeason As String

    '<!-- Server List -->
    Public Shared BlackListItem As New List(Of String)
    Public Shared IPBlackListItem As New List(Of String)
    Public Shared MuteListItem As New List(Of String)
    Public Shared WhiteListItem As New List(Of String)
    Public Shared OPListItem As New List(Of String)

    Public Enum CheckMethod
        CheckMethod_Directory
        CheckMethod_Boolean
        CheckMethod_String
        CheckMethod_Integer
        CheckMethod_IP
        CheckMethod_Port
    End Enum

    Public Sub New()
        '<!-- Main -->
        ApplicationDirectory = Application.StartupPath

        '<!-- Pokémon 3D Server Client Setting File -->
        StartTime = DateTime.Now.ToString
        ApplicationVersion = My.Application.Info.Version.ToString
        ProtocalVersion = "0.5"

        ApplicationCheckForUpdate = "True"
        GeneratePublicIP = "True"
        Debugger = "False"

        '<!-- Main Server Property -->
        IPAddress = "Nothing"
        Port = "15124"

        ServerName = "Pokémon 3D Server"
        ServerMessage = "Nothing"
        WelcomeMessage = "Nothing"
        GameMode = "Pokemon 3D"
        MaxPlayers = "20"
        OfflineMode = "True"

        '<!-- Advanced Server Property -->
        BlackList = "True"
        WhiteList = "False"
        AllowOP = "True"

        NoPingKickTime = "60"
        AFKKickTime = "300"
        AutoRestartTime = "0"

        Weather = "-2"
        Season = "-2"
        DoDayCycle = "True"

        SeasonMonth = Nothing
        WeatherSeason = Nothing
        
    End Sub

    Public Sub Generate(ByVal Argument As String)
        If String.Equals(Argument, "ServerName", StringComparison.OrdinalIgnoreCase) Then
            ServerName = "Pokémon 3D Server"
        ElseIf String.Equals(Argument, "IPAddress", StringComparison.OrdinalIgnoreCase) Then
            IPAddress = Functions.GetPublicIp
        ElseIf String.Equals(Argument, "Port", StringComparison.OrdinalIgnoreCase) Then
            Port = "15124"
        ElseIf String.Equals(Argument, "MaxPlayers", StringComparison.OrdinalIgnoreCase) Then
            MaxPlayers = "20"
        ElseIf String.Equals(Argument, "BlackList", StringComparison.OrdinalIgnoreCase) Then
            BlackList = "True"
        ElseIf String.Equals(Argument, "WhiteList", StringComparison.OrdinalIgnoreCase) Then
            WhiteList = "False"
        ElseIf String.Equals(Argument, "OfflineMode", StringComparison.OrdinalIgnoreCase) Then
            OfflineMode = "False"
        ElseIf String.Equals(Argument, "ServerMessage", StringComparison.OrdinalIgnoreCase) Then
            ServerMessage = "Nothing"
        ElseIf String.Equals(Argument, "Weather", StringComparison.OrdinalIgnoreCase) Then
            Weather = "-2"
        ElseIf String.Equals(Argument, "Season", StringComparison.OrdinalIgnoreCase) Then
            Season = "-2"
        ElseIf String.Equals(Argument, "DoDayCycle", StringComparison.OrdinalIgnoreCase) Then
            DoDayCycle = "True"
        ElseIf String.Equals(Argument, "GameMode", StringComparison.OrdinalIgnoreCase) Then
            GameMode = "Pokemon 3D"
        ElseIf String.Equals(Argument, "NoPingKickTime", StringComparison.OrdinalIgnoreCase) Then
            NoPingKickTime = "60"
        ElseIf String.Equals(Argument, "AFKKickTime", StringComparison.OrdinalIgnoreCase) Then
            AFKKickTime = "300"
        ElseIf String.Equals(Argument, "WelcomeMessage", StringComparison.OrdinalIgnoreCase) Then
            WelcomeMessage = "Nothing"
        ElseIf String.Equals(Argument, "AllowOP", StringComparison.OrdinalIgnoreCase) Then
            AllowOP = "True"
        ElseIf String.Equals(Argument, "ApplicationCheckForUpdate", StringComparison.OrdinalIgnoreCase) Then
            ApplicationCheckForUpdate = "True"
        ElseIf String.Equals(Argument, "GeneratePublicIP", StringComparison.OrdinalIgnoreCase) Then
            GeneratePublicIP = "True"
        ElseIf String.Equals(Argument, "Debugger", StringComparison.OrdinalIgnoreCase) Then
            Debugger = "False"
        ElseIf String.Equals(Argument, "SeasonMonth", StringComparison.OrdinalIgnoreCase) Then
            SeasonMonth = Nothing
        ElseIf String.Equals(Argument, "WeatherSeason", StringComparison.OrdinalIgnoreCase) Then
            WeatherSeason = Nothing
        ElseIf String.Equals(Argument, "AutoRestartTime", StringComparison.OrdinalIgnoreCase) Then
            AutoRestartTime = "0"
        End If
    End Sub

    Public Sub Load()
        Try
            If HaveSettingFile() Then
                Dim Flag As Boolean = False
                Dim Flag2 As Boolean = False
                Dim FlagData As String = Nothing
                For Each Lines As String In File.ReadAllLines(ApplicationDirectory + "\application_settings.dat")
                    If Lines.Contains("<!--") Or Lines.Contains("/*") Then
                        Flag = True
                    End If
                    If Flag = True Then
                        If Lines.Contains("-->") Or Lines.Contains("*/") Then
                            Flag = False
                        End If
                    End If
                    If Not Flag And Not Lines.Contains("-->") Then
                        If Lines.Contains("ServerName|") Then
                            ServerName = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("IPAddress|") Then
                            IPAddress = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("Port|") Then
                            Port = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("MaxPlayers|") Then
                            MaxPlayers = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("BlackList|") Then
                            BlackList = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("WhiteList|") Then
                            WhiteList = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("OfflineMode|") Then
                            OfflineMode = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("ServerMessage|") Then
                            ServerMessage = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("Weather|") Then
                            Weather = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("Season|") And Not Lines.Contains("WeatherSeason|") Then
                            Season = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("DoDayCycle|") Then
                            DoDayCycle = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("GameMode|") Then
                            GameMode = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("NoPingKickTime|") Then
                            NoPingKickTime = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("AFKKickTime|") Then
                            AFKKickTime = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("AutoRestartTime|") Then
                            AutoRestartTime = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("WelcomeMessage|") Then
                            WelcomeMessage = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("AllowOP|") Then
                            AllowOP = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("ApplicationCheckForUpdate|") Then
                            ApplicationCheckForUpdate = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("GeneratePublicIP|") Then
                            GeneratePublicIP = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("Debugger|") Then
                            Debugger = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("SeasonMonth|") Then
                            SeasonMonth = Lines.Remove(0, 12)
                        ElseIf Lines.Contains("WeatherSeason|") Then
                            WeatherSeason = Lines.Remove(0, 14)
                        End If

                        If Lines.Contains("BlackListData|") Then
                            Flag2 = True
                            FlagData = "BlackListData"
                        End If
                        If Lines.Contains("IPBlackListData|") Then
                            Flag2 = True
                            FlagData = "IPBlackListData"
                        End If
                        If Lines.Contains("MuteListData|") Then
                            Flag2 = True
                            FlagData = "MuteListData"
                        End If
                        If Lines.Contains("WhiteListData|") Then
                            Flag2 = True
                            FlagData = "WhiteListData"
                        End If
                        If Lines.Contains("OPListData|") Then
                            Flag2 = True
                            FlagData = "OPListData"
                        End If

                        If Flag2 Then
                            If String.Equals(FlagData, "BackListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("BackListData|") Then
                                If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                    BlackListItem.Add(Lines)
                                End If
                            ElseIf String.Equals(FlagData, "IPBackListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("IPBackListData|") Then
                                If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                    IPBlackListItem.Add(Lines)
                                End If
                            ElseIf String.Equals(FlagData, "MuteListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("MuteListData|") Then
                                If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                    MuteListItem.Add(Lines)
                                End If
                            ElseIf String.Equals(FlagData, "WhiteListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("WhiteListData|") Then
                                If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                    WhiteListItem.Add(Lines)
                                End If
                            ElseIf String.Equals(FlagData, "OPListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("OPListData|") Then
                                If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                    OPListItem.Add(Lines)
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            CheckAll()
        Catch ex As Exception
            Functions.CatchError(ex)
        End Try
    End Sub

    Public Shared Sub Save()
        Try
            Dim Setting As New List(Of String)
            Setting.Add("<!-- Pokémon 3D Server Client Setting File -->")
            Setting.Add("StartTime|" + StartTime)
            Setting.Add("ApplicationVersion|" + ApplicationVersion)
            Setting.Add("ProtocalVersion|" + ProtocalVersion)
            Setting.Add("")
            Setting.Add("ApplicationCheckForUpdate|" + ApplicationCheckForUpdate)
            Setting.Add("GeneratePublicIP|" + GeneratePublicIP)
            Setting.Add("Debugger|" + Debugger)
            Setting.Add("")
            Setting.Add("<!-- Main Server Property -->")
            Setting.Add("IPAddress|" + IPAddress)
            Setting.Add("Port|" + Port)
            Setting.Add("")
            Setting.Add("ServerName|" + ServerName)
            Setting.Add("ServerMessage|" + ServerMessage)
            Setting.Add("WelcomeMessage|" + WelcomeMessage)
            Setting.Add("GameMode|" + GameMode)
            Setting.Add("MaxPlayers|" + MaxPlayers)
            Setting.Add("OfflineMode|" + OfflineMode)
            Setting.Add("")
            Setting.Add("<!-- Advanced Server Property -->")
            Setting.Add("BlackList|" + BlackList)
            Setting.Add("WhiteList|" + WhiteList)
            Setting.Add("AllowOP|" + AllowOP)
            Setting.Add("")
            Setting.Add("NoPingKickTime|" + NoPingKickTime)
            Setting.Add("AFKKickTime|" + AFKKickTime)
            Setting.Add("AutoRestartTime|" + AutoRestartTime)
            Setting.Add("")
            Setting.Add("<!-- World Property -->")
            Setting.Add("<!-- Weather: Set the global weather. -->")
            Setting.Add("<!-- Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 -->")
            Setting.Add("Weather|" + Weather)
            Setting.Add("")
            Setting.Add("<!-- Season: Set the global season. -->")
            Setting.Add("<!-- Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 -->")
            Setting.Add("Season|" + Season)
            Setting.Add("")
            Setting.Add("<!-- DoDayCycle: To simulate world time. -->")
            Setting.Add("<!-- True = Server time will update | False = Server time will not update -->")
            Setting.Add("DoDayCycle|" + DoDayCycle)
            Setting.Add("")
            Setting.Add("<!-- Advanced World Property -->")
            Setting.Add("<!-- SeasonMonth: Set the season based on real world date (Overrides global season)-->")
            Setting.Add("<!-- Syntax (Index of season on each month separated by ""|""): Jan | Feb | March | April | May | June | July | Aug | Sep | Oct | Nov | Dec -->")
            Setting.Add("<!-- Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 -->")
            Setting.Add("<!-- Each Month cannot have an empty season or the program might fail to process it. -->")
            Setting.Add("SeasonMonth|" + SeasonMonth)
            Setting.Add("")
            Setting.Add("<!-- WeatherSeason: Set what weather will it be for each season. (Overrides global weather)-->")
            Setting.Add("<!-- Syntax (Index of weather on each season separated by ""|""): Winter | Spring | Summer | Fall -->")
            Setting.Add("<!-- Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 -->")
            Setting.Add("WeatherSeason|" + WeatherSeason)
            Setting.Add("")
            Setting.Add("<!-- BlackList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | Ban Reason | Ban Start Time | Ban Time -->")
            Setting.Add("BlackListData|")
            Setting.AddRange(BlackListItem)
            Setting.Add("")
            Setting.Add("<!-- IP BlackList Data -->")
            Setting.Add("<!-- IP | Ban Reason | Ban Start Time | Ban Time -->")
            Setting.Add("IPBlackListData|")
            Setting.AddRange(IPBlackListItem)
            Setting.Add("")
            Setting.Add("<!-- MuteList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time -->")
            Setting.Add("MuteListData|")
            Setting.AddRange(MuteListItem)
            Setting.Add("")
            Setting.Add("<!-- WhiteList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | WhiteList Reason -->")
            Setting.Add("WhiteListData|")
            Setting.AddRange(WhiteListItem)
            Setting.Add("")
            Setting.Add("<!-- OPList Data -->")
            Setting.Add("<!-- Name | GameJolt ID | OP Reason | OP Level -->")
            Setting.Add("OPListData|")
            Setting.AddRange(OPListItem)

            File.WriteAllLines(Settings.ApplicationDirectory + "\application_settings.dat", Setting)
        Catch ex As Exception
            Functions.CatchError(ex)
        End Try
    End Sub

    Public Function Check(ByVal Settings As String, ByVal CheckMethod As CheckMethod) As Boolean
        If HaveValidSettings(Settings, CheckMethod) = False Then
            Functions.ReturnMessage("Setting File (" + Settings + "): """ + Value(Settings) + """ detected invalid.", MsgBoxStyle.Information, "Settings")
            Generate(Settings)
            Return False
        End If
        Return True
    End Function

    Public Function CheckAll() As Boolean
        Dim ErrorMessage As New List(Of String)
        If HaveValidSettings(ServerName, CheckMethod.CheckMethod_String) = False Then
            ErrorMessage.Add("Setting File (ServerName): """ + ServerName + """ detected invalid.")
            Generate("ServerName")
        ElseIf HaveValidSettings(IPAddress, CheckMethod.CheckMethod_IP) = False Then
            ErrorMessage.Add("Setting File (IPAddress): """ + IPAddress + """ detected invalid.")
            Generate("IPAddress")
        ElseIf HaveValidSettings(Port, CheckMethod.CheckMethod_Port) = False Then
            ErrorMessage.Add("Setting File (Port): """ + Port + """ detected invalid.")
            Generate("Port")
        ElseIf HaveValidSettings(MaxPlayers, CheckMethod.CheckMethod_Integer) = False Then
            ErrorMessage.Add("Setting File (MaxPlayers): """ + MaxPlayers + """ detected invalid.")
            Generate("MaxPlayers")
        ElseIf HaveValidSettings(BlackList, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (BlackList): """ + BlackList + """ detected invalid.")
            Generate("BlackList")
        ElseIf HaveValidSettings(WhiteList, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (WhiteList): """ + WhiteList + """ detected invalid.")
            Generate("WhiteList")
        ElseIf HaveValidSettings(OfflineMode, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (OfflineMode): """ + OfflineMode + """ detected invalid.")
            Generate("OfflineMode")
        ElseIf HaveValidSettings(ServerMessage, CheckMethod.CheckMethod_String) = False Then
            ErrorMessage.Add("Setting File (ServerMessage): """ + ServerMessage + """ detected invalid.")
            Generate("ServerMessage")
        ElseIf HaveValidSettings(Weather, CheckMethod.CheckMethod_Integer) = False Then
            ErrorMessage.Add("Setting File (Weather): """ + Weather + """ detected invalid.")
            Generate("Weather")
        ElseIf HaveValidSettings(Season, CheckMethod.CheckMethod_Integer) = False Then
            ErrorMessage.Add("Setting File (Season): """ + Season + """ detected invalid.")
            Generate("Season")
        ElseIf HaveValidSettings(DoDayCycle, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (DoDayCycle): """ + DoDayCycle + """ detected invalid.")
            Generate("DoDayCycle")
        ElseIf HaveValidSettings(DoDayCycle, CheckMethod.CheckMethod_String) = False Then
            ErrorMessage.Add("Setting File (GameMode): """ + GameMode + """ detected invalid.")
            Generate("GameMode")
        ElseIf HaveValidSettings(NoPingKickTime, CheckMethod.CheckMethod_Integer) = False Then
            ErrorMessage.Add("Setting File (NoPingKickTime): """ + NoPingKickTime + """ detected invalid.")
            Generate("NoPingKickTime")
        ElseIf HaveValidSettings(AFKKickTime, CheckMethod.CheckMethod_Integer) = False Then
            ErrorMessage.Add("Setting File (AFKKickTime): """ + AFKKickTime + """ detected invalid.")
            Generate("AFKKickTime")
        ElseIf HaveValidSettings(WelcomeMessage, CheckMethod.CheckMethod_String) = False Then
            ErrorMessage.Add("Setting File (WelcomeMessage): """ + WelcomeMessage + """ detected invalid.")
            Generate("WelcomeMessage")
        ElseIf HaveValidSettings(AllowOP, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (AllowOP): """ + AllowOP + """ detected invalid.")
            Generate("AllowOP")
        ElseIf HaveValidSettings(ApplicationCheckForUpdate, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (ApplicationCheckForUpdate): """ + ApplicationCheckForUpdate + """ detected invalid.")
            Generate("ApplicationCheckForUpdate")
        ElseIf HaveValidSettings(GeneratePublicIP, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (GeneratePublicIP): """ + GeneratePublicIP + """ detected invalid.")
            Generate("GeneratePublicIP")
        ElseIf HaveValidSettings(Debugger, CheckMethod.CheckMethod_Boolean) = False Then
            ErrorMessage.Add("Setting File (Debugger): """ + Debugger + """ detected invalid.")
            Generate("Debugger")
        ElseIf HaveValidSettings(AutoRestartTime, CheckMethod.CheckMethod_Integer) = False Then
            ErrorMessage.Add("Setting File (AutoRestartTime): """ + AutoRestartTime + """ detected invalid.")
            Generate("AutoRestartTime")
        End If

        For Each Data As String In BlackListItem
            ' Name | GameJolt ID | Ban Reason | Ban Start Time | Ban Time
            If CType(Functions.GetSplit(Data, 3, "|"), DateTime).AddSeconds(CDbl(Functions.GetSplit(Data, 4, "|"))) <= DateTime.Now Then
                BlackListItem.Remove(Data)
            End If
        Next

        For Each Data As String In IPBlackListItem
            ' IP | Ban Reason | Ban Start Time | Ban Time
            If CType(Functions.GetSplit(Data, 2, "|"), DateTime).AddSeconds(CDbl(Functions.GetSplit(Data, 3, "|"))) <= DateTime.Now Then
                IPBlackListItem.Remove(Data)
            End If
        Next

        For Each Data As String In MuteListItem
            ' Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time
            If CType(Functions.GetSplit(Data, 3, "|"), DateTime).AddSeconds(CDbl(Functions.GetSplit(Data, 4, "|"))) <= DateTime.Now Then
                MuteListItem.Remove(Data)
            End If
        Next

        If ErrorMessage.Count > 0 Then
            Dim ErrorText As String = Nothing
            For Each Errors As String In ErrorMessage
                ErrorText = ErrorText + Errors + vbNewLine
                Main.AddLog(Errors, Main.LogType.Warning)
                Main.AddLog("Application will generate the default setting for the above error.", Main.LogType.Info)
            Next
            Functions.ReturnMessage(ErrorText, MsgBoxStyle.Information, "Settings")
            Return False
        End If
        Return True
    End Function

    Public Function Value(ByVal Setting As String) As String
        Try
            If HaveSettingFile() Then
                If Not (String.IsNullOrEmpty(Setting) Or String.IsNullOrWhiteSpace(Setting)) Then
                    Dim Flag As Boolean = False
                    For Each Lines As String In File.ReadAllLines(Settings.ApplicationDirectory + "\application_settings.dat")
                        If Lines.Contains("<!--") Or Lines.Contains("/*") Then
                            Flag = True
                        End If
                        If Flag = True Then
                            If Lines.Contains("-->") Or Lines.Contains("*/") Then
                                Flag = False
                            End If
                        Else
                            If Lines.Contains(Setting) Then
                                Return Functions.GetSplit(Lines, 1, "|")
                            End If
                        End If
                    Next
                End If
            End If
            Return Nothing
        Catch ex As Exception
            Functions.CatchError(ex)
            Return Nothing
        End Try
    End Function

    Public Function HaveSettingFile() As Boolean
        Try
            If File.Exists(Settings.ApplicationDirectory + "\application_settings.dat") Then
                Dim str1 As String = File.ReadAllText(Settings.ApplicationDirectory + "\application_settings.dat")
                If String.IsNullOrEmpty(str1) Or String.IsNullOrWhiteSpace(str1) Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            Functions.CatchError(ex)
            Return False
        End Try
    End Function

    Public Function HaveValidSettings(ByVal Settings As String, ByVal CheckMethod As CheckMethod) As Boolean
        If Settings = Nothing Then
            Return False
        Else
            If CheckMethod = CheckMethod.CheckMethod_Directory Then
                If Directory.Exists(Settings) Then
                    Return True
                Else
                    Return False
                End If
            ElseIf CheckMethod = CheckMethod.CheckMethod_Boolean Then
                If String.Equals(Settings, "True", StringComparison.OrdinalIgnoreCase) Or String.Equals(Settings, "False", StringComparison.OrdinalIgnoreCase) Then
                    Return True
                Else
                    Return False
                End If
            ElseIf CheckMethod = CheckMethod.CheckMethod_String Then
                If String.IsNullOrEmpty(Settings) Or String.IsNullOrWhiteSpace(Settings) Then
                    Return False
                Else
                    Return True
                End If
            ElseIf CheckMethod = CheckMethod.CheckMethod_Integer Then
                Dim Temp As Integer
                If Integer.TryParse(Settings, Temp) Then
                    Return True
                Else
                    Return False
                End If
            ElseIf CheckMethod = CheckMethod.CheckMethod_IP Then
                Dim Temp As IPAddress = Nothing
                If Net.IPAddress.TryParse(Settings, Temp) Then
                    Return True
                Else
                    Return False
                End If
            ElseIf CheckMethod = CheckMethod.CheckMethod_Port Then
                Dim Temp As Integer
                If Integer.TryParse(Settings, Temp) Then
                    If Temp <= 65535 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End If
    End Function

End Class
