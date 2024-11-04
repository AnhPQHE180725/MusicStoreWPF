using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public partial class MusicStoreContext : DbContext
    {
        public  DbSet<User> Users { get; set; }
        public  DbSet<Roles> Roles { get; set; }
        public  DbSet<Songs> Songs { get; set; }
        public  DbSet<Orders> Orders { get; set; }
        public  DbSet<OrderDetails> OrderDetails { get; set; }
        public  DbSet<Memberships> Memberships { get; set; }
        public  DbSet<Genres> Genres { get; set; }
        public  DbSet<Artists> Artists { get; set; }
        public  DbSet<Albums> Albums { get; set; }

        public MusicStoreContext() { }
        public MusicStoreContext(DbContextOptions<MusicStoreContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
                                          .SetBasePath(Directory.GetCurrentDirectory())
                                          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionString"));
        }


    }

}
