using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace BabyStore.Models
{
    public class ProductImage
    {
        public int ID { set; get; }

        [Display(Name = "File")]
        [StringLength(100)]
        [Index(IsUnique = true)]
        public string FileName { set; get; }

        public virtual ICollection<ProductImageMapping> ProductImageMappings { set; get; }
    }
}