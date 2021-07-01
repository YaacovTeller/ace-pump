using System;
using AcePump.Common.Storage;
using AcePump.Domain.DataSource;

namespace AcePump.WebApi.Startup
{
    public class TenantContext : IDisposable
    {
        public AcePumpContext Db { get; set; }
        public IStorageProvider StorageProvider { get; set; }

        public void Dispose()
        {
            if(Db != null)
            {
                Db.Dispose();
            }
        }
    }
}