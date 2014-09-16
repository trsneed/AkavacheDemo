using System;
using System.Threading.Tasks;
using Akavache;
using AkavacheDemo.Common;
using System.Reactive.Linq;
using System.Collections.Generic;

namespace AkavacheDemo.Droid
{
    public class DataCache
    {
        public async Task StoreAirport(Airport airport)
        {
            await BlobCache.LocalMachine.InsertObject(airport.code, airport);
        }

        public async Task StoreAirportWithTimeLimit(Airport airport)
        {
            //Only store data for 5 minutes
            await BlobCache.LocalMachine.InsertObject(airport.code, airport, DateTime.Now.AddMinutes(5));
        }

        public async Task<Airport> GetAirport(string airportCode)
        {
            try
            {
               return await BlobCache.LocalMachine.GetObject<Airport>(airportCode);
            }
            catch(KeyNotFoundException ex)
            {
                return null;
            }
        }
    }
}

