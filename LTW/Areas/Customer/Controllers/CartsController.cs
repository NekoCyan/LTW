using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public IActionResult TotalPrice()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null) return Content("0");

            CartViewModel cartModel = new CartViewModel()
            {
                Carts = _db.Carts
                .Include("Product")
                .Where(x => x.ApplicationUserId == claim.Value)
                .ToList().ConvertAll(x =>
                {
                    x.Product.ImageParser();
                    return x;
                }),
                Invoices = new Invoice()
            };

            foreach (var cart in cartModel.Carts)
            {
                cart.Price = cart.Quantity * cart.Product.Price;
                cartModel.Invoices.Total += cart.Price;
            }

            //return Content(cartModel.Invoices.Total.ToString());
            return Json(new { success = true, data = cartModel.Invoices.Total.ToString() });
        }

        [Authorize]
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            CartViewModel cartModel = new CartViewModel()
            {
                Carts = _db.Carts
                .Include("Product")
                .Where(x => x.ApplicationUserId == claim.Value)
                .ToList().ConvertAll(x =>
                {
                    x.Product.ImageParser();
                    return x;
                }),
                Invoices = new Invoice()
            };

            foreach (var cart in cartModel.Carts)
            {
                cart.Price = cart.Quantity * cart.Product.Price;
                cartModel.Invoices.Total += cart.Price;
            }

            return View(cartModel);
        }

        [HttpGet]
        public IActionResult Giam(int giohangId)
        {
            var cart = _db.Carts.FirstOrDefault(gh => gh.Id == giohangId);
            if (cart != null)
            {
                cart.Quantity -= 1;
                if (cart.Quantity <= 0)
                {
                    _db.Carts.Remove(cart);
                }
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Tang(int giohangId)
        {
            var cart = _db.Carts.FirstOrDefault(gh => gh.Id == giohangId);
            if (cart != null)
            {
                cart.Quantity += 1;
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Xoa(int giohangId)
        {
            var cart = _db.Carts.FirstOrDefault(gh => gh.Id == giohangId);
            if (cart != null)
            {
                _db.Carts.Remove(cart);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
