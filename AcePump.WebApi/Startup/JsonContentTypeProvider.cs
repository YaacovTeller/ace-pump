using Microsoft.Owin.StaticFiles.ContentTypes;

namespace AcePump.WebApi.Startup
{
    public class JsonContentTypeProvider : FileExtensionContentTypeProvider
    {
        public JsonContentTypeProvider()
        {
            Mappings.Add(".json", "application/json");
        }
    }
}