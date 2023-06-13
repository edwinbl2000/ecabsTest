using Microsoft.VisualStudio.TestTools.UnitTesting;
using APITest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using ApiAutomationTesting;

namespace EcabsTest
{
    /// <summary>
    /// All tests in this section are made with the API provided at https://reqres.in/. 
    /// All requests andrequests and responses are in JSON
    /// </summary>
    [TestClass]
    public class EcabsApiUnitTest
    {

        webClient web = new webClient();

        /// <summary>
        /// user registration data
        /// </summary>
        public static IEnumerable<object[]> apiRegisterUsers
        {
            get
            {
                return new[]
                {
                    new object[] { "reqres", "https://reqres.in/api/register", "eve.holt@reqres.in", "pistol"},
                    new object[] { "reqres", "https://reqres.in/api/register", "egbarreto2001@gmail.com", "pistol"},
                    new object[] { "reqres",  "https://reqres.in/api/register", "egbarreto2002@gmail.com", "pistol"},
                    new object[] { "reqres",  "https://reqres.in/api/register", "egbarreto2003@gmail.com", "pistol" }
                };
            }
        }

        /// <summary>
        /// user unregistration data
        /// </summary>
        public static IEnumerable<object[]> apiUnseRegisterUsers
        {
            get
            {
                return new[]
                {
                    new object[] { "reqres", "https://reqres.in/api/register", "eve.holt@reqres.in", ""},
                    new object[] { "reqres", "https://reqres.in/api/register", "egbarreto2001@gmail.com", ""},
                    new object[] { "reqres",  "https://reqres.in/api/register", "egbarreto2002@gmail.com", ""},
                    new object[] { "reqres",  "https://reqres.in/api/register", "egbarreto2003@gmail.com", "" }
                };
            }
        }

        /// <summary>
        /// get user data
        /// </summary>
        public static IEnumerable<object[]> apiGetUsers
        {
            get
            {
                return new[]
                {
                    new object[] { "reqres", "https://reqres.in/api/register"}
                    
                };
            }
        }

        /// <summary>
        /// method to verify user registration in the api
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(apiRegisterUsers))]
        public void task_1_api_successful_register_users(string app, string url, string user, string pass)
        {
            string asserRegister = string.Empty;

            asserRegister = web.RegisterUserApi(url, user, pass);

            Assert.AreEqual(17, asserRegister.Length, "the user did not register correctly");
        }

        /// <summary>
        /// method to verify user unregistration in the api
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(apiRegisterUsers))]
        public void task_1_api_unsuccessful_register_users(string app, string url, string user, string pass)
        {
            string asserRegister = string.Empty;

            asserRegister = web.RegisterUserApi(url, user, pass);

            Assert.AreEqual(17, asserRegister.Length, "the user did not register correctly");
        }

        /// <summary>
        /// method for verifying the list of api users
        /// </summary>
        [TestMethod]
        [DynamicData(nameof(apiGetUsers))]
        public void task_1_api_get_list_users(string app, string url)
        {
            int asserRegister = 0;

            asserRegister = web.getUserApi(url);

            Assert.IsTrue(asserRegister >0);

        }
       
    }
}
