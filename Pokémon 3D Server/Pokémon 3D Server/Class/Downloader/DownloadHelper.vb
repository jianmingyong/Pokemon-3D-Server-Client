Imports System.Net

Public Class DownloadHelper

    Public Property DownloadQueue As Integer
    Public Property DownloadStatus As DownloadStatuses
    Public Property DownloadSize As Long
    Public Property DownloadRate As Long
    Public Property DownloadTimeLeft As Integer

    Public Property DownloadStartPoint As Long
    Public Property DownloadEndPoint As Long
    Public Property DownloadURL As String
    Public Property DownloadTotalFileSize As Long

    Private Property WebRequest As HttpWebRequest
    Private Property WebResponse As HttpWebResponse

    Public Enum DownloadStatuses
        Downloading
        Paused
        Pending
        [Error]
    End Enum

    Public Sub New(ByVal QueueIndex As Integer, ByVal DownloadStartPoint As Long, ByVal DownloadEndPoint As Long)
        DownloadQueue = QueueIndex
        DownloadStatus = DownloadStatuses.Pending
        DownloadSize = 0
        DownloadRate = 0
        DownloadTimeLeft = 0

        Me.DownloadStartPoint = DownloadStartPoint
        Me.DownloadEndPoint = DownloadEndPoint
    End Sub

    Public Sub Start()

    End Sub

    Private Sub CheckProperty()
        If DownloadStartPoint < 0 Then
            Throw New Exception("Download Start Point is less than zero.")
        End If
        If DownloadEndPoint > DownloadTotalFileSize Then
            Throw New Exception("Download End Point is not valid.")
        End If
    End Sub
End Class