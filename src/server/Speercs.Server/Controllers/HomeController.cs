using Microsoft.AspNetCore.Mvc;

namespace Speercs.Server.Controllers {
    public class HomeController : Controller {
        public IActionResult index() {
            return View();
        }

        public IActionResult error() {
            return View();
        }
    }
}