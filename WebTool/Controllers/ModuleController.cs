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

namespace ccCovers.Controllers
{
    public class ModuleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Midule/
        //[NewAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有模块信息，分层结构
        /// </summary>
        /// <returns></returns>
        public ActionResult Details()
        {
            var md = db.Module.Select(o => new { o.ModuleId, o.ParentId, o.Name, o.ControlName, o.ActionName, o.Type, o.Stat, o.Sort, o.Icon, ParentName = o.ParentModule.Name }).OrderBy(o => o.Sort).ToList();
            return Json(md, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public ActionResult GetTree()
        {
            var md = db.Module.Where(o => o.ParentId == null).OrderBy(o => o.Sort).Select(o => new { o.ModuleId, o.Name, expanded = true, items = o.Children.Select(p => new { p.ModuleId, p.Name }) }).ToList();
            return Json(md, JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[NewAuthorize]
        public ActionResult Edit([Bind(Include = "ModuleId,Name,ControlName,ActionName,Type,Stat,Sort,ParentId,Status,Icon")] Module module)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (module.ModuleId == "0")
                    {
                        module.ModuleId = Guid.NewGuid().ToString();
                        db.Module.Add(module);
                    }
                    else
                    {
                        var list = db.Auth.Where(o => o.ModuleId == module.ModuleId).ToList();
                        foreach (var item in list)
                        {
                            item.ControlName = module.ControlName;
                        }
                        db.Entry(module).State = EntityState.Modified;
                    }
                    db.SaveChanges();
                    return Json("", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            string errmsg = "";
            if (errmsg == "")
            {
                errmsg = "数据保存错误，请检查";
            }
            return Json(errmsg, JsonRequestBehavior.AllowGet);
        }



        // POST: /Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //[NewAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return Json("未提供数据ID", JsonRequestBehavior.AllowGet);
            }
            //FindAsync(Object[]) 异步查找带给定主键值的实体
            Module module = await db.Module.FindAsync(id);
            if (module == null)
            {
                return Json("未找到指定数据", JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.Module.Remove(module);
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
