using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoblerAPI.Models {
    public class UserGroups {
        public User User { get; set; }
        public List<Group> Groups { get; set; }
    }
}