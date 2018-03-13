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
        [HttpGet("sites")] //Get all Sites
        public List<Site> GetSites()
        {
           List<Site> sites = new List<Site>(); 

           string queryString = "SELECT * FROM BUILDING";
           
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
                            longitude = Double.Parse(reader[2].ToString()),
                            latitude = Double.Parse(reader[3].ToString())
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
        [HttpGet("machines/{site}/{room}")] //Get all Rooms By Site
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
        [HttpGet("technicians/{site}")] //Get all Rooms By Site
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

        [HttpGet("technicians")] //Get all Rooms By Site
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

        [HttpGet("tickets")]
        public List<Ticket> GetTickets() //Get All Tickets
        {
            List<Ticket> tickets = new List<Ticket>(); 

            string queryString = @"SELECT TECH.Firstname + ' ' + TECH.Lastname as Technician, 
            U.Firstname + ' ' + U.Lastname as Requester, M.MachineName, R.RoomNum,
            T.Description, T.RequestDateTime, T.CompletionDateTime, T.Ticketnum
            FROM TICKETS T
            JOIN TECHNICIANS TECH ON T.Technician = TECH.TechID
            JOIN USERS U ON T.[User] = U.UserID
            JOIN MACHINES M ON M.MachineID = T.Machine
            JOIN ROOMS R ON R.RoomID = T.RoomNum";
           
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
                            requestdate = DateTime.Parse(reader[5].ToString()),
                            completedate = Convert.ToDateTime(reader[6].ToString()),
                            ticketnum = Int32.Parse(reader[7].ToString())
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
        public Object GetSystemInfo() //Get specific machine info
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
        public Object GetSystemSoftware() //Get machine software info
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

        [HttpGet("opentickets/{uid}")] //Get MotD
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
        [HttpGet("closedtickets/{uid}")] //Get MotD
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

        [HttpPost("ticket/{user}")] //Add user
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

        [HttpPost("sites")] //Add sites
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

        [HttpPost("machine")] //Add machine
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
        [HttpPost("user/{uid}")] //Add user
        public ObjectResult CreateUser(string uid, [FromBody] User user)
        {          
            JObject resp = new JObject();

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

            return Ok(resp);
        }

        [HttpPost("upload/{uid}")] //Upload profile pic
        public ObjectResult UploadProfilePhoto(string uid)
        {          
            JObject resp = new JObject();

            var pathWithEnv = @"%USERPROFILE%\Desktop\SeniorProject\client\src\assets\ProfilePics\" + uid;
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

        [HttpGet("user/{uid}")]
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

        [HttpDelete("{id}")] //Delete API
        public void Delete(int id)
        {
        }
    }
}
