using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace LTW.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Products.Include("Category").ToList().ConvertAll(x =>
            {
                x.ImageUrls = x.ImageUrl.Split(" ").ToList();
                return x;
            });

            return View(products);
        }

        //[HttpGet]
        //public IActionResult Details(int Id)
        //{
        //    Product pd = _db.Products.Include("Category").FirstOrDefault(x => x.Id == Id);
        //    if (pd == null)
        //    {
        //        return NotFound();
        //    }

        //    pd.ImageUrls = pd.ImageUrl.Split(" ").ToList();

        //    return View(pd);
        //}
        [HttpGet]
        public IActionResult Details(int productId)
        {
            if (productId == 0)
            {
                return NotFound();
            }

            Cart cart = new Cart()
            {
                ProductId = productId,
                Quantity = 1,
                Product = _db.Products.Include("Category").FirstOrDefault(x => x.Id == productId),
            };

            cart.Product.ImageUrls = cart.Product.ImageUrl.Split(" ").ToList();

            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(int productId)
        {
            if (ModelState.IsValid)
            {
                var identity = (ClaimsIdentity)User.Identity;
                var claim = identity.FindFirst(ClaimTypes.NameIdentifier);
                var cart = new Cart()
                {
                    ProductId = productId,
                    Quantity = 1,
                    Product = _db.Products.Include("Category").FirstOrDefault(x => x.Id == productId),
                }
                cart.ApplicationUserId = claim.Value;
                _db.Carts.Add(cart);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cart);
        }

        [HttpPost]
        public IActionResult NextProduct(int Id)
        {
            var products = _db.Products.ToList();
            var currentProduct = products.Find(x => x.Id == Id);
            if (currentProduct == null)
            {
                return NotFound();
            }

            int index = products.IndexOf(currentProduct);
            if (products.Count == index + 1)
            {
                return RedirectToAction("Details", new { productId = products.First().Id });
            }
            else
            {
                return RedirectToAction("Details", new { productId = products[index + 1].Id });
            }
        }

        [HttpPost]
        public IActionResult PrevProduct(int Id)
        {
            var products = _db.Products.ToList();
            var currentProduct = products.Find(x => x.Id == Id);
            if (currentProduct == null)
            {
                return NotFound();
            }

            int index = products.IndexOf(currentProduct);
            if (index == 0)
            {
                return RedirectToAction("Details", new { productId = products.Last().Id });
            }
            else
            {
                return RedirectToAction("Details", new { productId = products[index - 1].Id });
            }
        }
    }
}
