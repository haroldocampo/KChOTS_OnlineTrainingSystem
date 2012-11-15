using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using KChOTS.Models.Teacher;
using KChOTS.Models.Resources;
using KChOTS.Security;
namespace KChOTS.Controllers
{
    public class TeacherController : KChOTSController
    {
        Encrypt encrypt = new Encrypt();
        
        //
        // GET: /Teacher/

        public ActionResult Index()
        {
            if (!Application.IsAuthenticated)
            {
                return RedirectToAction("SignIn");
            }
            return View(new TeacherIndexViewModel(Application.TeacherID, Application.Db));
        }

        public ActionResult UploadFile() {
            if (!Application.IsAuthenticated && Application.TeacherID == 0)
            {
                return RedirectToAction("SignIn");
            }

            return View(new TeacherUploadViewModel(Application.Db));
        }

        [HttpPost]
        public ActionResult UploadFile(TeacherUploadViewModel model) {
            if (!Application.IsAuthenticated && Application.TeacherID == 0) {
                return RedirectToAction("SignIn");
            }

            if(model.Resource.Name == ""){
                ViewBag.Header = "Input Invalid";
                ViewBag.Message = "Please fill up Resource Name";
            }

            if (model.Resource.ResourceFile.ContentLength > 26214400) {
                ViewBag.Header = "Exceeded Max File Size";
                ViewBag.Message = "The maximum file size capacity for upload is 25MB";
                return View("Message", "_LayoutGuest");
            }

            var Model = new TeacherUploadViewModel(model, Application.Db);
            Model.SaveFile(model, Server, Application.TeacherID, Application.Db);
            return View(Model);
        }

        public ActionResult Register()
        {
            return View(new TeacherViewModel());
        }

        [HttpPost]
        public ActionResult Register(TeacherViewModel model) {
            _db = new KnowledgeChannelEntities();

            bool emailExists = _db.Teachers.Where(x => x.Email == model.Teacher.Email).ToList().Count > 0;

            if (emailExists) {
                ModelState.AddModelError("EmailExists", "Email is already taken");
            }

            if (!model.Teacher.Email.IsEmail()) {
                ModelState.AddModelError("EmailWrongFormat", "Email is not in correct format");
            }

            if (!model.Teacher.ContactNumber.IsNumeric())
            {
                ModelState.AddModelError("ContactNotNumeric", "Not a valid contact number");
            }

            if (model.Teacher.Gender == null || model.Teacher.Email == null || model.Teacher.LastName == null || model.Teacher.FirstName == null || model.Teacher.Password == null){
                ModelState.AddModelError("InputRequired", "Please fill up the required fields");
            }

            if (model.Teacher.Password != model.Teacher.ConfirmPassword)
                ModelState.AddModelError("PasswordsDoNotMatch", "Passwords do not match");

            if (model.Teacher.Password.Length < 6)
                ModelState.AddModelError("PasswordsDoNotMatch", "Password must at least have 6 characters");

            if (model.Teacher.SchoolID == 0)
                ModelState.AddModelError("NoSchoolSelected", "School not selected correctly. Please retry inputting your school");

            if (!ModelState.IsValid){
                return View(new TeacherViewModel());
            }

            RegisterTeacher(model);
            return RedirectToAction("SignIn");
        }

        private void RegisterTeacher(TeacherViewModel model) {
            var teacher = Application.Db.Teachers.CreateObject();
            teacher.BirthDate = model.Teacher.BirthDate;
            teacher.Gender = model.Teacher.Gender;
            teacher.ContactNo = model.Teacher.ContactNumber;
            teacher.Email = model.Teacher.Email;
            teacher.FirstName = model.Teacher.FirstName;
            teacher.LastName = model.Teacher.LastName;
            teacher.Password = Encrypt.ComputeHash(model.Teacher.Password, "SHA512", null);
            teacher.SchoolID = model.Teacher.SchoolID;
            teacher.created_at = DateTime.Now;
            Application.Db.AddToTeachers(teacher);
            Application.Db.SaveChanges();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(string email, string password) {
            //TODO: Authenticate user
            _db = Application.Db; 
            var User = _db.Teachers.Where(x => x.Email == email).FirstOrDefault();

            if (User == null || !Encrypt.VerifyHash(password, "SHA512",User.Password))
            {
                ModelState.AddModelError("UserNotFound", "Email or Password is incorrect");
                return View();
            }

            if (Session["TeacherID"] != null)
            {
                Session.Remove("TeacherID");
            }
            Session.Add("TeacherID", User.TeacherID);
            //TODO: Session Handling, Authorization Tokens
            //TODO: Redirect to Index page
            //TODO: Create Model for Index/Home Page
            return RedirectToAction("Index");
        }

        public ActionResult Resources() {
            if (!Application.IsAuthenticated && Application.TeacherID != 0)
            {
                return RedirectToAction("SignIn");
            }
            var model = new TeacherResourcesViewModel(Application.TeacherID, Application.Db);
            return View(model);
        }

        private KnowledgeChannelEntities _db;
    }
}
