using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LTW.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CartsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<Cart> carts;

            if (claim != null)
            {
                carts = _db.Carts.Include("Product").Where(x => x.ApplicationUserId == claim.Value).ToList().ConvertAll(x =>
                {
                    x.Product.ImageParser();
                    return x;
                });
            }
            else
            {
                carts = new List<Cart>();
            }

            return View(carts);
        }
    }
}
