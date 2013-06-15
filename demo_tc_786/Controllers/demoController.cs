using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demo_tc_786.Controllers
{
    public class demoController : Controller
    {
        //
        // GET: /demo/

        public ActionResult Index()
        {
            return Content("Is demo index");
        }

        public ActionResult Dt(DateTime dt)
        {
            return Content("Is demo DateTime");
        }
    }
}
