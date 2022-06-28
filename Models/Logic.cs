using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;

namespace CurrencyCalculator.Models
{
    public class Logic
    {
        public Models.Root[] objJson;
        public void test()
        {

        }
        public void GetDataFromApi()
        {
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://api.nbp.pl/api/exchangerates/tables/a/?format=json");
            objJson = JsonConvert.DeserializeObject<Models.Root[]>(json);
            
        }
    }

}
