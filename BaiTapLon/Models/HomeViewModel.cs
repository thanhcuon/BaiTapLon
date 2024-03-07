using BaiTapLon.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaiTapLon.Models
{
    public class HomeViewModel
    {
        public List<Slider> Sliders { get; set; }
        public List<Category> Categories { get; set; }
        public List<Post> LatestPosts { get; set; }
        public List<Document> LatestDocuments { get; set; }
        public List<About> AboutsInCategory { get; set; }
        public List<Robocon> RoboconInCategory { get; set; }
        public List<Post> PostsInCategory { get; set; }
        public List<tbl_tuyensinh> TuyenSinhInCategory { get; set; }
        public List<student> studentsInCategory { get; set; }
        public List<Science> sciencesInCategory { get; set; }
        public List<University> universityInCategory { get; set; }
        public List<Universities_BanMoTa> BanMoTa { get; set; }
        public List<Universities_ChuongTrinhDaoTao> ChuongTrinhDaoTao { get; set; }
        public List<Cours> coursInCategory { get; set; }
        public List<Courses_BanMoTa> cours_BanMoTaInCategory { get; set; }
        public List<Courses_ChuongTrinhDaoTao> cours_ChuongTrinhDaoTaoInCategory { get; set; }
        public List<Major> majorInCategory { get; set; }
        public List<Majors_BanMoTa> major_BanMoTaInCategory { get; set; }
        public List<Majors_BanMoTa> major_ChuongTrinhDaoTaoInCategory { get; set; }
        public List<About> AboutsViewInCategory { get; set; }
        public List<Post> RandomPosts { get; set; }
        public List<tbl_tuyensinh> RandomTuyenSinh { get; set; }
        public List<student> RamdomStudents { get; set; }
        public List<Science> RamdomSciences { get; set; }
        public Post PostDetail { get; set; }
        public tbl_tuyensinh TuyenSinhDetail { get; set; }
        public Major MajorDetail { get; set; }
        public Majors_BanMoTa Major_BanMoTaDetail { get; set; }
        public Majors_BanMoTa Major_ChuongTrinhDaoTaoDetail { get; set; }
        public About AboutsDetail { get; set; }
        public student StudentsDetail { get; set; }
        public Science SciencesDetail { get; set; }
        public Document DocumentDeltail { get; set; }
        public List<Category> ChildCategories { get; set; } // Thêm danh sách ChildCategories vào ViewModel
        public Category SelectedCategory { get; set; }
        public Category ParentCategory { get; set; }
        // Thêm thuộc tính để lưu trữ các bài viết mới nhất và cũ nhất
        public List<Post> NewestPosts { get; set; }
        public List<Post> OldestPosts { get; set; }
        public List<tbl_tuyensinh> NewestTuyenSinh { get; set; }
        public List<tbl_tuyensinh> OldestTuyenSinh { get; set; }
        public List<About> NewestAbouts { get; set; }
        public List<About> OldestAbouts { get; set; }
        public List<student> NewestStudents { get; set; }
        public List<student> OldestStudents { get; set; }
        public List<Science> NewestSciences { get; set; }
        public List<Science> OldestSciences { get; set; }
        public List<Post> OtherPosts { get; set; }
        public List<PostsViewModel> PostsView { get; set; }
        public int DocumentCount { get; set; }

    }
}