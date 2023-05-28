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
        private string _created_at;
        private string _update_at;

        public int Id { get => _id; set => _id = value; }
        public int Role_id { get => _role_id; set => _role_id = value; }
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
        public int Status { get => _status; set => _status = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Update_at { get => _update_at; set => _update_at = value; }
    }
}