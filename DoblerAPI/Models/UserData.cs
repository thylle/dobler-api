using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoblerAPI.Models {
    public class UserData {
        public User User { get; set; }
        public List<Group> Groups { get; set; }
        public List<Coupon> Coupons { get; set; }
    }
}