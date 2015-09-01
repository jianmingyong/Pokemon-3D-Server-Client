Imports System.IO
Imports Newtonsoft.Json

#If RELEASEVERSION Then
Public Class OnlineSetting

    '<!-- Pokémon 3D Server Client Setting File -->
    Public Property Name As String
    Public Property GameJoltID As Integer
    Private ReadOnly Property LastUpdated As Date = Date.Now

    '<!-- World Property -->
    Public Property Season As Integer
    Public Property Weather As Integer

    '<!-- Mute List Data -->
    Public Property MuteListData As New List(Of MuteList)

    Public Sub New(ByVal Name As String, ByVal GameJoltID As Integer)
        Me.Name = Name
        Me.GameJoltID = GameJoltID
        Season = World.SeasonTypes.Nothing
        Weather = World.WeatherTypes.Nothing
        Load()
    End Sub

    Private Sub Load()
        Try
            If HaveSettingFile(GameJoltID) Then
                Dim Reader As JsonTextReader = New JsonTextReader(New StringReader(File.ReadAllText(Main.Setting.ApplicationDirectory & "\Data\UserSetting\" & Me.GameJoltID.ToString & ".json")))

                Reader.DateParseHandling = DateParseHandling.DateTime
                Reader.FloatParseHandling = FloatParseHandling.Double

                Dim StartObjectDepth As Integer = -1
                Dim ObjectPropertyName As String = Nothing
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
                        If Not TempPropertyName = Nothing AndAlso Not TempPropertyName = ObjectPropertyName Then
                            ObjectPropertyName = TempPropertyName
                            TempPropertyName = Nothing
                        End If
                    ElseIf Reader.TokenType = JsonToken.EndObject
                        If StartObjectDepth = 1 AndAlso String.Equals(ObjectPropertyName, "MuteListData", StringComparison.OrdinalIgnoreCase) Then
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

                    If StartObjectDepth = 1 AndAlso String.Equals(ObjectPropertyName, "World Property", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Season", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Season = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: Season does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "Weather", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    Weather = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: Weather does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    ElseIf StartObjectDepth = 1 AndAlso String.Equals(ObjectPropertyName, "MuteListData", StringComparison.OrdinalIgnoreCase) Then
                        If Reader.TokenType = JsonToken.Boolean OrElse Reader.TokenType = JsonToken.Bytes OrElse Reader.TokenType = JsonToken.Date OrElse Reader.TokenType = JsonToken.Float OrElse Reader.TokenType = JsonToken.Integer OrElse Reader.TokenType = JsonToken.Null OrElse Reader.TokenType = JsonToken.String Then
                            If String.Equals(PropertyName, "Name", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    Name = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: MuteListData.Name does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "GameJoltID", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    GameJoltID = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: MuteListData.GameJoltID does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteReason", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.String Then
                                    MuteReason = Reader.Value.ToString
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: MuteListData.MuteReason does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteStartTime", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Date Then
                                    MuteStartTime = CDate(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: MuteListData.MuteStartTime does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            ElseIf String.Equals(PropertyName, "MuteDuration", StringComparison.OrdinalIgnoreCase) Then
                                If Reader.TokenType = JsonToken.Integer Then
                                    MuteDuration = CInt(Reader.Value)
                                Else
                                    Main.Main.QueueMessage("OnlineSetting.vb: MuteListData.MuteDuration does not match the require type. Default value will be used.", Main.LogType.Warning)
                                End If
                            End If
                        End If
                    End If
                End While
            End If
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Sub Save()
        Try
            If Not Directory.Exists(Main.Setting.ApplicationDirectory & "\Data\UserSetting") Then
                Directory.CreateDirectory(Main.Setting.ApplicationDirectory & "\Data\UserSetting")
            End If

            Dim List As String = Nothing
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

            File.WriteAllText(Main.Setting.ApplicationDirectory & "\Data\UserSetting\" & GameJoltID.ToString & ".json",
"{
    ""Pokémon 3D Server Client Setting File"":
    {
        ""Name"": """ & Name & """,
        ""GameJoltID"": " & GameJoltID.ToString & ",
        ""LastUpdate"": """ & LastUpdated.ToString("yyyy-MM-ddTHH\:mm\:ss.fffffffK") & """
    },

    ""World Property"":
    {
        ""Season"": " & Season & ",
        ""Weather"": " & Weather & "
    }

    ""MuteListData"":
    [
" & List & "
    ]
}", Text.Encoding.Unicode)

        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Private Function HaveSettingFile(ByVal GameJoltID As Integer) As Boolean
        Try
            If File.Exists(Main.Setting.ApplicationDirectory & "\Data\OnlineSetting\" & GameJoltID.ToString & ".json") Then
                Dim str1 As String = File.ReadAllText(Main.Setting.ApplicationDirectory & "\Data\OnlineSetting\" & GameJoltID.ToString & ".json")
                If String.IsNullOrWhiteSpace(str1) Then
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

#Region "MuteList"
    Public Function IsMuted(ByVal Player As Player) As Boolean
        If Main.Setting.MuteList Then
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
End Class
#Else
Public Class OldOnlineSetting

    Public Shared Weather As String
    Public Shared Season As String
    Public Shared MuteListItem As New List(Of String)

    Public Shared Sub Load(ByVal PlayerID As Integer)
        Try
            If HaveSettingFile(PlayerID) Then
                Dim Flag As Boolean = False
                Dim Flag2 As Boolean = False
                Dim FlagData As String = Nothing
                For Each Lines As String In File.ReadAllLines(OldSetting.ApplicationDirectory + "\UserSetting\" + PlayerID.ToString + ".dat")
                    If Lines.Contains("<!--") Or Lines.Contains("/*") Then
                        Flag = True
                    End If
                    If Flag = True Then
                        If Lines.Contains("-->") Or Lines.Contains("*/") Then
                            Flag = False
                        End If
                    End If
                    If Not Flag And Not Lines.Contains("-->") Then
                        If Lines.Contains("Weather|") Then
                            Weather = Functions.GetSplit(Lines, 1, "|")
                        ElseIf Lines.Contains("Season|") Then
                            Season = Functions.GetSplit(Lines, 1, "|")
                        End If

                        If Lines.Contains("MuteListData|") Then
                            Flag2 = True
                            FlagData = "MuteListData"
                        End If

                        If Flag2 Then
                            If String.Equals(FlagData, "MuteListData", StringComparison.OrdinalIgnoreCase) And Not Lines.Contains("MuteListData|") Then
                                If Not (String.IsNullOrEmpty(Lines) Or String.IsNullOrWhiteSpace(Lines)) Then
                                    MuteListItem.Add(Lines)
                                End If
                            End If
                        End If
                    End If
                Next
            End If

            For Each Data As String In MuteListItem
                ' Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time
                If CType(Functions.GetSplit(Data, 3, "|"), Date).AddSeconds(CDbl(Functions.GetSplit(Data, 4, "|"))) <= Date.Now Then
                    MuteListItem.Remove(Data)
                End If
            Next

        Catch ex As Exception
            Functions.CatchError(ex)
        End Try
    End Sub

    Public Shared Sub Save(ByVal PlayerID As Integer)
        Try
            Dim Settings As New List(Of String)
            Settings.Add("<!-- Pokémon 3D Server Client Setting File -->")
            Settings.Add("Name|" + OldPlayer.Name(OldPlayer.GameJoltID.IndexOf(PlayerID)))
            Settings.Add("GameJolt ID|" + PlayerID.ToString)
            Settings.Add("Last Updated|" + Date.Now.ToString)
            Settings.Add("")
            Settings.Add("<!-- World Property -->")
            Settings.Add("<!-- Weather: Set the global weather. -->")
            Settings.Add("<!-- Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 | Server Default Weather = -3 -->")
            Settings.Add("Weather|" + Weather)
            Settings.Add("")
            Settings.Add("<!-- Season: Set the global season. -->")
            Settings.Add("<!-- Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 | Server Default Season = -3 -->")
            Settings.Add("Season|" + Season)
            Settings.Add("")
            Settings.Add("<!-- MuteList Data -->")
            Settings.Add("<!-- Name | GameJolt ID | Mute Reason | Mute Start Time | Mute Time -->")
            Settings.Add("MuteListData|")
            Settings.AddRange(MuteListItem)

            If Not Directory.Exists(OldSetting.ApplicationDirectory + "\UserSetting\") Then
                Directory.CreateDirectory(OldSetting.ApplicationDirectory + "\UserSetting\")
            End If
            File.WriteAllLines(OldSetting.ApplicationDirectory + "\UserSetting\" + PlayerID.ToString + ".dat", Settings)
        Catch ex As Exception
            Functions.CatchError(ex)
        End Try
    End Sub

    Public Shared Function HaveSettingFile(ByVal PlayerID As Integer) As Boolean
        Try
            If File.Exists(OldSetting.ApplicationDirectory + "\UserSetting\" + PlayerID.ToString + ".dat") Then
                Dim str1 As String = File.ReadAllText(OldSetting.ApplicationDirectory + "\UserSetting\" + PlayerID.ToString + ".dat")
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
End Class
#End If