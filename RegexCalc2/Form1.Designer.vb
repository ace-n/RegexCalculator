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
        Me.btnHelp = New System.Windows.Forms.Button()
        Me.cbxUseDegrees = New System.Windows.Forms.CheckBox()
        Me.btnClear = New System.Windows.Forms.Button()
        Me.lbxAs = New System.Windows.Forms.ListBox()
        Me.lbxQs = New System.Windows.Forms.ListBox()
        Me.txtAnswer = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnHelp
        '
        Me.btnHelp.Location = New System.Drawing.Point(325, 422)
        Me.btnHelp.Margin = New System.Windows.Forms.Padding(4)
        Me.btnHelp.Name = "btnHelp"
        Me.btnHelp.Size = New System.Drawing.Size(100, 28)
        Me.btnHelp.TabIndex = 19
        Me.btnHelp.Text = "Show Help"
        Me.btnHelp.UseVisualStyleBackColor = True
        '
        'cbxUseDegrees
        '
        Me.cbxUseDegrees.AutoSize = True
        Me.cbxUseDegrees.Location = New System.Drawing.Point(158, 429)
        Me.cbxUseDegrees.Margin = New System.Windows.Forms.Padding(4)
        Me.cbxUseDegrees.Name = "cbxUseDegrees"
        Me.cbxUseDegrees.Size = New System.Drawing.Size(113, 21)
        Me.cbxUseDegrees.TabIndex = 18
        Me.cbxUseDegrees.Text = "Use Degrees"
        Me.cbxUseDegrees.UseVisualStyleBackColor = True
        '
        'btnClear
        '
        Me.btnClear.Location = New System.Drawing.Point(537, 422)
        Me.btnClear.Margin = New System.Windows.Forms.Padding(4)
        Me.btnClear.Name = "btnClear"
        Me.btnClear.Size = New System.Drawing.Size(100, 28)
        Me.btnClear.TabIndex = 17
        Me.btnClear.Text = "Clear"
        Me.btnClear.UseVisualStyleBackColor = True
        '
        'lbxAs
        '
        Me.lbxAs.FormattingEnabled = True
        Me.lbxAs.ItemHeight = 16
        Me.lbxAs.Location = New System.Drawing.Point(464, 13)
        Me.lbxAs.Margin = New System.Windows.Forms.Padding(4)
        Me.lbxAs.Name = "lbxAs"
        Me.lbxAs.Size = New System.Drawing.Size(172, 292)
        Me.lbxAs.TabIndex = 16
        '
        'lbxQs
        '
        Me.lbxQs.FormattingEnabled = True
        Me.lbxQs.ItemHeight = 16
        Me.lbxQs.Location = New System.Drawing.Point(13, 13)
        Me.lbxQs.Margin = New System.Windows.Forms.Padding(4)
        Me.lbxQs.Name = "lbxQs"
        Me.lbxQs.Size = New System.Drawing.Size(455, 292)
        Me.lbxQs.TabIndex = 15
        '
        'txtAnswer
        '
        Me.txtAnswer.Location = New System.Drawing.Point(13, 390)
        Me.txtAnswer.Margin = New System.Windows.Forms.Padding(4)
        Me.txtAnswer.Name = "txtAnswer"
        Me.txtAnswer.Size = New System.Drawing.Size(623, 22)
        Me.txtAnswer.TabIndex = 14
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(13, 370)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(279, 17)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Press evaluate to display the answer below"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 322)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(378, 17)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Type an equation here (using 0-9 and order of operations)"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(13, 422)
        Me.Button1.Margin = New System.Windows.Forms.Padding(4)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(100, 28)
        Me.Button1.TabIndex = 11
        Me.Button1.Text = "Evaluate"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(17, 342)
        Me.TextBox1.Margin = New System.Windows.Forms.Padding(4)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(619, 22)
        Me.TextBox1.TabIndex = 10
        Me.TextBox1.Text = "::1:: + ::2::"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(652, 462)
        Me.Controls.Add(Me.btnHelp)
        Me.Controls.Add(Me.cbxUseDegrees)
        Me.Controls.Add(Me.btnClear)
        Me.Controls.Add(Me.lbxAs)
        Me.Controls.Add(Me.lbxQs)
        Me.Controls.Add(Me.txtAnswer)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnHelp As System.Windows.Forms.Button
    Friend WithEvents cbxUseDegrees As System.Windows.Forms.CheckBox
    Friend WithEvents btnClear As System.Windows.Forms.Button
    Friend WithEvents lbxAs As System.Windows.Forms.ListBox
    Friend WithEvents lbxQs As System.Windows.Forms.ListBox
    Friend WithEvents txtAnswer As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox

End Class
