using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KChOTS.Models.Resources;

namespace KChOTS.Controllers
{
    public class ResourceController : KChOTSController
    {
        //
        // GET: /Resource/

        public ActionResult Index()
        {
            if (!Application.IsAuthenticated) {
                ViewBag.Header = "Not yet logged in?";
                ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as an administrator to log-in.";
                return View("Message", "_LayoutGuest");
            }
            return View();
        }
        public ActionResult Subjects(int id)
        {
            if (!Application.IsAuthenticated) {
                ViewBag.Header = "Not yet logged in?";
                ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as an administrator to log-in.";
                return View("Message", "_LayoutGuest");
            }
            Session.Add("levelId", id);
            return View();
        }
        public ActionResult Page(int id = 0)
        {
            if (!Application.IsAuthenticated) {
                ViewBag.Header = "Not yet logged in?";
                ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as an administrator to log-in.";
                return View("Message", "_LayoutGuest");
            }
            if (id == 0 || Session["levelId"] == null)
            {
                return Redirect(Url.Action("Index", "Error"));
            }
            int levelId = (int)Session["levelId"];
            ResourcePageViewModel model = new ResourcePageViewModel(levelId, id, Application.Db);
            model.PopulateData();
            return View(model);
        }

        public ActionResult Search() {
            if (!Application.IsAuthenticated) {
                ViewBag.Header = "Not yet logged in?";
                ViewBag.Message = "Please go to the Sign-In page and enter your credentials or Register as an administrator to log-in.";
                return View("Message", "_LayoutGuest");
            }

            return View();
        }

        [HttpPost]
        public JsonResult Search(string resourceName) {
            ResourcePageViewModel model = new ResourcePageViewModel(Application.Db);
            model.Resources = Application.Db.Resources.Where(x=>x.Name.Contains(resourceName)).ToList();
            List<SearchResource> searchResources = new List<SearchResource>();
            foreach (var item in model.Resources) {
                searchResources.Add(new SearchResource()
                {
                    ResourceFile = item.ResourceFile,
                    Name = item.Name,
                    Description = item.Description,
                    DateCreated = item.DateCreated.Value.ToShortDateString()
                });
            }
            return Json(new { resources = searchResources }, JsonRequestBehavior.AllowGet);
        }

        public class SearchResource {
            public string ResourceFile { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string DateCreated { get; set; }
        }

        public ActionResult KChResource() {
            ResourcePageViewModel model = new ResourcePageViewModel(Application.Db);
            return View("Page",model);
        }
    }
}
