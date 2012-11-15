using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KChOTS.Models.Admin
{
    public class AdminDataModel
    {
        public int AdminID { get; set; }
        public int AdminType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; }
        public string FirstNAme { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set;}
        public DateTime DateCreated { get; set; }
        
    }
}