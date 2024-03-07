using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BaiTapLon.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            var userRole = Session["UserRole"] as string;
            if (userRole != "admin")
            {
                // Nếu không có quyền admin, chuyển hướng về trang chủ hoặc trang nào đó khác
                return RedirectToAction("Index", "Home", new { area = "" }); // Chuyển hướng về trang chủ
            }

            // Nếu có quyền admin, hiển thị trang quản trị
            return View();
        }
    }

}