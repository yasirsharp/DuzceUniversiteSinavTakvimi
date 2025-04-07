using Business.Abstract;
using Entity.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DersController : ControllerBase
    {
        private readonly IDersService _dersService;

        public DersController(IDersService dersService)
        {
            _dersService = dersService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _dersService.GetList();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _dersService.GetById(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Ders ders)
        {
            var result = _dersService.Add(ders);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpPut]
        public IActionResult Update([FromBody] Ders ders)
        {
            var result = _dersService.Update(ders);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var ders = _dersService.GetById(id).Data;
            if (ders == null)
            {
                return NotFound("Ders bulunamadı");
            }

            var result = _dersService.Delete(ders);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result.Message);
        }
    }
} 