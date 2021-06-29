using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedCarApp.Models.ViewModels
{
    public class UpdateAd
    {
        public Ad SelectedAd { get; set; }

        public IEnumerable<Car> CarOptions { get; set; }

        public IEnumerable<User> UserOptions { get; set; }
    }
}