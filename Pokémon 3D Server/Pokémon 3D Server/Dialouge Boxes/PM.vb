Imports System.Windows.Forms

Public Class PM

    Public Shared PlayerName As String

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        'OldPlayer.SendData(OldPackage.CreateData(OldPackage.PackageTypes.ChatMessage, -1, Reason.Text), OldPlayer.PlayerClient(OldPlayer.Name.IndexOf(PlayerName)))
        'OldServerClient.QueueMessage("You have sent a PM to " + PlayerName + " with the following content: " + Reason.Text, Main.LogType.Info)
        'Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub Reason_TextChanged(sender As Object, e As EventArgs) Handles Reason.TextChanged
        If Reason.Text.Length > 0 Then
            OK_Button.Enabled = True
        Else
            OK_Button.Enabled = False
        End If
    End Sub
End Class
