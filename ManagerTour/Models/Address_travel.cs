using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        private string _created_at;
        private string _updated_at;
        private int _currentPage;
        private float _totalPage;
        private Province_city _city;
        private District _district;
        private Town _town;
        private Type_travel _type;
        private List<Province_city> _listCity;
        private List<District> _listDistrict;
        private List<Town> _listTown;
        private List<Type_travel> _listType;

        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Tên địa điểm du lịch không được để trống")]
        [StringLength(60, ErrorMessage = "Tên địa điểm du lịch dưới 60 ký tự")]
        public string Name_travel { get => _name_travel; set => _name_travel = value; }
        public string City_matp { get => _city_matp; set => _city_matp = value; }
        public string District_maqh { get => _district_maqh; set => _district_maqh = value; }
        public string Town_xaid { get => _town_xaid; set => _town_xaid = value; }
        public int Type_travel_id { get => _type_travel_id; set => _type_travel_id = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
        public float TotalPage { get => _totalPage; set => _totalPage = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public List<Province_city> ListCity { get => _listCity; set => _listCity = value; }
        public List<District> ListDistrict { get => _listDistrict; set => _listDistrict = value; }
        public List<Town> ListTown { get => _listTown; set => _listTown = value; }
        public List<Type_travel> ListType { get => _listType; set => _listType = value; }
        public Province_city City { get => _city; set => _city = value; }
        public District District { get => _district; set => _district = value; }
        public Town Town { get => _town; set => _town = value; }
        public Type_travel Type { get => _type; set => _type = value; }
    }
}