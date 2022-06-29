using System;
using System.Collections.Generic;

#nullable disable

namespace CurrencyCalculator.Models
{
    public partial class RootDb
    {
        public RootDb()
        {
            RatesDbs = new HashSet<RatesDb>();
        }

        public string Table { get; set; }
        public string No { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int Id { get; set; }

        public virtual ICollection<RatesDb> RatesDbs { get; set; }
    }
}
