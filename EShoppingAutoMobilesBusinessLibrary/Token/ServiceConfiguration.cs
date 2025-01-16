using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShoppingAutoMobilesBusinessLibrary.Token
{
    public class ServiceConfiguration
    {
        public JwtSettings JwtSettings { get; set; }
    }

    public class JwtSettings
    {
        public string Secret { get; set; }

        public TimeSpan TokenLifetime { get; set; }
    }
}
