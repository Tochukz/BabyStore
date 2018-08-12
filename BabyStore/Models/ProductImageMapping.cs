using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyStore.Models
{
    public class ProductImageMapping
    {
        public int ID { set; get; }
        public int ImageNumber { set; get; }
        public int ProductID { set; get; }
        public int ProductImageID { set; get; }

        public virtual Product Product { set; get; }
        public virtual ProductImage ProductImage { set; get; }
    }
}