using Microsoft.AspNetCore.Mvc;
using Business.Abstract;
using Entity.Concrete;
using Core.Utilities.Results;
using Entity.DTOs;
using System.Linq;

namespace Frontend1.Controllers
{
    public class SinavDetayController : Controller
    {
        private readonly ISinavDetayService _sinavDetayService;
        private readonly IDBAPService _dBAPService;
        private readonly IDerslikService _derslikService;
        private readonly ISinavDerslikService _sinavDerslikService;
        private readonly IAkademikPersonelService _akademikPersonelService;

        public SinavDetayController(
            ISinavDetayService sinavDetayService, 
            IDBAPService dBAPService, 
            IDerslikService derslikService,
            ISinavDerslikService sinavDerslikService,
            IAkademikPersonelService akademikPersonelService)
        {
            _sinavDetayService = sinavDetayService;
            _dBAPService = dBAPService;
            _derslikService = derslikService;
            _sinavDerslikService = sinavDerslikService;
            _akademikPersonelService = akademikPersonelService;
        }

        public IActionResult Index(int? bolumId)
        {
            ViewData["DBAP"] = _dBAPService.GetAll();
            ViewData["DBAPDetail"] = _dBAPService.GetAllDetails();
            ViewData["Derslikler"] = _derslikService.GetList();
            ViewData["AkademikPersoneller"] = _akademikPersonelService.GetList();
            
            if (bolumId.HasValue)
            {
                ViewData["SeciliBolumId"] = bolumId.Value;
                var bolumSinavlari = _sinavDetayService.GetByBolumId(bolumId.Value);
                ViewData["BolumSinavlari"] = bolumSinavlari.Data;
            }
            
            return View();
        }

        [HttpGet]
        [Route("SinavDetay/GetEvents")]
        public IActionResult GetEvents(int? bolumId)
        {
            IDataResult<List<SinavDetayDTO>> result;
            
            if (bolumId.HasValue)
            {
                result = _sinavDetayService.GetByBolumId(bolumId.Value);
            }
            else
            {
                result = _sinavDetayService.GetAllDetails();
            }

            if (!result.Success)
            {
                return Json(new { success = false, message = result.Message });
            }

            var events = result.Data.Select(s => new
            {
                id = s.Id,
                title = $"{s.DersAd} - {s.BolumAd}",
                start = s.SinavTarihi.ToString("yyyy-MM-dd") + "T" + s.SinavBaslangicSaati.ToString("HH:mm"),
                end = s.SinavTarihi.ToString("yyyy-MM-dd") + "T" + s.SinavBitisSaati.ToString("HH:mm"),
                extendedProps = new
                {
                    derslikler = s.Derslikler,
                    gozetmenler = s.Gozetmenler,
                    bolumAd = s.BolumAd,
                    dersAd = s.DersAd,
                    akademikPersonelAd = s.AkademikPersonelAd
                }
            });

            return Json(events);
        }

        [HttpPost]
        [Route("SinavDetay/Add")]
        public IActionResult Add([FromBody] SinavKayitDTO model)
        {
            try
            {
                var result = _sinavDetayService.Add(model);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }
                return Json(new { success = true, message = result.Message });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "İşlem sırasında bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [Route("SinavDetay/Update")]
        public IActionResult Update([FromBody] SinavGuncelleDTO model)
        {
            try
            {
                if (model == null || model.Id <= 0)
                {
                    return Json(new { success = false, message = "Geçersiz sınav verisi." });
                }

                // Mevcut derslik-gözetmen eşleştirmelerini sil
                var mevcutDerslikler = _sinavDerslikService.GetBySinavDetayId(model.Id);
                if (mevcutDerslikler.Success)
                {
                    foreach (var derslik in mevcutDerslikler.Data)
                    {
                        _sinavDerslikService.Delete(derslik);
                    }
                }

                // Sınav detayını güncelle
                var sinavDetay = new SinavDetay
                {
                    Id = model.Id,
                    DerBolumAkademikPersonelId = model.DbapId,
                    SinavTarihi = model.SinavTarihi,
                    SinavBaslangicSaati = model.SinavBaslangicSaati,
                    SinavBitisSaati = model.SinavBitisSaati
                };

                var updateResult = _sinavDetayService.Update(sinavDetay);
                if (!updateResult.Success)
                {
                    return Json(new { success = false, message = updateResult.Message });
                }

                // Yeni derslik-gözetmen eşleştirmelerini ekle
                foreach (var derslik in model.Derslikler)
                {
                    var sinavDerslik = new SinavDerslik
                    {
                        SinavDetayId = model.Id,
                        DerslikId = derslik.DerslikId,
                        GozetmenId = derslik.GozetmenId ?? 0
                    };

                    var derslikResult = _sinavDerslikService.Add(sinavDerslik);
                    if (!derslikResult.Success)
                    {
                        return Json(new { success = false, message = derslikResult.Message });
                    }
                }

                return Json(new { success = true, message = "Sınav başarıyla güncellendi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Güncelleme sırasında bir hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        [Route("SinavDetay/Delete")]
        public IActionResult Delete([FromBody] int id)
        {
            try
            {
                var sinavDetay = new SinavDetay { Id = id };
                var result = _sinavDetayService.Delete(sinavDetay);

                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                return Json(new { success = true, message = "Sınav başarıyla silindi." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Silme işlemi sırasında bir hata oluştu: " + ex.Message });
            }
        }

        [HttpGet]
        [Route("SinavDetay/GetEventDetails/{id}")]
        public IActionResult GetEventDetails(int id)
        {
            try
            {
                var result = _sinavDetayService.GetById(id);
                if (!result.Success)
                {
                    return Json(new { success = false, message = result.Message });
                }

                var derslikler = _sinavDerslikService.GetBySinavDetayId(id);
                if (!derslikler.Success)
                {
                    return Json(new { success = false, message = derslikler.Message });
                }

                return Json(new { 
                    success = true, 
                    data = new {
                        sinavDetay = result.Data,
                        derslikler = derslikler.Data
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Detaylar getirilirken bir hata oluştu: " + ex.Message });
            }
        }
    }
}
