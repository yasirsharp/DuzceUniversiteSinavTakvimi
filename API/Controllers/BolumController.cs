using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolumController : ControllerBase
    {
        private readonly IBolumService _bolumService;

        public BolumController(IBolumService bolumService)
        {
            _bolumService = bolumService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _bolumService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _bolumService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Bolum bolum)
        {
            var result = _bolumService.Add(bolum);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Bolum bolum)
        {
            var result = _bolumService.Update(bolum);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var bolum = _bolumService.GetById(id).Data;
            if (bolum == null)
            {
                return NotFound("Bölüm bulunamadı");
            }

            var result = _bolumService.Delete(bolum);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
} 