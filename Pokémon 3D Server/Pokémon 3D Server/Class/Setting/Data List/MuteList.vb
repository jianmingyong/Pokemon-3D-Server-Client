Public Class MuteList
    Public Property Name As String
    Public Property GameJoltID As Integer
    Public Property MuteReason As String
    Public Property MuteStartTime As Date
    Public Property MuteDuration As Integer

    Public ReadOnly Property BanRemainingTime As String
        Get
            Dim BanRemainingTimes As TimeSpan
            Dim BanRemainingText As String = Nothing

            If MuteDuration > 0 Then
                BanRemainingTimes = MuteStartTime.AddSeconds(MuteDuration) - Date.Now
                If BanRemainingTimes.Days = 1 Then
                    BanRemainingText &= BanRemainingTimes.Days & " day "
                ElseIf BanRemainingTimes.Days > 1
                    BanRemainingText &= BanRemainingTimes.Days & " days "
                End If

                If BanRemainingTimes.Hours = 1 AndAlso BanRemainingTimes.Days = 0 Then
                    BanRemainingText &= BanRemainingTimes.Hours & " hour "
                ElseIf BanRemainingTimes.Hours > 1 AndAlso BanRemainingTimes.Days = 0 Then
                    BanRemainingText &= BanRemainingTimes.Hours & " hours "
                End If

                If BanRemainingTimes.Minutes = 1 AndAlso BanRemainingTimes.Days = 0 Then
                    BanRemainingText &= BanRemainingTimes.Minutes & " minute "
                ElseIf BanRemainingTimes.Minutes > 1 AndAlso BanRemainingTimes.Days = 0 Then
                    BanRemainingText &= BanRemainingTimes.Minutes & " minutes "
                End If

                If BanRemainingTimes.Seconds = 1 AndAlso BanRemainingTimes.Days = 0 Then
                    BanRemainingText &= BanRemainingTimes.Seconds & " second "
                ElseIf BanRemainingTimes.Seconds > 1 AndAlso BanRemainingTimes.Days = 0 Then
                    BanRemainingText &= BanRemainingTimes.Seconds & " seconds "
                End If
            Else
                BanRemainingText = "Permanent"
            End If
            Return BanRemainingText
        End Get
    End Property

    Public Sub New(ByVal Name As String, ByVal GameJoltID As Integer, ByVal MuteReason As String, ByVal MuteStartTime As Date, ByVal MuteDuration As Integer)
        Me.Name = Name
        Me.GameJoltID = GameJoltID
        Me.MuteReason = MuteReason
        Me.MuteStartTime = MuteStartTime
        Me.MuteDuration = MuteDuration
    End Sub
End Class