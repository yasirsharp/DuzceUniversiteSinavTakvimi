using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GorevliSinavYonetimController : ControllerBase
    {
        private readonly IBolumService _bolumService;
        private readonly ISinavDetayService _sinavDetayService;

        public GorevliSinavYonetimController(IBolumService bolumService, ISinavDetayService sinavDetayService)
        {
            _bolumService = bolumService;
            _sinavDetayService = sinavDetayService;
        }

        [HttpGet("bolumler")]
        public IActionResult GetBolumler()
        {
            var bolumIds = GetBolumIdsFromToken();
            var uniqueBolumIds = bolumIds.Distinct().ToList();
            var bolums = new List<Bolum>();

            foreach (var item in uniqueBolumIds)
            {
                var result = _bolumService.GetById(item);
                if (result.Success)
                {
                    bolums.Add(result.Data);
                }
            }

            return Ok(bolums.OrderBy(b => b.Ad).ToList());
        }

        [HttpGet("sinavlar/{bolumId}")]
        public IActionResult GetSinavlarByBolum(int bolumId)
        {
            var sinavlar = _sinavDetayService.GetByBolumId(bolumId);
            if (sinavlar.Success)
            {
                return Ok(sinavlar.Data);
            }
            return BadRequest(sinavlar.Message);
        }

        private List<int> GetBolumIdsFromToken()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return new List<int>();

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var claims = jwtToken.Claims;

                var bolumIds = claims
                    .Where(c => c.Type.EndsWith(".BolumId"))
                    .Select(c => int.TryParse(c.Value, out int id) ? id : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id.Value)
                    .ToList();

                return bolumIds;
            }
            catch
            {
                return new List<int>();
            }
        }
    }
} 