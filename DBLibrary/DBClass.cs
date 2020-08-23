using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace DBLibrary
{
    public static class DBClass
    {
        #region Global variables
        public static int UserId;
        public static string UserName = "";
        public static string UserType = "";
        public static int BankCustomerId;
        public static string ServerName = "";
        public static string DBName = "";

        public static decimal VAT = 0;
        public static decimal MAXDISC = 0;
        public static decimal MARKUP = 0;

        public static SqlConnection connection;
        public static SqlCommand command;

        public static string Connstr = "";
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion
    

    public static void SetConnectionString()
        {
            string strDefault = "";
            StringBuilder strBuffer = new StringBuilder(1024);
            long lngVal;

            lngVal = GetPrivateProfileString("CONNECT", "SERVER", strDefault, strBuffer, strBuffer.Capacity, System.IO.Directory.GetCurrentDirectory() + "\\SETTINGS.INI");
            if (lngVal <= 0)
            {

            }
            else
            {
                int val = Convert.ToInt32(lngVal);
                ServerName = strBuffer.ToString().Substring(0, val);  //Left(strBuffer.ToString, lngVal);

            }

            lngVal = GetPrivateProfileString("CONNECT", "DATABASENAME", strDefault, strBuffer, strBuffer.Capacity, System.IO.Directory.GetCurrentDirectory() + "\\SETTINGS.INI");
            if (lngVal <= 0)
            {
                
            }
            else
            {
                int val = Convert.ToInt32(lngVal);
                DBName = strBuffer.ToString().Substring(0, val);  //Left(strBuffer.ToString, lngVal);
                
            }
            
            ///// Set Default Value
            lngVal = GetPrivateProfileString("DEFAULT_VALUE", "VAT", strDefault, strBuffer, strBuffer.Capacity, System.IO.Directory.GetCurrentDirectory() + "\\SETTINGS.INI");
            if (lngVal <= 0)
            {

            }
            else
            {
                int val = Convert.ToInt32(lngVal);
                VAT = Convert.ToDecimal(strBuffer.ToString().Substring(0, val));  //Left(strBuffer.ToString, lngVal);

            }

            lngVal = GetPrivateProfileString("DEFAULT_VALUE", "MAXDISC", strDefault, strBuffer, strBuffer.Capacity, System.IO.Directory.GetCurrentDirectory() + "\\SETTINGS.INI");
            if (lngVal <= 0)
            {

            }
            else
            {
                int val = Convert.ToInt32(lngVal);
                MAXDISC = Convert.ToDecimal(strBuffer.ToString().Substring(0, val));  //Left(strBuffer.ToString, lngVal);

            }

            lngVal = GetPrivateProfileString("DEFAULT_VALUE", "MARKUP", strDefault, strBuffer, strBuffer.Capacity, System.IO.Directory.GetCurrentDirectory() + "\\SETTINGS.INI");
            if (lngVal <= 0)
            {

            }
            else
            {
                int val = Convert.ToInt32(lngVal);
                MARKUP = Convert.ToDecimal(strBuffer.ToString().Substring(0, val));  //Left(strBuffer.ToString, lngVal);

            }

            Connstr = "Data Source= " + ServerName + ";Initial Catalog= " + DBName + ";Integrated Security=True";
            connection = new SqlConnection(Connstr);
        }

        public static DataTable GetTableRecords(string Tablename)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();

            string sql = "select *from " + Tablename;
            connection.Open();
            command = new SqlCommand(sql, connection);
            da.SelectCommand = command;
            da.Fill(dt);
            dt.TableName = Tablename;

            connection.Close();
            return dt;


        }

        public static SqlDataAdapter GetAdaptor(string Tablename)
        {
            DataTable dt = new DataTable();
            
            connection = new SqlConnection(Connstr);
            string sql = "select *from " + Tablename;
            connection  .Open();
            command = new SqlCommand(sql, connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            connection.Close();
            return da;

        }

        public static DataTable GetTableByQuery(string Query)
        {
            DataTable dt = new DataTable();
            connection = new SqlConnection(Connstr);

            connection.Open();
            command = new SqlCommand(Query, connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dt);
            connection.Close();
            return dt;
        }

        public static SqlDataAdapter GetAdapterByQuery(string Query)
        {
            connection = new SqlConnection(Connstr);
            connection.Open();
            command = new SqlCommand(Query, connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            
            connection.Close();
            return da;
        }

        public static int GetIdByQuery(string Query)
        {
            SqlCommand command = new SqlCommand();
            command = new SqlCommand(Query, connection);

            connection.Open();
            SqlDataAdapter adp = new SqlDataAdapter(command);
            connection.Close();

            DataTable dt = new DataTable();
            adp.Fill(dt);
            connection.Close();

            if (dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
            {
                return int.Parse(dt.Rows[0][0].ToString());
            }
            return 1;
        }

        public static string GetColValueByQuery(string Query)
        {
            SqlCommand command = new SqlCommand();
            command = new SqlCommand(Query, connection);

            connection.Open();
            SqlDataAdapter adp = new SqlDataAdapter(command);
            connection.Close();

            DataTable dt = new DataTable();
            adp.Fill(dt);
            connection.Close();

            if (dt.Rows.Count > 0)
            {
                if ( dt.Rows[0][0].ToString() != "")
                {
                    return dt.Rows[0][0].ToString();
                }
            }

            return "";

        }

        public static string[] GetMultipleColValueByQuery(string Query, int TotalColumnReturn)
        {
            SqlCommand command = new SqlCommand();
            command = new SqlCommand(Query, connection);

            string []ColValue=new string[TotalColumnReturn];

            connection.Open();
            SqlDataAdapter adp = new SqlDataAdapter(command);
            connection.Close();

            DataTable dt = new DataTable();
            adp.Fill(dt);
            connection.Close();

            if (dt.Rows.Count > 0)
            {
                if ( dt.Rows[0][0].ToString() != "")
                {
                    for (int Col = 0; Col < TotalColumnReturn; Col++)
			            {
			                ColValue[Col]=dt.Rows[0][Col].ToString();
			            }
                }
            }
            return ColValue;
        }

        public static int ExecuteNonQuery(string Query)
        {
            //string constr = ConfigurationManager.ConnectionStrings["COSConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(Connstr);
            SqlCommand cmd = new SqlCommand(Query, conn);
            //cmd.Parameters.AddRange(Params);

            int NewId = 0;
            try
            {
                conn.Open();
                NewId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            catch (Exception)
            {
                
            }
            finally
            {

                cmd.Parameters.Clear();
                cmd.Dispose();
                conn.Dispose();
            }
            return NewId;
        }

        public static bool AddUser(string username, string password)
        {
            // This function will add a user to our database

            // First create a new Guid fior the user
            Guid userGuid = System.Guid.NewGuid();

            //string hashedPassword = Security.HashSHA1(password + userGuid.ToString());
            string EncryptedPwd = Security.ECODE_DECODE(password,"E");
            DataTable dt = new DataTable();
            dt = DBClass.GetTableRecords("User_Master");

            string UserType = "";
            if (dt.Rows.Count == 0)
                UserType = "ADMIN";
            else
                UserType = "USER";

            SqlConnection con = new SqlConnection(Connstr);
            using (SqlCommand cmd = new SqlCommand("INSERT INTO [User_Master](User_Name,User_Password,User_Guid,User_Type) VALUES (@User_Name, @User_Password, @User_Guid,@User_Type)", con))
            {
                cmd.Parameters.AddWithValue("@User_Name", username);
                cmd.Parameters.AddWithValue("@User_password", EncryptedPwd);
                cmd.Parameters.AddWithValue("@User_Guid", userGuid);
                cmd.Parameters.AddWithValue("@User_Type", UserType);

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return true;
        }

        public static int GetUserIdByUsernameAndPassword(string username, string password)
        {
            // this is the value we will return
            int userId = 0;

            SqlConnection conn = new SqlConnection(Connstr);
            using (
                SqlCommand cmd =
                    new SqlCommand("SELECT User_Id, User_Password, User_Guid FROM [User_Master] WHERE User_Name=@user_Name", conn))
            {
                cmd.Parameters.AddWithValue("@User_Name", username);
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    // read data from database if we found any user
                    int dbUserId = Convert.ToInt32(dr["User_Id"]);
                    string dbPassword = Convert.ToString(dr["User_Password"]);
                    string dbUserGuid = Convert.ToString(dr["User_Guid"]);

                    // Now we hash the UserGuid from the database with the password we wan't to check
                    // In the same way as when we saved it to the database in the first place.
                    //string hashedPassword = Security.HashSHA1(password + dbUserGuid.ToLower());

                    string EncryptedPwd = Security.ECODE_DECODE(password, "E");
                    // if its correct password the result of the hash is the same as in the database
                    if (dbPassword == EncryptedPwd)
                    {
                        // The password is correct
                        userId = dbUserId;
                    }
                }
                conn.Close();
            }

            // Return the user id which is 0 if we did not found a user.
            return userId;
        }

       
    }
}
