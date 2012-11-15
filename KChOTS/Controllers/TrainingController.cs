using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KChOTS.Models.Training;

namespace KChOTS.Controllers
{
    public class TrainingController : KChOTSController
    {
        public ActionResult Index()
        {
            if (!Application.IsAuthenticated && Application.TeacherID == 0) {
                return RedirectToAction("SignIn", "Teacher");
            }
            var teacher = Application.Db.Teachers.Where(x => x.TeacherID == Application.TeacherID).SingleOrDefault();
            string username = teacher == null ? null : teacher.FirstName;
            TrainingViewModel model = new TrainingViewModel(username);
            return View(model);
        }

    }
}
