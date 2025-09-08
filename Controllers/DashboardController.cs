using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
