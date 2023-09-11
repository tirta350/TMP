using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;

namespace TMP.Controllers
{
    public class NotificationController : Controller
    {
        // GET: Notification
        public ActionResult Index()
        {
            Proyek _proyek = new Proyek();
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(_proyek.getAllData(user));
        }
    }
}