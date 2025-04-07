using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolumDersAkademikPersonelController : ControllerBase
    {
        private readonly IBolumService _bolumService;
        private readonly IDersService _dersService;
        private readonly IAkademikPersonelService _akademikPersonelService;
        private readonly IDBAPService _dbapService;

        public BolumDersAkademikPersonelController(
            IDBAPService dBAPService,
            IBolumService bolumService,
            IDersService dersService,
            IAkademikPersonelService akademikPersonelService)
        {
            _bolumService = bolumService;
            _dersService = dersService;
            _akademikPersonelService = akademikPersonelService;
            _dbapService = dBAPService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _dbapService.GetAllDetails();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("bolum/{bolumId}")]
        public IActionResult GetByBolumId(int bolumId)
        {
            var result = _dbapService.GetByBolumId(bolumId);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetDetail(int id)
        {
            var result = _dbapService.GetDetail(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add([FromBody] DersBolumAkademikPersonel model)
        {
            var result = _dbapService.Add(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody] DersBolumAkademikPersonel model)
        {
            var result = _dbapService.Delete(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
} 