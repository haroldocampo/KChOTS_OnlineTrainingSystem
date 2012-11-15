using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KChOTS.Models.Feedback {
    public class FeedbackDataModel {
        public KChOTS.Feedback Feedback { get; set; }
        public KChOTS.Teacher Teacher { get; set; }
    }
}