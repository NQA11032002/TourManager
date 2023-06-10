using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Post_comments
    {
        private int _id;
        private int _post_id;
        private int _user_id;
        private string _content;
        private string _created_at;
        private string _updated_at;

        public int Id { get => _id; set => _id = value; }
        public int Post_id { get => _post_id; set => _post_id = value; }
        public int User_id { get => _user_id; set => _user_id = value; }
        public string Content { get => _content; set => _content = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
    }
}