Imports System.Drawing.Imaging

Public Class FormMain

    Dim mOldImageContainer As New List(Of PictureBox)
    Dim mNewImageContainer As New List(Of PictureBox)
    Dim mMatchImageContainer As New List(Of PictureBox)

    Dim mIsDragging As Boolean = False
    Dim mXXX As Integer = 0
    Dim mYYY As Integer = 0

    Dim OldImageRank As New Dictionary(Of Integer, Image)
    Dim NewImageRank As New Dictionary(Of Integer, Image)
    Dim StoryImage As New List(Of Image)
    'For Randomize
    Dim usedNewImages As New List(Of Integer)
    Dim usedOldImages As New List(Of Integer)
    'For retriet animation of Picturebox
    Dim newImageInitialPositions As New List(Of Point)
    Dim speed As Integer

    Dim mCurrentPicture As PictureBox
    Dim mCurrentOldIndex As Integer
    Dim fade As Single

    Dim mCongratsPicture As New PictureBox
    Dim ButtonCongrats As Button = New Button()
    Dim NumberOfOldImages As Integer = 10




    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DoubleBuffered = True
        FaderTimer.Enabled = True
        FaderTimer.Interval = 30
        FaderTimer.Stop()
        AnimationTimer.Enabled = True
        AnimationTimer.Interval = 1
        AnimationTimer.Stop()

        CreateImageDictionaries()
        CreatePictureBoxes(4)
        Me.BackgroundImage = Image.FromFile("background.jpg")
        Me.BackgroundImageLayout = ImageLayout.Center

    End Sub

    Private Sub ButtonClose_Click(sender As Object, e As EventArgs) Handles ButtonClose.Click
        Me.Close()
    End Sub

    Private Sub CreatePictureBoxes(limit As Integer)
        Dim x As Integer, y As Integer, width As Integer, height As Integer, distance As Integer, newImageY As Integer, matchImageY As Integer
        Dim totalWidth As Integer

        newImageY = Me.Height - 280

        width = 135
        height = 170
        distance = width + 100

        totalWidth = width + (distance * (limit - 1))

        x = ((Me.Width / 2) - (totalWidth / 2))
        y = 30

        'New Image Container
        x = x - 10
        y = y + newImageY
        newImageInitialPositions.Clear()
        mNewImageContainer.Clear()
        For index = 0 To limit - 1
            Dim p As New PictureBox
            'Position Picture Box
            p.Visible = True
            p.SizeMode = PictureBoxSizeMode.StretchImage
            'Load Image
            p.Image = RandomizedNewImage()
            p.BackColor = Color.Transparent
            p.SetBounds(x + (index * distance), y, width, height)
            newImageInitialPositions.Add(New Point(x + (index * distance), y))

            Me.Controls.Add(p)
            mNewImageContainer.Add(p)

            'Add handler for drag n drop
            AddHandler p.MouseDown, AddressOf DragStarts
            AddHandler p.MouseMove, AddressOf Draging
            AddHandler p.MouseUp, AddressOf DragStops
        Next
        y = y - newImageY

        'Match Image Container
        x = x - 20
        y = y + 20
        mMatchImageContainer.Clear()
        For index = 0 To limit - 1
            Dim p As New PictureBox
            'Position Picture Box
            p.Visible = False
            p.SizeMode = PictureBoxSizeMode.StretchImage
            p.BackColor = Color.Transparent
            p.SetBounds(x + (index * distance), y, width, height)
            Me.Controls.Add(p)
            mMatchImageContainer.Add(p)
        Next

        'Old Image Container
        x = x + 20
        y = y - 20
        mOldImageContainer.Clear()
        For index = 0 To limit - 1
            Dim p As New PictureBox
            'Position Picture Box

            p.Visible = True
            p.SizeMode = PictureBoxSizeMode.StretchImage
            'Load Image
            p.Image = RandomizedOldImage()
            p.BackColor = Color.Transparent
            p.SetBounds(x + (index * distance), y, width, height)

            Me.Controls.Add(p)
            mOldImageContainer.Add(p)
        Next

        'Loading Images For Match
        For index = 0 To limit - 1
            mMatchImageContainer.ElementAt(index).Image = mOldImageContainer.ElementAt(index).Image.Clone
        Next


        'Setting Congrats Picture Box
        'Position Picture Box
        mCongratsPicture.Visible = False
        mCongratsPicture.SizeMode = PictureBoxSizeMode.StretchImage
        'Load Image        
        mCongratsPicture.BackColor = Color.Transparent
        mCongratsPicture.SetBounds(((Me.Width / 2) - (totalWidth / 2)), Me.Height - (Me.Height / 2), ((Me.Width / 2) - ((Me.Width / 2) - (totalWidth / 2)) + (totalWidth / 2)), Me.Height - (Me.Height / 1.5))

        ButtonCongrats.Text = "COMPLETED"
        ButtonCongrats.Left = mCongratsPicture.Width / 3 + mCongratsPicture.Left
        ButtonCongrats.Top = mCongratsPicture.Height / 1.5 + mCongratsPicture.Top
        ButtonCongrats.Width = mCongratsPicture.Width / 3
        ButtonCongrats.Height = mCongratsPicture.Height / 3
        ButtonCongrats.Font = New Font(ButtonCongrats.Font.FontFamily, 15, ButtonCongrats.Font.Style)
        ButtonCongrats.Visible = False

        'Add handler for drag n drop
        AddHandler ButtonCongrats.Click, AddressOf ButtonClose_Click

        Me.Controls.Add(ButtonCongrats)
        Me.Controls.Add(mCongratsPicture)
    End Sub

    Private Sub DragStops(sender As Object, e As MouseEventArgs)
        mIsDragging = False
        Dim newImage As PictureBox = sender
        Dim p As PictureBox = GetImageOnDropped(e.Location.X + newImage.Left, e.Location.Y + newImage.Top)

        Dim index As Integer = CheckForMatch(p, newImage)
        If index > -1 Then
            mMatchImageContainer.ElementAt(index).Visible = True
            newImage.Visible = False

            fade = 0.9
            mCurrentOldIndex = index
            FaderTimer.Start()
            Dim FormStory As New FormStoryTeller
            Dim rank As Integer
            For Each item In NewImageRank
                If (item.Value.Equals(newImage.Image)) Then
                    rank = item.Key
                    Exit For
                End If
            Next

            'Success Sound
            My.Computer.Audio.Play("matched.wav", AudioPlayMode.Background)

            FormStory.ImageStory = StoryImage.ElementAt(rank - 1)
            FormStory.ShowDialog()

            If (IsFullCompleted()) Then
                mCongratsPicture.Visible = True
                ButtonCongrats.Visible = True
            End If


        Else
            speed = 30
            AnimationTimer.Start()
            'Fail sound
            My.Computer.Audio.Play("unmatched.wav", AudioPlayMode.Background)
        End If

    End Sub

    Private Sub Draging(sender As Object, e As MouseEventArgs)
        If (mIsDragging) Then
            Dim p As PictureBox = mCurrentPicture

            mCurrentPicture.Top = mCurrentPicture.Top + e.Y - mYYY
            mCurrentPicture.Left = mCurrentPicture.Left + e.X - mXXX
        End If

    End Sub

    Private Sub DragStarts(sender As Object, e As MouseEventArgs)
        Dim p As PictureBox = sender
        mCurrentPicture = p
        mIsDragging = True

        mXXX = e.X
        mYYY = e.Y
    End Sub

    Private Function CheckForMatch(oldI As PictureBox, newI As PictureBox) As Integer
        'Send -1 if no match
        If (OldImageRank.ContainsValue(oldI.Image)) Then
            Dim oldRank As Integer, newRank As Integer
            For Each item In OldImageRank
                If (item.Value.Equals(oldI.Image)) Then
                    oldRank = item.Key
                    Exit For
                End If
            Next

            For Each item In NewImageRank
                If (item.Value.Equals(newI.Image)) Then
                    newRank = item.Key
                    Exit For
                End If
            Next

            If (oldRank = newRank) Then
                CheckForMatch = mOldImageContainer.IndexOf(oldI)
                Exit Function
            End If


        End If
        CheckForMatch = -1
    End Function

    Private Function GetImageOnDropped(x As Integer, y As Integer) As PictureBox

        For Each item As PictureBox In mOldImageContainer
            If (item.Left <= x And item.Right >= x And item.Top <= y And item.Bottom >= y) Then
                GetImageOnDropped = item
                Exit Function
            End If
        Next
        GetImageOnDropped = New PictureBox()
    End Function

    Private Sub CreateImageDictionaries()
        'Old Images
        OldImageRank.Add(1, Image.FromFile("imageold1.jpg"))
        OldImageRank.Add(2, Image.FromFile("imageold2.jpg"))
        OldImageRank.Add(3, Image.FromFile("imageold3.jpg"))
        OldImageRank.Add(4, Image.FromFile("imageold4.jpg"))

        'New Images
        NewImageRank.Add(1, Image.FromFile("imagenew1.jpg"))
        NewImageRank.Add(2, Image.FromFile("imagenew2.jpg"))
        NewImageRank.Add(3, Image.FromFile("imagenew3.jpg"))
        NewImageRank.Add(4, Image.FromFile("imagenew4.jpg"))

        'New Images
        StoryImage.Add(Image.FromFile("story1.jpg"))
        StoryImage.Add(Image.FromFile("story2.jpg"))
        StoryImage.Add(Image.FromFile("story3.jpg"))
        StoryImage.Add(Image.FromFile("story4.jpg"))

        'Congrats Image
        mCongratsPicture.Image = Image.FromFile("congrats.jpg")
    End Sub

    Private Function RandomizedNewImage() As Image

        Randomize()
        Dim key As Integer = CInt(Int((4 * Rnd()) + 1))

        While (usedNewImages.Contains(key))
            Randomize()
            key = CInt(Int((4 * Rnd()) + 1))
        End While
        usedNewImages.Add(key)
        RandomizedNewImage = NewImageRank(key)
    End Function

    Private Function RandomizedOldImage() As Image
        Randomize()
        Dim key As Integer = CInt(Int((4 * Rnd()) + 1))

        While (usedOldImages.Contains(key))
            Randomize()
            key = CInt(Int((4 * Rnd()) + 1))
        End While
        usedOldImages.Add(key)
        RandomizedOldImage = OldImageRank(key)
    End Function

    Private Sub MoveToSource()
        Dim index As Integer = mNewImageContainer.IndexOf(mCurrentPicture)
        Dim cPoint As Point = newImageInitialPositions(index)
        If (Not (mCurrentPicture.Left = cPoint.X) Or Not (mCurrentPicture.Top = cPoint.Y)) Then
            Dim x1 As Integer = mCurrentPicture.Left
            Dim y1 As Integer = mCurrentPicture.Top
            Dim x2 As Integer = cPoint.X
            Dim y2 As Integer = cPoint.Y
            Dim ww As Integer = mCurrentPicture.Width

            Dim xx As Double = x1 - x2
            Dim yy As Double = y1 - y2
            Dim m As Double = 0

            Dim radius As Integer = ww / 2
            Dim distance As Integer = (xx * xx) + (yy * yy)
            distance = Math.Sqrt(distance)
            If (20 >= distance) Then
                speed = 0
                mCurrentPicture.Left = cPoint.X
                mCurrentPicture.Top = cPoint.Y
                AnimationTimer.Stop()
            End If

            If (xx = 0) Then
                If (yy > 0) Then
                    mCurrentPicture.Top = mCurrentPicture.Top - speed
                Else
                    mCurrentPicture.Top = mCurrentPicture.Top + speed
                End If
            ElseIf (yy = 0) Then
                If (xx > 0) Then
                    mCurrentPicture.Left = mCurrentPicture.Left - speed
                Else
                    mCurrentPicture.Left = mCurrentPicture.Left + speed
                End If
            Else
                m = yy / xx
                Dim c As Double = 1 / Math.Sqrt((1 + (m * m)))
                Dim s As Double = m / Math.Sqrt((1 + (m * m)))

                If (xx > 0 And yy > 0) Then
                    mCurrentPicture.Left = mCurrentPicture.Left - (speed * c)
                    mCurrentPicture.Top = mCurrentPicture.Top - (speed * s)
                ElseIf (xx < 0 And yy > 0) Then
                    mCurrentPicture.Left = mCurrentPicture.Left + (speed * c)
                    mCurrentPicture.Top = mCurrentPicture.Top + (speed * s)
                ElseIf (xx < 0 And yy < 0) Then
                    mCurrentPicture.Left = mCurrentPicture.Left + (speed * c)
                    mCurrentPicture.Top = mCurrentPicture.Top + (speed * s)
                ElseIf (xx > 0 And yy < 0) Then
                    mCurrentPicture.Left = mCurrentPicture.Left - (speed * c)
                    mCurrentPicture.Top = mCurrentPicture.Top - (speed * s)
                End If

            End If

        End If
    End Sub

    Private Sub AnimationTimer_Tick(sender As Object, e As EventArgs) Handles AnimationTimer.Tick
        MoveToSource()
    End Sub

    Private Function ChangeImageOpacity(img As Image, opacityvalue As Single) As Image
        Dim bmp As New Bitmap(img.Width, img.Height)
        Dim gr As Graphics = Graphics.FromImage(bmp)
        Dim colorMat As New ColorMatrix
        Dim imgAttr As New ImageAttributes
        colorMat.Matrix33 = opacityvalue
        imgAttr.SetColorMatrix(colorMat, ColorMatrixFlag.Default, ColorAdjustType.Bitmap)
        gr.DrawImage(img, New Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, imgAttr)

        gr.Dispose()
        Return bmp
    End Function

    Private Sub FaderTimer_Tick(sender As Object, e As EventArgs) Handles FaderTimer.Tick
        mOldImageContainer.ElementAt(mCurrentOldIndex).Image = ChangeImageOpacity(mOldImageContainer.ElementAt(mCurrentOldIndex).Image, fade)
        fade = fade - 0.01
        If fade <= 0.85 Then
            FaderTimer.Stop()
        End If
    End Sub


    Private Function IsFullCompleted() As Boolean
        For Each item In mNewImageContainer
            If (item.Visible = True) Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Sub ButtonRestart_Click(sender As Object, e As EventArgs) Handles ButtonRestart.Click
        ' Add any initialization after the InitializeComponent() call.
        FaderTimer.Enabled = True
        FaderTimer.Interval = 30
        FaderTimer.Stop()
        AnimationTimer.Enabled = True
        AnimationTimer.Interval = 1
        AnimationTimer.Stop()
        usedNewImages.Clear()
        usedOldImages.Clear()
        RemoveFromForm()
        CreatePictureBoxes(4)
    End Sub

    Private Sub RemoveFromForm()
        For Each item In mNewImageContainer
            Me.Controls.Remove(item)
        Next
        For Each item In mOldImageContainer
            Me.Controls.Remove(item)
        Next
        For Each item In mMatchImageContainer
            Me.Controls.Remove(item)
        Next
    End Sub

    Private Sub GlowTimer_Tick(sender As Object, e As EventArgs) Handles GlowTimer.Tick
        For Each item In mOldImageContainer

        Next

    End Sub

    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
