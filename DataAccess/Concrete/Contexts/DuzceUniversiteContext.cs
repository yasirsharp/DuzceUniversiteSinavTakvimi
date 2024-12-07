using Entity.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.Contexts
{
    public class DuzceUniversiteContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=YASIR\SQLEXPRESS;Database=DuzceUniversite;Trusted_Connection=true;TrustServerCertificate=True");
        }

        public DbSet<Bolum> Bolum { get; set; }
        public DbSet<Ders> Ders { get; set; }
        public DbSet<AkademikPersonel> AkademikPersonel { get; set; }
        public DbSet<Derslik> Derslik { get; set; }
        public DbSet<DersBolumAkademikPersonel> DersBolumAkademikPersonel { get; set; }
        public DbSet<SinavDetay> SinavDetay { get; set; }

    }
}
