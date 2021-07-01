using System;

namespace AcePump.WebApi.Models
{
    public class SignatureDataModel    
    {
        public int DeliveryTicketID { get; set; }
        public DateTime? SignatureDate { get; set; }
        public string SignatureName { get; set; }
        public string SignatureCompanyName { get; set; }
        public byte[] Signature { get; set; }
        public decimal SalesTaxRate { get; set; }
    }
}

