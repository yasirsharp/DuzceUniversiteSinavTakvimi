using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AkademikPersonelController : ControllerBase
    {
        private readonly IAkademikPersonelService _akademikPersonelService;

        public AkademikPersonelController(IAkademikPersonelService akademikPersonelService)
        {
            _akademikPersonelService = akademikPersonelService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _akademikPersonelService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _akademikPersonelService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Add(akademikPersonel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] AkademikPersonel akademikPersonel)
        {
            var result = _akademikPersonelService.Update(akademikPersonel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var akademikPersonel = _akademikPersonelService.GetById(id).Data;
            if (akademikPersonel == null)
            {
                return NotFound("Akademik personel bulunamadı");
            }

            var result = _akademikPersonelService.Delete(akademikPersonel);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
} 