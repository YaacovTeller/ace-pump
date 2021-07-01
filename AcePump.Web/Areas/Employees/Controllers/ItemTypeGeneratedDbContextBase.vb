Imports System.Data.Entity
Imports AcePump.Common
Imports System.Reflection
Imports System.Data.Common

Namespace Areas.Employees.Controllers
    Public Class ItemTypeGeneratedDbContextBase
        Inherits DbContext

        Public Const TypesTablePrefix As String = "Types_"

        Private _ItemTypeName As String
        Private ReadOnly Property ItemTypeName As String
            Get
                If String.IsNullOrEmpty(_ItemTypeName) Then
                    Dim myRuntimeType As Type = Me.GetType()
                    Dim itemsSet As PropertyInfo = myRuntimeType.GetProperty("Items")
                    Dim itemType As Type = itemsSet.PropertyType.GetGenericArguments()(0)

                    _ItemTypeName = itemType.Name
                End If

                Return _ItemTypeName
            End Get
        End Property


        Public Sub New()
            MyBase.New(AcePumpEnvironment.Environment.Configuration.Database.ConnectionStringName)
        End Sub

        Public Overrides Function SaveChanges() As Integer
            VerifyTableExists()

            Return MyBase.SaveChanges()
        End Function

        Private Sub VerifyTableExists()
            Dim tblExistsSql As String = String.Format(
                  "SELECT COUNT(*) " _
                & "FROM INFORMATION_SCHEMA.TABLES " _
                & "WHERE TABLE_NAME = '{0}'",
                TypesTablePrefix & ItemTypeName)

            If ExecuteScalar(tblExistsSql) = 0 Then
                CreateTable()
            End If
        End Sub

        Private Function ExecuteScalar(cmdText As String) As Object
            Dim dbCmd As DbCommand = Database.Connection.CreateCommand()
            dbCmd.CommandText = cmdText

            If Database.Connection.State <> ConnectionState.Open Then
                Database.Connection.Open()
            End If

            Return dbCmd.ExecuteScalar()
        End Function

        Private Sub CreateTable()
            Dim createTableSql As String = String.Format(
                  "CREATE TABLE {0} (" _
                & "    ItemTypeID int IDENTITY(1,1) PRIMARY KEY," _
                & "    DisplayText nvarchar(500)" _
                & ")",
                TypesTablePrefix & ItemTypeName)

            Database.ExecuteSqlCommand(createTableSql)
        End Sub
    End Class
End Namespace