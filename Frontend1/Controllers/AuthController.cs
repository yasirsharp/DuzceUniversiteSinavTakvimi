using Entity.DTOs;
using Frontend1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Frontend1.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpService _httpService;

        public AuthController(HttpService httpService)
        {
            _httpService = httpService;
        }

        private void SetUserCookies(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims;

            // Token'ı cookie'ye kaydet
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(120)
            });

            // Kullanıcı bilgilerini cookie'lere kaydet
            Response.Cookies.Append("UserInfo-Id", 
                claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? "",
                new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(120)
                });

            Response.Cookies.Append("UserInfo-FullName", 
                claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value ?? "",
                new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(120)
                });

            Response.Cookies.Append("UserInfo-Email", 
                claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "",
                new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(120)
                });

            Response.Cookies.Append("UserInfo-Role", 
                claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value ?? "",
                new CookieOptions
                {
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.Now.AddMinutes(120)
                });

            // Bölüm ID'lerini cookie'lere kaydet
            for (int i = 1; i <= 3; i++)
            {
                var bolumId = claims.FirstOrDefault(c => c.Type == $"{i}.BolumId")?.Value;
                if (!string.IsNullOrEmpty(bolumId))
                {
                    Response.Cookies.Append($"UserInfo-BolumId-{i}", bolumId, new CookieOptions
                    {
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.Now.AddMinutes(120)
                    });
                }
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies.ContainsKey("AuthToken"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost("/auth/login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            try
            {
                var response = await _httpService.PostAsync<object>("api/auth/login", userForLoginDto);
                var result = JsonSerializer.Deserialize<JsonElement>(response.ToString());
                
                var token = result.GetProperty("token").GetString();
                SetUserCookies(token);

                return Ok(new { message = "Giriş başarılı!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (Request.Cookies.ContainsKey("AuthToken"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost("/auth/register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            try
            {
                var response = await _httpService.PostAsync<object>("api/auth/register", userForRegisterDto);
                var result = JsonSerializer.Deserialize<JsonElement>(response.ToString());

                var token = result.GetProperty("token").GetString();
                SetUserCookies(token);

                return Ok(new { message = "Kayıt başarılı!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _httpService.PostAsync<object>("api/auth/logout", null);
                
                // Tüm cookie'leri sil
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);
                }

                return Ok(new { message = "Çıkış başarılı!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
