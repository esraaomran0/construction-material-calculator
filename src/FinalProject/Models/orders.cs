using FinalProject.Enums;
using Newtonsoft.Json.Converters;
using System;
using Newtonsoft.Json;

namespace FinalProject.Models
{
    public class orders
    {
        public int OrderNumber { get; set; }
        public string MaterialName { get; set; }
        public string Category { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ElementTypes elementtype { get; set; }

        public double Quantity { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }

        //[JsonIgnore]
        public double Total => Quantity * (double)UnitPrice;

        public DateTime Date { get; set; }
        public string Status { get; set; }
    }
}