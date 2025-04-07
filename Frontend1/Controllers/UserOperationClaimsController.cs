using Core.Entities.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "super.admin")]
    public class UserOperationClaimsController : ControllerBase
    {
        private readonly HttpService _httpService;

        public UserOperationClaimsController(HttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _httpService.GetAsync<List<UserOperationClaim>>("api/useroperationclaim");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _httpService.GetAsync<UserOperationClaim>($"api/useroperationclaim/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] UserOperationClaim userOperationClaim)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/useroperationclaim", userOperationClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/useroperationclaim/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
} 