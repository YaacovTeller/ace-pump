using AcePump.Domain.Models;
using AcePump.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace AcePump.WebApi.Controllers
{

    [RoutePrefix("api/pumps")]
    public class PumpController : BaseApiController
    {
        [Route("numbers/next")]        
        [HttpGet]
        public string GetNextPumpNumber(int shopLocationId)
        {
            string nextAvailablePumpNumber = findNextAvailableNumber(shopLocationId);

            if (string.IsNullOrEmpty(nextAvailablePumpNumber))
            {
                nextAvailablePumpNumber = Db.ShopLocations
                    .Where(x => x.ShopLocationID == shopLocationId)
                    .Select(x => x.StartingPumpNumber)
                    .FirstOrDefault()
                    .ToString();
            }            
            return nextAvailablePumpNumber;
        }

        private string findNextAvailableNumber(int shopLocationId)
        {
            string previousPumpNumberString = (from pump in Db.Pumps
                                                    where pump.ShopLocationID == shopLocationId
                                                    orderby pump.PumpNumber.Length descending, pump.PumpNumber descending                                                    
                                                    select pump.PumpNumber)
                                                    .FirstOrDefault();

            int parsedNumber;
            if (!int.TryParse(previousPumpNumberString, out parsedNumber))
            {
                parsedNumber = 0;
            }

            if (parsedNumber == 0) {
                int startingNumber = Db.ShopLocations.Where(x => x.ShopLocationID == shopLocationId)
                    .Select(x => x.StartingPumpNumber)
                    .FirstOrDefault();
                if(startingNumber != 0)
                {
                    return startingNumber.ToString();
                }
            }
            return (parsedNumber + 1).ToString();            
        }

        [Route("shopLocations")]        
        [HttpGet]
        public IQueryable<ShopLocationModel> GetShopLocations()
        {
            return from l in Db.ShopLocations                   
                   select new ShopLocationModel
                   {
                      ShopLocationID = l.ShopLocationID,
                      Prefix =l.Prefix,
                      Name= l.Name
                   };
        }
    }
}
