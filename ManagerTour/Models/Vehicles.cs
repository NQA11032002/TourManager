using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Vehicles
    {
        private int _id;
        private string _name;
        private string _created_at;
        private string _updated_at;
        private int _currentPage;
        private float _totalPage;
        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Tên xe du lịch không được để trống")]
        [StringLength(255, ErrorMessage = "Tên xe du lịch không vượt quá 255 ký tự")]
        public string Name { get => _name; set => _name = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public float TotalPage { get => _totalPage; set => _totalPage = value; }
    }
}