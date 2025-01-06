using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assessment.HandlerFiles
{
    /// <summary>
    /// Summary description for UserInfoUploadHandler
    /// </summary>
    public class UserInfoUploadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
             try
            { var file = context.Request.Files[0];
              string[] tempFileName = file.FileName.Split('\\');
              string fileName = tempFileName[tempFileName.Length - 1];
              string ext = fileName.Substring(fileName.LastIndexOf('.') + 1).ToLower();
              int fileSize = context.Request.ContentLength;//file size in bytes
              string[] tempSaveName = fileName.Split('.');
              string tempSave = tempSaveName[0] + "_" + DateTime.Now.ToString("ddMMyyyyhhmmssffff"); ;
              string pathSave = System.Configuration.ConfigurationManager.AppSettings["LogFileLocation"] + tempSave + ".xls";
               file.SaveAs(pathSave);
               string logFile = string.Empty;
               logFile = BLL.CommonBLL.UploadUserDetails(tempSave);
              context.Response.Write("Success," + logFile);
              
             }
              catch (Exception ex)
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("false," + ex.Message);
                }
                finally
                {
                    context.Response.End();
                }
            }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}