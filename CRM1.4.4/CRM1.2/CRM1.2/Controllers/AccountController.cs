using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CRM1._2.Models;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using System.Linq.Dynamic;
using CRM1._2.App_Start;
using System.Threading.Tasks;

namespace CRM1._2.Controllers
{
    
    public class AccountController : Controller
    {

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index(int page = 1, string sort = "FirstName", string sortdir = "asc", string search = "")
        {
            int pageSize = 10;
            int totalRecord = 0;
            if (page < 1) page = 1;
            int skip = (page * pageSize) - pageSize;
            var data = GetUsers(search, sort, sortdir, skip, pageSize, out totalRecord);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            return View(data);
        }

        public List<UserAccount> GetUsers(string search, string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            using (MainDBEntities dc = new MainDBEntities())
            {
                var v = (from a in dc.UserAccounts
                         where
                                 a.FirstName.Contains(search) ||
                                 a.LastName.Contains(search) ||
                                 a.Email.Contains(search) ||
                                 a.UserName.Contains(search) ||
                                 a.Roles.Contains(search)
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

        //rejestracja get
        public ActionResult Register(int ?id)
        {
            using (MainDBEntities db = new MainDBEntities())
            {
                var v = db.UserAccounts.Where(a => a.UserID == id).FirstOrDefault();
                return View(v);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserAccount account)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                using (MainDBEntities db = new MainDBEntities())
                {
                    if (account.UserID > 0)
                    {
                        //edycja
                        var s = db.UserAccounts.Where(a => a.UserID == account.UserID).FirstOrDefault();
                        if (s != null)
                        {
                            s.UserID = account.UserID;
                            s.FirstName = account.FirstName;
                            s.LastName = account.LastName;
                            s.Email = account.Email;
                            s.Roles = account.Roles;
                        }
                    }
                    else
                    {
                        db.UserAccounts.Add(account);                      
                    }

                    db.SaveChanges();
                    status = true;

                    //ModelState.Clear();
                    //ViewBag.Message = account.FirstName + " " + account.LastName + " rejestracja zakonczona sukcesem.";
                }
            }
            new JsonResult { Data = new { status = status } };
            return RedirectToAction("index", "Account");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            using (MainDBEntities mainDB = new MainDBEntities())
            {
                var del = mainDB.UserAccounts.Where(a => a.UserID == id).FirstOrDefault();
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
                var del = mainDB.UserAccounts.Where(a => a.UserID == id).FirstOrDefault();
                if (del != null)
                {
                    mainDB.UserAccounts.Remove(del);
                    mainDB.SaveChanges();
                    status = true;
                }
            }
            new JsonResult { Data = new { status = status } };
            return RedirectToAction("index", "Account");
        }

        //logowanie get
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (MainDBEntities db = new MainDBEntities())
            {
                var usr = db.UserAccounts.Where(u => u.UserName == user.UserName && u.Password == user.Password).Count();
                if (usr == 0)
                {
                    ViewBag.Msg = "Nieprawidlowy uzytkownik lub haslo";
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, false);
                    //Session["UserID"] = usr.UserID.ToString();
                    //Session["UserName"] = usr.UserName.ToString();
                    return RedirectToAction("LoggedIn");
                }

            }
        }

        public ActionResult LoggedIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }


        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        #endregion
    }
}


//[HttpPost]
//[ValidateAntiForgeryToken]
//public async Task<ActionResult> Register(UserAccount model)
//{
//    if (ModelState.IsValid)
//    {
//        var user = new ApplicationUser { UserName = model.Email, Email = model.Email, UserAccount = new UserAccount() };
//        var result = await UserManager.CreateAsync(user, model.Password);
//        if (result.Succeeded)
//        {
//            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

//            return RedirectToAction("Index", "Home");
//        }
//        AddErrors(result);
//    }
//    return View(model);
//}

//private void AddErrors(IdentityResult result)
//{
//    foreach (var error in result.Errors)
//    {
//        ModelState.AddModelError("", error);
//    }
//}



//public ActionResult LogOff()
//{
//    //AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
//    //if (Session["UserName"] == null)
//    Session.Clear();
//    return RedirectToAction("Login", "Account");

//}