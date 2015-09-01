Public Class MapFileList
    Public Property Path As String
    Public Property Name As String

    Public Sub New(ByVal Path As String, ByVal Name As String)
        Me.Path = Path
        Me.Name = Name
    End Sub
End Class
