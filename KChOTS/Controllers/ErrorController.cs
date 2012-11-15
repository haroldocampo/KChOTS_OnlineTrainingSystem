using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KChOTS.Controllers
{
    public class ErrorController : KChOTSController
    {
        public ActionResult Index()
        {
            return View("Error");
        }
    }
}
