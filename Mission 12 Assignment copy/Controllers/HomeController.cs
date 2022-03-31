using m12.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace m12.Controllers
{
    public class HomeController : Controller
    {
        private AppointmentContext repo { get; set; }

       
        public HomeController(AppointmentContext temp)
        {
            repo = temp;
        }

        public IActionResult Index()
        {

            return View();
        }

   
        public IActionResult SignUp(string currentDate)
        {

        
            if (currentDate == null)
            {
        
                currentDate = DateTime.Now.ToString("MM/dd/yyyy");
            }

         
            ViewBag.CurrentDate = currentDate;

       
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(currentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);

            ViewBag.Disable = false;

            if (currentTime.AddDays(-1) < DateTime.Today)
            {
                ViewBag.Disable = true;
            }
        
            var responses = repo.Response.Where(i => i.Date == currentDate).ToList();

            return View(responses);
        }

   
        public IActionResult Appointments()
        {

        
            var responses = repo.Response.OrderBy(i => i.Date).ToList();

            return View(responses);
        }

    
        public IActionResult PreviousDate(string CurrentDate)
        {


            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(CurrentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime previousDate = currentTime.AddDays(-1);
            string previous = previousDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = previous;

      
            return RedirectToAction("SignUp", new { CurrentDate = previous });
        }

        public IActionResult NextDate(string CurrentDate)
        {

         
            string[] dateformat = { "MM/dd/yyyy" };
            DateTime currentTime = DateTime.ParseExact(CurrentDate, dateformat, System.Globalization.CultureInfo.InvariantCulture);
            DateTime nextDate = currentTime.AddDays(1);
            string next = nextDate.ToString("MM/dd/yyyy");

            ViewBag.CurrentDate = next;
            return RedirectToAction("SignUp", new { CurrentDate = next });
        }


        [HttpGet]
        public IActionResult Form(string currentDate, string time)
        {
            ViewBag.CurrentDate = currentDate;
            ViewBag.scheduledTime = time;


            if (HttpContext.Session.GetString("date") == null)
            {
                HttpContext.Session.SetString("date", currentDate);
            }
            else
            {
                ViewBag.CurrentDate = HttpContext.Session.GetString("date");
            }
            return View();
        }


        [HttpPost]
        public IActionResult Form(AppointmentResponse r)
        {
            if (r.Date == null)
            {
                r.Date = HttpContext.Session.GetString("date");
              
                if (r.Name != "" && r.Email != "" && (r.Size > 0 && r.Size <= 15))
                {
                    ModelState.Clear();
                }
            }
        
            if (ModelState.IsValid)
            {
                repo.Add(r);
                repo.SaveChanges();

                ViewBag.CurrentDate = "";
                ViewBag.scheduledTime = "";


                return RedirectToAction("Index");

            }
  
            ViewBag.CurrentDate = HttpContext.Session.GetString("date");
            ViewBag.scheduledTime = r.Time;
            r.Date = HttpContext.Session.GetString("date");
            return View(r);
        }


        public IActionResult Delete(int aptid)
        {
       
            var apt = repo.Response.Single(i => i.AppointmentId == aptid);

            repo.Response.Remove(apt);
            repo.SaveChanges();

            var list = repo.Response.ToList();

            return RedirectToAction("Appointments", list);
        }

        //Editing the Temple Appointments







        [HttpGet]
        public IActionResult Edit(int aptid)
        {
            ViewBag.New = false;

    
            var apt = repo.Response.Single(i => i.AppointmentId == aptid);


            ViewBag.CurrentDate = apt.Date;
            ViewBag.scheduledTime = apt.Time;

            HttpContext.Session.SetString("date", apt.Date);
            HttpContext.Session.SetString("time", apt.Time);
            return View("Form", apt);
        }

        [HttpPost]
        public IActionResult Edit(AppointmentResponse r)
        {
            if (r.Date == null || r.Time == null)
            {
                r.Date = HttpContext.Session.GetString("date");
                r.Time = HttpContext.Session.GetString("time");
            }

            //Validation conditions
            if (r.Name != "" && r.Email != "" && (r.Size > 0 && r.Size <= 10))
            {
                ModelState.Clear();
            }

           
            if (ModelState.IsValid)
            {
                repo.Update(r);
                repo.SaveChanges();

                ViewBag.CurrentDate = "";
                ViewBag.scheduledTime = "";
                HttpContext.Session.Remove("date");
                HttpContext.Session.Remove("time");

                return RedirectToAction("Appointments");
            }

            ViewBag.New = false;

            ViewBag.CurrentDate = HttpContext.Session.GetString("date");

            ViewBag.scheduledTime = r.Time;

            r.Date = HttpContext.Session.GetString("date");

            return View("Form", r);
        }
    }
}
