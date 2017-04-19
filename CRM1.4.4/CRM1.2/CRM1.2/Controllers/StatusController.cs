using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM1._2.Models;
using System.Web.Security;

namespace CRM1._2.Controllers
{
    [Authorize]
    public class StatusController : Controller
    {
        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "A")]
        public ActionResult GetStatus()
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var status = mainDB.StatusTables.OrderBy(a => a.StatusName).ToList();
                return Json(new { data = status }, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult getClientss(int clientID)
        //{

        //}

        [Authorize(Roles = "A")]
        [HttpGet]
        public ActionResult Save(int id)
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var v = mainDB.StatusTables.Where(a => a.StatusID == id).FirstOrDefault();
                return View(v);
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Save(StatusTable statuss)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MainDBEntities mainDB = new MainDBEntities())
                {
                    if (statuss.StatusID > 0)
                    {
                        //edycja
                        var s = mainDB.StatusTables.Where(a => a.StatusID == statuss.StatusID).FirstOrDefault();
                        if (s != null)
                        {
                            s.StatusName = statuss.StatusName;                           
                        }
                    }
                    else
                    {
                        //zapis
                        mainDB.StatusTables.Add(statuss);
                    }
                    mainDB.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }

        [Authorize(Roles = "A")]
        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var del = mainDB.StatusTables.Where(a => a.StatusID == id).FirstOrDefault();
                if (del != null)
                {
                    return View(del);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteClient(int id)
        {
            bool status = false;
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var del = mainDB.StatusTables.Where(a => a.StatusID == id).FirstOrDefault();
                if (del != null)
                {
                    mainDB.StatusTables.Remove(del);
                    mainDB.SaveChanges();
                    status = true;
                }
            }

            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult LogOff()
        {
            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            //if (Session["UserName"] == null)
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }
    }
}