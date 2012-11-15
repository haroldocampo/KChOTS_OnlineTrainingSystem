using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KChOTS.Models.Resources {
    public class ResourceDataModel {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ResourceFilePath { get; set; }
        public DateTime DateCreated { get; set; }
        public string Status { get; set; }
        public int ResourceLevel { get; set; }
        public int ResourceSubject { get; set; }
        public int TeacherID { get; set; }
        public HttpPostedFileBase ResourceFile { get; set; }
    }
}