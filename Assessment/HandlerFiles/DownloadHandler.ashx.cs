using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Assessment.HandlerFiles
{
    /// <summary>
    /// Summary description for DownloadHandler
    /// </summary>
    public class DownloadHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            // downloading all user info
            if (context.Request.QueryString["userInfoExcel"] != null)
            {
                string userInfo = context.Request.QueryString["userInfoExcel"];
                DownloadExcelFile(userInfo, context);
                return;
            }

            // downloading user info log file
            if (context.Request.QueryString["userInfoLogFile"] != null)
            {
                string logFileName = context.Request.QueryString["userInfoLogFile"];
                DownloadLogFile(logFileName, context);
                return;
            }
        }
        private void DownloadExcelFile(string excelFile, HttpContext context)
        {
            string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFileLocation"] + excelFile;
            if (logFilePath != null && logFilePath.Length > 4)
            {
                if (logFilePath.Contains(@"\"))
                {
                    if (File.Exists(logFilePath))
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFilePath);
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + excelFile);
                        context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        context.Response.TransmitFile(fileInfo.FullName);
                        context.Response.Flush();
                    }
                }
            }
        }

        private void DownloadLogFile(string CheckAndDownloadLogFile, HttpContext context)
        {
            //Log file reading
            string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFileLocation"] + CheckAndDownloadLogFile;
            if (logFilePath != null && logFilePath.Length > 4)
            {
                if (logFilePath.Contains(@"\"))
                {
                    if (File.Exists(logFilePath))
                    {
                        System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFilePath);
                        context.Response.Clear();
                        context.Response.ContentType = "application/octet-stream";
                        context.Response.AddHeader("Content-Disposition", "attachment; filename=" + CheckAndDownloadLogFile);
                        context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                        context.Response.TransmitFile(fileInfo.FullName);
                        context.Response.Flush();
                    }
                }
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