using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Management;
using Microsoft.Win32; 
using Microsoft.Management.Infrastructure;
using SeniorProject.Models;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace SeniorProject.Controllers
{
    [Route("api/[controller]")]
    public class SystemInfoController : Controller
    {
        public static string connectionString = @"";

        // GET api/values
        [HttpGet("sites")] //Get Site Info
        public List<SiteInfo> GetSites()
        {
           List<SiteInfo> sites = new List<SiteInfo>(); 

           string queryString = @"select B.BuildingId, buildingname, latitude, longitude,
            SUM(case when DateOfCompletion is not null then 1 else 0 end) as ClosedTickets,
            SUM(case when DateOfCompletion is null and t.buildingid is not null then 1 else 0 end) as OpenTickets,
            COUNT(t.BuildingID) as TotalTickets,
            (select COUNT(DISTINCT ROOMNUM) from room r where r.BuildingID = b.BuildingID) AS Rooms,
            (select COUNT(DISTINCT machineid) from machine m 
            JOIN BUILDING SUB ON SUB.BuildingID = M.BuildingID
            WHERE SUB.BuildingID = B.BuildingID) AS Machines
            FROM building b
            LEFT JOIN ticket t on b.BuildingID = t.BuildingID
            GROUP BY b.BuildingId, BuildingName, Latitude, Longitude;";


           
           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            sites.Add(new SiteInfo() { 
                            siteid = reader[0].ToString(),
                            sitename = reader[1].ToString(), 
                            latitude = Double.Parse(reader[2].ToString()),
                            longitude = Double.Parse(reader[3].ToString()),
                            opentickets = Int32.Parse(reader[4].ToString()),
                            closedtickets = Int32.Parse(reader[5].ToString()),
                            totaltickets = Int32.Parse(reader[6].ToString()),
                            roomcount = Int32.Parse(reader[7].ToString()),
                            machinecount = Int32.Parse(reader[8].ToString())
                            });
                        }
                    } 
                    
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
                
            return sites;
            }  
        }

        [HttpGet("sitelist")] //Get all Sites
        public List<Site> GetSiteList()
        {
           List<Site> sites = new List<Site>(); 

           string queryString = @"select BuildingId, buildingname, latitude, longitude from building";

           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            sites.Add(new Site() { 
                            siteid = reader[0].ToString(),
                            sitename = reader[1].ToString(),
                            latitude = Double.Parse(reader[2].ToString()),
                            longitude = Double.Parse(reader[3].ToString())
                            });
                        }
                    } 
                    
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
                
            return sites;
            }  
        }

         [HttpGet("service")] //Get All Services
        public List<Service> SiteService()
        {
           List<Service> services = new List<Service>(); 

           string queryString = @"Select * From Service";

           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            services.Add(new Service() { 
                            buildingid = reader[0].ToString(),
                            userid = reader[1].ToString(),
                            roomnum = reader[2].ToString()
                            });
                        }
                    } 
                    
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
     
                
            return services;
            }  
        }

        [HttpGet("site/{siteid}")] //Get Sites By Site ID
         public SiteInfo GetSite(string siteid)
        {
           SiteInfo site = new SiteInfo(); 

           string queryString = @"select B.BuildingId, buildingname, latitude, longitude,
            SUM(case when DateOfCompletion is not null then 1 else 0 end) as ClosedTickets,
            SUM(case when DateOfCompletion is null and t.buildingid is not null then 1 else 0 end) as OpenTickets,
            COUNT(t.BuildingID) as TotalTickets,
            (select COUNT(DISTINCT ROOMNUM) from room r where r.BuildingID = b.BuildingID) AS Rooms,
            (select COUNT(DISTINCT machineid) from machine m 
            JOIN BUILDING SUB ON SUB.BuildingID = M.BuildingID
            WHERE SUB.BuildingID = B.BuildingID) AS Machines
            FROM building b
            LEFT JOIN ticket t on b.BuildingID = t.BuildingID
            WHERE t.BUILDINGID = @buildingid
            GROUP BY b.BuildingId, BuildingName, Latitude, Longitude;";
     
           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                command.Parameters.AddWithValue("@buildingid", siteid);  
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            site.siteid = reader[0].ToString();
                            site.sitename = reader[1].ToString(); 
                            site.latitude = Double.Parse(reader[2].ToString());;
                            site.longitude = Double.Parse(reader[3].ToString());
                            site.opentickets = Int32.Parse(reader[4].ToString());
                            site.closedtickets = Int32.Parse(reader[5].ToString());
                            site.totaltickets = Int32.Parse(reader[6].ToString());
                            site.roomcount = Int32.Parse(reader[7].ToString());
                            site.machinecount = Int32.Parse(reader[8].ToString());
                        }
                    } 
                    
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
                
            return site;
            }  
        }

        [HttpGet("rooms/{site}")] //Get all Rooms By Site
        public List<Room> GetRooms(string site)
        {
           List<Room> rooms = new List<Room>(); 

           string queryString = "SELECT ROOMNUM FROM ROOM WHERE BUILDINGID = @buildingid";
           
           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                command.Parameters.AddWithValue("@buildingid", site);  
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            rooms.Add(new Room() { 
                            roomnum = reader[0].ToString()
                            });
                        }
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return rooms;
            }  
        }
        [HttpGet("rooms")] //Get all Rooms
        public List<Room> RoomsBuilding()
        {
           List<Room> rooms = new List<Room>(); 

           string queryString = "SELECT buildingname, r.BUILDINGID, ROOMNUM FROM ROOM r join building b on b.buildingid = r.buildingid";
           
           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {  
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            rooms.Add(new Room() {
                            buildingname = reader[0].ToString(), 
                            buildingid = reader[1].ToString(),
                            roomnum = reader[2].ToString()
                            });
                        }
                    } catch (Exception ex) {
                        Console.WriteLine(ex);
                    }
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return rooms;
            }  
        }
        [HttpGet("machines/{site}/{room}")] //Get Machines w/ Site and Room
        public List<SystemInfo> GetMachines(string site, string room)
        {
           List<SystemInfo> machines = new List<SystemInfo>(); 

           string queryString = "Select machineid, machinename From Machine where buildingid = @building and roomid = @roomnum ";
           
           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                command.Parameters.AddWithValue("@building", site); 
                command.Parameters.AddWithValue("@roomnum", room); 
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            machines.Add(new SystemInfo() { 
                            computerid = reader[0].ToString(),
                            computerName = reader[1].ToString()
                            });
                        }
                    }
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return machines;
            }  
        }
        [HttpGet("technicians/{site}")] //Get Technicians By Site
        public List<Tech> GetTechs(string site)
        {
           List<Tech> technicians = new List<Tech>(); 

           string queryString = 
           @"Select T.UserID, T.firstname, T.lastname From Technicians T
            JOIN Service S on S.Technician = T.UserID
            WHERE buildingid = @building";

           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                command.Parameters.AddWithValue("@building", site); 
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            technicians.Add(new Tech() { 
                            userid = reader[0].ToString(),
                            firstname = reader[1].ToString(),
                            lastname = reader[2].ToString()
                            });
                        }
                    }
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return technicians;
            }  
        }

        [HttpGet("technicians")] //Get All Technicians
        public List<Tech> GetAllTechs(string site)
        {
           List<Tech> technicians = new List<Tech>(); 

           string queryString = 
           @"Select T.UserID, T.firstname, T.lastname From Technicians T";

           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            technicians.Add(new Tech() { 
                            userid = reader[0].ToString(),
                            firstname = reader[1].ToString(),
                            lastname = reader[2].ToString()
                            });
                        }
                    }
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return technicians;
            }  
        }

        [HttpGet("machinelist")] //Get All Machines
        public List<Machine> GetAllMachines()
        {
           List<Machine> machines = new List<Machine>(); 

           string queryString = 
           @"Select [MachineID]
            ,[BuildingID]
            ,[RoomID]
            ,[MachineName]
            ,[OperatingSystem]
            ,[Architecture]
            ,[ServicePack]
            ,[RAM]
            ,[HDD]
            ,[TotalSpaceUsed]
            ,[SerialNum]
            ,[Processor] FROM MACHINE";

           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            machines.Add(new Machine() { 
                            computerid = reader[0].ToString(),
                            buildingid = reader[1].ToString(),
                            roomid = reader[2].ToString(),
                            computerName = reader[3].ToString(),
                            operatingSystem = reader[4].ToString(),
                            architecture = reader[5].ToString(),
                            servicePack = reader[6].ToString(),
                            ram = Double.Parse(reader[7].ToString()),
                            hdd = Int32.Parse(reader[8].ToString()),
                            usedhdd = Int32.Parse(reader[9].ToString()),
                            processor = reader[10].ToString(),
                            serialnum = reader[11].ToString()
                            });
                        }
                    }
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return machines;
            }  
        }

        [HttpGet("tickets")]
        public List<Ticket> GetTickets() //Get All Tickets
        {
            List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT   
            TECH.Firstname + ' ' + TECH.Lastname as Technician, 
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.ROOMNUM,
            T.Description, T.[DateOfRequest], T.[DateOfCompletion], T.Ticketnum, T.Notes, T.Status
            FROM TICKET T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.UserID
            JOIN USERS U ON T.[Requester] = U.UserID
            JOIN MACHINE M ON M.MachineID = T.MachineID
            JOIN ROOM R ON R.RoomNum = T.RoomNum AND R.BuildingID = T.BuildingID";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
            using (var command = new SqlCommand(queryString, connection)) {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
          
                try
                {
                    while (reader.Read())
                    {            
                        tickets.Add(new Ticket() 
                        {
                            technician = reader[0].ToString(),
                            requester = reader[1].ToString(), 
                            machine = reader[2].ToString(), 
                            room = Int32.Parse(reader[3].ToString()), 
                            description = reader[4].ToString(), 
                            requestdate = Convert.ToDateTime(reader[5].ToString()),
                            completedate =  reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6),
                            ticketnum = Int32.Parse(reader[7].ToString()),
                            notes = reader[8].ToString(),
                            status = reader[9].ToString()
                        });
                    }
                }
                
                finally
                {       
                    reader.Dispose();
                }  
            }
            return tickets;
            }   
        }
       
       [HttpGet("unassignedtickets")]
        public List<Ticket> GetUnassignedTickets() //Get All Unassigned Tickets
        {
            List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT    
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.ROOMNUM,
            T.Description, T.[DateOfRequest], T.[DateOfCompletion], T.Ticketnum, T.Notes, T.Status
            FROM TICKET T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.UserID
            JOIN USERS U ON T.[Requester] = U.UserID
            JOIN MACHINE M ON M.MachineID = T.MachineID
            JOIN ROOM R ON R.RoomNum = T.RoomNum AND R.BuildingID = T.BuildingID
            WHERE TECHNICIAN IS NULL;";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
            using (var command = new SqlCommand(queryString, connection)) {
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
          
                try
                {
                    while (reader.Read())
                    {            
                        tickets.Add(new Ticket() 
                        {
                            requester = reader[0].ToString(), 
                            machine = reader[2].ToString(), 
                            room = Int32.Parse(reader[3].ToString()), 
                            description = reader[4].ToString(), 
                            requestdate = Convert.ToDateTime(reader[5].ToString()),
                            completedate =  reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6),
                            ticketnum = Int32.Parse(reader[7].ToString()),
                            notes = reader[8].ToString(),
                            status = reader[9].ToString()
                        });
                    }
                }
                
                finally
                {       
                    reader.Dispose();
                }  
            }
            return tickets;
            }   
        }

        [HttpGet("machine")] 
        public Object GetSystemInfo() //Get Specific Machine Info
        {
            var localIP = WebHelpers.GetLocalIP;
            Console.WriteLine(localIP);
    
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            SystemInfo si = new SystemInfo();

            ManagementScope scope = new ManagementScope("\\\\" + localIP + "\\root\\cimv2", options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();

            foreach ( ManagementObject m in queryCollection)
            {
                si.computerName = m["csname"].ToString(); //Display Computer Name
                si.operatingSystem = m["Caption"].ToString(); //Display OS Type
                si.architecture=m["OSArchitecture"].ToString(); //Display operating system architecture.
                if (m["CSDVersion"] != null)
                {
                    si.servicePack = m["CSDVersion"].ToString(); //Display operating system version.
                }
                si.ram = Math.Round(Convert.ToDouble(m["TotalVisibleMemorySize"].ToString()) / (1024*1024),2); //Display RAM
            }        

            RegistryKey processor_name = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor\0", 
            RegistryKeyPermissionCheck.ReadSubTree);

            if (processor_name != null)
            {
                if (processor_name.GetValue("ProcessorNameString") != null)
                {
                    si.processor = processor_name.GetValue("ProcessorNameString").ToString();
                }
            }

            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            si.hdd= Convert.ToInt64(disk["Size"].ToString()) / 1073741824;
            si.usedhdd = Convert.ToInt64(disk["FreeSpace"].ToString()) / 1073741824;


            ObjectQuery bios = new ObjectQuery("select * from Win32_Bios");
            
            ManagementObjectSearcher biossearch = new ManagementObjectSearcher(scope, bios);

            ManagementObjectCollection bioscollection = biossearch.Get();

            foreach ( ManagementObject m in bioscollection)
            {
                string serial = m["SerialNumber"] as string;
                if (serial != null)
                si.serialnum = serial;
            }
            
            return si;
        }

        [HttpGet("software")] 
        public Object GetSystemSoftware() //Get Machine Software Info
        {
            var localIP = WebHelpers.GetLocalIP;
    
            ConnectionOptions options = new ConnectionOptions();
            options.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            ManagementScope scope = new ManagementScope("\\\\" + localIP + "\\root\\cimv2", options);
            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Product");
            
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

            ManagementObjectCollection queryCollection = searcher.Get();

            List<Software> software = new List<Software>(); 

            foreach ( ManagementObject m in queryCollection)
            {
                software.Add(new Software() {
                software = m["Name"].ToString()
            });
        }      
            
            return software;
        }

        [HttpGet("motd")] //Get MotD
        public Object GetMotD()
        {
            Message message = new Message();
           string queryString = "SELECT Message FROM MessageOfTheDay";
           
           using (SqlConnection connection = new SqlConnection(connectionString))  
           {    
              
                using (var command = new SqlCommand(queryString, connection)) {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
            
                try
                    {
                        while (reader.Read())
                        {            
                            message = new Message() {
                                message = reader[0].ToString()
                            };
                        }
                    } 
                    
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            return message;
            }  
        }

        [HttpGet("opentickets/{uid}")] //Get All Open Tickets By Technician
        public List<Ticket> GetOpenTickets(string uid)
        {
            List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT   
            TECH.Firstname + ' ' + TECH.Lastname as Technician, 
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.ROOMNUM,
            T.Description, T.[DateOfRequest], T.Ticketnum
            FROM TICKET T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.UserID
            JOIN USERS U ON T.[Requester] = U.UserID
            JOIN MACHINE M ON M.MachineID = T.MachineID
            JOIN ROOM R ON R.RoomNum = T.RoomNum AND R.BuildingID = T.BuildingID
            WHERE T.Requester = @uid and T.[DateOfCompletion] is null";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
            using (var command = new SqlCommand(queryString, connection)) {
            command.Parameters.AddWithValue("@uid", uid); 
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
          
                try
                {
                    while (reader.Read())
                    {            
                        tickets.Add(new Ticket() 
                        {
                            technician = reader[0].ToString(),
                            requester = reader[1].ToString(), 
                            machine = reader[2].ToString(), 
                            room = Int32.Parse(reader[3].ToString()), 
                            description = reader[4].ToString(), 
                            requestdate = DateTime.Parse(reader[5].ToString()),
                            ticketnum = Int32.Parse(reader[6].ToString())
                        });
                    }
                }
                
                finally
                {       
                    reader.Dispose();
                }  
            }
            return tickets;
            }   
        }
        [HttpGet("closedtickets/{uid}")] //Get Closed Tickets By Technician
        public List<Ticket> GetClosedTickets(string uid)
        {
            List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT   
            TECH.Firstname + ' ' + TECH.Lastname as Technician, 
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.ROOMNUM,
            T.Description, T.[DateOfRequest], T.Ticketnum, T.dateofcompletion, T.notes
            FROM TICKET T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.UserID
            JOIN USERS U ON T.[Requester] = U.UserID
            JOIN MACHINE M ON M.MachineID = T.MachineID
            JOIN ROOM R ON R.RoomNum = T.RoomNum AND R.BuildingID = T.BuildingID
            WHERE T.Requester = @uid and T.[DateOfCompletion] is not null";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
            using (var command = new SqlCommand(queryString, connection)) {
            command.Parameters.AddWithValue("@uid", uid); 
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
          
                try
                {
                    while (reader.Read())
                    {            
                        tickets.Add(new Ticket() 
                        {
                            technician = reader[0].ToString(),
                            requester = reader[1].ToString(), 
                            machine = reader[2].ToString(), 
                            room = Int32.Parse(reader[3].ToString()), 
                            description = reader[4].ToString(), 
                            requestdate = DateTime.Parse(reader[5].ToString()),
                            ticketnum = Int32.Parse(reader[6].ToString()),
                            completedate = DateTime.Parse(reader[7].ToString()),
                            notes = reader[8].ToString()
                        });
                    }
                }
                
                finally
                {       
                    reader.Dispose();
                }  
            }
            return tickets;
            }   
        }

        [HttpGet("openticketcount/{uid}")] //Get All Open Tickets by Technician
        public JsonResult GetOpenTicketCount(string uid)
        {
            int ticketcount = 0; 

            string queryString = @"SELECT   
            Count(*)
            FROM TICKET
            WHERE Technician = @uid and [DateOfCompletion] is null";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
            using (var command = new SqlCommand(queryString, connection)) {
            command.Parameters.AddWithValue("@uid", uid); 
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
          
                try
                {
                    while (reader.Read())
                    {            
                        ticketcount = Convert.ToInt32(reader[0].ToString());
                    }
                }
                
                finally
                {       
                    reader.Dispose();
                }  
            }
            return Json(ticketcount);
            }   
        }

        [HttpGet("ticketsbytech/{uid}")] //ALl Tickets By Individual Technicians
        public List<Ticket> GetTicketsByTech(string uid) 
        {
            List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT   
            TECH.Firstname + ' ' + TECH.Lastname as Technician, 
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.ROOMNUM,
            T.Description, T.[DateOfRequest], T.[DateOfCompletion], T.Ticketnum, T.Notes, T.Status
            FROM TICKET T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.UserID
            JOIN USERS U ON T.[Requester] = U.UserID
            JOIN MACHINE M ON M.MachineID = T.MachineID
            JOIN ROOM R ON R.RoomNum = T.RoomNum AND R.BuildingID = T.BuildingID
            WHERE T.Technician = @uid;";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  {    
              
            using (var command = new SqlCommand(queryString, connection)) {
            command.Parameters.AddWithValue("@uid", uid); 
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
          
                try
                {
                    while (reader.Read())
                    {            
                        tickets.Add(new Ticket() 
                        {
                            technician = reader[0].ToString(),
                            requester = reader[1].ToString(), 
                            machine = reader[2].ToString(), 
                            room = Int32.Parse(reader[3].ToString()), 
                            description = reader[4].ToString(), 
                            requestdate = DateTime.Parse(reader[5].ToString()),
                            completedate = reader.IsDBNull(6) ? null : (DateTime?)reader.GetDateTime(6),
                            ticketnum = Int32.Parse(reader[7].ToString()),
                            notes = reader[8].ToString(),
                            status = reader[9].ToString()
                        });
                    }
                }
                
                finally
                {       
                    reader.Dispose();
                }  
            }
            return tickets;
            }   
        }

        [HttpPost("ticket/{user}")] //Post Tickets
        public ObjectResult CreateTicket(string user, [FromBody] Ticket ticket)
        {          
            JObject resp = new JObject();

            string ticketInsert = 
            @"INSERT INTO Ticket (
            [Technician]
            ,[Requester]
            ,[BuildingID]
            ,[RoomNum]
            ,[MachineID]
            ,[Description]
            ,[DateOfRequest])
            VALUES (@tech, @requester, @building, @room, @machine, @description, @dor);";
     
            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                ticketInsert, connection);
                command.Parameters.AddWithValue("@tech", ticket.technician);  
                command.Parameters.AddWithValue("@requester", user);  
                command.Parameters.AddWithValue("@building", ticket.site);   
                command.Parameters.AddWithValue("@room", ticket.room);
                command.Parameters.AddWithValue("@machine", ticket.machine);  
                command.Parameters.AddWithValue("@description", ticket.description);  
                command.Parameters.AddWithValue("@dor", SqlDbType.DateTime).Value = DateTime.Now;   
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }

            return Ok(resp); 
        }  

        [HttpPost("sites")] //Post Sites
        public void AddSite([FromBody] Site newsite) 
        {
            string queryString = 
            @"INSERT INTO Building (BuildingName, Longitude, Latitude) 
            VALUES (@buildingname, @longitude, @latitude);";
       
            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                queryString, connection);
                command.Parameters.AddWithValue("@buildingname", newsite.sitename);  
                command.Parameters.AddWithValue("@longitude", newsite.longitude);  
                command.Parameters.AddWithValue("@latitude", newsite.latitude);     
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
        } 

        [HttpPost("machine")] //Post Machine
        public void AddMachine([FromBody] SystemInfo machine) 
        {

            string queryString = 
            @"INSERT INTO Machines ([MachineName]
            ,[OperatingSystem]
            ,[Architecture]
            ,[RAM]
            ,[HDD]
            ,[Processor]
            ,[TotalSpaceUsed]
            ,[SerialNum]) 
            VALUES (@machinename, @os, @arch, @ram, @hdd, @processor, @usedspace, @serialnum);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
    
                SqlCommand command = new SqlCommand(
                queryString, connection);
                command.Parameters.AddWithValue("@machinename", machine.computerName);  
                command.Parameters.AddWithValue("@os", machine.operatingSystem);  
                command.Parameters.AddWithValue("@arch", machine.architecture);   
                command.Parameters.AddWithValue("@ram", machine.ram);  
                command.Parameters.AddWithValue("@hdd", machine.hdd);  
                command.Parameters.AddWithValue("@processor", machine.processor);  
                command.Parameters.AddWithValue("@usedspace", machine.usedhdd);  
                command.Parameters.AddWithValue("@serialnum", machine.serialnum);   
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
                
            }
        }
        [HttpPost("user/{uid}")] //Post USer
        public JsonResult CreateUser(string uid, [FromBody] User user)
        {                 
            string UserInsert = 
            @"INSERT INTO Users (UserID, FirstName, LastName, Email, Role, DateRegistered)
            VALUES (@uid, @firstname, @lastname, @email, @role, @registered);";
   
            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                UserInsert, connection);
                command.Parameters.AddWithValue("@uid", uid);  
                command.Parameters.AddWithValue("@firstname", user.firstname);  
                command.Parameters.AddWithValue("@lastname", user.lastname);   
                command.Parameters.AddWithValue("@email", user.email);
                command.Parameters.AddWithValue("@role", user.role);  
                command.Parameters.AddWithValue("@registered", SqlDbType.DateTime).Value = DateTime.Now;   
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }

            return Json(user.role);
        }

        [HttpPost("upload/{uid}")] //Upload profile pic
        public ObjectResult UploadProfilePhoto(string uid)
        {          
            JObject resp = new JObject();

            var pathWithEnv = @"C:\ProfilePics\" + uid;
            var filePath = Path.Combine(Environment.ExpandEnvironmentVariables(pathWithEnv), "profile.jpg");

            foreach (var file in Request.Form.Files)
            {
                Console.WriteLine(file.FileName);
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
                fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            string UserInsert = 
            @"UPDATE Users SET 
            ProfilePic = @path 
            WHERE
            UserID = @uid;";
   
            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                UserInsert, connection);
                command.Parameters.AddWithValue("@uid", uid);  
                command.Parameters.AddWithValue("@path", filePath);    
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
            return Ok(resp);
        }

        [HttpGet("profilephoto/{uid}")] //Get profile pic
        public ActionResult GetProfilePhoto(string uid)
        {          
            string queryString = 
            @"Select ProfilePic from Users
            WHERE
            UserID = @uid;";

            var profileLocation = "";
            
            using (SqlConnection connection = new SqlConnection(connectionString)) 
            {    
              
                using (var command = new SqlCommand(queryString, connection)) 
                {
                    command.Parameters.AddWithValue("@uid", uid); 
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                
                    try
                    {
                        while (reader.Read())
                        {            
                            profileLocation = reader[0].ToString();
                        }
                    }
                    
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
            }
            Console.WriteLine(profileLocation);
            FileStream fs = new FileStream(profileLocation, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] imgdata = br.ReadBytes((int)fs.Length);
            br.Close();
            fs.Close();

            var result = Convert.ToBase64String(imgdata);

            return File(System.Convert.FromBase64String(result), "image/gif");
        }

        [HttpGet("twilio")] //Send Text
        public void AssignTicket()
        {
            var accountSid = "";
            var authToken = "";   

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                to: new PhoneNumber("+"),
                from: new PhoneNumber("+ "),
                body: "You have been assigned a new ticket!"
            );
        }

        [HttpGet("user/{uid}")] //Get User
        public Object GetUser(string uid)
        {          
            string queryString = 
            @"SELECT UserId, FirstName, LastName, Email, ProfilePic, Role, DateRegistered From Users
            WHERE UserID = @uid";
           
            using (SqlConnection connection = new SqlConnection(connectionString))  
            {    
                User user = new User();
                using (var command = new SqlCommand(queryString, connection)) 
                {
                    command.Parameters.AddWithValue("@uid", uid);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                
                    try
                    {
                        while (reader.Read())
                        {            
                            user = new User() {
                                userid = reader[0].ToString(),
                                firstname = reader[1].ToString(),
                                lastname = reader[2].ToString(),
                                email = reader[3].ToString(),
                                profilepic = reader[4].ToString(),
                                role = reader[5].ToString(),
                                registerdate = Convert.ToDateTime(reader[6].ToString())
                            };
                        }
                    }
                
                    finally
                    {       
                        reader.Dispose();
                    }  
                }
                return user;
            }
        }

        [HttpPut("updateticket")] //Update Ticket
        public ObjectResult UpdateTicket([FromBody] Ticket ticketinfo)
        {
            JObject resp = new JObject();

            string queryString = 
            @"UPDATE TICKET SET 
            [Status] = @status,
            [DateOfCompletion] = @completedate,
            [LastModified] = @modifieddate,
            [Notes] = @notes
            where [TicketNum] = @ticketnum;";
   
            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                queryString, connection);
                command.Parameters.AddWithValue("@status", ticketinfo.status);  
                command.Parameters.AddWithValue("@completedate", DateTime.Now); 
                command.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                command.Parameters.AddWithValue("@notes", ticketinfo.notes);
                command.Parameters.AddWithValue("@ticketnum", ticketinfo.ticketnum);   
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
        return Ok(resp);
        }

        [HttpDelete("deleteticket/{ticketnum}")] //Delete Ticket
        public ObjectResult DeleteTicket(string ticketnum)
        {
            JObject resp = new JObject();

            string queryString = 
            @"Delete from ticket where ticketnum = @ticketnum;";
   
            using (SqlConnection connection = new SqlConnection(connectionString))
            {    
                SqlCommand command = new SqlCommand(
                queryString, connection);
                command.Parameters.AddWithValue("@ticketnum", ticketnum);   
                connection.Open();
                command.ExecuteNonQuery();
                connection.Dispose();
            }
        return Ok(resp);
        }
    }
}
