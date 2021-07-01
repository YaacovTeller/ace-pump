Imports AcePump.Domain.Models
Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PumpBarrelViewModel
        <Required()> Public Property Length As String
        <Required()> Public Property Type As String
        <Required()> Public Property Material As String
        <Required()> Public Property Washer As String

        Public Shared Widening Operator CType(pumpBarrel As PumpBarrel) As PumpBarrelViewModel
            If pumpBarrel Is Nothing Then
                Return Nothing

            Else
                Return New PumpBarrelViewModel() With {
                    .Type = pumpBarrel.Type,
                    .Material = pumpBarrel.Material,
                    .Length = pumpBarrel.Length,
                    .Washer = pumpBarrel.Washer
                }
            End If
        End Operator

        Public Shared Widening Operator CType(pumpBarrel As PumpBarrelViewModel) As PumpBarrel
            If pumpBarrel Is Nothing Then
                Return Nothing

            Else
                Return New PumpBarrel() With {
                    .Type = pumpBarrel.Type,
                    .Material = pumpBarrel.Material,
                    .Length = pumpBarrel.Length,
                    .Washer = pumpBarrel.Washer
                }
            End If
        End Operator
    End Class
End Namespace