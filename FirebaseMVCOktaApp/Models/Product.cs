using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirebaseMVCOktaApp.Models
{
    public class Product
    {
        public string TimestampUtc { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
    }
}