Namespace Models
    Public Class WidgetGlobalResponse
        Public Property CustomerID As Integer

        Public Property CustomerName As String
        Public Property LeaseName As String
        Public Property WellNumber As String

        Public Property Errors As List(Of String)

        Public Property notes As List(Of CategoryChartNote)
    End Class

    ''' <summary>
    ''' Represents a note which will be shown on all matching charts.
    ''' </summary>
    Public Class CategoryChartNote
        ''' <summary>
        ''' The value on the chart to show the note at.  This should be a string
        ''' value which matches what's printed on the x-axis.
        ''' </summary>
        Public Property value As Object

        ''' <summary>
        ''' The type of value. Can be a string, date, number, float, etc.
        ''' </summary>
        Public Property valueType As String

        ''' <summary>
        ''' The URL to open when the note is clicked.
        ''' </summary>
        Public Property link As String

        ''' <summary>
        ''' The text to show on the chart.
        ''' </summary>
        Public Property text As String
    End Class
End Namespace