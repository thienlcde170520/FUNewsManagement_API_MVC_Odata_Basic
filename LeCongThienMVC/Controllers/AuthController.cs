using FUnewsDTO;
using LeCongThienMVC.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using System.Net.Http;
using System.Security.Claims;

namespace LeCongThienMVC.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISystemAccountService _authService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient _httpClient;
        public AuthController(ISystemAccountService authService, IHttpClientFactory httpClientFactory, HttpClient httpClient)
        {
            _authService = authService;
            _httpClientFactory = httpClientFactory;
            _httpClient = httpClient;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(model);

            var loginResponse = await _authService.Login(model);
            if (loginResponse == null)
            {
                ModelState.AddModelError(string.Empty, "Mật khẩu hoặc email sai, vui lòng nhập lại");
                ViewBag.ErrorMessage = "Mật khẩu hoặc email sai, vui lòng nhập lại";
                return View(model);
            }

            // Lưu token vào Session
            HttpContext.Session.SetString("AccessToken", loginResponse.Token);

            // Giải mã token để lấy claims
            var claims = JwtUtils.DecodeToken(loginResponse.Token).ToList();

            // Thêm các claim cần thiết
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied() => View();

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AccessToken");
            return RedirectToAction("Login", "Auth");
        }
    }
}
