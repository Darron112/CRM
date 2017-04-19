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
    public class TypeController : Controller
    {
        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "A")]
        public ActionResult GetTypes()
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var type = mainDB.TypeTables.OrderBy(a => a.TypeName).ToList();
                return Json(new { data = type }, JsonRequestBehavior.AllowGet);
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
                var v = mainDB.TypeTables.Where(a => a.TypeID == id).FirstOrDefault();
                return View(v);
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Save(TypeTable types)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MainDBEntities mainDB = new MainDBEntities())
                {
                    if (types.TypeID > 0)
                    {
                        //edycja
                        var s = mainDB.TypeTables.Where(a => a.TypeID == types.TypeID).FirstOrDefault();
                        if (s != null)
                        {
                            s.TypeName = types.TypeName;
                        }
                    }
                    else
                    {
                        //zapis
                        mainDB.TypeTables.Add(types);
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
                var del = mainDB.TypeTables.Where(a => a.TypeID == id).FirstOrDefault();
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
                var del = mainDB.TypeTables.Where(a => a.TypeID == id).FirstOrDefault();
                if (del != null)
                {
                    mainDB.TypeTables.Remove(del);
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