using LTW.Data;
using Microsoft.AspNetCore.Mvc;

namespace LTW.ViewComponents
{
    public class CategoryViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;
        public CategoryViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _db.Categories.ToList();
            return View(categories);
        }
    }
}
