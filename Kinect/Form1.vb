Imports System.Runtime.InteropServices
Imports Microsoft.Kinect
Imports OpenGL

Public Class Form1
    Dim kSensor As KinectSensor
    Dim colorReader As ColorFrameReader
    Dim depthReader As DepthFrameReader
    Dim irReader As InfraredFrameReader
    Dim bodyReader As BodyFrameReader

    Dim pixelBuffer As Byte() = Nothing
    Dim depthBuffer As UShort() = Nothing
    Dim irBuffer As UShort() = Nothing

    Dim bmpColour As Bitmap = Nothing
    Dim bmpDepth As Bitmap = Nothing
    Dim bmpIR As Bitmap = Nothing



    Dim glControl As OpenGL.GlControl

    Private Sub btnTest_Click(sender As Object, e As EventArgs) Handles btnTest.Click


        kSensor = KinectSensor.GetDefault

        Dim frameDescription As FrameDescription = kSensor.ColorFrameSource.CreateFrameDescription(ColorImageFormat.Rgba)

        depthReader = kSensor.DepthFrameSource.OpenReader()
        colorReader = kSensor.ColorFrameSource.OpenReader()
        irReader = kSensor.InfraredFrameSource.OpenReader()
        bodyReader = kSensor.BodyFrameSource.OpenReader

        AddHandler colorReader.FrameArrived, AddressOf ColorFrameHandler
        AddHandler depthReader.FrameArrived, AddressOf DepthFrameHandler
        AddHandler irReader.FrameArrived, AddressOf IRFrameHandler
        AddHandler bodyReader.FrameArrived, AddressOf BodyFrameHandler

        pixelBuffer = New Byte((frameDescription.Width * frameDescription.Height * 4) - 1) {}
        depthBuffer = New UShort((kSensor.DepthFrameSource.FrameDescription.Width * kSensor.DepthFrameSource.FrameDescription.Height) - 1) {}
        irBuffer = New UShort(kSensor.InfraredFrameSource.FrameDescription.LengthInPixels - 1) {}

        bmpColour = New Bitmap(frameDescription.Width, frameDescription.Height)
        bmpDepth = New Bitmap(kSensor.DepthFrameSource.FrameDescription.Width, kSensor.DepthFrameSource.FrameDescription.Width, Imaging.PixelFormat.Format16bppGrayScale)
        bmpIR = New Bitmap(kSensor.InfraredFrameSource.FrameDescription.Width, kSensor.InfraredFrameSource.FrameDescription.Height, Imaging.PixelFormat.Format16bppGrayScale)


        kSensor.Open()
    End Sub

    Dim trackedJoints As New List(Of Joint)

    Private Sub BodyFrameHandler(sender As Object, e As BodyFrameArrivedEventArgs)
        Using frame As BodyFrame = e.FrameReference.AcquireFrame
            If frame Is Nothing Then
                groupBody.Text = "Bodies: 0"
                Exit Sub
            End If

            Dim bodies() As Body = New Body(frame.BodyCount - 1) {}
            frame.GetAndRefreshBodyData(bodies)


            glControl.Invalidate()


            trackedJoints.Clear()

            For Each joint In bodies(0).Joints
                If joint.Value.TrackingState = TrackingState.Tracked Then
                    trackedJoints.Add(joint.Value)
                End If
            Next

            groupBody.Text = "Bodies: " & frame.BodyCount & ", " & trackedJoints.Count & " joints"


        End Using
    End Sub

    Private Sub IRFrameHandler(sender As Object, e As InfraredFrameArrivedEventArgs)
        Using frame As InfraredFrame = e.FrameReference.AcquireFrame
            If frame Is Nothing Then
                Exit Sub
            End If

            Dim w As Integer = frame.FrameDescription.Width
            Dim h As Integer = frame.FrameDescription.Height

            frame.CopyFrameDataToArray(irBuffer)

            Dim pixelData As Imaging.BitmapData = bmpIR.LockBits(New Rectangle(0, 0, w, h), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format16bppGrayScale)
            frame.CopyFrameDataToIntPtr(pixelData.Scan0, w * h * 2)
            Dim i As New Emgu.CV.Image(Of Emgu.CV.Structure.Gray, Int16)(w, h, pixelData.Stride, pixelData.Scan0)

            bmpIR.UnlockBits(pixelData)

            picIR.BackgroundImage = i.ToBitmap
            picIR.Invalidate()
        End Using
    End Sub

    Private Sub DepthFrameHandler(sender As Object, e As DepthFrameArrivedEventArgs)
        Using frame As DepthFrame = e.FrameReference.AcquireFrame
            If frame Is Nothing Then
                Exit Sub
            End If

            Dim w As Integer = frame.FrameDescription.Width
            Dim h As Integer = frame.FrameDescription.Height

            frame.CopyFrameDataToArray(depthBuffer)

            Dim pixelData As Imaging.BitmapData = bmpDepth.LockBits(New Rectangle(0, 0, w, h), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format16bppGrayScale)
            frame.CopyFrameDataToIntPtr(pixelData.Scan0, w * h * 2)
            Dim i As New Emgu.CV.Image(Of Emgu.CV.Structure.Gray, Int16)(w, h, pixelData.Stride, pixelData.Scan0)

            bmpDepth.UnlockBits(pixelData)

            picDepth.BackgroundImage = i.ToBitmap
            picDepth.Invalidate()
        End Using
    End Sub




    Private Sub ColorFrameHandler(sender As Object, e As ColorFrameArrivedEventArgs)

        Using frame As ColorFrame = e.FrameReference.AcquireFrame()
            If frame Is Nothing Then
                Exit Sub
            End If

            Dim w As Integer = frame.FrameDescription.Width
            Dim h As Integer = frame.FrameDescription.Height


            frame.CopyConvertedFrameDataToArray(pixelBuffer, ColorImageFormat.Bgra)

            Dim pixelData As Imaging.BitmapData = bmpColour.LockBits(New Rectangle(0, 0, w, h), Imaging.ImageLockMode.ReadWrite, Imaging.PixelFormat.Format32bppArgb)
            Runtime.InteropServices.Marshal.Copy(pixelBuffer, 0, pixelData.Scan0, pixelBuffer.Length)


            bmpColour.UnlockBits(pixelData)


            picPreview.BackgroundImage = bmpColour
            picPreview.Invalidate()


        End Using
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        glControl = New OpenGL.GlControl()
        groupBody.Controls.Add(glControl)
        glControl.Dock = DockStyle.Fill
        AddHandler glControl.Render, AddressOf GLRender
        AddHandler glControl.ContextCreated, AddressOf GLContextCreated
    End Sub

    Private Sub GLContextCreated(sender As Object, e As GlControlEventArgs)
        Gl.MatrixMode(MatrixMode.Projection)
        Gl.LoadIdentity()
        Gl.Ortho(0, 1, 0, 1, 0, 1)
        Gl.MatrixMode(MatrixMode.Modelview)
        Gl.LoadIdentity()
    End Sub

    Private Sub GLRender(sender As Object, e As GlControlEventArgs)
        Gl.VB.Viewport(0, 0, glControl.Width, glControl.Height)
        Gl.VB.Clear(ClearBufferMask.ColorBufferBit)
        Dim currentViewport As Integer() = New Integer(3) {0, 0, 0, 0}
        Gl.Get(Gl.VBEnum.VIEWPORT, currentViewport)

        Gl.Begin(PrimitiveType.Lines)

        Gl.Color3(1.0F, 0.0F, 0.0F)
        For Each joint In trackedJoints
            Gl.Vertex3(joint.Position.X, joint.Position.Y, joint.Position.Z)
        Next


        Gl.End()
    End Sub
End Class
