using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class VehicleController : Controller
    {
        private List<Vehicles> _listVehicle;
        public List<Vehicles> ListVehicle { get => _listVehicle; set => _listVehicle = value; }

        //Pagination for table post
        private int pageSize = 15;
        private int currentPage = 1;
        private float totalPage = 0;

        public VehicleController()
        {
            ListVehicle = new List<Vehicles>();
            totalVehicles();
        }

        // GET: Vehicle
        public ActionResult Index(string keyword = null)
        {
            try
            {
                string query = "SELECT * FROM `vehicles`";

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
                        Vehicles vehicle = new Vehicles
                        {
                            Id = Int32.Parse(row["id"].ToString()),
                            Name = row["name"].ToString(),
                            Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                            Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                            TotalPage = totalPage,
                            CurrentPage = currentPage,
                        };

                        ListVehicle.Add(vehicle);
                    }
                }

                ViewBag.keyword = keyword;

                return View("Index", ListVehicle);
            }
            finally
            {

            }
        }


        //get view detail vehicle travel
        public ActionResult Detail(int id)
        {
            string query = "SELECT * FROM vehicles WHERE id = " + id;
            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Vehicles vehicle = new Vehicles
                    {
                        Id = Int32.Parse(row["id"].ToString()),
                        Name = row["name"].ToString(),
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"]),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"]),
                    };

                    ListVehicle.Add(vehicle);
                }
            }

            return View(ListVehicle);
        }

        //update information of the vehicle travel 
        public ActionResult Update(int id, List<Vehicles> listVehicle)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string query = "UPDATE `vehicles` SET `name`='" + listVehicle[0].Name + "' , `updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "' WHERE id = " + id;

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


        //get view Insert vehicle travel
        public ActionResult Insert()
        {
            return View(new Vehicles());
        }

        //perform insert vehicle travel
        public ActionResult postInsert(Type_travel type)
        {
            try
            {
                string query = "INSERT INTO `vehicles`(`name`) VALUES ('" + type.Name + "')";

                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");

            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }

        //get total vehicle
        public void totalVehicles()
        {
            string query = "SELECT * FROM vehicles";

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

        //delete vehicle by ID
        public ActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM `vehicles` WHERE id = " + id;
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

            return View("Index", ListVehicle);
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

            return View("Index", ListVehicle);
        }
    }
}