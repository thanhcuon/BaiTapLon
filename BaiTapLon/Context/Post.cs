//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BaiTapLon.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class Post
    {
        public int id { get; set; }
        public string name { get; set; }
        public string image_path { get; set; }
        public string image_name { get; set; }
        public string content { get; set; }
        public Nullable<int> category_id { get; set; }
        public string tac_gia { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
        public Nullable<System.DateTime> updated_at { get; set; }
        public string slug { get; set; }
        public Nullable<int> views { get; set; }
        public string filename { get; set; }
        public string filepath { get; set; }
        public Nullable<int> department_id { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Department Department { get; set; }
    }
}
