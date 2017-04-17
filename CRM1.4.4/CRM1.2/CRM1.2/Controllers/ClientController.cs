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
    public class ClientController : Controller
    {
        [Authorize(Roles = "A, U")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "A, U")]
        public ActionResult GetClients()
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var clients = mainDB.ClientTables.OrderBy(a => a.CompanyName).ToList();
                return Json(new { data = clients }, JsonRequestBehavior.AllowGet);
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
                var v = mainDB.ClientTables.Where(a => a.ClientID == id).FirstOrDefault();
                return View(v);
            }
        }

        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Save(ClientTable client)
        {
            bool status = false;
            if (ModelState.IsValid)
            {
                using (MainDBEntities mainDB = new MainDBEntities())
                {
                    if (client.ClientID > 0)
                    {
                        //edycja
                        var s = mainDB.ClientTables.Where(a => a.ClientID == client.ClientID).FirstOrDefault();
                        if (s != null)
                        {
                            s.Nip = client.Nip;
                            s.CompanyName = client.CompanyName;
                            s.Address = client.Address;
                            s.Phone = client.Phone;
                            s.Email = client.Email;
                        }
                    }
                    else
                    {
                        //zapis
                        mainDB.ClientTables.Add(client);
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
                var del = mainDB.ClientTables.Where(a => a.ClientID == id).FirstOrDefault();
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
                var del = mainDB.ClientTables.Where(a => a.ClientID == id).FirstOrDefault();
                if (del != null)
                {
                    mainDB.ClientTables.Remove(del);
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