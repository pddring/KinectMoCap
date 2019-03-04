Imports System.Net
Imports System.Net.Sockets
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Threading
Imports Microsoft.Kinect
Imports OpenGL

Public Class Form1
    Dim kSensor As KinectSensor
    Dim colorReader As ColorFrameReader
    Dim depthReader As DepthFrameReader
    Dim irReader As InfraredFrameReader
    Dim bodyReader As BodyFrameReader

    Dim sampleData As String

    Dim pixelBuffer As Byte() = Nothing
    Dim depthBuffer As UShort() = Nothing
    Dim irBuffer As UShort() = Nothing

    Dim bmpColour As Bitmap = Nothing
    Dim bmpDepth As Bitmap = Nothing
    Dim bmpIR As Bitmap = Nothing

    Dim bodies() As Body

    Dim bonesize As Double = 0.1

    Dim colors() As Color = {Color.Red, Color.Blue, Color.Yellow, Color.Violet, Color.Green, Color.Gold}

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

    Dim bestTrackedBodyIndex As Integer = 0


    Private Sub BodyFrameHandler(sender As Object, e As BodyFrameArrivedEventArgs)
        Using frame As BodyFrame = e.FrameReference.AcquireFrame
            If frame Is Nothing Then
                groupBody.Text = "Bodies: 0"
                Exit Sub
            End If

            If IsNothing(bodies) Then
                bodies = New Body(frame.BodyCount - 1) {}
            End If
            frame.GetAndRefreshBodyData(bodies)

            glControl.Invalidate()

            Dim title As String = "Bodies: " & frame.BodyCount & " "

            Dim highestTracked As Integer = 0
            For i = 0 To bodies.Count - 1

                Dim total As Integer = 0
                For Each joint In bodies(i).Joints

                    If joint.Value.TrackingState = TrackingState.Tracked Then
                        total += 1
                    End If

                Next
                If total > highestTracked Then
                    highestTracked = total
                    bestTrackedBodyIndex = i
                End If
                title &= total & " "
            Next


            groupBody.Text = title

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

    Dim x As Double = -0.5
    Dim y As Double = 0
    Dim z As Double = -4

    Sub updateCoordinates()
        lblCoordinates.Text = x & ", " & y & ", " & z
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        updateCoordinates()

        sampleData = My.Computer.FileSystem.ReadAllText("SampleJoints.txt")

        Dim hostname As String = Dns.GetHostName
        Dim ipEntry As IPHostEntry = Dns.GetHostEntry(hostname)

        For Each address In ipEntry.AddressList
            txtIP.Text &= address.ToString & vbNewLine
        Next

        serverThread = New Thread(New ThreadStart(AddressOf serverThreadCode))
        serverThread.Start()

    End Sub

    Protected serverThread As Thread

    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        glControl = New OpenGL.GlControl()
        groupBody.Controls.Add(glControl)
        glControl.Dock = DockStyle.Fill
        AddHandler glControl.Render, AddressOf GLRender
    End Sub

    Private Sub c(body As Body, j1 As JointType, j2 As JointType)
        If body.Joints(j1).TrackingState <> TrackingState.NotTracked And body.Joints(j2).TrackingState <> TrackingState.NotTracked Then
            Gl.Begin(PrimitiveType.Lines)
            Gl.Vertex3(body.Joints(j1).Position.X, body.Joints(j1).Position.Y, body.Joints(j1).Position.Z)
            Gl.Vertex3(body.Joints(j2).Position.X, body.Joints(j2).Position.Y, body.Joints(j2).Position.Z)
            Gl.End()
        End If
    End Sub


    Private Sub GLRender(sender As Object, e As GlControlEventArgs)
        Dim currentViewport As Integer() = New Integer(3) {0, 0, 0, 0}
        Gl.Get(Gl.VBEnum.VIEWPORT, currentViewport)

        Gl.VB.Viewport(0, 0, glControl.Width, glControl.Height)
        Gl.VB.Clear(ClearBufferMask.ColorBufferBit)

        Gl.MatrixMode(MatrixMode.Projection)
        Gl.LoadIdentity()
        Dim aspect As Double = glControl.Height / glControl.Width
        Dim fieldOfView As Double = 45.0
        Dim zNear As Double = 0.01
        Dim zFar As Double = 1000.0F
        Dim fH As Double = Math.Tan(fieldOfView * 3.14159F / 360.0F) * zNear
        Dim fW As Double = fH * aspect
        Gl.Frustum(-fW, fW, -fH, fH, zNear, zFar)

        Gl.Translate(x, y, z)

        Dim bodyCount As Integer = 0
        If Not IsNothing(bodies) Then
            For Each body In bodies

                For Each joint In body.Joints
                    Gl.Begin(PrimitiveType.Triangles)

                    Gl.Color3(colors(bodyCount).R, colors(bodyCount).G, colors(bodyCount).B)
                    Gl.Vertex3(joint.Value.Position.X, joint.Value.Position.Y, joint.Value.Position.Z)
                    Gl.Vertex3(joint.Value.Position.X, joint.Value.Position.Y + bonesize, joint.Value.Position.Z)
                    Gl.Vertex3(joint.Value.Position.X, joint.Value.Position.Y + bonesize, joint.Value.Position.Z + bonesize)
                    Gl.End()
                Next

                c(body, JointType.SpineBase, JointType.SpineMid)
                c(body, JointType.SpineMid, JointType.SpineShoulder)
                c(body, JointType.SpineShoulder, JointType.Neck)
                c(body, JointType.Neck, JointType.Head)
                c(body, JointType.SpineBase, JointType.HipRight)
                c(body, JointType.SpineBase, JointType.HipLeft)
                c(body, JointType.HipLeft, JointType.KneeLeft)
                c(body, JointType.HipRight, JointType.KneeRight)
                c(body, JointType.KneeLeft, JointType.AnkleLeft)
                c(body, JointType.KneeRight, JointType.AnkleRight)
                c(body, JointType.AnkleLeft, JointType.FootLeft)
                c(body, JointType.AnkleRight, JointType.FootRight)
                c(body, JointType.SpineShoulder, JointType.ShoulderLeft)
                c(body, JointType.SpineShoulder, JointType.ShoulderRight)
                c(body, JointType.ShoulderLeft, JointType.ElbowLeft)
                c(body, JointType.ShoulderRight, JointType.ElbowRight)
                c(body, JointType.ElbowLeft, JointType.WristLeft)
                c(body, JointType.ElbowRight, JointType.WristRight)
                c(body, JointType.WristLeft, JointType.HandLeft)
                c(body, JointType.WristRight, JointType.HandRight)
                c(body, JointType.HandLeft, JointType.HandTipLeft)
                c(body, JointType.HandRight, JointType.HandTipRight)
                c(body, JointType.WristLeft, JointType.ThumbLeft)
                c(body, JointType.WristRight, JointType.ThumbRight)


                bodyCount += 1
            Next
        End If


        Gl.Begin(PrimitiveType.LineStrip)
        Gl.Color3(1.0F, 0.0F, 0.0F)
        Gl.Vertex3(0F, 0F, 0F)
        Gl.Vertex3(1.0F, 0F, 0F)
        Gl.End()

        Gl.Begin(PrimitiveType.LineStrip)
        Gl.Color3(0.0F, 1.0F, 0.0F)
        Gl.Vertex3(0F, 0F, 0F)
        Gl.Vertex3(0F, 1.0F, 0F)
        Gl.End()

        Gl.Begin(PrimitiveType.LineStrip)
        Gl.Color3(0.0F, 0.0F, 1.0F)
        Gl.Vertex3(0F, 0F, 0F)
        Gl.Vertex3(0F, 0F, 1.0F)
        Gl.End()
    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.W
                z += 0.1
            Case Keys.S
                z -= 0.1
            Case Keys.A
                x += 0.1
            Case Keys.D
                x -= 0.1
            Case Keys.R
                y += 0.1
            Case Keys.F
                y -= 0.1
        End Select
        updateCoordinates()
        glControl.Invalidate()
    End Sub

    Private Sub txtBoneSize_TextChanged(sender As Object, e As EventArgs) Handles txtBoneSize.TextChanged
        bonesize = txtBoneSize.Text
    End Sub

    Private Sub btnDisconnect_Click(sender As Object, e As EventArgs) Handles btnDisconnect.Click
        kSensor.Close()
    End Sub

    Private Sub btnClientTest_Click(sender As Object, e As EventArgs) Handles btnClientTest.Click
        Dim udpClient As New UdpClient

        Dim server As New IPEndPoint(New IPAddress({127, 0, 0, 1}), 3456)
        udpClient.Connect(server)
        Dim txBuffer As Byte() = Encoding.UTF8.GetBytes("GETJOINTS")
        udpClient.Send(txBuffer, txBuffer.Length)

        Dim rxBuffer As Byte() = udpClient.Receive(server)
        txtClientTest.Text = Encoding.UTF8.GetString(rxBuffer)

    End Sub

    Public Sub serverThreadCode()

        While True
            Using udpClient As New UdpClient(3456)
                Dim remoteIpEndPoint As New IPEndPoint(IPAddress.Any, 0)
                Dim buffer As Byte()
                buffer = udpClient.Receive(remoteIpEndPoint)
                Dim response As String = Encoding.UTF8.GetString(buffer)

                lstLog.BeginInvoke(Sub()
                                       lstLog.Items.Add(response & " from " & remoteIpEndPoint.ToString)
                                   End Sub)

                'TODO deal with response
                Dim txBuffer As Byte() = Encoding.UTF8.GetBytes(GetJointData(bestTrackedBodyIndex))
                udpClient.Connect(remoteIpEndPoint)
                udpClient.Send(txBuffer, txBuffer.Length)
            End Using
        End While
    End Sub

    Public Function GetJointData(bodyIndex As Integer) As String
        Dim data As String = "Not connected"
        ' sending sample data
        If IsNothing(bodies) Then
            data = sampleData
        Else
            data = "Body " & bodyIndex & vbNewLine
            For Each joint In bodies(bodyIndex).Joints
                data &= joint.Key.ToString & ": " & joint.Value.Position.X & "," & joint.Value.Position.Y & "," & joint.Value.Position.Z & vbNewLine
            Next
        End If
        Return data

    End Function

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        serverThread.Abort()
    End Sub
End Class
