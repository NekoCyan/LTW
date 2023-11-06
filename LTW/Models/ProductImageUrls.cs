using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace LTW.Models
{
    //[Index(nameof(ImageUrl), IsUnique = true)]
    public class ProductImageUrls
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public string ImageUrls { get; set; }

        [ForeignKey(nameof(ProductId))]
        [ValidateNever]
        public Product Product { get; set; }
    }
}
