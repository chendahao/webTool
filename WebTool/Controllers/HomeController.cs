using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebTool.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        public ActionResult GetSecurityCode()
        {
            SecurityCode code = new SecurityCode();
            Session["skey"] = code.CreateRandomCode(4);
            byte[] buf = code.CreateValidateGraphic(Session["skey"].ToString());
            return File(buf, "image/Jpeg");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}