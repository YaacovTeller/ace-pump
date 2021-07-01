Imports System.Collections.Generic
Imports System.Net.Http

Namespace BugzScout
    ''' <summary>
    ''' Client to post bugs to a FogBugz account.  Can be used with either FogBugz for your server or FogBugz on Demand (Fog Creek Hosted).
    ''' 
    ''' Documentation provided from the official Fog Creek help center - http://help.fogcreek.com/7566/bugzscout-for-automatic-crash-reporting.
    ''' </summary>
    Public Class BugzScoutClient
        Private Property FogBugzUrl As String

        ''' <summary>
        ''' The default full name of the FogBugz user the case creation or edit should be made as.  Used if you do not specify a username when you call Submit.
        ''' </summary>
        Public Property DefaultUsername As String

        ''' <summary>
        ''' The default project that new cases should be created in (must be a valid project name).  Used if you do not specify a project when you call Submit.
        ''' </summary>
        Public Property DefaultProject As String

        ''' <summary>
        ''' The default area that new cases should go into(must be a valid area in the ScoutProject).  Used if you do not specify an area when you call Submit.
        ''' </summary>
        Public Property DefaultArea As String

        ''' <summary>
        ''' The default email address to associate with the report, often the customer’s email.  Used if you do not specify an email when you call Submit.
        ''' </summary>
        Public Property DefaultEmail As String

        ''' <summary>
        ''' The default text that will be returned by the HTTP post request if no specific message exists for this bug.  Used if you do not specify a default message when you call Submit.
        ''' </summary>
        Public Property DefaultDefaultMessage As String

        ''' <summary>
        ''' Override the normal automatic consolidation of BugzScout reports by default.  Used if you do not specify force new bug when you call Submit.
        ''' </summary>
        Public Property DefaultForceNewBug As Boolean?

        Public Sub New(fogbugzUrl As String)
            Me.FogBugzUrl = fogbugzUrl
        End Sub

        ''' <summary>
        ''' Submit a bug report to your FogBugz site via the BugzScout API.
        ''' </summary>
        ''' <param name="description">
        '''    This is the unique string that identifies the particular crash that has just occurred. BugzScout submissions will be consolidated into one case based on the Description. When a BugzScout submission arrives in FogBugz, if there are no existing cases with the exact same Description, then a new case is created in the ScoutProject and ScoutArea.If a case is found with a matching description, the new submission is APPENDED to that case’s history, and a new case will NOT be created in FogBugz.When appending to a case, the ScoutProject and ScoutArea fields are ignored.
        '''    Because BugzScout submissions are automatically consolidated, it is important to include appropriate information when constructing the Description.We always include the short error message that was generated, and then some additional information.For example, it is usually wise to include the application version number so that a similar crash in version 1.1 and in version 1.2 will NOT append to the same case. You may consider including the OS that the crash occurred on, if relevant.
        '''    Here is a good starting point:
        '''    MyAppName X.Y.Z SomekindofException: Exception message text here which yields something like this actual error from part of the FogBugz javascript code: FogBugz-8.9.72.0H: JS Error: TypeError: 'undefined' is not an object (evaluating 'bug.ixBug')
        ''' </param>
        ''' <param name="additionalInformation">The details about this particular crash. This is often a good place to put a stack trace, operating environment information, HTTP headers, or other details about what might have caused the error. Include as much information as you can, but beware of sending sensitive information. For example, in billing code, make sure your exception handling code strips out credit card information.</param>
        ''' <param name="username">The full name of the FogBugz user the case creation or edit should be made as.  Fog Creek recommend's using a Virtual User for this.</param>
        ''' <param name="project">The Project that new cases should be created in (must be a valid project name).  Note that if BugzScout appends to an existing case, this field is ignored.</param>
        ''' <param name="area">The Area that new cases should go into(must be a valid area in the ScoutProject).  Note that if BugzScout appends to an existing case, this field is ignored.</param>
        ''' <param name="email">An email address to associate with the report, often the customer’s email.  This overwrites the correspondent field on the case with each appended occurrence, so it is automatically included at the end of the case event as well.</param>
        ''' <param name="defaultMessage">This is the default text that will be returned by the HTTP post request. If the submission is appended to an existing case, and that case has some text in the “Scout Message” field, then that text will be returned instead.This is useful when you want to let your first user experiencing a crash know that “we are investigating the issue,” but update that message in the case later on when you know that “this problem is fixed in the next version.”</param>
        ''' <param name="forceNewBug">Override the normal automatic consolidation of BugzScout report - a new case will always created from this submission.</param>
        ''' <returns></returns>
        Public Function Submit(description As String, additionalInformation As String, Optional username As String = Nothing, Optional project As String = Nothing, Optional area As String = Nothing, Optional email As String = Nothing, Optional defaultMessage As String = Nothing, Optional forceNewBug As Boolean? = Nothing) As BugzScoutResponse
            If (username = Nothing) Then username = DefaultUsername
            If (project = Nothing) Then project = DefaultProject
            If (area = Nothing) Then area = DefaultArea
            If (email = Nothing) Then email = DefaultEmail
            If (defaultMessage = Nothing) Then defaultMessage = DefaultDefaultMessage
            If (Not forceNewBug.HasValue) Then forceNewBug = If(DefaultForceNewBug, False)

            Using httpClient As New HttpClient()
                Dim postTask = httpClient.PostAsync(
                    requestUri:=FogBugzUrl & "/scoutSubmit.asp",
                    content:=New FormUrlEncodedContent(New Dictionary(Of String, String) From
                    {
                        {"ScoutUserName", username},
                        {"ScoutProject", project},
                        {"ScoutArea", area},
                        {"Description", description},
                        {"Extra", additionalInformation},
                        {"ForceNewBug", If(forceNewBug.Value, "1", "0")},
                        {"ScoutDefaultMessage", defaultMessage},
                        {"FriendlyResponse", "0"}
                    })
                )
                postTask.Wait()

                Dim getResponseTask = postTask.Result.Content.ReadAsStringAsync()
                getResponseTask.Wait()

                Return New BugzScoutResponse With {
                    .Success = getResponseTask.Result.Contains("<Success>")
                }
            End Using
        End Function
    End Class
End Namespace