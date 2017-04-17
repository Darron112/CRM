using CRM1._2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Web.Security;

namespace CRM1._2.Controllers
{
    [Authorize(Roles = "A, U")]
    public class RequestController : Controller
    {
        public ActionResult Index(int page = 1, string sort = "RequestID", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetRequests(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        public List<RequestTable> GetRequests(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (MainDBEntities dc = new MainDBEntities())
            {
                
                var v = (from a in dc.RequestTables                         
                         where
                                    
                                  a.RequestID.ToString().Contains(search) ||
                                  a.RequestDate.ToString().Contains(search) ||
                                  a.Title.Contains(search) ||
                                  a.ClientID.ToString().Contains(search) ||
                                  a.TypeID.ToString().Contains(search) ||
                                  a.Description.Contains(search)
                         select a
                                );
                totalRecord = v.Count();
                v = v.OrderBy(sort + " " + sortdir);
                if (pageSize > 0)
                {
                    v = v.Skip(skip).Take(pageSize);
                    
                }
                return v.ToList();
            }
        }

        public JsonResult getStatus()
        {
            List<StatusTable> status = new List<StatusTable>();
            using (MainDBEntities dc = new MainDBEntities())
            {
                status = dc.StatusTables.OrderBy(a => a.StatusName).ToList();
            }
            return new JsonResult { Data = status, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getUser()
        {
            List<UserAccount> user = new List<UserAccount>();
            using (MainDBEntities dc = new MainDBEntities())
            {
                user = dc.UserAccounts.OrderBy(a => a.UserName).ToList();
            }
            return new JsonResult { Data = user, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getClient()
        {
            List<ClientTable> client = new List<ClientTable>();
            using (MainDBEntities dc = new MainDBEntities())
            {
                client = dc.ClientTables.OrderBy(a => a.CompanyName).ToList();
            }
            return new JsonResult { Data = client, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult getType()
        {
            List<TypeTable> type = new List<TypeTable>();
            using (MainDBEntities dc = new MainDBEntities())
            {
                type = dc.TypeTables.OrderBy(a => a.TypeName).ToList();
            }
            return new JsonResult { Data = type, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //[HttpGet]
        public ActionResult Save(int? id)
        {

            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var v = mainDB.RequestTables.Where(a => a.RequestID == id).FirstOrDefault();
                return View(v);
            }
        }

        [HttpPost]
        public JsonResult Save(RequestTable request /*RequestDetail rd*/)
        {
            bool status = false;

            DateTime dateOrg;
            var isValidDate = DateTime.TryParseExact(request.RequestDateString, "mm-dd-yyyy", null, System.Globalization.DateTimeStyles.None, out dateOrg);
            if (isValidDate)
            {
                request.RequestDate = dateOrg;
            }

            if (ModelState.IsValid)
            {
                using (MainDBEntities mainDB = new MainDBEntities())
                {
                    //var isValidModel = TryUpdateModel(request);
                    if (request.RequestID > 0 /*|| rd.RequestDetailsID > 0*/)
                    {
                        //edycja
                        var s = mainDB.RequestTables.Where(a => a.RequestID == request.RequestID).FirstOrDefault();                          
                        if (s != null)
                        {
                            s.RequestID = request.RequestID;
                            s.RequestDate = request.RequestDate;
                            s.Title = request.Title;
                            //s.Status = request.Status;
                            //s.Type = request.Type;
                            s.Description = request.Description;
                        }

                    }
                    else /*if(isValidModel)*/
                    {
                        //zapis
                        mainDB.RequestTables.Add(request);
                        //mainDB.RequestDetails.Add(rd);
                    }
                    mainDB.SaveChanges();
                    status = true;                  
                }
            }

            RedirectToAction("index", "Request");
            return new JsonResult { Data = new { status = status } };         
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var del = mainDB.RequestTables.Where(a => a.RequestID == id).FirstOrDefault();
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

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteRequest(int id)
        {
            bool status = false;
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var del = mainDB.RequestTables.Where(a => a.RequestID == id).FirstOrDefault();
                if (del != null)
                {
                    mainDB.RequestTables.Remove(del);
                    mainDB.SaveChanges();
                    status = true;
                }
            }

            new JsonResult { Data = new { status = status } };
            return RedirectToAction("index", "Request");
        }

        public ActionResult LogOff()
        {

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");

        }
    }
}


//AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
//if (Session["UserName"] == null)



//var clients = (from a in mainDB.ClientTables.Where(a => a.ClientID == 1) select a.CompanyName).ToList();

//public JsonResult getCompany(int clientID)
//{
//    List<ClientTable> clients = new List<ClientTable>();
//    using (MainDBEntities dc = new MainDBEntities())
//    {
//        clients = dc.ClientTables.Where(a => a.ClientID.Equals(clientID)).OrderBy(a => a.CompanyName).ToList();
//    }
//    return new JsonResult { Data = clients, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
//}



//var r = mainDB.RequestDetails.Where(a => a.RequestDetailsID == rd.RequestDetailsID).FirstOrDefault();
//if (r != null)
//{
//    r.RequestDetailsID = rd.RequestDetailsID;
//    r.StatusID = rd.StatusID;
//    r.UserID = rd.UserID;
//    r.StageNumber = rd.StageNumber;
//    r.StageTime = rd.StageTime;
//}


//return Json(new
//{
//    redirectUrl = Url.Action("Index", "Request"),
//    isRedirect = true
//});
//return RedirectToAction("index", "Request");   