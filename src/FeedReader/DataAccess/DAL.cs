using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace FeedReader.DataAccess
{
    public class DAL
    {
        private static SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["FeedReaderDBConnection"].ToString());
        private static SqlConnection conn2 = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        private static DataTable dt = new DataTable();

        public static string GetUserId(string _username)
        {
            string userID = "";
            string query = string.Format("SELECT * from [UserProfile] where Username = '{0}'", _username);
            var cmd = new SqlCommand(query, conn2);
            
            conn2.Open();

            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                int id = (Int32)rdr["UserId"];
                string username = (string)rdr["UserName"];
                //HttpContext.Current.Session["USERID"] = id.ToString();
                userID = id.ToString();
                conn2.Close();
                return userID;
            }
            userID = "";
            conn2.Close();
            return userID;
            
        }

        public static DataTable SubscribeFeed(int _userid, string _rssfeed)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "sp_UserFeedSubscription";
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@userid", _userid);
                comm.Parameters.AddWithValue("@rsslink", _rssfeed);
                comm.Parameters.AddWithValue("@isactive", 1);
                comm.Parameters.AddWithValue("@flag", "ADD");
                SqlDataAdapter myDa = new SqlDataAdapter(comm);
                dt.Clear();
                myDa.Fill(dt);
                comm.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return dt;
            
        }

        public static DataTable ViewSubscribedFeed(int _userid)
        {
            try
            {
                conn.Open();
                SqlCommand comm = new SqlCommand();
                comm.Connection = conn;
                comm.CommandText = "sp_UserFeedSubscription";
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@userid", _userid);
                comm.Parameters.AddWithValue("@rsslink", "");
                comm.Parameters.AddWithValue("@isactive", 0);
                comm.Parameters.AddWithValue("@flag", "VIEW");
                SqlDataAdapter myDa = new SqlDataAdapter(comm);
                dt.Clear();
                myDa.Fill(dt);
                comm.Dispose();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw;
            }

            return dt;
           
        }
    }
}