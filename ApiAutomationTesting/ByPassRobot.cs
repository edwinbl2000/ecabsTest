using System;
using System.Globalization;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.WebDriver.UndetectedChromeDriver;
using Sl.Selenium.Extensions.Chrome;


namespace APITest
{
    /// <summary>
    /// Class containing the setting so that the chromedriver is not detected by applications.
    /// </summary>
    public class ByPassRobot
    {
        /// <summary>
        /// method containing the setting so that the chromedriver is not detected by applications.
        /// </summary>
        static public int bypassconfig(string url)
        {
            try
            {
                int ret = 0;
                /// parameter that disables robot detection
                UndetectedChromeDriver.ENABLE_PATCHER = true;

                var aprams = new ChromeDriverParameters()
                {
                    Timeout = TimeSpan.FromSeconds(500)
                };

                using (var driver = UndetectedChromeDriver.Instance(aprams))
                {
                    /// driver configuration
                    driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(12);
                    driver.GoTo("https://www.expedia.com/");
                    driver.FindElement(By.CssSelector(".uitk-tab:nth-child(2) .uitk-tab-text")).Click();
                    Thread.Sleep(4000);

                    /// navigating to flight options
                    driver.SwitchTo();
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                    var myElement = wait.Until(x => x.FindElement(By.CssSelector(".uitk-tab-button:nth-child(3) .uitk-tab-text")));
                    if (myElement.Displayed)
                    {
                        driver.FindElement(By.CssSelector(".uitk-tab-button:nth-child(3) .uitk-tab-text")).Click();

                    }

                    /// choosing four people
                    Thread.Sleep(1000);
                    Thread.Sleep(1000);
                    driver.FindElement(By.CssSelector(".uitk-link-no-wrap")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".uitk-spacing:nth-child(1) > .uitk-layout-flex > .uitk-layout-flex > .uitk-layout-flex-item:nth-child(3) .uitk-icon")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".uitk-link-no-wrap")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".uitk-spacing:nth-child(1) > .uitk-layout-flex > .uitk-layout-flex > .uitk-layout-flex-item:nth-child(3) .uitk-icon")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".uitk-link-no-wrap")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector(".uitk-spacing:nth-child(1) > .uitk-layout-flex > .uitk-layout-flex > .uitk-layout-flex-item:nth-child(3) .uitk-icon")).Click();
                    Thread.Sleep(500);

                    /// choosing initial destination
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector("#location-field-leg1-origin-menu .uitk-fake-input")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.Id("location-field-leg1-origin")).SendKeys("BOGOTA");
                    Thread.Sleep(1000);
                    driver.FindElement(By.CssSelector(".uitk-action-list-item:nth-child(1) .uitk-button")).Click();
                    Thread.Sleep(1000);

                    /// choosing intermediate destination
                    Thread.Sleep(500);
                    driver.FindElement(By.CssSelector("#location-field-leg1-destination-menu .uitk-fake-input")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.Id("location-field-leg1-destination")).SendKeys("PARIS");
                    Thread.Sleep(1000);
                    driver.FindElement(By.CssSelector(".uitk-action-list-item:nth-child(1) .uitk-button")).Click();
                    Thread.Sleep(1000);

                    
                    driver.FindElement(By.Id("d1-btn")).Click();
                    Thread.Sleep(2000);
                    driver.FindElement(By.CssSelector(".uitk-date-picker-month:nth-child(1) tr:nth-child(4) > .uitk-date-picker-day-number:nth-child(2) > .uitk-date-picker-day")).Click();
                    Thread.Sleep(2000);
                    driver.FindElement(By.CssSelector(".uitk-layout-flex-item-flex-shrink-0 > .uitk-button")).Click();
                    Thread.Sleep(500);

                    /// choosing start date
                    driver.FindElement(By.CssSelector("#location-field-leg2-destination-menu .uitk-fake-input")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.Id("location-field-leg2-destination")).SendKeys("BOGOTA");
                    Thread.Sleep(1000);
                    driver.FindElement(By.CssSelector(".uitk-action-list-item:nth-child(1) .uitk-button")).Click();
                    Thread.Sleep(1000);

                    driver.FindElement(By.CssSelector(".uitk-layout-grid-item-has-column-start-by-large > .uitk-button")).Click();
                    Thread.Sleep(13000);

                    /// completing reservation
                    driver.FindElement(By.XPath("(//button[@type='button'])[26]")).Click();
                    Thread.Sleep(3000);
                    driver.FindElement(By.CssSelector(".uitk-layout-flex:nth-child(1) > .uitk-card .uitk-button")).Click();
                    Thread.Sleep(13000);
                    driver.FindElement(By.XPath("(//button[@type='button'])[26]")).Click();
                    Thread.Sleep(8000);
                    driver.FindElement(By.CssSelector(".uitk-layout-flex:nth-child(1) > .uitk-card .uitk-button")).Click();
                    Thread.Sleep(20000);

                    /// capturing value of one passenger and the total of all passengers
                    driver.SwitchTo().Window(driver.WindowHandles[1]);
                    string travel1 = driver.FindElement(By.CssSelector(".uitk-table-row:nth-child(1) .uitk-text .uitk-text")).Text;
                    string traveltotal = driver.FindElement(By.CssSelector(".uitk-table-cell-align-trailing .uitk-heading")).Text;

                    travel1 = travel1.Replace("$", "");
                    traveltotal = traveltotal.Replace("$", "");
                    decimal dtravel1 = Convert.ToDecimal(travel1, CultureInfo.GetCultureInfo("en-US"));
                    decimal dtraveltotal = Convert.ToDecimal(traveltotal, CultureInfo.GetCultureInfo("en-US"));

                    /// multiplying the value of one passenger by four
                    decimal travelTest = dtravel1 * 4;

                    /// by comparing the total value of the site by the calculated
                    if (travelTest == dtraveltotal)
                    {
                        ret = 1;
                    }

                    return ret;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
           
        }
            
    }
}
