using System;
using System.Collections.Generic;

namespace SeniorProject.Models
{
    public class Ticket
    {
        public int ticketnum { get; set; }
        public string technician { get; set; }
        public string requester { get; set; }
        public string site { get; set; }
        public string machine { get; set; }
        public int room { get; set; }
        public string description { get; set; }
        public DateTime requestdate { get; set; }
        public string completedate { get; set; }
    }
}