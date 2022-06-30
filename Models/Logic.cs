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
        public void GetData()
        {
            try
            {
                GetDataFromApi();
            }
            catch
            {
                ReadDataFromDb();
            }
        }
        public void GetDataFromApi()
        {
            var webClient = new WebClient();
            var json = webClient.DownloadString(@"http://api.nbp.pl/api/exchangerates/tables/a/?format=json");
            objJson = null;
            objJson = JsonConvert.DeserializeObject<Models.Root[]>(json); 
            Models.Rate ratePLN = new Models.Rate();
            ratePLN.currency = "Polski Zloty";
            ratePLN.code = "PLN";
            ratePLN.mid = 1;
            objJson[0].rates.Add(ratePLN);
            RemoveDate();
            SaveData();
            

        }
        public void RemoveDate()
        {
            using( var context = new CurrencyContext())
            {
                var x = from rates in context.RatesDbs
                        select rates;
                foreach( var item in x)
                {
                    context.RatesDbs.Remove(item);
                }
                var y = from root in context.RootDbs
                        select root;
                foreach( var item in y)
                {
                    context.RootDbs.Remove(item);
                }
                var cnt = context.SaveChanges();
            }
        }

        public void ReadDataFromDb()
        {
            objJson = null;
            int i = 0;
            using (var context = new CurrencyContext())
            {
                var x = (from rates in context.RatesDbs
                        select new
                        {
                            rates.Code,
                            rates.Mid,
                            rates.Currency
                        }).ToList();

                var y = (from root in context.RootDbs
                        select new
                        {
                            root.EffectiveDate,
                            root.No,
                            root.Table
                        }).ToList();
                objJson = new Models.Root[y.Count];
                objJson[0] = new Root();
                foreach(var rootItem in y)
                {
                    objJson[0].no = rootItem.No;
                    objJson[0].table = rootItem.Table;
                    objJson[0].rates = new List<Rate>();
                    objJson[0].effectiveDate = ((DateTime)(rootItem.EffectiveDate)).ToString("yyyy-MM-dd");
                    foreach ( var ratesItem in x)
                    {
                        Rate rate = new Rate();
                        rate.code = ratesItem.Code;
                        rate.currency = ratesItem.Currency;
                        rate.mid = (double)ratesItem.Mid;
                        objJson[0].rates.Add(rate);
                        i++;
                    }
                }
            }
        }
        public void SaveData()
        {
            using(var context = new CurrencyContext())
            {
                foreach(var x in objJson)
                {
                    RootDb rootDb = new();
                    rootDb.EffectiveDate = DateTime.ParseExact( x.effectiveDate,"yyyy-MM-dd",null);
                    rootDb.No = x.no;
                    rootDb.Table = x.table;
                    rootDb.Id = 0;
                    context.RootDbs.Add(rootDb);
                    foreach(var y in x.rates)
                    {
                        RatesDb rateDb = new();
                        rateDb.Code = y.code;
                        rateDb.Currency = y.currency;
                        rateDb.Mid = y.mid;
                        rateDb.RootId = rootDb.Id;
                        context.RatesDbs.Add(rateDb);
                        
                        
                    }
                    try
                    {
                        context.SaveChanges();
                    }
                    catch
                    {

                    }
                }   
            }
        }
        public float ConvertCurrency(string curr1, string curr2, float curr_value1)
        {
            Models.Rate curr_obj1=null;
            Models.Rate curr_obj2=null;
         
            foreach (var x in objJson)
            {
                foreach( var y in x.rates)
                {
                    if(y.code == curr1)
                    {
                        curr_obj1 = y;
                    }
                    if(y.code == curr2)
                    {
                        curr_obj2 = y;
                    }
                  
                }    
            }
            if (curr_obj1 != null && curr_obj2 != null)
            {
                float mid_value1 = (float)curr_obj1.mid * curr_value1 / (float)curr_obj2.mid;
                return mid_value1;

            }
            else
            {
                return 0;
            }
        }
    }

}
