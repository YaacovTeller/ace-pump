using System;
using System.Web.Hosting;
using AcePump.Common;

namespace AcePump.WebApi
{
    public class VirtualPathMapper : IVirtualPathMapper
    {

        private static Lazy<VirtualPathMapper> Ctor = new Lazy<VirtualPathMapper>(()  => new VirtualPathMapper());

        public static VirtualPathMapper Instance
        {
            get
            {
                return Ctor.Value;
            }
        }

        private VirtualPathMapper()
        {
        }

        public string MapPath(string appRelativePath)
        {
            return HostingEnvironment.MapPath(appRelativePath);
        }
    }
}