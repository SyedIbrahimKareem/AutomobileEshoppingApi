using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace EShoppingBusinessLibrary.UserModels
{
    public class UserMaster
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserEmail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int UserPurchaseHistoryId { get; set; }
        public int UserCartHistoryId { get; set; }
        public string CreatedOn { get; set; }
        public string UpdatedOn { get; set; }
    }
    public class UserRegisteration
    {
        [Key]
        public int UserId { get; set; }
        public string userEmail { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
    }
}
