Imports AcePump.SyncForQuickBooks.PtpApi

Namespace UI
    Public Class SyncLogin

        ' TODO: Insert code to perform custom authentication using the provided username and password 
        ' (See http://go.microsoft.com/fwlink/?LinkId=35339).  
        ' The custom principal can then be attached to the current thread's principal as follows: 
        '     My.User.CurrentPrincipal = CustomPrincipal
        ' where CustomPrincipal is the IPrincipal implementation used to perform authentication. 
        ' Subsequently, My.User will return identity information encapsulated in the CustomPrincipal object
        ' such as the username, display name, etc.

        Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
            If Validate() Then
                Login()
                If DialogResult = DialogResult.OK Then
                    Me.Close()
                End If
            End If
        End Sub

        Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
            Me.Close()
        End Sub

        Private Sub Login()
            LoginMessageLabel.Visible = False
            Dim tokenResponse As ApiTokenResponse = PtpApi.ApiConnector.GetToken(UsernameTextBox.Text, PasswordTextBox.Text)
            If tokenResponse.ResponseType = ApiTokenResponseType.SuccessfulLogin Then
                ApiConnector.ApiToken = tokenResponse
                DialogResult = DialogResult.OK
            ElseIf tokenResponse.ResponseType = ApiTokenResponseType.Unauthorized Then
                LoginMessageLabel.Text = "Login was unsuccessful. Please check user name and password and make sure the correct permissions are set." & vbNewLine & tokenResponse.Message
                LoginMessageLabel.Visible = True
            Else
                LoginMessageLabel.Text = "There was a problem connecting to the program online. Please try again later or contact your administrator."
                LoginMessageLabel.Visible = True
            End If
        End Sub

        Private Function Validate() As Boolean
            If String.IsNullOrWhiteSpace(UsernameTextBox.Text) Or String.IsNullOrWhiteSpace(PasswordTextBox.Text) Then
                LoginMessageLabel.Text = "User name and password are required."
                LoginMessageLabel.Visible = True
                Return False
            End If
            Return True
        End Function

        Private Sub SetVersion()
            If (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed) Then
                With System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion
                    VersonLabel.Text = "V" & .Major & "." & .Minor & "." & .Build & "." & .Revision
                End With
            Else
                VersonLabel.Text = Application.ProductVersion
            End If
        End Sub

        Private Sub SyncLogin_Load(sender As Object, e As System.EventArgs) Handles Me.Load
            SetVersion()
        End Sub
    End Class
End Namespace