Imports AcePump.Domain.Models
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PartTemplateModel
        Inherits PartTemplateModelBase

        Public Property PartTemplateID As Integer

        Public Property AssemblyID As Integer?
        Public Property AssemblyPartNumber As String

        Public Property Manufacturer As String
        Public Property ManufacturerPartNumber As String

        <ForeignKey("MaterialID")>
        Public Property Material As String
    End Class
End Namespace