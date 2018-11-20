using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebTool.Models;
using System.Text.RegularExpressions;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ccCovers.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /User/
        [NewAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [NewAuthorize]
        public ActionResult Details()
        {
            //获取当前展示的页
            int pageIndx = int.Parse(Request["page"]);
            //获取每页显示的数量
            int pageSize = int.Parse(Request["pageSize"]);
            //获取查询字段和值
            //ReadFilter(context);

            var data = db.Users.Include(o => o.Roles).Where(o => o.Id != null);
            //分页
            int count = data.Count();
            data = data.OrderByDescending(o => o.RegDate).Skip((pageIndx - 1) * pageSize).Take(pageSize);
            var result = data.ToList();
            //数据转换
            foreach (var r in result)
            {
                List<string> list = r.Roles.Select(o => o.Role.Name).ToList();
                r.SecurityStamp = string.Join(",", list);
                List<string> list2 = r.Roles.Select(o => o.Role.Id).ToList();
                r.PasswordHash = string.Join(",", list2);
            }
            var res = result.Select(o => new { o.Id, o.Cname, o.Email, o.Mobile, o.PasswordHash, o.RegDate, o.SecurityStamp, o.Stat, o.UserName }).ToList();
            return Json(new { total = count, data = res }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetRoles()
        {
            var list = db.Roles.ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        // POST: /Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NewAuthorize]
        public ActionResult Edit([Bind(Include = "Id,UserName,CName,Mobile,Email,Roles,OrgId,Password")] NewuserViewModel li)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Roles.Any(o => o.Name == li.UserName && o.Id != li.Id))
                    {
                        return Json("输入的用户名称已经存在，请重新输入！", JsonRequestBehavior.AllowGet);
                    }
                    string userId = "";
                    if (li.Id == "0")
                    {
                        var user = new AppUser() { UserName = li.UserName, Cname = li.Cname, Mobile = li.Mobile, Email = li.Email,CampanyName="1",RegIP="1",QQNO="1" };
                        var result = IdentityManager.CreateUser(user, li.Password);
                        if (!result)
                        {
                            return Json("用户名已存在，请重新输入", JsonRequestBehavior.AllowGet);
                        }
                        userId = user.Id;
                    }
                    else
                    {
                        var user = db.Users.Find(li.Id);
                        user.Cname = li.Cname;
                        user.Email = li.Email;
                        user.Mobile = li.Mobile;

                        db.Entry(user).State = EntityState.Modified;
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (Exception ex2)
                        {
                            return Json(ex2.Message, JsonRequestBehavior.AllowGet);
                        }
                        userId = user.Id;
                    }
                    //AppUser ruser = db.Users.Find(li.Id);
                    //移除之前的所有角色
                    IdentityManager.ClearUserRoles(userId);
                    //添加新角色
                    string[] rid = li.Roles.Split(',');
                    var roles = db.Roles.ToList();
                    int count = rid.Count();
                    for (int i = 0; i < count; i++)
                    {
                        var role = roles.FirstOrDefault(o => o.Id == rid[i]);
                        if (role != null)
                        {
                            IdentityManager.AddUserToRole(userId, role.Name);
                        }
                    }
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                string errmsg = "";
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        for (int i = item.Errors.Count - 1; i >= 0; i--)
                        {
                            errmsg += (errmsg == "" ? "" : "<br/>") + item.Errors[i].ErrorMessage;
                        }
                    }
                }
                return Json(errmsg, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Invalid(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("未指定ID", JsonRequestBehavior.AllowGet);
            }
            var li = db.Users.Find(id);
            if (li == null)
            {
                return Json("未找到匹配ID数据", JsonRequestBehavior.AllowGet);
            }
            li.Stat = (li.Stat == 0 ? 1 : 0);
            db.Entry(li).State = EntityState.Modified;
            try
            {
                //保存用户状态
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///修改用户密码 
        /// </summary>
        /// <param name="uid">uid</param>
        /// <param name="oldpassword">oldpassword</param>
        /// <param name="password">password</param>
        /// <param name="rpassword">rpassword</param>
        /// <returns></returns>
        public ActionResult SavePass(string uid, string oldpassword, string password, string rpassword)
        {
            if (string.IsNullOrEmpty(uid))
            {
                return Json("您还未登录该系统，请登录后再进行该操作", JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(oldpassword))
            {
                return Json("请输入原密码", JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrEmpty(password))
            {
                return Json("请输入新密码", JsonRequestBehavior.AllowGet);
            }
            if (password.Length < 6)
            {
                return Json("密码应不少于六位", JsonRequestBehavior.AllowGet);
            }
            if (password != rpassword)
            {
                return Json("两次输入的密码不一致", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var result = IdentityManager.ChangePwd(uid, oldpassword, password);
                if (!result)
                {
                    return Json("修改密码时出错，请检查密码是否正确。", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [NewAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return Json("未提供数据ID", JsonRequestBehavior.AllowGet); ;
            }
            AppUser users = db.Users.Find(id);
            if (users == null)
            {
                return Json("未找到指定数据", JsonRequestBehavior.AllowGet); ;
            }
            try
            {
                db.Users.Remove(users);
                await db.SaveChangesAsync();
                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
