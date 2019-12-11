<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.progBar = New System.Windows.Forms.ProgressBar()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtUrl = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDestDir = New System.Windows.Forms.TextBox()
        Me.cmdBrowse = New System.Windows.Forms.Button()
        Me.cmdPasteUrl = New System.Windows.Forms.Button()
        Me.cmdLoadPage = New System.Windows.Forms.Button()
        Me.wbc = New System.Windows.Forms.WebBrowser()
        Me.cmdDownloadFiles = New System.Windows.Forms.Button()
        Me.txtFileExt = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lstFiles = New System.Windows.Forms.CheckedListBox()
        Me.chkAll = New System.Windows.Forms.CheckBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'progBar
        '
        Me.progBar.Location = New System.Drawing.Point(7, 660)
        Me.progBar.Name = "progBar"
        Me.progBar.Size = New System.Drawing.Size(1097, 23)
        Me.progBar.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "URL of Page"
        '
        'txtUrl
        '
        Me.txtUrl.Location = New System.Drawing.Point(82, 15)
        Me.txtUrl.Name = "txtUrl"
        Me.txtUrl.Size = New System.Drawing.Size(404, 20)
        Me.txtUrl.TabIndex = 3
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 43)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(105, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Destination Directory"
        '
        'txtDestDir
        '
        Me.txtDestDir.Location = New System.Drawing.Point(123, 40)
        Me.txtDestDir.Name = "txtDestDir"
        Me.txtDestDir.Size = New System.Drawing.Size(312, 20)
        Me.txtDestDir.TabIndex = 5
        '
        'cmdBrowse
        '
        Me.cmdBrowse.Location = New System.Drawing.Point(441, 39)
        Me.cmdBrowse.Name = "cmdBrowse"
        Me.cmdBrowse.Size = New System.Drawing.Size(51, 23)
        Me.cmdBrowse.TabIndex = 6
        Me.cmdBrowse.Text = "Browse"
        Me.cmdBrowse.UseVisualStyleBackColor = True
        '
        'cmdPasteUrl
        '
        Me.cmdPasteUrl.Location = New System.Drawing.Point(491, 14)
        Me.cmdPasteUrl.Name = "cmdPasteUrl"
        Me.cmdPasteUrl.Size = New System.Drawing.Size(51, 23)
        Me.cmdPasteUrl.TabIndex = 7
        Me.cmdPasteUrl.Text = "Paste"
        Me.cmdPasteUrl.UseVisualStyleBackColor = True
        '
        'cmdLoadPage
        '
        Me.cmdLoadPage.Location = New System.Drawing.Point(350, 117)
        Me.cmdLoadPage.Name = "cmdLoadPage"
        Me.cmdLoadPage.Size = New System.Drawing.Size(94, 23)
        Me.cmdLoadPage.TabIndex = 8
        Me.cmdLoadPage.Text = "Load Web Page"
        Me.cmdLoadPage.UseVisualStyleBackColor = True
        '
        'wbc
        '
        Me.wbc.Location = New System.Drawing.Point(504, 52)
        Me.wbc.MinimumSize = New System.Drawing.Size(20, 20)
        Me.wbc.Name = "wbc"
        Me.wbc.Size = New System.Drawing.Size(594, 605)
        Me.wbc.TabIndex = 9
        '
        'cmdDownloadFiles
        '
        Me.cmdDownloadFiles.Location = New System.Drawing.Point(337, 146)
        Me.cmdDownloadFiles.Name = "cmdDownloadFiles"
        Me.cmdDownloadFiles.Size = New System.Drawing.Size(121, 23)
        Me.cmdDownloadFiles.TabIndex = 10
        Me.cmdDownloadFiles.Text = "Download 0 Files"
        Me.cmdDownloadFiles.UseVisualStyleBackColor = True
        '
        'txtFileExt
        '
        Me.txtFileExt.Location = New System.Drawing.Point(147, 64)
        Me.txtFileExt.Name = "txtFileExt"
        Me.txtFileExt.Size = New System.Drawing.Size(258, 20)
        Me.txtFileExt.TabIndex = 12
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(133, 13)
        Me.Label3.TabIndex = 11
        Me.Label3.Text = "File Extension to download"
        '
        'lstFiles
        '
        Me.lstFiles.CheckOnClick = True
        Me.lstFiles.FormattingEnabled = True
        Me.lstFiles.Location = New System.Drawing.Point(15, 186)
        Me.lstFiles.Name = "lstFiles"
        Me.lstFiles.Size = New System.Drawing.Size(462, 469)
        Me.lstFiles.TabIndex = 13
        '
        'chkAll
        '
        Me.chkAll.AutoSize = True
        Me.chkAll.Location = New System.Drawing.Point(18, 169)
        Me.chkAll.Name = "chkAll"
        Me.chkAll.Size = New System.Drawing.Size(15, 14)
        Me.chkAll.TabIndex = 14
        Me.chkAll.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 149)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 13)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Files to Download"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1110, 687)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.chkAll)
        Me.Controls.Add(Me.lstFiles)
        Me.Controls.Add(Me.txtFileExt)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdDownloadFiles)
        Me.Controls.Add(Me.wbc)
        Me.Controls.Add(Me.cmdLoadPage)
        Me.Controls.Add(Me.cmdPasteUrl)
        Me.Controls.Add(Me.cmdBrowse)
        Me.Controls.Add(Me.txtDestDir)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtUrl)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.progBar)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents progBar As System.Windows.Forms.ProgressBar
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents txtUrl As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtDestDir As System.Windows.Forms.TextBox
    Friend WithEvents cmdBrowse As System.Windows.Forms.Button
    Friend WithEvents cmdPasteUrl As System.Windows.Forms.Button
    Friend WithEvents cmdLoadPage As System.Windows.Forms.Button
    Friend WithEvents wbc As System.Windows.Forms.WebBrowser
    Friend WithEvents cmdDownloadFiles As System.Windows.Forms.Button
    Friend WithEvents txtFileExt As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents lstFiles As System.Windows.Forms.CheckedListBox
    Friend WithEvents chkAll As System.Windows.Forms.CheckBox
    Friend WithEvents Label4 As System.Windows.Forms.Label

End Class
