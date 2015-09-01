<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Downloader
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.ProgressBar1 = New System.Windows.Forms.ProgressBar()
        Me.Downloader_FullStatus = New BrightIdeasSoftware.ObjectListView()
        Me.OlvColumn1 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn2 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.OlvColumn3 = CType(New BrightIdeasSoftware.OLVColumn(), BrightIdeasSoftware.OLVColumn)
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.Downloader_URL = New System.Windows.Forms.Label()
        Me.TableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
        Me.Downloader_Cancel = New System.Windows.Forms.Button()
        Me.Downloader_StartPause = New System.Windows.Forms.Button()
        Me.TableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Downloader_Status = New System.Windows.Forms.Label()
        Me.Downloader_FileSize = New System.Windows.Forms.Label()
        Me.Downloader_Downloaded = New System.Windows.Forms.Label()
        Me.Downloader_TransferRate = New System.Windows.Forms.Label()
        Me.Downloader_TimeLeft = New System.Windows.Forms.Label()
        Me.Downloader_ResumeCompatibility = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.Downloader_FullStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.TableLayoutPanel3.SuspendLayout()
        Me.TableLayoutPanel4.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(3, 3)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(686, 217)
        Me.TabControl1.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.TableLayoutPanel2)
        Me.TabPage1.Location = New System.Drawing.Point(4, 25)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(678, 188)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Download Status"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.ProgressBar1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Downloader_FullStatus, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.TableLayoutPanel3, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 42.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(692, 497)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'ProgressBar1
        '
        Me.ProgressBar1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ProgressBar1.Location = New System.Drawing.Point(3, 226)
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(686, 18)
        Me.ProgressBar1.TabIndex = 1
        '
        'Downloader_FullStatus
        '
        Me.Downloader_FullStatus.AllColumns.Add(Me.OlvColumn1)
        Me.Downloader_FullStatus.AllColumns.Add(Me.OlvColumn2)
        Me.Downloader_FullStatus.AllColumns.Add(Me.OlvColumn3)
        Me.Downloader_FullStatus.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.OlvColumn1, Me.OlvColumn2, Me.OlvColumn3})
        Me.Downloader_FullStatus.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Downloader_FullStatus.Location = New System.Drawing.Point(3, 289)
        Me.Downloader_FullStatus.Name = "Downloader_FullStatus"
        Me.Downloader_FullStatus.Size = New System.Drawing.Size(686, 205)
        Me.Downloader_FullStatus.TabIndex = 2
        Me.Downloader_FullStatus.UseCompatibleStateImageBehavior = False
        Me.Downloader_FullStatus.View = System.Windows.Forms.View.Details
        '
        'OlvColumn1
        '
        Me.OlvColumn1.Text = "No."
        '
        'OlvColumn2
        '
        Me.OlvColumn2.Text = "Downloaded"
        Me.OlvColumn2.Width = 200
        '
        'OlvColumn3
        '
        Me.OlvColumn3.Text = "Info"
        Me.OlvColumn3.Width = 250
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 1
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel2.Controls.Add(Me.Downloader_URL, 0, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.TableLayoutPanel4, 0, 1)
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(6, 6)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 2
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(666, 179)
        Me.TableLayoutPanel2.TabIndex = 0
        '
        'Downloader_URL
        '
        Me.Downloader_URL.AutoSize = True
        Me.Downloader_URL.Dock = System.Windows.Forms.DockStyle.Left
        Me.Downloader_URL.Location = New System.Drawing.Point(3, 0)
        Me.Downloader_URL.Name = "Downloader_URL"
        Me.Downloader_URL.Size = New System.Drawing.Size(106, 17)
        Me.Downloader_URL.TabIndex = 0
        Me.Downloader_URL.Text = "Download_URL"
        Me.Downloader_URL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TableLayoutPanel3
        '
        Me.TableLayoutPanel3.ColumnCount = 3
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel3.Controls.Add(Me.Downloader_StartPause, 1, 0)
        Me.TableLayoutPanel3.Controls.Add(Me.Downloader_Cancel, 2, 0)
        Me.TableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel3.Location = New System.Drawing.Point(3, 250)
        Me.TableLayoutPanel3.Name = "TableLayoutPanel3"
        Me.TableLayoutPanel3.RowCount = 1
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel3.Size = New System.Drawing.Size(686, 33)
        Me.TableLayoutPanel3.TabIndex = 3
        '
        'Downloader_Cancel
        '
        Me.Downloader_Cancel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Downloader_Cancel.Location = New System.Drawing.Point(517, 3)
        Me.Downloader_Cancel.Name = "Downloader_Cancel"
        Me.Downloader_Cancel.Size = New System.Drawing.Size(166, 27)
        Me.Downloader_Cancel.TabIndex = 0
        Me.Downloader_Cancel.Text = "Cancel"
        Me.Downloader_Cancel.UseVisualStyleBackColor = True
        '
        'Downloader_StartPause
        '
        Me.Downloader_StartPause.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Downloader_StartPause.Location = New System.Drawing.Point(346, 3)
        Me.Downloader_StartPause.Name = "Downloader_StartPause"
        Me.Downloader_StartPause.Size = New System.Drawing.Size(165, 27)
        Me.Downloader_StartPause.TabIndex = 1
        Me.Downloader_StartPause.Text = "Pause"
        Me.Downloader_StartPause.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel4
        '
        Me.TableLayoutPanel4.ColumnCount = 2
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75.0!))
        Me.TableLayoutPanel4.Controls.Add(Me.Downloader_ResumeCompatibility, 1, 5)
        Me.TableLayoutPanel4.Controls.Add(Me.Downloader_TimeLeft, 1, 4)
        Me.TableLayoutPanel4.Controls.Add(Me.Downloader_TransferRate, 1, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.Downloader_Downloaded, 1, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.Label2, 0, 1)
        Me.TableLayoutPanel4.Controls.Add(Me.Label3, 0, 2)
        Me.TableLayoutPanel4.Controls.Add(Me.Label4, 0, 3)
        Me.TableLayoutPanel4.Controls.Add(Me.Label5, 0, 4)
        Me.TableLayoutPanel4.Controls.Add(Me.Label6, 0, 5)
        Me.TableLayoutPanel4.Controls.Add(Me.Downloader_Status, 1, 0)
        Me.TableLayoutPanel4.Controls.Add(Me.Downloader_FileSize, 1, 1)
        Me.TableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel4.Location = New System.Drawing.Point(3, 20)
        Me.TableLayoutPanel4.Name = "TableLayoutPanel4"
        Me.TableLayoutPanel4.RowCount = 6
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.0!))
        Me.TableLayoutPanel4.Size = New System.Drawing.Size(660, 156)
        Me.TableLayoutPanel4.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 39)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Status"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(61, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "File Size"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(3, 62)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(86, 17)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Downloaded"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(3, 85)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(96, 17)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Transfer Rate"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(3, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(67, 17)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Time Left"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(3, 131)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(143, 17)
        Me.Label6.TabIndex = 5
        Me.Label6.Text = "Resume Compatibility"
        '
        'Downloader_Status
        '
        Me.Downloader_Status.AutoSize = True
        Me.Downloader_Status.Dock = System.Windows.Forms.DockStyle.Left
        Me.Downloader_Status.ForeColor = System.Drawing.Color.Blue
        Me.Downloader_Status.Location = New System.Drawing.Point(168, 0)
        Me.Downloader_Status.Name = "Downloader_Status"
        Me.Downloader_Status.Size = New System.Drawing.Size(48, 39)
        Me.Downloader_Status.TabIndex = 6
        Me.Downloader_Status.Text = "Pause"
        Me.Downloader_Status.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'Downloader_FileSize
        '
        Me.Downloader_FileSize.AutoSize = True
        Me.Downloader_FileSize.Location = New System.Drawing.Point(168, 39)
        Me.Downloader_FileSize.Name = "Downloader_FileSize"
        Me.Downloader_FileSize.Size = New System.Drawing.Size(68, 17)
        Me.Downloader_FileSize.TabIndex = 7
        Me.Downloader_FileSize.Text = "0.000 MB"
        '
        'Downloader_Downloaded
        '
        Me.Downloader_Downloaded.AutoSize = True
        Me.Downloader_Downloaded.Location = New System.Drawing.Point(168, 62)
        Me.Downloader_Downloaded.Name = "Downloader_Downloaded"
        Me.Downloader_Downloaded.Size = New System.Drawing.Size(68, 17)
        Me.Downloader_Downloaded.TabIndex = 8
        Me.Downloader_Downloaded.Text = "0.000 MB"
        '
        'Downloader_TransferRate
        '
        Me.Downloader_TransferRate.AutoSize = True
        Me.Downloader_TransferRate.Location = New System.Drawing.Point(168, 85)
        Me.Downloader_TransferRate.Name = "Downloader_TransferRate"
        Me.Downloader_TransferRate.Size = New System.Drawing.Size(81, 17)
        Me.Downloader_TransferRate.TabIndex = 9
        Me.Downloader_TransferRate.Text = "0 Bytes/sec"
        '
        'Downloader_TimeLeft
        '
        Me.Downloader_TimeLeft.AutoSize = True
        Me.Downloader_TimeLeft.Location = New System.Drawing.Point(168, 108)
        Me.Downloader_TimeLeft.Name = "Downloader_TimeLeft"
        Me.Downloader_TimeLeft.Size = New System.Drawing.Size(42, 17)
        Me.Downloader_TimeLeft.TabIndex = 10
        Me.Downloader_TimeLeft.Text = "0 sec"
        '
        'Downloader_ResumeCompatibility
        '
        Me.Downloader_ResumeCompatibility.AutoSize = True
        Me.Downloader_ResumeCompatibility.Location = New System.Drawing.Point(168, 131)
        Me.Downloader_ResumeCompatibility.Name = "Downloader_ResumeCompatibility"
        Me.Downloader_ResumeCompatibility.Size = New System.Drawing.Size(38, 17)
        Me.Downloader_ResumeCompatibility.TabIndex = 11
        Me.Downloader_ResumeCompatibility.Text = "True"
        '
        'Downloader
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(692, 497)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "Downloader"
        Me.Text = "Downloader"
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.Downloader_FullStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.TableLayoutPanel3.ResumeLayout(False)
        Me.TableLayoutPanel4.ResumeLayout(False)
        Me.TableLayoutPanel4.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents Downloader_URL As Label
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents Downloader_FullStatus As BrightIdeasSoftware.ObjectListView
    Friend WithEvents OlvColumn1 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn2 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents OlvColumn3 As BrightIdeasSoftware.OLVColumn
    Friend WithEvents TableLayoutPanel3 As TableLayoutPanel
    Friend WithEvents Downloader_StartPause As Button
    Friend WithEvents Downloader_Cancel As Button
    Friend WithEvents TableLayoutPanel4 As TableLayoutPanel
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Downloader_ResumeCompatibility As Label
    Friend WithEvents Downloader_TimeLeft As Label
    Friend WithEvents Downloader_TransferRate As Label
    Friend WithEvents Downloader_Downloaded As Label
    Friend WithEvents Downloader_Status As Label
    Friend WithEvents Downloader_FileSize As Label
End Class
