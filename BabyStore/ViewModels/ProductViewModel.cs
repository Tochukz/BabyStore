using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BabyStore.ViewModels
{
    public class ProductViewModel
    {
        public int ID { set; get; }

        [Required(ErrorMessage = "The product name is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = " Product name must be between 3 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Product name must be made up of letters and numbers only")]
        public string Name { set; get; }

        [Required(ErrorMessage = "The product description is required")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "The product description must be between 10 and 200 characters")]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s]*$", ErrorMessage = "Product description must be of letters numbers and commas")]
        [DataType(DataType.MultilineText)]
        public string Description { set; get; }

        [Required(ErrorMessage = "Product price is required")]
        [Range(0.10, 10000, ErrorMessage = "Product price must be between 0.01 and 10000")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString  = "{0:c}")]
        [RegularExpression(@"[0-9]+(\\.[0-9][0-9]?)?", ErrorMessage = "The price can only be a 2 decimal number")]
        public decimal Price { set; get; }

        [Display(Name = "Category")]
        public int CategoryID { set; get; }

        public SelectList CategoryList { set; get; }

        public List<SelectList> ImageLists { set; get; }

        //To be useds the name of the select list in the view
        public string[] ProductImages { set; get; }

    }
}