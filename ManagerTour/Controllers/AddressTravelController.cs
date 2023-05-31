using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class AddressTravelController : Controller
    {
        private List<Address_travel> _listAddress;
        public List<Address_travel> ListAddress { get => _listAddress; set => _listAddress = value; }

        private List<Province_city> _listCity;
        public List<Province_city> ListCity { get => _listCity; set => _listCity = value; }

        private List<District> _listDistrict;
        public List<District> ListDistrict { get => _listDistrict; set => _listDistrict = value; }

        private List<Town> _listTown;
        public List<Town> ListTown { get => _listTown; set => _listTown = value; }

        private List<Type_travel> _listType;
        public List<Type_travel> ListType { get => _listType; set => _listType = value; }

        //Pagination for table user
        private int pageSize = 15;
        private int currentPage = 1;
        private float totalPage = 0;

        public AddressTravelController()
        {
            ListAddress = new List<Address_travel>();
            ListCity = new List<Province_city>();
            ListDistrict = new List<District>();
            ListTown = new List<Town>();
            ListType = new List<Type_travel>();

            ViewBag.listCity = getListCity();
            ViewBag.listDistrict = getListDistrict();
            ViewBag.listTown = getListTown();
            ViewBag.listType = getListType();
            totalAddress();
        }

        //get list city
        public List<Province_city> getListCity()
        {
            string query = "SELECT * FROM province_city";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Province_city city = new Province_city
                    {
                        Matp = row["matp"].ToString(),
                        Name = row["name"].ToString(),
                        Type = row["type"].ToString(),
                        Slug = row["slug"].ToString(),
                    };

                    ListCity.Add(city);
                }
            }


            return ListCity;
        }

        //get list district
        public List<District> getListDistrict()
        {
            string query = "SELECT * FROM district";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    District district = new District
                    {
                        Maqh = row["maqh"].ToString(),
                        Name = row["name"].ToString(),
                        Type = row["type"].ToString(),
                        Matp = row["matp"].ToString(),
                    };

                    ListDistrict.Add(district);
                }
            }


            return ListDistrict;
        }

        //get list town
        public List<Town> getListTown()
        {
            string query = "SELECT * FROM town";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Town town = new Town
                    {
                        Xaid = row["xaid"].ToString(),
                        Name = row["name"].ToString(),
                        Type = row["type"].ToString(),
                        Maqh = row["maqh"].ToString(),
                    };

                    ListTown.Add(town);
                }
            }

            return ListTown;
        }

        //get list type
        public List<Type_travel> getListType()
        {
            string query = "SELECT * FROM type_travel";

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
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                    };

                    ListType.Add(type);
                }
            }

            return ListType;
        }

        // GET: AdressTravel
        public ActionResult Index(string keyword = null, string filter_city = "0", string filter_district = "0", string filter_town = "0", int filter_type = 0)
        {
            string query = "SELECT a.id, a.name_travel, a.city_matp, a.district_maqh, a.town_xaid, a.type_travel_id, c.name as city, t.name as town, d.name as district, v.name as type, a.created_at, a.updated_at " +
                            "FROM `address_travel` as a " +
                            "join province_city as c on a.city_matp = c.matp " +
                            "join district as d on a.district_maqh = d.maqh " +
                            "join town as t on a.town_xaid = t.xaid " +
                            "join type_travel as v on a.type_travel_id = v.id WHERE 1";

            //if filter exists perform filter value from database
            if (!string.IsNullOrEmpty(filter_city) && !string.IsNullOrWhiteSpace(filter_city) && filter_city != "0")
            {
                query += " and a.city_matp like '" + filter_city + "'";
            }

            if (!string.IsNullOrEmpty(filter_district) && !string.IsNullOrWhiteSpace(filter_district) && filter_district != "0")
            {
                query += " and a.district_maqh like '" + filter_district + "'";
            }

            if (!string.IsNullOrEmpty(filter_town) && !string.IsNullOrWhiteSpace(filter_town) && filter_town != "0")
            {
                query += " and a.town_xaid like '" + filter_town + "'";
            }

            if (!string.IsNullOrEmpty(filter_type.ToString()) && !string.IsNullOrWhiteSpace(filter_type.ToString()) && filter_type.ToString() != "0")
            {
                query += " and a.type_travel_id = " + filter_type + "";
            }

            //if keyword search exists perform search with field name
            if (!string.IsNullOrEmpty(keyword) && !string.IsNullOrWhiteSpace(keyword))
            {
                query += " and a.name_travel like '%" + keyword + "%'";
            }

            int totalRecords = (currentPage - 1) * pageSize;

            query += " ORDER BY a.id LIMIT " + pageSize + " OFFSET " + totalRecords;

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Address_travel adress = new Address_travel
                    {
                        Id = Int32.Parse(row["id"].ToString()),
                        Name_travel = row["name_travel"].ToString(),
                        City_matp = row["city_matp"].ToString(),
                        District_maqh = row["district_maqh"].ToString(),
                        Town_xaid = row["town_xaid"].ToString(),
                        Type_travel_id = Int32.Parse(row["type_travel_id"].ToString()),
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                        City = new Province_city { Name = row["city"].ToString() },
                        District = new District { Name = row["district"].ToString() },
                        Town = new Town { Name = row["town"].ToString() },
                        Type = new Type_travel { Name = row["type"].ToString() },
                        TotalPage = totalPage,
                        CurrentPage = currentPage,
                    };

                    ListAddress.Add(adress);
                }
            }

            //save value into viewbag to show client after search or filter
            ViewBag.keyword = keyword;
            ViewBag.filter_city = filter_city;
            ViewBag.filter_district = filter_district;
            ViewBag.filter_town = filter_town;
            ViewBag.filter_type = filter_type;

            return View(ListAddress);
        }


        //get total address travel
        public void totalAddress()
        {
            string query = "SELECT * FROM address_travel";

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

        //get view address travel
        public ActionResult Detail(int id)
        {
            string query = "SELECT * FROM address_travel WHERE id = " + id;
            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    Address_travel address = new Address_travel
                    {
                        Id = Int32.Parse(row["id"].ToString()),
                        Name_travel = row["name_travel"].ToString(),
                        City_matp = row["city_matp"].ToString(),
                        District_maqh = row["district_maqh"].ToString(),
                        Town_xaid = row["town_xaid"].ToString(),
                        Type_travel_id = Int32.Parse(row["type_travel_id"].ToString()),
                        Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                        Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                        ListCity = ListCity,
                        ListDistrict = ListDistrict,
                        ListTown = ListTown,
                        ListType = ListType,
                    };

                    ListAddress.Add(address);
                }
            }

            return View(ListAddress);
        }

        //update information of the address travel 
        public ActionResult Update(int id, string city, string district, string town, int type, List<Address_travel> listAddress)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string query = "UPDATE `address_travel` SET `name_travel`='" + listAddress[0].Name_travel + "',`city_matp`='" + city + "',`district_maqh`='" + district + "'," +
                        "`town_xaid`='" + town + "',`type_travel_id`='" + type + "',`updated_at`='" + DateTime.Now.ToString("yyyy-MM-dd H:m:s") + "'" +
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


        //delete addres by ID
        public ActionResult Delete(int id)
        {
            try
            {
                string query = "DELETE FROM `address_travel` WHERE id = " + id;
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }
        }


        //get view Insert address travel
        public ActionResult Insert()
        {
            return View(new Address_travel());
        }

        //perform insert address travel
        public ActionResult postInsert(Address_travel address, string city, string district, string town, int type)
        {
            try
            {
                string query = "INSERT INTO `address_travel`(`name_travel`, `city_matp`, `district_maqh`, `town_xaid`, `type_travel_id`) " +
                    "VALUES ('" + address.Name_travel + "','" + city + "','" + district + "','" + town + "','" + type + "')";

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

            return View("Index", ListAddress);
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

            return View("Index", ListAddress);
        }
    }
}