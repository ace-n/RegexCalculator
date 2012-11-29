' --- COPIED FROM MathChecker ON 10/17/2012 ---
Imports System.Text.RegularExpressions

Public Class RegexSolving

    Public Shared Answer = 0

    ' Inserts multiplication signs into functions so that the regex function can parse them (e.g. "2(x+3)" --> "2*(x+3)")
    Public Shared Function InsertMultiplySymbols(ByVal ExprStrMaster) As String

        Dim MultInsertRegex As New Regex("(\d(\(|[a-z])|\)\()")
        Dim MultInsertMatch As Match = MultInsertRegex.Match(ExprStrMaster)
        While MultInsertMatch.Success
            ExprStrMaster = ExprStrMaster.Insert(MultInsertMatch.Index + 1, "*")
            MultInsertMatch = MultInsertRegex.Match(ExprStrMaster, MultInsertMatch.Index) ' Start at previous match index to speed things up
        End While

        Return ExprStrMaster

    End Function

    ' Replaces named constants with their numerical values
    Public Shared Function ReplaceConstants(ByVal ExprStrMaster As String) As String

        '   Euler's constant (e)
        Dim ConstantRegex As New Regex("(\W|\s)e(\W|\s)")
        Dim ConstantMatch As Match = ConstantRegex.Match(ExprStrMaster)
        Dim ConstantStr As String = Math.E.ToString
        While ConstantMatch.Success
            ExprStrMaster = ExprStrMaster.Remove(ConstantMatch.Index + 1, 1).Insert(ConstantMatch.Index + 1, ConstantStr)
            ConstantMatch = ConstantRegex.Match(ExprStrMaster, ConstantMatch.Index + ConstantStr.Length) ' Start at previous match index to speed things up
        End While

        '   Pi
        ConstantRegex = New Regex("(\W|\s)pi(\W|\s)")
        ConstantMatch = ConstantRegex.Match(ExprStrMaster)
        ConstantStr = Math.PI.ToString
        While ConstantMatch.Success
            ExprStrMaster = ExprStrMaster.Remove(ConstantMatch.Index + 1, 2).Insert(ConstantMatch.Index + 1, ConstantStr)
            ConstantMatch = ConstantRegex.Match(ExprStrMaster, ConstantMatch.Index + ConstantStr.Length)  ' Start at previous match index to speed things up
        End While

        ' Return result
        Return ExprStrMaster

    End Function

    Public Shared Function RegexFunctionEvaluate(ByVal ExprStrMasterIn As String, Optional ByVal UseDegrees As Boolean = False, Optional ByVal NotPreformatted As Boolean = True)

        ' Precision of the calculations being performed
        Dim Precision As Integer = 8

        ' Skip null strings
        If ExprStrMasterIn.Length = 0 OrElse String.IsNullOrWhiteSpace(ExprStrMasterIn) Then
            Return 0
        End If

        ' Conversion factor between radians and degrees
        Dim RadDegConver As Double = 1
        If UseDegrees Then
            RadDegConver = Math.PI / 180
        End If

        ' Make sure the number of parenthesis is appropriate
        Dim StrA As String = ExprStrMasterIn.Replace("(", "").Replace("[", "")
        Dim StrB As String = ExprStrMasterIn.Replace(")", "").Replace("]", "")
        If StrA.Length <> StrB.Length Then
            MsgBox("Mismatched parenthesis/brackets")
            'Throw New Exception("FunctionEvaluate ERROR: The parentheses do not line up properly. Function: " & ExprStrMasterIn)
            Return 0
        End If

        ' Variables
        Dim ExprStrMaster As String = " " & ExprStrMasterIn ' The space in front allows constants to be properly replaced

        ' Preformatting = add multiplication symbols, then replace constants with their approximate numerical values
        If NotPreformatted Then

            ' Multiplication symbol insertion
            ExprStrMaster = InsertMultiplySymbols(ExprStrMaster)

            ' Constant replacement
            ExprStrMaster = ReplaceConstants(ExprStrMaster)

        End If

        ' Remove any commas
        If ExprStrMaster.Contains(",") Then
            ExprStrMaster = ExprStrMaster.LastIndexOf(",")
        End If

        ' Regex
        Dim NumberRegex As New Regex("(-|_|)(\d|:v:)+")
        Dim SubRegex As New Regex("\d+-(_|\d)+")
        Dim RegexParenConstant As New Regex(":y:(_|\d|:v:)+:z:")
        Dim RegexParen As New Regex("(:y:)(:A:|:S:|:M:|:D:|:E:|_|-|\d|:v:)*(:z:)")
        Dim RegexOperator As New Regex("(:A:|:S:|:M:|:D:|:E:|:y:|:z:|:w:|:x:)")

        ' If there is a constant that is enclosed by parenthesis, remove the parenthesis
        ExprStrMaster = ExprStrMaster.Replace("(", ":y:").Replace(")", ":z:").Replace(".", ":v:")

        ' ---------- Negative formatting ----------

        ' :S: a negative = :A:
        ExprStrMaster = ExprStrMaster.Replace("--", ":A:")

        ' For - (:S: and negative)
        Dim SubMatch As Match = SubRegex.Match(ExprStrMaster)

        ' --------- Mass formatting ---------
        ' Note: perhaps make a multiple parameter replacement system that does one pass and replaces everything
        ' HINT: Conduct all possible preformat functions before sending function (in a batch manner)
        ExprStrMaster = ExprStrMaster.Replace("pi", CStr(Math.PI)).Replace(" ", "").Replace("+", ":A:").Replace("*", ":M:").Replace("/", ":D:").Replace("^", ":E:").Replace("ans", CStr(Answer))

        ' For functions
        ExprStrMaster = ExprStrMaster.Replace("[", ":w:").Replace("]", ":x:")

        ' Define Regexes (EMDAS)
        Dim AddRegex As New Regex("(-|_|\d|:v:)+:A:(_|\d|:v:)+")
        Dim MulRegex As New Regex("(-|_|\d|:v:)+:M:(_|\d|:v:)+")
        Dim DivRegex As New Regex("(-|_|\d|:v:)+:D:(_|\d|:v:)+")
        Dim ExpRegex As New Regex("(-|_|\d|:v:)+:E:(_|\d|:v:)+")
        Dim SubNumRegex As New Regex("(-|_|\d|:v:)+(:S:|_)(_|\d|:v:)+")

        ' Define Regexes (Trig functions)
        '   NOTE: :y: and :z: ( '(' and ')' ) used to be :w: and :x: ( '[' and ']' ) respectively
        Dim SinRegex As New Regex("sin" & "(-|_|)(\d|:v:)+")
        Dim CosRegex As New Regex("cos" & "(-|_|)(\d|:v:)+")
        Dim TanRegex As New Regex("tan" & "(-|_|)(\d|:v:)+")
        Dim AsinRegex As New Regex("asin" & "(-|_|)(\d|:v:)+")
        Dim AcosRegex As New Regex("acos" & "(-|_|)(\d|:v:)+")
        Dim AtanRegex As New Regex("atan" & "(-|_|)(\d|:v:)+")

        Dim VecLenRegex As New Regex("veclen:w:(_|\d|:v:)+,(_|\d|:v:)+:x:")
        Dim ClampRegex As New Regex("clamp:w:(_|\d|:v:)+,(_|\d|:v:)+,(_|\d|:v:)+:x:")

        ' Loop through parenthesis
        Dim PastExprStrMaster As String = ""
        While NumberRegex.Match(ExprStrMaster).Length <> ExprStrMaster.Length

            ' DEBUG
            Console.WriteLine("----- MainEx: " & ExprStrMaster & " -----")

            ' If past expression equals current one, there is an error
            Dim MatchParen As Match = RegexParen.Match(ExprStrMaster)

            ' Operate on parenthesis-enclosed string
            Dim ExprStr As String = ExprStrMaster
            If MatchParen.Success() Then

                ExprStr = RegexParen.Match(ExprStrMaster).Value
                ExprStr = ExprStr.Substring(3, ExprStr.Length - 6)

            End If

            ' Unaltered copy of ExprStr
            Dim ExprStrCopy As String = ExprStr

            ' If ExprStr is a constant, drop the surrounding parenthesis and try again
            If NumberRegex.Match(ExprStr).Length = ExprStr.Length And MatchParen.Success Then

                ExprStrMaster = ExprStrMaster.Replace(MatchParen.Value, ExprStr)
                Continue While

            End If

            ' Step 0: Organize negatives
            ' Replace all subtraction -'s with ":S:"
            While SubRegex.Match(ExprStr).Success

                SubMatch = SubRegex.Match(ExprStr)
                ExprStr = ExprStr.Replace(SubMatch.Value, SubMatch.Value.Replace("-", ":S:"))

            End While

            ExprStr = ExprStr.Replace("--", ":A:")
            ExprStr = ExprStr.Replace("-", "_")
            ExprStr = ExprStr.Replace(".", ":v:")

            ' If the function is purely a number, exit the solving loop
            If NumberRegex.Match(ExprStrMaster).Length = ExprStrMaster.Length Then
                Continue While
            End If

            ' Main loop
            While True

                ' Debug
                'Console.WriteLine("TempEx: " & ExprStr)

                ' If the function isn't fully solvable, throw an exception (to say that there was a problem with the function)
                'If PastExprStrMaster = ExprStr Then
                '    Throw New Exception("FunctionEvaluate ERROR: Improper function entered; Function: " & ExprStrMasterIn & ", Solution: " & ExprStrMaster)
                '    Return 0
                'End If

                ' Basic formatting - after all, "--" = "+"
                ExprStr = ExprStr.Replace("--", ":A:")
                ExprStr = ExprStr.Replace(".", ":v:")

                ' Uses PEMDAS

                ' Step 2: Define Regex matches
                Dim AddMatch As Match = AddRegex.Match(ExprStr)
                SubMatch = SubNumRegex.Match(ExprStr)
                Dim MulMatch As Match = MulRegex.Match(ExprStr)
                Dim DivMatch As Match = DivRegex.Match(ExprStr)
                Dim ExpMatch As Match = ExpRegex.Match(ExprStr)

                Dim VecLenMatch As Match = VecLenRegex.Match(ExprStr)
                Dim ClampMatch As Match = ClampRegex.Match(ExprStr)

                ' Step 3: Functions
                If VecLenMatch.Success Then

                    ' Get X
                    Dim Base As String = VecLenMatch.Value.Substring(3)
                    Dim NumX As String = NumberRegex.Match(Base).Value

                    ' Get Y
                    Dim NumY As String = NumberRegex.Match(Base, NumX.Length + 1).Value

                    ' Calculate vector length
                    Dim Result As Double = Math.Round(Math.Sqrt(CDbl(NumX) ^ 2 + CDbl(NumY) ^ 2), Precision)

                    ' Replace data in string
                    ExprStr = ExprStr.Replace(VecLenMatch.Value, Result)

                    ' Continue
                    ExprStr = ExprStr.Replace(".", ":v:")
                    PastExprStrMaster = ExprStrMaster
                    Continue While

                End If

                If ClampMatch.Success Then

                    ' Get parameters
                    Dim Base As String = ClampMatch.Value.Substring(9)

                    ' Get Num
                    Dim Num As String = NumberRegex.Match(Base).Value.Replace(":v:", ".").Replace("_", "-")

                    ' Get Min
                    Dim Min As String = NumberRegex.Match(Base, Num.Length + 1).Value.Replace(":v:", ".").Replace("_", "-")

                    ' Get Max
                    Dim Max As String = NumberRegex.Match(Base, Num.Length + Min.Length + 2).Value.Replace(":v:", ".").Replace("_", "-")

                    ' Make sure min is smaller than max
                    If CDbl(Min) > CDbl(Max) Then
                        Throw New ArithmeticException("FunctionEvaluate ERROR: Clamping function's MINIMUM value is more than its MAXIMUM; Function: " & ExprStrMasterIn & ", Solution: " & ExprStrMaster)
                        Return 0
                    End If

                    ' Calculate result
                    Dim ResultNum As Double = Num
                    If ResultNum > Max Then
                        ResultNum = Max
                    ElseIf ResultNum < Min Then
                        ResultNum = Min
                    End If

                    ' Replace string data
                    ExprStr = ExprStr.Replace(ClampMatch.Value, ResultNum)
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    ExprStr = ExprStr.Replace(".", ":v:")
                    PastExprStrMaster = ExprStrMaster
                    Continue While


                End If

                ' Step 4: Trig functions
                Dim SinMatch As Match = SinRegex.Match(ExprStr)
                Dim CosMatch As Match = CosRegex.Match(ExprStr)
                Dim TanMatch As Match = TanRegex.Match(ExprStr)

                Dim AsinMatch As Match = AsinRegex.Match(ExprStr)
                Dim AcosMatch As Match = AcosRegex.Match(ExprStr)
                Dim AtanMatch As Match = AtanRegex.Match(ExprStr)

                If AsinMatch.Success Then

                    ' Get number
                    Dim Num As Double = NumberRegex.Match(AsinMatch.Value).Value.Replace("_", "-").Replace(":v:", ".")

                    ' Get value
                    Dim Value As Double = Math.Round(Math.Asin(Num * RadDegConver), Precision)

                    ExprStr = ExprStr.Replace(AsinMatch.Value, CStr(Value).Replace("_", "-").Replace(":v:", "."))
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    Continue While

                End If

                If AcosMatch.Success Then

                    ' Get number
                    Dim Num As Double = NumberRegex.Match(AcosMatch.Value).Value.Replace("_", "-").Replace(":v:", ".")

                    ' Get value
                    Dim Value As Double = Math.Round(Math.Acos(Num * RadDegConver), Precision)

                    ExprStr = ExprStr.Replace(AcosMatch.Value, CStr(Value).Replace("_", "-").Replace(":v:", "."))
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    Continue While

                End If

                If AtanMatch.Success Then

                    ' Get number
                    Dim Num As Double = NumberRegex.Match(AtanMatch.Value).Value.Replace("_", "-").Replace(":v:", ".")

                    ' Get value
                    Dim Value As Double = Math.Round(Math.Atan(Num * RadDegConver), Precision)

                    ExprStr = ExprStr.Replace(AtanMatch.Value, CStr(Value).Replace("_", "-").Replace(":v:", "."))
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    Continue While

                End If

                If SinMatch.Success Then

                    ' Get number
                    Dim Num As Double = NumberRegex.Match(SinMatch.Value).Value.Replace("_", "-").Replace(":v:", ".")

                    ' Get value
                    Dim Value As Double = Math.Round(Math.Sin(Num * RadDegConver), Precision)

                    ExprStr = ExprStr.Replace(SinMatch.Value, CStr(Value).Replace("_", "-").Replace(":v:", "."))
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    Continue While

                End If

                If CosMatch.Success Then

                    ' Get number
                    Dim Num As Double = NumberRegex.Match(CosMatch.Value).Value.Replace("_", "-").Replace(":v:", ".")

                    ' Get value
                    Dim Value As Double = Math.Round(Math.Round(Math.Cos(Num * RadDegConver), 5))

                    ExprStr = ExprStr.Replace(CosMatch.Value, CStr(Value).Replace("_", "-").Replace(":v:", "."))
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    Continue While

                End If

                If TanMatch.Success Then

                    ' Get number
                    Dim Num As Double = NumberRegex.Match(TanMatch.Value).Value.Replace("_", "-").Replace(":v:", ".")

                    ' Get value
                    Dim Value As Double = Math.Round(Math.Tan(Num * RadDegConver), Precision)

                    ExprStr = ExprStr.Replace(TanMatch.Value, CStr(Value).Replace("_", "-").Replace(":v:", "."))
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    ' Continue
                    Continue While

                End If

                ' Step 5: Exponents
                If ExpMatch.Success Then

                    ' Get A number
                    Dim NumA As String = NumberRegex.Match(ExpMatch.Value).Value
                    NumA = NumA.Replace(":v:", ".").Replace("_", "-")

                    ' Get B number
                    Dim NumB As String = NumberRegex.Match(ExpMatch.Value, NumA.Length + 2).Value
                    NumB = NumB.Replace(":v:", ".").Replace("_", "-")

                    ' Conduct operation
                    Dim Result As String = Math.Round(NumA ^ NumB, Precision)
                    Result = Result.Replace("-", "_").Replace(".", ":v:")

                    ExprStr = ExprStr.Replace(ExpMatch.Value, Result)


                    ' Reformat
                    ExprStr = ExprStr.Replace(".", ":v:")
                    ExprStr = ExprStr.Replace("-", "neg")

                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    PastExprStrMaster = ExprStrMaster

                    ' Retry loop
                    Continue While

                End If

                ' Step 6: Multiplication (only acts if there is no division closer to the left)
                If MulMatch.Success And ((MulMatch.Index < DivMatch.Index) Or Not DivMatch.Success) Then

                    ' Get A number
                    Dim NumA As String = NumberRegex.Match(MulMatch.Value).Value
                    NumA = NumA.Replace(":v:", ".").Replace("_", "-")

                    ' Get B number
                    Dim NumB As String = NumberRegex.Match(MulMatch.Value, NumA.Length + 2).Value
                    NumB = NumB.Replace(":v:", ".").Replace("_", "-")

                    ' Conduct operation
                    Dim Result As String = CStr(Math.Round(CDbl(NumA) * CDbl(NumB), Precision))
                    Result = Result.Replace("-", "_").Replace(".", ":v:")

                    ExprStr = ExprStr.Replace(MulMatch.Value, Result)

                    ' Reformat
                    ExprStr = ExprStr.Replace(".", ":v:")
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    PastExprStrMaster = ExprStrMaster

                    ' Retry loop
                    Continue While

                End If

                ' Step 7: Division
                If DivMatch.Success Then

                    ' Get A number
                    Dim NumA As String = NumberRegex.Match(DivMatch.Value).Value
                    NumA = NumA.Replace(":v:", ".").Replace("_", "-")

                    ' Get B number
                    Dim NumB As String = NumberRegex.Match(DivMatch.Value, NumA.Length + 2).Value
                    NumB = NumB.Replace(":v:", ".").Replace("_", "-")

                    ' Conduct operation
                    Dim Result As String = CStr(Math.Round(CDbl(NumA) / CDbl(NumB), Precision))
                    Result = Result.Replace("-", "_").Replace(".", ":v:")

                    ExprStr = ExprStr.Replace(DivMatch.Value, Result)

                    ' Reformat
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    PastExprStrMaster = ExprStrMaster

                    ' Retry loop
                    Continue While

                End If

                ' Step 8: Addition (only acts if there is no subtraction closer to the left)
                If AddMatch.Success And ((AddMatch.Index < SubMatch.Index) Or (Not SubMatch.Success)) Then

                    ' Get A number
                    Dim NumA As String = NumberRegex.Match(AddMatch.Value).Value
                    NumA = NumA.Replace(":v:", ".").Replace("_", "-")

                    ' Get B number
                    Dim NumB As String = NumberRegex.Match(AddMatch.Value, NumA.Length + 2).Value
                    NumB = NumB.Replace(":v:", ".").Replace("_", "-")

                    ' Conduct operation
                    Dim Result As String = Math.Round(CDbl(NumA) + CDbl(NumB), Precision)
                    Result = Result.Replace("-", "_").Replace(".", ":v:")

                    ExprStr = ExprStr.Replace(AddMatch.Value.Replace("-", "_"), Result)

                    ' Reformat
                    ExprStr = ExprStr.Replace(".", ":v:")
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    PastExprStrMaster = ExprStrMaster

                    ' Retry loop
                    Continue While

                End If

                ' Step 9: Subtraction
                '   NOTE: Subtraction operates slightly differently than the other operators - this is because "1:S:3" and "1_3" are considered equivalents (all other operations only have a :*: form)
                If SubMatch.Success Then

                    ' Get A number
                    Dim NumA As String = NumberRegex.Match(SubMatch.Value).Value
                    NumA = NumA.Replace(":v:", ".").Replace("_", "-")

                    ' Get B number
                    Dim NumB As String = NumberRegex.Match(SubMatch.Value, NumA.Length + 2 - SubMatch.Value.Contains(":S")).Value
                    NumB = NumB.Replace(":v:", ".").Replace("_", "-")

                    ' Conduct operation
                    Dim Result As String = Math.Round(CDbl(NumA) - CDbl(NumB), Precision)
                    Result = Result.Replace("-", "_").Replace(".", ":v:")

                    ExprStr = ExprStr.Replace(SubMatch.Value, Result)
                    ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)

                    PastExprStrMaster = ExprStrMaster

                    ' Retry loop
                    Continue While

                End If

                ' Update master expression string
                ExprStrMaster = ExprStrMaster.Replace(ExprStrCopy, ExprStr)
                Exit While

            End While

            ' Check to make sure that the function is written properly
            If ExprStrCopy = ExprStr Then

                ' If a valid number is found, perform a few exception fixing operations and return it
                ExprStrMaster = ExprStrMaster.Replace(":v:", ".").Replace("_", "-")
                If NumberRegex.Match(ExprStrMaster).Length = ExprStrMaster.Length Then

                    Return CDbl(ExprStrMaster)

                Else

                    ' If the function cannot be completed, throw an exception
                    'Throw New ArithmeticException("FunctionEvaluate ERROR: Improper or unsolvable function entered: Function: " & ExprStrMasterIn & ", Solution: " & ExprStrMaster)
                    MsgBox("FunctionEvaluate ERROR: Improper or unsolvable function entered: Function: " & ExprStrMasterIn & ", Solution: " & ExprStrMaster)
                    Return 0

                End If

            End If

        End While

        Return CDbl(ExprStrMaster.Replace(":v:", ".").Replace("_", "-"))

    End Function

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

    '   --- KEPT FOR REFERENCE (primarily about the "print" and "solve" commands") ---
    '    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

    '        ' Replace any old answers
    '        Dim ReplacedText As String = TextBox1.Text
    '        For i = lbxAs.Items.Count To 1 Step -1
    '            ReplacedText = ReplacedText.Replace("A(" & i & ")", lbxAs.Items.Item(i - 1).ToString)
    '        Next
    '        If ReplacedText.Contains("A(") Then
    '            MsgBox("Invalid past answer specified.")
    '            Exit Sub
    '        End If

    '        If ReplacedText.Contains("print[") Then

    '            If Not (ReplacedText.StartsWith("print[") And ReplacedText.EndsWith("]")) Then
    '                MsgBox("Something went wrong with the print command. The print command should be used as: print[f(x),start,end[,interval]]")
    '                Exit Sub
    '            End If

    '            ' --- Get string data ---

    '            ' Function
    '            Dim FXStr As String = ReplacedText.Substring(6, ReplacedText.IndexOf(",") - 6)

    '            ' Starting value
    '            Dim StartStr As String = ReplacedText.Substring(ReplacedText.IndexOf(",") + 1)
    '            StartStr = StartStr.Substring(0, StartStr.IndexOf(","))
    '            Dim StartNum As Double = CDbl(StartStr)

    '            ' Ending value
    '            Dim EndStr As String = ReplacedText.Substring(8 + FXStr.Length + StartStr.Length)
    '            Dim EndNum As Double = CDbl(EndStr.Substring(0, Math.Max(EndStr.IndexOf(","), EndStr.IndexOf("]"))))

    '            ' Interval (if one applies)
    '            Dim IntervalStr As String = ""
    '            If EndStr.Contains(",") Then
    '                IntervalStr = EndStr.Substring(EndStr.IndexOf(",") + 1)
    '                IntervalStr = IntervalStr.Substring(0, IntervalStr.Length - 1)
    '            Else
    '                IntervalStr = 1
    '            End If
    '            Dim IntNum As Double = CDbl(IntervalStr)

    '            ' Prevent infinite loop
    '            If Math.Sign(EndNum - StartNum) <> Math.Sign(IntNum) Then
    '                Dim N2 As Integer = EndNum
    '                EndNum = StartNum
    '                StartNum = N2
    '            End If

    '            ' Calculation loop
    '            Dim Results As New List(Of Double)
    '            Try
    '                For x As Double = StartNum To EndNum Step IntervalStr

    '                    ' Solve the function
    '                    Dim RegexResult As Double = RegexFunctionEvaluate(FXStr.Replace("ans", CStr(Answer)).Replace("x", CStr(x)))
    '                    Answer = RegexResult

    '                    ' Add to preliminary results
    '                    Results.Add(CStr(RegexResult))

    '                Next

    '            Catch
    '                MsgBox("Something went wrong. Double check everything to make sure it is correct, and then try again.")
    '                Exit Sub
    '            End Try

    '            ' Add final results to list
    '            For Each D As Double In Results
    '                lbxAs.Items.Add(D)
    '            Next

    '        ElseIf ReplacedText.Contains("solve[") Then

    '            If Not (ReplacedText.StartsWith("solve[") And ReplacedText.EndsWith("]")) Then
    '                MsgBox("Something went wrong with the solve command. The solve command should be used as: solve[f(x)[,guess]]")
    '                Exit Sub
    '            End If

    '            ' Get function
    '            Dim FXStr As String = ReplacedText.Substring(6, Math.Max(ReplacedText.IndexOf(","), ReplacedText.IndexOf("]")) - 6)

    '            ' Get guess, if provided
    '            If ReplacedText.Contains(",") Then

    '                Dim Guess As Double = CStr(ReplacedText.Substring(ReplacedText.IndexOf(",") + 1).Replace("]", ""))

    '                lbxQs.Items.Add(ReplacedText)
    '                lbxAs.Items.Add(NewtonRegexSolve(FXStr, Guess))

    '            Else
    '                lbxQs.Items.Add(ReplacedText)
    '                lbxAs.Items.Add(NewtonRegexSolve(FXStr))
    '            End If

    '        Else

    '            Try

    '                ' Calculate!
    '                Dim RegexResult As Double = RegexFunctionEvaluate(ReplacedText)
    '                txtAnswer.Text = RegexResult

    '                ' Add strings to main calculation listviews
    '                lbxQs.Items.Add(ReplacedText.Replace("ans", CStr(Answer)))
    '                lbxAs.Items.Add(CStr(RegexResult))

    '                ' Update answer
    '                Answer = RegexResult

    '            Catch
    '                MsgBox("Something went wrong. Double check everything to make sure it is correct, and then try again.")
    '                Exit Sub
    '            End Try

    '        End If


    '    End Sub

End Class
