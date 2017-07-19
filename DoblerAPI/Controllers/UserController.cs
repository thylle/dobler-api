using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoblerAPI.Models;
using DoblerAPI.Services;

namespace DoblerAPI.Controllers {
    public class UserController : ApiController {

        private UserRepository userRepository;

        public UserController () {
            this.userRepository = new UserRepository();
        }

        // GET: api/User
        //public IEnumerable<User> Get () {
        //    return userRepository.GetAllUsers();
        //}

        public UserData GetUserData (string name, string email) {
            var user = new User();
            //Change input to lowercase - to make sure that we don't have 2 f the same accounts just because of casing. 
            user.Name = name.ToLower();
            user.Email = email.ToLower();

            return userRepository.GetUserData(user);
        }

        public IEnumerable<User> GetUsersForGroup (int groupId) {
            return userRepository.GetUsersForGroup(groupId);
        }

        // POST: api/User
        public void Post ([FromBody]string value) {
        }

        // PUT: api/User/5
        public void Put (int id, [FromBody]string value) {
        }

        // DELETE: api/User/5
        public void Delete (int id) {
        }
    }
}
