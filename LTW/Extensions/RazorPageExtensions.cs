using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using LTW.Data;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LTW.Extensions
{
    public static class RazorPageExtensions
    {
        public static string GetUserDisplayName(this RazorPageBase page)
        {
            // Access the HttpContext to get the user's name
            string userName = page.ViewContext.HttpContext.User.Identity.Name;

            // Retrieve the ApplicationDbContext from the service container
            var dbContext = page.ViewContext.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();

            // Use the dbContext to access the database
            var user = dbContext.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);

            // Retrieve the display name if available
            return user?.Name ?? userName;
        }

        public static void ExcludeScripts(this RazorPage page, params string[] scripts)
        {
            page.ViewBag["ExcludedScripts"] = scripts.ToList();
        }

        public static void GetExcludedScripts(this RazorPage page)
        {
            if (page.ViewBag["ExcludedScripts"] == null)
            {
                page.ViewBag["ExcludedScripts"] = new List<string>();
            } else
            {
                page.ViewBag["ExcludedScripts"] = page.ViewBag["ExcludedScripts"];
            }
        }
    }
}
