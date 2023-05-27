using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Address_travel
    {
        private int _id;
        private string _name_travel;
        private string _city_matp;
        private string _district_maqh;
        private string _town_xaid;
        private int _type_travel_id;
        private DateTime _created_at;
        private DateTime _updated_at;

        public int Id { get => _id; set => _id = value; }
        public string Name_travel { get => _name_travel; set => _name_travel = value; }
        public string City_matp { get => _city_matp; set => _city_matp = value; }
        public string District_maqh { get => _district_maqh; set => _district_maqh = value; }
        public string Town_xaid { get => _town_xaid; set => _town_xaid = value; }
        public int Type_travel_id { get => _type_travel_id; set => _type_travel_id = value; }
        public DateTime Created_at { get => _created_at; set => _created_at = value; }
        public DateTime Updated_at { get => _updated_at; set => _updated_at = value; }
    }
}