using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using Entity.Concrete;
using Entity.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class DBAPDal : EfEntityRepositoryBase<DersBolumAkademikPersonel, DuzceUniversiteContext>, IDBAPDal
    {
        public List<DersBolumAkademikPersonelDTO> GetDetails()
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from dba in context.Ders_Bolum_AkademikPersonel
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             select new DersBolumAkademikPersonelDTO
                             {
                                 Id = dba.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan
                             };
                return result.ToList();
            }
        }
    }
}
