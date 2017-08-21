using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoblerAPI.Models {
    public class User {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public string FavoriteTeam { get; set; }
    }
}