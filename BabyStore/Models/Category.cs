using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace BabyStore.Models
{
    public class Category
    {
        public int ID { set; get; }
        [Display(Name = "Category Name")]
        public string Name { set; get; }
        public virtual ICollection<Product> Products {set; get; }
    }
}