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
            var categories = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "All Categories",
                    Value = "0",
                    Selected = true,
                }
            };

            categories.AddRange(_db.Categories.ToList().ConvertAll(c =>
            {
                return new SelectListItem()
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                    Selected = false,
                };
            }));

            return View(categories);
        }
    }
}
