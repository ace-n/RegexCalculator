Imports System.Text.RegularExpressions
Imports RegexCalc2.RegexSolving

Public Class Form1

    Public Answer = 0

    Public Function NewtonRegexSolve(ByVal ExprStrMasterIn As String, Optional ByVal Guess As Double = 1)

        ' Get equation left/right hand side
        Dim ELeft As String = ExprStrMasterIn.Substring(0, ExprStrMasterIn.LastIndexOf("="))
        Dim ERight As String = ExprStrMasterIn.Substring(ExprStrMasterIn.LastIndexOf("=") + 1)
        If ERight.Contains(",") Then
            ERight = ERight.Substring(0, ERight.LastIndexOf(","))
        End If

        ' Solve using Newton's method
        Dim Y As Double = RegexFunctionEvaluate(ELeft.Replace("x", CStr(Guess))) - RegexFunctionEvaluate(ERight.Replace("x", CStr(Guess)))
        Dim Deriv As Double
        Deriv /= 2

        For i = 1 To 50

            Deriv = (RegexFunctionEvaluate(ELeft.Replace("x", CStr(Guess))) - RegexFunctionEvaluate(ELeft.Replace("x", CStr(Guess - 0.001)))) * 1000
            Guess -= Y / Deriv

            Y = RegexFunctionEvaluate(ELeft.Replace("x", CStr(Guess))) - RegexFunctionEvaluate(ERight.Replace("x", CStr(Guess)))
            If Math.Abs(Y) < 10 ^ -10 Then
                Return Math.Round(Guess, 10)
            End If
        Next

        Return Math.Round(Guess, 10)

    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        ' Replace any old answers
        Dim ReplacedText As String = TextBox1.Text
        For i = lbxAs.Items.Count To 1 Step -1
            ReplacedText = ReplacedText.Replace("A(" & i & ")", lbxAs.Items.Item(i - 1).ToString)
        Next
        If ReplacedText.Contains("A(") Then
            MsgBox("Invalid past answer specified.")
            Exit Sub
        End If

        If ReplacedText.Contains("print[") Then

            If Not (ReplacedText.StartsWith("print[") And ReplacedText.EndsWith("]")) Then
                MsgBox("Something went wrong with the print command. The print command should be used as: print[f(x),start,end[,interval]]")
                Exit Sub
            End If

            ' --- Get string data ---

            ' Function
            Dim FXStr As String = ReplacedText.Substring(6, ReplacedText.IndexOf(",") - 6)

            ' Starting value
            Dim StartStr As String = ReplacedText.Substring(ReplacedText.IndexOf(",") + 1)
            StartStr = StartStr.Substring(0, StartStr.IndexOf(","))
            Dim StartNum As Double = CDbl(StartStr)

            ' Ending value
            Dim EndStr As String = ReplacedText.Substring(8 + FXStr.Length + StartStr.Length)
            Dim EndNum As Double = CDbl(EndStr.Substring(0, Math.Max(EndStr.IndexOf(","), EndStr.IndexOf("]"))))

            ' Interval (if one applies)
            Dim IntervalStr As String = ""
            If EndStr.Contains(",") Then
                IntervalStr = EndStr.Substring(EndStr.IndexOf(",") + 1)
                IntervalStr = IntervalStr.Substring(0, IntervalStr.Length - 1)
            Else
                IntervalStr = 1
            End If
            Dim IntNum As Double = CDbl(IntervalStr)

            ' Prevent infinite loop
            If Math.Sign(EndNum - StartNum) <> Math.Sign(IntNum) Then
                Dim N2 As Integer = EndNum
                EndNum = StartNum
                StartNum = N2
            End If

            ' Calculation loop
            Dim Results As New List(Of Double)
            Try
                For x As Double = StartNum To EndNum Step IntervalStr

                    ' Solve the function
                    Dim RegexResult As Double = RegexFunctionEvaluate(FXStr.Replace("ans", CStr(Answer)).Replace("x", CStr(x)))
                    Answer = RegexResult

                    ' Add to preliminary results
                    Results.Add(CStr(RegexResult))

                Next

            Catch
                MsgBox("Something went wrong. Double check everything to make sure it is correct, and then try again.")
                Exit Sub
            End Try

            ' Add final results to list
            For Each D As Double In Results
                lbxAs.Items.Add(D)
            Next

        ElseIf ReplacedText.Contains("solve[") Then

            If Not (ReplacedText.StartsWith("solve[") And ReplacedText.EndsWith("]")) Then
                MsgBox("Something went wrong with the solve command. The solve command should be used as: solve[f(x)[,guess]]")
                Exit Sub
            End If

            ' Get function
            Dim FXStr As String = ReplacedText.Substring(6, Math.Max(ReplacedText.IndexOf(","), ReplacedText.IndexOf("]")) - 6)

            ' Get guess, if provided
            If ReplacedText.Contains(",") Then

                Dim Guess As Double = CStr(ReplacedText.Substring(ReplacedText.IndexOf(",") + 1).Replace("]", ""))

                lbxQs.Items.Add(ReplacedText)
                lbxAs.Items.Add(NewtonRegexSolve(FXStr, Guess))

            Else
                lbxQs.Items.Add(ReplacedText)
                lbxAs.Items.Add(NewtonRegexSolve(FXStr))
            End If

        Else

            Try

                ' Calculate!
                Dim RegexResult As Double = RegexFunctionEvaluate(ReplacedText)
                txtAnswer.Text = RegexResult

                ' Add strings to main calculation listviews
                lbxQs.Items.Add(ReplacedText.Replace("ans", CStr(Answer)))
                lbxAs.Items.Add(CStr(RegexResult))

                ' Update answer
                Answer = RegexResult

            Catch
                MsgBox("Something went wrong. Double check everything to make sure it is correct, and then try again.")
                Exit Sub
            End Try

        End If

        ' Shorten lists if necessary
        While lbxQs.Items.Count > 18
            lbxQs.Items.RemoveAt(0)
        End While
        While lbxAs.Items.Count > 18
            lbxAs.Items.RemoveAt(0)
        End While

    End Sub

    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click

        lbxQs.Items.Clear()
        lbxAs.Items.Clear()

    End Sub

    Private Sub CopyQ(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxQs.MouseDoubleClick
        Try
            My.Computer.Clipboard.SetText(lbxQs.SelectedItem.ToString)
        Catch
        End Try
    End Sub
    Private Sub CopyA(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxAs.MouseDoubleClick
        Try
            My.Computer.Clipboard.SetText(lbxAs.SelectedItem.ToString)
        Catch
        End Try
    End Sub

    ' List de-selectors
    Private Sub DSQ(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxAs.MouseClick
        lbxQs.ClearSelected()
    End Sub
    Private Sub DSA(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lbxQs.MouseClick
        lbxAs.ClearSelected()
    End Sub

End Class
