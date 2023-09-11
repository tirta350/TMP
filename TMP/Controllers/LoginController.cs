using TMP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMP.Controllers
{
    public class LoginController : Controller
    {
        User _user = new User();

        // GET: Login
        public ActionResult User()
        {
            //Notification notification = new Notification();
            //notification.triggernotification();
            return View();
        }

        [HttpPost]
        public ActionResult User(UserModel _userMode)
        {
            string username = _userMode.username;
            string password = _userMode.password;

            if (_user.isUserValid(username, password))
            {
                Session["user"] = _user.getUser(username, password);
                UserModel userModel = _user.getUser(username, password);
                TempData["SuccessMessage"] = "Welcome " + userModel.nama_user;
                TempData["Direct"] = null;
                switch (userModel.role)
                {
                    case "Admin":
                        return Redirect(Url.Action("Dashboard_Admin", "Dashboard"));
                    case "Dosen":
                        return Redirect(Url.Action("Dashboard_Dosen", "Dashboard"));
                    case "Mahasiswa":
                        return Redirect(Url.Action("Dashboard_Mahasiswa", "Dashboard"));
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Wrong Employees Number or Password !";
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("User");
        }
    }
}