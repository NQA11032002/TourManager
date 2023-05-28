using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class User_information
    {
        private int _id;
        private int _user_id;
        private string _name;
        private string _birth_date;
        private int _gender;
        private string _address;
        private string _phone;
        private string _education;
        private string _image;
        private int _is_login;
        private string _created_at;
        private string _updated_at;

        public int Id { get => _id; set => _id = value; }
        public int User_id { get => _user_id; set => _user_id = value; }
        public string Name { get => _name; set => _name = value; }
        public string Birth_date { get => _birth_date; set => _birth_date = value; }
        public int Gender { get => _gender; set => _gender = value; }
        public string Address { get => _address; set => _address = value; }
        public string Phone { get => _phone; set => _phone = value; }
        public string Education { get => _education; set => _education = value; }
        public string Image { get => _image; set => _image = value; }
        public int Is_login { get => _is_login; set => _is_login = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
    }
}