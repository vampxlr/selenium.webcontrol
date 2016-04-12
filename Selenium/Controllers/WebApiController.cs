using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Selenium.Models;
using Selenium.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace Selenium.Controllers
{

    public abstract class BaseApiController : ApiController
    {
        public static FirefoxDriver driver = new FirefoxDriver(new FirefoxBinary(), MvcApplication.FProfile);
       // public static IWebDriver driver = new RemoteWebDriver(new Uri("http://127.0.0.1:6666/wd/hub"),
      //                 DesiredCapabilities.HtmlUnitWithJavaScript());
    }



    public class WebApiController : BaseApiController
    {



        [HttpGet]
        public void GetFirefox()
        {
            driver.Navigate().GoToUrl("http://www.usangels.com/");
        }
        [HttpGet]
        public string RegisterAndLogin()
        {

            driver.Navigate().GoToUrl("https://www.usangels.com/account/login");

            IWebElement EmailAddress = driver.FindElement(By.Name("NewEmailAddress"));

            string RandomEmail = generateRandomEmail();
            EmailAddress.SendKeys(RandomEmail + "@gmail.com");



            IWebElement Password = driver.FindElement(By.Name("NewPassword"));

            Password.SendKeys(RandomEmail + "007");
            IWebElement ConfirmPassword = driver.FindElement(By.Name("NewConfirmPassword"));
            ConfirmPassword.SendKeys(RandomEmail + "007");

            string css = ".four p:nth-child(5) button";
            IWebElement Register = driver.FindElement(By.CssSelector(css));
            Register.Click();
            return RandomEmail + "@gmail.com";
        }

            [HttpPost]
        public string ManualRegister(AccountViewModel Account)
        {
            string status = "Success";
            driver.Navigate().GoToUrl("https://www.usangels.com/account/login");

            IWebElement EmailAddress = driver.FindElement(By.Name("NewEmailAddress"));

   
            EmailAddress.SendKeys(Account.Email);



            IWebElement Password = driver.FindElement(By.Name("NewPassword"));

            Password.SendKeys(Account.Password);
            IWebElement ConfirmPassword = driver.FindElement(By.Name("NewConfirmPassword"));
            ConfirmPassword.SendKeys(Account.Password);

            string css = ".four p:nth-child(5) button";
            IWebElement Register = driver.FindElement(By.CssSelector(css));
            Register.Click();
            return status;
        }

            [HttpPost]
            public string ManualLogin(AccountViewModel Account)
            {
               // string status = "Success";
                driver.Navigate().GoToUrl("https://www.usangels.com/account/login");


                IWebElement EmailAddress = driver.FindElement(By.Name("EmailAddress"));


                EmailAddress.SendKeys(Account.Email);



                IWebElement Password = driver.FindElement(By.Name("Password"));

                Password.SendKeys(Account.Password);

                string css = ".first p:nth-child(4) button";
                //            IWebElement Login = driver.FindElement(By.CssSelector(css));
                IWebElement Login = driver.FindElement(By.ClassName("checkout"));

                Login.Click();
                return Account.Email;

            }


        public string generateRandomEmail()
        {

            using (var context = new dbContext())
            {
                string randomEmailString = "";
                int count = 0;

                while (count == 0)
                {
                    randomEmailString = generateRandomString(5);

                    count++;

                    emailListModel emailList = new emailListModel { Email = randomEmailString, Password = randomEmailString + "007" };
                    bool get = context.EmailLists.Any(r => r.Email == emailList.Email);

                    if (get)
                    {
                        count = 0;
                    }

                }
                emailListModel emailListToAdd = new emailListModel { Email = randomEmailString, Password = randomEmailString + "007" };


                context.EmailLists.Add(emailListToAdd);
                context.SaveChanges();
                context.Dispose();


                return randomEmailString;
            }


        }

        public String generateRandomString(int length)
        {
            //Initiate objects & vars    
            Random random = new Random();
            String randomString = "";
            int randNumber;

            //Loop ‘length’ times to generate a random number or character
            for (int i = 0; i < length; i++)
            {
                if (random.Next(1, 3) == 1)
                    randNumber = random.Next(97, 123); //char {a-z}
                else
                    randNumber = random.Next(48, 58); //int {0-9}

                //append random char or digit to random string
                randomString = randomString + (char)randNumber;
            }
            //return the random string
            return randomString;
        }
        [HttpGet]
        public IEnumerable<emailListModel> getEmailList()
        {


            using (dbContext context = new dbContext())
            {

                var emails = context.EmailLists.ToList();

                return emails;
            }


        }
        [HttpPost]
        public string LoginUsingEmail(emailListModel model)
        {

            driver.Navigate().GoToUrl("https://www.usangels.com/account/login");

            
            IWebElement EmailAddress = driver.FindElement(By.Name("EmailAddress"));


            EmailAddress.SendKeys(model.Email + "@gmail.com");



            IWebElement Password = driver.FindElement(By.Name("Password"));

            Password.SendKeys(model.Password);

            string css = ".first p:nth-child(4) button";
//            IWebElement Login = driver.FindElement(By.CssSelector(css));
            IWebElement Login = driver.FindElement(By.ClassName("checkout"));
            
            Login.Click();
            return model.Email + "@gmail.com";

        }
        [HttpGet]
        public string LogOut()
        {


            driver.Navigate().GoToUrl("https://www.usangels.com/account/logout");

            return "";


        }
        [HttpGet]
        public int placeOrderRandomFallHoliday()
        {
            bool noError = true;
            int count = 1;
            while (noError)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1006#page=1");

                Thread.Sleep(10000);
                string css = "div.sort-list:first-child .view-all";
                IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                ViewAll.Click();

                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    noError = false;
                }



            }
            driver.Navigate().GoToUrl("https://www.usangels.com/cart");
            string csss = ".inline";
            IWebElement Checkout = driver.FindElement(By.CssSelector(csss));
            Checkout.Click();

            return count;
        }

        [HttpGet]
        public int placeOrderRandomCommunion()
        {
            bool noError = true;
            int count = 1;


            while (noError)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1005");

                Thread.Sleep(10000);
                string css = "div.sort-list:first-child .view-all";
                IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                ViewAll.Click();

                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    noError = false;
                }

            }

            driver.Navigate().GoToUrl("https://www.usangels.com/cart");
            string csss = ".inline";
            IWebElement Checkout = driver.FindElement(By.CssSelector(csss));
            Checkout.Click();
            return count;
        }

        [HttpGet]
        public int placeOrderRandomFlowerGirl()
        {
            bool noError = true;
            int count = 1;

            while (noError)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1004");

                Thread.Sleep(10000);
                string css = "div.sort-list:first-child .view-all";
                IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                ViewAll.Click();

                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    noError = false;
                }

            }
            driver.Navigate().GoToUrl("https://www.usangels.com/cart");
            string csss = ".inline";
            IWebElement Checkout = driver.FindElement(By.CssSelector(csss));
            Checkout.Click();

            return count;
        }


        [HttpGet]
        public int placeOrderRandomSpringHoliday()
        {
            bool noError = true;
            int count = 1;

            while (noError)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1026");

                Thread.Sleep(10000);
                string css = "div.sort-list:first-child .view-all";
                try
                {
                    IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                    ViewAll.Click();
                }
                catch (Exception e)
                {


                }


                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    noError = false;
                }

            }
            driver.Navigate().GoToUrl("https://www.usangels.com/cart");
            string csss = ".inline";
            IWebElement Checkout = driver.FindElement(By.CssSelector(csss));
            Checkout.Click();
            return count;
        }

        [HttpGet]
        public int placeOrderRandomAll()
        {


            using (dbContext context = new dbContext())
            {
                var objCtx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
                objCtx.ExecuteStoreCommand("TRUNCATE TABLE [stockModels]");
            
            }

           

            int count = 1;
            Int64 endCount = 1000;
            while (count <= endCount)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1006#page=1");
                Thread.Sleep(10000);




                string css = "div.sort-list:first-child .view-all";
                IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                ViewAll.Click();

                Int64 totalItemNumber = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.products li').size()");
                endCount = totalItemNumber;



                css = ".products li:nth-child(" + count + ") div";
              // IWebElement product = driver.FindElement(By.CssSelector(css));

               try
               {
                   IWebElement product = (IWebElement)((IJavaScriptExecutor)driver).ExecuteScript("return $('.products li:nth-child(1) .list-img')");
                   product.Click();
               }
               catch (Exception e)
               {

                   throw e;
               }
                
               

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));


                
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = false, countId = currentCountNo, itemName = itemName, type = "FallHoliday", url = curretUrl });
                        context.SaveChanges();
                    }


                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = true, countId = currentCountNo, itemName = itemName, type = "FallHoliday", url = curretUrl });
                        context.SaveChanges();
                    }
                    count++;
                }

            }

            count = 1;
            while (count <= endCount)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1005");

                Thread.Sleep(10000);

                string css = "div.sort-list:first-child .view-all";
                IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                ViewAll.Click();

                Int64 totalItemNumber = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.products li').size()");
                endCount = totalItemNumber;


                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = false, countId = currentCountNo, itemName = itemName, type = "Communion", url = curretUrl });
                        context.SaveChanges();
                    }
                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = true, countId = currentCountNo, itemName = itemName, type = "Communion", url = curretUrl });
                        context.SaveChanges();
                    }

                    count++;
                }

            }


            count = 1;
            while (count <= endCount)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1004");

                Thread.Sleep(10000);

                string css = "div.sort-list:first-child .view-all";
                IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                ViewAll.Click();

                Int64 totalItemNumber = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.products li').size()");
                endCount = totalItemNumber;

                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = false, countId = currentCountNo, itemName = itemName, type = "FlowerGirl", url = curretUrl });
                        context.SaveChanges();
                    }


                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = true, countId = currentCountNo, itemName = itemName, type = "FlowerGirl", url = curretUrl });
                        context.SaveChanges();
                    }

                    count++;
                }

            }

            count = 1;
            while (count <= endCount)
            {
                driver.Navigate().GoToUrl("https://www.usangels.com/Department/1026");

                Thread.Sleep(10000);
                string css = "div.sort-list:first-child .view-all";
                try
                {
                    IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                    ViewAll.Click();
                }
                catch (Exception e)
                {


                }


                Int64 totalItemNumber = (Int64)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.products li').size()");
                endCount = totalItemNumber;

                css = ".products li:nth-child(" + count + ") div";
                IWebElement product = driver.FindElement(By.CssSelector(css));
                product.Click();

                css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = false, countId = currentCountNo, itemName = itemName, type = "SpringHoliday", url = curretUrl });
                        context.SaveChanges();
                    }

                    count++;
                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    string itemName = (string)((IJavaScriptExecutor)driver).ExecuteScript("return    $('.col-main h3').text()");
                    string curretUrl = driver.Url;
                    int currentCountNo = count;
                    using (var context = new dbContext())
                    {
                        context.Stocks.Add(new stockModel { inStock = true, countId = currentCountNo, itemName = itemName, type = "SpringHoliday", url = curretUrl });
                        context.SaveChanges();
                    }

                    count++;
                }

            }

            return count;
        }

        [HttpGet]
        public bool placeOrderByStockId(int id)
        {
            bool success = false;
            using (var context = new dbContext())
            {

                stockModel Stock = context.Stocks.SingleOrDefault(r => r.id == id);
                driver.Navigate().GoToUrl(Stock.url);

                //Thread.Sleep(10000);
                //string css = "div.sort-list:first-child .view-all";
                //IWebElement ViewAll = driver.FindElement(By.CssSelector(css));
                //ViewAll.Click();

                //css = ".products li:nth-child(" + Stock.countId + ") div";
                //IWebElement product = driver.FindElement(By.CssSelector(css));
                //product.Click();

                string css = "ul li:first-child div";
                IWebElement color = driver.FindElement(By.CssSelector(css));
                color.Click();
                ((IJavaScriptExecutor)driver).ExecuteScript("$('.chosen :nth(1)').attr('selected', true)");
                ((IJavaScriptExecutor)driver).ExecuteScript(" $('.add-button button').click()");
                Thread.Sleep(3000);
                string returnMessage = (string)((IJavaScriptExecutor)driver).ExecuteScript("return  $('.errors div:nth(2)').text()");
                if (returnMessage == "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    success = false;

                    using (var contexts = new dbContext())
                    {

                        Stock.inStock = false;
                        contexts.Stocks.Attach(Stock);
                        var entry = contexts.Entry(Stock);
                        entry.Property(e => e.inStock).IsModified = true;
                        // other changed properties
                        contexts.SaveChanges();

                    }

                }
                if (returnMessage != "We're sorry, the selected item does not have 1 item in stock. Please try again.")
                {
                    success = true;
                }

            }

            return success;
        }


        [HttpGet]
        public IEnumerable<stockModel> GetAllStock()
        {
            using (var context = new dbContext())
            {
                return context.Stocks.ToList();
            }

        }

        [HttpPost]
        public bool enterBillingInformation(BillingViewModel model)
        {
            driver.Navigate().GoToUrl("https://www.usangels.com/Checkout/Info");

            IWebElement BillFirstName = driver.FindElement(By.Id("BillFirstName"));
            BillFirstName.SendKeys(model.firstName);



            IWebElement ShipFirstName = driver.FindElement(By.Id("ShipFirstName"));
            ShipFirstName.SendKeys(model.firstName);

            IWebElement BillLastName = driver.FindElement(By.Id("BillLastName"));
            BillLastName.SendKeys(model.lastName);


            IWebElement ShipLastName = driver.FindElement(By.Id("ShipLastName"));
            ShipLastName.SendKeys(model.lastName);

            

            IWebElement BillAddress = driver.FindElement(By.Id("BillAddress"));
            BillAddress.SendKeys(model.BillingAddress1);

            IWebElement ShipAddress = driver.FindElement(By.Id("ShipAddress"));
            ShipAddress.SendKeys(model.BillingAddress1);

            


            IWebElement BillAddress2 = driver.FindElement(By.Id("BillAddress2"));
            BillAddress2.SendKeys(model.BillingAddress2);


            IWebElement ShipAddress2 = driver.FindElement(By.Id("ShipAddress2"));
            ShipAddress2.SendKeys(model.BillingAddress2);
            

            IWebElement BillCity = driver.FindElement(By.Id("BillCity"));
            BillCity.SendKeys(model.city);




            IWebElement ShipCity = driver.FindElement(By.Id("ShipCity"));
            ShipCity.SendKeys(model.city);

              

            IWebElement BillZipCode = driver.FindElement(By.Id("BillZipCode"));
            BillZipCode.SendKeys(model.zip);


            IWebElement ShipZipCode = driver.FindElement(By.Id("ShipZipCode"));
            ShipZipCode.SendKeys(model.zip);


            IWebElement BillPhone = driver.FindElement(By.Id("BillPhone"));
            BillPhone.SendKeys(model.phone);



            IWebElement ShipPhone = driver.FindElement(By.Id("ShipPhone"));
            ShipPhone.SendKeys(model.phone);

            IWebElement CreditCardNum = driver.FindElement(By.Id("CreditCardNum"));
            CreditCardNum.SendKeys(model.ccNumber);

            IWebElement CreditCardCVV = driver.FindElement(By.Id("CreditCardCVV"));
            CreditCardCVV.SendKeys(model.cvv);

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""option:selected"").removeAttr(""selected"") ");

            ((IJavaScriptExecutor)driver).ExecuteScript(@"$("".col-1 .chosen option[value='" + model.state + @" ']"").attr(""selected"",""selected"")");
            
            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#CreditCardTypeID option[value='" + model.ccType + @"']"").attr(""selected"",""selected"");");
            
            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#CreditCardExpirationMonth option[value='" + model.expirationMonth + @"']"").attr(""selected"",""selected"");");
            
            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#CreditCardExpirationYear option[value='" + model.expirationYear + @"']"").attr(""selected"",""selected"");");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#lst_shipping option[value='" + model.shipping + @"']"").attr(""selected"",""selected"");");

            ((IJavaScriptExecutor)driver).ExecuteScript(@"  $('#chck_same_bill').attr('checked','checked'); ");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $('.btn-pry').click(); ");

            Thread.Sleep(3000);
            ((IJavaScriptExecutor)driver).ExecuteScript(@" $('.checkout').click(); ");
            


            return true;

        }

        public bool enterBillingInformationAsGuest(BillingViewModel model)
        {
            driver.Navigate().GoToUrl("https://www.usangels.com/cart");


            ((IJavaScriptExecutor)driver).ExecuteScript(@" $('.btn-pry').click(); ");
            ((IJavaScriptExecutor)driver).ExecuteScript(@" $('.btn-wdith').click(); ");




            IWebElement EmailAddress = driver.FindElement(By.Id("EmailAddress"));
            EmailAddress.SendKeys("email@email.com");



            IWebElement BillFirstName = driver.FindElement(By.Id("BillFirstName"));
            BillFirstName.SendKeys(model.firstName);



            IWebElement ShipFirstName = driver.FindElement(By.Id("ShipFirstName"));
            ShipFirstName.SendKeys(model.firstName);

            IWebElement BillLastName = driver.FindElement(By.Id("BillLastName"));
            BillLastName.SendKeys(model.lastName);


            IWebElement ShipLastName = driver.FindElement(By.Id("ShipLastName"));
            ShipLastName.SendKeys(model.lastName);



            IWebElement BillAddress = driver.FindElement(By.Id("BillAddress"));
            BillAddress.SendKeys(model.BillingAddress1);

            IWebElement ShipAddress = driver.FindElement(By.Id("ShipAddress"));
            ShipAddress.SendKeys(model.BillingAddress1);




            IWebElement BillAddress2 = driver.FindElement(By.Id("BillAddress2"));
            BillAddress2.SendKeys(model.BillingAddress2);


            IWebElement ShipAddress2 = driver.FindElement(By.Id("ShipAddress2"));
            ShipAddress2.SendKeys(model.BillingAddress2);


            IWebElement BillCity = driver.FindElement(By.Id("BillCity"));
            BillCity.SendKeys(model.city);




            IWebElement ShipCity = driver.FindElement(By.Id("ShipCity"));
            ShipCity.SendKeys(model.city);



            IWebElement BillZipCode = driver.FindElement(By.Id("BillZipCode"));
            BillZipCode.SendKeys(model.zip);


            IWebElement ShipZipCode = driver.FindElement(By.Id("ShipZipCode"));
            ShipZipCode.SendKeys(model.zip);


            IWebElement BillPhone = driver.FindElement(By.Id("BillPhone"));
            BillPhone.SendKeys(model.phone);



            IWebElement ShipPhone = driver.FindElement(By.Id("ShipPhone"));
            ShipPhone.SendKeys(model.phone);

            IWebElement CreditCardNum = driver.FindElement(By.Id("CreditCardNum"));
            CreditCardNum.SendKeys(model.ccNumber);

            IWebElement CreditCardCVV = driver.FindElement(By.Id("CreditCardCVV"));
            CreditCardCVV.SendKeys(model.cvv);

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""option:selected"").removeAttr(""selected"") ");

            ((IJavaScriptExecutor)driver).ExecuteScript(@"$("".col-1 .chosen option[value='" + model.state + @" ']"").attr(""selected"",""selected"")");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#CreditCardTypeID option[value='" + model.ccType + @"']"").attr(""selected"",""selected"");");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#CreditCardExpirationMonth option[value='" + model.expirationMonth + @"']"").attr(""selected"",""selected"");");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#CreditCardExpirationYear option[value='" + model.expirationYear + @"']"").attr(""selected"",""selected"");");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $(""#lst_shipping option[value='" + model.shipping + @"']"").attr(""selected"",""selected"");");

            ((IJavaScriptExecutor)driver).ExecuteScript(@"  $('#chck_same_bill').attr('checked','checked'); ");

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $('.btn-pry').click(); ");

            Thread.Sleep(3000);

            ((IJavaScriptExecutor)driver).ExecuteScript(@" $('.checkout').click(); ");



            return true;

        }
    }
}