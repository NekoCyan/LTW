using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            IEnumerable<Product> products = _db.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Details(int Id)
        {
            Product pd = _db.Products.FirstOrDefault(x => x.Id == Id);
            if (pd == null)
            {
                return NotFound();
            }

            return View(pd);
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
                return RedirectToAction("Details", new { id = products.First().Id });
            }
            else
            {
                return RedirectToAction("Details", new { id = products[index + 1].Id });
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
                return RedirectToAction("Details", new { id = products.Last().Id });
            }
            else
            {
                return RedirectToAction("Details", new { id = products[index - 1].Id });
            }
        }
    }
}
