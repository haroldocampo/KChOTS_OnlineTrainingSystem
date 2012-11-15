using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KChOTS.Controllers
{
    public class UtilityController : KChOTSController
    {
        //
        // GET: /Utility/

        [HttpPost]
        public JsonResult AutoComplete(string searchText)
        {
            db = new KnowledgeChannelEntities();
            List<School> schoolList;
            IQueryable<School> schools = db.Schools;
            schoolList = schools.Where(x => x.SchoolName.Contains(searchText)).Take(5).ToList();
            return Json((from school in schoolList select new { SchoolID = school.SchoolID, SchoolName = school.SchoolName }));
        }

        public ActionResult Message(string Header, string Message) {
            ViewBag.Header = Header;
            ViewBag.Message = Message;
            return View("Message", "_LayoutGuest");
        }

        [HttpPost]
        public ActionResult VideoStream(string image) {
            return Json(new { Image = image }); 
        }

        #region Calendar Events
        [HttpPost]
        public ActionResult CreateEvent(string title, string start, string end) {
            Event calendarEvent = new Event();
            //Check if start has no value
            if (start == "") {
                return Json(new { success = false });
            }
            //Init date vars
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = end == "" ? startDate : DateTime.Parse(end); //If no value for End Date set to Start Date
            //Validate
            if (startDate > endDate) {
                return Json(new { success = false });
            }
            calendarEvent.Title = title;
            calendarEvent.StartDate = startDate;
            calendarEvent.EndDate = endDate;
            Application.Db.AddToEvents(calendarEvent);
            Application.Db.SaveChanges();
            return Json(new { success = true, title = title, start = start, end = end == "" ? start : end, item = calendarEvent.Id });
        }

        [HttpPost]
        public ActionResult SelectEvent(string eventID) {
            Session.Add("eventId", eventID);
            return null;
        }

        [HttpPost]
        public ActionResult DeleteEvent() {
            try {
                int eventId = int.Parse(Session["eventId"].ToString());
                Event calendarEvent = Application.Db.Events.Where(x => x.Id == eventId).SingleOrDefault();
                Application.Db.DeleteObject(calendarEvent);
                Application.Db.SaveChanges();
                return Json(new { success = true, item = eventId });
            }
            catch {
                return Json(new { success = false, });
            }
        }

        [HttpPost]
        public ActionResult ModifyEvent(string title, string start, string end) {
            int eventId = int.Parse(Session["eventId"].ToString());
            if (start == "" || eventId == 0) {
                return Json(new { success = false });
            }
            //Init date vars
            DateTime startDate = DateTime.Parse(start);
            DateTime endDate = end == "" ? startDate : DateTime.Parse(end); //If no value for End Date set to Start Date
            //Validate
            if (startDate > endDate) {
                return Json(new { success = false });
            }
            Event calendarEvent = Application.Db.Events.Where(x => x.Id == eventId).SingleOrDefault();
            calendarEvent.Title = title;
            calendarEvent.StartDate = startDate;
            calendarEvent.EndDate = endDate;
            Application.Db.SaveChanges();
            return Json(new { success = true, title = title, start = start, end = end == "" ? start : end, item = eventId });
        }

        public ActionResult Download(string filename) {
            Response.ContentType = "application/octet-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + filename);
            Response.TransmitFile(Server.MapPath("~/" + filename));
            return null;
        }

        public JsonResult Calendar() {
            IList<CEvent> events = new List<CEvent>();
            foreach (var item in Application.Db.Events)
            {
                events.Add(new CEvent()
                {
                    id = item.Id,
                    title = item.Title,
                    start = item.StartDate.ToShortDateString(),
                    end = item.EndDate.Value.ToShortDateString()
                    
                });
            }
            return Json(events, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult SelectReply(string FeedbackId) {
            Session.Add("FeedbackId", FeedbackId);
            return null;
        }

        [HttpPost]
        public ActionResult RespondReply(string message) {
            try {
                //Validate Response
                if (message == null) return Json(new { success = false, });

                //Main Logic
                int FeedbackId = int.Parse(Session["FeedbackId"].ToString());
                Feedback calendarEvent = Application.Db.Feedbacks.Where(x => x.ID == FeedbackId).SingleOrDefault();
                calendarEvent.Response = message;
                calendarEvent.IsResponded = true;
                calendarEvent.AdminID = Application.AdminID == 0 ? 3 : Application.AdminID;
                Application.Db.SaveChanges();
                return Json(new { success = true, item = FeedbackId });
            }
            catch {
                return Json(new { success = false, });
            }
        }

        public ActionResult Logout() {
            Session.Abandon();
            ViewBag.Header = "Good bye!";
            ViewBag.Message = "Thank you! You have been successfully logged out.";
            return View("Message", "_LayoutGuest");
        }

        private KnowledgeChannelEntities db;
    }

    public class CEvent{
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }

}
