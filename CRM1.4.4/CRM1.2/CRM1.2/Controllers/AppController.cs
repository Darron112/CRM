//using CRM1._2.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace CRM1._2.Controllers
//{
//    public class AppController : Controller
//    {

//        public ActionResult Index()
//        {
//            return View();
//        }

//        public ActionResult GetEmployees()
//        {
//            using (MainDBEntities mainDB = new MainDBEntities())
//            {
//                var employees = mainDB.Employees.OrderBy(a => a.FirstName).ToList();
//                return Json(new { data = employees }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        [HttpGet]
//        public ActionResult Save(int id)
//        {
//            using (MainDBEntities mainDB = new MainDBEntities())
//            {
//                var v = mainDB.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
//                return View(v);
//            }
//        }

//        [HttpPost]
//        public ActionResult Save(Employee employee)
//        {
//            bool status = false;
//            if (ModelState.IsValid)
//            {
//                using (MainDBEntities mainDB = new MainDBEntities())
//                {
//                    if (employee.EmployeeID > 0)
//                    {
//                        //edycja
//                        var s = mainDB.Employees.Where(a => a.EmployeeID == employee.EmployeeID).FirstOrDefault();
//                        if (s != null)
//                        {
//                            s.FirstName = employee.FirstName;
//                            s.LastName = employee.LastName;
//                            s.Email = employee.Email;
//                            s.Address = employee.Address;
//                            s.Phone = employee.Phone;
//                        }
//                    }
//                    else
//                    {
//                        //zapis
//                        mainDB.Employees.Add(employee);
//                    }
//                    mainDB.SaveChanges();
//                    status = true;
//                }
//            }
//            return new JsonResult { Data = new { status = status } };
//        }

//        [HttpGet]
//        public ActionResult Delete(int id)
//        {
//            using (MainDBEntities mainDB = new MainDBEntities())
//            {
//                var del = mainDB.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
//                if (del != null)
//                {
//                    return View(del);
//                }
//                else
//                {
//                    return HttpNotFound();
//                }
//            }
//        }

//        [HttpPost]
//        [ActionName("Delete")]
//        public ActionResult DeleteEmployee(int id)
//        {
//            bool status = false;
//            using (MainDBEntities mainDB = new MainDBEntities())
//            {
//                var del = mainDB.Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
//                if (del != null)
//                {
//                    mainDB.Employees.Remove(del);
//                    mainDB.SaveChanges();
//                    status = true;
//                }
//            }

//            return new JsonResult { Data = new { status = status } };
//        }

//        public ActionResult LogOff()
//        {
//            //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
//            //if (Session["UserName"] == null)
//            Session.Clear();
//            return RedirectToAction("Login", "Account");

//        }
//    }

//}