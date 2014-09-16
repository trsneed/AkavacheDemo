using System;
using System.Net.Http;
using ModernHttpClient;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AkavacheDemo.Common
{
    public class Webservice
    {
        private const string _endpoint="http://airportcode.riobard.com/airport/";
        private const string _dataType="?fmt=JSON";


        public async Task<Airport> GetAirportByCode(string code)
        {
            using (var client = new HttpClient(new NativeMessageHandler()))
            {
                var request = string.Format("{0}{1}{2}", _endpoint, code, _dataType);
                var result =  await client.GetAsync(request);

                using (var reader = new StreamReader(await result.Content.ReadAsStreamAsync()))
                {
                    return JsonConvert.DeserializeObject<Airport>(await reader.ReadToEndAsync());
                }
            }
        }
    }
}

