using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;
using System.Net;
using KChOTS.Security;

namespace KChOTS.Controllers
{
    public class AccountController : KChOTSController
    {
        //
        // GET: /Account/
        public ActionResult Index()
        {
            if (Application.IsAuthenticated) {

                if (Application.AdminID != 0) {
                    var model = Application.Db.Admins.Where(x => x.AdminID == Application.AdminID).SingleOrDefault();
                    return View("Admin", model);
                }

                if (Application.TeacherID != 0) {
                    var model = Application.Db.Teachers.Where(x => x.TeacherID == Application.TeacherID).SingleOrDefault();
                    return View("Teacher", model);
                }

            }
            // If Not Autheniticated
            ViewBag.Header = "Not yet logged in?";
            ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as an administrator to log-in.";
            return View("Message", "_LayoutGuest");
        }

        public ActionResult RequestResetPassword() {
            return View();
        }

        [HttpPost]
        public ActionResult RequestResetPassword(string email) {
            if (email == "") { 
                return View("Error");
            }

            bool emailExisting = false;

            KChOTS.Admin admin = Application.Db.Admins.Where(x => x.Email == email).SingleOrDefault();
            if (admin != null) {
                var passRequest = new PasswordReset();
                passRequest.Id = Guid.NewGuid();
                passRequest.NewPassword = GenerateNewPassword();
                passRequest.AdminId = admin.AdminID;
                passRequest.ExpiryDate = DateTime.Now.AddHours(3);
                Application.Db.AddToPasswordResets(passRequest);
                Application.Db.SaveChanges();
                SendPasswordToEmail(email, passRequest.NewPassword, passRequest.Id.ToString());
                emailExisting = true;
            }

            KChOTS.Teacher teacher = Application.Db.Teachers.Where(x => x.Email == email).SingleOrDefault();
            if (admin == null && teacher != null) {
                var passRequest = new PasswordReset();
                passRequest.Id = Guid.NewGuid();
                passRequest.NewPassword = GenerateNewPassword();
                passRequest.TeacherId = teacher.TeacherID;
                passRequest.ExpiryDate = DateTime.Now.AddHours(3);
                Application.Db.AddToPasswordResets(passRequest);
                Application.Db.SaveChanges();
                SendPasswordToEmail(email, passRequest.NewPassword, passRequest.Id.ToString());
                emailExisting = true;
            }

            if (!emailExisting) {
                ViewBag.Header = "E-mail does not exist";
                ViewBag.Message = email + " does not exist in the system.";
                return View("Message", "_LayoutGuest");
            }

            ViewBag.Header = "We've sent someting to your e-mail";
            ViewBag.Message = "Please check your e-mail("+email+") to verify the password reset request.";
            return View("Message", "_LayoutGuest");
        }

        [HttpPost]
        public ActionResult Admin(Admin model) {
            if (!model.Email.IsEmail()) {
                ViewBag.Header = "Inputs were incorrect";
                ViewBag.Message = "Please go back to the admin form and input the correct data";
                return View("Message", "_LayoutAdmin");
            }

            if (!model.ContactNo.IsNumeric()) {
                ViewBag.Header = "Inputs were incorrect";
                ViewBag.Message = "Please go back to the admin form and input the correct data";
                return View("Message", "_LayoutAdmin");
            }

            if (model.Password.Length < 6) {
                ViewBag.Header = "Password incorrect";
                ViewBag.Message = "For security reasons, please go back to the admin form and input the correct data";
                return View("Message", "_LayoutAdmin");
            }

            var admin = Application.Db.Admins.Where(x => x.AdminID == Application.AdminID).SingleOrDefault();
            admin.Username = model.Username;
            admin.Password = Encrypt.ComputeHash(model.Password, "SHA512", null);
            admin.FirstName = model.FirstName;
            admin.LastName = model.LastName;
            admin.ContactNo = model.ContactNo;
            admin.Email = model.Email;
            Application.Db.SaveChanges();
            ViewBag.Header = "Account Modified";
            ViewBag.Message = "Your account has been updated";
            return View("Message", "_LayoutAdmin");
        }

        [HttpPost]
        public ActionResult Teacher(Teacher model) {
            if (!model.Email.IsEmail()) {
                ViewBag.Header = "Inputs were incorrect";
                ViewBag.Message = "Please go back to the form and input the correct data";
                return View("Message", "_Layout");
            }

            if (!model.ContactNo.IsNumeric()) {
                ViewBag.Header = "Inputs were incorrect";
                ViewBag.Message = "Please go back to the admin form and input the correct data";
                return View("Message", "_Layout");
            }

            if (model.Password.Length < 6) {
                ViewBag.Header = "Password incorrect";
                ViewBag.Message = "For security reasons, please go back to the admin form and input the correct data";
                return View("Message", "_Layout");
            }

            var teacher = Application.Db.Teachers.Where(x => x.TeacherID == Application.TeacherID).SingleOrDefault();
            teacher.BirthDate = model.BirthDate;
            teacher.Gender = model.Gender;
            teacher.ContactNo = model.ContactNo;
            teacher.Email = model.Email;
            teacher.FirstName = model.FirstName;
            teacher.LastName = model.LastName;
            teacher.Password = Encrypt.ComputeHash(model.Password, "SHA512", null);
            teacher.SchoolID = model.SchoolID;
            Application.Db.SaveChanges();
            ViewBag.Header = "Account Modified";
            ViewBag.Message = "Your account has been updated";
            return View("Message", "_Layout");
        }

        private string GenerateNewPassword() {
            Random random = new Random();
            int phrase1 = random.Next(100, 999);
            string phrase2 = (((char)random.Next(97, 122)).ToString() + ((char)random.Next(97, 122)) + ((char)random.Next(97, 122)) + ((char)random.Next(97, 122))).ToString();
            int phrase3 = random.Next(100, 999);
            string newPassword = phrase1 + phrase2 + phrase3;
            return newPassword;
        }

        public ActionResult FinalResetPassword(string id) {
            Guid resetId = Guid.Parse(id);
            var model = Application.Db.PasswordResets.Where(x => x.Id == resetId).SingleOrDefault();
            if (model == null) {
                ViewBag.Header = "404";
                ViewBag.Message = "Page not found";
                return View("Message", "_LayoutGuest");
            }
            if (model.AdminId == null) { //Teacher
                var teacher = Application.Db.Teachers.Where(x => x.TeacherID == model.TeacherId).SingleOrDefault();
                teacher.Password = Encrypt.ComputeHash(model.NewPassword,"SHA512",null);
            }
            else //Admin
            {
                var admin = Application.Db.Admins.Where(x => x.AdminID == model.AdminId).SingleOrDefault();
                admin.Password = Encrypt.ComputeHash(model.NewPassword,"SHA512",null);
            }

            Application.Db.SaveChanges();
            ViewBag.Header = "Reset Success";
            ViewBag.Message = "Your password has been reset!";
            return View("Message", "_LayoutGuest");
        }

        private void SendPasswordToEmail(string email, string newPassword, string Guid) {
            string from = "kchonlinets@gmail.com";

            //Create Mail Body
            StringBuilder body = new StringBuilder();
            body.AppendLine("Thank you for your patience!"); body.AppendLine();
            body.AppendLine("Your new password: <b>" + newPassword + "</b>");
            body.AppendLine("Please click the link to complete the password reset:");
            body.AppendLine("<a href='knowledgechannel.hocampo.com/" + Url.Action("FinalResetPassword", "Account", new { id = Guid }) + "'>knowledgechannel.hocampo.com/" + Url.Action("FinalResetPassword", "Account", new { id = Guid }) + "</a>");

            MailMessage mail = new MailMessage();
            mail.To.Add(email);
            mail.From = new MailAddress(from);
            mail.Subject = "Password Reset from Online Training System";
            mail.Body = body.ToString();
            mail.IsBodyHtml = true;

            SmtpClient mailClient = new SmtpClient();
            mailClient.Host = "smtp.gmail.com";
            mailClient.Credentials = new System.Net.NetworkCredential("kchonlinets@gmail.com", "inn0v@ti0n");
            mailClient.EnableSsl = true;
            mailClient.Send(mail);
        }
    }
}
