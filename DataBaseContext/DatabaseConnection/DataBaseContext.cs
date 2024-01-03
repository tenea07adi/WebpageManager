using DataModels.DatabaseModels.Webpage;
using DataModels.DatabaseModels.WebpageSimpleInfo;
using DataModels.StorageModels.Auth;
using Microsoft.EntityFrameworkCore;

namespace DatabaseLayer.DatabaseConnection
{
    public class DataBaseContext : DbContext
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<AuthenticationToken> AuthenticationTokens { get; set; }
        public DbSet<RegistrationInvite> RegistrationInvites { get; set; }

        public DbSet<Webpage> Webpages { get; set; }
        public DbSet<WebpageInfoCollection> WebpageInfoCollections { get; set; }
        public DbSet<WebpageInfo> WebpageInfos { get; set; }

    }
}
