using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace ManagerTour.Controllers
{
    public class PostController : Controller
    {
        private List<Posts> listPosts;
        public List<Posts> ListPosts { get => listPosts; set => listPosts = value; }

        private List<Type_travel> listTypeTravel;
        public List<Type_travel> ListTypeTravel { get => listTypeTravel; set => listTypeTravel = value; }

        private List<Address_travel> listAddressTravel;
        public List<Address_travel> ListAddressTravel { get => listAddressTravel; set => listAddressTravel = value; }

        public PostController()
        {
            ListPosts = new List<Posts>();
            ListTypeTravel = new List<Type_travel>();
            ListAddressTravel = new List<Address_travel>();

            getTypeTravel();
            getAddressTravel();
        }

        // GET: Post
        public ActionResult Index(string keywordInput = null)
        {
            try
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
                        User = new User_information { Name = row["user_name"].ToString() },
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
            finally 
            {
               
            }
        }

        //get list type travel
        public ActionResult getTypeTravel()
        {
            string query = "SELECT * FROM type_travel";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                Type_travel type = new Type_travel
                {
                    Id = Int32.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                    Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                    Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                };


                ListTypeTravel.Add(type);
            }

            return View(ListTypeTravel);
        }

        //get list Address
        public ActionResult getAddressTravel()
        {
            string query = "SELECT * FROM address_travel";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                Address_travel address = new Address_travel
                {
                    Id = Int32.Parse(row["id"].ToString()),
                    Name_travel = row["name_travel"].ToString(),
                };

                ListAddressTravel.Add(address);
            }

            return View(ListAddressTravel);
        }

        //get view detail
        public ActionResult Detail(int id)
        {
            string query = "SELECT * FROM posts WHERE id = " + id;
            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if(dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Posts post = new Posts
                    {
                        Id = Int32.Parse(row["id"].ToString()),
                        User_id = Int32.Parse(row["user_id"].ToString()),
                        Address_travel_id = Int32.Parse(row["address_travel_id"].ToString()),
                        Type_travel_id = Int32.Parse(row["type_travel_id"].ToString()),
                        Title = row["title"].ToString(),
                        Content = row["content"].ToString(),
                        Status = row["status"].ToString(),
                        ListTypeTravel = ListTypeTravel,
                        ListAddressTravel = ListAddressTravel,
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                    };


                    ListPosts.Add(post);
                }

            }

            return View(ListPosts);
        }

        //delete post by ID
        public ActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM `posts` WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error" + e.Message, "Notifycation", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Index");
            }

        }

        //update information of the post 
        public ActionResult Update(int id, int address, int type, string title, string content)
        {
            try
            {
                string query = "UPDATE `posts` SET `address_travel_id`=" + address + " , `type_travel_id`=" + type + " , `title`='" + title + "' , `content`='" + content + "' , `updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "' WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                MessageBox.Show("Error" + e.Message, "Notifycation", MessageBoxButton.OK, MessageBoxImage.Error);
                return RedirectToAction("Detail");
            }
        }
    }
}