using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Selenium.Models.ViewModels
{
    public class BillingViewModel
    {
       

                    public string firstName { get; set; }
                    public string lastName { get; set; }
                    public string BillingAddress1 { get; set; }
                    public string BillingAddress2 { get; set; }
                    public string city { get; set; }
                    public string phone { get; set; }
                    public string state { get; set; }
                    public string expirationMonth { get; set; }
                    public string expirationYear { get; set; }
                    public string ccType { get; set; }
                    public string ccNumber { get; set; }
                    public string cvv { get; set; }
                    public string shipping { get; set; }
                    public string zip { get; set; }
                    
    }
}