using Microsoft.AspNetCore.Mvc;

namespace TrainingApp.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
