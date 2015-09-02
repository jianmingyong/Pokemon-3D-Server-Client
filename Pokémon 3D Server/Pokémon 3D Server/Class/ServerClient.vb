Imports System.Net
Imports System.Net.Sockets
Imports System.IO

Public Class ServerClient

    Public ServerClientStatus As Status
    Public ThreadCollection As New List(Of Threading.Thread)
    Public TimerCollection As New List(Of Timer)

    Private IPEndPoint As IPEndPoint
    Private Listener As TcpListener
    Private Client As TcpClient

    Public Enum Status
        Started
        Stopped
    End Enum

    Public Sub New() : End Sub

    Public Sub Port(ByVal Port As Integer)
        IPEndPoint = New IPEndPoint(IPAddress.Any, Port)
    End Sub

    Public Sub Start()
        ' Start Listening
        Listener = New TcpListener(IPEndPoint)
        Listener.Start()

        Dim Thread As New Threading.Thread(AddressOf ThreadStartListening)
        Thread.IsBackground = True
        Thread.Start()
        ThreadCollection.Add(Thread)

        Dim Timer As New Timer
        Dim Timer2 As New Timer

        AddHandler Timer.Tick, AddressOf StartListening
        AddHandler Timer2.Tick, AddressOf Main.World.Update

        Timer.Interval = 1000
        Timer.Start()
        Timer2.Interval = 1000 * 60 * 60
        Timer2.Start()

        TimerCollection.AddRange({Timer, Timer2})

        Main.World.Update(Nothing, Nothing)

        ServerClientStatus = Status.Started

        Main.Main.QueueMessage("ServerClient.vb: Server Client started.", Main.LogType.Info)
            If Main.Setting.OfflineMode Then
                Main.Main.QueueMessage("ServerClient.vb: Players with offline profile can join the server.", Main.LogType.Info)
            End If
            Main.Main.QueueMessage("ServerClient.vb: Server Started. Players can join using the following address: " & Main.Setting.IPAddress & ":" & Main.Setting.Port.ToString & " (Global), " & GetPrivateIP() & ":" & Main.Setting.Port.ToString & " (Local) and with the following gamemode: " & Main.Setting.GameMode, Main.LogType.Info)
    End Sub

    Public Sub [Stop]()
        ' Abort ReListening
        TimerCollection(0).Stop()

        ' Abort Updating World
        TimerCollection(1).Stop()

        ServerClientStatus = Status.Stopped

        Main.Main.QueueMessage("ServerClient.vb: Server Client stopped.", Main.LogType.Info)
    End Sub

    Public Sub [Resume](ByVal Port As Integer)
        ' Resume ReListening
        Listener.Stop()
        Me.Port(Port)
        Listener = New TcpListener(IPEndPoint)
        Listener.Start()
        TimerCollection(0).Start()

        ' Resume Updating World
        TimerCollection(1).Start()

        Main.World.Update(Nothing, Nothing)

        ServerClientStatus = Status.Started

        Main.Main.QueueMessage("ServerClient.vb: Server Client started.", Main.LogType.Info)
            If Main.Setting.OfflineMode Then
                Main.Main.QueueMessage("ServerClient.vb: Players with offline profile can join the server.", Main.LogType.Info)
            End If
            Main.Main.QueueMessage("ServerClient.vb: Server Started. Players can join using the following address: " & Main.Setting.IPAddress & ":" & Main.Setting.Port.ToString & " (Global), " & GetPrivateIP() & ":" & Main.Setting.Port.ToString & " (Local) and with the following gamemode: " & Main.Setting.GameMode, Main.LogType.Info)
    End Sub

    Private Sub StartListening(sender As Object, e As EventArgs)
        If ThreadCollection.Count = 0 Then
            Dim Thread As New Threading.Thread(AddressOf ThreadStartListening)
            Thread.IsBackground = True
            Thread.Start()
            ThreadCollection.Add(Thread)

            Main.Main.QueueMessage("ServerClient.vb: Server Client restarted.", Main.LogType.Info)
        End If
    End Sub

    Private Sub ThreadStartListening()
        Try
            ServerClientStatus = Status.Started
            Do
                Client = Listener.AcceptTcpClient
                If ServerClientStatus = Status.Stopped Then
                    Client.Close()
                Else
                    Dim StreamReader As New StreamReader(Client.GetStream)
                    Dim ReturnMessage As String = StreamReader.ReadLine
                    Dim Package As New Package(ReturnMessage, Client)
                    Main.Main.QueueMessage("ServerClient.vb: Receive: " & ReturnMessage, Main.LogType.Debug, Client)
                    If Package.IsValid Then
                        Threading.ThreadPool.QueueUserWorkItem(AddressOf Package.Handle)
                    End If
                End If
            Loop
        Catch ex As SocketException
            If Client IsNot Nothing AndAlso Client.Connected Then
                Client.Close()
            End If
            ThreadCollection.RemoveAt(0)
            ServerClientStatus = Status.Stopped
        Catch ex As IOException
            If Client IsNot Nothing AndAlso Client.Connected Then
                Client.Close()
            End If
            ThreadCollection.RemoveAt(0)
            ServerClientStatus = Status.Stopped
        Catch ex As Exception
            ex.CatchError
            If Client IsNot Nothing AndAlso Client.Connected Then
                Client.Close()
            End If
            ThreadCollection.RemoveAt(0)
            ServerClientStatus = Status.Stopped
        End Try
    End Sub

    Public Sub SendData(ByVal p As Package)
        Try
            Dim Package As Package = p
            If Package.Client.Connected Then
                Dim Writer As New StreamWriter(Package.Client.GetStream)
                Writer.WriteLine(Package.ToString)
                Writer.Flush()
                Main.Main.QueueMessage("ServerClient.ThreadSendData.vb: Sent: " & Package.ToString, Main.LogType.Debug, Client)
            End If
        Catch ex As SocketException
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        Catch ex As IOException
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Sub SendOperatorData(ByVal p As Package)
        Try
            Dim Package As Package = p
            Dim CurrentPlayer As List(Of Player) = Main.Player

            For Each Player As Player In CurrentPlayer
                If Main.Setting.OperatorPermission(Player) > Player.OperatorPermission.Player AndAlso Player.PlayerClient IsNot Package.Client Then
                    If Player.PlayerClient.Connected Then
                        Dim Writer As New StreamWriter(Player.PlayerClient.GetStream)
                        Writer.WriteLine(Package.ToString)
                        Writer.Flush()
                    End If
                End If
            Next

            Main.Main.QueueMessage("ServerClient.ThreadSendOperatorData.vb: Sent: " & Package.ToString, Main.LogType.Debug)
        Catch ex As SocketException
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        Catch ex As IOException
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

    Public Sub SendAllData(ByVal p As Package)
        Try
            Dim Package As Package = p
            Dim CurrentPlayer As List(Of Player) = Main.Player

            For Each Player As Player In CurrentPlayer
                If Player.PlayerClient.Connected Then
                    Dim Writer As New StreamWriter(Player.PlayerClient.GetStream)
                    Writer.WriteLine(Package.ToString)
                    Writer.Flush()
                End If
            Next

            Main.Main.QueueMessage("ServerClient.ThreadSendAllData.vb: Sent: " & Package.ToString, Main.LogType.Debug)
        Catch ex As SocketException
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        Catch ex As IOException
            Main.Main.QueueMessage(ex.Message, Main.LogType.Warning)
        Catch ex As Exception
            ex.CatchError
        End Try
    End Sub

End Class