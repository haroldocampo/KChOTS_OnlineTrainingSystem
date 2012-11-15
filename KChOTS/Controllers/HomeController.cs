using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KChOTS.Models.ContentPage;

namespace KChOTS.Controllers
{
    public class HomeController : KChOTSController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ContentViewModel model = new ContentViewModel(Application.Db);
            model.PopulateHomeContent();
            if (Application.IsAuthenticated) {

                if (Application.AdminID != 0) {
                    return RedirectToAction("Index", "Admin");
                }

                if (Application.TeacherID != 0) {
                    return RedirectToAction("Index", "Teacher");
                }
                return View("Index", "_LayoutGuest", model);
            }
            else {
                return View("Index", "_LayoutGuest", model);
            }
        }
    }
}
