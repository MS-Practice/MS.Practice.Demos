using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Test._360doo.com.ServiceReference1;

namespace Test._360doo.com
{
    internal class AccessToken
    {
        public static readonly string access_token = string.Empty;
        static AccessToken() {
            PAHaoCheTokenServiceSoapClient service = new PAHaoCheTokenServiceSoapClient();
            string name = "";
            access_token = service.RequestAccessToken(out name);
        }
    }
}
