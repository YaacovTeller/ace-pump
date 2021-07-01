namespace AcePump.WebApi.Models
{
    public class SignatureLineItemModel    
    {
        public decimal Quantity { get; set; }
        public string Item { get; set; }
        public decimal UnitPrice { get; set; }
        public bool LineIsTaxable { get; set; }
        public decimal LineTotal {
            get { return UnitPrice * Quantity; }
        }        
    }
}

