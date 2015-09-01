Imports System.Windows.Forms

Public Class Ban

    Public Shared PlayerName As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'If String.IsNullOrEmpty(DurationText.Text) Or String.IsNullOrWhiteSpace(DurationText.Text) Then
        '    If String.IsNullOrEmpty(ReasonText.Text) Or String.IsNullOrWhiteSpace(ReasonText.Text) Then
        '        OldPackage.HandleChatMessageCommand("/ban " + PlayerName, Nothing, True)
        '    Else
        '        OldPackage.HandleChatMessageCommand("/ban " + PlayerName + " " + Integer.MaxValue.ToString + " " + ReasonText.Text, Nothing, True)
        '    End If
        'Else
        '    If String.IsNullOrEmpty(ReasonText.Text) Or String.IsNullOrWhiteSpace(ReasonText.Text) Then
        '        OldPackage.HandleChatMessageCommand("/ban " + PlayerName + " " + DurationText.Text, Nothing, True)
        '    Else
        '        OldPackage.HandleChatMessageCommand("/ban " + PlayerName + " " + DurationText.Text + " " + ReasonText.Text, Nothing, True)
        '    End If
        'End If
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

End Class
