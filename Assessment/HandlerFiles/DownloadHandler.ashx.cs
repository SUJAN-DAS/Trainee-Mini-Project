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

        //public void ProcessRequest(HttpContext context)
        //{
        //    context.Response.ContentType = "text/plain";
        //    context.Response.Write("Hello World");
        //    #region Register Detail
        //    // Download Register Detail
        //    if (context.Request.QueryString["type"] != null && context.Request.QueryString["type"] == "sujan")
        //    {
        //        string registerDetailFile = "RegisteredDetails.xlsx";
        //        DownloadTemplate(registerDetailFile, context);
        //        return;
        //    }
        //    #endregion
        //    if (context.Request.QueryString["sujan"] != null)
        //    {
        //        string preweighRulelExcel = context.Request.QueryString["sujan"];
        //        DownloadExcelFile(preweighRulelExcel, context);
        //        return;
        //    }
        //}

        //private void DownloadTemplate(string fileName, HttpContext context)
        //{
        //    string path = context.Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["ExcelFormatFolder"]);

        //    string filePath = path + fileName;
        //    if (filePath != null && filePath.Length > 4)
        //    {
        //        if (filePath.Contains(@"\"))
        //        {
        //            if (File.Exists(filePath))
        //            {
        //                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);

        //                context.Response.Clear();
        //                context.Response.ContentType = "application/octet-stream";
        //                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        //                context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        //                context.Response.TransmitFile(fileInfo.FullName);
        //                context.Response.Flush();
        //            }
        //        }
        //    }
        //}

        //private void DownloadLogFile(string CheckAndDownloadLogFile, HttpContext context)
        //{
        //    //Log file reading
        //    string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFileLocation"] + CheckAndDownloadLogFile;
        //    if (logFilePath != null && logFilePath.Length > 4)
        //    {
        //        if (logFilePath.Contains(@"\"))
        //        {
        //            if (File.Exists(logFilePath))
        //            {
        //                System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFilePath);
        //                context.Response.Clear();
        //                context.Response.ContentType = "application/octet-stream";
        //                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + CheckAndDownloadLogFile);
        //                context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        //                context.Response.TransmitFile(fileInfo.FullName);
        //                context.Response.Flush();
        //            }
        //        }
        //    }
        //}

        //private void DownloadExcelFile(string excelFile, HttpContext context)
        //{
        //    string logFilePath = System.Configuration.ConfigurationManager.AppSettings["LogFileLocation"] + excelFile;
        //    if (logFilePath != null && logFilePath.Length > 4)
        //    {
        //        if (logFilePath.Contains(@"\"))
        //        {
        //            if (File.Exists(logFilePath))
        //            {
        //                System.IO.FileInfo fileInfo = new System.IO.FileInfo(logFilePath);
        //                context.Response.Clear();
        //                context.Response.ContentType = "application/octet-stream";
        //                context.Response.AddHeader("Content-Disposition", "attachment; filename=" + excelFile);
        //                context.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
        //                context.Response.TransmitFile(fileInfo.FullName);
        //                context.Response.Flush();
        //            }
        //        }
        //    }
        //}

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}