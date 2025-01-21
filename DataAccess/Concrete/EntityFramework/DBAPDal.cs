using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.Contexts;
using Entity.Concrete;
using Entity.DTOs;

namespace DataAccess.Concrete.EntityFramework
{
    public class DBAPDal : EfEntityRepositoryBase<DersBolumAkademikPersonel, DuzceUniversiteContext>, IDBAPDal
    {
        public DersBolumAkademikPersonelDTO GetDetail(int dbapId)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from dba in context.Ders_Bolum_AkademikPersonel
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             where dba.Id == dbapId
                             select new DersBolumAkademikPersonelDTO
                             {
                                 Id = dba.Id,
                                 DersId = d.Id,
                                 DersAd = d.Ad,
                                 BolumId = b.Id,
                                 BolumAd = b.Ad,
                                 AkademikPersonelId = ap.Id,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan
                             };
                return result.FirstOrDefault();
            }
        }

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
                                 DersId = d.Id,
                                 DersAd = d.Ad,
                                 BolumId = b.Id,
                                 BolumAd = b.Ad,
                                 AkademikPersonelId = ap.Id,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan
                             };
                return result.ToList();
            }
        }
    }
}
