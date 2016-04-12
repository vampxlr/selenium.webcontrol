using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Selenium.Models
{
    public class dbContext  :DbContext
    {

        public dbContext()
            : base("name=TestSite")
        {
        }
        public DbSet<emailListModel> EmailLists { get; set; }
        public DbSet<stockModel> Stocks { get; set; }
       

    }
}