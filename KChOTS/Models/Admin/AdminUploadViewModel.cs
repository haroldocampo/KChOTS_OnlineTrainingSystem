using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KChOTS.Models.Resources;
using System.IO;

namespace KChOTS.Models.Admin {
    public class AdminUploadViewModel {

        public ResourceDataModel Resource { get; set; }

        public AdminUploadViewModel() { }

        public AdminUploadViewModel(AdminUploadViewModel model, KnowledgeChannelEntities db) {
            this.model = model;
        }

        public void SaveFile(AdminUploadViewModel model, HttpServerUtilityBase server, int adminId, KnowledgeChannelEntities db) {
            var file = model.Resource.ResourceFile;
            if (file.ContentLength > 0) {
                var admin = db.Teachers.Where(x => x.TeacherID == adminId).SingleOrDefault();
                var fileName = Path.GetFileName(file.FileName);
                var folderName = Path.Combine(server.MapPath("~/FileResources"), "KnowledgeChannel");
                var folderNameTosave = Path.Combine("FileResources", "KnowledgeChannel");
                if (!Directory.Exists(folderName)) {
                    Directory.CreateDirectory(folderName);
                }
                var pathFileServer = Path.Combine(folderName, fileName);
                file.SaveAs(pathFileServer);
                var pathDatabase = Path.Combine(folderNameTosave, fileName);
                SaveInDatabase(model, pathDatabase, adminId, db);
            }
        }

        private void SaveInDatabase(AdminUploadViewModel Model, string path, int adminId, KnowledgeChannelEntities db) {
            var resource = db.Resources.CreateObject();
            resource.DateCreated = DateTime.Now;
            resource.Name = Model.Resource.Name;
            resource.Description = Model.Resource.Description;
            resource.ResourceFile = path;
            resource.LevelID = 4;
            resource.AdminID = adminId;
            db.Resources.AddObject(resource);
            db.SaveChanges();
        }

        private AdminUploadViewModel model;
    }
}