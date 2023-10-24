using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Mvc;

namespace LTW.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductManagerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductManagerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Products.ToList();
            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int Id)
        {
            Product product = new Product();

            if (Id == 0)
            {
                return View(product);
            }
            else
            {
                product = _db.Products.Find(Id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Product product)
        {
            if (ModelState.IsValid)
            {
                if (product.Id == 0)
                {
                    _db.Products.Add(product);
                }
                else
                {
                    _db.Products.Update(product);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Product product = _db.Products.FirstOrDefault(sp => sp.Id == Id);
            if (product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công." });
            }
            return Json(new { success = false, message = "Không tìm thấy sản phầm cần xóa." });
        }
    }
}
