using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace cis237inclass6.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
            //return ContentResult();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            // This is a dynamic object that can just have a property
            // attached to it with a value. Here we made the 'key'
            // WhateverYouWantToPut, and the 'value' Some Value.
            //This viewbag is available in the view. This is how we
            // pass data from the controller to the view.
            ViewBag.WhateverYouWantToPut = "Some Value";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Added this Method for a static page.
        // When the uer goes to /home/foo this is the method that will be
        // called. See the routecinfig file for more info...

        //Just added authorize so that you can't go to foo if you aren't logged in
        [Authorize]
        public ActionResult Foo()
        {
            return View();
        }
    }
}