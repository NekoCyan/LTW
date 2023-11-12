using LTW.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var categories = _db.Categories.ToList().ConvertAll(c =>
            {
                return new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = false,
                };
            });

            return View(categories);
        }
    }
}
