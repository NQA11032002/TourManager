using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class Roles
    {
        private int _id;
        private string _role;
        private string _created_at;
        private string _updated_at;
        private int _currentPage;
        private float _totalPage;
        public int Id { get => _id; set => _id = value; }

        [Required(ErrorMessage = "Vai trò không được để trống")]
        [StringLength(255, ErrorMessage = "Vai trò không vượt quá 255 ký tự")]
        public string Role { get => _role; set => _role = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
        public int CurrentPage { get => _currentPage; set => _currentPage = value; }
        public float TotalPage { get => _totalPage; set => _totalPage = value; }
    }
}