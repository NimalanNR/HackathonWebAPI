using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UserService.Models
{
    public class ComparisonModel
    {
        public string Current_user_Bike_Model { get; set; }

        public string Current_user_Mileage { get; set; }
        public string Compare_user_Bike_Model { get; set; }

        public string Compare_user_Mileage { get; set; }

        public string Performance_Message { get; set; }

    }
}