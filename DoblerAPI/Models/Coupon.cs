using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoblerAPI.Models {
    public class Coupon {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public DateTime Created { get; set; }
        public bool Done { get; set; }
        public bool Won { get; set; }
        public int Amount { get; set; }
        public float TotalReturns { get; set; }
    }
}