using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LTW.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryManagerController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryManagerController(ApplicationDbContext db)
        {
            _db = db;

        }
        public IActionResult Index()
        {
            IEnumerable<Category> theloai = _db.Categories.ToList();
            return View(theloai);
        }

        [HttpGet]
        public IActionResult Upsert(int Id)
        {
            Category theloai = new Category();

            if (Id == 0)
            {
                return View(theloai);
            }
            else
            {
                theloai = _db.Categories.Find(Id);
                if (theloai == null)
                {
                    return NotFound();
                }
                return View(theloai);
            }
        }

        [HttpPost]
        public IActionResult Upsert(Category theloai)
        {
            if (ModelState.IsValid)
            {
                if (theloai.Id == 0)
                {
                    _db.Categories.Add(theloai);
                }
                else
                {
                    _db.Categories.Update(theloai);
                }
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            Category theloai = _db.Categories.FirstOrDefault(sp => sp.Id == Id);
            if (theloai != null)
            {
                _db.Categories.Remove(theloai);
                _db.SaveChanges();
                return Json(new { success = true, message = "Xóa thành công." });
            }
            return Json(new { success = false, message = "Không tìm thấy thể loại cần xóa." });
        }
    }
}
