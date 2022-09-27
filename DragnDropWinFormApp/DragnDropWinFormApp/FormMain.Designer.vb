<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FormMain
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
        Me.components = New System.ComponentModel.Container()
        Me.ButtonClose = New System.Windows.Forms.Button()
        Me.AnimationTimer = New System.Windows.Forms.Timer(Me.components)
        Me.FaderTimer = New System.Windows.Forms.Timer(Me.components)
        Me.ButtonRestart = New System.Windows.Forms.Button()
        Me.GlowTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'ButtonClose
        '
        Me.ButtonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonClose.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonClose.Location = New System.Drawing.Point(784, 522)
        Me.ButtonClose.Name = "ButtonClose"
        Me.ButtonClose.Size = New System.Drawing.Size(132, 41)
        Me.ButtonClose.TabIndex = 0
        Me.ButtonClose.Text = "Close"
        Me.ButtonClose.UseVisualStyleBackColor = True
        '
        'AnimationTimer
        '
        '
        'FaderTimer
        '
        '
        'ButtonRestart
        '
        Me.ButtonRestart.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonRestart.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ButtonRestart.Location = New System.Drawing.Point(646, 522)
        Me.ButtonRestart.Name = "ButtonRestart"
        Me.ButtonRestart.Size = New System.Drawing.Size(132, 41)
        Me.ButtonRestart.TabIndex = 1
        Me.ButtonRestart.Text = "Restart"
        Me.ButtonRestart.UseVisualStyleBackColor = True
        '
        'GlowTimer
        '
        Me.GlowTimer.Interval = 1
        '
        'FormMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.CancelButton = Me.ButtonClose
        Me.ClientSize = New System.Drawing.Size(928, 575)
        Me.Controls.Add(Me.ButtonRestart)
        Me.Controls.Add(Me.ButtonClose)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FormMain"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Match Current Packs to the New Packs"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents ButtonClose As Button
    Friend WithEvents AnimationTimer As Timer
    Friend WithEvents FaderTimer As Timer
    Friend WithEvents ButtonRestart As Button
    Friend WithEvents GlowTimer As Timer
End Class
