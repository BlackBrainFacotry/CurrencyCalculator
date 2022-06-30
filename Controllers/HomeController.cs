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
        public static Logic l = new Logic();

        public HomeController(ILogger<HomeController> logger)
        {            
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.cur1 = "PLN";
            ViewBag.cur2 = "PLN";
            ViewBag.curr_value1 = 1;
            ViewBag.curr_value2 = 1;
            l.GetData();
            return View(l);
        }
        [HttpPost]
        public IActionResult Index(string currency1,string currency2,string currency_value1)
        {
            ViewBag.cur1 = currency1;
            ViewBag.cur2 = currency2;
            ViewBag.curr_value1 = currency_value1;            
            ViewBag.curr_value2 = l.ConvertCurrency(currency1, currency2, currency_value1);
            if (currency_value1 == null) ViewBag.curr_value1 = ViewBag.curr_value2;
            return View(l);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
