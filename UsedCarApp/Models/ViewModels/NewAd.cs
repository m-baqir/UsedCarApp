using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedCarApp.Models.ViewModels
{
    public class NewAd
    {
        public IEnumerable<Car> AllCars { get; set; }
        public IEnumerable<UserDto> AllUsers { get; set; }
    }
}