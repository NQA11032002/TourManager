using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class TypeTravelController : Controller
    {
        private List<Type_travel> _listType;
        public List<Type_travel> ListType { get => _listType; set => _listType = value; }

        //Pagination for table post
        private int pageSize = 15;
        private int currentPage = 1;
        private float totalPage = 0;

        public TypeTravelController()
        {
            ListType = new List<Type_travel>();
            totalType();
        }

        // GET: TypeTravel
        public ActionResult Index(string keyword = null)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "SELECT * FROM `type_travel`";

                    //if search keyword != null and != empty is perform
                    if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
                    {
                        query += " WHERE name like '%" + keyword + "%'";
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
                            Type_travel type = new Type_travel
                            {
                                Id = Int32.Parse(row["id"].ToString()),
                                Name = row["name"].ToString(),
                                Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                                Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                                TotalPage = totalPage,
                                CurrentPage = currentPage,
                            };

                            ListType.Add(type);
                        }
                    }

                    ViewBag.keyword = keyword;

                    return View("Index", ListType);
                }
                finally
                {

                }
            }

            return RedirectToAction("Login", "Auth");
        }

        //get view detail type travel
        public ActionResult Detail(int id)
        {
            if (Session["user"] != null)
            {
                string query = "SELECT * FROM type_travel WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(query).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Type_travel type = new Type_travel
                        {
                            Id = Int32.Parse(row["id"].ToString()),
                            Name = row["name"].ToString(),
                            Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                            Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                        };

                        ListType.Add(type);
                    }
                }

                return View(ListType);
            }

            return RedirectToAction("Login", "Auth");
        }

        //update information of the type travel 
        public ActionResult Update(int id, List<Type_travel> listTypeTravel)
        {
            if (Session["user"] != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        string query = "UPDATE `type_travel` SET `name`='" + listTypeTravel[0].Name + "' , `updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "' WHERE id = " + id;

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


        //get view Insert address travel
        public ActionResult Insert()
        {
            if (Session["user"] != null)
            {
                return View(new Type_travel());
            }

            return RedirectToAction("Login", "Auth");
        }

        //perform insert address travel
        public ActionResult postInsert(Type_travel type)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "INSERT INTO `type_travel`(`name`) VALUES ('" + type.Name + "')";

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


        //get total type travel
        public void totalType()
        {
            string query = "SELECT * FROM type_travel";

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

        //delete type travel by ID
        public ActionResult Delete(int id)
        {
            if (Session["user"] != null)
            {
                try
                {
                    string query = "DELETE FROM `type_travel` WHERE id = " + id;
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

            return View("Index", ListType);
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

            return View("Index", ListType);
        }
    }
}