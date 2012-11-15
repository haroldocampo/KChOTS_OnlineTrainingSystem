using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;

namespace KChOTS
{
    public class KChOTSController : Controller
    {
        public ApplicationContext Application { get; set; }

        public KChOTSController() { Application = new ApplicationContext(); }

        protected override void OnException(ExceptionContext e) {
            //Create Mail Body
            StringBuilder body = new StringBuilder();
            body.AppendLine("An Exception has occured"); body.AppendLine();
            body.AppendLine("<br/><br/>Controller: " + e.Controller);
            body.AppendLine("<br/>URL: " + e.RouteData);
            body.AppendLine("<br/>Exception Details");
            body.AppendLine("<br/>Message: " + e.Exception.Message);
            body.AppendLine("<br/>InnerException: " + e.Exception.InnerException);
            body.AppendLine("<br/>Stack Trace: " + e.Exception.StackTrace);

            MailMessage mail = new MailMessage();
            mail.To.Add("haroldocampo@live.com");
            mail.From = new MailAddress("kchonlinets@gmail.com");
            mail.Subject = "Exception from Online Training System";
            mail.Body = body.ToString();
            mail.IsBodyHtml = true;

            SmtpClient mailClient = new SmtpClient();
            mailClient.Host = "smtp.gmail.com";
            mailClient.Credentials = new System.Net.NetworkCredential("kchonlinets@gmail.com", "inn0v@ti0n");
            mailClient.EnableSsl = true;
            try
            {
                mailClient.Send(mail);
            }
            catch { }//To avoid exception
            e.ExceptionHandled = true;
            UrlHelper urlHelper = new UrlHelper(this.Request.RequestContext);
            Response.Redirect(urlHelper.Action("Index", "Error"));
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            base.OnActionExecuting(filterContext);
            InitializeAppContext();
        }

        protected override void Dispose(bool disposing) {
            Application.Db.Dispose();
        }

        private void InitializeAppContext() {
            Application.TeacherID = Session["TeacherID"] == null ? 0 : (int)Session["TeacherID"];
            Application.AdminID = Session["AdminID"] == null ? 0 : (int)Session["AdminID"];
            Application.AdminType = Session["AdminType"] == null ? 0 : (int)Session["AdminType"];
            if (Application.TeacherID == 0 && Application.AdminID == 0)
            {
                Application.IsAuthenticated = false;
            }
            else
            {
                Application.IsAuthenticated = true;
            }
        }
    }
}
