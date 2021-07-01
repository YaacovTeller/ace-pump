Imports System.Configuration

Namespace BuildModes
    Public Class PtpApiConfigurationElement
        Inherits ConfigurationElement

        ''' <summary>
        ''' The URI the AcePump.Web project WebApi section is hosted at in this build mode.  This is a special instance
        ''' of WebApi hosted inside an older MVC project.  It should not be used for new development.
        ''' </summary>
        <ConfigurationProperty("uriV1", IsRequired:=True)>
        Public Property UriV1 As String
            Get
                Return DirectCast(Me("uriV1"), String)
            End Get
            Set(ByVal value As String)
                Me("uriV1") = value
            End Set
        End Property

        ''' <summary>
        ''' The URI the AcePump.WebApi project is hosted at in this build mode.  This is newer version of the API and
        ''' should be used for all new development.
        ''' </summary>
        <ConfigurationProperty("uriV2", IsRequired:=True)>
        Public Property UriV2 As String
            Get
                Return DirectCast(Me("uriV2"), String)
            End Get
            Set(ByVal value As String)
                Me("uriV2") = value
            End Set
        End Property
    End Class
End Namespace