using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace demo_tc_786.Controllers
{
    public class twMVCController : Controller
    {
        //
        // GET: /twMVC/

        public ActionResult Index()
        {
            return Content("is twMVC index");
        }

    }
}
