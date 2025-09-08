using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class ClaimController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
