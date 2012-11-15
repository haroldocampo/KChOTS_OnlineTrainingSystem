using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KChOTS.Models.Resources;

namespace KChOTS.Models.Teacher {
    public class TeacherIndexViewModel {

        public TeacherIndexViewModel() { }

        public TeacherIndexViewModel(int teacherID, KnowledgeChannelEntities db) {
            // TODO: Complete member initialization
            _teacherID = teacherID;
            PopulateTeacher(teacherID, db);
            PopulateResourcesFromTeacherId(teacherID, db);
        }

        private void PopulateTeacher(int teacherID, KnowledgeChannelEntities db) {
            IQueryable<KChOTS.Teacher> allTeachers = db.Teachers;
            var teacher = allTeachers.Where(x => x.TeacherID == teacherID).SingleOrDefault();
            Teacher = new TeacherDataModel();
            Teacher.FirstName = teacher.FirstName;
            Teacher.LastName = teacher.LastName;
            Teacher.Email = teacher.Email;
        }

        public List<ResourceDataModel> Resources { get; set; }

        public TeacherDataModel Teacher { get; set; }

        public List<KChOTS.Feedback> Feedbacks { get; set; }

        private void PopulateResourcesFromTeacherId(int teacherId, KnowledgeChannelEntities db) {
            var model = db.Resources.Where(x => x.TeacherID == teacherId);
            // Resources
            PopulateResources(model);
            // Feedback Responses
            IQueryable<KChOTS.Feedback> allFeedbacks = db.Feedbacks;
            Feedbacks = allFeedbacks.Where(x => x.TeacherID == teacherId && x.IsResponded == true && x.IsRead == false).ToList();
        }

        private void PopulateResources(IQueryable<Resource> model) {
            List<ResourceDataModel> resources = new List<ResourceDataModel>();
            foreach (var item in model) {
                resources.Add(new ResourceDataModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    ResourceLevel = item.Level.LevelID,
                    ResourceSubject = item.Subject.SubjectID,
                    ResourceFilePath = item.ResourceFile
                });
            }
            Resources = resources;
        }

        private int _teacherID;
    }
}