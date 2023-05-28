using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Roles
    {
        private int _id;
        private string _role;
        public int Id { get => _id; set => _id = value; }
        public string Role { get => _role; set => _role = value; }
    }
}