using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ManagerTour.Controllers
{
    public class HomeController : Controller
    {
        private List<User_information> _listUser;
        public List<User_information> ListUser { get => _listUser; set => _listUser = value; }

        private List<Posts> _listPosts;
        public List<Posts> ListPosts { get => _listPosts; set => _listPosts = value; }

        private List<Tours> _listTour;
        public List<Tours> ListTour { get => _listTour; set => _listTour = value; }

        private List<Tours> _listTourGroup;
        public List<Tours> ListTourGroup { get => _listTourGroup; set => _listTourGroup = value; }

        private List<Tours> _listTourAgree;
        public List<Tours> ListTourAgree { get => _listTourAgree; set => _listTourAgree = value; }

        public HomeController()
        {
            ListUser = new List<User_information>();
            ListPosts = new List<Posts>();
            ListTour = new List<Tours>();
            ListTourGroup = new List<Tours>();
            ListTourAgree = new List<Tours>();
        }

        // GET: Home
        public ActionResult Index()
        {
            if (Session["user"] != null)
            {
                ViewBag.listUsers = getListUser();
                ViewBag.totalLogin = getTotalUserLogin();
                ViewBag.listPosts = getListPost();
                ViewBag.listTours = getListTour();
                ViewBag.listToursGroup = getListTourGroup();

                //get new tour need agree
                HttpContext.Session["ListTour"] = getListTour("0");
                return View();
            }

            return RedirectToAction("Login", "Auth");
        }

        //get list users
        public List<User_information> getListUser()
        {
            try
            {
                string query = "SELECT i.id, i.user_id, i.name, i.birth_date, i.gender, i.address, i.phone, i.education, i.image, i.is_login, i.created_at, i.updated_at, users.email, users.status, users.role_id" +
                                " FROM `user_information` as i join users on i.user_id = users.id ORDER BY i.id DESC LIMIT 7";

                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(query).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        User_information user = new User_information
                        {
                            Id = Int32.Parse(row["id"].ToString()),
                            User_id = Int32.Parse(row["user_id"].ToString()),
                            Name = row["name"].ToString(),
                            Birth_date = String.Format("{0:yyyy-MM-dd}", row["birth_date"]),
                            Image = row["image"].ToString(),
                            Is_login = Int32.Parse(row["is_login"].ToString()),
                            Created_at = String.Format("{0:yyyy-MM-dd}", row["created_at"]),
                            Updated_at = String.Format("{0:yyyy-MM-dd}", row["updated_at"]),
                            User = new Users { Email = row["email"].ToString(), Role_id = Int32.Parse(row["role_id"].ToString()), Status = Int32.Parse(row["status"].ToString()) },
                        };

                        ListUser.Add(user);
                    }
                }

                return ListUser;
            }
            catch
            {
                return ListUser;
            }

        }

        //get list tour group by created
        public List<Tours> getListTourGroup()
        {
            try
            {
                string query = "SELECT id, COUNT(*) as count, Month(created_at) as month FROM `tours` WHERE 1 GROUP BY Month(created_at)";

                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(query).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Tours tour = new Tours
                        {
                            Id = Int32.Parse(row["id"].ToString()),
                            Count = Int32.Parse(row["count"].ToString()),
                            Month = Int32.Parse(row["month"].ToString()),
                        };

                        ListTourGroup.Add(tour);
                    }
                }

                return ListTourGroup;
            }
            catch
            {
                return ListTourGroup;
            }
        }

        // GET: Post
        public List<Posts> getListPost()
        {
            try
            {
                string query = "SELECT p.id, p.updated_at, p.user_id, p.address_travel_id, p.type_travel_id, p.title, p.content, p.status, p.created_at, a.name_travel, t.name as type_name, u.name as user_name " +
                                "from posts as p join address_travel as a on p.address_travel_id = a.id join type_travel as t on p.type_travel_id  = t.id join user_information as u on p.user_id = u.id WHERE 1 ORDER BY p.id DESC";

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
                            Title = row["title"].ToString(),
                            Content = row["content"].ToString(),
                            Status = Int32.Parse(row["status"].ToString()),
                            Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                            Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                        };

                        ListPosts.Add(post);
                    }
                }

                return ListPosts;
            }
            catch
            {
                return ListPosts;
            }
        }

        // GET: Tour
        public List<Tours> getListTour(string status = null)
        {
            try
            {
                string query = "SELECT t.id, t.user_id, t.vehicle_id, t.title, t.description, t.address_start, t.address_end, t.date_start, t.date_end, " +
                                "t.price_tour, t.detail_price_tour, t.amount_customer_maximum, t.amount_customer_present, t.status, t.created_at, u.name as userName, u.image as userImage," +
                                " v.name as vehicleName, t.created_at, t.updated_at" +
                                " FROM tours as t join user_information as u on t.user_id = u.id join vehicles as v on t.vehicle_id = v.id";

                if(!string.IsNullOrEmpty(status))
                {
                    query += " WHERE t.status = "+Int32.Parse(status)+ "";
                }

                query += " ORDER BY t.id DESC";

                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(query).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Tours tour = new Tours
                        {
                            Id = Int32.Parse(row["id"].ToString()),
                            User_id = Int32.Parse(row["user_id"].ToString()),
                            Vehicle_id = Int32.Parse(row["vehicle_id"].ToString()),
                            Title = row["title"].ToString(),
                            Description = row["description"].ToString(),
                            Address_start = row["address_start"].ToString(),
                            Address_end = row["address_end"].ToString(),
                            Date_start = String.Format("{0:yyyy-MM-dd}", row["date_start"]),
                            Date_end = String.Format("{0:yyyy-MM-dd}", row["date_end"]),
                            Price_tour = double.Parse(row["price_tour"].ToString()),
                            Detail_price_tour = row["detail_price_tour"].ToString(),
                            Amount_customer_maximum = Int32.Parse(row["amount_customer_maximum"].ToString()),
                            Amount_customer_present = Int32.Parse(row["amount_customer_present"].ToString()),
                            Status = Int32.Parse(row["status"].ToString()),
                            Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                            Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                            User = new User_information { Name = row["userName"].ToString(), Image = row["userImage"].ToString() },
                            Vehicles = new Vehicles { Name = row["vehicleName"].ToString() },
                        };

                        if(string.IsNullOrEmpty(status))
                        {
                            ListTour.Add(tour);
                        }
                        else
                        {
                            ListTourAgree.Add(tour);
                        }
                    }
                }
            }
            catch
            {
            }

            if (string.IsNullOrEmpty(status))
            {
                return ListTour;
            }
            return ListTourAgree;
        }

        //get total user is login
        public int getTotalUserLogin()
        {
            int count = 0;

            if (ListUser.Count > 0)
            {
                ListUser.ForEach(p =>
                {
                    if (p.Is_login == 1)
                    {
                        count++;
                    }
                });
            }

            return count;
        }
    }
}