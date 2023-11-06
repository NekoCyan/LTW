using System.Security.Principal;
using LTW.Data;
using LTW.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace LTW.Extensions
{
    public static class IIdentityExtensions
    {
        //public static string GetUserDisplayName(this IIdentity identity, ApplicationDbContext _db)
        //{
        //    ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(x => x.UserName == identity.Name);
        //    if (user != null)
        //    {
        //        return user.Name;
        //    }
        //    return string.Empty;
        //}
    }
}
