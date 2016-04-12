using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selenium.Models
{
    public class stockModel
    {
        public int id { get; set; }
        public string itemName { get; set; }
        public int countId { get; set; }
        public bool inStock { get; set; }
        public string url { get; set; }
        public string type { get; set; }
    }
}