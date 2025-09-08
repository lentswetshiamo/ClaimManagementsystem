using Microsoft.AspNetCore.Mvc;

namespace ClaimManagementsystem.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
