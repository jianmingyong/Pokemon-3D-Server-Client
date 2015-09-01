Imports System.Net

Public Class World
    Public _Season As Integer
    Public _Weather As Integer
    Public _CurrentTime As Date
    Public _TimeOffset As Integer = 0

    Public Property Season As Integer
        Set(value As Integer)
            If value < 0 Then
                _Season = 0
            ElseIf value > 3
                _Season = 3
            Else
                _Season = value
            End If
        End Set
        Get
            Return _Season
        End Get
    End Property
    Public Property Weather As Integer
        Set(value As Integer)
            If value < 0 Then
                _Weather = 0
            ElseIf value > 9
                _Weather = 9
            Else
                _Weather = value
            End If
        End Set
        Get
            Return _Weather
        End Get
    End Property
    Public Property CurrentTime As String
        Set(value As String)
            _CurrentTime = New DateTime(Date.Now.Year, Date.Now.Month, Date.Now.Day, CInt(value.GetSplit(0, ",")), CInt(value.GetSplit(1, ",")), CInt(value.GetSplit(2, ",")))
        End Set
        Get
            Return _CurrentTime.Hour & "," & _CurrentTime.Minute & "," & _CurrentTime.Second
        End Get
    End Property
    Public Property TimeOffset As Integer
        Set(value As Integer)
            If IsNumeric(value) Then
                _TimeOffset = value
            Else
                _TimeOffset = 0
            End If
        End Set
        Get
            Return _TimeOffset
        End Get
    End Property
    Public Property CanUpdate As Boolean = True

    Public Enum SeasonTypes
        Winter = 0
        Spring = 1
        Summer = 2
        Fall = 3
        Random = -1
        DefaultSeason = -2
        Custom = -3
        [Nothing] = -4
    End Enum
    Public Enum WeatherTypes
        Clear = 0
        Rain = 1
        Snow = 2
        Underwater = 3
        Sunny = 4
        Fog = 5
        Thunderstorm = 6
        Sandstorm = 7
        Ash = 8
        Blizzard = 9
        Random = -1
        DefaultWeather = -2
        Custom = -3
        Real = -5
        [Nothing] = -4
    End Enum

#Region "Class of Advanced Weather System"
    Public Class SeasonMonth
        '<!-- Advanced World Property -->
        '<!-- SeasonMonth Set the season based On real world Date (Overrides Global season)-->
        '<!-- Syntax (Index of season on each month separated by "|"): Jan | Feb | March | April | May | June | July | Aug | Sep | Oct | Nov | Dec -->
        '<!-- Winter = 0 | Spring = 1 | Summer = 2 | Fall = 3 | Random = -1 | Default Season = -2 -->
        '<!-- Each Month cannot have an empty season Or the program might fail to process it. -->

        Public Property SeasonData As String
        Public Property SeasonList As New List(Of Integer)

        Public Sub New()
            SeasonData = Nothing
            SeasonList = New List(Of Integer)
        End Sub

        Public Sub New(ByVal Data As String)
            If Data.Contains("|") AndAlso Data.SplitCount >= 12 Then
                SeasonData = Data
            End If
        End Sub

        Public Function GetSeason() As List(Of Integer)
            SeasonList = New List(Of Integer)
            Dim MonthIndex As Integer = Date.Now.Month - 1
            If SeasonData.GetSplit(MonthIndex).Contains(",") Then
                Try
                    For Each season As String In SeasonData.GetSplit(MonthIndex).Split(","c)
                        SeasonList.Add(CInt(season))
                    Next
                Catch ex As Exception
                    SeasonList.Add(-2)
                End Try
            Else
                Try
                    SeasonList.Add(CInt(SeasonData.GetSplit(MonthIndex)))
                Catch ex As Exception
                    SeasonList.Add(-2)
                End Try
            End If
            Return SeasonList
        End Function
    End Class

    Public Class WeatherSeason
        '<!-- WeatherSeason: Set what weather will it be For Each season. (Overrides Global weather)-->
        '<!-- Syntax (Index of weather on each season separated by "|"): Winter | Spring | Summer | Fall -->
        '<!-- Clear = 0 | Rain = 1 | Snow = 2 | Underwater = 3 | Sunny = 4 | Fog = 5 | Thunderstorm = 6 | Sandstorm = 7 | Ash = 8 | Blizzard = 9 | Random = -1 | Default Weather = -2 -->

        Public Property WeatherData As String
        Public Property WeatherList As List(Of Integer)

        Public Sub New()
            WeatherData = Nothing
            WeatherList = New List(Of Integer)
        End Sub

        Public Sub New(ByVal Data As String)
            If Data.Contains("|") AndAlso Data.SplitCount >= 4 Then
                WeatherData = Data
            End If
        End Sub

        Public Function GetWeather() As List(Of Integer)
            WeatherList = New List(Of Integer)
            If WeatherData.GetSplit(Main.World.Season).Contains(",") Then
                Try
                    For Each weather As String In WeatherData.GetSplit(Main.World.Season).Split(","c)
                        WeatherList.Add(CInt(weather))
                    Next
                Catch ex As Exception
                    WeatherList.Add(-2)
                End Try
            Else
                Try
                    WeatherList.Add(CInt(WeatherData.GetSplit(Main.World.Season)))
                Catch ex As Exception
                    WeatherList.Add(-2)
                End Try
            End If
            Return WeatherList
        End Function
    End Class
#End Region

    Public Sub New()
        _CurrentTime = Date.Now
        TimeOffset = 0
    End Sub

    Public Sub Update(sender As Object, e As EventArgs)
        If CanUpdate Then
            Dim WeekOfYear As Integer = CInt(Date.Now.DayOfYear - (Date.Now.DayOfWeek - DayOfWeek.Monday) / 7.0 + 1.0)

            _CurrentTime = Date.Now

            If Main.Setting.Season = SeasonTypes.DefaultSeason Then
                Select Case WeekOfYear Mod 4
                    Case 0
                        Season = SeasonTypes.Fall
                    Case 1
                        Season = SeasonTypes.Winter
                    Case 2
                        Season = SeasonTypes.Spring
                    Case 3
                        Season = SeasonTypes.Summer
                    Case Else
                        Season = SeasonTypes.Summer
                End Select
            ElseIf Main.Setting.Season = SeasonTypes.Random
                Season = Random(0, 3)
            ElseIf Main.Setting.Season = SeasonTypes.Custom
                Season = GetCustomSeason()
            Else
                Season = Main.Setting.Season
            End If

            If Main.Setting.Weather = WeatherTypes.DefaultWeather Then
                Dim Random As Integer = New Random().Next(1, 100)
                Select Case Season
                    Case SeasonTypes.Winter
                        If Random <= 20 Then
                            Weather = WeatherTypes.Rain
                        ElseIf Random > 20 And Random <= 50 Then
                            Weather = WeatherTypes.Clear
                        Else
                            Weather = WeatherTypes.Snow
                        End If
                    Case SeasonTypes.Spring
                        If Random <= 5 Then
                            Weather = WeatherTypes.Snow
                        ElseIf Random > 5 And Random <= 40 Then
                            Weather = WeatherTypes.Rain
                        Else
                            Weather = WeatherTypes.Clear
                        End If
                    Case SeasonTypes.Summer
                        If Random <= 10 Then
                            Weather = WeatherTypes.Rain
                        Else
                            Weather = WeatherTypes.Clear
                        End If
                    Case SeasonTypes.Fall
                        If Random <= 5 Then
                            Weather = WeatherTypes.Snow
                        ElseIf Random > 5 And Random <= 80 Then
                            Weather = WeatherTypes.Rain
                        Else
                            Weather = WeatherTypes.Clear
                        End If
                End Select
            ElseIf Main.Setting.Weather = WeatherTypes.Random
                Weather = Random(0, 9)
            ElseIf Main.Setting.Weather = WeatherTypes.Custom
                Weather = GetCustomWeather()
            Else
                Weather = Main.Setting.Weather
            End If

            Main.Main.QueueMessage("World.vb: Current Season: " & GetSeasonName(Season) & " | Current Weather: " & GetWeatherName(Weather) & " | Current Time: " & _CurrentTime.AddSeconds(TimeOffset).ToString, Main.LogType.Info)

            Dim CurrentPlayer As List(Of Player) = Main.Player
            For Each Player As Player In CurrentPlayer
                Main.ServerClient.SendData(New Package(Package.PackageTypes.WorldData, GenerateWorld(Player), Player.PlayerClient))
            Next
        End If
    End Sub

    Public Function GenerateWorld(ByVal Player As Player) As List(Of String)
        Dim list As New List(Of String)

        If Main.Setting.DoDayCycle Then
            CurrentTime = _CurrentTime.AddSeconds(TimeOffset).Hour.ToString & "," & _CurrentTime.AddSeconds(TimeOffset).Minute.ToString & "," & _CurrentTime.AddSeconds(TimeOffset).Second.ToString
        Else
            CurrentTime = "12,0,0"
        End If

        If Player.isGameJoltPlayer Then
            Dim PlayerSetting As OnlineSetting = Main.Setting.GetSetting(Player)

            Dim Season As Integer = 0
            Dim Weather As Integer = 0

            If PlayerSetting Is Nothing Then
                Season = Me.Season
                Weather = Me.Weather
            Else
                Season = GenerateSeason(PlayerSetting.Season)
                Weather = GenerateWeather(PlayerSetting.Weather, Season)
            End If

            list.Add(Season.ToString)
            list.Add(Weather.ToString)
            list.Add(CurrentTime.ToString)
        Else
            list.Add(Season.ToString)
            list.Add(Weather.ToString)
            list.Add(CurrentTime.ToString)
        End If

        Return list
    End Function

    Public Function GenerateSeason(ByVal Season As Integer) As Integer
        Dim WeekOfYear As Integer = CInt(Date.Now.DayOfYear - (Date.Now.DayOfWeek - DayOfWeek.Monday) / 7.0 + 1.0)

        If Season = SeasonTypes.DefaultSeason Then
            Select Case WeekOfYear Mod 4
                Case 0
                    Return SeasonTypes.Fall
                Case 1
                    Return SeasonTypes.Winter
                Case 2
                    Return SeasonTypes.Spring
                Case 3
                    Return SeasonTypes.Summer
                Case Else
                    Return SeasonTypes.Summer
            End Select
        ElseIf Season = SeasonTypes.Random
            Return Random(0, 3)
        ElseIf Season = SeasonTypes.Custom
            Return GetCustomSeason()
        ElseIf Season = SeasonTypes.Nothing
            Return Me.Season
        Else
            Return Season
        End If
    End Function

    Public Function GenerateWeather(ByVal Weather As Integer, ByVal Season As Integer) As Integer
        Dim WeekOfYear As Integer = CInt(Date.Now.DayOfYear - (Date.Now.DayOfWeek - DayOfWeek.Monday) / 7.0 + 1.0)

        If Weather = WeatherTypes.DefaultWeather Then
            Dim Random As Integer = New Random().Next(1, 100)
            Select Case Season
                Case SeasonTypes.Winter
                    If Random <= 20 Then
                        Return WeatherTypes.Rain
                    ElseIf Random > 20 And Random <= 50 Then
                        Return WeatherTypes.Clear
                    Else
                        Return WeatherTypes.Snow
                    End If
                Case SeasonTypes.Spring
                    If Random <= 5 Then
                        Return WeatherTypes.Snow
                    ElseIf Random > 5 And Random <= 40 Then
                        Return WeatherTypes.Rain
                    Else
                        Return WeatherTypes.Clear
                    End If
                Case SeasonTypes.Summer
                    If Random <= 10 Then
                        Return WeatherTypes.Rain
                    Else
                        Return WeatherTypes.Clear
                    End If
                Case SeasonTypes.Fall
                    If Random <= 5 Then
                        Return WeatherTypes.Snow
                    ElseIf Random > 5 And Random <= 80 Then
                        Return WeatherTypes.Rain
                    Else
                        Return WeatherTypes.Clear
                    End If
                Case Else
                    Return WeatherTypes.Clear
            End Select
        ElseIf Weather = WeatherTypes.Random
            Return Random(0, 9)
        ElseIf Weather = WeatherTypes.Custom
            Return GetCustomWeather()
        ElseIf Weather = WeatherTypes.Nothing
            Return Me.Weather
        Else
            Return Weather
        End If
    End Function

    Private Function GetCustomSeason() As Integer
        Try
            Return Main.Setting.SeasonMonth.GetSeason(Random(0, Main.Setting.SeasonMonth.SeasonList.Count - 1))
        Catch ex As Exception
            ex.CatchError
        End Try
        Return SeasonTypes.Winter
    End Function

    Private Function GetCustomWeather() As Integer
        Try
            Return Main.Setting.WeatherSeason.GetWeather(Random(0, Main.Setting.WeatherSeason.WeatherList.Count - 1))
        Catch ex As Exception
            ex.CatchError
        End Try
        Return WeatherTypes.Clear
    End Function

    Public Function GetSeasonName(ByVal Season As Integer) As String
        Select Case Season
            Case SeasonTypes.Winter
                Return "Winter"
            Case SeasonTypes.Spring
                Return "Spring"
            Case SeasonTypes.Summer
                Return "Summer"
            Case SeasonTypes.Fall
                Return "Fall"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function GetWeatherName(ByVal Weather As Integer) As String
        Select Case Weather
            Case WeatherTypes.Ash
                Return "Ash"
            Case WeatherTypes.Blizzard
                Return "Blizzard"
            Case WeatherTypes.Clear
                Return "Clear"
            Case WeatherTypes.Fog
                Return "Fog"
            Case WeatherTypes.Rain
                Return "Rain"
            Case WeatherTypes.Sandstorm
                Return "Sandstorm"
            Case WeatherTypes.Snow
                Return "Snow"
            Case WeatherTypes.Sunny
                Return "Sunny"
            Case WeatherTypes.Thunderstorm
                Return "Thunderstorm"
            Case WeatherTypes.Underwater
                Return "Underwater"
            Case Else
                Return Nothing
        End Select
    End Function
End Class