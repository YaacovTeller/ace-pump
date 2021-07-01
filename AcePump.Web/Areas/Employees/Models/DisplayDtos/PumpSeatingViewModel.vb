Imports AcePump.Domain.Models
Imports System.ComponentModel.DataAnnotations

Namespace Areas.Employees.Models.DisplayDtos
    Public Class PumpSeatingViewModel
        <Required()> Public Property Location As String
        <Required()> Public Property Type As String

        Public Shared Widening Operator CType(pumpSeating As PumpSeating) As PumpSeatingViewModel
            If pumpSeating Is Nothing Then
                Return Nothing

            Else
                Return New PumpSeatingViewModel() With {
                    .Type = pumpSeating.Type,
                    .Location = pumpSeating.Location
                }
            End If
        End Operator

        Public Shared Widening Operator CType(pumpSeating As PumpSeatingViewModel) As PumpSeating
            If pumpSeating Is Nothing Then
                Return Nothing

            Else
                Return New PumpSeating() With {
                    .Type = pumpSeating.Type,
                    .Location = pumpSeating.Location
                }
            End If
        End Operator
    End Class
End Namespace