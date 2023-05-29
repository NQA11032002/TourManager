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

        public UserController()
        {
            ListUser = new List<User_information>();

        }

        // GET: User
        public ActionResult Index(string keyword = null)
        {
            string query = "SELECT i.id, i.user_id, i.name, i.birth_date, i.gender, i.address, i.phone, i.education, i.image, i.is_login, i.created_at, i.updated_at, users.email, users.status, users.role_id" +
                            " FROM `user_information` as i join users on i.user_id = users.id";

            //if keyword search exists perform search with field title or description of the tour
            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                query += " WHERE i.name like '%" + keyword + "%' or i.id = " + keyword;
            }

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
                        Birth_date = String.Format("{0:dd-MM-yyyy}", row["birth_date"].ToString()),
                        Gender = Int32.Parse(row["gender"].ToString()),
                        Address = row["address"].ToString(),
                        Phone = row["phone"].ToString(),
                        Education = row["education"].ToString(),
                        Image = row["image"].ToString(),
                        Is_login = Int32.Parse(row["is_login"].ToString()),
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                        User = new Users { Email = row["email"].ToString(), Role_id = Int32.Parse(row["role_id"].ToString()), Status = Int32.Parse(row["status"].ToString()) }
                    };

                    ListUser.Add(user);
                }
            }

            return View(ListUser);
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
    }
}