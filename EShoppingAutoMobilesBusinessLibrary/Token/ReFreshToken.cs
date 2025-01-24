using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppingBusinessLibrary.Token
{
    public class ReFreshToken
    {
        public int RefreshtokenId { get; set; }
        public string Token {  get; set; }
        public string JwtId {  get; set; }
        public int UserId {  get; set; }
        public DateTime CreationDate {  get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
