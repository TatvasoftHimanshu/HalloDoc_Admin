using Microsoft.AspNetCore.Mvc;

namespace HalloDoc_Admin_.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
