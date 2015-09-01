Public Class SwearInfractionList
    Public Property Name As String
    Public Property GameJoltID As Integer
    Public Property Points As Integer
    Public Property Muted As Integer
    Public Property StartTime As Date

    Public Sub New(ByVal Name As String, ByVal GameJoltID As Integer, ByVal Points As Integer, ByVal Muted As Integer, ByVal StartTime As Date)
        Me.Name = Name
        Me.GameJoltID = GameJoltID
        Me.Points = Points
        Me.Muted = Muted
        Me.StartTime = StartTime
    End Sub
End Class