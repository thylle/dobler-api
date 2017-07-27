using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DoblerAPI.Models;
using DoblerAPI.Services;

namespace DoblerAPI.Controllers {
    public class DataController : ApiController {

        private UserRepository userRepository;
        private GroupRepository groupRepository;

        public DataController () {
            this.userRepository = new UserRepository();
            this.groupRepository = new GroupRepository();
        }

        

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

        [HttpGet]
        public Coupon CreateCoupon (int UserId, int GroupId, int Amount, float TotalReturns) {

            var coupon = new Coupon{
                UserId = UserId,
                GroupId = GroupId,
                Amount = Amount,
                TotalReturns = TotalReturns
            };

            return userRepository.CreateCoupon(coupon);
        }

        // GET: api/data/getAllGroups
        public IEnumerable<Group> GetAllGroups () {
            return groupRepository.GetAllGroups();
        }

        public Group GetGroup (int id) {
            return groupRepository.GetGroup(id);
        }

        [HttpGet]
        public Group CreateGroup (string name, bool isPrivate, int userId) {
            return groupRepository.CreateGroup(name, isPrivate, userId);
        }

        [HttpGet]
        public Group JoinGroup (int userId, int groupId) {
            return groupRepository.JoinGroup(userId, groupId);
        }

        [HttpGet]
        public Group LeaveGroup (int userId, int groupId) {
            return groupRepository.LeaveGroup(userId, groupId);
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
