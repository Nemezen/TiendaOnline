using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TiendaOnline.Data;

namespace TiendaOnline.Controllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(ApplicationDbContext context)
            :base (context) { }

        public IActionResult Index ()
        {
            return View();
        }
    }
}
