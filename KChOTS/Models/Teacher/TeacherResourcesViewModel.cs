using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KChOTS.Models.Resources;

namespace KChOTS.Models.Teacher {
    public class TeacherResourcesViewModel {

        public IEnumerable<ResourceDataModel> Resources { get; set; }

        public TeacherResourcesViewModel() { }

        public TeacherResourcesViewModel(int teacherId, KnowledgeChannelEntities db) {
            PopulateResourcesFromTeacherId(teacherId, db);
        }

        private void PopulateResourcesFromTeacherId(int teacherId, KnowledgeChannelEntities db) {
            var model = db.Resources.Where(x => x.TeacherID == teacherId);
            List<ResourceDataModel> resources = new List<ResourceDataModel>();
            foreach (var item in model)
            {
                resources.Add(new ResourceDataModel()
                {
                    ID = item.ID,
                    Name = item.Name,
                    Description = item.Description,
                    ResourceFilePath = item.ResourceFile,
                    DateCreated = (DateTime)item.DateCreated
                });
            }
            Resources = resources;
        }
    }
}