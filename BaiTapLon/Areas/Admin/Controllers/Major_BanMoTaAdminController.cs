using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using BaiTapLon.Context;
using BaiTapLon.Helpers;
using System.Data.Entity;
using Slugify;
using PagedList;
using PagedList.Mvc;
using BaiTapLon.Areas.Admin.Controllers;

namespace BaiTapLon.Areas.Admin.Controllers
{
    public class Major_BanMoTaAdminController : Controller
    {
        // GET: Admin/Major_BanMoTaAdmin
        CTTDTEntities _dbContext = new CTTDTEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var majors = _dbContext.Majors_BanMoTa.OrderBy(c => c.id).ToPagedList(pageNumber, pageSize);

            // Truyền danh sách các bài viết đến view
            return View(majors);
        }
        [HttpGet]
        public JsonResult Search(string keyword)
        {
            try
            {
                var searchResults = _dbContext.Majors_BanMoTa
                    .Where(s => s.name.Contains(keyword) || s.id.ToString().Contains(keyword) || s.category_id.ToString().Contains(keyword))
                    .Select(s => new
                    {
                        Id = s.id,
                        Name = s.name,
                        Category_id = s.category_id,
                    })
                    .ToList();

                return Json(searchResults, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Handle the error and return an appropriate response
                return Json(new { error = "An error occurred while processing the request." }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Create()
        {
            var recursiveHelper = new RecursiveHelper(_dbContext.Categories.ToList());
            var categoryOptions = recursiveHelper.GetCategoryOptions();

            ViewBag.CategoryOptions = new HtmlString(categoryOptions); // Sử dụng HtmlString để hiển thị HTML không bị mã hóa
            var cours = _dbContext.Courses_BanMoTa.ToList(); // Lấy danh sách các departments
            ViewBag.CoursesOptions = new SelectList(cours, "id", "Name");
            return View();
        }
        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Majors_BanMoTa model, HttpPostedFileBase file, HttpPostedFileBase filename)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var slugHelper = new SlugHelper();
                    var slug = slugHelper.GenerateSlug(model.name);
                    var dataInsert = new Majors_BanMoTa
                    {
                        name = model.name,
                        content = model.content,
                        category_id = model.category_id,
                        courses_id = model.courses_id,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                        slug = model.slug
                        // Gán giá trị cho các thuộc tính khác của đối tượng Post
                    };

                    if (filename != null && filename.ContentLength > 0)
                    {
                        string folderName = "Files";
                        string filePath = FileUploadHelper.UploadFile(filename, folderName);

                        dataInsert.filename = Path.GetFileName(filePath); // Chỉ lấy tên tệp tin đã được mã hóa, không bao gồm đường dẫn đầy đủ
                        dataInsert.filepath = filePath;
                    }
                    // Thêm đối tượng Post vào DbContext và lưu vào cơ sở dữ liệu
                    _dbContext.Majors_BanMoTa.Add(dataInsert);
                    _dbContext.SaveChanges();

                    // Chuyển hướng đến trang chi tiết hoặc trang danh sách tùy theo yêu cầu của bạn
                    return RedirectToAction("Index");
                }

                // Nếu ModelState không hợp lệ, hiển thị lại form với thông báo lỗi
                var recursiveHelper = new RecursiveHelper(_dbContext.Categories.ToList());
                var categoryOptions = recursiveHelper.GetCategoryOptions();
                ViewBag.CategoryOptions = categoryOptions;
                return View(model);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần
                // Log.Error("Message: " + ex.Message + "--- Line: " + ex.StackTrace);

                // Redirect với thông báo lỗi nếu cần
                return RedirectToAction("Create");
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var major = _dbContext.Majors_BanMoTa.Find(id);
            if (major == null)
            {
                return HttpNotFound(); // Hoặc chuyển hướng đến trang lỗi nếu không tìm thấy bài viết
            }

            // Load danh sách danh mục để hiển thị trong dropdownlist (tương tự như Create)
            var recursiveHelper = new RecursiveHelper(_dbContext.Categories.ToList());
            var categoryOptions = recursiveHelper.GetCategoryOptions();
            ViewBag.CategoryOptions = new HtmlString(categoryOptions);
            var cours = _dbContext.Courses_BanMoTa.ToList(); // Lấy danh sách các departments
            ViewBag.CoursesOptions = new SelectList(cours, "id", "Name", major.courses_id); // Tạo SelectList cho DropdownList, chọn Department hiện tại
            return View(major);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Majors_BanMoTa model, HttpPostedFileBase file, HttpPostedFileBase filename)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMajor = _dbContext.Majors_BanMoTa.Find(model.id);
                    if (existingMajor == null)
                    {
                        return HttpNotFound(); // Hoặc chuyển hướng đến trang lỗi nếu không tìm thấy bài viết
                    }

                    // Cập nhật thuộc tính của bài viết từ model
                    existingMajor.name = model.name;
                    existingMajor.content = model.content;
                    existingMajor.category_id = model.category_id;
                    existingMajor.courses_id = model.courses_id;
                    existingMajor.updated_at = DateTime.Now;
                    var slugHelper = new SlugHelper();
                    existingMajor.slug = slugHelper.GenerateSlug(model.name);

                    // Xử lý tệp tin đính kèm
                    if (filename != null && filename.ContentLength > 0)
                    {
                        string folderName = "Files";
                        string filePath = FileUploadHelper.UploadFile(filename, folderName);

                        existingMajor.filename = Path.GetFileName(filePath);
                        existingMajor.filepath = filePath;
                    }
                    _dbContext.Entry(existingMajor).State = EntityState.Modified;
                    _dbContext.SaveChanges();

                    return RedirectToAction("Index"); // Hoặc chuyển hướng đến trang chi tiết hoặc trang danh sách tùy theo yêu cầu của bạn
                }

                // Nếu ModelState không hợp lệ, hiển thị lại form sửa bài viết với thông báo lỗi
                var recursiveHelper = new RecursiveHelper(_dbContext.Categories.ToList());
                var categoryOptions = recursiveHelper.GetCategoryOptions();
                ViewBag.CategoryOptions = new HtmlString(categoryOptions);
                return View(model);
            }
            catch (Exception ex)
            {
                // Xử lý lỗi (ghi log hoặc hiển thị thông báo lỗi)
                return RedirectToAction("Edit", new { id = model.id }); // Chuyển hướng về trang sửa bài viết với thông báo lỗi nếu cần
            }
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var major = _dbContext.Majors_BanMoTa.Find(id);

                if (major == null)
                {
                    return HttpNotFound(); // Hoặc trả về một mã lỗi thích hợp tùy vào yêu cầu của bạn
                }

                _dbContext.Majors_BanMoTa.Remove(major);
                _dbContext.SaveChanges();

                return Json(new { code = 200, message = "success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Ghi log lỗi và lấy thông tin dòng gây ra lỗi
                Console.WriteLine($"Message: {ex.Message}, Stack Trace: {ex.StackTrace}");
                return Json(new { code = 500, message = "fail" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}