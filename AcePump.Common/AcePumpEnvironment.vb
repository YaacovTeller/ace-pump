Imports System.Configuration
Imports AcePump.Common.BuildModes

Public Class AcePumpEnvironment
    Private Shared SingletonLoader As New Lazy(Of AcePumpEnvironment)(Function() New AcePumpEnvironment())
    Public Shared ReadOnly Property Environment As AcePumpEnvironment
        Get
            Return SingletonLoader.Value
        End Get
    End Property

    Public Property Configuration As BuildModeConfigurationElement

    Private Sub New()
        Init()
    End Sub

    Private Sub Init()
        Dim buildModeName As String = BuildModeNames.GetCurrentBuildModeName()
        Configuration = GetElementByName(buildModeName)
        If Configuration Is Nothing Then
            Throw New ConfigurationErrorsException($"Could not find configuration for {buildModeName}.  Did you define the buildModes element in Web.config?")
        End If
    End Sub

    Private Function GetElementByName(ByVal buildModeName As String) As BuildModeConfigurationElement
        Dim section As BuildModesConfigurationSection = CType(ConfigurationManager.GetSection("buildModes"), BuildModesConfigurationSection)
        For Each element As BuildModeConfigurationElement In section.Modes
            If element.Name = buildModeName Then
                Return element
            End If
        Next

        Return Nothing
    End Function
End Class
