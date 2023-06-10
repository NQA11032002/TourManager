using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ManagerTour.Models
{
    public class tour_picture
    {
        private int _id;
        private int _tour_id;
        private string _images;
        private string _created_at;
        private string _updated_at;
        public int Id { get => _id; set => _id = value; }
        public int Tour_id { get => _tour_id; set => _tour_id = value; }
        public string Images { get => _images; set => _images = value; }
        public string Created_at { get => _created_at; set => _created_at = value; }
        public string Updated_at { get => _updated_at; set => _updated_at = value; }
    }
}