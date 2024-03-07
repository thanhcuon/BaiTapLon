using BaiTapLon.Context;
using BaiTapLon.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace BaiTapLon.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        CTTDTEntities _dbContext = new CTTDTEntities();
        public ActionResult Dang_Nhap()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Dang_Nhap(User user)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra thông tin đăng nhập với cơ sở dữ liệu
                var loggedInUser = _dbContext.Users.FirstOrDefault(u => u.email == user.email && u.password == user.password);

                if (loggedInUser != null)
                {
                    // Đăng nhập thành công, thiết lập Session
                    Session["UserID"] = loggedInUser.id;
                    Session["UserName"] = loggedInUser.name;
                    Session["UserRole"] = loggedInUser.role;
                    Session["UserEmail"] = loggedInUser.email;

                    // Redirect đến trang sau khi đăng nhập thành công
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                }
            }
            return View(user);
        }
        public ActionResult Logout()
        {
            // Xóa Session hoặc thông tin đăng nhập của người dùng (ví dụ: UserID)
            Session.Clear(); // Xóa hết các Session

            // Chuyển hướng người dùng đến trang đăng nhập hoặc trang chủ
            return RedirectToAction("Dang_Nhap", "Auth"); // Điều hướng về trang đăng nhập
        }
    }
}