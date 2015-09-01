Public Class OperatorList
    Public Property Name As String
    Public Property GameJoltID As Integer
    Public Property OperatorReason As String
    Public Property OperatorLevel As Integer

    Public Sub New(ByVal Name As String, ByVal GameJoltID As Integer, ByVal OperatorReason As String, ByVal OperatorLevel As Integer)
        Me.Name = Name
        Me.GameJoltID = GameJoltID
        Me.OperatorReason = OperatorReason
        Me.OperatorLevel = OperatorLevel
    End Sub
End Class