Imports AcePump.Domain.Models
Imports AcePump.Common
Imports AcePump.Web.Controllers
Imports System.IO
Imports Kendo.Mvc.UI
Imports Kendo.Mvc.Extensions
Imports System.Data.Entity
Imports AcePump.Web.Areas.Customers.Models
Imports Yesod.Mvc
Imports AcePump.Domain.BL
Imports AcePump.Common.ImageProcessing
Imports AcePump.Common.Storage
Imports System.Net

Namespace Areas.Customers.Controllers

    <Authorize(Roles:=AcePumpSecurityRoles.Customer)> _
    Public Class DeliveryTicketController
        Inherits AcePumpControllerBase

        Private Const SignatureDataFormat = "data:image/png;base64,"

        Private _CustomerAccessIDs As List(Of Integer)
        Private ReadOnly Property CustomerAccessIDs As List(Of Integer)
            Get
                If _CustomerAccessIDs Is Nothing Then
                    _CustomerAccessIDs = HttpContext.AcePumpUser().Profile.CustomerAccessList.Values.ToList()
                End If

                Return _CustomerAccessIDs
            End Get
        End Property

        Private _DeliveryTicketSignatureViewModelMapper As ModelMapper(Of DeliveryTicket, DeliveryTicketSignatureViewModel)
        Private ReadOnly Property DeliveryTicketSignatureViewModelMapper As ModelMapper(Of DeliveryTicket, DeliveryTicketSignatureViewModel)
            Get
                If _DeliveryTicketSignatureViewModelMapper Is Nothing Then
                    _DeliveryTicketSignatureViewModelMapper = New ModelMapper(Of DeliveryTicket, DeliveryTicketSignatureViewModel)(
                        Function(x) New DeliveryTicketSignatureViewModel With {
                            .DeliveryTicketID = x.DeliveryTicketID,
                            .SignatureDate = x.SignatureDate,
                            .SignatureName = x.SignatureName,
                            .Signature = x.Signature,
                            .SignatureCompanyName = x.SignatureCompanyName,
                            .CustomerName = x.Customer.CustomerName,
                            .Lease = x.Well.Lease.LocationName,
                            .Well = x.Well.WellNumber,
                            .TicketDate = x.TicketDate,
                            .CloseTicket = x.CloseTicket.HasValue AndAlso x.CloseTicket.Value,
                            .SalesTaxRate = x.SalesTaxRate,
                            .LineItems = x.LineItems.OrderBy(Function(y) y.SortOrder) _
                                                    .Select(Function(y) New DeliveryTicketSignatureLineItemGridRowModel With {
                                                                .Item = y.Description,
                                                                .Quantity = y.Quantity,
                                                                .UnitPrice = (y.UnitPrice * (1 - y.UnitDiscount)),
                                                                .LineIsTaxable = y.CollectSalesTax.HasValue AndAlso y.CollectSalesTax.Value
                                                            })
                        })
                End If

                Return _DeliveryTicketSignatureViewModelMapper
            End Get
        End Property

        Private Property FileManager As New KendoGridRequestManager(Of DeliveryTicketImageUpload, DeliveryTicketImageUploadGridRow)(
            DataSource,
            Function(x) New DeliveryTicketImageUploadGridRow() With {
                .DeliveryTicketImageUploadID = x.DeliveryTicketImageUploadID,
                .LargeImagePath = "/DeliveryTicket/GetLargeImage/" & x.DeliveryTicketImageUploadID,
                .SmallImagePath = "/DeliveryTicket/GetSmallImage/" & x.DeliveryTicketImageUploadID,
                .UploadedBy = x.UploadedBy,
                .UploadedOn = x.UploadedOn,
                .Note = x.Note
            },
            Function(x) x.DeliveryTicketID,
            Me
        )

        Private _TearDownItemManager As KendoGridRequestManager(Of PartInspection, TearDownItemGridRowModel)
        Private ReadOnly Property TearDownItemManager As KendoGridRequestManager(Of PartInspection, TearDownItemGridRowModel)
            Get
                If _TearDownItemManager Is Nothing Then
                    _TearDownItemManager = New KendoGridRequestManager(Of PartInspection, TearDownItemGridRowModel)(
                        DataSource,
                        Function(x As PartInspection) New TearDownItemGridRowModel With {
                            .PartInspectionID = x.PartInspectionID,
                            .DeliveryTicketID = x.DeliveryTicketID,
                            .SortOrder = If(x.Sort.HasValue, x.Sort.Value, 0),
                            .Result = x.Result,
                            .ReasonRepaired = x.ReasonRepaired,
                            .OriginalPartTemplateNumber = If(x.PartFailed IsNot Nothing, x.PartFailed.Number, ""),
                            .OriginalPartTemplateID = If(x.PartFailedID.HasValue, x.PartFailedID.Value, 0),
                            .PartDescription = If(x.PartFailed IsNot Nothing, x.PartFailed.Description, ""),
                            .Quantity = If(x.Quantity.HasValue, x.Quantity.Value, 0)
                        },
                        Function(x) x.DeliveryTicketID,
                        Me
                    )
                End If

                Return _TearDownItemManager
            End Get
        End Property

        '
        ' GET: /DeliveryTicket/Index

        Function Index(pumpId As Integer?) As ActionResult
            Dim model As PumpViewModel = Nothing

            If pumpId.HasValue Then
                model = DataSource.Pumps _
                            .Select(Function(x) New PumpViewModel() With {
                                        .PumpID = x.PumpID,
                                        .ShopLocationPrefix = x.ShopLocation.Prefix,
                                        .PumpNumber = x.PumpNumber
                                    }) _
                            .SingleOrDefault(Function(x) x.PumpID = pumpId.Value)
            End If

            Return View(model)
        End Function

        '
        ' POST: /DeliveryTicket/List

        <HttpPost()> _
        Public Function List(pumpId As Integer?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim filtered As IQueryable(Of DeliveryTicket) = DataSource.DeliveryTickets _
                                                                .Where(Function(x) x.CustomerID.HasValue AndAlso CustomerAccessIDs.Contains(x.CustomerID.Value)) _
                                                                .OrderByDescending(Function(x) x.TicketDate)

            If pumpId.HasValue Then
                filtered = filtered.Where(Function(x) _
                                            (x.PumpFailedID.HasValue AndAlso x.PumpFailedID.Value = pumpId.Value) _
                                            Or (x.PumpDispatchedID.HasValue AndAlso x.PumpDispatchedID.Value = pumpId.Value))
            End If

            Dim projected = From x In filtered
                            Select New DeliveryTicketGridRowViewModel() With {
                                .DeliveryTicketID = x.DeliveryTicketID,
                                .TicketDate = If(x.TicketDate.HasValue, x.TicketDate.Value, Nothing),
                                .IsClosed = (x.CloseTicket.HasValue AndAlso x.CloseTicket.Value),
                                .WellNumber = If(x.Well IsNot Nothing, x.Well.WellNumber, ""),
                                .Lease = If(x.Well IsNot Nothing, x.Well.Lease.LocationName, ""),
                                .PumpFailedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix & x.PumpFailed.PumpNumber, ""),
                                .PumpFailedDate = If(x.PumpFailedDate.HasValue, x.PumpFailedDate.Value, Nothing),
                                .PumpDispatchedNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix & x.PumpDispatched.PumpNumber, ""),
                                .PumpDispatchedTemplateVerbose = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.PumpTemplate.VerboseSpecificationSummary, ""),
                                .ShowDeliveryTicket = True,
                                .ShowRepairTicket = True
                            }

            Return Json(projected.ToDataSourceResult(req))

        End Function


        '
        ' GET: /DeliveryTicket/Details/[id]

        <HttpGet()> _
        Public Function Details(id As Integer) As ActionResult
            Dim dt As DeliveryTicketViewModel = DataSource.DeliveryTickets _
                                                   .Where(Function(t) _
                                                                        t.DeliveryTicketID = id _
                                                                        And t.CustomerID.HasValue AndAlso CustomerAccessIDs.Contains(t.CustomerID.Value)) _
                                                    .Select(Function(x) New DeliveryTicketViewModel() With {
                                                        .DeliveryTicketID = x.DeliveryTicketID,
                                                        .TicketDate = x.TicketDate.Value,
                                                        .Notes = x.Notes,
                                                        .HoldDown = x.HoldDown,
                                                        .LeaseAndWell = x.Well.Lease.LocationName & " " & x.Well.WellNumber,
                                                        .PumpDispatchedTemplateVerbose = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.PumpTemplate.VerboseSpecificationSummary, ""),
                                                        .ShipVia = x.ShipVia,
                                                        .PONumber = x.PONumber,
                                                        .ShipDate = x.ShipDate,
                                                        .PumpRepairedNumber = If(x.PumpFailed IsNot Nothing, x.PumpFailed.ShopLocation.Prefix & x.PumpFailed.PumpNumber, ""),
                                                        .PumpOutNumber = If(x.PumpDispatched IsNot Nothing, x.PumpDispatched.ShopLocation.Prefix & x.PumpDispatched.PumpNumber, ""),
                                                        .OrderedBy = x.OrderedBy,
                                                        .LastPull = x.LastPull,
                                                        .Stroke = x.Stroke,
                                                        .InvBarrel = x.InvBarrel,
                                                        .InvSVCages = x.InvSVCages,
                                                        .InvDVCages = x.InvDVCages,
                                                        .InvSVSeats = x.InvSVSeats,
                                                        .InvDVSeats = x.InvDVSeats,
                                                        .InvSVBalls = x.InvSVBalls,
                                                        .InvDVBalls = x.InvDVBalls,
                                                        .InvHoldDown = x.InvHoldDown,
                                                        .InvValveRod = x.InvValveRod,
                                                        .InvPlunger = x.InvPlunger,
                                                        .InvPTVCages = x.InvPTVCages,
                                                        .InvPDVCages = x.InvPDVCages,
                                                        .InvPTVSeats = x.InvPTVSeats,
                                                        .InvPDVSeats = x.InvPDVSeats,
                                                        .InvPTVBalls = x.InvPTVBalls,
                                                        .InvPDVBalls = x.InvPDVBalls,
                                                        .InvRodGuide = x.InvRodGuide,
                                                        .InvTypeBallandSeat = x.InvTypeBallandSeat
                                                        }).FirstOrDefault()

            If dt IsNot Nothing Then
                Return View(dt)

            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET: /DeliveryTicket/Choose

        <Authorize(Roles:=AcePumpSecurityRoles.DeliveryTicketSigner)> _
        <HttpGet()> _
        Public Function Choose() As ActionResult
            If TempData("Error") IsNot Nothing Then
                ModelState.AddModelError("", TempData("Error"))
            End If

            Return View(ModelState)
        End Function

        '
        ' POST: /DeliveryTicket/Choose

        <Authorize(Roles:=AcePumpSecurityRoles.DeliveryTicketSigner)> _
        <HttpPost()> _
        Public Function Choose(id As Integer?) As ActionResult
            If id.HasValue Then
                Return RedirectToAction("Sign", New With {.id = id})
            Else
                ModelState.AddModelError("", "No valid deliveryticket number was entered.")
                Return View()
            End If
        End Function

        '
        ' GET: /DeliveryTicket/Sign

        <Authorize(Roles:=AcePumpSecurityRoles.DeliveryTicketSigner)> _
        <HttpGet()> _
        Public Function Sign(id As Integer?) As ActionResult
            If id.HasValue Then
                Dim deliveryTicket As DeliveryTicketSignatureViewModel = DataSource.DeliveryTickets _
                                                                                   .Where(Function(x) x.DeliveryTicketID = id.Value AndAlso CustomerAccessIDs.Contains(x.CustomerID.Value)) _
                                                                                   .Select(DeliveryTicketSignatureViewModelMapper.Selector) _
                                                                                   .SingleOrDefault()


                If deliveryTicket Is Nothing Then
                    TempData("Error") = "Could not find that delivery ticket. Please try again."
                    Return RedirectToAction("Choose")
                End If

                Return View(deliveryTicket)
            Else
                Return RedirectToAction("Choose")
            End If
        End Function

        '
        ' POST: /DeliveryTicket/Sign

        <Authorize(Roles:=AcePumpSecurityRoles.DeliveryTicketSigner)> _
        <HttpPost()> _
        Public Function Sign(model As DeliveryTicketSignatureViewModel) As ActionResult
            If Not ModelState.IsValid() Then
                Return View(model)
            End If

            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets _
                                                                .Include(Function(x) x.LineItems) _
                                                                .SingleOrDefault(Function(x) x.DeliveryTicketID = model.DeliveryTicketID AndAlso CustomerAccessIDs.Contains(x.CustomerID.Value))

            If deliveryTicket Is Nothing Then
                TempData("Error") = "There was a problem signing that delivery ticket. Please try again."
                Return RedirectToAction("Choose")
            End If

            Dim signatureByteArray As Byte() = ConvertSignatureBase64ToByteArray(model.SignatureBase64)
            deliveryTicket.SignatureDate = Today()
            deliveryTicket.SignatureName = model.SignatureName
            deliveryTicket.SignatureCompanyName = model.SignatureCompanyName
            deliveryTicket.Signature = signatureByteArray

            model.SignatureDate = deliveryTicket.SignatureDate
            model.Signature = signatureByteArray

            DataSource.SaveChanges()

            Return View(DeliveryTicketSignatureViewModelMapper.Convert(deliveryTicket))
        End Function

        Private Function ConvertSignatureBase64ToByteArray(signature As String) As Byte()
            signature = signature.Replace(SignatureDataFormat, "")
            Return Convert.FromBase64String(signature)
        End Function

        '
        ' POST: /DeliveryTicket/Resign

        <Authorize(Roles:=AcePumpSecurityRoles.DeliveryTicketSigner)>
        <HttpPost()>
        Public Function Resign(model As DeliveryTicketSignatureViewModel) As ActionResult
            Dim deliveryTicket As DeliveryTicket = DataSource.DeliveryTickets.SingleOrDefault(Function(x) x.DeliveryTicketID = model.DeliveryTicketID AndAlso CustomerAccessIDs.Contains(x.CustomerID.Value))

            If deliveryTicket Is Nothing Then
                TempData("Error") = "There was a problem resigning that delivery ticket. Please try again."
                Return RedirectToAction("Choose", New With {.id = model.DeliveryTicketID})
            End If

            deliveryTicket.SignatureName = ""
            deliveryTicket.SignatureDate = Nothing
            deliveryTicket.SignatureCompanyName = ""

            DataSource.SaveChanges()

            Return RedirectToAction("Sign", New With {.id = model.DeliveryTicketID})
        End Function

        '
        ' GET: /DeliveryTicket/RepairDetails

        Function RepairDetails(id As Integer) As ActionResult
            Dim rtm As RepairTicketViewModel = DataSource.DeliveryTickets _
                                                           .Where(Function(t) t.DeliveryTicketID = id _
                                                                                And t.CustomerID.HasValue AndAlso CustomerAccessIDs.Contains(t.CustomerID.Value)) _
                                                            .Select(Function(t) New RepairTicketViewModel() With {
                                                                 .DeliveryTicketID = t.DeliveryTicketID,
                                                                 .TicketDate = t.TicketDate,
                                                                 .Notes = t.Notes,
                                                                 .HoldDown = t.HoldDown,
                                                                 .LeaseAndWell = t.Well.Lease.LocationName & " " & t.Well.WellNumber,
                                                                 .PumpFailedID = t.PumpFailedID,
                                                                 .PumpRepairedNumber = t.PumpFailed.ShopLocation.Prefix & t.PumpFailed.PumpNumber,
                                                                 .PumpOutNumber = t.PumpDispatched.ShopLocation.Prefix & t.PumpDispatched.PumpNumber,
                                                                 .PumpRepairedTemplateVerbose = t.PumpFailed.PumpTemplate.VerboseSpecificationSummary,
                                                                 .PlungerBarrelWear = t.PlungerBarrelWear,
                                                                 .IsRepairComplete = t.RepairComplete,
                                                                 .RepairMode = t.RepairMode,
                                                                 .CustomerUsesInventory = t.Customer.UsesInventory
                                                             }) _
                                                             .FirstOrDefault()

            If rtm.RepairMode = AcePumpRepairModes.TearDown Then
                Return RedirectToAction("TearDownDetails", New With {.id = id})
            End If

            If rtm IsNot Nothing Then
                Return View(rtm)
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET: /DeliveryTicket/TearDownDetails

        Function TearDownDetails(id As Integer) As ActionResult
            Dim tdm As TearDownViewModel = DataSource.DeliveryTickets _
                                                           .Where(Function(t) t.DeliveryTicketID = id _
                                                                                And t.CustomerID.HasValue AndAlso CustomerAccessIDs.Contains(t.CustomerID.Value)) _
                                                            .Select(Function(t) New TearDownViewModel() With {
                                                                 .DeliveryTicketID = t.DeliveryTicketID,
                                                                 .TicketDate = t.TicketDate,
                                                                 .Notes = t.Notes,
                                                                 .PumpRepairedNumber = t.PumpFailed.ShopLocation.Prefix & t.PumpFailed.PumpNumber,
                                                                 .PumpRepairedTemplateVerbose = t.PumpFailed.PumpTemplate.VerboseSpecificationSummary,
                                                                 .IsRepairComplete = t.RepairComplete,
                                                                 .RepairMode = t.RepairMode
                                                             }) _
                                                             .FirstOrDefault()

            If tdm.RepairMode = AcePumpRepairModes.Repair Then
                Return RedirectToAction("RepairDetails", New With {.id = id})
            End If

            If tdm IsNot Nothing Then
                Return View(tdm)
            Else
                Return RedirectToAction("Index")
            End If
        End Function

        '
        ' GET: /DeliveryTicket/GetSmallImage/id
        <HttpGet()>
        Public Function GetSmallImage(id As Integer) As ActionResult
            Using dtImageRepo As New DeliveryTicketImageUploadRepository(DataSource, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance))
                Dim smImage = dtImageRepo.GetSmallImage(id)

                If smImage IsNot Nothing Then
                    Return New FileStreamResult(smImage.ReadStream, If(smImage.MimeType, "image/jpeg"))

                Else
                    Return New HttpStatusCodeResult(HttpStatusCode.NotFound)
                End If
            End Using
        End Function

        '
        ' GET: /DeliveryTicket/GetLargeImage/id
        <HttpGet()>
        Public Function GetLargeImage(id As Integer) As ActionResult
            Using dtImageRepo As New DeliveryTicketImageUploadRepository(DataSource, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance))
                Dim lgImage = dtImageRepo.GetLargeImage(id)

                If lgImage IsNot Nothing Then
                    Return New FileStreamResult(lgImage.ReadStream, If(lgImage.MimeType, "image/jpeg"))

                Else
                    Return New HttpStatusCodeResult(HttpStatusCode.NotFound)
                End If
            End Using
        End Function

        '
        ' POST: /DeliveryTicket/InspectionList

        <HttpPost()>
        Public Function InspectionList(id As Integer?, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Dim tickets As IQueryable(Of PartInspection) = DataSource.PartInspections _
                                                                .Where(Function(x) x.DeliveryTicketID = id _
                                                                           And x.DeliveryTicket.CustomerID.HasValue _
                                                                           AndAlso CustomerAccessIDs.Contains(x.DeliveryTicket.CustomerID.Value))
            Dim r = tickets _
                            .Select(Function(t) New RepairTicketGridRowViewModel() With {
                                .DeliveryTicketID = t.DeliveryTicketID,
                                .PartInspectionID = t.PartInspectionID,
                                .Quantity = t.Quantity,
                                .OriginalPartTemplateNumber = If(t.PartFailed IsNot Nothing, t.PartFailed.Number, ""),
                                .PartDescription = If(t.PartFailed IsNot Nothing, t.PartFailed.Description, ""),
                                .Result = t.Result,
                                .ReplacementQuantity = t.ReplacementQuantity,
                                .ReplacementPartTemplateNumber = If(t.PartReplaced IsNot Nothing, t.PartReplaced.Number, ""),
                                .ReasonRepaired = t.ReasonRepaired,
                                .SortOrder = t.Sort,
                                .ReplacedWithInventoryPartID = t.ReplacedWithInventoryPartID,
                                .TemplatePartDefID = t.TemplatePartDefID
                            })
            Return Json(r.ToDataSourceResult(req))
        End Function

        '
        ' POST: /DeliveryTicket/TearDownItemList

        <HttpPost()>
        Public Function TearDownItemList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return TearDownItemManager.List(id, req)
        End Function

        '
        ' POST: /DeliveryTicket/ImageGridList

        <HttpPost()> _
        Public Function ImageGridList(id As Integer, <DataSourceRequest()> req As DataSourceRequest) As ActionResult
            Return FileManager.List(id, req)
        End Function


    End Class
End Namespace
