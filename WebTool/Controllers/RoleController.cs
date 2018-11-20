using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebTool.Models;
using Microsoft.AspNet.Identity.EntityFramework;


namespace ccCover.Controllers
{
    public class RoleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Role/
        [NewAuthorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Details()
        {
            //获取当前展示的页
            int pageIndx = int.Parse(Request["page"]);
            //获取每页显示的数量
            int pageSize = int.Parse(Request["pageSize"]);
            //获取查询字段和值
            //ReadFilter(context);

            //分页
            var data = db.Roles.Where(o => o.Id != null);
            int count = data.Count();
            data = data.OrderBy(o => o.Name).Skip((pageIndx - 1) * pageSize).Take(pageSize);
            var result = data.ToList();
            return Json(new { total = count, data = result }, JsonRequestBehavior.AllowGet);
        }
        void ReadFilter(HttpRequestBase req)
        {
            var dicSearch = new Dictionary<string, object>();//定义变量存储查询的字段和值
            //获取所有存储查询字段和值的url的query参数，“filter[filters]”是进行查询请求时的请求标识
            var querys = req.Form.AllKeys.Where(m => m.StartsWith("filter[filters]"));
            //计算查询请求时查询参数的个数
            var queryCount = querys.Count(m => m.EndsWith("[field]"));
            for (int i = 0; i < queryCount; i++)
            {
                //请查询字段和对应的值存储在一个字典中
                dicSearch.Add(req.QueryString["filter[filters][" + i + "][field]"], req.QueryString["filter[filters][" + i + "][value]"]);
            }
        }
        [NewAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[NewAuthorize]
        public ActionResult Edit([Bind(Include = "Id,Name")] IdentityRole li)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (db.Roles.Any(o => o.Name == li.Name && o.Id != li.Id))
                    {
                        return Json("输入的角色名称已经存在，请重新输入！", JsonRequestBehavior.AllowGet);
                    }
                    if (li.Id == "0")
                    {
                        li.Id = Guid.NewGuid().ToString();
                        db.Roles.Add(li);
                    }
                    else
                    {
                        db.Entry(li).State = EntityState.Modified;
                    }
                    db.SaveChanges();
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

        [NewAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json("未指定ID", JsonRequestBehavior.AllowGet);
            }
            var li = db.Roles.Find(id);
            if (li == null)
            {
                return Json("未找到匹配ID数据", JsonRequestBehavior.AllowGet);
            }
            db.Roles.Remove(li);
            try
            {
                db.SaveChanges();
                string sql = "delete from auth where RoleId='" + id + "'";
                db.ExecSql(sql);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取授权
        /// </summary>
        /// <returns></returns>
        public ActionResult GetModule(string id)
        {
            var auth = db.Auth.Where(p => p.RoleId == id);
            var module = db.Module.GroupJoin(auth, md => md.ModuleId, au => au.ModuleId, (md, au) => new
            {
                md.ModuleId,
                md.ParentId,
                md.ControlName,
                md.Name,
                md.Icon,
                md.Sort,
                rid = id,
                Create = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Create") : false),
                Edit = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Edit") : false),
                Delete = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Delete") : false),
                Index = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Index") : false),
                Details = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Details") : false),
                Audit = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Audit") : false),
                Invalid = (au.FirstOrDefault() != null ? au.FirstOrDefault().Acl.Contains("Invalid") : false),
            }).OrderBy(o => o.Sort).ToArray();
            //var data = new { rows = module };
            return Json(module, JsonRequestBehavior.AllowGet);
        }
        [NewAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[NewAuthorize]
        public ActionResult Audit(string rid, string ModuleId, string ControlName, string Create, string Edit, string Delete, string Index, string Details, string Audit, string Invalid)
        {
            if (rid == null || ModuleId == null)
            {
                return Json("未提供数据ID", JsonRequestBehavior.AllowGet);
            }
            string acl = "";
            acl += (string.IsNullOrEmpty(Index) ? "" : ",Index");
            acl += (string.IsNullOrEmpty(Details) ? "" : ",Details");
            acl += (string.IsNullOrEmpty(Create) ? "" : ",Create");
            acl += (string.IsNullOrEmpty(Edit) ? "" : ",Edit");
            acl += (string.IsNullOrEmpty(Delete) ? "" : ",Delete");
            acl += (string.IsNullOrEmpty(Audit) ? "" : ",Audit");
            acl += (string.IsNullOrEmpty(Invalid) ? "" : ",Invalid");
            //注意事项：acl为空时，表示没有任何权限，特殊处理，当有此记录时删除，没有记录时跳过
            var auth = db.Auth.Where(o => o.RoleId == rid && o.ModuleId == ModuleId).FirstOrDefault();
            if (auth == null)
            {
                if (acl.Trim() != "")
                {
                    Auth au = new Auth();
                    au.RoleId = rid;
                    au.ModuleId = ModuleId;
                    au.ControlName = ControlName;
                    au.Acl = acl;
                    db.Auth.Add(au);
                }
            }
            else
            {
                if (acl.Trim() == "")
                {
                    db.Auth.Remove(auth);
                }
                else
                {
                    db.Entry(auth).State = EntityState.Modified;
                    auth.ControlName = ControlName;
                    auth.Acl = acl;
                }
            }
            try
            {
                db.SaveChanges();

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
