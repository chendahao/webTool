using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Net;

namespace WebTool.Models
{
    public class NewAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            filterContext.Controller.ViewData["Ctrl"] = controllerName;
            if (System.Web.HttpContext.Current.Session["cchp.auth"] == null)
            {
                filterContext.HttpContext.Session["error"] = new Exception("您未被授权，请联系管理员为您授权");
                //filterContext.Result =  new RedirectResult("~/Home/Error");
                if (actionName.ToLower().Trim() == "index" && controllerName.ToLower() == "home")
                {
                    filterContext.Result = new RedirectResult("~/Account/Login");
                }
                else if (actionName.ToLower().Trim() == "index")
                {
                    filterContext.Result = new RedirectResult("~/Home/Error");
                }
                else if (actionName.ToLower().Trim() == "details")
                {
                    filterContext.Result = new RedirectResult("~/Home/Details");
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/Home/JError");
                }
            }
            else
            {
                List<Auth> list = System.Web.HttpContext.Current.Session["cchp.auth"] as List<Auth>;
                List<Auth> clist = list.Where(o => o.Acl.Contains(actionName) && o.ControlName == controllerName).ToList();
                if (clist.Count < 1)
                {
                    filterContext.HttpContext.Session["error"] = new Exception("您未被授权使用此功能：" + controllerName + "/" + actionName);
                    //filterContext.Result = new RedirectResult("~/Home/Error");
                    if (actionName.ToLower().Trim() == "index")
                    {
                        filterContext.Result = new RedirectResult("~/Home/Error");
                    }
                    else if (actionName.ToLower().Trim() == "details")
                    {
                        filterContext.Result = new RedirectResult("~/Home/Details");
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult("~/Home/JError");
                    }
                }
                string keys = "";
                foreach (Auth au in clist)
                {
                    keys += "," + au.Acl;
                }
                filterContext.Controller.ViewData["cchp.keys"] = new AuthorizeKey(keys);
            }

        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            if (Roles == null)
            {
                return true;
            }
            if (Roles.Length == 0)
            {
                return true;
            }
            return false;
        }
    }
}