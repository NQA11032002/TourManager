using ManagerTour.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ManagerTour.Controllers
{
    public class PostController : Controller
    {

        // GET: Post
        public ActionResult Index()
        {
            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData("SELECT * from tours").Tables[0];


            return View(dt);
        }

        public ActionResult getPosts()
        {

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData("SELECT * from vehicles").Tables[0];

            if (dt != null)
            {
                return View(dt);
            }

            return null;
        }
    }
}