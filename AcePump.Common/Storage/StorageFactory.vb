Imports System.Configuration
Imports AcePump.Common.BuildModes
Imports AcePump.Common.Storage.Azure
Imports AcePump.Common.Storage.FileSystem

Namespace Storage
    Public Class StorageFactory
        Public Shared Function GetStorageProvider(hostingEnvironment As IVirtualPathMapper) As IStorageProvider
            Select Case AcePumpEnvironment.Environment.Configuration.Storage.StorageType
                Case AcePumpStorageType.Azure
                    Return New AzureStorageProvider(AcePumpEnvironment.Environment.Configuration.Storage.ConnectionString)

                Case AcePumpStorageType.FileSystem
                    Dim rootPath As String = hostingEnvironment.MapPath(AcePumpEnvironment.Environment.Configuration.Storage.ConnectionString)
                    Return New FileSystemStorageProvider(rootPath)

                Case Else
                    Throw New ConfigurationErrorsException("Unrecognized storage type")
            End Select
        End Function
    End Class
End Namespace