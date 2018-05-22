using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace thippeswamy.Models
{
    public class ShelfContext
    {
        public List<int> sensors { get; set; }
        public List<string> productNames { get; set; }
        public int rowCount { get; set; }
        public int columnCount { get; set; }
        public int sensorId { get; set; }
        public string productName { get; set; }
        public string shelfCreatedSuccessMsg { get; set; }
    }
}