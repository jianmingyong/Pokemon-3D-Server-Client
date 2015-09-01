Public Class BlackList
    Public Property Name As String
    Public Property GamejoltID As Integer
    Public Property BanReason As String
    Public Property BanStartTime As Date
    Public Property BanDuration As Integer

    Public ReadOnly Property BanRemainingTime As String
        Get
            Dim BanRemainingTimes As TimeSpan
            Dim BanRemainingText As String = Nothing

            If BanDuration > 0 Then
                BanRemainingTimes = BanStartTime.AddSeconds(BanDuration) - Date.Now
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

    Public Sub New(ByVal Name As String, ByVal GameJoltID As Integer, ByVal BanReason As String, ByVal BanStartTime As Date, ByVal BanDuration As Integer)
        Me.Name = Name
        Me.GamejoltID = GameJoltID
        Me.BanReason = BanReason
        Me.BanStartTime = BanStartTime
        Me.BanDuration = BanDuration
    End Sub
End Class