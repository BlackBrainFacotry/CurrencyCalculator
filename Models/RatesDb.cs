using System;
using System.Collections.Generic;

#nullable disable

namespace CurrencyCalculator.Models
{
    public partial class RatesDb
    {
        public string Currency { get; set; }
        public string Code { get; set; }
        public double? Mid { get; set; }
        public int RootId { get; set; }

        public virtual RootDb Root { get; set; }
    }
}
