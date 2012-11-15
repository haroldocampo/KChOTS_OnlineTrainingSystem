using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KChOTS.Models.Training;
using KChOTS.Models.Feedback;
using KChOTS.Models.Admin;
using KChOTS.Security;

namespace KChOTS.Controllers
{
    public class AdminController : KChOTSController
    {
        //
        // GET: /Admin/

        public ActionResult Index() {
            if (!Application.IsAuthenticated && Application.AdminID != 0) {
                ViewBag.Header = "Not yet logged in?";
                ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as an administrator to log-in.";
                return View("Message", "_LayoutGuest");
            }
            AdminDataModel model = new AdminDataModel()
            {
                AdminID = Application.AdminID,
                AdminType = Application.Db.Admins.Where(a => a.AdminID == Application.AdminID).SingleOrDefault().AdminType.Value
            };
            return View(model);
        }

        public ActionResult ViewAdmins()
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }
            using (KnowledgeChannelEntities context = new KnowledgeChannelEntities())
            { 
                IEnumerable<Admin> List = context.Admins.ToList();
                return View(List);
            }
        }

        //Harold's Code
        public ActionResult Training() {
            if (!Application.IsAuthenticated && Application.AdminType != 3) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutAdmin");
            }
            TrainingViewModel model = new TrainingViewModel("Administrator");
            return View(model);
        }

        //Create

        public ActionResult CreateAdmin()
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutAdmin");
            }
            return View();
        }


        [HttpPost]
        public ActionResult CreateAdmin(Admin admin)
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }

            if (!admin.Email.IsEmail()) {
                ViewBag.Header = "Inputs were incorrect";
                ViewBag.Message = "Please go back to the form and input the correct data";
                return View("Message", "_LayoutAdmin");
            }

            //Airah's Code
            using (KnowledgeChannelEntities context = new KnowledgeChannelEntities()) {
                
                if (ModelState.IsValid) {
                    admin.Password = Encrypt.ComputeHash(admin.Password, "SHA512", null);
                    admin.DateCreated = DateTime.Now;
                    context.AddToAdmins(admin);
                    context.SaveChanges();
                    return RedirectToAction("ViewAdmins");
                }
            }

            return View();
        }

        //Edit

        public ActionResult EditAdmins(int id)
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }

            Admin admin = Application.Db.Admins.Where(a => a.AdminID == id).SingleOrDefault();
            return View(admin);
        }

        [HttpPost]
        public ActionResult EditAdmin(Admin model)
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutAdmin");
            }

            if (!model.Email.IsEmail()) {
                ViewBag.Header = "Inputs were incorrect";
                ViewBag.Message = "Please go back to the form and input the correct data";
                return View("Message", "_LayoutAdmin");
            }

            using (KnowledgeChannelEntities context = new KnowledgeChannelEntities())
            {
                if (model.Username == null && model.Password == null) {
                    ModelState.AddModelError("NullError", "Username and Password fields cannot be empty");
                }
                if (ModelState.IsValid)
                {   
                    Admin admin = context.Admins.Where(a => a.AdminID == model.AdminID).Single();
                    admin.AdminType = model.AdminType;
                    admin.Username = model.Username;
                    admin.Password = Encrypt.ComputeHash(model.Password,"SHA512",null);
                    admin.LastName = model.LastName;
                    admin.FirstName = model.FirstName;
                    admin.ContactNo = model.ContactNo;
                    admin.Email = model.Email;
                    admin.DateCreated = model.DateCreated;
                    context.SaveChanges();
                    return RedirectToAction("ViewAdmins");
                }

                return View();
            }
        }

        //Delete

        public ActionResult DeleteAdmin(int id)
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }
            Admin admin = Application.Db.Admins.Where(a => a.AdminID == id).SingleOrDefault();
            return View(admin);

        }

        [HttpPost]
        public ActionResult DeleteAdmin(Admin model)
        {
            if (!Application.IsAuthenticated && Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }
            int adminid = model.AdminID;
            using (KnowledgeChannelEntities context = new KnowledgeChannelEntities())
            {
                Admin admin = context.Admins.Where(a => a.AdminID == adminid).Single();
                context.DeleteObject(admin);
                context.SaveChanges();
                return RedirectToAction("ViewAdmins");
            }
        }

        //TODO: 
        public ActionResult ManageFeedback() {
            if (!Application.IsAuthenticated && Application.AdminType != 2) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }
            FeedbackViewModel model = new FeedbackViewModel(Application.Db);
            return View(model);
        }

        public ActionResult ManageEvents() {
            if (!Application.IsAuthenticated && Application.AdminType != 2) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }
            IEnumerable<Event> model = Application.Db.Events;
            return View(model);
        }

        //SignIn
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string Username, string Password)
        {
            var admin = Application.Db.Admins.Where(x => x.Username == Username).SingleOrDefault();

            if (admin == null || !Encrypt.VerifyHash(Password,"SHA512", admin.Password)) {
                ModelState.AddModelError("UserNotFound", "Email or Password is incorrect");
                return View();
            }

            if (Session["AdminID"] != null) {
                Session.Remove("AdminID");
            }
            Session.Add("AdminID", admin.AdminID);
            Session.Add("AdminType", admin.AdminType);
            //TODO: Session Handling, Authorization Tokens
            //TODO: Redirect to Index page
            //TODO: Create Model for Index/Home Page
            return RedirectToAction("Index");
        }

        public ActionResult UploadFile() {
            if (!Application.IsAuthenticated && Application.AdminType != 2) {
                return RedirectToAction("SignIn");
            }

            return View();
        }

        [HttpPost]
        public ActionResult UploadFile(AdminUploadViewModel model) {
            if (!Application.IsAuthenticated && Application.AdminType != 2) {
                return RedirectToAction("SignIn");
            }

            var Model = new AdminUploadViewModel(model, Application.Db);
            Model.SaveFile(model, Server, Application.AdminID, Application.Db);

            ViewBag.Header = "File has been uploaded";
            ViewBag.Message = "Your file has been successfully uploaded!";
            return View("Message", "_LayoutGuest");
        }
    }
}
