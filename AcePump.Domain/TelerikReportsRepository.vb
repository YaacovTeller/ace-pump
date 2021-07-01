Imports AcePump.Domain.Models
Imports AcePump.Domain.DataSource

<System.ComponentModel.DataObject()> _
Public Class ReportingRepository
    <System.ComponentModel.DataObjectMethod(ComponentModel.DataObjectMethodType.Select)> _
    Public Function GetPumps() As IEnumerable(Of Pump)
        Dim dataSource As AcePumpContext = DataSourceFactory.GetAcePumpDataSource()

        Return dataSource.Pumps
    End Function

End Class