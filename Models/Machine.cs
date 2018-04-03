using System;
using System.Collections.Generic;

namespace SeniorProject.Models
{
    public class Machine
    {
        public string computerid { get; set; }
        public string buildingid { get; set; }
        public string roomid { get; set; }
        public string computerName { get; set; }      
        public string operatingSystem { get; set; }
        public string architecture { get; set; }
        public string servicePack { get; set; }
        public double ram { get; set; }
        public long hdd { get; set; }  
        public long usedhdd { get; set; }  
        public string processor { get; set; }  
        public string serialnum { get; set; }  

    }
}