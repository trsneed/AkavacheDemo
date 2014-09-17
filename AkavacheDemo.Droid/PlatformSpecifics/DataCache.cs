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
            await BlobCache.LocalMachine.InsertObject(airport.code.ToLower(), airport);
        }
            
        public async Task<Airport> GetAirport(string airportCode)
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<Airport>(airportCode.ToLower());
            }
            catch(KeyNotFoundException ex)
            {
                //for now we want to have the service go get information
                return null;
            }
        }
    }
}

