using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LTW.Models
{
    public class CartViewModel
    {
        public IEnumerable<Cart> Carts { get; set; }
        //public double TotalPrice { get; set; }
        public Invoice Invoices { get; set; }
    }
}
