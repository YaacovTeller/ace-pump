Imports AcePump.Domain.DataSource

Namespace DataSourceSeeders
    Public Interface IDataSourceSeeder
        Sub Seed(dataSource As AcePumpContext)
    End Interface
End Namespace