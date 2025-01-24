using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppingBusinessLibrary.UserModels
{
    public class RoleMaster
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedOn {  get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
