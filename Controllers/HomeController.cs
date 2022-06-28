using CurrencyCalculator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;

namespace CurrencyCalculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected Root[] data;
       
        public HomeController(ILogger<HomeController> logger)
        {            
            _logger = logger;
        }

        public IActionResult Index()
        {
            GetDataFromApi();        
            return View();
        }

    

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      
        public ActionResult  GetDataFromApi()
        {           
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://api.nbp.pl/api/exchangerates/tables/a/?format=json");
            Models.Root[] objJson = JsonConvert.DeserializeObject<Models.Root[]>(json); 
            return View(objJson);  
        }
    }
}
