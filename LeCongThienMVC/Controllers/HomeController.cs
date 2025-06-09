using System.Diagnostics;
using LeCongThienMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace LeCongThienMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Category()
        {
            return RedirectToAction("Index","Categories");
        }
        public IActionResult NewsArticle()
        {
            return RedirectToAction("Index", "NewsArticles");
        }
        public IActionResult Tag()
        {
            return RedirectToAction("Index", "Tags");
        }
        public IActionResult SystemAccount()
        {
            return RedirectToAction("Index", "SystemAccounts");
        }
        public IActionResult Login()
        {
            return RedirectToAction("Login", "Access");
        }
        public IActionResult Profile()
        {
            return RedirectToAction("Profile", "Profile");
        }
        public IActionResult Report()
        {
            return RedirectToAction("Report", "NewsArticles");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
