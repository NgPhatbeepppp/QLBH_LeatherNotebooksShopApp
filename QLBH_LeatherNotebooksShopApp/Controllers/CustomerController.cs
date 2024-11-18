using System;
using System.Linq;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.Text;
using QLBH_LeatherNotebooksShopApp.Models;
using System.Collections.Generic;
using System.Net;

namespace QLBH_LeatherNotebooksShopApp.Controllers
{
    public class CustomerController : Controller
    {
        private QLBH_LeatherNotebooksShopAppEntities db = new QLBH_LeatherNotebooksShopAppEntities();

        // Helper function to hash passwords for security
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        // GET: Customer/Index
        public ActionResult Index()
        {
            // Lấy danh sách tất cả khách hàng từ cơ sở dữ liệu
            var customers = db.Customers.ToList();

            // Trả về View với danh sách khách hàng
            return View(customers);
        }
        // GET: Customer/Register
        public ActionResult Register()
        {
            var model = new Customer(); // Create a new Customer object
            return View(model); // Pass the model to the view
        }

        // POST: Customer/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = db.Customers.FirstOrDefault(c => c.Username == customer.Username || c.EmailCus == customer.EmailCus);
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập hoặc email đã tồn tại.");
                    return View(customer);
                }

                if (string.IsNullOrEmpty(confirmPassword) || customer.Password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu và xác nhận mật khẩu không khớp.");
                    return View(customer);
                }

                try
                {
                    customer.Password = HashPassword(customer.Password);
                    customer.CreatedDate = DateTime.Now;
                    customer.IsActive = true;
                    customer.Role = "Customer";
                    db.Customers.Add(customer);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Đăng ký thành công. Hãy đăng nhập.";
                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.");
                    Console.WriteLine(ex.Message); // Log lỗi ra console
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // In lỗi ra console
                }
            }
            return View(customer);
        }


        // GET: Customer/Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Customer/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string hashedPassword = HashPassword(password);
                    var customer = db.Customers.FirstOrDefault(c => c.Username == username && c.Password == hashedPassword && c.IsActive == true);

                    if (customer != null)
                    {
                        Session["CustomerID"] = customer.IDCus;
                        Session["CustomerUsername"] = customer.Username;
                        Session["UserRole"] = customer.Role; 
                        Session["Customer"] = customer;

                        if (customer.Role == "Admin")
                        {
                            return RedirectToAction("Index", "Categories");
                        }
                        else if (customer.Role == "Employee") 
                        {
                            return RedirectToAction("Index", "Categories");
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng nhập. Vui lòng thử lại.");
                    // Optional: Log the exception (ex) for further debugging.
                }
            }
            return View();
        }

        // GET: Customer/Logout
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
        // GET: Customer/AddCustomer
        public ActionResult AddCustomer()
        {
            var customer = new Customer(); // Initialize the model
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCustomer(Customer customer, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem username hoặc email có tồn tại không
                var existingCustomer = db.Customers.FirstOrDefault(c => c.Username == customer.Username || c.EmailCus == customer.EmailCus);
                if (existingCustomer != null)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập hoặc email đã tồn tại.");
                    return View(customer);
                }

                // Kiểm tra mật khẩu và xác nhận mật khẩu
                if (string.IsNullOrEmpty(confirmPassword) || customer.Password != confirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Mật khẩu và xác nhận mật khẩu không khớp.");
                    return View(customer);
                }

                try
                {
                    // Hash mật khẩu
                    customer.Password = HashPassword(customer.Password);
                    customer.CreatedDate = DateTime.Now;
                    customer.IsActive = true;

                    // Đảm bảo rằng Role được gán từ form
                    if (string.IsNullOrEmpty(customer.Role))
                    {
                        customer.Role = "Customer";  // Gán mặc định nếu không có giá trị Role
                    }

                    // Thêm khách hàng vào cơ sở dữ liệu
                    db.Customers.Add(customer);
                    db.SaveChanges();

                    // Thông báo đăng ký thành công
                    TempData["SuccessMessage"] = "Đăng ký thành công.";
                    return RedirectToAction("Index", "Customer");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Đã xảy ra lỗi trong quá trình đăng ký. Vui lòng thử lại.");
                    Console.WriteLine(ex.Message); // Log lỗi ra console
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // In lỗi ra console
                }
            }

            // Truyền danh sách Role vào View
            ViewBag.RoleList = new SelectList(new List<string> { "Customer", "Employee", "Admin" }, customer.Role);
            return View(customer);
        }
        // GET: Customer/Delete/6
        public ActionResult Delete(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customer/ConfirmDelete/6
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            var customer = db.Customers.Find(id);
            if (customer != null)
            {
                try
                {
                    db.Customers.Remove(customer);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Xóa khách hàng thành công.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Đã xảy ra lỗi khi xóa khách hàng. Vui lòng thử lại.";
                    Console.WriteLine(ex.Message); // Optional: Log the exception for debugging
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng.";
            }

            return RedirectToAction("Index");
        }


        // GET: Customer/Edit/5
        public ActionResult Edit(int id)
{
    var customer = db.Customers.Find(id);
    if (customer == null)
    {
        return HttpNotFound();
    }
    return View(customer);
}

// POST: Customer/Edit/5
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Edit(Customer customer, string newPassword, string confirmNewPassword)
{
    if (ModelState.IsValid)
    {
        try
        {
            var existingCustomer = db.Customers.Find(customer.IDCus);
            if (existingCustomer != null)
            {
                // Update the editable fields
                existingCustomer.NameCus = customer.NameCus;
                existingCustomer.PhoneCus = customer.PhoneCus;
                existingCustomer.EmailCus = customer.EmailCus;
                existingCustomer.Address = customer.Address;
                existingCustomer.Role = customer.Role;
                existingCustomer.IsActive = customer.IsActive;

                // Check if admin wants to update the password
                if (!string.IsNullOrEmpty(newPassword) && newPassword == confirmNewPassword)
                {
                    existingCustomer.Password = HashPassword(newPassword);
                }
                else if (!string.IsNullOrEmpty(newPassword) && newPassword != confirmNewPassword)
                {
                    ModelState.AddModelError("confirmNewPassword", "Mật khẩu mới và xác nhận mật khẩu không khớp.");
                    return View(customer);
                }

                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật thông tin khách hàng thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng.";
                return RedirectToAction("Index");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin khách hàng.");
            Console.WriteLine(ex.Message); // Optional: Log the exception for debugging
        }
    }
    return View(customer);
}

    }
}
