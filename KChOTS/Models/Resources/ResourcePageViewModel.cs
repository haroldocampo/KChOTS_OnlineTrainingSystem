using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KChOTS.Enums;

namespace KChOTS.Models.Resources
{
    public class ResourcePageViewModel
    {

        public ResourcePageViewModel(int levelId, int subjectId, KnowledgeChannelEntities knowledgeChannelEntities)
        {
            // TODO: Complete member initialization
            this.levelId = levelId;
            this.subjectId = subjectId;
            this.db = knowledgeChannelEntities;
        }

        public ResourcePageViewModel(KnowledgeChannelEntities knowledgeChannelEntities) {
            // TODO: Complete member initialization
            this.db = knowledgeChannelEntities;
            LevelName = "Knowledge Channel";
            SubjectName = "Resources";
            Resources = db.Resources.Where(x => x.LevelID == 4).ToList();
        }

        public string LevelName { get; set; }
        public string SubjectName { get; set; }

        public List<Resource> Resources { get; set; }

        internal void PopulateData()
        {
            Resources = db.Resources.Where(x => x.LevelID == levelId && x.SubjectID == subjectId).ToList();
            PopulateLevelName();
            switch (subjectId)
            {
                case 1:
                    SubjectName = "English";
                    break;
                case 2:
                    SubjectName = "Mathematics";
                    break;
                case 3:
                    SubjectName = "Science";
                    break;
                case 4:
                    SubjectName = "Filipino";
                    break;
                case 5:
                    SubjectName = "Araling Panlipunan";
                    break;
                case 6:
                    SubjectName = "TLE";
                    break;
                case 7:
                    SubjectName = "MAPEH";
                    break;
                case 8:
                    SubjectName = "Values Education";
                    break;
            }
        }

        private void PopulateLevelName() {
            switch (levelId)
            {
                case 1:
                    LevelName = "Primary";
                    break;
                case 2:
                    LevelName = "Elementary";
                    break;
                case 3:
                    LevelName = "Elementary";
                    break;
            }
        }

        private int levelId;
        private int subjectId;
        private KnowledgeChannelEntities db;
    }
}