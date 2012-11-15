using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KChOTS.Models.ContentPage;

namespace KChOTS.Controllers
{
    public class PageController : KChOTSController
    {
        //
        // GET: /Page/

        public ActionResult Index()
        {
            if (!Application.IsAuthenticated) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }

            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Index(string content, string subject)
        {
            if (!Application.IsAuthenticated) {
                ViewBag.Header = "Please Log In";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }

            try
            {
                ContentPage page = new ContentPage();
                page.Name = subject;
                page.PageContent = content;
                page.DateModified = DateTime.Now;
                page.AdminID = Application.AdminID;
                page.CmsTypeID = 2;
                Application.Db.AddToContentPages(page);
                Application.Db.SaveChanges();
                return RedirectToAction("index");
            }
            catch
            {
                return View();
            }

        }

        public ActionResult Article(int id) {
            ContentPage article = Application.Db.ContentPages.Where(x => x.ID == id).SingleOrDefault();

            if (article == null) {
                ViewBag.Header = "Page Not Found";
                ViewBag.Message = "The page that you are trying to access is not available.";
                return View("Message");
            }
            return View(article);
        }

        public ActionResult ViewArticle()
        {
            using (KnowledgeChannelEntities context = new KnowledgeChannelEntities())
            {
                IEnumerable<ContentPage> List = context.ContentPages.ToList();
                return View(List);
            }
        }

        public ActionResult UpdatePage()
        {
            if (!Application.IsAuthenticated || Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }

            var model = new ContentViewModel(Application.Db);
            model.PopulateUpdateView();
            return View("UpdatePage","_LayoutAdmin", model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdatePage(string content, string subject, int cmsType)
        {
            if (!Application.IsAuthenticated || Application.AdminType != 1) {
                ViewBag.Header = "Authorization Level Too Low";
                ViewBag.Message = "Your authorization is not valid for this type of operation";
                return View("Message", "_LayoutGuest");
            }

            ContentPage contents = Application.Db.ContentPages.Where(x => x.CmsTypeID == cmsType).FirstOrDefault();
            contents.Name = subject;
            contents.PageContent = content;
            Application.Db.SaveChanges();

            ViewBag.Header = "Content Modified";
            ViewBag.Message = "The content has been modified";
            return View("Message", "_LayoutGuest");
        }

    }
}
