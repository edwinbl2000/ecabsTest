using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAutomationTesting
{
        public class SingleUserResponseObject
        {
            public int page { get; set; }
            public int per_page { get; set; }
            public int total { get; set; }
            public int total_pages { get; set; }
            public Data data { get; set; }
            public support support { get; set; }
        }

        public class Data
        {
            public int id { get; set; }
            public string email { get; set; }
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string avatar { get; set; }
        }

        public class support
        {
            public string url { get; set; }
            public string text { get; set; }
        }

        public class UsersRequestObject
        {
            public string email { get; set; }
            public string password { get; set; }
        }


        public class UsersResponseObject
        {
            public string name { get; set; }
            public string job { get; set; }
            public string id { get; set; }
            public DateTime createdAt { get; set; }
        }


        public class UsersResponseRegisterObject
        {
            public string id { get; set; }
            public string Token { get; set; }
        }

        public class userss
        {


            public int id { get; set; }

            public string email { get; set; }

            public string first_name { get; set; }

            public string last_name { get; set; }

            public string avatar { get; set; }
        }
}
