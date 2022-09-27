Public Class FormStoryTeller

    Public ImageStory As Image

    Private Sub FormStoryTeller_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Loading the current Image
        Me.BackgroundImage = ImageStory

        TimerAnimate.Enabled = True
        TimerAnimate.Interval = 1
        TimerAnimate.Start()
        Me.AllowTransparency = True
        Me.Opacity = 0.0
        Me.ButtonClose.Visible = False
    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub

    Private Sub TimerAnimate_Tick(sender As Object, e As EventArgs) Handles TimerAnimate.Tick
        Me.Opacity = Me.Opacity + 0.08
        If (Me.Opacity >= 1) Then
            TimerAnimate.Stop()
            Me.ButtonClose.Visible = True
        End If
    End Sub
End Class