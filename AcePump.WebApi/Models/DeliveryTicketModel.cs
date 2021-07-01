using System;

namespace AcePump.WebApi.Models
{
    public class DeliveryTicketModel
    {

        public int DeliveryTicketID { get; set; }
        public int? WellID { get; set; }
        public string WellNumber { get; set; }
        public string LeaseName { get; set; }
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }        
        public int? PumpFailedID { get; set; }
        public string PumpFailedPrefix { get; set; }
        public string PumpFailedNumber { get; set; }
        public string ReasonStillOpen { get; set; }        
        public DateTime? TicketDate { get; set; }
        public bool? CloseTicket { get; set; }
        public bool HasSignature { get; set; }
    }
}