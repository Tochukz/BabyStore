using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BabyStore.DAL;
using BabyStore.Models;
using BabyStore.ViewModels;
using PagedList;
namespace BabyStore.Controllers
{
    public class ProductsController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Products
        public ActionResult Index(string category, string search, string sortby, int? page)
        {
            ProductIndexViewModel viewModel = new ProductIndexViewModel();
            var products = db.Products.Include(p => p.Category);                   

            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p =>
                    p.Name.Contains(search) ||
                    p.Description.Contains(search) ||
                    p.Category.Name.Contains(search)
                );
                //ViewBag.Search = search;
                viewModel.Search = search;
            }

            //Group search reults iinto categories and count how many items in ech category.
            viewModel.CatsWithCount = from matchingProducts in products
                                      where
                                      matchingProducts.CategoryID != null
                                      group matchingProducts by
                                      matchingProducts.Category.Name into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };
            //var categories = products.OrderBy(p => p.Category.Name).Select(p => p.Category.Name).Distinct();

            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                viewModel.Category = category;
            }
           
            switch(sortby)
            {
                case "lowest_price":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "highest_price":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name); //Because PagedLIst requires the list it receives to be sorted.
                    break;
            }

            //ViewBag.Category = new SelectList(categories);
            //viewModel.Products = products;
           
            int currentPage = (page ?? 1);   
            viewModel.Products = products.ToPagedList(currentPage, Constants.ItemsPerPage);
            viewModel.SortBy = sortby;
            viewModel.Sorts = new Dictionary<string, string>
            {
                { "Highest Price", "highest_price"},
                {  "Lowest Price" , "lowest_price"}
            };

            //return View(products.ToList());
            return View(viewModel);
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name");
            viewModel.ImageLists = new List<SelectList>();
            for(int i=0; i<Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            Product product = new Product();
            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.CategoryID = viewModel.CategoryID;
            product.ProductImageMappings = new List<ProductImageMapping>();
            //Get a list of selectd images without any blanks
            string[] productImages = viewModel.ProductImages.Where(pi => !String.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < productImages.Length; i++)
            {
                product.ProductImageMappings.Add(new ProductImageMapping { 
                    ProductImage = db.ProductImages.Find(int.Parse(productImages[i])),
                    ImageNumber = i
                });
            }

            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();
            for(int i=0; i<Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", viewModel.ProductImages[i]));
            }
            
            return View(viewModel);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();
            foreach(var imageMapping in product.ProductImageMappings.OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName", imageMapping.ProductImageID));
            }
            
            for(int i = viewModel.ImageLists.Count; i<Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(db.ProductImages, "ID", "FileName"));
            }

            viewModel.ID = product.ID;
            viewModel.Name = product.Name;
            viewModel.Description = product.Description;
            viewModel.Price = product.Price;

            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            Product productToUpdate = db.Products.Include(p => p.ProductImageMappings).Where(p => p.ID == viewModel.ID).Single();
            if (TryUpdateModel(productToUpdate, "", new string[] { "Name", "Description", "Price", "CategoryID"})){
                if(productToUpdate.ProductImageMappings == null)
                {
                    productToUpdate.ProductImageMappings = new List<ProductImageMapping>();
                }
                //get a list of selected images without any blanks
                string[] productImages = viewModel.ProductImages.Where(pi => !String.IsNullOrEmpty(pi)).ToArray();
                for(int i=0; i<productImages.Length; i++)
                {
                    //get the image currently stored
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //find the new image
                    var image = db.ProductImages.Find(int.Parse(productImages[i]));
                    
                    if(imageMappingToEdit == null)
                    { //if there is nothing stored then we need to add a new mapping
                      //add image to  the imagemappings
                        productToUpdate.ProductImageMappings.Add(new ProductImageMapping
                        { 
                            ImageNumber = i,
                            ProductImage = image,
                            ProductImageID = image.ID
                        });
                    }
                    else{
                        //It is not a new file, so edit the current mapping
                        if (imageMappingToEdit.ProductImageID != int.Parse(productImages[i]))
                        {
                            //if they are not the same
                            //assign images property f the image mapping
                            imageMappingToEdit.ProductImage = image;
                        }
                    }
                }
                //delete any other imagesmappings that the user did not include in their selection fo the product
                for(int i = productImages.Length; i<Constants.NumberOfProductImages; i++)
                {
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //if here is something stored in the mappiing
                    if(imageMappingToEdit != null)
                    {
                        //delete the record from the mapping table directly.
                        //just calling productToUpdate.ProductImageMapping.Remove(imageMappingToEdit) results in FK error
                        db.ProductImageMappings.Remove(imageMappingToEdit);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
