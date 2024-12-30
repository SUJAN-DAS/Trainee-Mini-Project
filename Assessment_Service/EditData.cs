using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Assessment_Service
{
    [DataContract]
    public class EditData
    {
        [DataMember] public int UserId { get; set; }
        [DataMember] public string FirstName { get; set; }

        [DataMember] public string LastName { get; set; }

        [DataMember] public string Email { get; set; }

    }
}