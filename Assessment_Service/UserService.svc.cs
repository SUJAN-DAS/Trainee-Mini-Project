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

namespace Assessment_Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "UserService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select UserService.svc or UserService.svc.cs at the Solution Explorer and start debugging.
    public class UserService : IUserService
    {
        //delegate void DataInfoExcel(Database db, string fileName);
        delegate void DownloadUserInfoExcelTemplate(Database db,string fileName);
        public int RegistrationDetails(string firstName, string lastName, string emailId, string mobileNo, string password)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.RegisterData(db, firstName, lastName, emailId, mobileNo, password);

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

        public List<DataListInfo> FetchUserData()
        {
            Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
            DataTable dataTable = Registration.GetUserData(db);
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
        public EditData GetUsers(int userID)
        {
            EditData users = null;
            Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");

            try
            {
                DataTable dataTable = Registration.GetUsers(db, userID);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    DataRow row = dataTable.Rows[0];
                    
                      users=new EditData
                      {
                             UserId = row["FUSERID"] != DBNull.Value ? Convert.ToInt32(row["FUSERID"]) : 0,
                             FirstName = row["FFNAME"].ToString(),
                             LastName = row["FLNAME"].ToString(),
                             Email = row["FEMAILID"].ToString(),
                          //Role = row["FROLEID"] != DBNull.Value ? Convert.ToInt32(row["FROLEID"]) : 0
                          //tt
                          //Role = row["FROLEID"].ToString()
                          //Role = row["RoleName"].ToString()
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
        public bool UpdateUserData(EditData userInfo)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.UpdateUserData(db, userInfo.UserId, userInfo.FirstName, userInfo.LastName, userInfo.Email);
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
        public string DeleteUserId(int userid)
        {
            try
            {
                Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
                return Registration.DeleteUserData(db, userid);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Error";
            }
        }

        //Download and Upload 

        //private static void send(IAsyncResult ar)
        //{
        //    string callerMethodName = string.Empty;
        //    try
        //    {
        //        AsyncResult result = (AsyncResult)ar;
        //        Type delegateType = result.AsyncDelegate.GetType();

        //        MethodInfo methodInfo = delegateType.GetMethod("EndInvoke");
        //        callerMethodName = methodInfo.DeclaringType.FullName;
        //        methodInfo.Invoke(result.AsyncDelegate, new object[] { ar });
        //    }
        //    catch (Exception ex)
        //    {
        //        //Common.LogException(ex, callerMethodName, "Error occured in async call", "iPAS_Service", System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty);
        //        Console.WriteLine(ex.Message);
        //    }
        //}
        //public string DownloadAllPPE()
        //{
        //    try
        //    {
        //        string fileName = "PPE_Excel_" + DateTime.Now.ToString("ddMMhhmmssffff");
        //        Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
        //        DataInfoExcel asyncLabels = new DataInfoExcel(GetPPEForExcel);
        //        AsyncCallback cb = new AsyncCallback(send);
        //        asyncLabels.BeginInvoke(db, fileName, cb, null);
        //        return fileName;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        //private void GetPPEForExcel(Database db, string fileName)
        //{
        //    // string path = ConfigurationManager.AppSettings["LogFileLocation"];
        //    string path = ConfigurationManager.AppSettings["LogFileLocation"];
        //    Workbook wBook = new Workbook();
        //    Worksheet wSheet = new Worksheet("PPEInformation");
        //    wBook.Worksheets.Add(wSheet);
        //    int columnIndex = 0;
        //    int rowIndex = 0;
        //    try
        //    {
        //        wSheet.Cells[rowIndex, columnIndex] = new Cell("First Name");
        //        columnIndex++;
        //        wSheet.Cells[rowIndex, columnIndex] = new Cell("Last Name");
        //        columnIndex++;
        //        wSheet.Cells[rowIndex, columnIndex] = new Cell("EmailId");
        //        columnIndex++;
        //        //wSheet.Cells[rowIndex, columnIndex] = new Cell("Mobile Number");
        //        //columnIndex++;
        //        //wSheet.Cells[rowIndex, columnIndex] = new Cell("Password");
        //        //columnIndex++;
        //        //wSheet.Cells[rowIndex, columnIndex] = new Cell("IsAdmin");
        //        //columnIndex++;
        //        //Binding Row data
        //        string completeFileName = fileName + ".xlsx";
        //        string targetFile = path + completeFileName;

        //        GetContentForPPE(db, targetFile, wBook, wSheet);

        //        string txtFile = path + "//" + fileName + "_tempSuccess.txt";
        //        using (FileStream fs2 = new FileStream(txtFile, FileMode.Create, FileAccess.Write))
        //        {
        //            using (StreamWriter sw2 = new StreamWriter(fs2))
        //            {
        //                sw2.Write("Excel File Successfully created.");
        //                sw2.Close();
        //            }
        //            fs2.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //string txtFile = path + "//" + fileName + "_tempError.txt";

        //        //string exceptionErrorMessage = Common.GetErrorMessage(ex.Message);
        //        //using (FileStream fs3 = new FileStream(txtFile, FileMode.Create, FileAccess.Write))
        //        //{
        //        //    using (StreamWriter sw3 = new StreamWriter(fs3))
        //        //    {
        //        //        sw3.Write(exceptionErrorMessage);
        //        //        sw3.Close();
        //        //    }
        //        //    fs3.Close();
        //        //}
        //        //Common.LogException(ex, "PlantService", "Error while downloading PPE", "iPAS_Service", System.Reflection.MethodBase.GetCurrentMethod().Name, " SiteID: " + basicParam.SiteID + ", UserID:" + basicParam.UserID);
        //        //throw new FaultException(exceptionErrorMessage);
        //        Console.WriteLine(ex.Message);
        //    }
        //}


        //trail
        //private void GetContentForPPE(Database db,string targetFile, Workbook wbook, Worksheet wsheet)
        //{
        //    IDataReader dataReaderPPE = null;
        //    try
        //    {
        //        dataReaderPPE = Registration.GetPPEForSendToExcel(db);
        //        string FirstName = string.Empty;
        //        string LastName = string.Empty;
        //        string EmailId = string.Empty;
        //        //string MobileNo = string.Empty;
        //        //string Password = string.Empty;
        //        //string Admin = string.Empty;
        //        int rowIndex = 1;
        //        int columnIndex = 0;
        //        while (dataReaderPPE.Read())
        //        {
        //            FirstName = dataReaderPPE["FFNAME"] != DBNull.Value ? dataReaderPPE["FFNAME"].ToString() : string.Empty;
        //            LastName = dataReaderPPE["FLNAME"] != DBNull.Value ? dataReaderPPE["FLNAME"].ToString() : string.Empty;
        //            EmailId = dataReaderPPE["FEMAILID"] != DBNull.Value ? dataReaderPPE["FEMAILID"].ToString() : string.Empty;

        //            wsheet.Cells[rowIndex, columnIndex] = new Cell(FirstName);
        //            columnIndex++;
        //            wsheet.Cells[rowIndex, columnIndex] = new Cell(LastName);
        //            columnIndex++;
        //            wsheet.Cells[rowIndex, columnIndex] = new Cell(EmailId);

        //            rowIndex++;
        //            columnIndex = 0;
        //        }

        //        dataReaderPPE.Close();
        //        wsheet.Cells.ColumnWidth[0] = 4000;
        //        wsheet.Cells.ColumnWidth[1] = 6000;
        //        wsheet.Cells.ColumnWidth[2] = 30000;
        //        wsheet.Cells.ColumnWidth[3] = 5000;
        //        wsheet.Cells.ColumnWidth[4] = 5000;

        //        //if (rowIndex < 100)
        //        //{
        //        //    int rows = 100 - rowIndex;
        //        //    for (int i = 0; i < rows; i++)            //Add some blank values -Excel dll has a bug. It doesn't open if number of rows are very less
        //        //    {
        //        //        columnIndex = 0;
        //        //        wsheet.Cells[rowIndex, columnIndex] = new Cell(" ");
        //        //        rowIndex++;
        //        //    }
        //        //}

        //        wbook.Save(targetFile);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw new FaultException(ex.Message);
        //    }
        //    finally
        //    {
        //        if (dataReaderPPE != null && !dataReaderPPE.IsClosed)
        //        {
        //            dataReaderPPE.Close();
        //        }
        //    }
        //}

        //#region Download and Upload

        //public string DownloadCreateUserInfoTemplate()
        //{
        //    try
        //    {
        //        string fileName = string.Empty;
        //        AsyncCallback cb = new AsyncCallback(send);
        //        Database db = DatabaseFactory.CreateDatabase("ApplicationConnection");
        //        fileName = "UserDetails_" + DateTime.Now.ToString("ddMMhhmmssffff");
        //        DownloadUserInfoExcelTemplate asyncDowntimeTemplate = new DownloadUserInfoExcelTemplate(GetCreateUserInfoTemplateExcel);
        //        asyncDowntimeTemplate.BeginInvoke(db,fileName, cb, null);

        //        return fileName;
        //    }
        //    catch
        //    {
        //        return string.Empty;
        //    }
        //}

        //private void GetCreateUserInfoTemplateExcel(Database db,string fileName)
        //{
        //    IDataReader dataReaderRoles = null;
        //    try
        //    {
        //        string path = string.Empty;
        //        int columnIndex = 1;
        //        int rowIndex = 1;

        //        path = ConfigurationManager.AppSettings["LogFileLocation"];
        //        string completeFileName = fileName + ".xlsx";
        //        string targetFile = path + completeFileName;
        //        bool IsSiteAdminVisible = false;

        //        FileInfo templateFile = new FileInfo(targetFile);
        //        if (templateFile.Exists)//for overwrite on existing file if exist.
        //        {
        //            templateFile.Delete();
        //            templateFile = new FileInfo(targetFile);
        //        }

        //        using (ExcelPackage package = new ExcelPackage(templateFile))
        //        {
        //            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");

        //            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(Resources.UserInformation_Resource.userInfo);
        //            BindCreateUserInfoTemplate(db,worksheet, colFromHex, package, rowIndex, columnIndex, userProfileFields, true, IsSiteAdminVisible);

        //            package.Save();
        //            package.Dispose();
        //        }

        //        string txtFile = path + "\\" + fileName + "_tempSuccess.txt";
        //        using (FileStream fStream = new FileStream(txtFile, FileMode.Create, FileAccess.Write))
        //        {
        //            using (StreamWriter sWriter = new StreamWriter(fStream))
        //            {
        //                sWriter.Write("Excel sheet downloaded successfully");
        //                sWriter.Close();
        //            }
        //            fStream.Close();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        //private void BindCreateUserInfoTemplate(Database db, ExcelWorksheet workSheet, Color colFromHex, ExcelPackage package, int rowIndex, int columnIndex, Plant.UserProfileFields userProfileFields, bool isTemplate, bool IsSiteAdminVisible)
        //{
        //    IDataReader dataReaderInfo = null;
        //    try
        //    {
        //        bool isOnlyNonMesUserAccess = false;
                
        //        List<string> customList = new List<string>();

        //        if (workSheet.Name == Resources.UserInformation_Resource.userInfo)
        //        {
        //            workSheet.Cells[rowIndex, columnIndex].Value ="First Name" + "(*)";
        //            iPAS_PalletInfo.Common.BindDataValidationForExcel(db, excelValidationInfo, workSheet, package, columnIndex,"First Name", string.Empty, 0, 18, "First Name", string.Empty, string.Empty, string.Empty, string.Empty, false, false, iPAS_PalletInfo.Common.ExcelDataValidation.TextLength);
        //            workSheet.Column(columnIndex).Width = 18;
        //            columnIndex++;

        //            workSheet.Cells[rowIndex, columnIndex].Value ="Last Name" + "(*)";
        //            iPAS_PalletInfo.Common.BindDataValidationForExcel(db, excelValidationInfo, workSheet, package, columnIndex,"Last Name", string.Empty, 0, 18,"Last Name", string.Empty, string.Empty, string.Empty, string.Empty, false, false, iPAS_PalletInfo.Common.ExcelDataValidation.TextLength);
        //            workSheet.Column(columnIndex).Width = 18;
        //            columnIndex++;

        //            if (userProfileFields.EnableEmailIDField)
        //            {
        //                workSheet.Cells[rowIndex, columnIndex].Value ="Email ID" + "(*)";
        //                iPAS_PalletInfo.Common.BindDataValidationForExcel(db, excelValidationInfo, workSheet, package, columnIndex, "Email ID", string.Empty, 0, 50, "Email ID", string.Empty, string.Empty, string.Empty, string.Empty, false, false, iPAS_PalletInfo.Common.ExcelDataValidation.TextLength);
        //                workSheet.Column(columnIndex).Width = 50;
        //                columnIndex++;
        //            }
        //        }

        //        using (ExcelRange headerColumnRange = workSheet.Cells[workSheet.Dimension.Address])
        //        {
        //            headerColumnRange.Style.Font.Bold = true;
        //            headerColumnRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
        //            headerColumnRange.Style.Fill.BackgroundColor.SetColor(colFromHex);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (dataReaderInfo != null && !dataReaderInfo.IsClosed)
        //            dataReaderInfo.Close();
        //    }
        //}

        //private static void send(IAsyncResult ar)
        //{
        //    string callerMethodName = string.Empty;
        //    try
        //    {
        //        AsyncResult result = (AsyncResult)ar;
        //        Type delegateType = result.AsyncDelegate.GetType();

        //        MethodInfo methodInfo = delegateType.GetMethod("EndInvoke");
        //        callerMethodName = methodInfo.DeclaringType.FullName;
        //        methodInfo.Invoke(result.AsyncDelegate, new object[] { ar });
        //    }
        //    catch (Exception ex)
        //    {
        //        string exceptionErrorMessage = Common.GetErrorMessage(ex.Message);
        //        Common.LogException(ex, callerMethodName, "Error occured in async call", "iPAS_Service", System.Reflection.MethodBase.GetCurrentMethod().Name, string.Empty);
        //        throw new FaultException(exceptionErrorMessage);
        //    }
        //}

        //#endregion


    }
    }
