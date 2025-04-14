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
        public void AddWithTransaction(SinavKayitDTO sinavKayitDTO)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        SinavDetay sinavDetay = new SinavDetay
                        {
                            DerBolumAkademikPersonelId = sinavKayitDTO.DerBolumAkademikPersonelId,
                            SinavTarihi = sinavKayitDTO.SinavTarihi,
                            SinavBaslangicSaati = sinavKayitDTO.SinavBaslangicSaati,
                            SinavBitisSaati = sinavKayitDTO.SinavBitisSaati
                        };
                        context.SinavDetay.Add(sinavDetay);
                        context.SaveChanges();

                        foreach (var derslik in sinavKayitDTO.Derslikler)
                        {
                            SinavDerslik sinavDerslik = new SinavDerslik
                            {
                                SinavDetayId = sinavDetay.Id,
                                DerslikId = derslik.DerslikId,
                                GozetmenId = derslik.GozetmenId ?? 0 
                            };
                            context.SinavDerslik.Add(sinavDerslik);
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception err)
                    {
                        transaction.Rollback();
                        throw new Exception(err.Message);
                    }
                }
            }
        }


        public SinavDetay ExistSinav(List<int> derslikIdleri, List<int> gozetmenIdleri, int akademikPersonelId, TimeOnly SinavBaslangicSaati, TimeOnly SinavBitisSaati, DateTime sinavTarihi)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId into sd_join
                             from sd in sd_join.DefaultIfEmpty()
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DerBolumAkademikPersonelId equals dba.Id into dba_join
                             from dba in dba_join.DefaultIfEmpty()
                             where s.SinavTarihi.Date == sinavTarihi.Date &&
                                   (
                                       // Derslik çakışma kontrolü
                                       (derslikIdleri.Contains(sd.DerslikId)) ||
                                       // Gözetmen çakışma kontrolü
                                       (gozetmenIdleri.Contains(sd.GozetmenId)) ||
                                       // Akademik personel çakışma kontrolü
                                       (akademikPersonelId == dba.AkademikPersonelId)
                                   ) &&
                                   (
                                       (s.SinavBaslangicSaati <= SinavBaslangicSaati && s.SinavBitisSaati > SinavBaslangicSaati) ||
                                       (s.SinavBaslangicSaati < SinavBitisSaati && s.SinavBitisSaati >= SinavBitisSaati) ||
                                       (s.SinavBaslangicSaati >= SinavBaslangicSaati && s.SinavBitisSaati <= SinavBitisSaati)
                                   )
                             select s;

                return result.FirstOrDefault();
            }
        }


        public List<SinavDetayDTO> GetByBolumId(int bolumId)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DerBolumAkademikPersonelId equals dba.Id
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId
                             join dlik in context.Derslik on sd.DerslikId equals dlik.Id
                             where b.Id == bolumId
                             select new SinavDetayDTO
                             {
                                 Id = s.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan,
                                 SinavTarihi = s.SinavTarihi,
                                 SinavBaslangicSaati = s.SinavBaslangicSaati,
                                 DerslikId = sd.DerslikId,
                                 DerslikKontenjan = dlik.Kapasite,
                                 GozetmenId = sd.GozetmenId,
                                 DersBolumAkademikPersonelId = dba.Id,
                                 SinavBitisSaati = s.SinavBitisSaati,
                                 Derslikler = context.SinavDerslik
                                     .Where(x => x.SinavDetayId == s.Id)
                                     .Select(x => new DerslikGozetmenDTO
                                     {
                                         DerslikId = x.DerslikId,
                                         GozetmenId = x.GozetmenId
                                     }).ToList(),
                                 Gozetmenler = context.SinavDerslik
                                     .Where(x => x.SinavDetayId == s.Id)
                                     .Join(context.AkademikPersonel,
                                         sd => sd.GozetmenId,
                                         ap => ap.Id,
                                         (sd, ap) => ap)
                                     .ToList()
                             };

                return result.ToList();
            }
        }

        public SinavDetayDTO GetDetail(int sinavDetayId)
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DerBolumAkademikPersonelId equals dba.Id
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId
                             where s.Id == sinavDetayId
                             select new SinavDetayDTO
                             {
                                 Id = s.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 DerslikId = sd.DerslikId,
                                 GozetmenId = sd.GozetmenId,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan,
                                 SinavTarihi = s.SinavTarihi,
                                 SinavBaslangicSaati = s.SinavBaslangicSaati,
                                 SinavBitisSaati = s.SinavBitisSaati,
                                 DersBolumAkademikPersonelId = dba.Id,
                                 Derslikler = context.SinavDerslik
                                     .Where(x => x.SinavDetayId == s.Id)
                                     .Select(x => new DerslikGozetmenDTO
                                     {
                                         DerslikId = x.DerslikId,
                                         GozetmenId = x.GozetmenId
                                     }).ToList(),
                                 Gozetmenler = context.SinavDerslik
                                     .Where(x => x.SinavDetayId == s.Id)
                                     .Join(context.AkademikPersonel,
                                         sd => sd.GozetmenId,
                                         ap => ap.Id,
                                         (sd, ap) => ap)
                                     .ToList()
                             };

                return result.FirstOrDefault()!;
            }
        }

        public List<SinavDetayDTO> GetDetails()
        {
            using (DuzceUniversiteContext context = new DuzceUniversiteContext())
            {
                var result = from s in context.SinavDetay
                             join dba in context.Ders_Bolum_AkademikPersonel on s.DerBolumAkademikPersonelId equals dba.Id
                             join d in context.Ders on dba.DersId equals d.Id
                             join b in context.Bolum on dba.BolumId equals b.Id
                             join ap in context.AkademikPersonel on dba.AkademikPersonelId equals ap.Id
                             join sd in context.SinavDerslik on s.Id equals sd.SinavDetayId
                             join dlik in context.Derslik on sd.DerslikId equals dlik.Id
                             select new SinavDetayDTO
                             {
                                 Id = s.Id,
                                 DersAd = d.Ad,
                                 BolumAd = b.Ad,
                                 AkademikPersonelAd = ap.Ad,
                                 Unvan = ap.Unvan,
                                 SinavTarihi = s.SinavTarihi,
                                 SinavBaslangicSaati = s.SinavBaslangicSaati,
                                 SinavBitisSaati = s.SinavBitisSaati,
                                 DerslikId = sd.DerslikId,
                                 DerslikKontenjan = dlik.Kapasite,
                                 GozetmenId = sd.GozetmenId,
                                 DersBolumAkademikPersonelId = dba.Id,
                                 Derslikler = context.SinavDerslik
                                     .Where(x => x.SinavDetayId == s.Id)
                                     .Select(x => new DerslikGozetmenDTO
                                     {
                                         DerslikId = x.DerslikId,
                                         GozetmenId = x.GozetmenId
                                     }).ToList(),
                                 Gozetmenler = context.SinavDerslik
                                     .Where(x => x.SinavDetayId == s.Id)
                                     .Join(context.AkademikPersonel,
                                         sd => sd.GozetmenId,
                                         ap => ap.Id,
                                         (sd, ap) => ap)
                                     .ToList()
                             };

                return result.ToList();
            }
        }
    }
}
