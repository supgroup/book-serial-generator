using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;

using Newtonsoft.Json.Converters;

namespace SerialGenerator.ApiClasses
{

    public class daysremain
    {
        public Nullable<int> days { get; set; }
        public Nullable<int> hours { get; set; }
        public Nullable<int> minute { get; set; }
        public string expirestate { get; set; }
    }
    public class ProgramDetails
    {
        public int id { get; set; }
        public string programName { get; set; }
        public string versionName { get; set; }       
        public string serial { get; set; }
        public string customerDeviceCode { get; set; }
        public Nullable<System.DateTime> expireDate { get; set; }
        public Nullable<System.DateTime> activateDate { get; set; }
        public string packageNumber { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<bool> isLimitDate { get; set; }

        public bool isActive { get; set; }
        public string customerName { get; set; }
        public string customerLastName { get; set; }
        public string agentName { get; set; }
        public string agentLastName { get; set; }
        public string agentAccountName { get; set; }
        public string notes { get; set; }

    }
}
