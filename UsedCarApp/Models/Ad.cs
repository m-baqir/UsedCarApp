using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsedCarApp.Models
{
    public class Ad
    {
        [Key]
        public int AdId { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public float Price { get; set; }
        public int Km { get; set; }

        //An ad has one car in it but a car can have many ads
        [ForeignKey("Car")]
        public int CarId { get; set; }
        public virtual Car Car { get; set; }

        //A user can have multiple ads
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

    }

    public class AdDto
    {
        public int AdId { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
        public string CarMake { get; set; }
        public string CarModel { get; set; }
        public int CarYear { get; set; }        
        public float Price { get; set; }
        public int Km { get; set; }
    }
}