<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Me.btnTest = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.picDepth = New System.Windows.Forms.PictureBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.picPreview = New System.Windows.Forms.PictureBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.picIR = New System.Windows.Forms.PictureBox()
        Me.groupBody = New System.Windows.Forms.GroupBox()
        Me.lblCoordinates = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtBoneSize = New System.Windows.Forms.TextBox()
        Me.btnDisconnect = New System.Windows.Forms.Button()
        Me.lstJoints = New System.Windows.Forms.ListBox()
        Me.GroupBox1.SuspendLayout()
        CType(Me.picDepth, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        CType(Me.picIR, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnTest
        '
        Me.btnTest.Location = New System.Drawing.Point(45, 86)
        Me.btnTest.Name = "btnTest"
        Me.btnTest.Size = New System.Drawing.Size(148, 52)
        Me.btnTest.TabIndex = 0
        Me.btnTest.Text = "Connect"
        Me.btnTest.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.picDepth)
        Me.GroupBox1.Location = New System.Drawing.Point(524, 222)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(264, 216)
        Me.GroupBox1.TabIndex = 3
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Depth"
        '
        'picDepth
        '
        Me.picDepth.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picDepth.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picDepth.Location = New System.Drawing.Point(3, 16)
        Me.picDepth.Name = "picDepth"
        Me.picDepth.Size = New System.Drawing.Size(258, 197)
        Me.picDepth.TabIndex = 3
        Me.picDepth.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.picPreview)
        Me.GroupBox2.Location = New System.Drawing.Point(527, 23)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(258, 193)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Colour"
        '
        'picPreview
        '
        Me.picPreview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picPreview.Location = New System.Drawing.Point(3, 16)
        Me.picPreview.Name = "picPreview"
        Me.picPreview.Size = New System.Drawing.Size(252, 174)
        Me.picPreview.TabIndex = 2
        Me.picPreview.TabStop = False
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.picIR)
        Me.GroupBox3.Location = New System.Drawing.Point(254, 219)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(264, 216)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Infrared"
        '
        'picIR
        '
        Me.picIR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.picIR.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picIR.Location = New System.Drawing.Point(3, 16)
        Me.picIR.Name = "picIR"
        Me.picIR.Size = New System.Drawing.Size(258, 197)
        Me.picIR.TabIndex = 3
        Me.picIR.TabStop = False
        '
        'groupBody
        '
        Me.groupBody.Location = New System.Drawing.Point(254, 23)
        Me.groupBody.Name = "groupBody"
        Me.groupBody.Size = New System.Drawing.Size(264, 193)
        Me.groupBody.TabIndex = 5
        Me.groupBody.TabStop = False
        Me.groupBody.Text = "Body"
        '
        'lblCoordinates
        '
        Me.lblCoordinates.AutoSize = True
        Me.lblCoordinates.Location = New System.Drawing.Point(25, 38)
        Me.lblCoordinates.Name = "lblCoordinates"
        Me.lblCoordinates.Size = New System.Drawing.Size(69, 13)
        Me.lblCoordinates.TabIndex = 6
        Me.lblCoordinates.Text = "Coordinates: "
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(42, 261)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 7
        Me.Label1.Text = "Bone Size:"
        '
        'txtBoneSize
        '
        Me.txtBoneSize.Location = New System.Drawing.Point(38, 289)
        Me.txtBoneSize.Name = "txtBoneSize"
        Me.txtBoneSize.Size = New System.Drawing.Size(136, 20)
        Me.txtBoneSize.TabIndex = 8
        Me.txtBoneSize.Text = "0.1"
        '
        'btnDisconnect
        '
        Me.btnDisconnect.Location = New System.Drawing.Point(45, 144)
        Me.btnDisconnect.Name = "btnDisconnect"
        Me.btnDisconnect.Size = New System.Drawing.Size(148, 52)
        Me.btnDisconnect.TabIndex = 9
        Me.btnDisconnect.Text = "Disconnect"
        Me.btnDisconnect.UseVisualStyleBackColor = True
        '
        'lstJoints
        '
        Me.lstJoints.FormattingEnabled = True
        Me.lstJoints.Location = New System.Drawing.Point(30, 322)
        Me.lstJoints.Name = "lstJoints"
        Me.lstJoints.Size = New System.Drawing.Size(172, 108)
        Me.lstJoints.TabIndex = 10
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lstJoints)
        Me.Controls.Add(Me.btnDisconnect)
        Me.Controls.Add(Me.txtBoneSize)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lblCoordinates)
        Me.Controls.Add(Me.groupBody)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.btnTest)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Name = "Form1"
        Me.Text = "Kinect Test"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.picDepth, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.picPreview, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.picIR, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnTest As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents picDepth As PictureBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents picPreview As PictureBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents picIR As PictureBox
    Friend WithEvents groupBody As GroupBox
    Friend WithEvents lblCoordinates As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtBoneSize As TextBox
    Friend WithEvents btnDisconnect As Button
    Friend WithEvents lstJoints As ListBox
End Class
