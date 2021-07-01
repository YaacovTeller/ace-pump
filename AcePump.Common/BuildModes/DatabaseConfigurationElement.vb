Imports System.Configuration

Namespace BuildModes
    Public Class DatabaseConfigurationElement
        Inherits ConfigurationElement

        <ConfigurationProperty("connectionStringName", IsRequired:=True)>
        Public Property ConnectionStringName As String
            Get
                Return DirectCast(Me("connectionStringName"), String)
            End Get
            Set(ByVal value As String)
                Me("connectionStringName") = value
            End Set
        End Property

        <ConfigurationProperty("dropAndSeed", IsRequired:=False, DefaultValue:=False)>
        Public Property DropAndSeedDatabase As Boolean
            Get
                Return DirectCast(Me("dropAndSeed"), Boolean)
            End Get
            Set(ByVal value As Boolean)
                Me("dropAndSeed") = value
            End Set
        End Property
    End Class
End Namespace