using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBH_LeatherNotebooksShopApp.Models;
using System.Data.Entity; // Để sử dụng Include()
using System.Linq; 

namespace QLBH_LeatherNotebooksShopApp.Controllers
{
    public class OrderController : Controller
    {
        private QLBH_LeatherNotebooksShopAppEntities db = new QLBH_LeatherNotebooksShopAppEntities();

        // Hiển thị danh sách đơn hàng
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customer).Include(o => o.OrderDetails).ToList();
            return View(orders);
        }

        // Thêm đơn hàng mới
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tạo đơn hàng mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order, List<OrderDetail> orderDetails)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();

                foreach (var item in orderDetails)
                {
                    item.IDOrder = order.ID;
                    db.OrderDetails.Add(item);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // Chỉnh sửa đơn hàng
        public ActionResult Edit(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.ID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Chỉnh sửa đơn hàng
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order, List<OrderDetail> orderDetails)
        {
            if (ModelState.IsValid)
            {
                var existingOrder = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.ID == order.ID);
                if (existingOrder == null)
                {
                    return HttpNotFound();
                }

                existingOrder.AddressDeliverry = order.AddressDeliverry;
                existingOrder.PaymentMethod = order.PaymentMethod;
                existingOrder.OrderStatus = order.OrderStatus;
                existingOrder.Note = order.Note;

                // Cập nhật chi tiết đơn hàng
                existingOrder.OrderDetails.Clear();
                foreach (var item in orderDetails)
                {
                    item.IDOrder = existingOrder.ID;
                    db.OrderDetails.Add(item);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // Xóa đơn hàng
        public ActionResult Delete(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails).FirstOrDefault(o => o.ID == id);
            if (order == null)
            {
                return HttpNotFound();
            }

            db.OrderDetails.RemoveRange(order.OrderDetails);  // Xóa chi tiết đơn hàng
            db.Orders.Remove(order);  // Xóa đơn hàng
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Xem chi tiết đơn hàng
        public ActionResult Details(int id)
        {
            var order = db.Orders.Include(o => o.OrderDetails).Include(o => o.Customer)
                .FirstOrDefault(o => o.ID == id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
    }

}

