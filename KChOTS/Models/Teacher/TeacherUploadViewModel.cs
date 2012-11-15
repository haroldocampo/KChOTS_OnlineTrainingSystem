using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KChOTS.Models.Resources;
using System.IO;
using System.Web.Mvc;

namespace KChOTS.Models.Teacher {
    public class TeacherUploadViewModel {

        public TeacherUploadViewModel() { }

        //POST
        public TeacherUploadViewModel(TeacherUploadViewModel model, KnowledgeChannelEntities db) {
            this.model = model;
            Message = "Thank you. Your file has been uploaded.";
            PopulateLevelsAndSubjects(db);
        }

        //VIEW
        public TeacherUploadViewModel(KnowledgeChannelEntities db) {
            PopulateLevelsAndSubjects(db);
        }

        public ResourceDataModel Resource { get; set; }
        public IEnumerable<SelectListItem> Subjects { get; set; }
        public IEnumerable<SelectListItem> Levels { get; set; }
        public string Message = "";

        public void SaveFile(TeacherUploadViewModel model, HttpServerUtilityBase server, int teacherId, KnowledgeChannelEntities db) {
            var file = model.Resource.ResourceFile;
            if (file.ContentLength > 0)
            {
                var teacher = db.Teachers.Where(x => x.TeacherID == teacherId).SingleOrDefault();
                var fileName = Path.GetFileName(file.FileName);
                var folderName = Path.Combine(server.MapPath("~/FileResources"), teacher.TeacherID.ToString());
                var folderNameTosave = Path.Combine("FileResources", teacher.TeacherID.ToString());
                if(!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);
                }
                var pathFileServer = Path.Combine(folderName, fileName);
                file.SaveAs(pathFileServer);
                var pathDatabase = Path.Combine(folderNameTosave, fileName);
                SaveInDatabase(model, pathDatabase, teacherId, db);
            }
        }

        private void SaveInDatabase(TeacherUploadViewModel Model,string path, int TeacherID, KnowledgeChannelEntities db) {
            var resource = db.Resources.CreateObject();
            resource.DateCreated = DateTime.Now;
            resource.Name = Model.Resource.Name;
            resource.Description = Model.Resource.Description;
            resource.ResourceFile = path;
            resource.LevelID = Model.Resource.ResourceLevel;
            resource.SubjectID = Model.Resource.ResourceSubject;
            resource.TeacherID = TeacherID;
            db.Resources.AddObject(resource);
            db.SaveChanges();
        }

        private void PopulateLevelsAndSubjects(KnowledgeChannelEntities db) {
            List<SelectListItem> subjects = new List<SelectListItem>();
            List<SelectListItem> levels = new List<SelectListItem>();
            foreach (var subject in db.Subjects.ToList())
            {
                subjects.Add(new SelectListItem() { Text = subject.SubjectName, Value = subject.SubjectID.ToString() });
            }
            foreach (var level in db.Levels.Where(x=>x.LevelID != 4).ToList())
            {
                levels.Add(new SelectListItem() { Text = level.LevelName, Value = level.LevelID.ToString() });
            }
            Subjects = subjects;
            Levels = levels;
        }

        private TeacherUploadViewModel model;
    }
}