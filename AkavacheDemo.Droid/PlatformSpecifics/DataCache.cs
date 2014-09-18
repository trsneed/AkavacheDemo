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
        private Webservice _service;
        public DataCache (Webservice service)
        {
            _service = service;
        }
        public async Task StoreAirport(Airport airport)
        {
            await BlobCache.LocalMachine.InsertObject(airport.code.ToLower(), airport, DateTimeOffset.Now.AddMinutes(1));
        }
            
        public async Task<Airport> GetAirport(string airportCode)
        {
            return await BlobCache.LocalMachine.GetOrFetchObject<Airport>(airportCode.ToLower(),
                async () => await _service.GetAirportByCode(airportCode),
                DateTimeOffset.Now.AddMinutes(1));
        }

        public async Task CleanTheCache()
        {
             await BlobCache.LocalMachine.Vacuum();
        }

        public async Task<Splat.IBitmap> GetAnImage()
        {
            return await BlobCache.LocalMachine.LoadImageFromUrl("http://images.trsneed.com/blogstuff/Akavache/Aerial_view_of_San_Francisco_International_Airport_2010.jpg", false, 200,200);
        }
    }
}

