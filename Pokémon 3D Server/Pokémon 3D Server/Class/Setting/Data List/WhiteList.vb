Public Class WhiteList
    Public Property Name As String
    Public Property GameJoltID As Integer
    Public Property WhiteListReason As String

    Public Sub New(ByVal Name As String, ByVal GameJoltID As Integer, ByVal WhiteListReason As String)
        Me.Name = Name
        Me.GameJoltID = GameJoltID
        Me.WhiteListReason = WhiteListReason
    End Sub
End Class