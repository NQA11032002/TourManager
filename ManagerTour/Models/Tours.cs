using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Tours
    {
        private int _id;
        private int _user_id;
        private int _vehicle_id;
        private string _title;
        private string _description;
        private string _address_start;
        private string _address_end;
        private string _date_start;
        private string _date_end;
        private double _price_tour;
        private string _detail_price_tour;
        private int _amount_customer_maximum;
        private int _amount_customer_present;
        private int _status;
        private string _created_at;
        private string _updated_at;

        public int Id { get => _id; set => _id = value; }
        public int User_id { get => _user_id; set => _user_id = value; }
        public int Vehicle_id { get => _vehicle_id; set => _vehicle_id = value; }
        public string Title { get => _title; set => _title = value; }
        public string Description { get => _description; set => _description = value; }
        public string Address_start { get => _address_start; set => _address_start = value; }
        public string Address_end { get => _address_end; set => _address_end = value; }
        public string Date_start { get => _date_start; set => _date_start = value; }
        public string Date_end { get => _date_end; set => _date_end = value; }
        public double Price_tour { get => _price_tour; set => _price_tour = value; }
        public string Detail_price_tour { get => _detail_price_tour; set => _detail_price_tour = value; }
        public int Amount_customer_maximum { get => _amount_customer_maximum; set => _amount_customer_maximum = value; }
        public int Amount_customer_present { get => _amount_customer_present; set => _amount_customer_present = value; }
        public int Status { get => _status; set => _status = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }

    }
}