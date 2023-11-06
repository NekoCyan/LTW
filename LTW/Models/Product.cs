﻿using static LTW.Utils.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTW.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "URL is required")]
        public List<string> ImageUrls { get; set; }

        public IEnumerable<ValidationResult> ValidateURL(ValidationContext context)
        {
            if (ImageUrls != null)
            {
                foreach (var url in ImageUrls)
                {
                    if (url == "")
                    {
                        yield return new ValidationResult("URL is Missing", new[] { nameof(ImageUrls) });
                    }
                    else if (!IsValidUrl(url))
                    {
                        yield return new ValidationResult("Invalid URL", new[] { nameof(ImageUrls) });
                    }
                }
            }
            else
            {
                yield return new ValidationResult("Image for Product is Missing, please reload page", new[] { nameof(ImageUrls) });
            }
        }
    }
}
