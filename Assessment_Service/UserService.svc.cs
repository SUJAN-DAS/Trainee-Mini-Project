using System;
using ExcelLibrary.SpreadSheet;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Assessment_DAL;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Reflection;
using System.Data.Common;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Data.OleDb;
using System.Text.RegularExpressions;

namespace Assessment_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    public class UserService : IUserService
    {
        //delegate void DataInfoExcel(Database db, string fileName);
        delegate void DownloadUserInfoExcelTemplate(Database db,string fileName);
        delegate void UploadUserInfoExcel(Database db, string fileName);
        public int RegistrationDetails(string firstName, string lastName, string emailId, string mobileNo, string password, string role)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.RegisterData(db, firstName, lastName, emailId, mobileNo, password,role);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new FaultException("An error occurred while registering the user.");
            }


        }

        public UserClass LoginDetails(string emailId, string password)
        {
            try
            {

                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                IDataReader dataReader = null;
                dataReader = Registration.LoginData(db, emailId, password);
                UserClass user = new UserClass();
                while (dataReader.Read())
                {
                    user.RoleId = dataReader.GetInt32(dataReader.GetOrdinal("FROLEID"));
                    user.UserId = dataReader.GetInt32(dataReader.GetOrdinal("FUSERID"));
                }

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new FaultException("Data Not Found in the Database .Register yourself first");
            }
        }
        //LoginFailedDetail
        public UserClass LogFailedLoginAttempt(string emailId, string password)
        {
            try
            {

                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                UserClass userClass = new UserClass();
                bool isInserted = Registration.LoginFailedData(db, emailId, password);
                if (isInserted)
                {
                    userClass.ErrorMessage = "Login Failed data";

                }
                return userClass;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new FaultException("Data Not Found in the Database .Register yourself first");
            }
        }
        //LoginFailedDetails

        //LoginFailed fetch Data
        public List<LogFailed> GetLogInfo(string emailId, string password)
        {
            Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");

            try
            {
                using (DbConnection connection = db.CreateConnection())
                {
                    connection.Open();
                    using (DbTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Check if the email exists
                            if (Registration.CheckMailExist(db, emailId))
                            {
                                // Update failed attempts if email exists
                                Registration.UpdateMailAttempt(db, emailId, password);
                            }
                            else
                            {
                                // Insert a new failed login record if email does not exist
                                Registration.LoginFailedData(db, emailId, password);
                            }

                            // Fetch the log data with search and sorting
                            DataTable dataTable = Registration.FetchLogFailed(db);

                            // Convert the DataTable to a List of LogFailed
                            List<LogFailed> log = new List<LogFailed>();
                            foreach (DataRow row in dataTable.Rows)
                            {
                                log.Add(new LogFailed
                                {
                                    UserName = row["FUSERNAME"].ToString(),
                                    Password = row["FPASSWORD"].ToString(),
                                    Timestamp = row["FTIMESTAMP"] != DBNull.Value
                                        ? Convert.ToDateTime(row["FTIMESTAMP"])
                                        : DateTime.MinValue,
                                    FailedAttemptsCount = row["FFAILEDATTEMPTSCOUNT"] != DBNull.Value
                                        ? Convert.ToInt32(row["FFAILEDATTEMPTSCOUNT"])
                                        : 0,
                                });
                            }

                            // Commit the transaction
                            transaction.Commit();
                            return log;
                        }
                        catch (Exception ex)
                        {
                            // Rollback the transaction on failure
                            transaction.Rollback();
                            Console.WriteLine($"Transaction failed: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<LogFailed>(); // Return an empty list on error
            }
        }

        //Search login in failed
        public List<LogFailed> SearchLogInfo(string username)
        {
            Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
            DataTable dataTable = Registration.SearchLogInfo(db,username);

            // Convert the DataTable to a List of LogFailed
            List<LogFailed> searchLog = new List<LogFailed>();
            foreach (DataRow row in dataTable.Rows)
            {
                searchLog.Add(new LogFailed
                {
                    UserName = row["FUSERNAME"].ToString(),
                    Password = row["FPASSWORD"].ToString(),
                    Timestamp = row["FTIMESTAMP"] != DBNull.Value
                        ? Convert.ToDateTime(row["FTIMESTAMP"])
                        : DateTime.MinValue,
                    FailedAttemptsCount = row["FFAILEDATTEMPTSCOUNT"] != DBNull.Value
                        ? Convert.ToInt32(row["FFAILEDATTEMPTSCOUNT"])
                        : 0,
                });
            }
            return searchLog;
        }

        //public List<LogFailed> GetLogInfo(string emailId, string password)
        //{
        //    Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");

        //    try
        //    {
        //        using (DbConnection connection = db.CreateConnection())
        //        {
        //            connection.Open();
        //            using (DbTransaction transaction = connection.BeginTransaction())
        //            {
        //                try
        //                {
        //                    // Check if the email exists
        //                    if (Registration.CheckMailExist(db, emailId))
        //                    {
        //                        // Update failed attempts if email exists
        //                        Registration.UpdateMailAttempt(db, emailId, password);
        //                    }
        //                    else
        //                    {
        //                        // Insert a new failed login record if email does not exist
        //                        Registration.LoginFailedData(db, emailId, password);
        //                    }

        //                    // Fetch the log data
        //                    DataTable dataTable = Registration.FetchLogFailed(db);

        //                    // Convert the DataTable to a List of LogFailed
        //                    List<LogFailed> log = new List<LogFailed>();
        //                    foreach (DataRow row in dataTable.Rows)
        //                    {
        //                        log.Add(new LogFailed
        //                        {
        //                            UserName = row["FUSERNAME"].ToString(),
        //                            Password = row["FPASSWORD"].ToString(),
        //                            Timestamp = row["FTIMESTAMP"] != DBNull.Value
        //                                ? Convert.ToDateTime(row["FTIMESTAMP"])
        //                                : DateTime.MinValue,
        //                            FailedAttemptsCount = row["FFAILEDATTEMPTSCOUNT"] != DBNull.Value
        //                                ? Convert.ToInt32(row["FFAILEDATTEMPTSCOUNT"])
        //                                : 0,
        //                        });
        //                    }

        //                    // Commit the transaction
        //                    transaction.Commit();
        //                    return log;
        //                }
        //                catch (Exception ex)
        //                {
        //                    // Rollback the transaction on failure
        //                    transaction.Rollback();
        //                    Console.WriteLine($"Transaction failed: {ex.Message}");
        //                    throw;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //        return new List<LogFailed>(); // Return an empty list on error
        //    }
        //}

        //DataTable dataTable = Registration.GetLogInfo(db,emailId,password);
        //List<LogFailed> log = new List<LogFailed>();

        //foreach (DataRow row in dataTable.Rows)
        //{
        //    log.Add(new LogFailed
        //    {
        //        UserName = row["FUSERNAME"].ToString(),
        //        Password = row["FPASSWORD"].ToString(),
        //        Timestamp = row["FTIMESTAMP"] != DBNull.Value ? Convert.ToDateTime(row["FTIMESTAMP"]) : DateTime.MinValue,
        //        FailedAttemptsCount = row["FFAILEDATTEMPTSCOUNT"] != DBNull.Value ? Convert.ToInt32(row["FFAILEDATTEMPTSCOUNT"]) : 0,

        //    });
        //}

        //return log;

        //LoginFailed fetch Data

        public int UserDetails(int userId,string city, string state, string country, string gender, DateTime dob, byte[] profileImage)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.UserData(db, userId,city, state, country, gender, dob, profileImage);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw new FaultException("An error occurred while registering the user.");
            }

        }

        public List<DataListInfo> GetAdminData()
        {
            Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
            DataTable dataTable = Registration.GetAdminData(db);
            List<DataListInfo> users = new List<DataListInfo>();

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new DataListInfo
                {
                    UserId = row["FUSERID"] != DBNull.Value ? Convert.ToInt32(row["FUSERID"]) : 0,
                    FirstName = row["FFNAME"].ToString(),
                    LastName = row["FLNAME"].ToString(),
                    Email = row["FEMAILID"].ToString(),
                    Role = row["RoleName"].ToString()
                    //Role = row["FROLEID"].ToString()
                    //Role = row["RoleName"].ToString()
                });
            }

            return users;
        }

        //Get all user data for a particular id
        public EditData GetUserDetail(int userID)
        {
            EditData users = null;
            Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");

            try
            {
                DataTable dataTable = Registration.GetUserDetail(db, userID);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    
                      users=new EditData
                      {
                             UserId = row["FUSERID"] != DBNull.Value ? Convert.ToInt32(row["FUSERID"]) : 0,
                             FirstName = row["FFNAME"].ToString(),
                             LastName = row["FLNAME"].ToString(),
                             Email = row["FEMAILID"].ToString(),
                          
                      };
             
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return users;
        }

        //to update the user data
        public bool UpdateUserInfo(EditData userInfo)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.UpdateUserInfo(db, userInfo.UserId, userInfo.FirstName, userInfo.LastName, userInfo.Email);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Update role from drop down 

        public bool UpdateChangeRole(int userId, string role)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.UpdateChangeRole(db, userId, role);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        //Update role from drop down

        //Delete user
        public string DeleteUser(int userid)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.DeleteUser(db, userid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }

        //Load admin name in admin page
        public string GetAdminFirstName(int userId)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.GetAdminFirstName(db, userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }

        //#region Download and Upload


        public string DownloadUserInfoTemplate()
        {
            try
            {
                string fileName = string.Empty;
                AsyncCallback cb = new AsyncCallback(send);
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                fileName = "UserDetails_" + DateTime.Now.ToString("ddMMhhmmssffff");
                DownloadUserInfoExcelTemplate asyncDowntimeTemplate = new DownloadUserInfoExcelTemplate(GetCreateUserInfoTemplateExcel);
                asyncDowntimeTemplate.BeginInvoke(db, fileName, cb, null);

                return fileName;
            }
            catch
            {
                return string.Empty;
            }
        }

        private void GetCreateUserInfoTemplateExcel(Database db, string fileName)
        {
            IDataReader dataReaderRoles = null;
            try
            {
                string path = string.Empty;
                int columnIndex = 1;
                int rowIndex = 1;

                path = ConfigurationManager.AppSettings["LogFileLocation"];
                string completeFileName = fileName + ".xlsx";
                string targetFile = path + completeFileName;
                bool IsSiteAdminVisible = false;
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                FileInfo templateFile = new FileInfo(targetFile);
                if (templateFile.Exists)//for overwrite on existing file if exist.
                {
                    templateFile.Delete();
                    templateFile = new FileInfo(targetFile);
                }

                using (ExcelPackage package = new ExcelPackage(templateFile))
                {
                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("UserInfoList");
                    BindCreateUserInfoTemplate(db, worksheet, colFromHex, package, rowIndex, columnIndex);

                    package.Save();
                    package.Dispose();
                }

                string txtFile = path + "\\" + fileName + "_tempSuccess.txt";
                using (FileStream fStream = new FileStream(txtFile, FileMode.Create, FileAccess.Write))
                {
                    using (StreamWriter sWriter = new StreamWriter(fStream))
                    {
                        sWriter.Write("Excel sheet downloaded successfully");
                        sWriter.Close();
                    }
                    fStream.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
            }
        }

        private void BindCreateUserInfoTemplate(Database db, ExcelWorksheet workSheet, Color colFromHex, ExcelPackage package, int rowIndex, int columnIndex)
        {
            IDataReader dataReaderInfo = null;
            try
            {
                List<string> customList = new List<string>();

                if (workSheet.Name == "UserInfoList")
                {
                    workSheet.Cells[rowIndex, columnIndex].Value = Resources.UserInformation_Resource.firstName + "(*)";
                    Common.BindDataValidationForExcel(db,workSheet, package, columnIndex, Resources.UserInformation_Resource.firstName, string.Empty, 0, 18, Resources.UserInformation_Resource.firstName, string.Empty, string.Empty, string.Empty, string.Empty, false, false, Common.ExcelDataValidation.TextLength);
                    workSheet.Column(columnIndex).Width = 18;
                    columnIndex++;

                    workSheet.Cells[rowIndex, columnIndex].Value = Resources.UserInformation_Resource.lastName + "(*)";
                    Common.BindDataValidationForExcel(db, workSheet, package, columnIndex, Resources.UserInformation_Resource.lastName, string.Empty, 0, 18, Resources.UserInformation_Resource.lastName, string.Empty, string.Empty, string.Empty, string.Empty, false, false,Common.ExcelDataValidation.TextLength);
                    workSheet.Column(columnIndex).Width = 18;
                    columnIndex++;


                    workSheet.Cells[rowIndex, columnIndex].Value = Resources.UserInformation_Resource.emailID + "(*)";
                    Common.BindDataValidationForExcel(db,workSheet, package, columnIndex, Resources.UserInformation_Resource.emailID, string.Empty, 0, 50, Resources.UserInformation_Resource.emailID, string.Empty, string.Empty, string.Empty, string.Empty, false, false, Common.ExcelDataValidation.TextLength);
                    workSheet.Column(columnIndex).Width = 50;
                    columnIndex++;

                    workSheet.Cells[rowIndex, columnIndex].Value = Resources.UserInformation_Resource.MobileNumber + "(*)";
                    Common.BindDataValidationForExcel(db, workSheet, package, columnIndex, Resources.UserInformation_Resource.MobileNumber, string.Empty, 0, 50, Resources.UserInformation_Resource.MobileNumber, string.Empty, string.Empty, string.Empty, string.Empty, false, false, Common.ExcelDataValidation.TextLength);
                    workSheet.Column(columnIndex).Width = 50;
                    columnIndex++;

                    workSheet.Cells[rowIndex, columnIndex].Value = Resources.UserInformation_Resource.password + "(*)";
                    Common.BindDataValidationForExcel(db, workSheet, package, columnIndex, Resources.UserInformation_Resource.password, string.Empty, 0, 50, Resources.UserInformation_Resource.password, string.Empty, string.Empty, string.Empty, string.Empty, false, false, Common.ExcelDataValidation.TextLength);
                    workSheet.Column(columnIndex).Width = 50;
                    columnIndex++;


                    workSheet.Cells[rowIndex, columnIndex].Value = Resources.UserInformation_Resource.role + "(*)";
                    Common.BindDataValidationForExcel(db, workSheet, package, columnIndex, Resources.UserInformation_Resource.role, string.Empty, 0, 50, Resources.UserInformation_Resource.role, string.Empty, string.Empty, string.Empty, string.Empty, false, false, Common.ExcelDataValidation.TextLength);
                    workSheet.Column(columnIndex).Width = 50;
                    columnIndex++;
                }

                using (ExcelRange headerColumnRange = workSheet.Cells[workSheet.Dimension.Address])
                {
                    headerColumnRange.Style.Font.Bold = true;
                    headerColumnRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    headerColumnRange.Style.Fill.BackgroundColor.SetColor(colFromHex);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (dataReaderInfo != null && !dataReaderInfo.IsClosed)
                    dataReaderInfo.Close();
            }
        }

        private static void send(IAsyncResult ar)
        {
            string callerMethodName = string.Empty;
            try
            {
                AsyncResult result = (AsyncResult)ar;
                Type delegateType = result.AsyncDelegate.GetType();

                MethodInfo methodInfo = delegateType.GetMethod("EndInvoke");
                callerMethodName = methodInfo.DeclaringType.FullName;
                methodInfo.Invoke(result.AsyncDelegate, new object[] { ar });
            }
            catch (Exception ex)
            {
                string exceptionErrorMessage = Common.GetErrorMessage(ex.Message);
                
                throw new FaultException(exceptionErrorMessage);
            }
        }
        public string UploadUserDetails(string fileName)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                UploadUserInfoExcel asyncUploadUser = new UploadUserInfoExcel(InsertUserDetails);
                AsyncCallback callBack = new AsyncCallback(send);
                asyncUploadUser.BeginInvoke(db,fileName, callBack, null);
                return fileName;
            }
            catch (Exception ex)
            {
                string exceptionErrorMessage = Common.GetErrorMessage(ex.Message);
                throw new FaultException(exceptionErrorMessage);
            }
        }

        private void InsertUserDetails(Database db, string fileName)
        {

            OleDbConnection olDbObjConn = null;
            string path = ConfigurationManager.AppSettings["LogFileLocation"];
            string pathRead = path + fileName;

            String connectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + pathRead + "; Extended Properties='Excel 8.0;HDR=Yes'";


            olDbObjConn = new OleDbConnection(connectionString);

            using (DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                try
                {
                    string logFileName = fileName + "_tempSuccess.txt";
                    string targetFile = path + "//" + logFileName;

                    InsertUserInfo(db, targetFile, olDbObjConn);
                }
                catch (Exception ex)
                {
                    string txtFile = path + "//" + fileName + "_tempError.txt";
                    string exceptionErrorMessage = Common.GetErrorMessage(ex.Message);

                    using (FileStream errorFileStream = new FileStream(txtFile, FileMode.Create, FileAccess.Write))
                    {
                        using (StreamWriter errorStreamWriter = new StreamWriter(errorFileStream))
                        {
                            errorStreamWriter.Write(exceptionErrorMessage);
                            errorStreamWriter.Close();
                        }
                        errorFileStream.Close();
                    }
                }
                finally
                {
                    connection.Close();

                    if (olDbObjConn != null)
                        olDbObjConn.Close();


                }
            }
        }

        private void InsertUserInfo(Database db, string fileName, OleDbConnection olDbObjConn)
        {
            int success = 0;
            int updated = 0;
            int failure = 0;
            int deleted = 0;
            int lineNumber = 0;
            string successMessage = string.Empty;
            string updateMessage = string.Empty;
            string failureMessage = string.Empty;
            string deletedMessage = string.Empty;
            
           
            IDataReader dataReaderSite = null;
            IDataReader dataReaderUser = null;
            FileStream logFileStream = null;
            StreamWriter logStreamWriter = null;

            bool missingColumnFound = false;
            System.Data.Common.DbTransaction transaction;
            string path = ConfigurationManager.AppSettings["LogFileLocation"].TrimEnd('\\');
            using (System.Data.Common.DbConnection connection = db.CreateConnection())
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {

                    OleDbDataAdapter oledbObject = new OleDbDataAdapter("Select * from [" + "UserInfoList" + "$]", olDbObjConn);

                    DataSet dtExcelDataSet = new DataSet();
                    oledbObject.Fill(dtExcelDataSet);
                    olDbObjConn.Close();
                    DataTable userInfoTable = dtExcelDataSet.Tables[0];

                    if (userInfoTable != null && userInfoTable.Rows.Count > 0)
                    {
                        lineNumber = 2;
                        string passwordErrorMessage = string.Empty;
                        string firstName = string.Empty;
                        string lastName = string.Empty;
                        string loginName = string.Empty;
                        string password = string.Empty;
                        string emailID = string.Empty;
                        string role = string.Empty;
                        bool isvalidMailID = true;
                        bool isUserAlreadyExists = false;
                        string hashedPassword = string.Empty;
                        string mobilenumber = string.Empty;


                        foreach (DataRow row in userInfoTable.Rows)
                        {
                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.firstName + "(*)"))
                                userInfoTable.Columns[Resources.UserInformation_Resource.firstName + "(*)"].ColumnName = Resources.UserInformation_Resource.firstName;

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.lastName + "(*)"))
                                userInfoTable.Columns[Resources.UserInformation_Resource.lastName + "(*)"].ColumnName = Resources.UserInformation_Resource.lastName;
                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.emailID + "(*)"))
                                userInfoTable.Columns[Resources.UserInformation_Resource.emailID + "(*)"].ColumnName = Resources.UserInformation_Resource.emailID;

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.MobileNumber + "(*)"))
                                userInfoTable.Columns[Resources.UserInformation_Resource.MobileNumber + "(*)"].ColumnName = Resources.UserInformation_Resource.MobileNumber;

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.password + "(*)"))
                                userInfoTable.Columns[Resources.UserInformation_Resource.password + "(*)"].ColumnName = Resources.UserInformation_Resource.password;

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.role + "(*)"))
                                userInfoTable.Columns[Resources.UserInformation_Resource.role + "(*)"].ColumnName = Resources.UserInformation_Resource.role;

                            passwordErrorMessage = string.Empty;
                            firstName = string.Empty;
                            lastName = string.Empty;
                            loginName = string.Empty;
                            password = string.Empty;
                            emailID = string.Empty;
                            isvalidMailID = true;
                            isUserAlreadyExists = false;
                            hashedPassword = string.Empty;
                            mobilenumber = string.Empty;
                            int userID = 0;

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.firstName))
                            {
                                firstName = Common.CleanHTMLTags(row[Resources.UserInformation_Resource.firstName].ToString().Trim());
                            }
                            else
                            {
                                missingColumnFound = true;
                                failureMessage += Resources.Service_Resource.missingColumn.Replace("[XXX]", Resources.UserInformation_Resource.firstName) + Environment.NewLine;
                                failure++;
                            }


                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.lastName))
                            {
                                lastName = Common.CleanHTMLTags(row[Resources.UserInformation_Resource.lastName].ToString().Trim());
                            }
                            else
                            {
                                missingColumnFound = true;
                                failureMessage += Resources.Service_Resource.missingColumn.Replace("[XXX]", Resources.UserInformation_Resource.lastName) + Environment.NewLine;
                                failure++;
                            }

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.emailID))
                            {
                                emailID = Common.CleanHTMLTags(row[Resources.UserInformation_Resource.emailID].ToString().Trim());
                            }
                            else
                            {
                                missingColumnFound = true;
                                failureMessage += Resources.Service_Resource.missingColumn.Replace("[XXX]", Resources.UserInformation_Resource.emailID) + Environment.NewLine;
                                failure++;
                            }


                            bool isPasswordFieldPresentInTemplate = false;
                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.password))
                            {
                                isPasswordFieldPresentInTemplate = true;
                                password = Common.CleanHTMLTags(row[Resources.UserInformation_Resource.password].ToString().Trim());
                            }
                            else
                            {
                                missingColumnFound = true;
                                failureMessage += Resources.Service_Resource.missingColumn.Replace("[XXX]", Resources.UserInformation_Resource.password) + Environment.NewLine;
                                failure++;
                            }

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.MobileNumber))
                            {
                                mobilenumber = Common.CleanHTMLTags(row[Resources.UserInformation_Resource.MobileNumber].ToString().Trim());
                            }
                            else
                            {
                                missingColumnFound = true;
                                failureMessage += Resources.Service_Resource.missingColumn.Replace("[XXX]", Resources.UserInformation_Resource.MobileNumber) + Environment.NewLine;
                                failure++;
                            }

                            if (userInfoTable.Columns.Contains(Resources.UserInformation_Resource.role))
                            {
                                role = Common.CleanHTMLTags(row[Resources.UserInformation_Resource.role].ToString().Trim());
                            }
                            else
                            {
                                missingColumnFound = true;
                                failureMessage += Resources.Service_Resource.missingColumn.Replace("[XXX]", Resources.UserInformation_Resource.role) + Environment.NewLine;
                                failure++;
                            }

                            if (missingColumnFound == true)
                            {
                                break;
                            }

                            if (firstName.Length > 0 || lastName.Length > 0 || password.Length > 0 || emailID.Length > 0)
                            {
                                if (firstName == string.Empty)
                                {
                                    failureMessage += Resources.UserInformation_Resource.EmptyFirstName.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                    failure++;
                                }
                                else if (firstName.Length > 100)
                                {
                                    failureMessage += Resources.UserInformation_Resource.FirstNameLengthError.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                    failure++;
                                }
                                else if (lastName.Length > 50)
                                {
                                    failureMessage += Resources.UserInformation_Resource.LastNameLengthError.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                    failure++;
                                }

                                else if (isPasswordFieldPresentInTemplate && password == string.Empty)
                                {
                                    failureMessage += Resources.UserInformation_Resource.EmptyPassword.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                    failure++;
                                }

                                else if (emailID == string.Empty)
                                {
                                    failureMessage += Resources.UserInformation_Resource.EmptyEmailID.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                    failure++;
                                }
                                else if (mobilenumber.Length > 10)
                                {
                                    failureMessage += Resources.UserInformation_Resource.MobileNumberLengthError.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                    failure++;
                                }

                                else
                                {
                                    bool isValidData = true;
                                    isvalidMailID = Regex.IsMatch(emailID, @"^([\w-\.]+@(?!hotmail.com)(?!yahoo.co.in)(?!aol.com)(?!abc.com)(?!xyz.com)(?!pqr.com)(?!rediffmail.com)(?!live.com)(?!outlook.com)(?!me.com)(?!msn.com)(?!ymail.com)([\w-]+\.)+[\w-]{2,4})?$", RegexOptions.IgnoreCase);
                                    if (!isvalidMailID)
                                    {
                                        isValidData = false;
                                        failureMessage += "Please enter your email address in line number :" + lineNumber + Environment.NewLine;
                                        failure++;
                                    }

                                    isvalidMailID = Regex.IsMatch(emailID, @"^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$", RegexOptions.IgnoreCase);
                                    if (!isvalidMailID)
                                    {
                                        isValidData = false;
                                        failureMessage += Resources.UserInformation_Resource.InValidEmailID.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                        failure++;
                                    }

                                    if (isValidData)
                                    {
                                        dataReaderUser = Registration.CheckEmailIDExist(db, transaction, 0, emailID);
                                        if (dataReaderUser.Read())
                                        {
                                            isUserAlreadyExists = true;
                                            userID = Convert.ToInt32(dataReaderUser["FUSERID"]);
                                        }
                                        dataReaderUser.Close();



                                        //bool isEmailIDAlreadyExist = false;

                                        //dataReaderUser = Registration.CheckEmailIDExist(db, transaction, userID, emailID);
                                        //if (dataReaderUser.Read())
                                        //{
                                        //    isEmailIDAlreadyExist = true;
                                        //    //userID= Convert.ToInt32(dataReaderUser["UserID"]);
                                        //}
                                        //dataReaderUser.Close();
                                        //if (isEmailIDAlreadyExist)
                                        //{
                                        //    failureMessage += Resources.UserInformation_Resource.EmailIDAlreadyExist.Replace("[XXX]", emailID).Replace("[YYY]", lineNumber.ToString()) + Environment.NewLine;
                                        //    failure++;
                                        //}
                                        //else if (isUserAlreadyExists == true)
                                        //{
                                        //    failureMessage += Resources.UserInformation_Resource.UserNameAlreadyExist.Replace("[XXX]", loginName).Replace("[YYY]", lineNumber.ToString()) + Environment.NewLine;
                                        //    failure++;
                                        //}
                                        if (isPasswordFieldPresentInTemplate == false && isUserAlreadyExists == false)
                                        {
                                            failureMessage += Resources.UserInformation_Resource.EmptyPassword.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                            failure++;
                                        }
                                        else if (isValidData && !string.IsNullOrEmpty(mobilenumber) && Registration.CheckMobileNoExist(db, mobilenumber, userID))
                                        {
                                            isValidData = false;
                                            failureMessage += Resources.UserInformation_Resource.PhoneNumberAlreadyExist.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                            failure++;
                                        }
                                        else
                                        {
                                            passwordErrorMessage = string.Empty;
                                            if (passwordErrorMessage.Length > 0)
                                            {
                                                failureMessage += passwordErrorMessage + " " + Resources.Service_Resource.InLineNumber.Replace("[XXX]", lineNumber.ToString()) + Environment.NewLine;
                                                failure++;
                                            }
                                            else
                                            {
                                                if (isUserAlreadyExists)
                                                {
                                                    Registration.UpdateUserInfo(db, transaction, userID, password, firstName, lastName, emailID, mobilenumber);
                                                    updateMessage += Resources.UserInformation_Resource.UserUpdated.Replace("[XXX]", lastName + " " + firstName).Replace("[YYY]", lineNumber.ToString()) + Environment.NewLine;
                                                    updated++;
                                                }
                                                else
                                                {
                                                    Registration.InsertUserInfo(db, transaction, password, firstName, lastName, emailID, mobilenumber, role);

                                                    successMessage += Resources.UserInformation_Resource.UserAdded.Replace("[XXX]", lastName + " " + firstName).Replace("[YYY]", lineNumber.ToString()) + Environment.NewLine;
                                                    success++;

                                                }
                                            }
                                        }

                                    }
                                }
                            }

                        

                        lineNumber++;
                    }
                    transaction.Commit();
                }

                    else
                    {
                        failureMessage += "No records found in excel sheet";
                        failure++;
                    }

                    using (logFileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                    {
                        using (logStreamWriter = new StreamWriter(logFileStream))
                        {
                            logStreamWriter.WriteLine("******************************************************************************************");
                            logStreamWriter.WriteLine(Resources.UserInformation_Resource.UserInformationUploadLog + ":" + DateTime.Now.ToString("yyyyMMddHHmmss"));
                            logStreamWriter.WriteLine("------------------------------------------------------------------------------------------");
                            logStreamWriter.WriteLine(Resources.Service_Resource.SccessfullyAdded + " : " + success.ToString());
                            logStreamWriter.WriteLine(Resources.Service_Resource.SuccessfullyUpdated + " : " + updated.ToString());
                            logStreamWriter.WriteLine(Resources.Service_Resource.SccessfullyDeleted + " : " + deleted.ToString());
                            logStreamWriter.WriteLine(Resources.Service_Resource.Failure + " : " + failure.ToString());
                            if (successMessage.ToString() != string.Empty)
                            {
                                logStreamWriter.WriteLine("---------------------------------------" + Resources.Service_Resource.AddedItems + "--------------------------------------");
                                logStreamWriter.WriteLine(successMessage);
                            }

                            if (updateMessage.ToString() != string.Empty)
                            {
                                logStreamWriter.WriteLine("---------------------------------------" + Resources.Service_Resource.UpdatedItems + "--------------------------------------");
                                logStreamWriter.WriteLine(updateMessage);
                            }

                            if (deletedMessage.ToString() != string.Empty)
                            {
                                logStreamWriter.WriteLine("---------------------------------------" + Resources.Service_Resource.DeletedItems + "--------------------------------------");
                                logStreamWriter.WriteLine(deletedMessage);
                            }

                            if (failureMessage.ToString() != string.Empty)
                            {
                                logStreamWriter.WriteLine("-------------------------------------" + Resources.Service_Resource.FailedItems + "------------------------------------");
                                logStreamWriter.WriteLine(failureMessage);
                            }
                            logStreamWriter.Close();
                        }
                        logFileStream.Close();

                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    transaction.Rollback();
                    throw;
                    
                
                }
                finally
                {
                    if (dataReaderSite != null && !dataReaderSite.IsClosed)
                        dataReaderSite.Close();

                    if (dataReaderUser != null && !dataReaderUser.IsClosed)
                        dataReaderUser.Close();

                    //if (dataReaderEmployeeID != null && !dataReaderEmployeeID.IsClosed)
                    //    dataReaderEmployeeID.Close();

                    if (logStreamWriter != null)
                        logStreamWriter.Close();

                    if (logFileStream != null)
                        logFileStream.Close();
                }
            }
        }


    }
}
