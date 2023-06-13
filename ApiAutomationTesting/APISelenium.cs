using System;
using System.Collections.Generic;
using OpenQA.Selenium.Chrome;
using System.Threading;
using OpenQA.Selenium;
using Keyboard = System.Windows.Forms.SendKeys;
using System.IO;

namespace APITest
{
        
    /// <summary>
    /// Class contains all remastered selenium objects and selenium logic used by tests.
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

                options.AddArguments(new List<string>() {

                                "--incognito",
                                "--window-size=1280x800"
                                });
                driver = new ChromeDriver(Directory.GetCurrentDirectory(), options, TimeSpan.FromSeconds(240));
                SetImplicitTimeoutSeconds(240);
                driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(240);
                driver.Manage().Window.Size = new System.Drawing.Size(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width - 30, System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height - 30);
                driver.Navigate().GoToUrl(UrlApp);


            }
            else if (modeSelection() == "headless")
            {
                options.AddArguments(new List<string>() {
                                
                                "--disable-gpu",
                                "--disable-software-rasterizer",
                                "--log-level=3",
                                "--window-size=1280x800"
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
        public int RegisterApp(string App, string User, string Password, string UrlApp, string file)
        {
            try
            {
                int result = 0;
                ///Choose the application
                if (App.ToLower() == "automationexercise")
                {
                    ///driver configuration
                    Config(UrlApp);
                    driver.FindElement(By.XPath("//input[@name='email']")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.XPath("//input[@name='email']")).SendKeys(User);              
                    Thread.Sleep(200);
                    ///take a photo by the report
                    Screenshot("Add_user", true, file);
               
                    driver.FindElement(By.XPath("//input[@name='password']")).Click();
                    Thread.Sleep(500);
                    driver.FindElement(By.XPath("//input[@name='password']")).SendKeys(Password);
                    Thread.Sleep(200);
                    Screenshot("Add_password", true, file);

                    driver.FindElement(By.XPath("//button[@type='submit']")).Click();

                    String message = string.Empty;

                    if (User == string.Empty && Password == string.Empty)
                    {
                        ///read the attribute validation messages
                        message = driver.FindElement(By.XPath("//input[@name='email']")).GetAttribute("validationMessage");
                        if (message.Length > 0)
                        {
                            result = 1;
                        }
                    }

                    else if (User != string.Empty && Password == string.Empty)
                    {
                        ///read the attribute validation messages
                        message = driver.FindElement(By.XPath("//input[@name='password']")).GetAttribute("validationMessage");
                        if (message.Length > 0)
                        {
                            result = 1;
                        }

                    }
                    else if (User == string.Empty && Password != string.Empty)
                    {
                        ///read the attribute validation messages
                        message = driver.FindElement(By.XPath("//input[@name='email']")).GetAttribute("validationMessage");
                        if (message.Length > 0)
                        {
                            result = 1;
                        }

                    }
                    else if (User != string.Empty && Password != string.Empty)
                    {
                        ///read the attribute validation messages
                        IList<IWebElement> elements = driver.FindElements(By.XPath("//p[contains(.,'Your email or password is incorrect!')]"));
                        if (elements.Count > 0)
                        {
                            Screenshot("Register_user_incorrect_1", true, file);
                            result = 1;
                        }

                    }

                    Thread.Sleep(2000);                  
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
        public int searchFlight(string App, string User, string UrlApp, string file) 
        {
            try
            {
                int result = 0;
                ///Choose the application
                if (App.ToLower() == "expedia")
                {
                    ///auxiliary class with a driver that avoids robot detection of applications 
                    int driver =ByPassRobot.bypassconfig(UrlApp);

                    return driver;
                }
                return result;
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
            if (driver != null)
            {
                driver.Close();
            }
          
        }

        /// <summary>
        /// Free the driver from memory
        /// </summary>
        /// <returns></returns>
        public void Dispose() 
        {
            if (driver != null)
            {
                driver.Dispose();
            }
        }

        /// <summary>
        /// Quit the driver from memory
        /// </summary>
        /// <returns></returns>
        public void Quit()
        {
            if (driver != null)
            {
                driver.Quit();
            }
        }

        public ChromeDriver returnDriver()
        {
            return driver;
        }          
    }
}
