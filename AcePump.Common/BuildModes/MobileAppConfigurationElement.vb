Imports System.Configuration

Namespace BuildModes
    Public Class MobileAppConfigurationElement
        Inherits ConfigurationElement

        ''' <summary>
        ''' Additions to the default-src for the Content-Security-Policy in this build mode.
        ''' </summary>
        <ConfigurationProperty("csp-default-src", IsRequired:=False, DefaultValue:="")>
        Public Property CspDefaultSrc As String
            Get
                Return DirectCast(Me("csp-default-src"), String)
            End Get
            Set(ByVal value As String)
                Me("csp-default-src") = value
            End Set
        End Property
    End Class
End Namespace