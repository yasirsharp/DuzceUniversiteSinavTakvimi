using Business.Abstract;
using Entity.Concrete;
using Entity.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinavDetayController : ControllerBase
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

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _sinavDetayService.GetAllDetails();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("bolum/{bolumId}")]
        public IActionResult GetByBolumId(int bolumId)
        {
            var result = _sinavDetayService.GetByBolumId(bolumId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add([FromBody] SinavKayitDTO model)
        {
            var result = _sinavDetayService.Add(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] SinavGuncelleDTO model)
        {
            if (model == null || model.Id <= 0)
            {
                return BadRequest("Geçersiz sınav verisi");
            }

            var mevcutDerslikler = _sinavDerslikService.GetBySinavDetayId(model.Id);
            if (mevcutDerslikler.Success)
            {
                foreach (var derslik in mevcutDerslikler.Data)
                {
                    _sinavDerslikService.Delete(derslik);
                }
            }

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
                return BadRequest(updateResult.Message);
            }

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
                    return BadRequest(derslikResult.Message);
                }
            }

            return Ok(new { message = "Sınav başarıyla güncellendi" });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _sinavDetayService.Delete(new SinavDetay { Id = id });
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("derslik/{derslikId}")]
        public IActionResult GetSinavlarByDerslik(int derslikId)
        {
            var result = _sinavDerslikService.GetByDerslikId(derslikId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("sinavderslikler/{sinavDetayId}")]
        public IActionResult GetSinavDerslikler(int sinavDetayId)
        {
            var result = _sinavDerslikService.GetBySinavDetayId(sinavDetayId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("sinavdersliklerdetay/{sinavDetayId}")]
        public IActionResult GetSinavDersliklerDetay(int sinavDetayId)
        {
            var result = _sinavDerslikService.GetBySinavDetayId(sinavDetayId);
            if (!result.Success)
            {
                return BadRequest(result.Message);
            }

            var detayliBilgiler = result.Data.Select(sd => {
                var derslik = _derslikService.GetById(sd.DerslikId);
                var gozetmen = sd.GozetmenId > 0 ? _akademikPersonelService.GetById(sd.GozetmenId) : null;

                return new {
                    DerslikAdi = derslik.Success ? derslik.Data.Ad : "Derslik Bilgisi Yok",
                    GozetmenAdi = gozetmen?.Success == true ? 
                        $"{gozetmen.Data.Unvan} {gozetmen.Data.Ad}" : 
                        "Gözetmen Atanmamış"
                };
            }).ToList();

            return Ok(detayliBilgiler);
        }
    }
} 