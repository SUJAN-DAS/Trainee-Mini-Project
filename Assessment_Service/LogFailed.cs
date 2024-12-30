using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Assessment_Service
{
    [DataContract]
    public class LogFailed
    {
        [DataMember] public string UserName { get; set; }
        [DataMember] public string Password { get; set; }
        [DataMember] public DateTime Timestamp { get; set; }
        [DataMember] public int FailedAttemptsCount { get; set; }
        


    }
}