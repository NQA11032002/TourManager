using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace ManagerTour.Controllers
{
    public class TourController : Controller
    {
        private List<Tours> _listTour;
        public List<Tours> ListTour { get => _listTour; set => _listTour = value; }

        private List<Vehicles> _listVehicle;
        public List<Vehicles> ListVehicle { get => _listVehicle; set => _listVehicle = value; }

        //Pagination for table tour
        private int pageSize = 10;
        private int currentPage = 1;
        private float totalPage = 0;

        public TourController()
        {
            ListTour = new List<Tours>();
            ListVehicle = new List<Vehicles>();

            getVehicles();
            totalTour();
        }

        // GET: Tour
        public ActionResult Index(string keyword = null)
        {
            string query = "SELECT t.id, t.user_id, t.vehicle_id, t.title, t.description, t.address_start, t.address_end, t.date_start, t.date_end, " +
                            "t.price_tour, t.detail_price_tour, t.amount_customer_maximum, t.amount_customer_present, t.status, t.created_at, u.name as userName, v.name as vehicleName, t.created_at, t.updated_at" +
                            " FROM tours as t join user_information as u on t.user_id = u.id join vehicles as v on t.vehicle_id = v.id";

            //if keyword search exists perform search with field title or description of the tour
            if(!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                query += " WHERE title like '%" + keyword + "%' or description like '%" + keyword + "%'";
            }


            int totalRecords = (currentPage - 1) * pageSize;

            query += " LIMIT " + pageSize + " OFFSET " + totalRecords;


            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if(dt.Rows.Count > 0)
            {
                foreach(DataRow row in dt.Rows)
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
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                        User = new User_information { Name = row["userName"].ToString() },
                        Vehicles = new Vehicles { Name = row["vehicleName"].ToString() },
                        TotalPage = totalPage,
                        CurrentPage = currentPage,
                    };

                    ListTour.Add(tour);
                }
            }

            ViewBag.keyword = keyword;

            return View(ListTour);
        }

        //get view detail of the tour
        public ActionResult Detail(int id)
        {
            string query = "SELECT * FROM tours WHERE id = " + id;
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
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                        ListVehicle = ListVehicle,
                    };


                    ListTour.Add(tour);
                }

            }

            return View(ListTour);
        }

        //get list Address
        public ActionResult getVehicles()
        {
            string query = "SELECT * FROM vehicles";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            foreach (DataRow row in dt.Rows)
            {
                Vehicles vehicle = new Vehicles
                {
                    Id = Int32.Parse(row["id"].ToString()),
                    Name = row["name"].ToString(),
                };

                ListVehicle.Add(vehicle);
            }

            return View(ListVehicle);
        }


        //get total tour
        public void totalTour()
        {
            string query = "SELECT * FROM tours";

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

        //delete tour by ID
        public ActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM `tours` WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        //update information of the tour 
        public ActionResult Update(int id, int status_tour, List<Tours> tour)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string query = "UPDATE `tours` SET `title`='"+tour[0].Title+ "',`description`='" + tour[0].Description + "',`price_tour`='" + tour[0].Price_tour + "'," +
                        "`detail_price_tour`='" + tour[0].Detail_price_tour + "',`amount_customer_maximum`='" + tour[0].Amount_customer_maximum + "', `amount_customer_present`='" + tour[0].Amount_customer_present + "'," +
                        "`status`='" + status_tour + "',`updated_at`='"+DateTime.Now.ToString("yyyy-MM-dd H:m:s")+"'" +
                        " WHERE id = " + id;

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

            return View("Index", ListTour);
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

            return View("Index", ListTour);
        }
    }
}