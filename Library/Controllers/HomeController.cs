using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.UI.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Home()
        {
            return View();
        }
    }
}
