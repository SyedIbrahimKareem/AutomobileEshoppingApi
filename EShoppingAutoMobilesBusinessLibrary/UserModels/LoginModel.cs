using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EShoppingBusinessLibrary.UserModels
{
    public class LoginModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
