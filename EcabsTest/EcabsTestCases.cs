using Microsoft.VisualStudio.TestTools.UnitTesting;
using APITest;
using static System.Net.WebRequestMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;


namespace EcabsTest
{
    /// <summary>
    /// class containing the automated use cases of expedia and automationexercise
    /// </summary>
    [TestClass]
    public class EcabsTestCases
    {
        APISelenium selenium = new APISelenium();
        ReportFunctions RP = new ReportFunctions();

        /// <summary>
        /// data for the authentication test on the automationexercise platform
        /// </summary>
        public static IEnumerable<object[]> AdditionDataRegister
        {
            get
            {
                return new[]
                {
                    new object[] { "automationexercise", "https://automationexercise.com/login", "",""},
                    new object[] { "automationexercise", "https://automationexercise.com/login", "","1234567"},
                    new object[] { "automationexercise", "https://automationexercise.com/login", "egbarreto00000501@gmail.com",""},
                    new object[] { "automationexercise", "https://automationexercise.com/login", "egbarreto00000501@gmail.com","123457"}
                };
            }
        }

        /// <summary>
        /// data for the ticket booking test on the expedia platform
        /// </summary>
        public static IEnumerable<object[]> AdditionDataLogin
        {
            get
            {
                return new[]
                {
                    new object[] { "expedia", "https://www.expedia.com/", "egbarreto00000501@gmail.com" }
                };
            }
        }


    
        ///  /// <summary>
        /// use case for automationexercise page login test
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(AdditionDataRegister))]
        public void Task_2_User_Register_automationexercise(string app, string url, string user, string pass)
        {
            int asserRegister = 0;
            string file = RP.CreteDocumentWordDinamic("A_User_Register");

            asserRegister =selenium.RegisterApp(app, user, pass, url, file);

            Assert.IsTrue(asserRegister > 0);
    
        }

        /// <summary>
        /// use case for flight search on expedia website
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(AdditionDataLogin))]
        public void Task_3_Search_flight_expedia(string app, string url, string user)
        {
            Random rnd = new Random();
            int asserLogin = 0;         
            string file = RP.CreteDocumentWordDinamic("B_User_LoginAcces");

            asserLogin = selenium.searchFlight(app, user, url, file);

            Assert.IsTrue(asserLogin > 0);
        }

        /// <summary>
        /// clean the test
        /// </summary>
        [TestCleanup]
        public void Clean()
        {
            selenium.Close();
            selenium.Quit();
            selenium.Dispose();
            RP.CleanProcess();

            DirectoryInfo di = new DirectoryInfo(Directory.GetCurrentDirectory());
            int numfiles = di.GetFiles("*.png", SearchOption.AllDirectories).Length;

            if (numfiles > 0)
            {
                foreach (FileInfo file in di.GetFiles("*.png", SearchOption.AllDirectories))
                {
                    file.Delete();
                }
            }

        }
    }
}
