using Microsoft.AspNetCore.Mvc;
using ProjectDotNet.Data;
using ProjectDotNet.Models;
using System.Diagnostics;

namespace ProjectDotNet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string searchquery)
        {
            HttpContext.Session.SetString("guest", "guest_user");
            HttpContext.Session.SetString("cartdata", "");
            List<Product> display = ProductList.GetProducts();
            if (searchquery != null)
            {
                List<Product> filtered = ProductList.filterlist(searchquery, display);
                ViewBag.ProductList = filtered;
            }
            else
            {
                ViewBag.ProductList = display;
            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username != null && password != null)
            {
                User targetuser = UserData.GetUser(username, password);
                if (targetuser != null)
                {
                    return Json(new { success = true, redirectToUrl = Url.Action("CheckOut") });
                }
                return Json(new { success = false, message = "Your username or password is wrong. " });
            }
            return Json(new { success = false, message = "Your username and password cannot be empty. " });
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        public IActionResult Purchases()
        {
            var purchases = PurchasesData.GetPurchases();
            return View(purchases);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
