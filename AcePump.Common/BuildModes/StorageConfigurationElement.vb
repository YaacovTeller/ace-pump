Imports System.Configuration

Namespace BuildModes
    Public Class StorageConfigurationElement
        Inherits ConfigurationElement

        <ConfigurationProperty("storageType", IsRequired:=True)>
        Public Property StorageType As AcePumpStorageType
            Get
                Return DirectCast(Me("storageType"), AcePumpStorageType)
            End Get
            Set(value As AcePumpStorageType)
                Me("storageType") = value
            End Set
        End Property

        <ConfigurationProperty("connectionString")>
        Public Property ConnectionString As String
            Get
                Return DirectCast(Me("connectionString"), String)
            End Get
            Set(value As String)
                Me("connectionString") = value
            End Set
        End Property

        <ConfigurationProperty("dtImageContainerName")>
        Public Property DtImageContainerName As String
            Get
                Return DirectCast(Me("dtImageContainerName"), String)
            End Get
            Set(value As String)
                Me("dtImageContainerName") = value
            End Set
        End Property
    End Class
End Namespace