using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BabyStore.Models
{
    public class Category
    {
        public int ID { set; get; }
        [Required(ErrorMessage = "The category name cannot be empty!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a Category name of length 3-50 chars.")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z]''-'\s]*$", ErrorMessage = "Category name must begin with an Uppercase letter and contain only letters and space")]
        [Display(Name = "Category Name")]
        public string Name { set; get; }
        public virtual ICollection<Product> Products {set; get; }
    }
}