using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoblerAPI.Models {
    public class Group {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Private { get; set; }
        public bool UserIsMember { get; set; }
    }
}