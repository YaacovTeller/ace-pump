Imports System.Runtime.CompilerServices
Imports System.Security.Principal

Public Module HttpContextExtensions
    <Extension()> _
    Public Function AcePumpUser(context As HttpContextBase) As AcePumpUserPrincipal
        ConfirmContextImplementsUser(context)

        If Not TypeOf context.User Is AcePumpUserPrincipal Then
            context.User = New AcePumpUserPrincipal(context.User)
        End If

        Return context.User
    End Function

    Private Property KnownNoUserContextTypes As New Dictionary(Of Type, Boolean)
    Private Property SyncLockObject As New Object()
    Private Sub ConfirmContextImplementsUser(ByRef context As HttpContextBase)
        Dim contextType As Type = context.GetType()
        If KnownNoUserContextTypes.ContainsKey(contextType) Then
            If KnownNoUserContextTypes(contextType) Then
                context = New HttpContextWrapper(HttpContext.Current)
            End If

        Else
            SyncLock (SyncLockObject)
                If KnownNoUserContextTypes.ContainsKey(contextType) Then
                    If KnownNoUserContextTypes(contextType) Then
                        context = New HttpContextWrapper(HttpContext.Current)
                    End If

                Else
                    Try
                        Dim a As IPrincipal = context.User
                        KnownNoUserContextTypes.Add(contextType, False)

                    Catch ex As NotImplementedException
                        KnownNoUserContextTypes.Add(contextType, True)
                        context = New HttpContextWrapper(HttpContext.Current)

                    Catch ex As Exception
                        KnownNoUserContextTypes.Add(contextType, True)
                        context = New HttpContextWrapper(HttpContext.Current)
                    End Try
                End If
            End SyncLock
        End If
    End Sub
End Module