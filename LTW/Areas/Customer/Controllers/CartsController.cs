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
        [Authorize]
        public IActionResult Checkout()
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
                {
                    ApplicationUser = _db.ApplicationUsers.FirstOrDefault(x => x.Id == claim.Value)
                }
            };

            if (cartModel.Carts.Count() == 0)
            {
                return RedirectToAction("Index", "Carts", new { area = "Customer" });
            }

            cartModel.Invoices.Name = cartModel.Invoices.ApplicationUser.Name;
            cartModel.Invoices.Address = cartModel.Invoices.ApplicationUser.Address;
            cartModel.Invoices.PhoneNumber = cartModel.Invoices.ApplicationUser.PhoneNumber;

            foreach (var cart in cartModel.Carts)
            {
                cart.Price = cart.Quantity * cart.Product.Price;
                cartModel.Invoices.Total += cart.Price;
            }

            return View(cartModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(CartViewModel cartModel)
        {
            var identity = (ClaimsIdentity)User.Identity;
            var claim = identity.FindFirst(ClaimTypes.NameIdentifier);

            cartModel.Carts = _db.Carts
                .Include("Product")
                .Where(x => x.ApplicationUserId == claim.Value)
                .ToList();

            if (cartModel.Carts.Count() == 0)
            {
                return RedirectToAction("Index", "Carts", new { area = "Customer" });
            }

            cartModel.Invoices.ApplicationUserId = claim.Value;
            cartModel.Invoices.OrderDate = DateTime.Now;
            cartModel.Invoices.OrderStatus = "Waiting for confirmation";

            foreach (var cart in cartModel.Carts)
            {
                cart.Price = cart.Quantity * cart.Product.Price;
                cartModel.Invoices.Total += cart.Price;
            }

            _db.Invoices.Add(cartModel.Invoices);
            _db.SaveChanges();

            foreach (var cart in cartModel.Carts)
            {
                var invoiceDetail = new InvoiceDetail()
                {
                    InvoiceId = cartModel.Invoices.Id,
                    ProductId = cart.ProductId,
                    Quantity = cart.Quantity,
                    Price = cart.Price
                };

                _db.InvoiceDetails.Add(invoiceDetail);
                _db.Carts.Remove(cart);
            }

            _db.Carts.RemoveRange(cartModel.Carts);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home", new { area = "Customer" });
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
