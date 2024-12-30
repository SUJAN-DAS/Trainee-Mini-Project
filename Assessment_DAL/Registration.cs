using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.EnterpriseLibrary.Data;


namespace Assessment_DAL
{
    public class Registration
    {
        public static int RegisterData(Database db, string firstName, string lastName, string emailId, string mobileNo, string password)
        {
            string hashedPassword = HashPassword(password);
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" INSERT INTO LDB1_LOGININFOTABLE_SUJAN ");
            sqlCmdBuilder.Append("(FFNAME, FLNAME, FMOB, FEMAILID, FPASSWORD, FROLEID) ");  
            sqlCmdBuilder.Append(" VALUES (:FIRSTNAME, :LASTNAME, :MOBILE, :EMAILID, :PASSWORD, :ROLEID) "); 

            System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

            // Adding parameters for the user data
            db.AddInParameter(dbCmd, ":FIRSTNAME", DbType.String, firstName);
            db.AddInParameter(dbCmd, ":LASTNAME", DbType.String, lastName);
            db.AddInParameter(dbCmd, ":MOBILE", DbType.String, mobileNo);  
            db.AddInParameter(dbCmd, ":EMAILID", DbType.String, emailId); 
            db.AddInParameter(dbCmd, ":PASSWORD", DbType.String, hashedPassword);

            
            db.AddInParameter(dbCmd, ":ROLEID", DbType.Int32, 2);

            return db.ExecuteNonQuery(dbCmd);
        }
        //For hashing
        private static string HashPassword(string password)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                // Compute the hash of the password
                byte[] bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Convert the byte array to a hexadecimal string
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public static IDataReader LoginData(Database db,string emailId,string password)
        {
            string hashedPassword = HashPassword(password);
            //int role = 0;
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append("SELECT FUSERID,FROLEID FROM LDB1_LOGININFOTABLE_SUJAN");
            sqlCmdBuilder.Append(" WHERE  FEMAILID=:EMAILID AND FPASSWORD=:PASSWORD");

            System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

            
            db.AddInParameter(dbCmd, ":EMAILID", DbType.String, emailId);
            db.AddInParameter(dbCmd, ":PASSWORD", DbType.String, hashedPassword);

            return db.ExecuteReader(dbCmd);
            //return db.ExecuteReader(DBParserBAL.ParseQueryForAnyDB(dbCmd));

            //if (result != null && int.TryParse(result.ToString(), out int roleId))
            //{
            //    role = roleId;
            //}


        }
        //LoginFailedData
        
        public static bool LoginFailedData(Database db, string emailId, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);
                StringBuilder sqlCmdBuilder = new StringBuilder();

                // Log the failed login attempt
                sqlCmdBuilder.Append("INSERT INTO LDB1_FAILLOGIN_SUJAN(FUSERNAME, FPASSWORD, FFAILEDATTEMPTSCOUNT, FTIMESTAMP) ");
                sqlCmdBuilder.Append(" VALUES (:EmailId, :Password, 1 , :Timestamp)");

                System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

                db.AddInParameter(dbCmd, ":EmailId", DbType.String, emailId);
                db.AddInParameter(dbCmd, ":Password", DbType.String, hashedPassword);
                db.AddInParameter(dbCmd, ":Timestamp", DbType.DateTime, DateTime.Now);
                Console.WriteLine("Executing SQL: " + sqlCmdBuilder.ToString());

                return db.ExecuteNonQuery(dbCmd)>0;

            }
            catch (Exception ex)
            {
                // Log the exception details for further analysis
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        //LoginFailedData

        public static int UserData(Database db, int userId, string city, string state, string country, string gender, DateTime dob, byte[] profileImage)
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" INSERT INTO LDB1_USERINFO_SUJAN ");
            sqlCmdBuilder.Append("(FUSERID, FCITY, FSTATE, FCOUNTRY, FGENDER, FDOB, FIMAGE) ");
            sqlCmdBuilder.Append(" VALUES (:UserId, :City, :State, :Country, :Gender, :DOB, :Image) ");

            System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

            // Adding parameters for the user data
            db.AddInParameter(dbCmd, ":UserId", DbType.Int32, userId);
            db.AddInParameter(dbCmd, ":City", DbType.String, city);
            db.AddInParameter(dbCmd, ":State", DbType.String, state);
            db.AddInParameter(dbCmd, ":Country", DbType.String, country);
            db.AddInParameter(dbCmd, ":Gender", DbType.String, gender);
            db.AddInParameter(dbCmd, ":DOB", DbType.Date, dob);
            db.AddInParameter(dbCmd, ":Image", DbType.Binary, profileImage);

            return db.ExecuteNonQuery(dbCmd);
        }

        public static DataTable GetUserData(Database db)
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();

            //27-12-2024 00:05 //19:02 28 dec 2024
            //sqlCmdBuilder.Append(" SELECT FUSERID,FFNAME, FLNAME, FEMAILID, FROLEID");
            //sqlCmdBuilder.Append(" FROM LDB1_LOGININFOTABLE_SUJAN ");
            sqlCmdBuilder.Append(" SELECT FUSERID, FFNAME, FLNAME, FEMAILID, R.FROLENAME AS RoleName ");
            sqlCmdBuilder.Append(" FROM LDB1_LOGININFOTABLE_SUJAN L ");
            sqlCmdBuilder.Append(" JOIN LDB1_ROLEINFOTABLE_SUJAN R ON L.FROLEID = R.FROLEID ");
            sqlCmdBuilder.Append(" WHERE R.FROLENAME != 'Admin' ");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            //db.AddInParameter(dbCmd, ":USER_ID", DbType.Int32, userId);

            return db.ExecuteDataSet(dbCmd).Tables[0];
        }

        public static DataTable SearchLogInfo(Database db, string username)
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" SELECT FUSERNAME,FPASSWORD,FTIMESTAMP,FFAILEDATTEMPTSCOUNT");
            sqlCmdBuilder.Append(" FROM LDB1_FAILLOGIN_SUJAN  ");
            sqlCmdBuilder.Append(" WHERE FUSERNAME LIKE :Username");
            sqlCmdBuilder.Append(" ORDER BY FTIMESTAMP DESC, FFAILEDATTEMPTSCOUNT DESC");

            DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, ":Username", DbType.String, username);

            return db.ExecuteDataSet(dbCmd).Tables[0];
        }

        //Login failed fetch
        //public static DataTable GetLogInfo(Database db, string emailId, string password)
        //{
        //    try
        //    {
        //        string hashedPassword = HashPassword(password);
        //        StringBuilder sqlCmdBuilder = new StringBuilder();

        //        // Check if the username exists and increment the failed attempts count
        //        sqlCmdBuilder.Append("BEGIN ");
        //        sqlCmdBuilder.Append(" IF EXISTS (SELECT 1 FROM LDB1_FAILLOGIN_SUJAN WHERE FUSERNAME = :EmailId) THEN ");
        //        sqlCmdBuilder.Append(" UPDATE LDB1_FAILLOGIN_SUJAN ");
        //        sqlCmdBuilder.Append(" SET FFAILEDATTEMPTSCOUNT = FFAILEDATTEMPTSCOUNT + 1, ");
        //        sqlCmdBuilder.Append(" FTIMESTAMP = :Timestamp,FPASSWORD=:PASSWORD");
        //        sqlCmdBuilder.Append(" WHERE FUSERNAME = :EmailId");
        //        sqlCmdBuilder.Append(" ELSE ");
        //        sqlCmdBuilder.Append(" INSERT INTO LDB1_FAILLOGIN_SUJAN (FUSERNAME, FPASSWORD, FFAILEDATTEMPTSCOUNT, FTIMESTAMP) ");
        //        sqlCmdBuilder.Append(" VALUES (:EmailId, :PASSWORD, 1, :Timestamp); ");
        //        sqlCmdBuilder.Append(" END IF; ");
        //        sqlCmdBuilder.Append(" END; ");

        //        // Query to get all fields from the table
        //        sqlCmdBuilder.Append(" SELECT FUSERNAME, FPASSWORD, FTIMESTAMP, FFAILEDATTEMPTSCOUNT ");
        //        sqlCmdBuilder.Append(" FROM LDB1_FAILLOGIN_SUJAN;");

        //        // Create the database command
        //        System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

        //        db.AddInParameter(dbCmd, ":EmailId", DbType.String, emailId);
        //        db.AddInParameter(dbCmd, ":PASSWORD", DbType.String, hashedPassword);
        //        db.AddInParameter(dbCmd, ":Timestamp", DbType.DateTime, DateTime.Now);

        //        // Execute the command and return the result as a DataTable
        //        DataSet dataSet = db.ExecuteDataSet(dbCmd);
        //        return dataSet.Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log exception details for debugging
        //        Console.WriteLine($"Error: {ex.Message}");
        //        throw;
        //    }
        //}
        //trail
        public static bool CheckMailExist(Database db, string emailId)
        {
            
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" SELECT COUNT(*) FROM LDB1_FAILLOGIN_SUJAN ");
            sqlCmdBuilder.Append(" WHERE FUSERNAME = :EmailId");
            System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

            db.AddInParameter(dbCmd, ":EmailId", DbType.String, emailId);
            int count = Convert.ToInt32(db.ExecuteScalar(dbCmd));
           if(count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }

        public static void UpdateMailAttempt(Database db, string emailId, string password)
        {
            string hashedPassword = HashPassword(password);
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" UPDATE LDB1_FAILLOGIN_SUJAN ");
            sqlCmdBuilder.Append(" SET FFAILEDATTEMPTSCOUNT = FFAILEDATTEMPTSCOUNT +1, ");
            sqlCmdBuilder.Append(" FTIMESTAMP = :Timestamp,FPASSWORD=:PASSWORD ");
            sqlCmdBuilder.Append(" WHERE FUSERNAME =:EmailId ");
            System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

            db.AddInParameter(dbCmd, ":EmailId", DbType.String, emailId);
            db.AddInParameter(dbCmd, ":PASSWORD", DbType.String, hashedPassword);
            db.AddInParameter(dbCmd, ":Timestamp", DbType.DateTime, DateTime.Now);
            db.ExecuteNonQuery(dbCmd);
        }

        //public static DataTable FetchLogFailed(Database db)
        //{
        //    StringBuilder sqlCmdBuilder = new StringBuilder();
        //    sqlCmdBuilder.Append(" SELECT FUSERNAME, FPASSWORD, FTIMESTAMP, FFAILEDATTEMPTSCOUNT ");
        //    sqlCmdBuilder.Append(" FROM LDB1_FAILLOGIN_SUJAN ");
        //    DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
        //    dbCmd.CommandType = CommandType.Text;

        //    return db.ExecuteDataSet(dbCmd).Tables[0];
        //}
        public static DataTable FetchLogFailed(Database db, string searchUsername = "", string sortColumn = "FTIMESTAMP", string sortOrder = "DESC")
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" SELECT FUSERNAME, ");
            sqlCmdBuilder.Append(" MAX(FPASSWORD) AS FPASSWORD, ");
            sqlCmdBuilder.Append(" MAX(FTIMESTAMP) AS FTIMESTAMP, ");
            sqlCmdBuilder.Append(" SUM(FFAILEDATTEMPTSCOUNT) AS FFAILEDATTEMPTSCOUNT ");
            sqlCmdBuilder.Append(" FROM LDB1_FAILLOGIN_SUJAN ");

            // If a username is provided for searching, add the WHERE clause
            if (!string.IsNullOrEmpty(searchUsername))
            {
                sqlCmdBuilder.Append(" WHERE FUSERNAME LIKE :searchUsername ");
            }

            sqlCmdBuilder.Append(" GROUP BY FUSERNAME "); // Add GROUP BY clause to group results by username

            // Add the sorting order
            // Ensure valid column and sort order for security
            string[] validColumns = { "FTIMESTAMP", "FUSERNAME", "FFAILEDATTEMPTSCOUNT" };
            string[] validSortOrders = { "ASC", "DESC" };

            if (!validColumns.Contains(sortColumn.ToUpper())) sortColumn = "FTIMESTAMP";
            if (!validSortOrders.Contains(sortOrder.ToUpper())) sortOrder = "DESC";

            sqlCmdBuilder.Append($" ORDER BY {sortColumn} {sortOrder} ");


            DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());

            // Add parameters to prevent SQL injection
            if (!string.IsNullOrEmpty(searchUsername))
            {
                db.AddInParameter(dbCmd, ":searchUsername", DbType.String, "%" + searchUsername + "%");
            }

            dbCmd.CommandType = CommandType.Text;

            return db.ExecuteDataSet(dbCmd).Tables[0];
        }

        //Login failed fetch

        //Get user info
        public static DataTable GetUsers(Database db, int userID)
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" SELECT FUSERID,FFNAME, FLNAME, FEMAILID");//remove role from here
            sqlCmdBuilder.Append("  FROM LDB1_LOGININFOTABLE_SUJAN  WHERE FUSERID=:UserId");
            //sqlCmdBuilder.Append(" SELECT FUSERID, FFNAME, FLNAME, FEMAILID, R.FROLENAME AS RoleName ");
            //sqlCmdBuilder.Append(" FROM LDB1_LOGININFOTABLE_SUJAN L ");
            //sqlCmdBuilder.Append(" JOIN LDB1_ROLEINFOTABLE_SUJAN R ON L.FROLEID = R.FROLEID ");
            //sqlCmdBuilder.Append(" WHERE FUSERID = :UserId");


            System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
            dbCmd.CommandType = CommandType.Text;

            db.AddInParameter(dbCmd, ":UserId", DbType.Int32, userID);
            return db.ExecuteDataSet(dbCmd).Tables[0];
        }

        //Update role from drop down 
        public static bool UpdateChangeRole(Database db, int userId, string role)
        {
            try
            {
                StringBuilder sqlCmdBuilder = new StringBuilder();
                sqlCmdBuilder.Append(" UPDATE LDB1_LOGININFOTABLE_SUJAN SET ");
                sqlCmdBuilder.Append(" FROLEID = (SELECT FROLEID FROM LDB1_ROLEINFOTABLE_SUJAN WHERE FROLENAME = :ROLENAME) ");
                sqlCmdBuilder.Append(" WHERE FUSERID = :UserId");

                System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
                dbCmd.CommandType = CommandType.Text;

                db.AddInParameter(dbCmd, ":ROLENAME", DbType.String, role);
                db.AddInParameter(dbCmd, ":UserId", DbType.String, userId);

                int rowsAffected = db.ExecuteNonQuery(dbCmd);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework or custom logic)
                Console.WriteLine($"Error updating role: {ex.Message}");
                return false;
            }

        }

        //Update role from drop down 

        //TO UPDATE USER DATA
        public static bool UpdateUserData(Database db, int userId, string firstName, string lastName, string email)
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append(" UPDATE LDB1_LOGININFOTABLE_SUJAN SET FFNAME=:FIRSTNAME,FLNAME=:LASTNAME, FEMAILID=:EMAILID");//remove role
            sqlCmdBuilder.Append(" WHERE FUSERID=:UserId");
            //sqlCmdBuilder.Append(" UPDATE LDB1_LOGININFOTABLE_SUJAN SET FFNAME=:FIRSTNAME, FLNAME=:LASTNAME, FEMAILID=:EMAILID, ");
            //sqlCmdBuilder.Append(" FROLEID = (SELECT FROLEID FROM LDB1_ROLEINFOTABLE_SUJAN WHERE FROLENAME = :ROLENAME) ");
            //sqlCmdBuilder.Append(" WHERE FUSERID = :UserId");

            try
            {
                System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
                dbCmd.CommandType = CommandType.Text;


                db.AddInParameter(dbCmd, ":FIRSTNAME", DbType.String, firstName);
                db.AddInParameter(dbCmd, ":LASTNAME", DbType.String, lastName);
                db.AddInParameter(dbCmd, ":EMAILID", DbType.String, email);
                //db.AddInParameter(dbCmd, ":ROLEID", DbType.Int32, role);
                db.AddInParameter(dbCmd, ":UserId", DbType.Int32, userId);

                int rowsAffected = db.ExecuteNonQuery(dbCmd);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating user data: " + ex.Message);
                throw;
            }

        }

        //Delete User
        public static string DeleteUserData(Database db, int userid)
        {
            StringBuilder sqlCmdBuilder = new StringBuilder();
            sqlCmdBuilder.Append("DELETE FROM LDB1_LOGININFOTABLE_SUJAN WHERE FUSERID=:UserId");

            try
            {
                System.Data.Common.DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
                dbCmd.CommandType = CommandType.Text;

                db.AddInParameter(dbCmd, ":UserId", DbType.Int32, userid);
                int rowsDeleted = db.ExecuteNonQuery(dbCmd);

                if (rowsDeleted > 0)
                {
                    return "1";
                }
                else
                {
                    return "No user found with the specified UserId.";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

        }

        //public static IDataReader GetPPEForSendToExcel(Database db)
        //{
        //    StringBuilder sqlCmdBuilder = new StringBuilder();
        //    sqlCmdBuilder.Append(" SELECT FUSERID,FFNAME, FLNAME, FEMAILID, FROLEID");

        //    sqlCmdBuilder.Append(" FROM LDB1_LOGININFOTABLE_SUJAN L");
            
        //    //sqlCmdBuilder.Append(" LEFT OUTER JOIN LDB1_MATERIAL_PPE M ON M.FPPECODE = C.FPPECODE AND C.FSITEID =M.FSITEID ");
        //    //sqlCmdBuilder.Append(" LEFT OUTER JOIN LDB1_PLANTPPE P ON M.FPPEID  = P.FPPEID AND M.FSITEID=P.FSITEID ");
        //    //sqlCmdBuilder.Append(" LEFT OUTER JOIN LDB1_USERINFO UI ON UI.FUSERID = DECODE(NVL(C.FUPDATEDBY,0),0,C.FCREATEDBY,C.FUPDATEDBY) ");
        //    //sqlCmdBuilder.Append(" WHERE C.FSITEID=:SITEID ");
        //    //sqlCmdBuilder.Append(" GROUP BY C.FPPECODE,C.FMATERIALTYPE,DECODE(NVL(C.FUPDATEDON,0),0,C.FCREATEDDATE,C.FUPDATEDON) ,CONCAT(CONCAT(NVL(INITCAP(UI.FLASTNAME),''), ' '), NVL(INITCAP(UI.FFIRSTNAME),'')) ");
        //    //sqlCmdBuilder.Append(" ORDER BY C.FPPECODE  ");

        //    DbCommand dbCmd = db.GetSqlStringCommand(sqlCmdBuilder.ToString());
        //    dbCmd.CommandType = CommandType.Text;
        //    //db.AddInParameter(dbCmd, ":SITEID", DbType.Int32, siteID);
        //    //return db.ExecuteReader(DBParserBAL.ParseQueryForAnyDB(dbCmd));
        //    return db.ExecuteReader(dbCmd);

        //}
    }
}
