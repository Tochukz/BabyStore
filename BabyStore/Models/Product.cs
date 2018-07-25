//using System.ComponentModel.DataAnnotations;
namespace BabyStore.Models
{
    public partial class Product
    {
        public int ID { set; get; }
        //[Display(Name = "Product Name")]
        public string Name { set; get; }
        public string Description { set; get; }
        public decimal Price { set; get; }
        public int? CategoryID { set; get; }
        public virtual Category Category { set; get; }
    }
}