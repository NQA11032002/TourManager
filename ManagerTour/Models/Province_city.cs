using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Province_city
    {
        private string _matp;
        private string _name;
        private string _type;
        private string _slug;

        public string Matp { get => _matp; set => _matp = value; }
        public string Name { get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }
        public string Slug { get => _slug; set => _slug = value; }
    }
}