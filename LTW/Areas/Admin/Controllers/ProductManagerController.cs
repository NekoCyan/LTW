using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.ComponentModel.DataAnnotations;

namespace LTW.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductManagerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductManagerController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> products = _db.Products.Include("Category").ToList().ConvertAll(x =>
            {
                x.ImageParser();
                return x;
            });

            return View(products);
        }

        [HttpGet]
        public IActionResult Upsert(int Id)
        {
            Product product;

            ViewBag.CategoriesList = _db.Categories.ToList().ConvertAll(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
            });

            if (Id == 0)
            {
                product = new Product();

                return View(product);
            }
            else
            {
                product = _db.Products.Include("Category").FirstOrDefault(x => x.Id == Id);

                if (product == null)
                {
                    return NotFound();
                }

                product.ImageParser();

                return View(product);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Product product)
        {
            // Validate URLs.
            var context = new ValidationContext(product);
            var results = product.ValidateURL(context);
            foreach (var result in results)
            {
                ModelState.AddModelError(result.MemberNames.First(), result.ErrorMessage);
            }

            if (product.ImageUrls != null && product.ImageUrls.Count > 0)
            {
                product.ImageUrl = string.Join(" ", product.ImageUrls.Distinct());
            }

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

            return View(product);
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
