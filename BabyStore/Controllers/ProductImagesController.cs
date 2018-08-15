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
using System.Web.Helpers;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;

namespace BabyStore.Controllers
{
    public class ProductImagesController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: ProductImages
        public ActionResult Index()
        {
            return View(db.ProductImages.ToList());
        }

        // GET: ProductImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // GET: ProductImages/Create
        public ActionResult Upload()
        {
            return View();
        }

        // POST: ProductImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            //Logging sql statement
            //db.Database.Log = sql => Trace.WriteLine(sql);
            if(files[0] != null)
            {
                if(files.Length <= 10)
                {
                    foreach(var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles += "," + file.FileName;
                        }
                    }
                                       
                    if (allValid)
                    {
                        foreach(var file in files)
                        {
                            try
                            {
                                SaveFileToDisk(file);
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("FileName", "Sorry, an error occured while trying to upload file!");
                            }
                        }                       
                    }
                    else
                    {
                        ModelState.AddModelError("FileName", "Files must be of types: jpeg, jpg, gif or png and of size " +
                            "less than 2MB. The files "+ inValidFiles + " were not valid.");
                    }
                            
                }
                else
                {
                    ModelState.AddModelError("FileName", "You can only upload a maximum of 10 files.");
                }
            }
            else
            {
                ModelState.AddModelError("FileName", "Please select a file.");
            }

            if(ModelState.IsValid)
            {
                bool duplicates = false;
                bool otherDbError = false;
                string duplicateFiles = "";

                foreach(var file in files)
                {
                    ProductImage productToAdd = new ProductImage { FileName = file.FileName };
                    try
                    {                        
                        db.ProductImages.Add(productToAdd);
                        db.SaveChanges();
                        //When working with very large upload it better to call saveChanges only once on the DB context after all items have been added.
                    }
                    catch(DbUpdateException ex)
                    {
                        SqlException innerException = ex.InnerException.InnerException as SqlException;
                        if(innerException != null && innerException.Number == 2601)
                        {
                            duplicates = true;
                            duplicateFiles += "," + file.FileName;
                            //Remove the entry that resulted in the exception from the db context
                            db.Entry(productToAdd).State = EntityState.Detached;
                        }
                        else
                        {
                            otherDbError = true;
                        }
                    }
                }

                if (duplicates)
                {
                    ModelState.AddModelError("FileName", "Duplicates Filename(s) was/were encoutered with the following name(s): " + duplicateFiles);
                    return View();
                }else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Other DB errrors occured.s");
                    return View();
                }

                return RedirectToAction("Index");
            }
           
            return View();   
        }

        // GET: ProductImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FileName")] ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productImage);
        }

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductImage productImage = db.ProductImages.Find(id);
            if (productImage == null)
            {
                return HttpNotFound();
            }
            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductImage productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
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

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".jpg", ".jpeg", ".gif", ".png"};
            if(file.ContentLength > 0 && file.ContentLength < 2097152 && allowedFileTypes.Contains(fileExtension))
            {
                return true;
            }
            return false;
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if(img.Width > 190)
            {
                img.Resize(190, img.Height);
            }
            img.Save(Constants.ProductImagePath + file.FileName);

            if (img.Width > 100)
            {
                img.Resize(100, img.Height);
            }
            img.Save(Constants.ProductThumbnailPath + file.FileName);

        }
    }
}
