using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoblerAPI.Models {
    public class UserGroup : Group {
        public int Bank { get; set; }
        public List<Coupon> Coupons { get; set; }
    }
}