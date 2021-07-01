using System.Net.Http;
using System.Web;
using System.Web.Http;
using AcePump.Domain.DataSource;
using AcePump.WebApi.Startup;
using Microsoft.AspNet.Identity.Owin;

namespace AcePump.WebApi.Controllers
{
    [Authorize]
    public class BaseApiController : ApiController
    {
        protected TenantContext TenantContext
        {
            get
            {
                return Request.GetOwinContext().Get<TenantContext>();
            }
        }

        protected AcePumpContext Db
        {
            get
            {
                return TenantContext.Db;
            }
        }
    }
}