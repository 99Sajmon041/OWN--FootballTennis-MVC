using FootballTennis.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballTennis.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            int? statusCode = null;

            if (TempData["StatusCode"] is int sc)
            {
                statusCode = sc;
            }

            var model = new ErrorModel
            {
                StatusCode = statusCode,
                TraceId = TempData["TraceId"] as string,
                Message = TempData["Error"] as string
            };

            return View(model);
        }

        public IActionResult Rules()
        {
            return View();
        }

        public IActionResult Forbidden()
        {
            return View();
        }

        public IActionResult NotFoundPage()
        {
            return View();
        }
    }
}
