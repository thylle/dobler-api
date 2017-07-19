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

        //public IEnumerable<User> GetAllUsers () {

        //    string sqlGet = "select * from Users";

        //    var response = connection.Query<User>(sqlGet).Select(item => new User {
        //        Id = item.Id,
        //        Name = item.Name
        //    });

        //    return response;
        //}


        public UserData GetUserData (User user) {
            var sqlGet = "select * from Users where Email = '" + user.Email + "'";
            var response = connection.Query<User>(sqlGet).FirstOrDefault();

            //Return the user if it already exists
            if (response != null) {
                user.Id = response.Id;
                user.Name = response.Name;
                user.Email = response.Email;

                var userData = new UserData{
                    User = user,
                    Groups = GetUserGroups(user),
                    Coupons = GetUserCoupons(user)
                };

                return userData;
            }

            //If user with this email does not exists - we create a new user
            return AddUser(user);
        }

        public List<Group> GetUserGroups (User user) {
            var sqlGet = "select g.Id, g.Name from User_Groups ug INNER JOIN Groups g ON g.Id = ug.GroupId WHERE ug.UserId = '" + user.Id + "'";
            var response = connection.Query<Group>(sqlGet);

            return response.ToList();
        }

        public List<Coupon> GetUserCoupons (User user) {
            var sqlGet = "select * from Coupons WHERE UserId = '" + user.Id + "' ORDER BY Created DESC";
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

        public List<User> GetUsersForGroup (int groupId) {

            var userList = new List<User>();
            var sqlGet = "select u.Id, u.Name from User_Groups ug INNER JOIN Users u ON u.Id = ug.UserId WHERE ug.GroupId = '" + groupId + "'";
            var response = connection.Query<User>(sqlGet);

            userList = response.ToList();

            return userList;
        }
    }
}