using System.Linq;
using System.Web.Mvc;
using QLBH_LeatherNotebooksShopApp.Models;
using System.Globalization;


namespace QLBH_LeatherNotebooksShopApp.Controllers
{
    public class HomeController : Controller
    {
        private QLBH_LeatherNotebooksShopAppEntities db = new QLBH_LeatherNotebooksShopAppEntities();

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }
        public ActionResult Details(int id)
        {
            var product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [HttpGet]
        public JsonResult Search(string query)
        {
            var products = db.Products
                .Where(p => p.NamePro.Contains(query))
                .Select(p => new
                {
                    p.ProductID,
                    p.NamePro,
                    p.Price
                })
                .ToList();

            return Json(products, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SetProducts()
        {
            // Lọc danh mục có CategoryID = 4
            var products = db.Products.Where(p => p.CategoryID == 6).ToList();

            // Đếm số sản phẩm
            ViewBag.ProductCount = products.Count;

            // Trả danh sách sản phẩm qua View
            return View(products);
        }
        public ActionResult BiaProducts()
        {
            // Lọc danh mục có CategoryID = 4
            var products = db.Products.Where(p => p.CategoryID == 2).ToList();

            // Đếm số sản phẩm
            ViewBag.ProductCount = products.Count;

            // Trả danh sách sản phẩm qua View
            return View(products);
        }
        public ActionResult RuotProducts()
        {
            // Lọc danh mục có CategoryID = 4
            var products = db.Products.Where(p => p.CategoryID == 3).ToList();

            // Đếm số sản phẩm
            ViewBag.ProductCount = products.Count;

            // Trả danh sách sản phẩm qua View
            return View(products);
        }
        public ActionResult PhuKienProducts()
        {
            // Lọc danh mục có CategoryID = 4
            var products = db.Products.Where(p => p.CategoryID == 4).ToList();

            // Đếm số sản phẩm
            ViewBag.ProductCount = products.Count;

            // Trả danh sách sản phẩm qua View
            return View(products);
        }
    }
}
