Imports AcePump.Domain.Models
Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PumpPlungerViewModel
        <Required()> Public Property Material As String
        <Required()> Public Property Length As String
        <Required()> Public Property Fit As String

        Public Shared Widening Operator CType(pumpPlunger As PumpPlunger) As PumpPlungerViewModel
            If pumpPlunger Is Nothing Then
                Return Nothing

            Else
                Return New PumpPlungerViewModel() With {
                    .Material = pumpPlunger.Material,
                    .Fit = pumpPlunger.Fit,
                    .Length = pumpPlunger.Length
                }
            End If
        End Operator

        Public Shared Widening Operator CType(pumpPlunger As PumpPlungerViewModel) As PumpPlunger
            If pumpPlunger Is Nothing Then
                Return Nothing

            Else
                Return New PumpPlunger() With {
                    .Material = pumpPlunger.Material,
                    .Fit = pumpPlunger.Fit,
                    .Length = pumpPlunger.Length
                }
            End If
        End Operator
    End Class
End Namespace