Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.Data

Namespace ReportExtensions
    Public Module ReportExtensions
        <Extension()> _
        Public Function EntityToDataTable(Of TEntity)(entity As TEntity) As DataTable
            Dim list As New List(Of TEntity)()

            If entity IsNot Nothing Then
                list.Add(entity)
            End If

            Return ToDataTable(list)
        End Function

        <Extension()> _
        Public Function ToDataTable(Of TEntity)(source As IEnumerable(Of TEntity)) As DataTable
            Dim entityType As Type = GetType(TEntity)
            Dim entityProperties As DataTablePropertyAcessor() = GetPropertyAccessors(entityType.GetProperties(BindingFlags.Public Or BindingFlags.Instance))
            Dim table As New DataTable()

            'define table structure based on type
            For Each entityProperty As DataTablePropertyAcessor In entityProperties
                table.Columns.Add(entityProperty.Name, entityProperty.DataTableType)
            Next

            'add rows
            For Each item As TEntity In source
                Dim valueArray(entityProperties.Length - 1) As Object

                For i As Integer = 0 To entityProperties.Length - 1
                    valueArray(i) = entityProperties(i).GetDataTableValue(item)
                Next

                table.Rows.Add(valueArray)
            Next

            Return table
        End Function

        Private Function GetPropertyAccessors(propertyInfos As PropertyInfo()) As DataTablePropertyAcessor()
            Dim accessors As New List(Of DataTablePropertyAcessor)

            For Each info As PropertyInfo In propertyInfos
                accessors.Add(New DataTablePropertyAcessor(info))
            Next

            Return accessors.ToArray()
        End Function

        Private Class DataTablePropertyAcessor
            Private ReadOnly _PropertyInfo As PropertyInfo

            Public ReadOnly Property Name As String
                Get
                    Return _PropertyInfo.Name
                End Get
            End Property

            Private ConvertNullable As Boolean

            Private ReadOnly _DataTableType As Type
            Public ReadOnly Property DataTableType As Type
                Get
                    Return _DataTableType
                End Get
            End Property

            Public Sub New(propertyInfo As PropertyInfo)
                _PropertyInfo = propertyInfo

                If _PropertyInfo.PropertyType.IsGenericType AndAlso _PropertyInfo.PropertyType.GetGenericTypeDefinition() = GetType(Nullable(Of )) Then
                    _DataTableType = Nullable.GetUnderlyingType(_PropertyInfo.PropertyType)
                    ConvertNullable = True
                Else
                    _DataTableType = _PropertyInfo.PropertyType
                    ConvertNullable = False
                End If
            End Sub

            Public Function GetDataTableValue(from As Object) As Object
                If Not ConvertNullable Then
                    Return _PropertyInfo.GetValue(from, Nothing)

                Else
                    Dim nullableValue As Object = _PropertyInfo.GetValue(from, Nothing)

                    Return nullableValue
                    'nullables auto box to non-nullables w/ object: http://stackoverflow.com/a/5194550/794234
                End If
            End Function
        End Class
    End Module
End Namespace
