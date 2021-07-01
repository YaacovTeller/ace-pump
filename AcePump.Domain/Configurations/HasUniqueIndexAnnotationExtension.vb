Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Infrastructure.Annotations
Imports System.Data.Entity.ModelConfiguration.Configuration
Imports System.Runtime.CompilerServices

Friend Module TypeConfigurationExtensions
    <Extension()>
    Function HasUniqueIndexAnnotation(ByVal [property] As PrimitivePropertyConfiguration, ByVal indexName As String, ByVal columnOrder As Integer) As PrimitivePropertyConfiguration
        Dim indexAttribute = New IndexAttribute(indexName, columnOrder) With {
            .IsUnique = True
        }
        Dim indexAnnotation = New IndexAnnotation(indexAttribute)
        Return [property].HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation)
    End Function
End Module
