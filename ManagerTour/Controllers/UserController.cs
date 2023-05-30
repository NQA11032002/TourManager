using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class UserController : Controller
    {
        private List<User_information> _listUser;
        public List<User_information> ListUser { get => _listUser; set => _listUser = value; }

        //Pagination for table user
        private int pageSize = 16;
        private int currentPage = 1;
        private float totalPage = 0;

        public UserController()
        {
            ListUser = new List<User_information>();

            totalUsers();
        }

        // GET: User
        public ActionResult Index(string keyword = null, string filter = "0")
        {
            string query = "SELECT i.id, i.user_id, i.name, i.birth_date, i.gender, i.address, i.phone, i.education, i.image, i.is_login, i.created_at, i.updated_at, users.email, users.status, users.role_id" +
                            " FROM `user_information` as i join users on i.user_id = users.id";

            //if filter exists perform filter value from database
            if(!string.IsNullOrEmpty(filter) && !string.IsNullOrWhiteSpace(filter))
            {
                switch (filter)
                {
                    case "1":
                        query += " WHERE users.status = 1";
                        break;
                    case "2":
                        query += " WHERE users.status = 0";
                        break;
                    case "3":
                        query += " WHERE i.is_login = 1";
                        break;
                    case "4":
                        query += " WHERE i.is_login = 0";
                        break;
                    default: break;
                }
            }

            //if keyword search exists perform search with field name
            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                query += " and i.name like '%" + keyword + "%'";
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
                    User_information user = new User_information
                    {
                        Id = Int32.Parse(row["id"].ToString()),
                        User_id = Int32.Parse(row["user_id"].ToString()),
                        Name = row["name"].ToString(),
                        Birth_date = String.Format("{0:yyyy-MM-dd}", row["birth_date"]),
                        Gender = Int32.Parse(row["gender"].ToString()),
                        Address = row["address"].ToString(),
                        Phone = row["phone"].ToString(),
                        Education = row["education"].ToString(),
                        Image = row["image"].ToString(),
                        Is_login = Int32.Parse(row["is_login"].ToString()),
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                        User = new Users { Email = row["email"].ToString(), Role_id = Int32.Parse(row["role_id"].ToString()), Status = Int32.Parse(row["status"].ToString()) },
                        TotalPage = totalPage,
                        CurrentPage = currentPage,
                    };

                    ListUser.Add(user);
                }
            }

            //save value into viewbag to show client after search or filter
            ViewBag.keyword = keyword;
            ViewBag.filter = filter;

            return View(ListUser);
        }


        //get view detail
        public ActionResult Detail(int id)
        {
            string query = "SELECT * FROM user_information WHERE id = " + id;
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
                        Gender = Int32.Parse(row["gender"].ToString()),
                        Address = row["address"].ToString(),
                        Phone = row["phone"].ToString(),
                        Education = row["education"].ToString(),
                        Image = row["image"].ToString(),
                        Is_login = Int32.Parse(row["is_login"].ToString()),
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                    };


                    ListUser.Add(user);
                }

            }

            return View(ListUser);
        }

        //get total users
        public void totalUsers()
        {
            string query = "SELECT * FROM user_information";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                //Ceiling làm tròn lên số nguyên gần nhất một giá trị số thập phân.
                totalPage = (int)Math.Ceiling((double)dt.Rows.Count / pageSize);

                if (totalPage <= 0)
                {
                    totalPage = 1;
                }
            }
        }


        //update information of the user 
        public ActionResult Update(int id, string birthDay, int gender, List<User_information> user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string query = "UPDATE `user_information` SET `name`='"+user[0].Name+"',`birth_date`='"+birthDay+"',`gender`='"+gender+ "',`address`='" + user[0].Address + "'," +
                        "`phone`='" + user[0].Phone + "',`education`='" + user[0].Education + "', `updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "' WHERE id = " + id;
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


        //delete user by ID
        public ActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM `user_information` WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        //lock user by ID
        public ActionResult Lock(int id)
        {
            try
            {
                string query = "UPDATE users SET status = 0 WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        //lock user by ID
        public ActionResult Unlock(int id)
        {
            try
            {
                string query = "UPDATE users SET status = 1 WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        //pagination next page
        public ActionResult NextPage()
        {
            //get currentPage from TempData if it exists
            if (TempData["currentPage"] != null)
            {
                currentPage = (int)TempData["currentPage"];
            }

            if (currentPage < totalPage)
            {
                currentPage++;

                //save currentPage into TempData
                TempData["currentPage"] = currentPage;
            }
            else
            {
                //save currentPage into TempData when last Page
                TempData["currentPage"] = currentPage;
            }

            Index();

            return View("Index", ListUser);
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

            return View("Index", ListUser);
        }
    }
}