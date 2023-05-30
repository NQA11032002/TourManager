using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Town
    {
        private string _xaid;
        private string _name;
        private string _type;
        private string _maqh;

        public string Xaid { get => _xaid; set => _xaid = value; }
        public string Name { get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }
        public string Maqh { get => _maqh; set => _maqh = value; }
    }
}