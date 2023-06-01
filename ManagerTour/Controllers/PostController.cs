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
        private List<Posts> _listPosts;
        public List<Posts> ListPosts { get => _listPosts; set => _listPosts = value; }

        private List<Type_travel> _listTypeTravel;
        public List<Type_travel> ListTypeTravel { get => _listTypeTravel; set => _listTypeTravel = value; }

        private List<Address_travel> _listAddressTravel;
        public List<Address_travel> ListAddressTravel { get => _listAddressTravel; set => _listAddressTravel = value; }

        //Pagination for table post
        private int pageSize = 10;
        private int currentPage = 1;
        private float totalPage = 0;

        public PostController()
        {
            ListPosts = new List<Posts>();
            ListTypeTravel = new List<Type_travel>();
            ListAddressTravel = new List<Address_travel>();

            getTypeTravel();
            getAddressTravel();
            totalPost();
        }

        // GET: Post
        public ActionResult Index(string keywordInput = null, int filter_type = 0)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "SELECT p.id, p.updated_at, p.user_id, p.address_travel_id, p.type_travel_id, p.title, p.content, p.status, p.created_at, a.name_travel, t.name as type_name, u.name as user_name " +
                                    "from posts as p join address_travel as a on p.address_travel_id = a.id join type_travel as t on p.type_travel_id  = t.id join user_information as u on p.user_id = u.id WHERE 1";

                    //if search keyword != null and != empty is perform
                    if (!string.IsNullOrEmpty(filter_type.ToString()) && !string.IsNullOrWhiteSpace(filter_type.ToString()) && filter_type != 0)
                    {
                        query += " and p.type_travel_id = " + filter_type;
                    }

                    //if search keyword != null and != empty is perform
                    if (!string.IsNullOrEmpty(keywordInput) && !string.IsNullOrWhiteSpace(keywordInput))
                    {
                        query += " and p.title like '%" + keywordInput + "%'";
                    }

                    int totalRecords = (currentPage - 1) * pageSize;

                    query += " LIMIT " + pageSize + " OFFSET " + totalRecords;

                    ConnectionMySQL connect = new ConnectionMySQL();
                    DataTable dt = new DataTable();
                    dt = connect.SelectData(query).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
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
                                TotalPage = totalPage,
                                CurrentPage = currentPage,
                                Title = row["title"].ToString(),
                                Content = row["content"].ToString(),
                                Status = Int32.Parse(row["status"].ToString()),
                                Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                                Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                            };

                            ListPosts.Add(post);
                        }
                    }

                    ViewBag.keywordInput = keywordInput;
                    ViewBag.filter_type = filter_type;

                    return View("Index", ListPosts);
                }
                finally
                {

                }
            }

            return RedirectToAction("Login", "Auth");
        }

        //get total post
        public void totalPost()
        {
            string query = "SELECT * FROM posts";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if(dt.Rows.Count > 0)
            {
                totalPage = (int)Math.Ceiling((double)dt.Rows.Count / pageSize);

                if (totalPage <= 0)
                {
                    totalPage = 1;
                }
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

            ViewBag.ListTypeTravel = ListTypeTravel;

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
            if (Session["user"] != null)
            {
                string query = "SELECT * FROM posts WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(query).Tables[0];

                if (dt.Rows.Count > 0)
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
                            Status = Int32.Parse(row["status"].ToString()),
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

            return RedirectToAction("Login", "Auth");
        }

        //delete post by ID
        public ActionResult Delete(int id)
        {
            if (Session["user"] != null)
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
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Login", "Auth");
        }

        //update information of the post 
        public ActionResult Update(int id, int address, int type, List<Posts> post)
        {
            if (Session["user"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string query = "UPDATE `posts` SET `address_travel_id`=" + address + " , `type_travel_id`=" + type + " , `title`='" + post[0].Title + "' , `content`='" + post[0].Content + "' , `updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "' WHERE id = " + id;
                        ConnectionMySQL connect = new ConnectionMySQL();
                        connect.ExecuteNonQuery(query);

                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Detail");
                    }
                }
                catch (Exception e)
                {
                    return RedirectToAction("Detail");
                }
            }

            return RedirectToAction("Login", "Auth");
        }

        //pagination next page
        public ActionResult NextPage()
        {      
            //get currentPage from TempData if it exists
            if(TempData["currentPage"] != null)
            {
                currentPage = (int)TempData["currentPage"];
            }

            if (currentPage < totalPage)
            {
                currentPage++;

                //save currentPage into TempData
                TempData["currentPage"] = currentPage;
            }else
            {
                //save currentPage into TempData when last Page
                TempData["currentPage"] = currentPage;
            }

            Index();

            return View("Index", ListPosts);
        }

        //pagination previous page
        public ActionResult PreviousPage()
        {
            if (TempData["currentPage"] != null)
            {
                currentPage = (int)TempData["currentPage"];
            }

            if (currentPage > 1)
            {
                currentPage--;

                //save currentPage into TempData
                TempData["currentPage"] = currentPage;
            }

            Index();

            return View("Index", ListPosts);
        }
    }
}