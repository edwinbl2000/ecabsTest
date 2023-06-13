using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using OpenQA.Selenium.Interactions;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using Keys = OpenQA.Selenium.Keys;
using Keyboard = System.Windows.Forms.SendKeys;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.Office.Interop.Word;
using System.Xml.Linq;
using System.IO;

namespace APITest
{
    /// <summary>
    /// Clase contains all remastered selenium objects and selenium logic used by tests.
    /// </summary>
    public class APISelenium
    {
        ChromeDriver driver;
        ReportFunctions RF = new ReportFunctions();
        String mainWin;
        String auxModalWin;

        public APISelenium()
        {
        }

        /// <summary>
        /// How to use Selenium, normal or headless
        /// </summary>
        /// <returns></returns> 
        public string modeSelection() {
            return "normal";
        }

        /// <summary>
        /// Webdriver configuration
        /// </summary>
        /// <returns></returns> 
        public void Config(string UrlApp)
        {
            var options = new ChromeOptions();
            if (modeSelection() == "normal")
            {

                options.AddArgument("-no-sandbox");
                driver = new ChromeDriver(Directory.GetCurrentDirectory(), options, TimeSpan.FromSeconds(240));
                SetImplicitTimeoutSeconds(240);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(240);
                driver.Manage().Window.Size = new System.Drawing.Size(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 30, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 30);
                driver.Navigate().GoToUrl(UrlApp);


            }
            else if (modeSelection() == "headless")
            {
                options.AddArguments(new List<string>() {
                                "--headless",
                                "--disable-gpu",
                                "--disable-software-rasterizer",
                                "--log-level=3",
                                "--window-size=1600x900"
                                });

                driver = new ChromeDriver(Directory.GetCurrentDirectory(), options, TimeSpan.FromSeconds(240));
                SetImplicitTimeoutSeconds(240);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(240);
                driver.Navigate().GoToUrl(UrlApp);

                Thread.Sleep(500);

            }
            Thread.Sleep(1500);
        }

        /// <summary>
        /// Register in the application
        /// </summary>
        /// <returns></returns>       
        public int RegisterApp(string App, string User, string Password, string UrlApp, string file, string indindicativenumberPhone, string numberPhone, string name)
        {
            try
            {
                int result = 0;
                ///Choose the application
                if (App.ToLower() == "fastrack")
                {
                    Config(UrlApp);
                    driver.FindElement(By.CssSelector(".register")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".button--intro")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".input")).Click();
                    Thread.Sleep(500);                   
                    driver.FindElement(By.CssSelector(".input")).SendKeys(User);              
                    Thread.Sleep(200);
                    Screenshot("Add_user", true, file);

                    driver.FindElement(By.CssSelector(".input-button")).Click();
                    Thread.Sleep(200);                  
                    driver.FindElement(By.CssSelector(".phone-number-input--small")).SendKeys(indindicativenumberPhone);
                    driver.FindElement(By.CssSelector(".phone-number-input:nth-child(2)")).SendKeys(numberPhone);
                    Thread.Sleep(200);
                    Screenshot("Add_number", true, file);

                    driver.FindElement(By.CssSelector(".input-button")).Click();
                    Thread.Sleep(200);
                    driver.FindElement(By.CssSelector(".input")).Click();
                    Thread.Sleep(200);
                    driver.FindElement(By.CssSelector(".input")).SendKeys(name);
                    Thread.Sleep(200);
                    Screenshot("Add_name", true, file);

                    driver.FindElement(By.CssSelector(".input-button")).Click();
                    Thread.Sleep(400);
                    driver.FindElement(By.CssSelector(".input")).Click();
                    Thread.Sleep(200);
                    driver.FindElement(By.CssSelector(".input")).SendKeys(Password);
                    Thread.Sleep(500);
                    Screenshot("Add_password", true, file);


                    driver.FindElement(By.CssSelector(".input-button")).Click();
                    Thread.Sleep(3000);
                    AcceptAlert();
                    Thread.Sleep(500);
                    IList<IWebElement> elements = driver.FindElements(By.XPath("//h1[contains(.,'Enter contact details')]"));
                    if (elements.Count > 0)
                    {
                        Screenshot("Register_user", true, file);
                        result =elements.Count;
                    }
                    else
                    {
                        result= 0;
                    }                    
                }
                return result;
            }
            catch(Exception ex)
            {
                return -1;
            }
            
           
        }

        /// <summary>
        /// is logged into the application
        /// </summary>
        /// <returns></returns>  
        public int LoginApps(string App, string User, string UrlApp, string file) 
        {
            try
            {
                int result = 0;
                Config(UrlApp);

                ///Choose the application
                if (App.ToLower() == "fastrack")
                {
                    driver.FindElement(By.XPath("//div[2]/button")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".input")).Click();

                    ///Write the email
                    driver.FindElement(By.CssSelector(".input")).SendKeys(User);
                    Thread.Sleep(200);
                    Screenshot("Write_email", true, file);

                    ///Accepts browser alert when error comes out and retries
                    new Actions(driver).DoubleClick(driver.FindElement(By.CssSelector(".input-button"))).Perform();
                    Thread.Sleep(2000);

                    IList<IWebElement> elements = driver.FindElements(By.CssSelector(".money"));

                    ///Find the element that shows the balance in the application
                    while (elements.Count <= 0)
                    {
                        AcceptAlert();
                        driver.FindElement(By.CssSelector(".input-button")).Click();
                        Thread.Sleep(2000);
                        elements = driver.FindElements(By.CssSelector(".money"));
                    }
                    
                    ///returns the 1 if the balance element is
                    if (elements.Count > 0)
                    {
                        Screenshot("Login", true, file);
                        result = elements.Count;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        /// <summary>
        /// Play Casino and Update Balance
        /// </summary>
        /// <returns></returns>
        public int CheckBalance(string App, string file)
        {
            try
            {  
                int result = 0;
                if (App.ToLower() == "fastrack")
                {
                    int valInprev = 0;
                    int valInBef = 0;
                    Thread.Sleep(1000);

                    /// Captures the previous balance and adds 500 to the balance sheet
                    valInprev = captureBalance(".money");
                    valInprev = valInprev + 500;
                    Screenshot("Capture_balance", true, file);

                    driver.FindElement(By.CssSelector(".money")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//div/div[3]/button")).Click();
                    Thread.Sleep(1000);
                    Screenshot("add_method_pay", true, file);

                    driver.FindElement(By.XPath("//button[contains(.,'€500')]")).Click();
                    Thread.Sleep(1000);
                    Screenshot("add_500_Euros_to_balance", true, file);

                    driver.FindElement(By.XPath("//div[3]/div/button")).Click();
                    Thread.Sleep(1000);
                    Screenshot("add_to_balance", true, file);
                    driver.FindElement(By.XPath("//button[contains(.,'OK')]")).Click();
                    Thread.Sleep(1000);

                    /// Captures the after balance
                    valInBef = captureBalance(".money");
                    Screenshot("capture_new_balance", true, file);

                    /// The previous balance plus 500 must be equal to the later balance
                    if (valInprev == valInBef)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        /// <summary>
        /// Play Casino and Update Balance
        /// </summary>
        /// <returns></returns>
        public int PlayGameUpdateBalance(string App, string file)
        {
            try
            {
                int result = 0;
                if (App.ToLower() == "fastrack")
                {

                    driver.FindElement(By.CssSelector(".svg-menu")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//a[contains(@href, '/casino')]")).Click();
                    Thread.Sleep(1000);

                    int valInprev = 0;
                    int valInBef = 0;
                    Thread.Sleep(1000);

                    /// Captures the previous balance
                    valInprev = captureBalance(".money");
                    Screenshot("capture_pre_balance", true, file);

                    driver.FindElement(By.XPath("//div/img")).Click();
                    Thread.Sleep(1000);
                    Screenshot("play_game", true, file);

                    /// Captures the after balance
                    valInBef = captureBalance(".money");

                    Screenshot("capture_befor_balance", true, file);
                    /// validate update balance
                    if (valInprev != valInBef)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        /// <summary>
        /// Buy the lottery and update the balance
        /// </summary>
        /// <returns></returns>
        public int BuyLoteryUpdateBalance(string App, string file)
        {
            try
            {
                int result = 0;
                if (App.ToLower() == "fastrack")
                {

                    driver.FindElement(By.CssSelector(".svg-menu")).Click();
                    Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//a[contains(@href, '/lottery')]")).Click();
                    Thread.Sleep(1000);

                    /// Lower the scrol until you find control
                    Screenshot("capture_lotery", true, file);
                    IWebElement ele = driver.FindElement(By.CssSelector(".button--secondary"));
                    if(ele != null)
                    {
                        Scroll(".button--secondary");
                    }
                    int valInprev = 0;
                    int valInBef = 0;
                    Thread.Sleep(1000);
                    Screenshot("capture_prev_balance", true, file);

                    /// Captures the previous balance
                    valInprev =captureBalance(".money");

                    driver.FindElement(By.CssSelector(".button--secondary")).Click();

                    Thread.Sleep(1000);
                    /// Captures the after balance
                    valInBef = captureBalance(".money");
                    Screenshot("capture_befor_balance", true, file);
                    if (valInprev != valInBef)
                    {
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        /// <summary>
        /// Capture the balance of the cacino
        /// </summary>
        /// <returns></returns>
        public int captureBalance(string cssSelector)
        {
            try
            {
                int balance= 0;
                IList<IWebElement> elementsprev = driver.FindElements(By.CssSelector(cssSelector));
                if (elementsprev.Count > 0)
                {
                    foreach (var el in elementsprev)
                    {
                        string val = el.Text;
                        val = val.Replace("€", "");
                        string[] spltval = val.Split('.');
                        balance = int.Parse(spltval[0]);
                    }

                }
                
                return balance;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        /// <summary>
        /// scrolls until you find control
        /// </summary>
        /// <returns></returns>
        public void Screenshot(string caseName, bool tag, string file)
        {      
            Screenshot image = ((ITakesScreenshot)driver).GetScreenshot();
            string name = caseName;
            string path = Directory.GetCurrentDirectory() + "\\" + caseName + ".bmp";
            image.SaveAsFile(string.Format(Directory.GetCurrentDirectory() + "\\{0}.bmp", name), ScreenshotImageFormat.Bmp);
            ReportFunctions.InsertAPicture(file, path, caseName, tag);
        }

        /// <summary>
        /// Accept browser alerts
        /// </summary>
        /// <returns></returns>
        public void AcceptAlert()
        {
            Thread.Sleep(5000);
            Keyboard.SendWait("{ENTER}");
            Thread.Sleep(5000);
        }

        /// <summary>
        /// scrolls until you find control
        /// </summary>
        /// <returns></returns>
        public void Scroll(string control)
        {
            IWebElement endScroll = driver.FindElement(By.CssSelector(control));
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            js.ExecuteScript("arguments[0].scrollIntoView(true);", endScroll);
        }

        /// <summary>
        /// Puts an implied time in seconds
        /// </summary>
        /// <returns></returns>
        public void SetImplicitTimeoutSeconds(int seconds)
        {
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Close the driver from memory
        /// </summary>
        /// <returns></returns>
        public void Close() 
        {
            driver.Close();
        }

        /// <summary>
        /// Free the driver from memory
        /// </summary>
        /// <returns></returns>
        public void Dispose() 
        {
            driver.Dispose();
        }

        /// <summary>
        /// Quit the driver from memory
        /// </summary>
        /// <returns></returns>
        public void Quit()
        {
            driver.Quit();
        }

        public ChromeDriver returnDriver()
        {
            return driver;
        }          
    }
}
