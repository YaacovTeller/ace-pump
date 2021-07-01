Imports System.Data.Entity
Imports AcePump.Common
Imports AcePump.Domain.DataSource
Imports AcePump.Domain.DataSourceSeeders

Public Class DataSourceFactory
    Private Shared Property ContextInitializerSet As Boolean = False
    Private Shared Property AppleTestContextInitializerSet As Boolean = False

    Public Shared Function GetAppleDataSource() As AcePumpContext
        If Not AppleTestContextInitializerSet Then
            Database.SetInitializer(Of AcePumpAppleTestingDbContext)(New AcePumpInitializer())

            AppleTestContextInitializerSet = True
        End If

        Dim ctx As AcePumpContext = New AcePumpAppleTestingDbContext("azure-acepump-apple-test-db")

        Return ctx
    End Function

    Public Shared Function GetAcePumpDataSource() As AcePumpContext
        If Not ContextInitializerSet Then
            Database.SetInitializer(Of AcePumpContext)(New AcePumpInitializer())

            ContextInitializerSet = True
        End If

        Dim ctx As AcePumpContext = New AcePumpContext(AcePumpEnvironment.Environment.Configuration.Database.ConnectionStringName)

        Return ctx
    End Function

    Private Class AcePumpInitializer
        Implements IDatabaseInitializer(Of AcePumpContext)

        Public Sub InitializeDatabase(context As AcePumpContext) Implements IDatabaseInitializer(Of AcePumpContext).InitializeDatabase
            If AcePumpEnvironment.Environment.Configuration.Database.DropAndSeedDatabase Then
                DropAndSeedOnModelChange(context)
            End If
        End Sub

        Private Sub DropAndSeedOnModelChange(context As AcePumpContext)
            If context.Database.Exists() Then
                If Not context.Database.CompatibleWithModel(False) Then
                    DropDatabase(context)
                Else
                    Return
                End If
            End If

            CreateDatabase(context)
            SeedDatabase(context)
        End Sub

        Private Sub DropDatabase(context As AcePumpContext)
            context.Database.Delete()
        End Sub

        Private Sub CreateDatabase(context As AcePumpContext)
            context.Database.Create()
        End Sub

        Private Sub SeedDatabase(context As AcePumpContext)
            Dim seeder As New DropAndSeedSeeder()

            seeder.Seed(context)
        End Sub
    End Class
End Class
