using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Panda.Core
{
    public class Schema : DbContext
    {
        public DbSet<Promoter> promoters { get; set; }
        public DbSet<PromoCode> promocodes { get; set; }
        public DbSet<Member> members { get; set; }
        public DbSet<SMSLog> smslogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionstr = ZC.Cfg.GetCfg("ConnectionString");
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(connectionstr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>().HasIndex(c => c.Contact).IsUnique();
            modelBuilder.Entity<Promoter>().HasIndex(c => c.Username).IsUnique();
            modelBuilder.Entity<PromoCode>().HasIndex(c => c.Code).IsUnique();
        }
    }
}
