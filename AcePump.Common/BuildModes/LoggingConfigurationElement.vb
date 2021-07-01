Imports System.Configuration

Namespace BuildModes
    Public Class LoggingConfigurationElement
        Inherits ConfigurationElement

        <ConfigurationProperty("logErrorsToFogBugz", IsRequired:=False, DefaultValue:=True)>
        Public Property LogErrorsToFogBugz As Boolean
            Get
                Return DirectCast(Me("logErrorsToFogBugz"), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me("logErrorsToFogBugz") = value
            End Set
        End Property

        <ConfigurationProperty("logSearches", IsRequired:=False, DefaultValue:=False)>
        Public Property LogSearches As Boolean
            Get
                Return DirectCast(Me("logSearches"), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me("logSearches") = value
            End Set
        End Property

        <ConfigurationProperty("logAllRequests", IsRequired:=False, DefaultValue:=False)>
        Public Property LogAllRequests As Boolean
            Get
                Return DirectCast(Me("logAllRequests"), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me("logAllRequests") = value
            End Set
        End Property

        <ConfigurationProperty("logEntryPrefix", IsRequired:=False, DefaultValue:="Ace Pump - General")>
        Public Property LogEntryPrefix As String
            Get
                Return DirectCast(Me("logEntryPrefix"), String)
            End Get
            Set(ByVal value As String)
                Me("logEntryPrefix") = value
            End Set
        End Property
    End Class
End Namespace