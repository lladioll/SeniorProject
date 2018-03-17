using System;
using System.Collections.Generic;

namespace SeniorProject.Models
{
    public class Site
    {
        public string siteid { get; set; }  
        public string sitename { get; set; }      
        public Double longitude { get; set; }
        public Double latitude { get; set; }
        public int opentickets { get; set; }
        public int closedtickets { get; set; }
        public int totaltickets { get; set; }
        public int roomcount { get; set; }
        public int machinecount { get; set; }
        
    }
   
}