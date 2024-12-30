using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Assessment_Service
{
    [DataContract]
    public class UserClass
    {
        [DataMember] public int UserId { get; set; }
        [DataMember] public int RoleId { get; set; }
        [DataMember] public string ErrorMessage { get; set; } = string.Empty;

    }
}