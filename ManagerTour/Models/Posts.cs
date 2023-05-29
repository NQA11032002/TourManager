using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Posts
    {
        private int _id;
        private int _user_id;
        private int _address_travel_id;
        private int _type_travel_id;
        private string _title;
        private string _content;
        private int _status;
        private string _created_at;
        private string _updated_at;
        private int _currentPage;
        private float _totalPage;

        private User_information _user_information;
        private Address_travel _address_travel;
        private Type_travel _type_travel;

        private List<Type_travel> listTypeTravel;
        public List<Type_travel> ListTypeTravel { get => listTypeTravel; set => listTypeTravel = value; }

        private List<Address_travel> listAddressTravel;
        public List<Address_travel> ListAddressTravel { get => listAddressTravel; set => listAddressTravel = value; }

        public int Id { get => _id; set => _id = value; }
        public int User_id { get => _user_id; set => _user_id = value; }
        public int Address_travel_id { get => _address_travel_id; set => _address_travel_id = value; }
        public int Type_travel_id { get => _type_travel_id; set => _type_travel_id = value; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(70, ErrorMessage = "Tiêu đề không được quá 70 ký tự")]
        public string Title { get => _title; set => _title = value; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get => _content; set => _content = value; }
        public int Status { get => _status; set => _status = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
        public User_information User { get => _user_information; set => _user_information = value; }
        public Address_travel Address_travel { get => _address_travel; set => _address_travel = value; }
        public Type_travel Type_travel { get => _type_travel; set => _type_travel = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public float TotalPage { get => _totalPage; set => _totalPage = value; }
    }
}