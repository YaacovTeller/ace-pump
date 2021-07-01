Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.Validation
    Public Class PartInspectionResultValidationAttribute
        Inherits ValidationAttribute

        Protected Overrides Function IsValid(value As Object, context As ValidationContext) As ValidationResult
            Dim result As String = CStr(value)

            If result = "Convert" Or result = "Replace" Then
                If context.ObjectInstance.PartReplacedID Is Nothing Then
                    Return New ValidationResult("You must specify a replacement part for any part that was converted or replaced.")
                End If

                If String.IsNullOrEmpty(context.ObjectInstance.ReasonRepaired) Then
                    Return New ValidationResult("You must specify a reason repaired for any part that was converted or replaced.")
                End If
            End If

            Return ValidationResult.Success
        End Function
    End Class
End Namespace