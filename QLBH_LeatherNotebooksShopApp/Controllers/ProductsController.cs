using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QLBH_LeatherNotebooksShopApp.Models;

namespace QLBH_LeatherNotebooksShopApp.Controllers
{
    public class ProductsController : Controller
    {
        private QLBH_LeatherNotebooksShopAppEntities db = new QLBH_LeatherNotebooksShopAppEntities();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
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
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCate");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Handle image upload
                    if (product.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(product.UploadImage.FileName);
                        string extension = Path.GetExtension(product.UploadImage.FileName);
                        filename = filename + extension;
                        product.ImagePro = "~/images/" + filename;
                        product.UploadImage.SaveAs(Path.Combine(Server.MapPath("~/images/"), filename));
                    }

                    // Add product to the database
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    // Log the error and show the view with an error message
                    // Consider logging the exception or showing an error message
                    ModelState.AddModelError("", "An error occurred while saving the product. Please try again.");
                    return View();
                }
            }

            // Populate Category dropdown
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCate", product.CategoryID);
            return View(product);
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

            // Tạo SelectList đúng cách
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCate", product.CategoryID);

            return View(product);
        }


        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (product.UploadImage != null)
                    {
                        string filename = Path.GetFileNameWithoutExtension(product.UploadImage.FileName);
                        string extension = Path.GetExtension(product.UploadImage.FileName);
                        filename = filename + extension;

                        string path = Path.Combine(Server.MapPath("~/images/"), filename);
                        product.UploadImage.SaveAs(path);

                        product.ImagePro = "~/images/" + filename;
                    }
                    else
                    {
                        db.Entry(product).Property(p => p.ImagePro).IsModified = false;
                    }

                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật sản phẩm. Vui lòng thử lại.");
                }
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "NameCate", product.CategoryID);

            return View(product);
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
        // GET: Product/AddReview
        public ActionResult AddReview(int productId)
        {
            var model = new Review { ProductId = productId };
            return View(model);
        }

        // POST: Product/AddReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(int ProductID, int Rating, string comment)
        {
            if (ModelState.IsValid)
            {
                // Assuming customer is logged in and CustomerID is stored in Session
                int customerId = (int)Session["CustomerID"];

                var review = new Review
                {
                    ProductId = ProductID,
                    CustomerId = customerId,
                    Comment = comment,
                    Rating = Rating,
                    CreatedDate = DateTime.Now
                };

                db.Reviews.Add(review);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Đánh giá của bạn đã được thêm thành công.";
                return RedirectToAction("ProductDetail", new { id = ProductID });
            }

            // If validation fails, reload the product detail view
            TempData["ErrorMessage"] = "Vui lòng kiểm tra thông tin đánh giá.";
            return RedirectToAction("ProductDetail", new { id = ProductID });
        }
        [HttpGet]
        public JsonResult Search(string query)
        {
            var products = db.Products
                .Where(p => p.NamePro.Contains(query)) // Điều kiện tìm kiếm
                .Select(p => new
                {
                    p.ProductID,
                    p.NamePro,
                    p.Price
                })
                .ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }


    }
}
