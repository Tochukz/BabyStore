using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; 

namespace BabyStore.Models
{
    public partial class Product
    {
        public int ID { set; get; }
        //[Display(Name = "Product Name")]
        [Required(ErrorMessage = "Product name cannot be empty!")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Product name must be of 3-50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Product name can only contain letters, numbers and space.")]
        public string Name { set; get; }

        [Required(ErrorMessage = "Description cannot be empty.")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters")]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s]*$", ErrorMessage = "Product description must be letters and numbers only!")]
        [DataType(DataType.MultilineText)]
        public string Description { set; get; }

        [Required(ErrorMessage = "Please enter the price.")]
        [Range(0.10, 1000, ErrorMessage = "Please enter a price of between 0.10 and 10,000")]
        [DataType(DataType.Currency)] //For currency formating
        [DisplayFormat(DataFormatString = "{0:c}")] //Also for currency formating. Either one will do.
        [RegularExpression("[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "Price must be a number of 2 decimal place.")]
        public decimal Price { set; get; }

        public int? CategoryID { set; get; }
        public virtual Category Category { set; get; }

        public virtual ICollection<ProductImageMapping> ProductImageMappings { set; get; }
    }
}