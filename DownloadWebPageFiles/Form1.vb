Option Strict On

Imports System.IO
Imports System.Net

Public Class Form1
    Private mcolFiles As List(Of KeyValuePair(Of String, String))   '(URL, Filename)
    Private mcolExts As List(Of String)

    Public Property Files() As List(Of KeyValuePair(Of String, String))
        Get
            If IsNothing(mcolFiles) Then
                mcolFiles = New List(Of KeyValuePair(Of String, String))
            End If
            Return mcolFiles
        End Get
        Set(ByVal value As List(Of KeyValuePair(Of String, String)))
            mcolFiles = value
        End Set
    End Property

    Private Sub Form1_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        progBar.Visible = False

        txtDestDir.Text = "C:\Users\gcook\Documents\HamRadio\Microwave\Course\Test"
        txtUrl.Text = "http://www.ittc.ku.edu/~jstiles/723/handouts"
        txtFileExt.Text = ".pdf"

        txtDestDir.Text = "C:\Users\gcook\Documents\French\course"
        txtUrl.Text = "http://fsi-languages.yojik.eu/languages/french-basic.html"
        txtFileExt.Text = ".mp3"

        txtDestDir.Text = "C:\Users\gcook\Documents\French\course"
        txtUrl.Text = "https://google.com"
        txtFileExt.Text = ".mp3"


        cmdDownloadFiles.Enabled = False
        EnableButtons()
    End Sub

    Private Sub EnableButtons()
        If Not String.IsNullOrEmpty(Trim(txtFileExt.Text)) And Not String.IsNullOrEmpty(Trim(txtDestDir.Text)) And Not String.IsNullOrEmpty(Trim(txtUrl.Text)) Then
            cmdLoadPage.Enabled = True
        Else
            cmdLoadPage.Enabled = False
        End If
        If Files.Count > 0 Then
            cmdDownloadFiles.Enabled = True
            cmdDownloadFiles.Text = "Download " & Files.Count.ToString & " Files"
        Else
            cmdDownloadFiles.Enabled = False
            cmdDownloadFiles.Text = "Download 0 Files"
        End If
        If lstFiles.Items.Count > 0 And Not cmdDownloadFiles.Enabled Then
            lstFiles.Items.Clear()
            chkAll.Checked = False
        End If
    End Sub

    Private Sub cmdBrowse_Click(sender As System.Object, e As System.EventArgs) Handles cmdBrowse.Click
        Dim dlg As New FolderBrowserDialog
        With dlg
            If Not String.IsNullOrEmpty(txtDestDir.Text) Then
                .SelectedPath = txtDestDir.Text
            End If
            If .ShowDialog = Windows.Forms.DialogResult.OK Then
                txtDestDir.Text = .SelectedPath
            End If
        End With
    End Sub

    Private Sub cmdPasteUrl_Click(sender As System.Object, e As System.EventArgs) Handles cmdPasteUrl.Click
        If Clipboard.ContainsText Then
            txtUrl.Text = Clipboard.GetText
        End If
    End Sub

    Private Sub cmdDownloadFiles_Click(sender As System.Object, e As System.EventArgs) Handles cmdDownloadFiles.Click
        txtUrl.Text = Trim(txtUrl.Text)
        txtDestDir.Text = Trim(txtDestDir.Text)
        txtFileExt.Text = Trim(txtFileExt.Text)

        Dim curs As Cursor = Cursor
        Cursor = Cursors.WaitCursor
        progBar.Visible = True
        DownloadFiles(txtDestDir.Text)
        progBar.Visible = False
        Cursor = curs
        MsgBox("File download complete.")
    End Sub

    Private Sub txtUrl_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtUrl.TextChanged
        ClearFiles()
        EnableButtons()
    End Sub

    Private Sub txtDestDir_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtDestDir.TextChanged
        ClearFiles()
        EnableButtons()
    End Sub

    Private Sub txtFileExt_TextChanged(sender As System.Object, e As System.EventArgs) Handles txtFileExt.TextChanged
        LoadExtensions()
        ClearFiles()
        EnableButtons()
    End Sub

    Private mstrOriginalUrl As String
    Private Sub cmdLoadPage_Click(sender As System.Object, e As System.EventArgs) Handles cmdLoadPage.Click
        LoadExtensions()
        mstrOriginalUrl = Trim(txtUrl.Text)
        txtUrl.Text = mstrOriginalUrl
        txtDestDir.Text = Trim(txtDestDir.Text)
        txtFileExt.Text = Trim(txtFileExt.Text)
        ClearFiles()
        wbc.Navigate(txtUrl.Text)
    End Sub

    Private Sub SetChecks(vbChecked As Boolean)
        If lstFiles.Items.Count > 0 Then
            For liIdx As Integer = 0 To lstFiles.Items.Count - 1
                lstFiles.SetItemChecked(liIdx, vbChecked)
            Next
        End If
    End Sub

    Private Sub wbc_DocumentCompleted(sender As Object, e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles wbc.DocumentCompleted
        GetFilenamesFromWebPage(txtFileExt.Text)
        EnableButtons()
    End Sub

    Private Sub DownloadFiles(vstrDestDir As String)

        Dim lstrUrl As String = Nothing
        Dim lobjClient As New WebClient()

        With progBar
            .Minimum = 0
            .Maximum = mcolFiles.Count
            .Value = 0
        End With

        Dim iter As IEnumerator(Of KeyValuePair(Of String, String)) = mcolFiles.GetEnumerator
        While iter.MoveNext
            'Dim baseUri As New Uri(vstrURL)
            'Dim myUri As New Uri(baseUri, iter.Current)
            ''Uri.TryCreate(
            'lstrUrl = myUri.ToString

            'lstrUrl = vstrURL & iter.Current

            Try
                If IsFileChecked(iter.Current.Value) Then
                    lobjClient.DownloadFile(iter.Current.Key, Path.Combine(vstrDestDir, iter.Current.Value))
                    progBar.Value += 1
                End If
            Catch ex As Exception
                Debug.Print("ERROR: File=" & lstrUrl & vbCrLf & ex.ToString)
            End Try
        End While
    End Sub

    Private Sub LoadExtensions()
        mcolExts = New List(Of String)
        Dim x() As String = Split(txtFileExt.Text, ";")
        For Each Str As String In x
            mcolExts.Add(Str)
        Next
    End Sub

    Private Function IsRightExtension(vstrIn As String) As Boolean

        For Each s As String In mcolExts
            Dim liLen As Integer = s.Length
            If vstrIn.Substring(vstrIn.Length - liLen, liLen) = s Then
                Return True
            End If
        Next

        Return False
    End Function

    Private Sub GetFilenamesFromWebPage(lstrFileExtension As String)
        Try
            Dim col As HtmlElementCollection
            ClearFiles()
            If Not IsNothing(wbc.Document) Then
                With wbc.Document
                    col = .GetElementsByTagName("a")

                    For Each elem As HtmlElement In col
                        Dim lstrURL As String = elem.GetAttribute("href")
                        'If Not String.IsNullOrEmpty(lstrURL) AndAlso lstrURL.Substring(lstrURL.Length - 4, 4) = lstrFileExtension Then
                        If Not String.IsNullOrEmpty(lstrURL) AndAlso IsRightExtension(lstrURL) Then
                            Dim baseUri As New Uri(lstrURL)
                            Dim lstrSegs() As String = baseUri.Segments
                            Dim lstrFName As String = lstrSegs(lstrSegs.GetUpperBound(0)).Replace("%20", "")
                            If Not IsFileInFiles(lstrFName) Then
                                Files.Add(New KeyValuePair(Of String, String)(baseUri.ToString, lstrFName))
                                lstFiles.SetItemChecked(lstFiles.Items.Add(lstrFName), True)
                            End If
                        End If
                    Next
                End With
            End If
            chkAll.Checked = True
        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Function IsFileInFiles(lstrFName As String) As Boolean
        For Each tmp As KeyValuePair(Of String, String) In Files
            If tmp.Value = lstrFName Then Return True
        Next
        Return False
    End Function

    Private Function IsFileChecked(lstrFName As String) As Boolean
        For Each tmp As Object In lstFiles.CheckedItems
            If tmp.ToString = lstrFName Then Return True
        Next
        Return False
    End Function

    Private Sub ClearFiles()
        Files.Clear()
        lstFiles.Items.Clear()
    End Sub
#Region "Load Test Data"

    Private Function LoadList1() As List(Of String)
        Dim x As New List(Of String)
        x.Add("2_1 Lumped Element Circuit Model.pdf")
        x.Add("2_1_Lumped_Element_Circuit_Model_package.pdf")
        x.Add("2_3 Terminated Lossless Line.pdf")
        Return x
    End Function

    Private Function LoadList() As List(Of String)
        Dim x As New List(Of String)
        x.Add("2_1 Lumped Element Circuit Model.pdf")
        x.Add("2_1_Lumped_Element_Circuit_Model_package.pdf")
        x.Add("2_3 Terminated Lossless Line.pdf")
        x.Add("2_3_Terminated_Lossless_Line_package.pdf")
        x.Add("2_4 The Smith Chart.pdf")
        x.Add("2_4_The_Smith_Chart_package.pdf")
        x.Add("2_5_The_Quarter_Wave_Transformer_package.pdf")
        x.Add("2_6 Generator and Load Mismatches.pdf")
        x.Add("2_6_Generator_and_Load_Mismatches_package.pdf")
        x.Add("2_7 Lossy Transmission Lines.pdf")
        x.Add("2_7_Lossy_Transmission_Lines_package.pdf")
        x.Add("3  Transmission Lines and Waveguides.pdf")
        x.Add("4_2 Impedance and Admittance Matricies.pdf")
        x.Add("4_2 Impedance and Admittance Matricies blank.pdf")
        x.Add("4_3 The Scattering Matrix.pdf")
        x.Add("4_3 The Scattering Matrix present.pdf")
        x.Add("4_4 The Transmission Matrix.pdf")
        x.Add("4_5 Signal Flow Graphs.pdf")
        x.Add("5_2 Single_Stub Tuning.pdf")
        x.Add("5_3 Double Stub Tuning.pdf")
        x.Add("5_3 Double Stub Tuning present.pdf")
        x.Add("5_4 The Quarter Wave Transformer.pdf")
        x.Add("5_4 The Quarter Wave Transformer present.pdf")
        x.Add("5_5 The Theory of Small Reflections.pdf")
        x.Add("5_6 Binomial Multisection Matching Transformers.pdf")
        x.Add("5_7 Chebyshev Multisection Matching Transformers.pdf")
        x.Add("5_8 Tapered Lines.pdf")
        x.Add("7_1 Basic Properties of Dividers and Couplers.pdf")
        x.Add("7_2 The T Junction power Divider.pdf")
        x.Add("7_3 The Wilkinson Power Divider.pdf")
        x.Add("7_5 The Quadrature Hybrid.pdf")
        x.Add("7_6 Coupled Line Directional Couplers.pdf")
        x.Add("7_8 The 180 Degree Hybrid.pdf")
        x.Add("8_4 Filter Transformations.pdf")
        x.Add("8_5 Filter Implementation.pdf")
        x.Add("8_5 Stepped Impedance Low Pass Filters.pdf")
        x.Add("10_3 RF Diode Charactertics.pdf")
        x.Add("10_4 RF Transistor Characteristics.pdf")
        x.Add("10_5 Microwave Integrated Circuits.pdf")
        x.Add("11_1 Two-Port Power Gains.pdf")
        x.Add("11_2 Stability.pdf")
        x.Add("11_3 Single-Stage Amp Design.pdf")
        x.Add("723 Design Project_2 s06.pdf")
        x.Add("723 Design Project_2 s09 report format.doc")
        x.Add("723_Design_Project_1.pdf")
        x.Add("723_Design_Project_1_s_07.pdf")
        x.Add("A Comparison of Common Transmission Lines and Waveguides.pdf")
        x.Add("A Comparison of Common Transmission Lines and Waveguides present.pdf")
        x.Add("AN-95-1A.MOV")
        x.Add("A Quad Mode Analsys of the Quadrature Hybrid.pdf")
        x.Add("A Transmission Line Connecting Source.doc")
        x.Add("A Transmission Line Connecting Source.pdf")
        x.Add("A Transmission Line Connecting Source and Load present.pdf")
        x.Add("Admittance.doc")
        x.Add("Admittance.pdf")
        x.Add("Admittance and the Smith Chart.pdf")
        x.Add("Admittance and the Smith Chart present.pdf")
        x.Add("Admittance present.pdf")
        x.Add("Analysis and Design of Coupled Line Couplers.pdf")
        x.Add("Characteristic Impedance present.pdf")
        x.Add("Circuit Symmetry.pdf")
        x.Add("Circuit Symmetry present.pdf")
        x.Add("Circulators.pdf")
        x.Add("Coaxial Connectors.pdf")
        x.Add("Coaxial Connectors present.pdf")
        x.Add("Coaxial Transmission Lines.pdf")
        x.Add("Coaxial Transmission Lines present.pdf")
        x.Add("Connecting Source and Load present.pdf")
        x.Add("Coupled Line Couplers.pdf")
        x.Add("Delivered Power.pdf")
        x.Add("Delivered Power present.pdf")
        x.Add("Design for Specified Gain.pdf")
        x.Add("Double Stub Tuning.pdf")
        x.Add("Double Stub Tuning present.pdf")
        x.Add("EECS723_intro_package.pdf")
        x.Add("Example A Lossless Reciprocal Network.pdf")
        x.Add("Example Admittance Calculations with the Smith Chart.pdf")
        x.Add("Example Another Boundary Value Problem.pdf")
        x.Add("Example Boundary Conditions.pdf")
        x.Add("Example Conservation of Energy and You.pdf")
        x.Add("Example Decomposition of Signal Flow Graph.pdf")
        x.Add("Example Determining the Scattering Matrix.pdf")
        x.Add("Example Determining the tl length.pdf")
        x.Add("Example Input Impedance.pdf")
        x.Add("Example Odd Even Mode Circuit Analysis.pdf")
        x.Add("Example Scattering Parameters.pdf")
        x.Add("Example Shorted Transmission Line.pdf")
        x.Add("Example Signal Flow Graph Analysis.pdf")
        x.Add("Example The Admittance Matrix.pdf")
        x.Add("Example The Load Impedance.pdf")
        x.Add("Example The Scattering Matrix.pdf")
        x.Add("Example The Tranmission Coefficient T.pdf")
        x.Add("Example Theory of Small Reflections.pdf")
        x.Add("Example Using Symmetry to Determine S.pdf")
        x.Add("Example Using the Impedance Matrix.pdf")
        x.Add("Example_Boundary_Conditions.pdf")
        x.Add("Example_Input_Impedance.pdf")
        x.Add("Example boundary conditions with sources.pdf")
        x.Add("Filter Design Worksheet.pdf")
        x.Add("Filter Dispersion.pdf")
        x.Add("Filter Realizations Using Lumped Elements.pdf")
        x.Add("Filter Spec Sheet.pdf")
        x.Add("Filter Transformations.pdf")
        x.Add("Filters.pdf")
        x.Add("Generalized Scattering Parameters.pdf")
        x.Add("Generalized Scattering Parameters present.pdf")
        x.Add("I_V_Z or.pdf")
        x.Add("I_V_Z or present.pdf")
        x.Add("Incident Reflected and Absorbed Power.pdf")
        x.Add("Incident Reflected and Absorbed Power present.pdf")
        x.Add("Kurodas Identities .pdf")
        x.Add("L Network Analysis.pdf")
        x.Add("L Network Analysis present.pdf")
        x.Add("Line Impedance.pdf")
        x.Add("Line Impedance present.pdf")
        x.Add("Line_Impedance.pdf")
        x.Add("MLR 3 port network .pdf")
        x.Add("MLR 3 port network present .pdf")
        x.Add("MLR  4 port network.pdf")
        x.Add("MLR  4 port network present.pdf")
        x.Add("Mapping Z to Gamma.pdf")
        x.Add("Mapping Z to Gamma present.pdf")
        x.Add("Matched reciprocal lossless.pdf")
        x.Add("Matched reciprocal lossless 723.pdf")
        x.Add("Matched reciprocal lossless present.pdf")
        x.Add("Matching Networks.pdf")
        x.Add("Matching Networks and Transmission Lines.pdf")
        x.Add("Matching Networks and Transmission Lines present.pdf")
        x.Add("Matching Networks present.pdf")
        x.Add("Maximally Flat Transformer Functions.pdf")
        x.Add("Maximally Flat Transformer Functions present.pdf")
        x.Add("Maximum Gain Amplifiers.pdf")
        x.Add("Microstrip Transmission Lines.pdf")
        x.Add("Microstrip Transmission Lines present.pdf")
        x.Add("Microwave Filter Design.pdf")
        x.Add("Microwave Integrated Circuits.pdf")
        x.Add("Microwave Sources.pdf")
        x.Add("Microwave Switches.pdf")
        x.Add("MultiSection Coupled Line Couplers.pdf")
        x.Add("Multiple Reflection Viewpoint.pdf")
        x.Add("Odd Even Mode Analysis.pdf")
        x.Add("Odd Even Mode Analysis present.pdf")
        x.Add("PIN Diode Microwave Switches.pdf")
        x.Add("PIN Diodes.pdf")
        x.Add("Parallel Rule.pdf")
        x.Add("Parallel Rule present.pdf")
        x.Add("Power Flow and Return Loss.pdf")
        x.Add("Power_Flow_and_Return_Loss.pdf")
        x.Add("Printed Circuit Board Transmission Lines.pdf")
        x.Add("Printed Circuit Board Transmission Lines present.pdf")
        x.Add("Printed_Circuit_Board_Transmission_Lines.pdf")
        x.Add("RF Transistors.pdf")
        x.Add("Reciprocal and Lossless Devices.pdf")
        x.Add("Reciprocal and Lossless Devices present.pdf")
        x.Add("Reflection Coefficient present.pdf")
        x.Add("Return Loss and VSWR.pdf")
        x.Add("Return Loss and VSWR present.pdf")
        x.Add("Review of Complex Arithmetic.pdf")
        x.Add("Richards Tranformation .pdf")
        x.Add("Richards Transformation.pdf")
        x.Add("Rules for Signal Flow Graph Decomposition.pdf")
        x.Add("SP 5.4-1.pdf")
        x.Add("SP 5.4-1 soln.pdf")
        x.Add("Self Loop Rule.pdf")
        x.Add("Self Loop Rule present.pdf")
        x.Add("Series Rule.pdf")
        x.Add("Series Rule present.pdf")
        x.Add("Series Stub Tuning.pdf")
        x.Add("Series Stub Tuning present.pdf")
        x.Add("Shunt Stub Tuning.pdf")
        x.Add("Shunt Stub Tuning present.pdf")
        x.Add("Signal Flow Graphs.pdf")
        x.Add("Signal Flow Graphs present.pdf")
        x.Add("Smith Chart Geograhpy present.pdf")
        x.Add("Smith Chart Geography.pdf")
        x.Add("Special Cases of Source and Load Impedance.pdf")
        x.Add("Special Cases of Source and Load Impedance present.pdf")
        x.Add("Special Cases of Source and Load present.pdf")
        x.Add("Special Values of Load Impedance.pdf")
        x.Add("Special Values of Load Impedance present.pdf")
        x.Add("Special_Values_of_Load_Impedance.pdf")
        x.Add("Splitting Rule.pdf")
        x.Add("Splitting Rule present.pdf")
        x.Add("Stability.pdf")
        x.Add("Stepped Impedance Low Pass Filters.pdf")
        x.Add("Stripline Transmission Lines.pdf")
        x.Add("Stripline Transmission Lines present.pdf")
        x.Add("Symmetric Circuit Analysis.pdf")
        x.Add("Symmetric Circuit Analysis present.pdf")
        x.Add("Tapered Lines.pdf")
        x.Add("Tapered Lines present.pdf")
        x.Add("The 3 port Coupler.pdf")
        x.Add("The 3 port Coupler present.pdf")
        x.Add("The 4 port coupler.pdf")
        x.Add("The 180 Degree Hybrid 723.pdf")
        x.Add("The Admittance  Matrix.pdf")
        x.Add("The Admittance  Matrix present.pdf")
        x.Add("The Binomial Multisection Matching Transformer.pdf")
        x.Add("The Binomial Multisection Matching Transformer present.pdf")
        x.Add("The Characteristic Impedance of a Transmission Line.pdf")
        x.Add("The Chebyshev Matching Transformer.pdf")
        x.Add("The Complex Gamma Plane.pdf")
        x.Add("The Complex Gamma Plane present.pdf")
        x.Add("The Complex Propagation Constant.pdf")
        x.Add("The Complex Propagation constant present.pdf")
        x.Add("The Directional Coupler.pdf")
        x.Add("The Directional Coupler present.pdf")
        x.Add("The Distortionless Line.pdf")
        x.Add("The Distortionless Line present.pdf")
        x.Add("The FET Small Signal Model.pdf")
        x.Add("The Filter Transfer Function.pdf")
        x.Add("The Frequency Response of a Quarter.pdf")
        x.Add("The Ideal Gain Element.pdf")
        x.Add("The Impedance Matrix.pdf")
        x.Add("The Impedance Matrix present.pdf")
        x.Add("The Insertion Loss Method.pdf")
        x.Add("The Linear Phase Filter.pdf")
        x.Add("The Lossless Divider.pdf")
        x.Add("The Lossless Transmission Line.pdf")
        x.Add("The Lossless Transmission Line present.pdf")
        x.Add("The Multisection Transformer.pdf")
        x.Add("The Outer Scale.pdf")
        x.Add("The Outer Scale present.pdf")
        x.Add("The Powers that Be.pdf")
        x.Add("The Propagation Series.pdf")
        x.Add("The Propagation Series present.pdf")
        x.Add("The Quadrature Hybrid Coupler 723.pdf")
        x.Add("The Quarter Wave Transformer.pdf")
        x.Add("The Quarter Wave Transformer Yet Again.pdf")
        x.Add("The Reflection Coefficient.pdf")
        x.Add("The Reflection Coefficient Transformation.pdf")
        x.Add("The Reflection Coefficient Transformation present.pdf")
        x.Add("The Resistive Divider.pdf")
        x.Add("The Scattering Matrix.pdf")
        x.Add("The Scattering Matrix 723.pdf")
        x.Add("The Scattering Matrix of a Connector.pdf")
        x.Add("The Scattering Matrix present.pdf")
        x.Add("The Signal Flow Graph of a QuarterWave Transformer.pdf")
        x.Add("The Smith Chart.pdf")
        x.Add("The Smith Chart present.pdf")
        x.Add("The T Junction Power Divider.pdf")
        x.Add("The Telegrapher Equations.pdf")
        x.Add("The Telegrapher Equations present.pdf")
        x.Add("The Terminated Lossless Line present.pdf")
        x.Add("The Terminated Lossless Transmission.pdf")
        x.Add("The Terminated Lossless Transmission Line.pdf")
        x.Add("The Theory of Small Reflections.pdf")
        x.Add("The Transmission Line Wave Equation.pdf")
        x.Add("The Transmission Line Wave Equation present.pdf")
        x.Add("The Transmission Matrix.pdf")
        x.Add("The Transmission Matrix present.pdf")
        x.Add("The Wilkinson Power Divider 723.pdf")
        x.Add("The_Binomial_Multisection_Matching_Transformer.pdf")
        x.Add("The_Characteristic_Impedance_of_a_Transmission_Line.pdf")
        x.Add("The_Complex_Propagation_Constant.pdf")
        x.Add("The_Lossless_Transmission_Line.pdf")
        x.Add("The_Reflection_Coefficient.pdf")
        x.Add("The_Reflection_Coefficient_Transformation.pdf")
        x.Add("The_Telegrapher_Equations.pdf")
        x.Add("The_Terminated_Lossless_Transmission.pdf")
        x.Add("The_Tranmission_Coefficient_T.pdf")
        x.Add("The_Transmission_Line_Wave_Equation.pdf")
        x.Add("Time Harmonic Solutions for Linear Circuits.pdf")
        x.Add("Time Harmonic Solutions for Linear Circuits present.pdf")
        x.Add("Time Harmonic Solutions for Transmission Lines present.pdf")
        x.Add("Transformations on the Complex.pdf")
        x.Add("Transformations on the Complex G plane present.pdf")
        x.Add("Transistors as a gain element.pdf")
        x.Add("Transmission Line Input Impedance.pdf")
        x.Add("Transmission Line Input Impedance present.pdf")
        x.Add("Transmission_Line_Input_Impedance.pdf")
        x.Add("Transmission_Lines_package.pdf")
        x.Add("Turning a Gain Element into an Amplifier.pdf")
        x.Add("Two Port Power Gains.pdf")
        x.Add("VSWR.pdf")
        x.Add("Waveguide.pdf")
        x.Add("Waveguide present.pdf")
        x.Add("Wilkinson Divider Even and Odd Mode Analysis.pdf")
        x.Add("Wilkinson Odd Even Mode Analysis.pdf")
        x.Add("Zin Calculations using the Smith Chart.pdf")
        x.Add("Zin Calculations using the Smith Chart present.pdf")
        x.Add("chapter_3_Transmission_Lines_and_Waveguides_lecture.pdf")
        x.Add("chapter_3_Transmission_Lines_and_Waveguides_package.pdf")
        x.Add("section 5_1 Matching with Lumped Elements.pdf")
        x.Add("section_2_1_The_Lumped_Element_Circuit_Model_package.pdf")
        x.Add("section_2_1_The_Lumped_Element_Circuit_Model_present.pdf")
        x.Add("section_2_3_The_Terminated_Lossless_Transmission_Line_lecture.pdf")
        x.Add("section_2_3_The_Terminated_Lossless_Transmission_Line_package.pdf")
        x.Add("section_2_3_The_Terminated_Lossless_Transmission_Line_present.pdf")
        x.Add("section_2_4_The_Smith_Chart_package.pdf")
        x.Add("section_2_4_The_Smith_Chart_present.pdf")
        x.Add("section_2_6_Generator_and_Load_Mismatch_lecture.pdf")
        x.Add("section_2_6_Generator_and_Load_Mismatch_package.pdf")
        x.Add("section_2_7_Lossy_Transmission_Lines_package.pdf")
        x.Add("section_2_7_Lossy_Transmission_Lines_present.pdf")
        x.Add("section_4_2_Impedance_and_Admittance_Matricies_lecture.pdf")
        x.Add("section_4_2_Impedance_and_Admittance_Matricies_package.pdf")
        x.Add("section_4_2_Impedance_and_Admittance_Matricies_present.pdf")
        x.Add("section_4_3_The_Scattering_Matrix_lecture.pdf")
        x.Add("section_4_3_The_Scattering_Matrix_package.pdf")
        x.Add("section_4_4_The_Transmission_Matrix_lecture.pdf")
        x.Add("section_4_4_The_Transmission_Matrix_package.pdf")
        x.Add("section_4_5_Signal_Flow_Graphs_lecture.pdf")
        x.Add("section_4_5_Signal_Flow_Graphs_package.pdf")
        x.Add("section_5_1_Matching_with_Lumped_Elements_lecture.pdf")
        x.Add("section_5_1_Matching_with_Lumped_Elements_package.pdf")
        x.Add("section_5_1_Matching_with_Lumped_Elements_present.pdf")
        x.Add("section_5_2_Single_Stub_Tuning_lecture.pdf")
        x.Add("section_5_2_Single_Stub_Tuning_package.pdf")
        x.Add("section_5_2_Single_Stub_Tuning_present.pdf")
        x.Add("section_5_3_Double_Stub_Tuning_lecture.pdf")
        x.Add("section_5_3_Double_Stub_Tuning_package.pdf")
        x.Add("section_5_4_The_Quarter_Wave_Transformer_package.pdf")
        x.Add("section_5_4_The_Quarter_Wave_Transformer_present.pdf")
        x.Add("section_5_5_The_Theory_of_Small_Reflections_package.pdf")
        x.Add("section_5_5_The_Theory_of_Small_Reflections_present.pdf")
        x.Add("section_5_6_Binomial_Multisection_Matching_Transformer_lecture.pdf")
        x.Add("section_5_6_Binomial_Multisection_Matching_Transformer_package.pdf")
        x.Add("section_5_6_Binomial_Multisection_Matching_Transformer_present.pdf")
        x.Add("section_5_7_Chebyshev_Multisection_Matching_Transformer_package.pdf")
        x.Add("section_5_7_Chebyshev_Multisection_Matching_Transformer_present.pdf")
        x.Add("section_5_8_Tapered_Lines_lecture.pdf")
        x.Add("section_5_8_Tapered_Lines_package.pdf")
        x.Add("section_5_8_Tapered_Lines_present.pdf")
        x.Add("section_7_1_Basic_Properties_of_Dividers_and_Couplers_lecture.pdf")
        x.Add("section_7_1_Basic_Properties_of_Dividers_and_Couplers_package.pdf")
        x.Add("section_7_1_Basic_Properties_of_Dividers_and_Couplers_present.pdf")
        x.Add("section_7_1_Properties_of_dividers_and_couplers_package.pdf")
        x.Add("section_7_2_The_T_Junction_Power_Divider_package.pdf")
        x.Add("section_7_2_The_T_Junction_Power_Divider_present.pdf")
        x.Add("section_7_3_The_Wilkinson_Power_Divider_package.pdf")
        x.Add("section_7_3_The_Wilkinson_Power_Divider_present.pdf")
        x.Add("section_7_5_The_Quadrature_Hybrid_package.pdf")
        x.Add("section_7_5_The_Quadrature_Hybrid_present.pdf")
        x.Add("section_7_6_Coupled_Line_Couplers_package.pdf")
        x.Add("section_7_6_Coupled_Line_Directional_Couplers_package.pdf")
        x.Add("section_7_6_Coupled_Line_Directional_Couplers_present.pdf")
        x.Add("section_7_8_The_180_Degree_Hybrid_package.pdf")
        x.Add("section_7_8_The_180_Degree_Hybrid_present.pdf")
        x.Add("section_8_3_Filter_Desigin_by_the_Insertion_Loss_Method_present.pdf")
        x.Add("section_8_3_Filter_Design_by_the_Insertion_Loss_Method_package.pdf")
        x.Add("section_8_4_Filter_Transformations_package.pdf")
        x.Add("section_8_4_Filter_Transformations_present.pdf")
        x.Add("section_8_5_Filter_Implementation_package.pdf")
        x.Add("section_8_5_Filter_Implementations_present.pdf")
        x.Add("section_8_6_Stepped_Impedance_Low_Pass_Filters_package.pdf")
        x.Add("section_8_6_Stepped_Impedance_Low_Pass_Filters_present.pdf")
        x.Add("section_10_3_RF_Diode_Characteristics_package.pdf")
        x.Add("section_10_4_RF_Transistor_Characteristics.pdf")
        x.Add("section_10_4_RF_Transistor_Characteristics_package.pdf")
        x.Add("section_10_5_Microwave_Integrated_Circuits_package.pdf")
        x.Add("section_11_1_Two_Port_Power_Gains_package.pdf")
        x.Add("section_11_2_Stability_package.pdf")
        x.Add("section_11_3_Single_Stage_Transistor_Amp_Design_package.pdf")

        Return x
    End Function

#End Region

    Private Sub chkAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkAll.CheckedChanged
        SetChecks(chkAll.Checked)
    End Sub

    Private Sub wbc_Navigated(sender As Object, e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles wbc.Navigated
        txtUrl.Text = wbc.Url.ToString

        'If mstrOriginalUrl <> wbc.Url.ToString Then
        '    Debug.Print("txtUrl:" & wbc.Url.ToString)
        'End If

        'If mstrOriginalUrl <> wbc.Url.ToString Then
        '    Debug.Print("wbc.Url.ToString" & wbc.Url.ToString)
        'End If
    End Sub

    Private Sub Form1_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        progBar.Top = Me.ClientSize.Height - progBar.Height
        lstFiles.Height = Me.ClientSize.Height - lstFiles.Top - progBar.Height - 5
        wbc.Height = Me.ClientSize.Height - wbc.Top - progBar.Height - 5
        wbc.Width = Me.ClientSize.Width - wbc.Left - 5
    End Sub

End Class
