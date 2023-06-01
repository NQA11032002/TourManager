using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Users
    {
        private int _id;
        private int _role_id;
        private string _email;
        private string _password;
        private int _status;
        private string _otp;
        private string _expire;
        private string _created_at;
        private string _update_at;
        private int _currentPage;
        private float _totalPage;
        public int Id { get => _id; set => _id = value; }
        public int Role_id { get => _role_id; set => _role_id = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
        public int Status { get => _status; set => _status = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Update_at { get => _update_at; set => _update_at = value; }
        public float TotalPage { get => _totalPage; set => _totalPage = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public string Otp { get => _otp; set => _otp = value; }
        public string Expire { get => _expire; set => _expire = value; }
    }
}