using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EShoppingBusinessLibrary.Error
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        //public override string ToString()
        //{
        //    return JsonSerializer.Serialize(this);
        //}
    }
}
