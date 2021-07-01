using AcePump.WebApi.Models;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.OData;
using System.IO;
using AcePump.Domain.Models;
using System.Web.Hosting;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System;
using AcePump.Common.Tools.Files;
using System.Threading.Tasks;
using System.Text;
using System.Security.Claims;
using System.Collections.Generic;
using AcePump.Rdlc.Builder;
using AcePump.Rdlc.Pdf;
using AcePump.Domain.ReportDefinitions;
using AcePump.Common.ImageProcessing;
using AcePump.Domain.BL;
using AcePump.Common.Storage;

namespace AcePump.WebApi.Controllers
{

    [RoutePrefix("api/deliverytickets")]
    public class DeliveryTicketController : BaseApiController
    {
        [Route("")]
        [HttpGet]
        [EnableQuery]
        public IQueryable<DeliveryTicketModel> Get()
        {
            return from dt in Db.DeliveryTickets
                   select new DeliveryTicketModel
                   {
                       DeliveryTicketID = dt.DeliveryTicketID,
                       WellID = dt.WellID,
                       WellNumber = dt.Well.WellNumber,
                       LeaseName = dt.Well.Lease.LocationName,
                       CustomerID = dt.CustomerID,
                       CustomerName = dt.Customer.CustomerName,
                       PumpFailedID = dt.PumpFailedID,
                       PumpFailedPrefix = dt.PumpFailed.ShopLocation.Prefix,
                       PumpFailedNumber = dt.PumpFailed.PumpNumber,
                       ReasonStillOpen = dt.ReasonStillOpen,
                       TicketDate = dt.TicketDate,
                       CloseTicket = dt.CloseTicket,
                       HasSignature = dt.Signature == null ? false : true
                   };
        }

        [Route("{id:int}/lineItems")]
        [HttpGet]
        public IQueryable<SignatureLineItemModel> GetLineItems(int id)
        {
            return from lineItem in Db.LineItems
                   where lineItem.DeliveryTicketID == id
                   select new SignatureLineItemModel
                   {
                       Item = lineItem.Description,
                       Quantity = lineItem.Quantity,
                       UnitPrice = (lineItem.UnitPrice * (1 - lineItem.UnitDiscount)),
                       LineIsTaxable = lineItem.CollectSalesTax ?? false
                   };
        }

        [Route("{id:int}/pdf")]
        [HttpGet]
        public HttpResponseMessage ShowDeliveryTicketPdf(int id)
        {
            try
            {
                DtReportDefinition reportDefinition = new DtReportDefinition(Db, id);
                RdlcBuilder builder = new RdlcBuilder();

                var strmPdf = builder.LoadReportDefinition(reportDefinition).CreatePdfStream();

                var result = new HttpResponseMessage(HttpStatusCode.OK);

                result.Content = new StreamContent(strmPdf);

                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
                result.Content.Headers.ContentDisposition  = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = builder.SaveAsName
                };
                return result;
            }
            catch (ArgumentException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }

        [Route("{id:int}/signatureData")]
        [HttpGet]
        public IHttpActionResult GetSignatureData(int id)
        {
            var signatureData = (from dt in Db.DeliveryTickets
                                where dt.DeliveryTicketID == id
                                select new SignatureDataModel
                                {
                                    DeliveryTicketID = dt.DeliveryTicketID,
                                    SalesTaxRate = dt.SalesTaxRate,
                                    Signature = dt.Signature,
                                    SignatureCompanyName = dt.SignatureCompanyName,
                                    SignatureDate = dt.SignatureDate,
                                    SignatureName = dt.SignatureName
                                }).FirstOrDefault();
            if (signatureData == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(signatureData) ;
            }
        }

        [Route("{id:int}/sign")]
        [HttpPatch]
        public IHttpActionResult Sign(SignatureDataModel signatureData)
        {
            DeliveryTicket dt = Db.DeliveryTickets.Find(signatureData.DeliveryTicketID);

            if (dt != null)
            {
                dt.Signature = signatureData.Signature;
                dt.SignatureCompanyName = signatureData.SignatureCompanyName;
                dt.SignatureDate = signatureData.SignatureDate;
                dt.SignatureName = signatureData.SignatureName;

                Db.SaveChanges();
                return Ok();
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }


        [Route("{id:int}/images")]
        [HttpGet]
        [EnableQuery]
        public IQueryable<DeliveryTicketImageModel> GetImages(int id)
        {
            return from img in Db.DeliveryTicketImageUploads
                   where img.DeliveryTicketID == id
                   select new DeliveryTicketImageModel
                   {
                       DeliveryTicketID = img.DeliveryTicketID,
                       DeliveryTicketImageUploadID = img.DeliveryTicketImageUploadID,
                       UploadedBy = img.UploadedBy,
                       UploadedOn = img.UploadedOn,
                       Note = img.Note,
                       LargeImageName = img.LargeImageName,
                       SmallImageName = img.SmallImageName,
                       MimeType = img.MimeType
                   };
        }

        [Route("{DeliveryTicketID:int}/smallImages/{DeliveryTicketImageUploadID:int}")]
        [HttpGet]
        public HttpResponseMessage GetSmallImage(int DeliveryTicketID, int DeliveryTicketImageUploadID)
        {
            using(DeliveryTicketImageUploadRepository dtImageRepo = new DeliveryTicketImageUploadRepository(Db, ImageProcessingFactory.GetImageProcessingLibrary(), TenantContext.StorageProvider))
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var smImage = dtImageRepo.GetSmallImage(DeliveryTicketImageUploadID);
                result.Content = new StreamContent(smImage.ReadStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(smImage.MimeType ?? "image/jpeg");

                return result;
            }
        }

        [Route("{DeliveryTicketID:int}/largeImages/{DeliveryTicketImageUploadID:int}")]
        [HttpGet]
        public HttpResponseMessage GetLargeImage(int DeliveryTicketID, int DeliveryTicketImageUploadID)
        {
            using (DeliveryTicketImageUploadRepository dtImageRepo = new DeliveryTicketImageUploadRepository(Db, ImageProcessingFactory.GetImageProcessingLibrary(), TenantContext.StorageProvider))
            {
                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                var lgImage = dtImageRepo.GetLargeImage(DeliveryTicketImageUploadID);
                result.Content = new StreamContent(lgImage.ReadStream);
                result.Content.Headers.ContentType = new MediaTypeHeaderValue(lgImage.MimeType ?? "image/jpeg");

                return result;
            }
        }

        [Route("{DeliveryTicketID:int}/images/{imageUploadId:int}")]
        [HttpDelete]        
        public void DeleteImage(int DeliveryTicketID, int imageUploadId)
        {
            using (DeliveryTicketImageUploadRepository dtImageRepo = new DeliveryTicketImageUploadRepository(Db, ImageProcessingFactory.GetImageProcessingLibrary(), StorageFactory.GetStorageProvider(VirtualPathMapper.Instance)))
            {
                dtImageRepo.DeleteUpload(imageUploadId);
            }
        }

        [Route("{DeliveryTicketID:int}/images/{imageUploadId:int}/Note")]
        [HttpPatch]
        public IHttpActionResult UpdateImageNote(int DeliveryTicketID, int imageUploadId, string note)
        {
            DeliveryTicketImageUpload image = Db.DeliveryTicketImageUploads.Find(imageUploadId);

            if(image != null) {
                image.Note = note;

                Db.SaveChanges();
                return Ok(new
                {
                    note = note
                });
            }
            else
            {
                return StatusCode(HttpStatusCode.BadRequest);
            }
        }

        [Route("{DeliveryTicketID:int}/images")]
        [HttpPut]
        public async Task<HttpResponseMessage> PutImage(int DeliveryTicketID)
        {
            DeliveryTicket deliveryTicket = Db.DeliveryTickets.Find(DeliveryTicketID);
            if (deliveryTicket == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            try
            {
                var provider = await Request.Content.ReadAsMultipartAsync();

                foreach (var stream in provider.Contents)
                {
                    byte[] imageBytes = await stream.ReadAsByteArrayAsync();
                    imageBytes = Convert.FromBase64String(Encoding.ASCII.GetString(imageBytes));

                    if (FileUtil.IsJPG(imageBytes))
                    {
                        using (DeliveryTicketImageUploadRepository dtImageRepo = new DeliveryTicketImageUploadRepository(Db, ImageProcessingFactory.GetImageProcessingLibrary(), TenantContext.StorageProvider))
                        {
                            var identity = (ClaimsIdentity)User.Identity;
                            IEnumerable<Claim> claims = identity.Claims;
                            string uploadedBy = claims.FirstOrDefault(x => x.Type == "username").Value;

                            string contentType = "image/jpeg";

                            var uploadDbRecord = dtImageRepo.AddUpload(imageBytes, contentType, uploadedBy, deliveryTicket);
                            Db.SaveChanges();
                        }
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "You must upload a JPG");
                    }
                }
                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}
