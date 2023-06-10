using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class RolesController : Controller
    {
        private List<Roles> _listRoles;
        public List<Roles> ListRoles { get => _listRoles; set => _listRoles = value; }

        //Pagination for table roles
        private int pageSize = 15;
        private int currentPage = 1;
        private float totalPage = 0;

        public RolesController()
        {
            ListRoles = new List<Roles>();
            totalRoles();
        }

        // GET: Roles
        public ActionResult Index(string keyword = null)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "SELECT * FROM `roles`";

                    //if search keyword != null and != empty is perform
                    if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        query += " WHERE role like '%" + keyword + "%'";
                    }

                    int totalRecords = (currentPage - 1) * pageSize;

                    query += " ORDER BY id LIMIT " + pageSize + " OFFSET " + totalRecords;

                    ConnectionMySQL connect = new ConnectionMySQL();
                    DataTable dt = new DataTable();
                    dt = connect.SelectData(query).Tables[0];

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Roles role = new Roles
                            {
                                Id = Int32.Parse(row["id"].ToString()),
                                Role = row["role"].ToString(),
                                Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                                Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                                TotalPage = totalPage,
                                CurrentPage = currentPage,
                            };

                            ListRoles.Add(role);
                        }
                    }

                    ViewBag.keyword = keyword;

                    return View("Index", ListRoles);
                }
                finally
                {

                }
            }

            return RedirectToAction("Login", "Auth");
        }


        //get total roles
        public void totalRoles()
        {
            string query = "SELECT * FROM roles";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                //làm tròn lên số nguyên gần nhất một giá trị số thập phân.
                totalPage = (int)Math.Ceiling((double)dt.Rows.Count / pageSize);

                if (totalPage <= 0)
                {
                    totalPage = 1;
                }
            }
        }


        //get view Insert roles
        public ActionResult Insert()
        {
            if (Session["user"] != null)
            {
                return View(new Roles());
            }

            return RedirectToAction("Login", "Auth");
        }

        //perform insert roles
        public ActionResult rolesInsert(Roles roles)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "INSERT INTO `roles`(`role`) VALUES ('" + roles.Role + "')";

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

        //get view detail type travel
        public ActionResult Detail(int id)
        {
            if (Session["user"] != null)
            {
                string query = "SELECT * FROM roles WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(query).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Roles role = new Roles
                        {
                            Id = Int32.Parse(row["id"].ToString()),
                            Role = row["role"].ToString(),
                            Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                            Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                            TotalPage = totalPage,
                            CurrentPage = currentPage,
                        };

                        ListRoles.Add(role);
                    }
                }

                return View(ListRoles);
            }

            return RedirectToAction("Login", "Auth");
        }

        //update information of the type travel 
        public ActionResult Update(int id, List<Roles> listRole)
        {
            if (Session["user"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string query = "UPDATE `roles` SET `role`='" + listRole[0].Role + "' , `updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "' WHERE id = " + id;

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

        //delete role by ID
        public ActionResult Delete(int id)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "DELETE FROM `roles` WHERE id = " + id;
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

            return View("Index", ListRoles);
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

            return View("Index", ListRoles);
        }
    }
}