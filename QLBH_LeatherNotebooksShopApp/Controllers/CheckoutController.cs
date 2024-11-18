using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_LeatherNotebooksShopApp.Models;

namespace QLBH_LeatherNotebooksShopApp.Controllers
{
    public class CheckoutController : Controller
    {
        private QLBH_LeatherNotebooksShopAppEntities db = new QLBH_LeatherNotebooksShopAppEntities();

        // GET: Checkout
        public ActionResult Index()
        {
            // Kiểm tra xem session Customer có tồn tại và vai trò có phải là Customer không
            if (Session["UserRole"] == null || Session["UserRole"].ToString() != "Customer")
            {
                return RedirectToAction("Login", "Customer"); // Redirect đến trang đăng nhập nếu chưa đăng nhập
            }
            return View(new CheckoutViewModel());
        }

        // POST: Checkout 
        [HttpPost]
        public ActionResult Index(CheckoutViewModel model)
        {
            var customer = (Customer)Session["Customer"];

            if (customer != null && ModelState.IsValid)
            {
                // Tạo một đối tượng đơn hàng mới
                var order = new Order
                {
                    DateOrder = DateTime.Now,
                    IDCus = customer.IDCus,
                    AddressDeliverry = model.DeliveryAddress, // Địa chỉ giao hàng nhập từ form
                    PaymentMethod = model.PaymentMethod, // Phương thức thanh toán
                    OrderStatus = "Pending",
                    Note = model.Note
                };

                db.Orders.Add(order);
                db.SaveChanges();

                // Thêm OrderDetails cho từng sản phẩm trong giỏ hàng
                var cart = (Cart)Session["Cart"];
                foreach (var item in cart.Items)
                {
                    var orderDetail = new OrderDetail
                    {
                        IDProduct = item.Product.ProductID,
                        IDOrder = order.ID,
                        Quantity = item.Quantity,
                        UnitPrice = (double?)item.Product.Price
                    };
                    db.OrderDetails.Add(orderDetail);

                    var product = db.Products.Find(item.Product.ProductID);
                    if (product != null)
                    {
                        product.Quantity -= item.Quantity; // Giảm số lượng sản phẩm trong kho
                    }
                }
                db.SaveChanges();

                // Xóa giỏ hàng sau khi thanh toán thành công
                Session["Cart"] = null;

                // Chuyển hướng đến trang thông báo đơn hàng thành công
                return RedirectToAction("OrderSuccess");
            }

            // Nếu không có khách hàng trong session hoặc model không hợp lệ, quay lại trang thanh toán
            return View(model);
        }


        public ActionResult OrderSuccess()
        {
            var customerId = (int)Session["CustomerID"];  // Hoặc lấy ID khách hàng từ session
            var order = db.Orders.FirstOrDefault(o => o.IDCus == customerId && o.OrderStatus == "Pending");  // Lấy đơn hàng đang chờ

            if (order != null)
            {
                var orderDetails = db.OrderDetails.Where(od => od.IDOrder == order.ID).ToList();  // Lấy chi tiết đơn hàng
                ViewBag.OrderDetails = orderDetails;  // Gán vào ViewBag
            }
            else
            {
                ViewBag.OrderDetails = new List<OrderDetail>();  // Nếu không tìm thấy đơn hàng, gán giá trị rỗng
            }
            return View();
        }

    }
}