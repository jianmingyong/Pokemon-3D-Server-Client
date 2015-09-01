Imports System.Windows.Forms

Public Class Kick

    Public Shared PlayerName As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'If String.IsNullOrEmpty(Reason.Text) Or String.IsNullOrWhiteSpace(Reason.Text) Then
        '    OldPackage.HandleChatMessageCommand("/kick " + PlayerName, Nothing, True)
        'Else
        '    OldPackage.HandleChatMessageCommand("/kick " + PlayerName + " " + Reason.Text, Nothing, True)
        'End If
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

End Class
