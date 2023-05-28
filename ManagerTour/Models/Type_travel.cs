using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Type_travel
    {
        private int _id;
        private string _name;
        private string created_at;
        private string updated_at;
        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Created_at { get => created_at; set => created_at = value; }
        public string Updated_at { get => updated_at; set => updated_at = value; }
    }
}