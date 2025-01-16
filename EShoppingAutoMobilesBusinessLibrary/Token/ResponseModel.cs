using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EShoppingAutoMobilesBusinessLibrary.Token
{
    public class ResponseModel<T>
    {
        public ResponseModel()
        {
            IsSuccess = true;
            Message = "";
        }

        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
