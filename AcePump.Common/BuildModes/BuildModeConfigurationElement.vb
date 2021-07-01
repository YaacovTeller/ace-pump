Imports System.Configuration

Namespace BuildModes
    Public Class BuildModeConfigurationElement
        Inherits ConfigurationElement

        ''' <summary>
        ''' Name of the build mode *as defined in the configuration manager*.  Make sure to add the BUILD_MODE_NAME
        ''' output variable to the AcePump.Common project with the same name.
        ''' </summary>
        <ConfigurationProperty("name", IsRequired:=True, IsKey:=True)>
        Public Property Name As String
            Get
                Return DirectCast(Me("name"), String)
            End Get
            Set(ByVal value As String)
                Me("name") = value
            End Set
        End Property

        ''' <summary>
        ''' Large title text that shows up next to the Ace Pump logo on all pages.
        ''' </summary>
        <ConfigurationProperty("applicationSubtitle", IsRequired:=False, DefaultValue:="")>
        Public Property ApplicationSubtitle As String
            Get
                Return DirectCast(Me("applicationSubtitle"), String)
            End Get
            Set(ByVal value As String)
                Me("applicationSubtitle") = value
            End Set
        End Property

        <ConfigurationProperty("logging", IsRequired:=True)>
        Public Property Logging As LoggingConfigurationElement
            Get
                Return DirectCast(Me("logging"), LoggingConfigurationElement)
            End Get
            Set(ByVal value As LoggingConfigurationElement)
                Me("logging") = value
            End Set
        End Property

        <ConfigurationProperty("storage", IsRequired:=True)>
        Public Property Storage As StorageConfigurationElement
            Get
                Return DirectCast(Me("storage"), StorageConfigurationElement)
            End Get
            Set(ByVal value As StorageConfigurationElement)
                Me("storage") = value
            End Set
        End Property

        <ConfigurationProperty("database", IsRequired:=True)>
        Public Property Database As DatabaseConfigurationElement
            Get
                Return DirectCast(Me("database"), DatabaseConfigurationElement)
            End Get
            Set(ByVal value As DatabaseConfigurationElement)
                Me("database") = value
            End Set
        End Property

        <ConfigurationProperty("mobileApp", IsRequired:=True)>
        Public Property MobileApp As MobileAppConfigurationElement
            Get
                Return DirectCast(Me("mobileApp"), MobileAppConfigurationElement)
            End Get
            Set(ByVal value As MobileAppConfigurationElement)
                Me("mobileApp") = value
            End Set
        End Property

        <ConfigurationProperty("ptpApi", IsRequired:=True)>
        Public Property PtpApi As PtpApiConfigurationElement
            Get
                Return DirectCast(Me("ptpApi"), PtpApiConfigurationElement)
            End Get
            Set(ByVal value As PtpApiConfigurationElement)
                Me("ptpApi") = value
            End Set
        End Property

        ''' <summary>
        ''' For checking ticket issue date against to force logout.
        ''' </summary>
        <ConfigurationProperty("authTicketsValidAfterDate", IsRequired:=False, DefaultValue:="")>
        Public Property AuthTicketsValidAfterDate As DateTime
            Get
                Return DirectCast(Me("authTicketsValidAfterDate"), DateTime)
            End Get
            Set(ByVal value As DateTime)
                Me("authTicketsValidAfterDate") = value
            End Set
        End Property
    End Class
End Namespace
