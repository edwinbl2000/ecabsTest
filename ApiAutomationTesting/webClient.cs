using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ApiAutomationTesting
{
    /// <summary>
    /// the client web class provides the necessary elements for the connection, 
    /// conversion and response analysis of the api rest
    /// </summary>
    public class webClient
    {

        /// <summary>
        /// method used to test user registration and deregistration in api rest
        /// </summary>
        public string RegisterUserApi(string urls, string usr = null, string pass = null)
        {
            try
            {
                string html;

                var reqObject = new UsersRequestObject();
                reqObject.email = usr;
                reqObject.password = pass;

                ///web client
                string request = JsonConvert.SerializeObject(reqObject);
                string url = urls;
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json";
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(request);
                }
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }

                ///valid that the service responds within 200 OK
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var apiResponse = JsonConvert.DeserializeObject<UsersResponseRegisterObject>(html);
                    if (apiResponse.Token.Length > 14)
                    {
                        ///return the generated token
                        return apiResponse.Token;
                    }
                    else
                    {
                        return string.Empty;
                    }

                }
                else
                {
                    return string.Empty;
                }

            }
            catch (Exception ex)
            {

                if (ex.ToString().IndexOf("400") != -1)
                {
                    ///if you can't log in return a fake token
                    return "00000000000000000";
                }
                return string.Empty;
            }

        }

        /// <summary>
        /// method used to read api rest users
        /// </summary>
        public int getUserApi(string urls)
        {
            try
            {
                string html;
                string url = urls;

                ///web client
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                }
                
                dynamic json = JsonConvert.DeserializeObject(html);

                List<userss> usr = new List<userss>();

                ///read json
                foreach (var user in json["data"])
                {
                    userss users = new userss();

                    users.id = user["id"];
                    users.email = user["email"];
                    users.first_name = user["first_name"];
                    users.last_name = user["last_name"];
                    users.avatar = user["avatar"];

                    usr.Add(users);

                }

                ///return user number
                return usr.Count;

            }
            catch (Exception ex)
            {
                return -1;
            }

        }
    }
}
