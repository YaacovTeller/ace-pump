Imports System.Data.Entity
Imports AcePump.Domain.Models
Imports System.Data.Common
Imports Yesod.LinqProviders
Imports AcePump.Domain.Configurations

Namespace DataSource
    Public Class AcePumpContext
        Inherits DbContext
        Implements IAccountDataSource

        Public Sub New()
            MyBase.New()
        End Sub

        Public Property Customers As IDbSet(Of Customer)
        Public Property DeliveryTickets As IDbSet(Of DeliveryTicket)
        Public Property DeliveryTicketImageUploads As IDbSet(Of DeliveryTicketImageUpload)

        Public Property WellLocations As IDbSet(Of Well)
        Public Property Assemblies As IDbSet(Of Assembly)
        Public Property LeaseLocations As IDbSet(Of Lease)
        Public Property LineItems As IDbSet(Of LineItem)
        Public Property Materials As IDbSet(Of Material)

        Public Property PartTemplates As IDbSet(Of PartTemplate)
        Public Property Parts As IDbSet(Of Part)
        Public Property PartOptions As IDbSet(Of SoldByOption)
        Public Property PartCategories As IDbSet(Of PartCategory)

        Public Property Pumps As IDbSet(Of Pump)
        Public Property PumpTemplates As IDbSet(Of PumpTemplate)
        Public Property PartInspections As IDbSet(Of PartInspection)
        Public Property TemplatePartDefs As IDbSet(Of TemplatePartDef)
        Public Property AssemblyPartDefs As IDbSet(Of AssemblyPartDef)

        Public Property CountySalesTaxRates As IDbSet(Of CountySalesTaxRate)

        Public Property AcePumpProfiles As IDbSet(Of AcePumpProfile)

        Public Property ApiTokens As IDbSet(Of ApiToken)
        Public Property QbInvoiceClasses As IDbSet(Of QbInvoiceClass)

        Public Property Users As DbSet(Of AccountDataStoreUser) Implements IAccountDataSource.Users
        Public Property Roles As DbSet(Of AccountDataStoreRole) Implements IAccountDataSource.Roles

        Public Property PumpRuntimes As IDbSet(Of PumpRuntime)
        Public Property PartRuntimes As IDbSet(Of PartRuntime)
        Public Property PartRuntimeSegments As IDbSet(Of PartRuntimeSegment)

        Public Property Log_HttpRequests As IDbSet(Of Log_HttpRequest)
        Public Property Log_HttpRequestParams As IDbSet(Of Log_HttpRequestParam)

        Public Property CustomerPartSpecials As IDbSet(Of CustomerPartSpecial)
        Public Property ShopLocations As IDbSet(Of ShopLocation)

        Public ReadOnly Property AssembliesWithRelatedParts As IQueryable(Of AssemblyWithRelatedPart)
            Get
                Return Assemblies _
                     .Include(Function(x) x.Parts) _
                     .Join(PartTemplates,
                            Function(x) x.AssemblyNumber,
                            Function(x) x.Number,
                            Function(assembly, partTemplate) New AssemblyWithRelatedPart With {
                                .Assembly = assembly,
                                .PartTemplate = partTemplate
                          })
            End Get
        End Property


        Public Sub New(nameOrConnectionString As String)
            MyBase.New(nameOrConnectionString)
        End Sub

        Public Sub New(connection As DbConnection)
            MyBase.New(connection, True)
        End Sub

        Private Sub IAccountDataSource_SaveChanges() Implements IAccountDataSource.SaveChanges
            SaveChanges()
        End Sub

        Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
            AddConfigurations(modelBuilder)

            modelBuilder.Conventions.Add(New CodeFirstStoreFunctions.FunctionsConvention("dbo", GetType(Udf)))

            MyBase.OnModelCreating(modelBuilder)
        End Sub

        Private Sub AddConfigurations(modelBuilder As DbModelBuilder)
            modelBuilder.Configurations.Add(New CustomerConfiguration())
            modelBuilder.Configurations.Add(New PumpTemplateConfiguration())
            modelBuilder.Configurations.Add(New TemplatePartDefConfiguration())
            modelBuilder.Configurations.Add(New PumpConfiguration())
            modelBuilder.Configurations.Add(New AcePumpProfileConfiguration())
            modelBuilder.Configurations.Add(New PartTemplateConfiguration())
            modelBuilder.Configurations.Add(New RoleConfiguration())
            modelBuilder.Configurations.Add(New AccountDataStoreUserConfiguration())
            modelBuilder.Configurations.Add(New DeliveryTicketConfiguration())
            modelBuilder.Configurations.Add(New PartInspectionConfiguration())
            modelBuilder.Configurations.Add(New PartCategoryConfiguration())
            modelBuilder.Configurations.Add(New SoldByOptionConfiguration())
            modelBuilder.Configurations.Add(New AssemblyConfiguration())
            modelBuilder.Configurations.Add(New LineItemConfiguration())
            modelBuilder.Configurations.Add(New PumpRuntimeConfiguration())
            modelBuilder.Configurations.Add(New PartRuntimeConfiguration())
            modelBuilder.Configurations.Add(New PartRuntimeSegmentConfiguration())
            modelBuilder.Configurations.Add(New CustomerPartSpecialConfiguration())
            modelBuilder.Configurations.Add(New CountySalesTaxRateConfiguration())
            modelBuilder.Configurations.Add(New WellConfiguration())
        End Sub
    End Class
End Namespace
