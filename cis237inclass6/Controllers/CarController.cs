using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using cis237inclass6.Models;

namespace cis237inclass6.Controllers
{
    [Authorize]
    public class CarController : Controller
    {
        private CarsTKeranenEntities db = new CarsTKeranenEntities();

        // GET: /Car/
        public ActionResult Index()
        {
            DbSet<Car> CarsToFilter = db.Cars;

            // Setup some strings to hold the data that might be in the session.
            // If there is nothing in the session we can still use these variables as the default value.
            string filterMake = "";
            string filterMin = "";
            string filterMax = "";

           // Define a min and mx for the cylinders
            int min = 0;
            int max = 16;

            // Check to see if there is a value in the session and if there is, then assign it
            // to the variable that we setup to hold the value
            if (Session["make"] != null && !String.IsNullOrWhiteSpace((string)Session["make"]))
            {
                filterMake = (string)Session["make"];
            }

            if (Session["min"] != null && !String.IsNullOrWhiteSpace((string)Session["min"]))
            {
                filterMin = (string)Session["min"];
                min = Int32.Parse(filterMin);
            }

            if (Session["max"] != null && !String.IsNullOrWhiteSpace((string)Session["max"]))
            {
                filterMax = (string)Session["max"];
                max = Int32.Parse(filterMax);
            }

            // Do the filter on the CarsToSearch Dataset. Use the where that we used before
            // when doing the EF work, only this time send in more lamda expressions to narrow it
            // down further. Since we setup default values for each of the filter parameters,
            // min, max, and filterMake, we can count on this always running with no errors.
            IEnumerable<Car> filtered = CarsToFilter.Where(car => car.cylinders >= min &&
                                                                  car.cylinders <= max &&
                                                                  car.make.Contains(filterMake));

            // Convert the database set to a list new that the query work is done on it.
            // The view is expecting a list, so we convert the database set to a list.
            IEnumerable<Car> finalFiltered = filtered.ToList();

            // Place the string representation of the values that are in the session into 
            // the viewbag so that they can be retrieved and displayed on the view.
            ViewBag.filterMake = filterMake;
            ViewBag.filtermin = filterMin;
            ViewBag.filterMax = filterMax;

            //return the view with a filtered selection of cars
            return View(finalFiltered);

            //return View(db.Cars.ToList());
        }

        // GET: /Car/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: /Car/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Car/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(car);
        }

        // GET: /Car/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: /Car/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="id,year,make,model,type,horsepower,cylinders")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(car);
        }

        // GET: /Car/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: /Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Car car = db.Cars.Find(id);
            db.Cars.Remove(car);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        // We need to add the HttpPost so it limits the type of requests it will handle to only POST. We can also
        // specify and Actio Name if we don;t wanti it to use the method name as the action name.
        [HttpPost, ActionName("Filter")]
        public ActionResult Filter()
        {
            // Get the form data that we sent out of the request object.
            // The string that is used as a key to get the data matches the
            // name of the property
            string make = Request.Form.Get("make");
            string min = Request.Form.Get("min");
            string max = Request.Form.Get("max");

            // Store hte form data into the session so that it can be retrieved later on to filter data.

            Session["make"] = make;
            Session["min"] = min;
            Session["max"] = max;

            // Redirect the user to the index page. We will do the work of actually
            // filtering the list in the index method.
            return RedirectToAction("Index");
        }
    }
}
