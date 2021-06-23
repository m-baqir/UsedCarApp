using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UsedCarApp.Models.ViewModels
{
    public class DetailsUser
    {
        public UserDto SelectedUser { get; set; }

        public IEnumerable<AdDto> RelatedAds { get; set; }
    }
}