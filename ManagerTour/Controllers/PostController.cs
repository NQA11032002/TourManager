using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class PostController : Controller
    {
        private List<Posts> listPosts;

        public List<Posts> ListPosts { get => listPosts; set => listPosts = value; }

        public PostController()
        {
            ListPosts = new List<Posts>();
        }

        // GET: Post
        public ActionResult Index(string keywordInput = null)
        {
            string query = "SELECT p.id, p.updated_at, p.user_id, p.address_travel_id, p.type_travel_id, p.title, p.content, p.status, p.created_at, a.name_travel, t.name as type_name, u.name as user_name " +
                "from posts as p join address_travel as a on p.address_travel_id = a.id join type_travel as t on p.type_travel_id  = t.id join user_information as u on p.user_id = u.id";

            if (!string.IsNullOrEmpty(keywordInput))
            {
                query += " WHERE p.title like '%" + keywordInput + "%'";
            }

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                Posts post = new Posts
                {
                    Id = Int32.Parse(row["id"].ToString()),
                    User_id = Int32.Parse(row["user_id"].ToString()),
                    Address_travel_id = Int32.Parse(row["address_travel_id"].ToString()),
                    Type_travel_id = Int32.Parse(row["type_travel_id"].ToString()),
                    User = new User_information { Name = row["user_name"].ToString()},
                    Address_travel = new Address_travel { Name_travel = row["name_travel"].ToString() },
                    Type_travel = new Type_travel { Name = row["type_name"].ToString() },
                    Title = row["title"].ToString(),
                    Content = row["content"].ToString(),
                    Status = row["status"].ToString(),
                    Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                    Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                };


                ListPosts.Add(post);
            }

            return View(ListPosts);
        }

        //get view detail
        public ActionResult Detail(int id)
        {

            return View();
        }

        //delete post by ID
        public ActionResult Delete(int id)
        {
            string query = "DELETE FROM `posts` WHERE id = " + id;
            ConnectionMySQL connect = new ConnectionMySQL();
            connect.Delete(query);

            return RedirectToAction("Index");
        }
    }
}