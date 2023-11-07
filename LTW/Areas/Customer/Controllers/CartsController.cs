using Microsoft.AspNetCore.Mvc;

namespace LTW.Areas.Customer.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
