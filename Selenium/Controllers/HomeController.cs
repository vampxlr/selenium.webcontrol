using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Selenium.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Selenium.Controllers
{
 
    public  class BaseController : Controller
    {
        //public static FirefoxDriver driver = new FirefoxDriver(new FirefoxBinary(), MvcApplication.FProfile);
    }

    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SPA()
        {
            return View();
        }


    }
}
