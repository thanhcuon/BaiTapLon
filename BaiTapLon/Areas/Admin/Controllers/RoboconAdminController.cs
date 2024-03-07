﻿using System;
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
    public class RoboconAdminController : Controller
    {
        // GET: Admin/RoboconAdmin
        CTTDTEntities _dbContext = new CTTDTEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var robocons = _dbContext.Robocons.OrderBy(c => c.id).ToPagedList(pageNumber, pageSize);

            // Truyền danh sách các bài viết đến view
            return View(robocons);
        }
        [HttpGet]
        public JsonResult Search(string keyword)
        {
            try
            {
                var searchResults = _dbContext.Robocons
                    .Where(s => s.name.Contains(keyword) || s.id.ToString().Contains(keyword) || s.image_path.Contains(keyword) || s.category_id.ToString().Contains(keyword) || s.tac_gia.Contains(keyword))
                    .Select(s => new
                    {
                        Id = s.id,
                        Name = s.name,
                        Image_path = s.image_path,
                        Category_id = s.category_id,
                        TacGia = s.tac_gia
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
            return View();
        }

        // POST: Post/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Robocon model, HttpPostedFileBase file, HttpPostedFileBase filename)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var slugHelper = new SlugHelper();
                    var slug = slugHelper.GenerateSlug(model.name);
                    var dataInsert = new Robocon
                    {
                        name = model.name,
                        content = model.content,
                        category_id = model.category_id,
                        tac_gia = model.tac_gia,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now,
                        views = 0, // Mặc định là 0 khi tạo mới
                        slug = model.slug
                        // Gán giá trị cho các thuộc tính khác của đối tượng Post
                    };

                    // Kiểm tra và xử lý hình ảnh
                    if (file != null && file.ContentLength > 0)
                    {
                        string folderName = "Robocons"; // Thay post bằng tên thư mục bạn muốn lưu ảnh vào

                        // Mã hóa tên tệp ảnh
                        string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string fileExtension = Path.GetExtension(file.FileName);
                        string encryptedFileName = EncryptionHelper.HashString(originalFileName + DateTime.Now.ToString());
                        string fileNameHash = encryptedFileName + fileExtension;

                        dataInsert.image_name = fileNameHash;
                        dataInsert.image_path = StorageImageHelper.StorageImageUpload(file, folderName, fileNameHash);

                    }
                    if (filename != null && filename.ContentLength > 0)
                    {
                        string folderName = "Files";
                        string filePath = FileUploadHelper.UploadFile(filename, folderName);

                        dataInsert.filename = Path.GetFileName(filePath); // Chỉ lấy tên tệp tin đã được mã hóa, không bao gồm đường dẫn đầy đủ
                        dataInsert.filepath = filePath;
                    }
                    // Thêm đối tượng Post vào DbContext và lưu vào cơ sở dữ liệu
                    _dbContext.Robocons.Add(dataInsert);
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
            var student = _dbContext.Robocons.Find(id);
            if (student == null)
            {
                return HttpNotFound(); // Hoặc chuyển hướng đến trang lỗi nếu không tìm thấy bài viết
            }

            // Load danh sách danh mục để hiển thị trong dropdownlist (tương tự như Create)
            var recursiveHelper = new RecursiveHelper(_dbContext.Categories.ToList());
            var categoryOptions = recursiveHelper.GetCategoryOptions();
            ViewBag.CategoryOptions = new HtmlString(categoryOptions);
            return View(student);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit(int id, Robocon model, HttpPostedFileBase file, HttpPostedFileBase filename)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingRobocon = _dbContext.Robocons.Find(model.id);
                    if (existingRobocon == null)
                    {
                        return HttpNotFound(); // Hoặc chuyển hướng đến trang lỗi nếu không tìm thấy bài viết
                    }

                    // Cập nhật thuộc tính của bài viết từ model
                    existingRobocon.name = model.name;
                    existingRobocon.content = model.content;
                    existingRobocon.category_id = model.category_id;
                    existingRobocon.tac_gia = model.tac_gia;
                    existingRobocon.updated_at = DateTime.Now;
                    var slugHelper = new SlugHelper();
                    existingRobocon.slug = slugHelper.GenerateSlug(model.name);

                    // Xử lý hình ảnh
                    if (file != null && file.ContentLength > 0)
                    {
                        string folderName = "Student";

                        // Mã hóa tên tệp ảnh
                        string originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                        string fileExtension = Path.GetExtension(file.FileName);
                        string encryptedFileName = EncryptionHelper.HashString(originalFileName + DateTime.Now.ToString());
                        string fileNameHash = encryptedFileName + fileExtension;

                        existingRobocon.image_name = fileNameHash;
                        existingRobocon.image_path = StorageImageHelper.StorageImageUpload(file, folderName, fileNameHash);
                    }

                    // Xử lý tệp tin đính kèm
                    if (filename != null && filename.ContentLength > 0)
                    {
                        string folderName = "Files";
                        string filePath = FileUploadHelper.UploadFile(filename, folderName);

                        existingRobocon.filename = Path.GetFileName(filePath);
                        existingRobocon.filepath = filePath;
                    }
                    _dbContext.Entry(existingRobocon).State = EntityState.Modified;
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
                var student = _dbContext.Robocons.Find(id);

                if (student == null)
                {
                    return HttpNotFound(); // Hoặc trả về một mã lỗi thích hợp tùy vào yêu cầu của bạn
                }

                _dbContext.Robocons.Remove(student);
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