using System;

namespace AcePump.WebApi.Models
{
    public class DeliveryTicketImageModel
    {

        public int DeliveryTicketID { get; set; }
        public int DeliveryTicketImageUploadID { get; set; }
        public DateTime UploadedOn { get; set; }
        public string UploadedBy { get; set; }
        public string Note { get; set; }
        public string LargeImageName { get; set; }
        public string SmallImageName { get; set; }
        public string MimeType { get; set; }
    }
}