Imports AcePump.Domain.DataSource
Imports AcePump.Common
Imports AcePump.Domain.Models
Imports Yesod.LinqProviders

Namespace DataSourceSeeders
    Public Class DropAndSeedSeeder
        Implements IDataSourceSeeder

        Private Property DataSource As AcePumpContext

        Private Property Customer1 As Customer
        Private Property Template1 As PumpTemplate
        Private Property Pump1 As Pump
        Private Property SoldBy As SoldByOption

        Public Sub Seed(dataSource As AcePumpContext) Implements IDataSourceSeeder.Seed
            Configure(dataSource)

            SeedGeneral()
            SeedTemplates()
            SeedCustomers()
            SeedPumps()
            SeedTickets()
            SeedMembership()
        End Sub

        Private Sub Configure(dataSource As AcePumpContext)
            Me.DataSource = dataSource
        End Sub

        Private Sub SeedMembership()
            Dim salt As String = CryptoService.GenerateSalt()
            Dim pass As String = CryptoService.HashPassword("chamber123", salt)

            Dim customerUser As New AccountDataStoreUser() With {.Username = "customerUser", .PasswordSalt = salt, .HashedPassword = pass, .CreateDate = Date.Now, .IsApproved = True, .LastPasswordFailureDate = Date.Now, .LastPasswordChangedDate = Date.Now, .LastLoginDate = Date.Now, .LastActivityDate = Date.Now, .LastLockoutDate = Date.Now}
            Dim acePumpUser As New AccountDataStoreUser() With {.Username = "acePumpUser", .PasswordSalt = salt, .HashedPassword = pass, .CreateDate = Date.Now, .IsApproved = True, .LastPasswordFailureDate = Date.Now, .LastPasswordChangedDate = Date.Now, .LastLoginDate = Date.Now, .LastActivityDate = Date.Now, .LastLockoutDate = Date.Now}
            Dim adminUser As New AccountDataStoreUser() With {.Username = "admin", .PasswordSalt = salt, .HashedPassword = pass, .CreateDate = Date.Now, .IsApproved = True, .LastPasswordFailureDate = Date.Now, .LastPasswordChangedDate = Date.Now, .LastLoginDate = Date.Now, .LastActivityDate = Date.Now, .LastLockoutDate = Date.Now}

            DataSource.Users.Add(customerUser)
            DataSource.Users.Add(acePumpUser)
            DataSource.Users.Add(adminUser)

            DataSource.AcePumpProfiles.Add(New AcePumpProfile() With {.User = customerUser, .Customer = Customer1})

            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.AccountManager, .Users = New List(Of AccountDataStoreUser) From {adminUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.Impersonator, .Users = New List(Of AccountDataStoreUser) From {adminUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.AcePumpEmployee, .Users = New List(Of AccountDataStoreUser) From {acePumpUser, adminUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.Customer, .Users = New List(Of AccountDataStoreUser) From {customerUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.SeeCost, .Users = New List(Of AccountDataStoreUser) From {adminUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.TypeManager, .Users = New List(Of AccountDataStoreUser) From {adminUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.DeliveryTicketSigner, .Users = New List(Of AccountDataStoreUser) From {customerUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.ApiQuickbooksUser, .Users = New List(Of AccountDataStoreUser) From {adminUser}})
            DataSource.Roles.Add(New AccountDataStoreRole() With {.RoleName = AcePumpSecurityRoles.AcePumpAdmin, .Users = New List(Of AccountDataStoreUser) From {adminUser}})

            DataSource.SaveChanges()
        End Sub

        Private Sub SeedGeneral()
            SoldBy = DataSource.PartOptions.Add(New SoldByOption() With {.Description = "Each"})
            DataSource.PartOptions.Add(New SoldByOption() With {.Description = "Foot"})
            DataSource.PartOptions.Add(New SoldByOption() With {.Description = "Pound"})
            DataSource.SaveChanges()
        End Sub

        Private Sub SeedTemplates()
            Template1 = DataSource.PumpTemplates.Add(New PumpTemplate() With {
                .Barrel = New PumpBarrel() With {
                    .Length = "barrel length",
                    .Material = "Stainless Steel Chrome Plated",
                    .Type = "Thin Wall for Soft Packed Plunger Pumps",
                    .Washer = "Barrel Washer"
                },
                .Plunger = New PumpPlunger() With {
                    .Material = "Spay Metal W/Mon Pin",
                    .Length = "5""",
                    .Fit = "1"""
                },
                .Seating = New PumpSeating() With {
                    .Location = "Top",
                    .Type = "seating type"
                },
                .ConciseSpecificationSummary = "unset template number",
                .TubingSize = "2""",
                .PumpBoreBasic = "3""",
                .LowerExtension = "None",
                .UpperExtension = "None",
                .PumpType = "Rod",
                .HoldDownType = "2.5"" API Mech B/L",
                .TravellingCages = "Steel Insert Guided Cage",
                .StandingValveCages = "traveling valve",
                .StandingValve = "RETRIEVABLE STANDING VALVE",
                .BallsAndSeats = "Titanium/Tungsten Carbide",
                .Collet = "VRB/TPC",
                .TopSeals = "Evitop Seal",
                .OnOffTool = "Included",
                .SpecialtyItems = "Charger Valve",
                .PonyRods = "1.0""D",
                .Strainers = "1.5"" x 24""",
                .KnockOut = "Included",
                .Markup = 0.01,
                .Discount = 0.02
            })

            Template1.Parts = New List(Of TemplatePartDef) From {
                New TemplatePartDef() With {.Quantity = 3, .PartTemplate = New PartTemplate() With {.SoldByOption = SoldBy, .Cost = 45, .Discount = 0.045, .Markup = 0.145, .PriceLastUpdated = Today.AddDays(-1), .Number = "45", .Description = "Description for Part Template 45", .Active = True, .PartCategory = New PartCategory() With {.CategoryName = "cat 1"}}, .SortOrder = 1},
                New TemplatePartDef() With {.Quantity = 1, .PartTemplate = New PartTemplate() With {.SoldByOption = SoldBy, .Cost = 15, .Discount = 0.015, .Markup = 0.115, .PriceLastUpdated = Today.AddDays(-1), .Number = "15", .Description = "Description for Part Template 15", .Active = True, .PartCategory = New PartCategory() With {.CategoryName = "cat 2"}}, .SortOrder = 2},
                New TemplatePartDef() With {.Quantity = 1, .PartTemplate = New PartTemplate() With {.SoldByOption = SoldBy, .Cost = 25, .Discount = 0.025, .Markup = 0.125, .PriceLastUpdated = Today, .Number = "25", .Description = "Description for Part Template 25", .Active = True, .PartCategory = New PartCategory() With {.CategoryName = "cat 3"}}, .SortOrder = 3},
                New TemplatePartDef() With {.Quantity = 1, .PartTemplate = New PartTemplate() With {.SoldByOption = SoldBy, .Cost = 10, .Discount = 0.01, .Markup = 0.11, .PriceLastUpdated = Today.AddDays(-15), .Number = "10", .Description = "Description for Part Template 10", .Active = True, .PartCategory = New PartCategory() With {.CategoryName = "cat 4"}}, .SortOrder = 4}
            }

            DataSource.SaveChanges()
        End Sub

        Private Sub SeedCustomers()
            Dim bkClass As QbInvoiceClass = DataSource.QbInvoiceClasses.Add(New QbInvoiceClass With {.FullName = "BK-PUMP SHOP"})
            Dim smClass As QbInvoiceClass = DataSource.QbInvoiceClasses.Add(New QbInvoiceClass With {.FullName = "SM-PUMP SHOP"})

            Customer1 = DataSource.Customers.Add(New Customer() With {
                .CustomerName = "Seneca",
                .QbInvoiceClass = bkClass,
                .UsesInventory = False,
                .CountySalesTaxRate = New CountySalesTaxRate() With {.CountyName = "FRESNO COUNTY", .SalesTaxRate = 0.00378},
                .Wells = New List(Of Well) From {New Well() With {.WellNumber = "testWell", .Lease = New Lease() With {.LocationName = "testLocation"}}}
            })
            DataSource.Customers.Add(New Customer() With {
                .CustomerName = "A.C. TERRA",
                .QbInvoiceClass = smClass,
                .UsesInventory = False,
                .Wells = New List(Of Well) From {New Well() With {.WellNumber = "testWellAcTerra", .Lease = New Lease() With {.LocationName = "testLocationACTerra"}}}
            })
            DataSource.Customers.Add(New Customer() With {
                .CustomerName = "FMOG",
                .QbInvoiceClass = smClass,
                .UsesInventory = False,
                .Wells = New List(Of Well) From {New Well() With {.WellNumber = "testWellFMOG", .Lease = New Lease() With {.LocationName = "testLocationFMOG"}}}
            })
            DataSource.Customers.Add(New Customer() With {
                .CustomerName = "PACIFIC COAST ENERGY COMPANY",
                .QbInvoiceClass = smClass,
                .UsesInventory = False,
                .Wells = New List(Of Well) From {New Well() With {.WellNumber = "testWellPacific", .Lease = New Lease() With {.LocationName = "testLocationPacific"}}}
            })
            DataSource.SaveChanges()
        End Sub

        Private Sub SeedPumps()
            Dim sm As ShopLocation = DataSource.ShopLocations.Add(New ShopLocation With {.Name = "Santa Maria", .Prefix = "SM"})
            DataSource.ShopLocations.Add(New ShopLocation With {.Name = "Bakersfield", .Prefix = "B"})
            DataSource.ShopLocations.Add(New ShopLocation With {.Name = "Ventura", .Prefix = "V"})

            Pump1 = DataSource.Pumps.Add(New Pump() With {
                .PumpNumber = "pump",
                .PumpTemplate = Template1,
                .InstalledInWell = Customer1.Wells(0),
                .ShopLocation = sm
            })

            DataSource.SaveChanges()
        End Sub

        Private Sub SeedTickets()
            Dim t As DeliveryTicket = DataSource.DeliveryTickets.Add(New DeliveryTicket() With {
                .CloseTicket = False,
                .PumpFailed = Pump1,
                .Customer = Customer1,
                .SalesTaxRate = 0.4D,
                .CompletedBy = "Completed By",
                .CustomerID = 1,
                .HoldDown = "Hold Down",
                .InvBarrel = "InvBarrel",
                .InvDVBalls = "InvDVBalls",
                .InvDVCages = "InvDVCages",
                .InvDVSeats = "InvDVSeats",
                .InvHoldDown = "InvHoldDown",
                .InvPDVBalls = "InvPDVBalls",
                .InvPDVCages = "InvPDVCages",
                .InvPDVSeats = "InvPDVSeats",
                .InvPlunger = "InvPlunger",
                .InvPTVBalls = "InvPTVBalls",
                .InvPTVCages = "InvPTVCages",
                .InvPTVSeats = "InvPTVSeats",
                .InvRodGuide = "InvRodGuide",
                .InvSVBalls = "InvSVBalls",
                .InvSVCages = "InvSVCages",
                .InvSVSeats = "InvSVSeats",
                .InvTypeBallandSeat = "InvTypeBallandSeat",
                .InvValveRod = "InvValveRod",
                .LastPull = Today.Date,
                .Notes = "Notes",
                .OrderDate = Today.Date,
                .OrderedBy = "OrderedBy",
                .PONumber = "PONumber",
                .ShipDate = Today.Date,
                .ShipVia = "ShipVia",
                .Stroke = "Stroke",
                .TicketDate = Today.AddMonths(-1),
                .Well = Pump1.InstalledInWell,
                .CountySalesTaxRate = New CountySalesTaxRate() With {.SalesTaxRate = 0.00478, .CountyName = "KERN COUNTY"}
            })

            t.Inspections = New List(Of PartInspection) From {
                New PartInspection() With {.Quantity = 1, .PartFailed = Template1.Parts(0).PartTemplate, .TemplatePartDefID = 1, .PartReplaced = Template1.Parts(0).PartTemplate, .Result = "Replace", .ReasonRepaired = "Worn", .Sort = 1},
                New PartInspection() With {.Quantity = 1, .PartFailed = Template1.Parts(1).PartTemplate, .TemplatePartDefID = 2, .PartReplaced = Template1.Parts(1).PartTemplate, .Result = "Replace", .ReasonRepaired = "Worn", .Sort = 2},
                New PartInspection() With {.Quantity = 3, .PartFailed = Template1.Parts(2).PartTemplate, .TemplatePartDefID = 3, .PartReplaced = Template1.Parts(2).PartTemplate, .Result = "Replace", .ReasonRepaired = "Gutted", .Sort = 3}
            }

            t.LineItems = New List(Of LineItem) From {
                New LineItem() With {.PartTemplate = Template1.Parts(0).PartTemplate, .PartInspection = t.Inspections(0), .Quantity = 3, .UnitDiscount = 0.02, .UnitPrice = 455, .CollectSalesTax = True, .SortOrder = 1},
                New LineItem() With {.PartTemplate = Template1.Parts(1).PartTemplate, .PartInspection = t.Inspections(1), .Quantity = 2, .UnitDiscount = 0.0, .UnitPrice = 1000, .CollectSalesTax = False, .SortOrder = 2},
                New LineItem() With {.PartTemplate = Template1.Parts(2).PartTemplate, .PartInspection = t.Inspections(2), .Quantity = 1, .UnitDiscount = 0.05, .UnitPrice = 1, .CollectSalesTax = True, .SortOrder = 3}
            }

            DataSource.SaveChanges()
        End Sub
    End Class
End Namespace