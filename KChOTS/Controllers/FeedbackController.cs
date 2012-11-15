using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KChOTS.Models.Feedback;

namespace KChOTS.Controllers
{
    public class FeedbackController : KChOTSController
    {
        //
        // GET: /Feedback/

        public ActionResult Index()
        {
            if (!Application.IsAuthenticated)
            {
                ViewBag.Header = "Not yet logged in?";
                ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as a teacher to log-in.";
                return View("Message", "_LayoutGuest");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string subject, string message)
        {
            if (subject == "" || message == "")
            {
                ViewBag.Header = "Input incorrect";
                ViewBag.Message = "Either the subject or message of your feedback is not filled.";
                return View("Message", "_Layout");
            }

            Feedback feedback = new Feedback();
            feedback.Subject = subject;
            feedback.Message = message;
            feedback.TeacherID = Application.TeacherID;
            Application.Db.AddToFeedbacks(feedback);
            Application.Db.SaveChanges();
            ViewBag.Header = "Thank you!";
            ViewBag.Message = "Your concern has been sent to the administrators.";
            return View("Message", "_Layout");
        }

        public ActionResult Response(int id) {
            FeedbackViewModel model = new FeedbackViewModel(Application.Db, id, Application.TeacherID);
            return View(model);
        }

    }
}
