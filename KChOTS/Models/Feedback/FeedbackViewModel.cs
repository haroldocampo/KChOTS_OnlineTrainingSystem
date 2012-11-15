using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KChOTS.Models.Feedback {
    public class FeedbackViewModel {

        public List<FeedbackDataModel> Feedback { get; set; }

        public FeedbackDataModel SpecificFeedback { get; set; }

        public FeedbackViewModel() { }

        public FeedbackViewModel(KnowledgeChannelEntities db) {
            _db = db;
            IQueryable<KChOTS.Feedback> feedbacks = _db.Feedbacks;
            List<FeedbackDataModel> model = new List<FeedbackDataModel>();
            foreach (var feedback in feedbacks.Where(x => x.IsResponded == false)) {
                KChOTS.Teacher teacher = _db.Teachers.Where(x => x.TeacherID == feedback.TeacherID).SingleOrDefault();
                model.Add(new FeedbackDataModel() { 
                    Feedback = feedback,
                    Teacher = teacher
                });
            }
            Feedback = model;
        }

        public FeedbackViewModel(KnowledgeChannelEntities db, int feedbackID, int teacherID) {
            _db = db;
            _feedbackId = feedbackID;
            _teacherId = teacherID;
            IQueryable<KChOTS.Feedback> feedbacks = _db.Feedbacks;
            var specific = feedbacks.Where(x => x.ID == _feedbackId && x.TeacherID == _teacherId).SingleOrDefault();
            specific.IsRead = true;
            SpecificFeedback = new FeedbackDataModel() { Feedback = specific };
            db.SaveChanges();
        }

        private int _feedbackId;
        private int _teacherId;
        private KnowledgeChannelEntities _db;
    }
}