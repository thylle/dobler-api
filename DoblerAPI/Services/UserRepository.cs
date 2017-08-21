using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Dapper;
using DoblerAPI.Models;

namespace DoblerAPI.Services {
    public class UserRepository {

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["doblerConnectionString"].ConnectionString);
        

        public UserData GetUserData (User user) {
            var sqlGet = "select * from Users where Email = '" + user.Email + "'";
            var response = connection.Query<User>(sqlGet).FirstOrDefault();

            //Return the user if it already exists
            if (response != null)
            {
                var userId = response.Id;
                var userGroups = GetUserGroups(userId);
                var userCoupons = GetUserCoupons(userId);
                var wins = userCoupons.Count(x => x.Won); //Count amount of coupons the user has won
                var losses = userCoupons.Count() - wins;

                user.Id = userId;
                user.Name = response.Name;
                user.Email = response.Email;
                user.Wins = wins;
                user.Losses = losses;

                var userData = new UserData {
                    User = user,
                    Groups = userGroups,
                    Coupons = userCoupons
                };

                return userData;
            }

            //If user with this email does not exists - we create a new user
            return AddUser(user);
        }

        public List<UserGroup> GetUserGroups (int userId) {
            var sqlGet = "SELECT g.Id, g.Name, g.Private, g.UserIsMember, g.AdminId, ug.Bank " +
                         "FROM User_Groups ug INNER JOIN Groups g ON g.Id = ug.GroupId WHERE ug.UserId = '" + userId + "'";
            var response = connection.Query<UserGroup>(sqlGet).ToList();

            if (response.Any()){
                foreach (var item in response){
                    item.Coupons = GetUserGroupCoupons(item.Id);
                }
            }

            return response;
        }

        public List<Coupon> GetUserCoupons (int userId) {
            var sqlGet = "SELECT * FROM Coupons WHERE UserId = '" + userId + "' ORDER BY Created DESC";
            var response = connection.Query<Coupon>(sqlGet);

            return response.ToList();
        }

        public List<Coupon> GetUserGroupCoupons (int groupId) {
            var sqlGet = "SELECT * FROM Coupons WHERE GroupId = '" + groupId + "' ORDER BY Created DESC";
            var response = connection.Query<Coupon>(sqlGet);

            return response.ToList();
        }



        public UserData AddUser (User user) {
            var sqlAdd = "INSERT INTO Users(Name, Email) VALUES('" + user.Name + "', '" + user.Email + "')";

            SqlCommand command = new SqlCommand(sqlAdd, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();

            //After we added the user, we get and return that new user.
            var sqlGet = "select * from Users where Email = '" + user.Email + "'";
            var response = connection.Query<User>(sqlGet).FirstOrDefault();
            if (response != null){
                var newUser = new UserData(){
                    User   = response,
                    Groups = null,
                    Coupons = null
                };

                return newUser;
            }

            return null;
        }

        public Coupon CreateCoupon (Coupon coupon) {

            coupon.Created = DateTime.Now;

            var sqlAdd = "INSERT INTO Coupons(UserId, GroupId, Created, Amount, TotalReturns) " +
                         "VALUES('" + coupon.UserId + "', '" + coupon.GroupId + "', '"  + coupon.Created + "', '" + coupon.Amount + "', '" + coupon.TotalReturns + "')";

            SqlCommand command = new SqlCommand(sqlAdd, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();

            return coupon;
        }

        public List<UserInGroup> GetUsersForGroup (int groupId) {

            var userList = new List<UserInGroup>();
            var sqlGet = "select u.Id, u.Name, ug.Bank from User_Groups ug INNER JOIN Users u ON u.Id = ug.UserId WHERE ug.GroupId = '" + groupId + "'";
            var response = connection.Query<UserInGroup>(sqlGet);

            userList = response.ToList();

            return userList;
        }
    }
}