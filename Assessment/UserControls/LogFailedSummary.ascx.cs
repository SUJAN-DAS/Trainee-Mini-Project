using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Assessment.ServiceReference1;

namespace Assessment.UserControls
{
    public partial class LogFailedSummary : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void BindLogSummary(LogFailed log)
        {

            tdUserName.InnerText = log.UserName;
            tdPassword.InnerText = log.Password;
            tdTimeStamp.InnerText = log.Timestamp.ToString("g");
            tdFailedAttempt.InnerText = log.FailedAttemptsCount.ToString();



        }
    }
}