using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Dapper;
using DoblerAPI.Models;

namespace DoblerAPI.Services {
    public class GroupRepository {

        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["doblerConnectionString"].ConnectionString);

        public List<Group> GetAllGroups () {
            var sqlGet = "SELECT * FROM Groups ORDER BY Name";
            return connection.Query<Group>(sqlGet).ToList();
        }

        public Group GetGroup (int id) {
            var sqlGet = "SELECT * FROM Groups WHERE Id = '" + id + "'";
            return connection.Query<Group>(sqlGet).FirstOrDefault();
        }

        public Group CreateGroup (string name, bool isPrivate, int userId) {

            connection.Open();
            SqlCommand command = new SqlCommand("[dbo].[CreateAndJoinGroup]", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@isPrivate", isPrivate);
            command.Parameters.AddWithValue("@userId", userId);
            command.ExecuteNonQuery();
            connection.Close();

            return null;
        }

        public Group JoinGroup (int userId, int groupId) {

            var sqlAdd = "INSERT INTO User_Groups(UserId, GroupId) " +
                         "VALUES('" + userId + "', '" + groupId + "')";

            SqlCommand command = new SqlCommand(sqlAdd, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();

            return null;
        }

        public Group LeaveGroup (int userId, int groupId) {
            var sqlAdd = "DELETE FROM User_Groups " +
                         "WHERE UserId = '" + userId + "' AND GroupId = '" + groupId + "'";

            SqlCommand command = new SqlCommand(sqlAdd, connection);
            command.Connection.Open();
            command.ExecuteNonQuery();
            command.Connection.Close();

            return null;
        }
    }
}