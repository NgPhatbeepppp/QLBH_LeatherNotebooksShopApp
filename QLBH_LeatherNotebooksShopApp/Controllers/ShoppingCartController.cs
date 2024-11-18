using System.Web.Mvc;
using QLBH_LeatherNotebooksShopApp.Models;

namespace QLBH_LeatherNotebooksShopApp.Controllers
{
    public class ShoppingCartController : Controller
    {
        private QLBH_LeatherNotebooksShopAppEntities db = new QLBH_LeatherNotebooksShopAppEntities();

        // Helper function to get the cart from the session
        private Cart GetCart()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public ActionResult ShowCart()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public JsonResult AddToCart(int productId, int quantity = 1)
        {
            var product = db.Products.Find(productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddToCart(product, quantity);
                return Json(new { success = true, itemCount = cart.GetTotalQuantity() });
            }
            return Json(new { success = false, message = "Product not found" });
        }

        public ActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            cart.RemoveFromCart(productId);
            return RedirectToAction("ShowCart");
        }

        public ActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart(); 
            cart.UpdateQuantity(productId, quantity); // Cập nhật số lượng cho sản phẩm
            return RedirectToAction("ShowCart"); // Quay lại trang giỏ hàng
        }

    }
}
