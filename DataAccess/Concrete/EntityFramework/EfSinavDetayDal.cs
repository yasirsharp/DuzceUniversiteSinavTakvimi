using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfSinavDetayDal : EfEntityRepositoryBase<SinavDetay, DuzceUniversiteContext>, ISinavDetayDal
    {
        public List<SinavDetayDTO> GetByBolumId(int bolumId)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DBAPId equals dba.Id
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId
                             join dlik in context.Derslik on sd.DerslikId equals dlik.Id
                             where b.Id == bolumId
                             select new SinavDetayDTO
                             {
                                 Id = dba.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan,
                                 SinavTarihi = s.SinavTarihi,
                                 SinavSaati = s.SinavSaati,
                                 DerslikId = sd.DerslikId,
                                 DerslikKontenjan = dlik.Kapasite,
                                 GozetmenId = sd.GozetmenId
                             };

                return result.ToList();
            }
        }

        public SinavDetayDTO GetDetail(int sinavDetayId)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DBAPId equals dba.Id
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId
                             where s.Id == sinavDetayId
                             select new SinavDetayDTO
                             {
                                 Id = dba.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 DerslikId = sd.DerslikId,
                                 GozetmenId = sd.GozetmenId,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan,
                                 SinavTarihi = s.SinavTarihi,
                                 SinavSaati = s.SinavSaati,
                             };

                return result.FirstOrDefault()!;
            }
        }

        public List<SinavDetayDTO> GetDetails()
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DBAPId equals dba.Id
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId
                             join dlik in context.Derslik on sd.DerslikId equals dlik.Id
                             select new SinavDetayDTO
                             {
                                 Id = dba.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan,
                                 SinavTarihi = s.SinavTarihi,
                                 SinavSaati = s.SinavSaati,
                                 DerslikId = sd.DerslikId,
                                 DerslikKontenjan = dlik.Kapasite,
                                 GozetmenId = sd.GozetmenId
                             };

                return result.ToList();
            }
        }
    }
}
