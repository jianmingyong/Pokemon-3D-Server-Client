Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions

Public Class Setting

#Region "Property"
    '<!-- Main -->
    Public ReadOnly Property ApplicationDirectory As String = Application.StartupPath

    '<!-- Pokémon 3D Server Client Setting File -->
    Public ReadOnly Property StartTime As Date = Date.Now
    Public ReadOnly Property ApplicationVersion As String = My.Application.Info.Version.ToString
    Public ReadOnly Property ProtocolVersion As String = "0.5"

    Public Property CheckForUpdate As Boolean = True
    Public Property GeneratePublicIP As Boolean = True

    '<!-- Main Server Property -->
    Public _IPAddress As IPAddress = Net.IPAddress.Parse(GetPublicIp)
    Public Property IPAddress As String
        Set(value As String)
            Try
                If String.IsNullOrWhiteSpace(value) Then
                    _IPAddress = Net.IPAddress.Parse(GetPublicIp)
                Else
                    _IPAddress = Net.IPAddress.Parse(value)
                End If
            Catch ex As Exception
                _IPAddress = Net.IPAddress.Parse(GetPublicIp)
            End Try
        End Set
        Get
            Return _IPAddress.ToString
        End Get
    End Property
    Public _Port As Integer = 15124
    Public Property Port As Integer
        Set(value As Integer)
            If CInt(value) < 0 Then
                _Port = 0
            ElseIf CInt(value) > 65535
                _Port = 65535
            Else
                _Port = CInt(value)
            End If
        End Set
        Get
            Return _Port
        End Get
    End Property
    Public Property ServerName As String = "P3D Server"
    Public Property ServerMessage As String
    Public Property WelcomeMessage As String
    Public Property GameMode As String = "Pokemon 3D"
    Public _MaxPlayers As Integer = 20
    Public Property MaxPlayers As Integer
        Set(value As Integer)
            If value < 1 Then
                _MaxPlayers = -1
            Else
                _MaxPlayers = value
            End If
        End Set
        Get
            Return _MaxPlayers
        End Get
    End Property
    Public Property OfflineMode As Boolean = False

    '<!-- Advanced Server Property -->
    '<!-- World: -->
    Public _Season As Integer = -2
    Public Property Season As Integer
        Set(value As Integer)
            If value < -4 Then
                _Season = -2
            ElseIf value > 3
                _Season = -2
            Else
                _Season = value
            End If
        End Set
        Get
            Return _Season
        End Get
    End Property
    Public _Weather As Integer = -2
    Public Property Weather As Integer
        Set(value As Integer)
            If value < -4 Then
                _Weather = -2
            ElseIf value > 9
                _Weather = -2
            Else
                _Weather = value
            End If
        End Set
        Get
            Return _Weather
        End Get
    End Property
    Public Property DoDayCycle As Boolean = True

    Public Property SeasonMonth As New World.SeasonMonth
    Public Property WeatherSeason As New World.WeatherSeason

    '<!-- FailSafe Features: -->
    Public _NoPingKickTime As Integer = 60
    Public Property NoPingKickTime As Integer
        Set(value As Integer)
            If value < 1 Then
                _NoPingKickTime = -1
            Else
                _NoPingKickTime = value
            End If
        End Set
        Get
            Return _NoPingKickTime
        End Get
    End Property
    Public _AFKKickTime As Integer = 300
    Public Property AFKKickTime As Integer
        Set(value As Integer)
            If value < 1 Then
                _AFKKickTime = -1
            Else
                _AFKKickTime = value
            End If
        End Set
        Get
            Return _AFKKickTime
        End Get
    End Property
    Public _AutoRestartTime As Integer = -1
    Public Property AutoRestartTime As Integer
        Set(value As Integer)
            If value < 1 Then
                _AutoRestartTime = -1
            Else
                _AutoRestartTime = value
            End If
        End Set
        Get
            Return _AutoRestartTime
        End Get
    End Property

    '<!-- Features: -->
    Public Property BlackList As Boolean = True
    Public Property BlackListData As New List(Of BlackList)
    Public Property IPBlackList As Boolean = True
    Public Property IPBlackListData As New List(Of IPBlackList)
    Public Property WhiteList As Boolean = False
    Public Property WhiteListData As New List(Of WhiteList)
    Public Property OperatorList As Boolean = True
    Public Property OperatorListData As New List(Of OperatorList)
    Public Property MuteList As Boolean = True
    Public Property MuteListData As New List(Of MuteList)
    Public Property OnlineSettingList As Boolean = True
    Public Property OnlineSettingListData As New List(Of OnlineSetting)
    Public Property SwearInfractionList As Boolean = False
    Public Property SwearInfractionListData As New List(Of SwearInfractionList)

    '<!-- Swear Infraction Feature -->
    Public Property SwearInfractionFilterListData As New List(Of SwearInfractionFilterList)
    Public _SwearInfractionCap As Integer
    Public Property SwearInfractionCap As Integer
        Set(value As Integer)
            If value < 1 Then
                _SwearInfractionCap = -1
            Else
                _SwearInfractionCap = value
            End If
        End Set
        Get
            Return _SwearInfractionCap
        End Get
    End Property
    Public _SwearInfractionReset As Integer
    Public Property SwearInfractionReset As Integer
        Set(value As Integer)
            If value < 1 Then
                _SwearInfractionReset = -1
            Else
                _SwearInfractionReset = value
            End If
        End Set
        Get
            Return _SwearInfractionReset
        End Get
    End Property

    '<!-- Spam Feature -->
    Public _SpamResetDuration As Integer = -1
    Public Property SpamResetDuration As Integer
        Set(value As Integer)
            If value < 1 Then
                _SpamResetDuration = -1
            Else
                _SpamResetDuration = value
            End If
        End Set
        Get
            Return _SpamResetDuration
        End Get
    End Property

    '<!-- Logger -->
    Public Property LoggerInfo As Boolean = True
    Public Property LoggerWarning As Boolean = True
    Public Property LoggerDebug As Boolean = False
    Public Property LoggerChat As Boolean = True
    Public Property LoggerPM As Boolean = True
    Public Property LoggerServer As Boolean = True
    Public Property LoggerTrade As Boolean = True
    Public Property LoggerPvP As Boolean = True
    Public Property LoggerCommand As Boolean = True

    '<!-- Token -->
    Public Property TokenDefination As New Dictionary(Of String, String)

    '<!-- MapFile -->
    Public Property MapFileListData As New List(Of MapFileList)

#End Region

    Public Sub Load()
        Main.Main.QueueMessage("Setting.vb: Load Setting.", Main.LogType.Info)

        Try

#Region "application_settings.json"
            If HaveSettingFile("application_settings.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\application_settings.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim ObjectPropertyName As String = Nothing
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing
                Dim SeasonMonth As New List(Of String)
                Dim WeatherSeason As New List(Of String)

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                        If Not TempPropertyName = Nothing AndAlso Not TempPropertyName = ObjectPropertyName Then
                            ObjectPropertyName = TempPropertyName
                            TempPropertyName = Nothing
                        End If
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 3 AndAlso String.Equals(ObjectPropertyName, "SeasonMonth", StringComparison.OrdinalIgnoreCase) Then
                            Dim TempValue As String = Nothing
                            For Each item As String In SeasonMonth
                                TempValue &= item & "|"
                            Next
                            TempValue = TempValue.Remove(TempValue.LastIndexOf("|"))
                            Me.SeasonMonth.SeasonData = TempValue
                        ElseIf StartObjectDepth = 3 AndAlso String.Equals(ObjectPropertyName, "WeatherSeason", StringComparison.OrdinalIgnoreCase) Then
                            Dim TempValue As String = Nothing
                            For Each item As String In WeatherSeason
                                TempValue &= item & "|"
                            Next
                            TempValue = TempValue.Remove(TempValue.LastIndexOf("|"))
                            Me.WeatherSeason.WeatherData = TempValue
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 AndAlso String.Equals(ObjectPropertyName, "Pokémon 3D Server Client Setting File", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "CheckForUpdate", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    CheckForUpdate = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: CheckForUpdate does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GeneratePublicIP", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    GeneratePublicIP = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: GeneratePublicIP does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 1 AndAlso String.Equals(ObjectPropertyName, "Main Server Property", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "IPAddress", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    IPAddress = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: IPAddress does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Port", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Port = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: Port does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "ServerName", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    ServerName = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: ServerName does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "ServerMessage", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    ServerMessage = Reader.Value.ToString
                                ElseIf Reader.TokenType = JsonToken.Null
                                    ServerMessage = Nothing
                                Else
                                    Main.Main.QueueMessage("Setting.vb: ServerMessage does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "WelcomeMessage", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    WelcomeMessage = Reader.Value.ToString
                                ElseIf Reader.TokenType = JsonToken.Null
                                    WelcomeMessage = Nothing
                                Else
                                    Main.Main.QueueMessage("Setting.vb: WelcomeMessage does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameMode", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    GameMode = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: GameMode does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MaxPlayers", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    MaxPlayers = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MaxPlayers does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "OfflineMode", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    OfflineMode = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OfflineMode does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 2 AndAlso String.Equals(ObjectPropertyName, "World", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Season", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Season = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: Season does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Weather", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Weather = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: Weather does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "DoDayCycle", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    DoDayCycle = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: DoDayCycle does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 3 AndAlso String.Equals(ObjectPropertyName, "SeasonMonth", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.String Then
                                SeasonMonth.Add(Reader.Value.ToString)
                            Else
                                Main.Main.QueueMessage("Setting.vb: SeasonMonth does not match the require type. Default value will be used.", Main.LogType.Warning)
                            End If
                        End If
                    ElseIf StartObjectDepth = 3 AndAlso String.Equals(ObjectPropertyName, "WeatherSeason", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.String Then
                                WeatherSeason.Add(Reader.Value.ToString)
                            Else
                                Main.Main.QueueMessage("Setting.vb: WeatherSeason does not match the require type. Default value will be used.", Main.LogType.Warning)
                            End If
                        End If
                    ElseIf StartObjectDepth = 2 AndAlso String.Equals(ObjectPropertyName, "FailSafe Features", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "NoPingKickTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    NoPingKickTime = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: NoPingKickTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "AFKKickTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    AFKKickTime = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: AFKKickTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "AutoRestartTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    AutoRestartTime = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: AutoRestartTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 2 AndAlso String.Equals(ObjectPropertyName, "Features", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "BlackList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    BlackList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: BlackList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "IPBlackList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    IPBlackList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: IPBlackList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "WhiteList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    WhiteList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: WhiteList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "OperatorList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    OperatorList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OperatorList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    MuteList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MuteList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "OnlineSettingList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    OnlineSettingList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OnlineSettingList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "SwearInfractionList", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    SwearInfractionList = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionList does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 3 AndAlso String.Equals(ObjectPropertyName, "Swear Infraction Feature", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "SwearInfractionCap", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    SwearInfractionCap = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionCap does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "SwearInfractionReset", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    SwearInfractionReset = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionReset does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 3 AndAlso String.Equals(ObjectPropertyName, "Spam Feature", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "SpamResetDuration", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    SpamResetDuration = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SpamResetDuration does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 1 AndAlso String.Equals(ObjectPropertyName, "Server Client Logger", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "LoggerInfo", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerInfo = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerInfo does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerWarning", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerWarning = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerWarning does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerDebug", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerDebug = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerDebug does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerChat", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerChat = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerChat does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerPM", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerPM = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerPM does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerServer", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerServer = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerServer does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerTrade", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerTrade = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerTrade does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerPvP", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerPvP = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerPvP does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "LoggerCommand", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    LoggerCommand = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: LoggerCommand does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\BlackList.json"
            If HaveSettingFile("Data\BlackList.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\BlackList.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Name As String = Nothing
                Dim GameJoltID As Integer = -1
                Dim BanReason As String = Nothing
                Dim BanStartTime As Date = Nothing
                Dim BanDuration As Integer = 0

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            BlackListData.Add(New BlackList(Name, GameJoltID, BanReason, BanStartTime, BanDuration))
                            Name = Nothing
                            GameJoltID = -1
                            BanReason = Nothing
                            BanStartTime = Nothing
                            BanDuration = 0
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: BlackList.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    GameJoltID = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: BlackList.GameJoltID does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "BanReason", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    BanReason = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: BlackList.BanReason does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "BanStartTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Date Then
                                    BanStartTime = CDate(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: BlackList.BanStartTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "BanDuration", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    BanDuration = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: BlackList.BanDuration does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\IPBlackList.json"
            If HaveSettingFile("Data\IPBlackList.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\IPBlackList.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim IPAddress As String = Nothing
                Dim BanReason As String = Nothing
                Dim BanStartTime As Date = Nothing
                Dim BanDuration As Integer = 0

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            IPBlackListData.Add(New IPBlackList(IPAddress, BanReason, BanStartTime, BanDuration))
                            IPAddress = Nothing
                            BanReason = Nothing
                            BanStartTime = Nothing
                            BanDuration = 0
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "IPAddress", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    IPAddress = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: IPBlackList.IPAddress does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "BanReason", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    BanReason = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: IPBlackList.BanReason does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "BanStartTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Date Then
                                    BanStartTime = CDate(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: IPBlackList.BanStartTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "BanDuration", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    BanDuration = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: IPBlackList.BanDuration does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\WhiteList.json"
            If HaveSettingFile("Data\WhiteList.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\WhiteList.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Name As String = Nothing
                Dim GameJoltID As Integer = -1
                Dim WhiteListReason As String = Nothing

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            WhiteListData.Add(New WhiteList(Name, GameJoltID, WhiteListReason))
                            Name = Nothing
                            GameJoltID = -1
                            WhiteListReason = Nothing
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: WhiteList.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    GameJoltID = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: WhiteList.GameJoltID does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "WhiteListReason", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    WhiteListReason = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: WhiteList.WhiteListReason does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\OperatorList.json"
            If HaveSettingFile("Data\OperatorList.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\OperatorList.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Name As String = Nothing
                Dim GameJoltID As Integer = -1
                Dim OperatorReason As String = Nothing
                Dim OperatorLevel As Integer = 0

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            OperatorListData.Add(New OperatorList(Name, GameJoltID, OperatorReason, OperatorLevel))
                            Name = Nothing
                            GameJoltID = -1
                            OperatorReason = Nothing
                            OperatorLevel = 0
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OperatorList.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    GameJoltID = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OperatorList.GameJoltID does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "OperatorReason", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    OperatorReason = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OperatorList.OperatorReason does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "OperatorLevel", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    OperatorLevel = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: OperatorList.OperatorLevel does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\MuteList.json"
            If HaveSettingFile("Data\MuteList.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\MuteList.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Name As String = Nothing
                Dim GameJoltID As Integer = -1
                Dim MuteReason As String = Nothing
                Dim MuteStartTime As Date = Nothing
                Dim MuteDuration As Integer = 0

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            MuteListData.Add(New MuteList(Name, GameJoltID, MuteReason, MuteStartTime, MuteDuration))
                            Name = Nothing
                            GameJoltID = -1
                            MuteReason = Nothing
                            MuteStartTime = Nothing
                            MuteDuration = 0
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MuteList.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    GameJoltID = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MuteList.GameJoltID does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteReason", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    MuteReason = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MuteList.MuteReason does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteStartTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Date Then
                                    MuteStartTime = CDate(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MuteList.MuteStartTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteDuration", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    MuteDuration = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MuteList.MuteDuration does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\Token.json"
            If HaveSettingFile("Data\Token.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\Token.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Name As String = Nothing
                Dim Description As String = Nothing

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            TokenDefination.Add(Name, Description)
                            Name = Nothing
                            Description = Nothing
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: Token.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Description", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Description = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: Token.Description does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\SwearFilter.json"
            If HaveSettingFile("Data\SwearFilter.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\SwearFilter.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Word As String = Nothing
                Dim CaseSensitive As Boolean = False

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            SwearInfractionFilterListData.Add(New SwearInfractionFilterList(Word, CaseSensitive))
                            Word = Nothing
                            CaseSensitive = False
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Word", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Word = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearFilter.Word does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "CaseSensitive", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Boolean Then
                                    CaseSensitive = CBool(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearFilter.CaseSensitive does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\SwearInfractionList.json"
            If HaveSettingFile("Data\SwearInfractionList.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\SwearInfractionList.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Name As String = Nothing
                Dim GameJoltID As Integer = -1
                Dim Points As Integer = 0
                Dim Muted As Integer = 0
                Dim StartTime As Date = Nothing

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            SwearInfractionListData.Add(New SwearInfractionList(Name, GameJoltID, Points, Muted, StartTime))
                            Name = Nothing
                            GameJoltID = -1
                            Points = 0
                            Muted = 0
                            StartTime = Nothing
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionList.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    GameJoltID = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionList.GameJoltID does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Points", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Points = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionList.Points does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Muted", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Muted = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionList.Muted does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "StartTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Date Then
                                    StartTime = CDate(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("Setting.vb: SwearInfractionList.StartTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

#Region "Data\MapFile.json"
            If HaveSettingFile("Data\MapFile.json") Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(ApplicationDirectory & "\Data\MapFile.json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim PropertyName As String = Nothing
                Dim TempPropertyName As String = Nothing

                Dim Path As String = Nothing
                Dim Name As String = Nothing

                While Reader.Read
                    If Reader.TokenType = JsonToken.StartObject Then
                        StartObjectDepth += 1
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 Then
                            MapFileListData.Add(New MapFileList(Path, Name))
                            Path = Nothing
                            Name = Nothing
                        End If
                        StartObjectDepth -= 1
                    End If

                    If Reader.TokenType = JsonToken.PropertyName Then
                        TempPropertyName = Reader.Value.ToString
                    ElseIf Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String
                        PropertyName = TempPropertyName
                        TempPropertyName = Nothing
                    End If

                    If StartObjectDepth = 1 Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Path", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Path = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MapFile.Path does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("Setting.vb: MapFile.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
#End Region

        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Sub Save()
        Main.Main.QueueMessage("Setting.vb: Save Setting.", Main.LogType.Info)

        Try
            If Not Directory.Exists(ApplicationDirectory & "\Data") Then
                Directory.CreateDirectory(ApplicationDirectory & "\Data")
            End If

#Region "application_settings.json"
            Dim ServerMessage As String = Me.ServerMessage
            If String.IsNullOrWhiteSpace(ServerMessage) Then
                ServerMessage = "null"
            Else
                ServerMessage = """" & Me.ServerMessage & """"
            End If

            Dim WelcomeMessage As String = Me.WelcomeMessage
            If String.IsNullOrWhiteSpace(WelcomeMessage) Then
                WelcomeMessage = "null"
            Else
                WelcomeMessage = """" & Me.WelcomeMessage & """"
            End If

            File.WriteAllText(ApplicationDirectory & "\application_settings.json",
"{

    /* Warning: The syntax for each setting is case sensitive.
       String: ""Text inside a quote""
       Integer: 0123456789
       Boolean: true
    */

	""Pokémon 3D Server Client Setting File"":
	{
		""StartTime"": """ & StartTime.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffK") & """,
        ""ApplicationVersion"": """ & ApplicationVersion & """,
        ""ProtocalVersion"": """ & ProtocolVersion & """,
        
        /* CheckForUpdate:  To allow application to check for update upon launch.
           Syntax: Boolean: true, false */
        ""CheckForUpdate"": " & CheckForUpdate.ToString.ToLower & ",
        
        /* GeneratePublicIP:  To allow application to update IP address upon launch.
		   Syntax: Boolean: true, false */
        ""GeneratePublicIP"": " & GeneratePublicIP.ToString.ToLower & "
    },
        
    ""Main Server Property"":
    {
        /* IPAddress:  Public IP address of your server.
		   Syntax: Valid IPv4 address. */
        ""IPAddress"": """ & IPAddress & """,
        
        /* Port:  The port to use on your server.
		   Syntax: Integer: Between 0 to 65535 inclusive. */
        ""Port"": " & Port.ToString & ",
        
        /* ServerName:  The server name to display to public.
		   Syntax: String */
        ""ServerName"": """ & ServerName & """,
        
        /* ServerMessage:  The server message to display when a player select a server.
		   Syntax: String: null for blank. */
        ""ServerMessage"": " & ServerMessage & ",

        /* WelcomeMessage:  The server message to display when a player joins a server.
		   Syntax: String: null for blank. */
        ""WelcomeMessage"": " & WelcomeMessage & ",

        /* GameMode:  The GameMode that player should play in order to join the server.
		   Syntax: String */
        ""GameMode"": """ & GameMode & """,

        /* MaxPlayers:  The maximum amount of player in the server that can join.
		   Syntax: Integer: -1: Unlimited. */
        ""MaxPlayers"": " & MaxPlayers.ToString & ",

        /* OfflineMode:  The ability for offline profile player to join the server.
		   Syntax: Boolean: true, false */
        ""OfflineMode"": " & OfflineMode.ToString.ToLower & "
    },

    ""Advanced Server Property"":
    {
        ""World"":
        {
            /* Season:  To set server default season.
			    Syntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | SeasonMonth = -3 */
            ""Season"": " & Season.ToString & ",

            /* Weather:  To set server default weather.
			    Syntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | WeatherSeason = -3 */
            ""Weather"": " & Weather.ToString & ",

            /* DoDayCycle:  To allow the server to update day cycle.
			    Syntax: Boolean: true, false */
			""DoDayCycle"": " & DoDayCycle.ToString.ToLower & ",

            /* SeasonMonth:  To set the season based on local date. Must set Season = -3
			    Syntax: Integer: Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2
			    You may insert more than one season by separating it with a comma. */
            ""SeasonMonth"":
            {
                ""January"": """ & SeasonMonth.SeasonData.GetSplit(0, "|", "-2") & """,
                ""February"": """ & SeasonMonth.SeasonData.GetSplit(1, "|", "-2") & """,
                ""March"": """ & SeasonMonth.SeasonData.GetSplit(2, "|", "-2") & """,
                ""April"": """ & SeasonMonth.SeasonData.GetSplit(3, "|", "-2") & """,
                ""May"": """ & SeasonMonth.SeasonData.GetSplit(4, "|", "-2") & """,
                ""June"": """ & SeasonMonth.SeasonData.GetSplit(5, "|", "-2") & """,
                ""July"": """ & SeasonMonth.SeasonData.GetSplit(6, "|", "-2") & """,
                ""August"": """ & SeasonMonth.SeasonData.GetSplit(7, "|", "-2") & """,
                ""September"": """ & SeasonMonth.SeasonData.GetSplit(8, "|", "-2") & """,
                ""October"": """ & SeasonMonth.SeasonData.GetSplit(9, "|", "-2") & """,
                ""November"": """ & SeasonMonth.SeasonData.GetSplit(10, "|", "-2") & """,
                ""December"": """ & SeasonMonth.SeasonData.GetSplit(11, "|", "-2") & """
            },

            /* WeatherSeason:  To set the weather based on server season. Must set Weather = -3
			    Syntax: Integer: Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2
			    You may insert more than one weather by separating it with a comma. */
            ""WeatherSeason"":
            {
                ""Winter"": """ & WeatherSeason.WeatherData.GetSplit(0, "|", "-2") & """,
                ""Spring"": """ & WeatherSeason.WeatherData.GetSplit(1, "|", "-2") & """,
                ""Summer"": """ & WeatherSeason.WeatherData.GetSplit(2, "|", "-2") & """,
                ""Fall"": """ & WeatherSeason.WeatherData.GetSplit(3, "|", "-2") & """
            }
        },

        ""FailSafe Features"":
        {
            /* NoPingKickTime:  To kick player out if there are no valid ping for n amount of seconds.
			    Syntax: Integer: -1 to disable it. */
            ""NoPingKickTime"": " & NoPingKickTime.ToString & ",

            /* AFKKickTime:  To kick player out if there are no valid activity for n amount of seconds.
			    Syntax: Integer: -1 to disable it. */
            ""AFKKickTime"": " & AFKKickTime.ToString & ",

            /* AutoRestartTime:  To automatically restart the server after n seconds. Disable PvP and trade features for the last 5 minutes of the countdown.
			    Syntax: Integer: -1 to disable it. */
            ""AutoRestartTime"": " & AutoRestartTime.ToString & "
        },

        ""Features"":
        {
            /* BlackList:  To allow using blacklist feature.
			    Syntax: Boolean: true, false */
            ""BlackList"": " & BlackList.ToString.ToLower & ",

            /* IPBlackList:  To allow using ipblacklist feature.
			    Syntax: Boolean: true, false */
            ""IPBlackList"": " & IPBlackList.ToString.ToLower & ",

            /* WhiteList:  To allow using whitelist feature.
			    Syntax: Boolean: true, false */
            ""WhiteList"": " & WhiteList.ToString.ToLower & ",

            /* OperatorList:  To allow using operator feature.
			    Syntax: Boolean: true, false */
            ""OperatorList"": " & OperatorList.ToString.ToLower & ",

            /* MuteList:  To allow using mute feature.
			    Syntax: Boolean: true, false */
            ""MuteList"": " & MuteList.ToString.ToLower & ",

            /* OnlineSettingList:  To allow using mute feature.
			    Syntax: Boolean: true, false */
            ""OnlineSettingList"": " & OnlineSettingList.ToString.ToLower & ",

            /* SwearInfractionList:  To allow using swear infraction feature.
			    Syntax: Boolean: true, false */
            ""SwearInfractionList"": " & SwearInfractionList.ToString.ToLower & ",

            ""Swear Infraction Feature"":
            {
                /* SwearInfractionCap:  Amount of infraction points before the first mute.
				    Syntax: Integer: -1 to disable. */
                ""SwearInfractionCap"": " & SwearInfractionCap.ToString & ",

                /* SwearInfractionReset:  Amount of days before it expire the infraction count.
				    Syntax: Integer: -1 to disable. */
                ""SwearInfractionReset"": " & SwearInfractionReset.ToString & "
            },

            ""Spam Feature"":
            {
                /* SpamResetDuration:  Amount of seconds for the user to send the same word again.
				    Syntax: Integer: -1 to disable. */
                ""SpamResetDuration"": " & SpamResetDuration.ToString & "
            }
        }
    },

    ""Server Client Logger"":
    {
        /* LoggerInfo:  To log server information.
		   Syntax: Boolean: true, false */
        ""LoggerInfo"": " & LoggerInfo.ToString.ToLower & ",

        /* LoggerWarning:  To log server warning including ex exception.
		   Syntax: Boolean: true, false */
        ""LoggerWarning"": " & LoggerWarning.ToString.ToLower & ",

        /* LoggerDebug:  To log server package data (Lag might happen if turn on).
		   Syntax: Boolean: true, false */
        ""LoggerDebug"": " & LoggerDebug.ToString.ToLower & ",

        /* LoggerChat:  To log server chat message.
		   Syntax: Boolean: true, false */
        ""LoggerChat"": " & LoggerChat.ToString.ToLower & ",

        /* LoggerPM:  To log server private chat message. (Actual Private Message content is not logged)
		   Syntax: Boolean: true, false */
        ""LoggerPM"": " & LoggerPM.ToString.ToLower & ",

        /* LoggerServer:  To log server message.
		   Syntax: Boolean: true, false */
        ""LoggerServer"": " & LoggerServer.ToString.ToLower & ",

        /* LoggerTrade:  To log trade request. (Actual Trade Request content is not logged)
		   Syntax: Boolean: true, false */
        ""LoggerTrade"": " & LoggerTrade.ToString.ToLower & ",

        /* LoggerPvP:  To log pvp request. (Actual PvP Request content is not logged)
		   Syntax: Boolean: true, false */
        ""LoggerPvP"": " & LoggerPvP.ToString.ToLower & ",

        /* LoggerCommand:  To log server command usage. (Debug Commands are not logged)
		   Syntax: Boolean: true, false */
        ""LoggerCommand"": " & LoggerCommand.ToString.ToLower & "
    }
}", Text.Encoding.Unicode)
#End Region

#Region "Data\BlackList.json"
            Dim List As String = Nothing
            If BlackListData.Count > 0 Then
                For Each Data As BlackList In BlackListData
                    List &=
"        {
            ""Name"": """ & Data.Name & """,
            ""GameJoltID"": " & Data.GamejoltID.ToString & ",
            ""BanReason"": """ & Data.BanReason & """,
            ""BanStartTime"": """ & Data.BanStartTime.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffK") & """,
            ""BanDuration"": " & Data.BanDuration.ToString & "
        },
"
                Next
                List = List.Remove(List.LastIndexOf(","))
            End If

            File.WriteAllText(ApplicationDirectory & "\Data\BlackList.json",
"{
	""BlackListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\IPBlackList.json"
            List = Nothing
            If IPBlackListData.Count > 0 Then
                For Each Data As IPBlackList In IPBlackListData
                    List &=
"        {
            ""IPAddress"": """ & Data.IPAddress & """,
            ""BanReason"": """ & Data.BanReason & """,
            ""BanStartTime"": """ & Data.BanStartTime.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffK") & """,
            ""BanDuration"": " & Data.BanDuration.ToString & "
        },
"
                Next
                List = List.Remove(List.LastIndexOf(","))
            End If

            File.WriteAllText(ApplicationDirectory & "\Data\IPBlackList.json",
"{
	""IPBlackListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\WhiteList.json"
            List = Nothing
            If WhiteListData.Count > 0 Then
                For Each Data As WhiteList In WhiteListData
                    List &=
"        {
            ""Name"": """ & Data.Name & """,
            ""GameJoltID"": " & Data.GameJoltID.ToString & ",
            ""WhiteListReason"": """ & Data.WhiteListReason & """
        },
"
                Next
                List = List.Remove(List.LastIndexOf(","))
            End If

            File.WriteAllText(ApplicationDirectory & "\Data\WhiteList.json",
"{
	""WhiteListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\OperatorList.json"
            List = Nothing
            If OperatorListData.Count > 0 Then
                For Each Data As OperatorList In OperatorListData
                    List &=
"        {
            ""Name"": """ & Data.Name & """,
            ""GameJoltID"": " & Data.GameJoltID.ToString & ",
            ""OperatorReason"": """ & Data.OperatorReason & """,
            ""OperatorLevel"": " & Data.OperatorLevel.ToString & "
        },
"
                Next
                List = List.Remove(List.LastIndexOf(","))
            End If

            File.WriteAllText(ApplicationDirectory & "\Data\OperatorList.json",
"{
	""OperatorListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\MuteList.json"
            List = Nothing
            If MuteListData.Count > 0 Then
                For Each Data As MuteList In MuteListData
                    List &=
"        {
            ""Name"": """ & Data.Name & """,
            ""GameJoltID"": " & Data.GameJoltID.ToString & ",
            ""MuteReason"": """ & Data.MuteReason & """,
            ""MuteStartTime"": """ & Data.MuteStartTime.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffK") & """,
            ""MuteDuration"": " & Data.MuteDuration.ToString & "
        },
"
                Next
                List = List.Remove(List.LastIndexOf(","))
            End If

            File.WriteAllText(ApplicationDirectory & "\Data\MuteList.json",
"{
	""MuteListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\Token.json"
            List = Nothing

            Dim Token As New Dictionary(Of String, String)
            Token.Add("SERVER_AFKKICKED", "You have been inactive for past {{{0}}}.")
            Token.Add("SERVER_AFKWARNING", "You have been inactive for past {{{0}}}. Please avoid afking or you will be kicked in {{{1}}}.")
            Token.Add("SERVER_BLACKLISTED", "You have been banned on the server. Reason: {{{0}}}")
            Token.Add("SERVER_CHATGAMEJOLT", "<{{{0}}} ({{{1}}})>: {{{2}}}")
            Token.Add("SERVER_CHATNOGAMEJOLT", "<{{{0}}}>: {{{1}}}")
            Token.Add("SERVER_CLOSE", "This server have been shut down or lost its connection. Sorry for the inconveniences caused.")
            Token.Add("SERVER_CONFIGNOTMATCH", "The server you are trying to join require {{{0}}}.")
            Token.Add("SERVER_FULL", "The server you are trying to join is currently full of players. Please try again later.")
            Token.Add("SERVER_GAMEJOLT", "{{{0}}} ({{{1}}}) {{{2}}}")
            Token.Add("SERVER_LOGINREMINDER", "You have been playing for {{{0}}}. We cherish your stay but we also encourage you to take a short break :)")
            Token.Add("SERVER_NOGAMEJOLT", "{{{0}}} {{{1}}}")
            Token.Add("SERVER_NOPING", "You have lost connection to the server or the server is having network issue. Sorry for the inconveniences caused.")
            Token.Add("SERVER_PLAYERDUPLICATE", "You are still on the server. Please try again later.")
            Token.Add("SERVER_PLAYERLEFT", "You have left the server.")
            Token.Add("SERVER_PLAYERMUTED", "You have been muted on the server. Reason: {{{0}}}")
            Token.Add("SERVER_PLAYERMUTED2", "You have been muted by that player. Reason: {{{0}}}")
            Token.Add("SERVER_PLAYERNOTFOUND", "The player does not exist in the server.")
            Token.Add("SERVER_PLAYERSWEARED", "The chat bot detected a swearing word. Please minimise the usage where possible. Triggered word: {{{0}}}")
            Token.Add("SERVER_RESTARTWARNING", "[WARNING]: This server is schedule for a restart in {{{0}}}. For your personal safety, starting a new trade and PvP during this period is disabled. Save your game now to prevent data lost.")
            Token.Add("SERVER_SPAMDETECTION", "The chat bot detected that you are trying to spam the same word again in quick succession. Please wait for {{{0}}} to send again.")
            Token.Add("SERVER_WHITELIST", "You are not whitelisted on the server.")
            Token.Add("SERVER_COMMANDPERMISSION", "You do not have the required permission to use this command.")

            For Each Data As KeyValuePair(Of String, String) In Token
                List &=
"        {
            ""Name"": """ & Data.Key & """,
            ""Description"": """ & Data.Value & """
        },
"
            Next
            List = List.Remove(List.LastIndexOf(","))

            File.WriteAllText(ApplicationDirectory & "\Data\Token.json",
"{
	""Token"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\SwearFilter.json"
            If Not HaveSettingFile("Data\SwearFilter.json") Then
                File.WriteAllText(ApplicationDirectory & "\Data\SwearFilter.json",
"{
	""SwearFilterData"":
	[
        {
			""Word"": ""anus"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""arse"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""arsehole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""ass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asshat"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assjabber"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asspirate"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assbandit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assbanger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assbite"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assclown"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asscock"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asscracker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asses"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assgoblin"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asshat"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asshead"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asshole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asshopper"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assjacker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asslick"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asslicker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assmonkey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assmunch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assmuncher"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assnigger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asspirate"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""assshole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asssucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asswad"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""asswipe"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""axwound"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bastard"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""beaner"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bitch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bitchass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bitches"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bitchtits"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bitchy"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""blowjob"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bollocks"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bollox"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""boner"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""brotherfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bullshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""bumblefuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""buttpirate"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""buttfucka"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""buttfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""carpetmuncher"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""chesticle"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""chinc"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""chink"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""choad"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""chode"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""clit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""clitface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""clitfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""clusterfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cock"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockbite"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockburger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockhead"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockjockey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockknoker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockmaster"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockmongler"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockmongruel"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockmonkey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockmuncher"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocknose"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocknugget"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocksmith"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocksmoke"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocksmoker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocksniffer"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cocksucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cockwaffle"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""coochie"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""coochy"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""coon"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cooter"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cum"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cumbubble"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cumdumpster"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cumguzzler"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cumjockey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cumslut"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cumtart"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cunnie"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cunnilingus"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cunt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cuntass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cuntface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cunthole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cuntlicker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cuntrag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""cuntslut"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dago"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""deggo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dick"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dicksneeze"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickbeaters"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickhead"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickhole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickjuice"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickmilk"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickmonger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dicks"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickslap"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dicksucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dicksucking"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dicktickler"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickwad"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickweasel"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickweed"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dickwod"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dike"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dildo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dipshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""doochbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dookie"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""douche"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""douchefag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""douchebag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""douchewaffle"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dumass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dumbass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dumbfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dumbshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dumshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""dyke"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fagbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fagfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""faggit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""faggot"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""faggotcock"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fagtard"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fatass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fellatio"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""feltch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""flamer"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckboy"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckbrain"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckbutt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckbutter"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucked"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckersucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckhead"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckhole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckin"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucking"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucknut"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucknutt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckoff"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucks"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckstick"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucktard"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fucktart"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckup"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckwad"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckwit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fuckwitt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""fudgepacker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gay"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gayass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gaybob"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gaydo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gayfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gayfuckist"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gaylord"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gaytard"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gaywad"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""goddamn"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""goddamnit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gooch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gook"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""gringo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""guido"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""handjob"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""hard On"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""heeb"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""hell"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""homo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""homodumbshit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""honkey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""humping"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""jackass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""jagoff"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""jerkass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""jigaboo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""jizz"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""junglebunny"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""kooch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""kootch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""kraut"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""kunt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""kyke"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""lameass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""lardass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""lesbian"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""lesbo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""lezzie"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""mcfagget"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""mick"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""minge"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""mothafucka"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""mothafuckin"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""motherfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""motherfucking"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""muff"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""muffdiver"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""munging"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""negro"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""nigaboo"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""nigga"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""nigger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""niggers"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""niglet"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""nutsack"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""paki"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""panooch"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pecker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""peckerhead"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""penis"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""penisbanger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""penisfucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""penispuffer"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""piss"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pissed"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pissed off"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pissflaps"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""polesmoker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pollock"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""poon"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""poonani"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""poonany"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""poontang"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""porch monkey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""porchmonkey"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""punanny"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""punta"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pussies"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pussy"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""pussylicking"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""puto"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""queef"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""queer"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""queerbait"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""queerhole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""renob"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""rimjob"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""ruski"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""sandnigger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""schlong"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""scrote"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitbagger"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitbrains"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitbreath"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitcanned"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitcunt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitdick"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitfaced"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shithead"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shithole"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shithouse"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitspitter"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitstain"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitter"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shittiest"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitting"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shitty"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shiz"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""shiznit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""skank"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""skeet"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""skullfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""slut"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""slutbag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""smeg"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""spic"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""spick"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""splooge"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""spook"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""suckass"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""tard"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""testicle"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""thundercunt"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""tit"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""titfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""tits"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""tittyfuck"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""twat"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""twatlips"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""twats"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""twatwaffle"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""unclefucker"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""vajj"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""vag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""vagina"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""vajayjay"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""vjayjay"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""wank"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""wankjob"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""wetback"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""whore"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""whorebag"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""whoreface"",
			""CaseSensitive"": false
		},
		{
			""Word"": ""wop"",
			""CaseSensitive"": false
		}
	]
}", Text.Encoding.Unicode)
            End If
#End Region

#Region "Data\SwearInfractionList.json"
            List = Nothing
            If SwearInfractionListData.Count > 0 Then
                For Each Data As SwearInfractionList In SwearInfractionListData
                    List &=
"        {
            ""Name"": """ & Data.Name & """,
            ""GameJoltID"": " & Data.GameJoltID.ToString & ",
            ""Points"": " & Data.Points & ",
            ""Muted"": " & Data.Muted & ",
            ""StartTime"": """ & Data.StartTime.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffK") & """,
        },
"
                Next
                List = List.Remove(List.LastIndexOf(","))
            End If

            File.WriteAllText(ApplicationDirectory & "\Data\SwearInfractionList.json",
"{
	""SwearInfractionListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)
#End Region

#Region "Data\MapFile.json"
            If Not HaveSettingFile("Data\MapFile.json") Then
                File.WriteAllText(ApplicationDirectory & "\Data\MapFile.json",
"{
	""MapFileListData"":
	[
        {
			""Path"": ""azalea.dat"",
			""Name"": ""Azalea Town""
		},
		{
			""Path"": ""barktown.dat"",
			""Name"": ""New Bark Town""
		},
		{
			""Path"": ""barktown0.dat"",
			""Name"": ""Your House""
		},
		{
			""Path"": ""BerryVista.dat"",
			""Name"": ""Berry Vista""
		},
		{
			""Path"": ""blackthorn.dat"",
			""Name"": ""Blackthorn City""
		},
		{
			""Path"": ""cherrygrove.dat"",
			""Name"": ""Cherrygrove City""
		},
		{
			""Path"": ""cherrygrove_center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""cherrygrove_mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""cianwood.dat"",
			""Name"": ""Cianwood City""
		},
		{
			""Path"": ""Ecruteak.dat"",
			""Name"": ""Ecruteak City""
		},
		{
			""Path"": ""elmlab.dat"",
			""Name"": ""Elms lab""
		},
		{
			""Path"": ""goldenrod.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""ilexforest.dat"",
			""Name"": ""Ilex Forest""
		},
		{
			""Path"": ""lakeofrage.dat"",
			""Name"": ""Lake of Rage""
		},
		{
			""Path"": ""mahogany.dat"",
			""Name"": ""Mahogany Town""
		},
		{
			""Path"": ""mrpokemonhouse.dat"",
			""Name"": ""Route 30""
		},
		{
			""Path"": ""Olivine.dat"",
			""Name"": ""Olivine City""
		},
		{
			""Path"": ""route29.dat"",
			""Name"": ""Route 29""
		},
		{
			""Path"": ""route30.dat"",
			""Name"": ""Route 30""
		},
		{
			""Path"": ""route31.dat"",
			""Name"": ""Route 31""
		},
		{
			""Path"": ""route32.dat"",
			""Name"": ""Route 32""
		},
		{
			""Path"": ""route32_center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""route33.dat"",
			""Name"": ""Route 33""
		},
		{
			""Path"": ""route36.dat"",
			""Name"": ""Route 36""
		},
		{
			""Path"": ""route37.dat"",
			""Name"": ""Route 37""
		},
		{
			""Path"": ""route38.dat"",
			""Name"": ""Route 38""
		},
		{
			""Path"": ""route39.dat"",
			""Name"": ""Route 39""
		},
		{
			""Path"": ""test.dat"",
			""Name"": ""Testworld""
		},
		{
			""Path"": ""tohjofalls.dat"",
			""Name"": ""Tohjo Falls""
		},
		{
			""Path"": ""violet.dat"",
			""Name"": ""Violet City""
		},
		{
			""Path"": ""violetroute31gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""violet_center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""violet_mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""yourroom.dat"",
			""Name"": ""Your Room""
		},
		{
			""Path"": ""alph\\alph01.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\alph02.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\alph03.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\alph04.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\alph05.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\alph06.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\alphhouse.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\deepruins.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""alph\\ruinsofalph.dat"",
			""Name"": ""Ruins of Alph""
		},
		{
			""Path"": ""azalea\\0.dat"",
			""Name"": ""Azalea Town""
		},
		{
			""Path"": ""azalea\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""azalea\\kurt.dat"",
			""Name"": ""Azalea Town""
		},
		{
			""Path"": ""azalea\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""barktown\\0.dat"",
			""Name"": ""New Bark Town""
		},
		{
			""Path"": ""barktown\\1.dat"",
			""Name"": ""New Bark Town""
		},
		{
			""Path"": ""blackthorn\\000.dat"",
			""Name"": ""Blackthorn City""
		},
		{
			""Path"": ""blackthorn\\001.dat"",
			""Name"": ""Blackthorn City""
		},
		{
			""Path"": ""blackthorn\\002.dat"",
			""Name"": ""Blackthorn City""
		},
		{
			""Path"": ""blackthorn\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""blackthorn\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""boon\\1.dat"",
			""Name"": ""Two Island""
		},
		{
			""Path"": ""boon\\2.dat"",
			""Name"": ""Two Island""
		},
		{
			""Path"": ""boon\\cape.dat"",
			""Name"": ""Cape Brink""
		},
		{
			""Path"": ""boon\\capehouse.dat"",
			""Name"": ""Cape Brink""
		},
		{
			""Path"": ""boon\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""boon\\dock.dat"",
			""Name"": ""Two Island""
		},
		{
			""Path"": ""boon\\town.dat"",
			""Name"": ""Two Island""
		},
		{
			""Path"": ""burnedtower\\burnedtower1f.dat"",
			""Name"": ""Burned Tower""
		},
		{
			""Path"": ""burnedtower\\burnedtowerb1f.dat"",
			""Name"": ""Burned Tower""
		},
		{
			""Path"": ""celadon\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""celadon\\condo1.dat"",
			""Name"": ""Celadon City""
		},
		{
			""Path"": ""celadon\\condo2.dat"",
			""Name"": ""Celadon City""
		},
		{
			""Path"": ""celadon\\condo3.dat"",
			""Name"": ""Celadon City""
		},
		{
			""Path"": ""celadon\\condo4.dat"",
			""Name"": ""Celadon City""
		},
		{
			""Path"": ""celadon\\diner.dat"",
			""Name"": ""Celadon Cafe""
		},
		{
			""Path"": ""celadon\\main.dat"",
			""Name"": ""Celadon City""
		},
		{
			""Path"": ""celadon\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""cerulean\\1.dat"",
			""Name"": ""Cerulean City""
		},
		{
			""Path"": ""cerulean\\2.dat"",
			""Name"": ""Cerulean City""
		},
		{
			""Path"": ""cerulean\\3.dat"",
			""Name"": ""Cerulean City""
		},
		{
			""Path"": ""cerulean\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""cerulean\\main.dat"",
			""Name"": ""Cerulean City""
		},
		{
			""Path"": ""cerulean\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""ceruleancave\\1f.dat"",
			""Name"": ""Cerulean Cave""
		},
		{
			""Path"": ""ceruleancave\\2f.dat"",
			""Name"": ""Cerulean Cave""
		},
		{
			""Path"": ""ceruleancave\\bf1.dat"",
			""Name"": ""Cerulean Cave""
		},
		{
			""Path"": ""cherrygrove\\0.dat"",
			""Name"": ""Cherrygrove City""
		},
		{
			""Path"": ""cherrygrove\\1.dat"",
			""Name"": ""Cherrygrove City""
		},
		{
			""Path"": ""cherrygrove\\2.dat"",
			""Name"": ""Cherrygrove City""
		},
		{
			""Path"": ""chrono\\1.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""chrono\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""chrono\\dock.dat"",
			""Name"": ""Five Island""
		},
		{
			""Path"": ""chrono\\meadow.dat"",
			""Name"": ""Five Isle Meadow""
		},
		{
			""Path"": ""chrono\\memorial.dat"",
			""Name"": ""Memorial Pillar""
		},
		{
			""Path"": ""chrono\\town.dat"",
			""Name"": ""Five Island""
		},
		{
			""Path"": ""chrono\\warehouse.dat"",
			""Name"": ""Rocket Warehouse""
		},
		{
			""Path"": ""cianwood\\000.dat"",
			""Name"": ""Cianwood City""
		},
		{
			""Path"": ""cianwood\\001.dat"",
			""Name"": ""Cianwood City""
		},
		{
			""Path"": ""cianwood\\002.dat"",
			""Name"": ""Cianwood City""
		},
		{
			""Path"": ""cianwood\\003.dat"",
			""Name"": ""Cianwood City""
		},
		{
			""Path"": ""cianwood\\004.dat"",
			""Name"": ""Cianwood City""
		},
		{
			""Path"": ""cianwood\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""cinnabar\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""cinnabar\\main.dat"",
			""Name"": ""Cinnabar Island""
		},
		{
			""Path"": ""darkcave\\darkcave0.dat"",
			""Name"": ""Dark Cave""
		},
		{
			""Path"": ""darkcave\\darkcave1.dat"",
			""Name"": ""Dark Cave""
		},
		{
			""Path"": ""diglettscave\\entpew.dat"",
			""Name"": ""Diglett's Cave""
		},
		{
			""Path"": ""diglettscave\\entver.dat"",
			""Name"": ""Diglett's Cave""
		},
		{
			""Path"": ""diglettscave\\main.dat"",
			""Name"": ""Diglett's Cave""
		},
		{
			""Path"": ""dragonsden\\0.dat"",
			""Name"": ""Dragon's Den""
		},
		{
			""Path"": ""dragonsden\\1.dat"",
			""Name"": ""Dragon's Den""
		},
		{
			""Path"": ""dragonsden\\main.dat"",
			""Name"": ""Dragon's Den""
		},
		{
			""Path"": ""dragonsden\\shrine.dat"",
			""Name"": ""Dragon's Den""
		},
		{
			""Path"": ""dungeon\\checkpoint1.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\chess.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\main.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\mindend.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\passage.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\0.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\1.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\10.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\11.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\12.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\13.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\14.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\2.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\3.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\4.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\5.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\6.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\7.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\8.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\0\\9.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\0.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\1.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\10.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\11.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\12.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\13.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\14.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\15.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\16.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\17.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\18.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\19.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\2.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\20.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\21.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\22.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\23.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\24.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\3.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\4.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\5.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\6.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\7.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\8.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\1\\9.dat"",
			""Name"": ""Ancient Ruins""
		},
		{
			""Path"": ""dungeon\\2\\0.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\1.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\2.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\3.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\4.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\5.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\6.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\7.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""dungeon\\2\\outside.dat"",
			""Name"": ""Millennial Star Tower""
		},
		{
			""Path"": ""ecruteak\\001.dat"",
			""Name"": ""Ecruteak City""
		},
		{
			""Path"": ""ecruteak\\002.dat"",
			""Name"": ""Ecruteak City""
		},
		{
			""Path"": ""ecruteak\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""ecruteak\\dance_theater.dat"",
			""Name"": ""Dance Theater""
		},
		{
			""Path"": ""ecruteak\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""ecruteak\\tintower10f.dat"",
			""Name"": ""Tin Tower Peak""
		},
		{
			""Path"": ""ecruteak\\tintower1f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower2f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower3f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower4f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower5f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower6f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower7f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower8f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintower9f.dat"",
			""Name"": ""Tin Tower""
		},
		{
			""Path"": ""ecruteak\\tintowergate1.dat"",
			""Name"": ""Ecruteak City""
		},
		{
			""Path"": ""ecruteak\\tintowergate2.dat"",
			""Name"": ""Ecruteak City""
		},
		{
			""Path"": ""ecruteak\\tintowergate3.dat"",
			""Name"": ""Ecruteak City""
		},
		{
			""Path"": ""faraway\\exterior.dat"",
			""Name"": ""Faraway Island""
		},
		{
			""Path"": ""faraway\\interior.dat"",
			""Name"": ""Faraway Island""
		},
		{
			""Path"": ""floe\\1.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""floe\\2.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""floe\\3.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""floe\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""floe\\daycare.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""floe\\dock.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""floe\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""floe\\town.dat"",
			""Name"": ""Four Island""
		},
		{
			""Path"": ""fortune\\dock.dat"",
			""Name"": ""Six Island""
		},
		{
			""Path"": ""frontier\\battlefrontier.dat"",
			""Name"": ""Battle Frontier""
		},
		{
			""Path"": ""frontier\\main.dat"",
			""Name"": ""Battle Frontier""
		},
		{
			""Path"": ""frontier\\battlefactory\\arena.dat"",
			""Name"": ""Battle Factory""
		},
		{
			""Path"": ""frontier\\battlefactory\\main.dat"",
			""Name"": ""Battle Factory""
		},
		{
			""Path"": ""frontier\\battlefactory\\rental.dat"",
			""Name"": ""Battle Factory""
		},
		{
			""Path"": ""frontier\\battletower\\arena.dat"",
			""Name"": ""Battle Tower""
		},
		{
			""Path"": ""frontier\\battletower\\main.dat"",
			""Name"": ""Battle Tower""
		},
		{
			""Path"": ""frontier\\battletower\\passage.dat"",
			""Name"": ""Battle Tower""
		},
		{
			""Path"": ""fuchsia\\1.dat"",
			""Name"": ""Fuchsia City""
		},
		{
			""Path"": ""fuchsia\\2.dat"",
			""Name"": ""Fuchsia City""
		},
		{
			""Path"": ""fuchsia\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""fuchsia\\main.dat"",
			""Name"": ""Fuchsia City""
		},
		{
			""Path"": ""fuchsia\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""fuchsia\\safarioffice.dat"",
			""Name"": ""Fuchsia City""
		},
		{
			""Path"": ""gates\\alphroute32gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\alphroute36gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\azaleailexgate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\battlegate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\berry-bridgegate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\cliffedge.dat"",
			""Name"": ""Cliff Edge Gate""
		},
		{
			""Path"": ""gates\\ecruteakroute38gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\ecruteakroute42gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\fuchsia-15gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\fuchsia-19gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\goldenrodroute35gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\ilexroute34gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\league.dat"",
			""Name"": ""League Reception Gate""
		},
		{
			""Path"": ""gates\\mahoganyroute43gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\route16.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\route17-18.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\route2946gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\route43gate.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\saffroneast.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\saffronnorth.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\saffronsouth.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""gates\\saffronwest.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""goldenrod\\001.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\002.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\bill.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""goldenrod\\center_friends.dat"",
			""Name"": ""Friends Hub""
		},
		{
			""Path"": ""goldenrod\\center_global.dat"",
			""Name"": ""Global Hub""
		},
		{
			""Path"": ""goldenrod\\flowershop.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""goldenrod\\martbasement.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""goldenrod\\namerater.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\taming_shop.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\trainstation.dat"",
			""Name"": ""Goldenrod City""
		},
		{
			""Path"": ""goldenrod\\underground0.dat"",
			""Name"": ""Underground""
		},
		{
			""Path"": ""goldenrod\\underground1.dat"",
			""Name"": ""Underground""
		},
		{
			""Path"": ""goldenrod\\underground2.dat"",
			""Name"": ""Underground""
		},
		{
			""Path"": ""goldenrod\\underground3.dat"",
			""Name"": ""Underground""
		},
		{
			""Path"": ""goldenrod\\radiotower\\1f.dat"",
			""Name"": ""Radio Tower""
		},
		{
			""Path"": ""goldenrod\\radiotower\\2f.dat"",
			""Name"": ""Radio Tower""
		},
		{
			""Path"": ""goldenrod\\radiotower\\3f.dat"",
			""Name"": ""Radio Tower""
		},
		{
			""Path"": ""goldenrod\\radiotower\\4f.dat"",
			""Name"": ""Radio Tower""
		},
		{
			""Path"": ""goldenrod\\radiotower\\5f.dat"",
			""Name"": ""Radio Tower""
		},
		{
			""Path"": ""gyms\\azalea_gym.dat"",
			""Name"": ""Azalea Town Gym""
		},
		{
			""Path"": ""gyms\\blackthorn_gym_1.dat"",
			""Name"": ""Blackthorn City Gym""
		},
		{
			""Path"": ""gyms\\blackthorn_gym_2.dat"",
			""Name"": ""Blackthorn City Gym""
		},
		{
			""Path"": ""gyms\\celadon_gym.dat"",
			""Name"": ""Celadon City Gym""
		},
		{
			""Path"": ""gyms\\cerulean_gym.dat"",
			""Name"": ""Cerulean City Gym""
		},
		{
			""Path"": ""gyms\\cianwood_gym.dat"",
			""Name"": ""Cianwood City Gym""
		},
		{
			""Path"": ""gyms\\cinnabar_gym.dat"",
			""Name"": ""Cinnabar Gym""
		},
		{
			""Path"": ""gyms\\ecruteak_gym.dat"",
			""Name"": ""Ecruteak City Gym""
		},
		{
			""Path"": ""gyms\\fuchsia_gym.dat"",
			""Name"": ""Fuchsia City Gym""
		},
		{
			""Path"": ""gyms\\goldenrod_gym.dat"",
			""Name"": ""Goldenrod City Gym""
		},
		{
			""Path"": ""gyms\\mahogany_gym.dat"",
			""Name"": ""Mahogany Town Gym""
		},
		{
			""Path"": ""gyms\\olivine_gym.dat"",
			""Name"": ""Olivine City Gym""
		},
		{
			""Path"": ""gyms\\pewter_gym.dat"",
			""Name"": ""Pewter City Gym""
		},
		{
			""Path"": ""gyms\\saffron_gym.dat"",
			""Name"": ""Saffron City Gym""
		},
		{
			""Path"": ""gyms\\vermilion_gym.dat"",
			""Name"": ""Vermilion City Gym""
		},
		{
			""Path"": ""gyms\\violet_gym.dat"",
			""Name"": ""Violet City Gym""
		},
		{
			""Path"": ""gyms\\viridian_gym.dat"",
			""Name"": ""Viridian City Gym""
		},
		{
			""Path"": ""hiddengrotto\\berryvista.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\ilexforest.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\indigo.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\lakeofrage.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\mahogany.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\nationalpark0.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route26.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route27.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route31.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route32.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route34.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route35.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route39.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route42.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route43.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route44.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route46.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route47.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\route48.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\twirl.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""hiddengrotto\\violet.dat"",
			""Name"": ""Hidden Grotto""
		},
		{
			""Path"": ""icefall\\basement.dat"",
			""Name"": ""Icefall Cave""
		},
		{
			""Path"": ""icefall\\bay.dat"",
			""Name"": ""Icefall Cave""
		},
		{
			""Path"": ""icefall\\dive.dat"",
			""Name"": ""Icefall Cave""
		},
		{
			""Path"": ""icefall\\ent.dat"",
			""Name"": ""Icefall Cave""
		},
		{
			""Path"": ""icefall\\ground.dat"",
			""Name"": ""Icefall Cave""
		},
		{
			""Path"": ""icepath\\1f.dat"",
			""Name"": ""Ice Path""
		},
		{
			""Path"": ""icepath\\2f.dat"",
			""Name"": ""Ice Path""
		},
		{
			""Path"": ""icepath\\3f.dat"",
			""Name"": ""Ice Path""
		},
		{
			""Path"": ""icepath\\4f.dat"",
			""Name"": ""Ice Path""
		},
		{
			""Path"": ""indigo\\bruno.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\halloffame.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\inside.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\karen.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\koga.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\lance.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\outside.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""indigo\\will.dat"",
			""Name"": ""Indigo Plateau""
		},
		{
			""Path"": ""kin\\1.dat"",
			""Name"": ""Three Island""
		},
		{
			""Path"": ""kin\\2.dat"",
			""Name"": ""Three Island""
		},
		{
			""Path"": ""kin\\3.dat"",
			""Name"": ""Three Island""
		},
		{
			""Path"": ""kin\\4.dat"",
			""Name"": ""Three Island""
		},
		{
			""Path"": ""kin\\5.dat"",
			""Name"": ""Three Island""
		},
		{
			""Path"": ""kin\\bbridge.dat"",
			""Name"": ""Bond Bridge""
		},
		{
			""Path"": ""kin\\berry.dat"",
			""Name"": ""Berry Forest""
		},
		{
			""Path"": ""kin\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""kin\\dock.dat"",
			""Name"": ""Three Isle Port""
		},
		{
			""Path"": ""kin\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""kin\\path.dat"",
			""Name"": ""Three Isle Path""
		},
		{
			""Path"": ""kin\\port.dat"",
			""Name"": ""Three Isle Port""
		},
		{
			""Path"": ""kin\\town.dat"",
			""Name"": ""Three Island""
		},
		{
			""Path"": ""knot\\1.dat"",
			""Name"": ""One Island""
		},
		{
			""Path"": ""knot\\2.dat"",
			""Name"": ""One Island""
		},
		{
			""Path"": ""knot\\dock.dat"",
			""Name"": ""One Island""
		},
		{
			""Path"": ""knot\\kindle.dat"",
			""Name"": ""Kindle Road""
		},
		{
			""Path"": ""knot\\spa.dat"",
			""Name"": ""Ember Spa""
		},
		{
			""Path"": ""knot\\town.dat"",
			""Name"": ""One Island""
		},
		{
			""Path"": ""knot\\treasure.dat"",
			""Name"": ""Treasure Beach""
		},
		{
			""Path"": ""kolben\\center.dat"",
			""Name"": ""Kolben Tower""
		},
		{
			""Path"": ""kolben\\devoffices.dat"",
			""Name"": ""Kolben Tower""
		},
		{
			""Path"": ""kolben\\elevator.dat"",
			""Name"": ""Kolben Tower""
		},
		{
			""Path"": ""kolben\\lounge.dat"",
			""Name"": ""Kolben Tower""
		},
		{
			""Path"": ""kolben\\servers.dat"",
			""Name"": ""Kolben Tower""
		},
		{
			""Path"": ""lakeofrage\\0.dat"",
			""Name"": ""Lake of Rage""
		},
		{
			""Path"": ""lakeofrage\\1.dat"",
			""Name"": ""Lake of Rage""
		},
		{
			""Path"": ""lavender\\1.dat"",
			""Name"": ""Lavender Town""
		},
		{
			""Path"": ""lavender\\2.dat"",
			""Name"": ""Lavender Town""
		},
		{
			""Path"": ""lavender\\3.dat"",
			""Name"": ""Lavender Town""
		},
		{
			""Path"": ""lavender\\cemetary.dat"",
			""Name"": ""Lavender Town""
		},
		{
			""Path"": ""lavender\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""lavender\\main.dat"",
			""Name"": ""Lavender Town""
		},
		{
			""Path"": ""lavender\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""lavender\\radio.dat"",
			""Name"": ""Lavender Radio Tower""
		},
		{
			""Path"": ""lighthouse\\lighthouse0.dat"",
			""Name"": ""Lighthouse""
		},
		{
			""Path"": ""mahogany\\0.dat"",
			""Name"": ""Mahogany Town""
		},
		{
			""Path"": ""mahogany\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""mahogany\\shop.dat"",
			""Name"": ""Mahogany Town""
		},
		{
			""Path"": ""mainmenu\\mainmenu0.dat"",
			""Name"": ""New Bark Town""
		},
		{
			""Path"": ""mainmenu\\mainmenu1.dat"",
			""Name"": ""Cherrygrove City""
		},
		{
			""Path"": ""mainmenu\\mainmenu2.dat"",
			""Name"": ""Azalea Town""
		},
		{
			""Path"": ""mainmenu\\_mainmenu3.dat"",
			""Name"": ""Violet City""
		},
		{
			""Path"": ""mtember\\braille1.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\braille2.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\exterior.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\peak.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\peakp1.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\peakp2.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\peakp3.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\secretpath1.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\secretpath2.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\secretpath3.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\secretpath4.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\secretpath5.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtember\\secretpath6.dat"",
			""Name"": ""Mt. Ember""
		},
		{
			""Path"": ""mtmoon\\main.dat"",
			""Name"": ""Mt. Moon""
		},
		{
			""Path"": ""mtmoon\\north.dat"",
			""Name"": ""Mt. Moon""
		},
		{
			""Path"": ""mtmoon\\shop.dat"",
			""Name"": ""Mt. Moon""
		},
		{
			""Path"": ""mtmoon\\south.dat"",
			""Name"": ""Mt. Moon""
		},
		{
			""Path"": ""mtmoon\\square.dat"",
			""Name"": ""Mt. Moon""
		},
		{
			""Path"": ""mtmortar\\mtmortarbf1.dat"",
			""Name"": ""Mt. Mortar""
		},
		{
			""Path"": ""mtmortar\\mtmortarf1.dat"",
			""Name"": ""Mt. Mortar""
		},
		{
			""Path"": ""mtmortar\\mtmortarf2.dat"",
			""Name"": ""Mt. Mortar""
		},
		{
			""Path"": ""mtmortar\\mtmortarmain.dat"",
			""Name"": ""Mt. Mortar""
		},
		{
			""Path"": ""mtsilver\\1f.dat"",
			""Name"": ""Mt. Silver""
		},
		{
			""Path"": ""mtsilver\\exterior.dat"",
			""Name"": ""Mt. Silver""
		},
		{
			""Path"": ""nationalpark\\contest.dat"",
			""Name"": ""National Park""
		},
		{
			""Path"": ""nationalpark\\nationalpark0.dat"",
			""Name"": ""National Park""
		},
		{
			""Path"": ""nationalpark\\nationalpark1.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""nationalpark\\nationalpark2.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""olivine\\001.dat"",
			""Name"": ""Olivine City""
		},
		{
			""Path"": ""olivine\\002.dat"",
			""Name"": ""Olivine City""
		},
		{
			""Path"": ""olivine\\003.dat"",
			""Name"": ""Olivine City""
		},
		{
			""Path"": ""olivine\\Cafe.dat"",
			""Name"": ""Olivine Cafe""
		},
		{
			""Path"": ""olivine\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""olivine\\dock.dat"",
			""Name"": ""Olivine City""
		},
		{
			""Path"": ""olivine\\dock_entrance.dat"",
			""Name"": ""Olivine City""
		},
		{
			""Path"": ""olivine\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""pallet\\Blue.dat"",
			""Name"": ""Blue's House""
		},
		{
			""Path"": ""pallet\\main.dat"",
			""Name"": ""Pallet Town""
		},
		{
			""Path"": ""pallet\\oaklab.dat"",
			""Name"": ""Oak's lab""
		},
		{
			""Path"": ""pallet\\Red1.dat"",
			""Name"": ""Red's House""
		},
		{
			""Path"": ""pallet\\Red2.dat"",
			""Name"": ""Red's House""
		},
		{
			""Path"": ""pewter\\1.dat"",
			""Name"": ""Pewter City""
		},
		{
			""Path"": ""pewter\\2.dat"",
			""Name"": ""Pewter City""
		},
		{
			""Path"": ""pewter\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""pewter\\main.dat"",
			""Name"": ""Pewter City""
		},
		{
			""Path"": ""pewter\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""pewter\\museum.dat"",
			""Name"": ""Pewter City""
		},
		{
			""Path"": ""quest\\dock.dat"",
			""Name"": ""Seven Island""
		},
		{
			""Path"": ""rocketbase\\rocketbase1.dat"",
			""Name"": ""Team Rocket HQ""
		},
		{
			""Path"": ""rocketbase\\rocketbase2.dat"",
			""Name"": ""Team Rocket HQ""
		},
		{
			""Path"": ""rocketbase\\rocketbase3.dat"",
			""Name"": ""Team Rocket HQ""
		},
		{
			""Path"": ""rocktunnel\\bf1.dat"",
			""Name"": ""Rock Tunnel""
		},
		{
			""Path"": ""rocktunnel\\bf2.dat"",
			""Name"": ""Rock Tunnel""
		},
		{
			""Path"": ""route39\\barn.dat"",
			""Name"": ""Route 39""
		},
		{
			""Path"": ""route39\\house.dat"",
			""Name"": ""Route 39""
		},
		{
			""Path"": ""routes\\berryhouse.dat"",
			""Name"": ""Route 30""
		},
		{
			""Path"": ""routes\\daycare.dat"",
			""Name"": ""Daycare""
		},
		{
			""Path"": ""routes\\route1.dat"",
			""Name"": ""Route 1""
		},
		{
			""Path"": ""routes\\route10.dat"",
			""Name"": ""Route 10""
		},
		{
			""Path"": ""routes\\route11.dat"",
			""Name"": ""Route 11""
		},
		{
			""Path"": ""routes\\route12.dat"",
			""Name"": ""Route 12""
		},
		{
			""Path"": ""routes\\route13.dat"",
			""Name"": ""Route 13""
		},
		{
			""Path"": ""routes\\route14.dat"",
			""Name"": ""Route 14""
		},
		{
			""Path"": ""routes\\route15.dat"",
			""Name"": ""Route 15""
		},
		{
			""Path"": ""routes\\route16.dat"",
			""Name"": ""Route 16""
		},
		{
			""Path"": ""routes\\route17.dat"",
			""Name"": ""Route 17""
		},
		{
			""Path"": ""routes\\route17offset.dat"",
			""Name"": ""Route 17""
		},
		{
			""Path"": ""routes\\route18.dat"",
			""Name"": ""Route 18""
		},
		{
			""Path"": ""routes\\route18offset.dat"",
			""Name"": ""Route 17""
		},
		{
			""Path"": ""routes\\route19.dat"",
			""Name"": ""Route 19""
		},
		{
			""Path"": ""routes\\route2.dat"",
			""Name"": ""Route 2""
		},
		{
			""Path"": ""routes\\route20.dat"",
			""Name"": ""Route 20""
		},
		{
			""Path"": ""routes\\route21.dat"",
			""Name"": ""Route 21""
		},
		{
			""Path"": ""routes\\route22.dat"",
			""Name"": ""Route 22""
		},
		{
			""Path"": ""routes\\route24.dat"",
			""Name"": ""Route 24""
		},
		{
			""Path"": ""routes\\route25.dat"",
			""Name"": ""Route 25""
		},
		{
			""Path"": ""routes\\route26.dat"",
			""Name"": ""Route 26""
		},
		{
			""Path"": ""routes\\route27.dat"",
			""Name"": ""Route 27""
		},
		{
			""Path"": ""routes\\route28.dat"",
			""Name"": ""Route 28""
		},
		{
			""Path"": ""routes\\route3.dat"",
			""Name"": ""Route 3""
		},
		{
			""Path"": ""routes\\route34.dat"",
			""Name"": ""Route 34""
		},
		{
			""Path"": ""routes\\route35.dat"",
			""Name"": ""Route 35""
		},
		{
			""Path"": ""routes\\route4.dat"",
			""Name"": ""Route 4""
		},
		{
			""Path"": ""routes\\route40.dat"",
			""Name"": ""Route 40""
		},
		{
			""Path"": ""routes\\route41.dat"",
			""Name"": ""Route 41""
		},
		{
			""Path"": ""routes\\route42.dat"",
			""Name"": ""Route 42""
		},
		{
			""Path"": ""routes\\route43.dat"",
			""Name"": ""Route 43""
		},
		{
			""Path"": ""routes\\route44.dat"",
			""Name"": ""Route 44""
		},
		{
			""Path"": ""routes\\route45.dat"",
			""Name"": ""Route 45""
		},
		{
			""Path"": ""routes\\route46.dat"",
			""Name"": ""Route 46""
		},
		{
			""Path"": ""routes\\route48.dat"",
			""Name"": ""Route 48""
		},
		{
			""Path"": ""routes\\route49.dat"",
			""Name"": ""Route 49""
		},
		{
			""Path"": ""routes\\route5.dat"",
			""Name"": ""Route 5""
		},
		{
			""Path"": ""routes\\route6.dat"",
			""Name"": ""Route 6""
		},
		{
			""Path"": ""routes\\route7.dat"",
			""Name"": ""Route 7""
		},
		{
			""Path"": ""routes\\route8.dat"",
			""Name"": ""Route 8""
		},
		{
			""Path"": ""routes\\route9.dat"",
			""Name"": ""Route 9""
		},
		{
			""Path"": ""routes\\route10\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""routes\\route10\\powerplant.dat"",
			""Name"": ""Power Plant""
		},
		{
			""Path"": ""routes\\route12\\fish.dat"",
			""Name"": ""Route 12""
		},
		{
			""Path"": ""routes\\route16\\1.dat"",
			""Name"": ""Route 16""
		},
		{
			""Path"": ""routes\\route2\\1.dat"",
			""Name"": ""Route 2""
		},
		{
			""Path"": ""routes\\route2\\gate.dat"",
			""Name"": ""Route 2""
		},
		{
			""Path"": ""routes\\route25\\bill.dat"",
			""Name"": ""Route 25""
		},
		{
			""Path"": ""routes\\route26\\0.dat"",
			""Name"": ""Route 26""
		},
		{
			""Path"": ""routes\\route26\\1.dat"",
			""Name"": ""Route 26""
		},
		{
			""Path"": ""routes\\route27\\1.dat"",
			""Name"": ""Route 27""
		},
		{
			""Path"": ""routes\\route47\\1.dat"",
			""Name"": ""Route 47""
		},
		{
			""Path"": ""routes\\route47\\2.dat"",
			""Name"": ""Route 47""
		},
		{
			""Path"": ""routes\\route47\\3.dat"",
			""Name"": ""Route 47""
		},
		{
			""Path"": ""routes\\route47\\c1.dat"",
			""Name"": ""Cliff Cave""
		},
		{
			""Path"": ""routes\\route47\\c2.dat"",
			""Name"": ""Cliff Cave""
		},
		{
			""Path"": ""routes\\route47\\cb.dat"",
			""Name"": ""Cliff Cave""
		},
		{
			""Path"": ""routes\\route5\\1.dat"",
			""Name"": ""Route 5""
		},
		{
			""Path"": ""safarizone\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""safarizone\\gate.dat"",
			""Name"": ""Safari Zone Gate""
		},
		{
			""Path"": ""safarizone\\main.dat"",
			""Name"": ""Safari Zone Gate""
		},
		{
			""Path"": ""safarizone\\areas\\0.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\1.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\10.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\11.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\12.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\13.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\14.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\15.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\16.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\17.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\18.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\19.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\2.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\3.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\4.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\5.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\6.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\7.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\8.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""safarizone\\areas\\9.dat"",
			""Name"": ""Safari Zone""
		},
		{
			""Path"": ""saffron\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""saffron\\copycat1.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\copycat2.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\dojo.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\main.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""saffron\\psychic.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\rotomroom.dat"",
			""Name"": ""Secret Laboratory""
		},
		{
			""Path"": ""saffron\\silph.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\trainstation.dat"",
			""Name"": ""Saffron City""
		},
		{
			""Path"": ""saffron\\underground0.dat"",
			""Name"": ""Underground Path""
		},
		{
			""Path"": ""seafoam\\bf1.dat"",
			""Name"": ""Seafoam Islands""
		},
		{
			""Path"": ""seafoam\\bf2.dat"",
			""Name"": ""Seafoam Islands""
		},
		{
			""Path"": ""seafoam\\bf3.dat"",
			""Name"": ""Seafoam Islands""
		},
		{
			""Path"": ""seafoam\\bf4.dat"",
			""Name"": ""Seafoam Islands""
		},
		{
			""Path"": ""seafoam\\entEast.dat"",
			""Name"": ""Seafoam Islands""
		},
		{
			""Path"": ""seafoam\\entWest.dat"",
			""Name"": ""Seafoam Islands""
		},
		{
			""Path"": ""slowpokewell\\slowpokewell1f.dat"",
			""Name"": ""Slowpoke Well""
		},
		{
			""Path"": ""slowpokewell\\slowpokewellb1f.dat"",
			""Name"": ""Slowpoke Well""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\bf1.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\captainoutside.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\main.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\1.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\2.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\3.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\4.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\5.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\6.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\7.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\8.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\firsttrip\\cabins\\captain.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\bf1.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\captainoutside.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\main.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\1.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\2.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\3.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\4.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\5.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\6.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\7.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\8.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\olivine\\cabins\\captain.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\bf1.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\captainoutside.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\main.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\1.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\2.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\3.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\4.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\5.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\6.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\7.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\8.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""SSAqua\\vermilion\\cabins\\captain.dat"",
			""Name"": ""S.S. Aqua""
		},
		{
			""Path"": ""tohjofalls\\hideout.dat"",
			""Name"": ""Tohjo Falls""
		},
		{
			""Path"": ""trainmaps\\center.dat"",
			""Name"": ""Train""
		},
		{
			""Path"": ""trainmaps\\fromgoldenrod.dat"",
			""Name"": ""Train""
		},
		{
			""Path"": ""trainmaps\\fromsaffron.dat"",
			""Name"": ""Train""
		},
		{
			""Path"": ""twirl forest\\main.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\0.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\1.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\10.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\11.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\15.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\2.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\3.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\4.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\5.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""twirl forest\\0\\9.dat"",
			""Name"": ""Twirl Forest""
		},
		{
			""Path"": ""unioncave\\unioncavebf1.dat"",
			""Name"": ""Union Cave""
		},
		{
			""Path"": ""unioncave\\unioncavebf2.dat"",
			""Name"": ""Union Cave""
		},
		{
			""Path"": ""unioncave\\unioncavef1.dat"",
			""Name"": ""Union Cave""
		},
		{
			""Path"": ""vermilion\\1.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\2.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\3.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""vermilion\\dock.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\dock_entrance.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\fan.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\main.dat"",
			""Name"": ""Vermilion City""
		},
		{
			""Path"": ""vermilion\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""victoryroad\\1.dat"",
			""Name"": ""Victory Road""
		},
		{
			""Path"": ""victoryroad\\2.dat"",
			""Name"": ""Victory Road""
		},
		{
			""Path"": ""victoryroad\\3.dat"",
			""Name"": ""Victory Road""
		},
		{
			""Path"": ""violet\\001.dat"",
			""Name"": ""Violet City""
		},
		{
			""Path"": ""violet\\002.dat"",
			""Name"": ""Violet City""
		},
		{
			""Path"": ""violet\\school.dat"",
			""Name"": ""Violet City""
		},
		{
			""Path"": ""violet\\sprouttowerf1.dat"",
			""Name"": ""Sprout Tower""
		},
		{
			""Path"": ""violet\\sprouttowerf2.dat"",
			""Name"": ""Sprout Tower""
		},
		{
			""Path"": ""violet\\sprouttowerf3.dat"",
			""Name"": ""Sprout Tower""
		},
		{
			""Path"": ""viridian\\1.dat"",
			""Name"": ""Viridian City""
		},
		{
			""Path"": ""viridian\\center.dat"",
			""Name"": ""Pokemon Center""
		},
		{
			""Path"": ""viridian\\main.dat"",
			""Name"": ""Viridian City""
		},
		{
			""Path"": ""viridian\\mart.dat"",
			""Name"": ""Pokemon Mart""
		},
		{
			""Path"": ""viridian\\trainerhouse\\arena.dat"",
			""Name"": ""Viridian City""
		},
		{
			""Path"": ""viridian\\trainerhouse\\bf.dat"",
			""Name"": ""Viridian City""
		},
		{
			""Path"": ""viridian\\trainerhouse\\main.dat"",
			""Name"": ""Viridian City""
		},
		{
			""Path"": ""v_forest\\main.dat"",
			""Name"": ""Viridian Forest""
		},
		{
			""Path"": ""v_forest\\north.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""v_forest\\south.dat"",
			""Name"": ""Gate""
		},
		{
			""Path"": ""whirlislands\\1fne.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\1fnw.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\1fse.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\1fsw.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\bf1main.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\lugia.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\sc1.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\sc2.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\sc3.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\sc4.dat"",
			""Name"": ""Whirl Islands""
		},
		{
			""Path"": ""whirlislands\\wfall.dat"",
			""Name"": ""Whirl Islands""
		}
	]
}", Text.Encoding.Unicode)
            End If
#End Region
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Function HaveSettingFile(ByVal Files As String) As Boolean
        Try
            If File.Exists(ApplicationDirectory & "\" & Files) Then
                Dim str1 As String = File.ReadAllText(ApplicationDirectory & "\" & Files)
                If String.IsNullOrEmpty(str1) Or String.IsNullOrWhiteSpace(str1) Then
                    Return False
                Else
                    Return True
                End If
            Else
                Return False
            End If
        Catch ex As Exception
            ex.CatchError
            Return False
        End Try
    End Function

    Public Function Token(ByVal Key As String, ParamArray ByVal Variable As String()) As String
        Dim Value As String = Nothing
        If TokenDefination.ContainsKey(Key) Then
            Value = TokenDefination.Item(Key)

            For i As Integer = 0 To Variable.Count - 1
                Value = Value.Replace("{{{" & i.ToString & "}}}", Variable(i))
            Next
        End If

        Return Value
    End Function

#Region "Data List Functions"
#Region "BlackList"
    Public Function IsBlackListed(ByVal Player As Player) As Boolean
        If BlackList Then
            Dim p As BlackList
            If Not Player.isGameJoltPlayer Then
                p = (From i As BlackList In BlackListData Select i Where i.Name = Player.Name And i.GamejoltID = -1).FirstOrDefault
            Else
                p = (From i As BlackList In BlackListData Select i Where i.GamejoltID = Player.GameJoltID).FirstOrDefault
            End If

            If p IsNot Nothing Then
                If p.BanDuration = -1 OrElse Date.Now < p.BanStartTime.AddSeconds(p.BanDuration) Then
                    Return True
                Else
                    BlackListData.Remove(p)
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetBlackListData(ByVal Player As Player) As BlackList
        If IsBlackListed(Player) Then
            Dim p As BlackList
            If Not Player.isGameJoltPlayer Then
                p = (From i As BlackList In BlackListData Select i Where i.Name = Player.Name And i.GamejoltID = -1).FirstOrDefault
            Else
                p = (From i As BlackList In BlackListData Select i Where i.GamejoltID = Player.GameJoltID).FirstOrDefault
            End If
            Return p
        Else
            Return Nothing
        End If
    End Function

    Public Sub AddBlackList(ByVal Player As Player, ByVal Duration As Integer, ByVal Reason As String)
        If Not IsBlackListed(Player) Then
            BlackListData.Add(New BlackList(Player.Name, Player.GameJoltID, Reason, Date.Now, Duration))
            Save()
        Else
            Dim p As BlackList
            If Not Player.isGameJoltPlayer Then
                p = (From i As BlackList In BlackListData Select i Where i.Name = Player.Name And i.GamejoltID = -1).FirstOrDefault
            Else
                p = (From i As BlackList In BlackListData Select i Where i.GamejoltID = Player.GameJoltID).FirstOrDefault
            End If
            p.BanDuration = Duration
            p.BanReason = Reason
            p.BanStartTime = Date.Now
            Save()
        End If
    End Sub

    Public Sub RemoveBlackList(ByVal Player As Player)
        If IsBlackListed(Player) Then
            Dim p As BlackList
            If Not Player.isGameJoltPlayer Then
                p = (From i As BlackList In BlackListData Select i Where i.Name = Player.Name And i.GamejoltID = -1).FirstOrDefault
            Else
                p = (From i As BlackList In BlackListData Select i Where i.GamejoltID = Player.GameJoltID).FirstOrDefault
            End If
            BlackListData.Remove(p)
            Save()
        End If
    End Sub
#End Region

#Region "IPBlackList"
    Public Function IsIPBlackListed(ByVal Player As Player) As Boolean
        If IPBlackList Then
            Dim p As IPBlackList = (From i As IPBlackList In IPBlackListData Select i Where i.IPAddress = CType(Player.PlayerClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString).FirstOrDefault

            If p IsNot Nothing Then
                If p.BanDuration = -1 OrElse Date.Now < p.BanStartTime.AddSeconds(p.BanDuration) Then
                    Return True
                Else
                    IPBlackListData.Remove(p)
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetIPBlackListData(ByVal Player As Player) As IPBlackList
        If IsIPBlackListed(Player) Then
            Dim p As IPBlackList = (From i As IPBlackList In IPBlackListData Select i Where i.IPAddress = CType(Player.PlayerClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString).FirstOrDefault
            Return p
        Else
            Return Nothing
        End If
    End Function

    Public Sub AddIPBlackList(ByVal Player As Player, ByVal Duration As Integer, ByVal Reason As String)
        If Not IsIPBlackListed(Player) Then
            IPBlackListData.Add(New IPBlackList(CType(Player.PlayerClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString, Reason, Date.Now, Duration))
            Save()
        Else
            Dim p As IPBlackList = (From i As IPBlackList In IPBlackListData Select i Where i.IPAddress = CType(Player.PlayerClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString).FirstOrDefault
            p.BanDuration = Duration
            p.BanReason = Reason
            p.BanStartTime = Date.Now
            Save()
        End If
    End Sub

    Public Sub RemoveIPBlackList(ByVal Player As Player)
        If IsIPBlackListed(Player) Then
            Dim p As IPBlackList = (From i As IPBlackList In IPBlackListData Select i Where i.IPAddress = CType(Player.PlayerClient.Client.RemoteEndPoint, IPEndPoint).Address.ToString).FirstOrDefault
            IPBlackListData.Remove(p)
            Save()
        End If
    End Sub
#End Region

#Region "MapFile"
    Public Function GetMapName(ByVal path As String) As String
        Dim Location As MapFileList = (From i As MapFileList In MapFileListData Select i Where i.Path = path).FirstOrDefault

        If Location IsNot Nothing Then
            Return Location.Name
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "MuteList"
    Public Function IsMuted(ByVal Player As Player) As Boolean
        If MuteList Then
            Dim p As MuteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As MuteList In MuteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As MuteList In MuteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If

            If p IsNot Nothing Then
                If p.MuteDuration = -1 OrElse Date.Now < p.MuteStartTime.AddSeconds(p.MuteDuration) Then
                    Return True
                Else
                    MuteListData.Remove(p)
                    Return False
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetMuteListData(ByVal Player As Player) As MuteList
        If IsMuted(Player) Then
            Dim p As MuteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As MuteList In MuteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As MuteList In MuteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            Return p
        Else
            Return Nothing
        End If
    End Function

    Public Sub AddMute(ByVal Player As Player, ByVal Duration As Integer, ByVal Reason As String)
        If Not IsMuted(Player) Then
            MuteListData.Add(New MuteList(Player.Name, Player.GameJoltID, Reason, Date.Now, Duration))
            Save()
        Else
            Dim p As MuteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As MuteList In MuteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As MuteList In MuteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            p.MuteDuration = Duration
            p.MuteReason = Reason
            p.MuteStartTime = Date.Now
            Save()
        End If
    End Sub

    Public Sub RemoveMute(ByVal Player As Player)
        If IsMuted(Player) Then
            Dim p As MuteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As MuteList In MuteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As MuteList In MuteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            MuteListData.Remove(p)
            Save()
        End If
    End Sub
#End Region

#Region "Online Setting"
    Public Function GetSetting(ByVal Player As Player) As OnlineSetting
        If Player.isGameJoltPlayer Then
            Dim p As OnlineSetting = (From i As OnlineSetting In OnlineSettingListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault

            If p IsNot Nothing Then
                Return p
            Else
                Return Nothing
            End If
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "Operator List"
    Public Function IsOperator(ByVal Player As Player) As Boolean
        If OperatorList Then
            Dim p As OperatorList
            If Not Player.isGameJoltPlayer Then
                p = (From i As OperatorList In OperatorListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As OperatorList In OperatorListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If

            If p IsNot Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            If Player.GameJoltID = 116016 OrElse Player.GameJoltID = 222452 Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    Public Function GetOperatorListData(ByVal Player As Player) As OperatorList
        If IsOperator(Player) Then
            Dim p As OperatorList
            If Not Player.isGameJoltPlayer Then
                p = (From i As OperatorList In OperatorListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As OperatorList In OperatorListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            Return p
        Else
            Return Nothing
        End If
    End Function

    Public Function OperatorPermission(ByVal Player As Player) As Integer
        If IsOperator(Player) Then
            Dim p As OperatorList
            If Not Player.isGameJoltPlayer Then
                p = (From i As OperatorList In OperatorListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As OperatorList In OperatorListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If

            Return p.OperatorLevel
        Else
            If Player.GameJoltID = 116016 OrElse Player.GameJoltID = 222452 Then
                Return Player.OperatorPermission.Administrator
            Else
                Return Player.OperatorPermission.Player
            End If
        End If
    End Function

    Public Sub AddOperator(ByVal Player As Player, ByVal Permission As Player.OperatorPermission, ByVal Reason As String)
        If Not IsOperator(Player) Then
            OperatorListData.Add(New OperatorList(Player.Name, Player.GameJoltID, Reason, Permission))
            Save()
        Else
            Dim p As OperatorList
            If Not Player.isGameJoltPlayer Then
                p = (From i As OperatorList In OperatorListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As OperatorList In OperatorListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            p.OperatorLevel = Permission
            p.OperatorReason = Reason
            Save()
        End If
    End Sub

    Public Sub RemoveOperator(ByVal Player As Player)
        If IsOperator(Player) Then
            Dim p As OperatorList
            If Not Player.isGameJoltPlayer Then
                p = (From i As OperatorList In OperatorListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As OperatorList In OperatorListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            OperatorListData.Remove(p)
            Save()
        End If
    End Sub
#End Region

#Region "Swear Infraction Filter List"
    Public Function PlayerSweared(ByVal text As String) As Boolean
        If SwearInfractionList Then
            For Each i As SwearInfractionFilterList In SwearInfractionFilterListData
                If Regex.IsMatch(text, i.Regex) Then
                    If Regex.Match(text, i.Regex).Length = i.Word.Count Then
                        Return True
                    End If
                End If
            Next
            Return False
        Else
            Return False
        End If
    End Function

    Public Function SwearedWord(ByVal text As String) As String
        If PlayerSweared(text) Then
            For Each i As SwearInfractionFilterList In SwearInfractionFilterListData
                If Regex.IsMatch(text, i.Regex) Then
                    If Regex.Match(text, i.Regex).Length = i.Word.Count Then
                        Return i.Word
                    End If
                End If
            Next
            Return Nothing
        Else
            Return Nothing
        End If
    End Function
#End Region

#Region "Swear Infraction"
    Public Function IsSwearInfracted(ByVal Player As Player) As Boolean
        If SwearInfractionList Then
            Dim p As SwearInfractionList
            If Not Player.isGameJoltPlayer Then
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If

            If p IsNot Nothing Then
                If SwearInfractionReset = -1 OrElse Date.Now < p.StartTime.AddDays(SwearInfractionReset) Then
                    Return True
                Else
                    p.Points = 0
                    Save()
                    Return True
                End If
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetSwearInfractionData(ByVal Player As Player) As SwearInfractionList
        If IsSwearInfracted(Player) Then
            Dim p As SwearInfractionList
            If Not Player.isGameJoltPlayer Then
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            Return p
        Else
            Return Nothing
        End If
    End Function

    Public Sub AddSwearInfracted(ByVal Player As Player, ByVal InfractionPoint As Integer)
        If Not IsSwearInfracted(Player) Then
            SwearInfractionListData.Add(New SwearInfractionList(Player.Name, Player.GameJoltID, InfractionPoint, 0, Date.Now))
        Else
            Dim p As SwearInfractionList
            If Not Player.isGameJoltPlayer Then
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If

            p.Points += InfractionPoint

            If SwearInfractionCap > 0 AndAlso p.Points >= SwearInfractionCap Then
                p.Points = 0
                p.Muted += 1
                p.StartTime = Date.Now
                AddMute(Player, 60 * 60, "You have sweared too much. Take a break please!")
            End If
            Save()
        End If
    End Sub

    Public Sub RemoveSwearInfracted(ByVal Player As Player)
        If IsSwearInfracted(Player) Then
            Dim p As SwearInfractionList
            If Not Player.isGameJoltPlayer Then
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            SwearInfractionListData.Remove(p)
            Save()
        End If
    End Sub

    Public Sub ResetSwearInfracted(ByVal Player As Player)
        If IsSwearInfracted(Player) Then
            Dim p As SwearInfractionList
            If Not Player.isGameJoltPlayer Then
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As SwearInfractionList In SwearInfractionListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            p.Points = 0
            p.StartTime = Date.Now
            Save()
        End If
    End Sub
#End Region

#Region "WhiteList"
    Public Function IsWhiteListed(ByVal Player As Player) As Boolean
        If WhiteList Then
            Dim p As WhiteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As WhiteList In WhiteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As WhiteList In WhiteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If

            If p IsNot Nothing Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If
    End Function

    Public Function GetWhiteListData(ByVal Player As Player) As WhiteList
        If IsWhiteListed(Player) Then
            Dim p As WhiteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As WhiteList In WhiteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As WhiteList In WhiteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            Return p
        Else
            Return Nothing
        End If
    End Function

    Public Sub AddWhiteList(ByVal Player As Player, ByVal Reason As String)
        If Not IsWhiteListed(Player) Then
            WhiteListData.Add(New WhiteList(Player.Name, Player.GameJoltID, Reason))
            Save()
        Else
            Dim p As WhiteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As WhiteList In WhiteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As WhiteList In WhiteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            p.WhiteListReason = Reason
            Save()
        End If
    End Sub

    Public Sub RemoveWhiteList(ByVal Player As Player)
        If IsWhiteListed(Player) Then
            Dim p As WhiteList
            If Not Player.isGameJoltPlayer Then
                p = (From i As WhiteList In WhiteListData Select i Where i.Name = Player.Name And i.GameJoltID = -1).FirstOrDefault
            Else
                p = (From i As WhiteList In WhiteListData Select i Where i.GameJoltID = Player.GameJoltID).FirstOrDefault
            End If
            WhiteListData.Remove(p)
            Save()
        End If
    End Sub
#End Region
#End Region

End Class