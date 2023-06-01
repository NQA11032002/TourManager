using ManagerTour.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BCryptNet = BCrypt.Net.BCrypt;

namespace ManagerTour.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Login()
        {
            HttpCookie email = Request.Cookies["email"];
            HttpCookie password = Request.Cookies["password"];

            //if exists cookie save email and password is perform login
            if (email != null && password != null)
            {
                return isLogin(email.Value.ToString(), password.Value.ToString());
            }
            else
            {
                return View();
            }
        }

        //is login
        public ActionResult isLogin(string email, string password, string cb_login = "false")
        {
            var query = "SELECT * FROM users WHERE email = '" + email + "'";

            ConnectionMySQL connect = new ConnectionMySQL();
            DataTable dt = new DataTable();
            dt = connect.SelectData(query).Tables[0];

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string passwordHash = row["password"].ToString();

                    bool passwordMatches = BCryptNet.Verify(password, passwordHash);

                    //check provide password with password hash from database return true is perform login
                    if (passwordMatches)
                    {
                        string queryUser = "SELECT i.id, i.user_id, i.name, i.birth_date, i.gender, i.address, i.phone, i.education, i.image, i.is_login, i.created_at, i.updated_at, users.email, users.status, users.role_id" +
                                        " FROM `user_information` as i join users on i.user_id = users.id";

                        dt = connect.SelectData(queryUser).Tables[0];

                        User_information user = new User_information
                        {
                            Id = Int32.Parse(dt.Rows[0]["id"].ToString()),
                            User_id = Int32.Parse(dt.Rows[0]["user_id"].ToString()),
                            Name = dt.Rows[0]["name"].ToString(),
                            Birth_date = String.Format("{0:yyyy-MM-dd}", dt.Rows[0]["birth_date"]),
                            Gender = Int32.Parse(dt.Rows[0]["gender"].ToString()),
                            Address = dt.Rows[0]["address"].ToString(),
                            Phone = dt.Rows[0]["phone"].ToString(),
                            Education = dt.Rows[0]["education"].ToString(),
                            Image = dt.Rows[0]["image"].ToString(),
                            Is_login = Int32.Parse(dt.Rows[0]["is_login"].ToString()),
                            Created_at = String.Format("{0:dd-MM-yyyy}", row["created_at"].ToString()),
                            Updated_at = String.Format("{0:dd-MM-yyyy}", row["updated_at"].ToString()),
                            User = new Users { Email = dt.Rows[0]["email"].ToString(), Role_id = Int32.Parse(dt.Rows[0]["role_id"].ToString()), Status = Int32.Parse(dt.Rows[0]["status"].ToString()) },
                        };

                        //save user information into session has key user
                        Session["user"] = user;

                        //remember information login
                        if (cb_login == "true")
                        {
                            HttpCookie emailCookie = new HttpCookie("email");
                            emailCookie.Value = email;
                            HttpCookie passwordCookie = new HttpCookie("password");
                            passwordCookie.Value = password;

                            Response.Cookies.Add(emailCookie);
                            Response.Cookies.Add(passwordCookie);
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        // Invalid login credentials
                        TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu không chính xác.!!!";
                    }
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Tài khoản không tồn tại!!!";
            }


            return RedirectToAction("Login");
        }

        public ActionResult Forgot()
        {
            return View();
        }

        //send mail get OTP
        public ActionResult sendMailOTP(string email)
        {
            try
            {
                string otp = GenerateRandomString(6);
                DateTime currentTime = DateTime.Now;
                DateTime expirationTime = currentTime.AddMinutes(10);

                string query = "UPDATE `users` SET `otp`='" + otp + "', `expire`='" + expirationTime + "' WHERE email = '" + email + "'";
                ConnectionMySQL connect = new ConnectionMySQL();
                connect.ExecuteNonQuery(query);

                return RedirectToAction("ResetPassword");
            }    
            catch
            {
                return RedirectToAction("Forgot");
            }
        }

        //reset password 
        public ActionResult ResetPassword()
        {
            return View();
        }

        //perform reset password
        public ActionResult isResetPassword(string email, string otp, string newPassword, string confirmPassword)
        {
            try
            {
                //get otp, expire...
                string selectOTP = "SELECT email, otp, expire FROM users WHERE email = '" + email + "'";
                ConnectionMySQL connect = new ConnectionMySQL();
                DataTable dt = new DataTable();
                dt = connect.SelectData(selectOTP).Tables[0];

                DateTime dateNow = DateTime.Now;

                if (string.Compare(dt.Rows[0]["otp"].ToString(), otp) == 0 && DateTime.Compare(dateNow, DateTime.Parse(dt.Rows[0]["expire"].ToString())) < 0)
                {
                    if (String.Compare(newPassword, confirmPassword) == 0)
                    {
                        string hashPassword = Crypto.HashPassword(newPassword);

                        string query = "UPDATE `users` SET `password`='" + hashPassword + "', `otp` = 'NULL', `expire` = 'NULL' WHERE email = '" + email + "'";
                        connect.ExecuteNonQuery(query);

                        return RedirectToAction("Login");
                    }
                    else
                    {
                        TempData["errorResetPassword"] = "Mật khẩu xác nhận không khớp";
                    }
                }else
                {
                    TempData["errorResetPassword"] = "Mã otp không hợp lệ";
                }

                return RedirectToAction("ResetPassword");
            }
            catch
            {
                return RedirectToAction("ResetPassword");
            }
        }

        //generate random ottp 
        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}