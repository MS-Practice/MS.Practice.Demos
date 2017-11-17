using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Owin2.ResourceServer.Controllers
{
    [Authorize]
    public class MeController : ApiController
    {
        // GET: Me
        public IEnumerable<object> Get()
        {
            var identity = User.Identity as ClaimsIdentity;
            return identity.Claims.Select(c => new
            {
                Type = c.Type,
                Value = c.Value
            });
        }
    }
}