using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class District
    {
        private string _maqh;
        private string _name;
        private string _type;
        private string _matp;
        public string Maqh { get => _maqh; set => _maqh = value; }
        public string Name { get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }
        public string Matp { get => _matp; set => _matp = value; }
    }
}