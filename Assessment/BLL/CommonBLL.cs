using System;
using System.Collections.Generic;
using System.Linq;
using Assessment.ServiceReference1;
using System.Web;

namespace Assessment.BLL
{
    public class CommonBLL
    {
        public static int RegistrationDetails(string firstName, string lastName, string emailId, string mobileNo, string password,string role)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                int test = service.RegistrationDetails(firstName, lastName, emailId, mobileNo, password,role);
                service.Close();
                return test;
            }

            catch (Exception ex)
            {
                service.Abort();
                throw;
            }
        }

        public static UserClass LoginDetails(string emailId, string password)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                UserClass output = service.LoginDetails(emailId, password);
                service.Close();
                return output;
            }
            catch(Exception ex)
            {
                service.Abort();
                throw;
            }
        }

        //LoginFailed
        public static UserClass LogFailedLoginAttempt(string emailId, string password)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                UserClass output = service.LogFailedLoginAttempt(emailId, password);
                service.Close();
                return output;
            }
            catch (Exception ex)
            {
                service.Abort();
                throw;
            }
        }

        public static int UserDetails(int userId,string city, string state, string country, string gender, DateTime dob, byte[] profileImage)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                int test = service.UserDetails(userId,city, state, country, gender, dob, profileImage);
                service.Close();
                return test;
            }

            catch (Exception ex)
            {
                service.Abort();
                throw;
            }
        }

        ///fetch data
        public static List<DataListInfo> GetAdminData()
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                List<DataListInfo> userList = service.GetAdminData().ToList();
                return userList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                service.Abort();
                throw;
            }
        }

        //Log Failed data
        public static List<LogFailed> GetLogInfo(string emailId, string password)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                List<LogFailed> output = service.GetLogInfo(emailId, password).ToList();
                service.Close();
                return output;
            }
            catch (Exception ex)
            {
                service.Abort();
                throw;
            }
        }
        //Log Failed Data
        //Search in Login Failed 
        public static List<LogFailed> SearchLogInfo(string username)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                List<LogFailed> output = service.SearchLogInfo(username).ToList();
                service.Close();
                return output;
            }
            catch (Exception ex)
            {
                service.Abort();
                throw;
            }
        }


            //Search in Login Failed 

        //update data
        public static EditData GetUserDetail(int userID)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                EditData userList = service.GetUserDetail(userID);
                return userList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                service.Abort();
                throw;
            }
        }

        public static bool UpdateUserInfo(EditData userInfo)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                bool output = service.UpdateUserInfo(userInfo);
                service.Close();
                return output;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                service.Abort();
                throw;
            }

        }

        //delete
        public static string DeleteUser(int userid)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                string result = service.DeleteUser(userid);
                service.Close();
                return result;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                service.Abort();
                throw;
            }
        }

        //Chnage the role from dropdown
        public static bool UpdateChangeRole(int userId, string role)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                bool roleChanged = service.UpdateChangeRole(userId,role);
                service.Close();
                return roleChanged;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                service.Abort();
                throw;
            }
        }
        //Chnage the role from dropdown

        //Load admin name in admin page
        public static string GetAdminFirstName(int userId)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                string adminFirstName= service.GetAdminFirstName(userId);
                service.Close();
                return adminFirstName;
                
            }
            catch (Exception ex)
            {
                service.Abort();
                throw ex;
            }

        }
        //Download and upload Excel
        public static string UploadUserDetails(string fileName)
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                string output = service.UploadUserDetails(fileName);
                service.Close();
                return output;
            }
            catch (Exception ex)
            {
                service.Abort();
                throw ex;
            }
        }

        public static string DownloadUserInfoTemplate()
        {
            ServiceReference1.UserServiceClient service = new ServiceReference1.UserServiceClient();
            try
            {
                string output = service.DownloadUserInfoTemplate();
                service.Close();
                return output;
            }
            catch (Exception ex)
            {
                service.Abort();
                throw ex;
            }
        }

    }
}