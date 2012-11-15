using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KChOTS.Models.Teacher {
    public class TeacherDataModel {
        public int TeacherID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public string ContactNumber { get; set; }
        public string Gender { get; set; }
        public int SchoolID { get; set; }
    }
}