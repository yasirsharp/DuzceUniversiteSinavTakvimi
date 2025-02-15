using Core.Entities.Concrete;
using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Concrete.EntityFramework
{
    public class DuzceUniversiteContext : DbContext
    {
        public DbSet<Bolum> Bolum { get; set; }
        public DbSet<Ders> Ders { get; set; }
        public DbSet<AkademikPersonel> AkademikPersonel { get; set; }
        public DbSet<Derslik> Derslik { get; set; }
        public DbSet<DersBolumAkademikPersonel> Ders_Bolum_AkademikPersonel { get; set; }
        public DbSet<SinavDetay> SinavDetay { get; set; }
        public DbSet<SinavDerslik> SinavDerslik { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=94.73.170.5;Database=u5627450_takvim;User Id=u5627450_takvim;Password=Yasir#Demirci0308;Trust Server Certificate=True;");
            //optionsBuilder.UseSqlServer(@"Server=94.73.170.5;Integrated Security=True;Database=u5627450_takvim;User Id=u5627450_takvim;Password=Yasir#Demirci0308;Trust Server Certificate=True;");
            //optionsBuilder.UseSqlServer(@"Server=YASIR\SQLEXPRESS;Integrated Security=True; Database=DuzceUniversite; Trust Server Certificate=True; Trusted_Connection=true");
            //Data Source=;Connect Timeout=30;Encrypt=True;Application Intent=ReadWrite;Multi Subnet Failover=False
            //optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DuzceUniversite;Trusted_Connection=true");
        }
    }
} 