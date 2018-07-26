using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BabyStore.Models;
using System.Web.Mvc;

namespace BabyStore.ViewModels
{
    public class ProductIndexViewModel
    {
        public string Search { set; get; }
        public IQueryable<Product> Products { set; get; }
        public IEnumerable<CategoryWithCount> CatsWithCount { set; get; }
        //To be used as the name of the select control.
        public string Category { set; get; }
        public IEnumerable<SelectListItem> CatFilterItems
        {
            get
            {
                var allCats = CatsWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CatNameWithCount
                });
                return allCats;
            }
        }
    }

    public class CategoryWithCount
    {        
        public int ProductCount { set; get; }
        public string CategoryName { set; get; }
        public string CatNameWithCount
        {
            get
            {
                return CategoryName + " (" + ProductCount.ToString() + ")";
            }
        }
    }
}