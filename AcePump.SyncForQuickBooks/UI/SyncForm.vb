Imports System.IO
Imports AcePump.SyncForQuickBooks.Qb
Imports AcePump.SyncForQuickBooks.UI.Tasks

Namespace UI
    Public Class SyncForm

        Private LoggedInUsername As String

        Private Sub btnDownloadTickets_Click(sender As System.Object, e As System.EventArgs) Handles btnDownloadTickets.Click
            DownloadTickets()
        End Sub

        Private Sub DownloadTickets()
            Using workerDialog As New WorkerForm(GetType(UIProcessDeliveryTicketTask))
                workerDialog.ShowDialog()
            End Using
        End Sub

        Public Sub DisplayChooseFileDialog()
            Dim result As DialogResult
            Dim fileName As String
            Using openFileDialogQuickbooks As New OpenFileDialog
                openFileDialogQuickbooks.Title = "Please select the Quickbooks file you'd like to use."
                openFileDialogQuickbooks.Filter = "Quickbooks Company File (*.QBW)|*.QBW"
                openFileDialogQuickbooks.ValidateNames = False
                openFileDialogQuickbooks.CheckFileExists = True
                openFileDialogQuickbooks.CheckPathExists = True

                result = openFileDialogQuickbooks.ShowDialog()
                fileName = openFileDialogQuickbooks.FileName
            End Using
            Cursor.Current = Cursors.WaitCursor

            If result = DialogResult.OK Then
                SaveNewFileName(fileName)
            End If

            SetupFormWithQBFileName()
        End Sub

        Private Sub SaveNewFileName(fileName As String)
            Cursor.Current = Cursors.WaitCursor
            Dim previousFileName As String = My.Settings.QBCompanyFile

            My.Settings.QBCompanyFile = fileName
            My.Settings.Save()

            If Not TestQbConnection(fileName) Then
                My.Settings.QBCompanyFile = previousFileName
                My.Settings.Save()
            End If
        End Sub

        Private Sub ClearCompanyFile()
            My.Settings.QBCompanyFile = ""
            My.Settings.Save()

            SetupFormWithQBFileName()
        End Sub

        Private Sub ChooseQBFileButton_Click(sender As System.Object, e As System.EventArgs) Handles ChooseQBFileButton.Click
            Cursor.Current = Cursors.WaitCursor
            DisplayChooseFileDialog()
            Cursor.Current = Cursors.Default
        End Sub

        Private Sub EnableDownloadButtons(enable As Boolean)
            btnDownloadTickets.Enabled = enable
        End Sub

        Private Sub SetupFormWithQBFileName()
            Dim fileName = My.Settings.QBCompanyFile
            If String.IsNullOrEmpty(fileName) Then
                QBFileExistsLabel.Text = "Please choose a QuickBooks Company File to continue."
                ChooseQBFileButton.Text = "Choose a company file"
                EnableDownloadButtons(False)
            Else
                QBFileExistsLabel.Text = "Using QB File: " & Path.GetFileName(fileName)
                ChooseQBFileButton.Text = "Choose a different QuickBooks file"
                EnableDownloadButtons(True)
            End If
        End Sub

        Private Sub Synchronize_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
            SetVersion()
            SetupFormWithQBFileName()
            ToolTip1.SetToolTip(Me.ClearFileButton, "Clear Company File")
            If LoggedInUsername.ToLower() = "admin" Then
                Me.btnCheckReadyTickets.Show()
                Me.btnTestQb.Show()
            End If
        End Sub

        Private Sub SetVersion()
            If (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed) Then
                With System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
                    VersonLabel.Text = "V" & .Major & "." & .Minor & "." & .Build & "." & .Revision
                End With
            Else
                VersonLabel.Text = Application.ProductVersion
            End If
        End Sub

        Private Function TestQbConnection(fileName As String) As Boolean
            Cursor.Current = Cursors.WaitCursor

            If String.IsNullOrEmpty(fileName) Then
                Return False
            Else
                Dim errorMessage As String = ""
                Dim session As New QbSession(My.Settings.QBCompanyFile.ToString())
                Try
                    Using session
                        Dim message As String
                        If session.TestConnection(message) Then
                            MessageBox.Show(message)
                        Else
                            errorMessage = message
                        End If
                        Return True
                    End Using
                Catch ex As Exception
                    session.Dispose()
                    errorMessage = ex.Message
                End Try

                Cursor.Current = Cursors.WaitCursor

                If Not String.IsNullOrWhiteSpace(errorMessage) Then
                    Dim result As DialogResult
                    Using authInstructionsDialog As New QbAuthorizationInstructions
                        authInstructionsDialog.FileName = fileName
                        authInstructionsDialog.QbResponseMessage = errorMessage
                        result = authInstructionsDialog.ShowDialog()
                    End Using

                    Cursor.Current = Cursors.WaitCursor

                    Select Case result
                        Case DialogResult.Retry
                            Return TestQbConnection(fileName)
                        Case DialogResult.Cancel
                            Return False
                        Case Else
                            Return False
                    End Select
                End If

                Return True
            End If
        End Function

        Private Sub ClearFileButton_Click(sender As System.Object, e As System.EventArgs) Handles ClearFileButton.Click
            ClearCompanyFile()
        End Sub

        Public Sub New(username As String)

            ' This call is required by the designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            LoggedInUsername = username
        End Sub

        Private Sub btnCheckReadyTickets_Click(sender As Object, e As EventArgs) Handles btnCheckReadyTickets.Click
            Using workerDialog As New WorkerForm(GetType(UICountReadyDeliveryticketsTask))
                workerDialog.ShowDialog()
            End Using
        End Sub

        Private Sub btnTestQb_Click(sender As Object, e As EventArgs) Handles btnTestQb.Click
            If Not String.IsNullOrEmpty(My.Settings.QBCompanyFile) Then
                If Not TestQbConnection(My.Settings.QBCompanyFile) Then
                    MessageBox.Show("Failed!")
                End If
            Else
                MessageBox.Show("No file set")
            End If
        End Sub
    End Class
End Namespace