using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsedCarApp.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }
        public int Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
    }

    


}