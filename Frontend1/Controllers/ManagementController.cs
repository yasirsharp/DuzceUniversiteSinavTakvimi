using Core.Entities.Concrete;
using Frontend1.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend1.Controllers
{
    public class ManagementController : Controller
    {
        private readonly HttpService _httpService;

        public ManagementController(HttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IActionResult> User()
        {
            try
            {
                var users = await _httpService.GetAsync<List<User>>("api/user");
                var operationClaims = await _httpService.GetAsync<List<OperationClaim>>("api/operationclaim");
                var userOperationClaims = await _httpService.GetAsync<List<UserOperationClaim>>("api/useroperationclaim");

                ViewData["Users"] = users;
                ViewData["OperationClaims"] = operationClaims;
                ViewData["UserOperationClaims"] = userOperationClaims;

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpGet]
        [Route("/management/operationclaim/getall")]
        public async Task<IActionResult> OperationClaimGetAll()
        {
            try
            {
                var result = await _httpService.GetAsync<List<OperationClaim>>("api/operationclaim");
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("/management/operationclaim/getbyid/{id}")]
        public async Task<IActionResult> OperationClaimGetById(int id)
        {
            try
            {
                var result = await _httpService.GetAsync<OperationClaim>($"api/operationclaim/{id}");
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("/management/useroperationclaim/getall")]
        public async Task<IActionResult> UserOperationClaimAll()
        {
            try
            {
                var result = await _httpService.GetAsync<List<UserOperationClaim>>("api/useroperationclaim");
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("/management/user/getall")]
        public async Task<IActionResult> UserAll()
        {
            try
            {
                var result = await _httpService.GetAsync<List<User>>("api/user");
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("/management/user/getbyid/{id}")]
        public async Task<IActionResult> UserGetById(int id)
        {
            try
            {
                var result = await _httpService.GetAsync<User>($"api/user/{id}");
                return Json(result);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        [HttpGet]
        [Route("/management/operationclaim/delete/{id}")]
        public async Task<IActionResult> OperationClaimDelete(int id)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/operationclaim/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/management/operationclaim/update")]
        public async Task<IActionResult> OperationClaimUpdate(OperationClaim operationClaim)
        {
            try
            {
                var result = await _httpService.PutAsync<object>("api/operationclaim", operationClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/management/operationclaim/add")]
        public async Task<IActionResult> OperationClaimAdd(OperationClaim operationClaim)
        {
            try
            {
                var result = await _httpService.PostAsync<object>("api/operationclaim", operationClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Route("/management/useroperationclaim/add")]
        public async Task<IActionResult> UserOperationClaimAdd([FromBody] UserOperationClaim userOperationClaim)
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

        [HttpGet]
        [Route("management/useroperationclaim/delete/{id}")]
        public async Task<IActionResult> DeleteUserOperationClaim(int id)
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

        [HttpPost]
        [Route("/management/useroperationclaim/update")]
        public async Task<IActionResult> UserOperationClaimUpdate(UserOperationClaim userOperationClaim)
        {
            try
            {
                var result = await _httpService.PutAsync<object>("api/useroperationclaim", userOperationClaim);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("/management/user/delete/{id}")]
        public async Task<IActionResult> UserDelete(int id)
        {
            try
            {
                var result = await _httpService.DeleteAsync<object>($"api/user/{id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        public IActionResult Settings()
        {
            return View();
        }
    }
}
