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

namespace WebTool.Controllers
{
    public class realmNameInfoesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: realmNameInfoes
        [NewAuthorize]
        public ActionResult Index()
        {
            return View();
        }

        [NewAuthorize]
        // GET: realmNameInfoes/Details/5
        public ActionResult Details(int? page, int? PageSize)
        {
            int Page = page ?? 1;
            int Rows = PageSize ?? 15;
            var list = db.realmNameInfo.OrderBy(o=>o.RealmName);
            var data = list.Skip((Page - 1) * Rows).Take(Rows).ToList();
            return Json(new { total = list.Count(), data = data }, JsonRequestBehavior.AllowGet);
        }



        // POST: realmNameInfoes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [NewAuthorize]
        public ActionResult Edit([Bind(Include = "Id,RealmName,RealmAddress,IsUsing")] realmNameInfo realmNameInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (realmNameInfo.Id=="0")
                    {
                        if (IsExist(realmNameInfo.RealmAddress))
                        {
                            return Json(new { errorMsg = "该地址已经存在,无需添加！" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            realmNameInfo.Id = Guid.NewGuid().ToString();
                            db.realmNameInfo.Add(realmNameInfo);
                        }
                    }
                    else
                    {
                        var ise2 = db.realmNameInfo.Where(o => o.Id != realmNameInfo.Id && o.RealmAddress == realmNameInfo.RealmAddress).FirstOrDefault();
                        if (IsExist(realmNameInfo.RealmAddress) && ise2!=null)
                        {
                            return Json(new { errorMsg = "该地址已经存在！" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            db.Entry(realmNameInfo).State = EntityState.Modified;
                        }
                    }
                    db.SaveChanges();
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { errorMsg = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { errorMsg = "数据保存时出错，请联系管理员" }, JsonRequestBehavior.AllowGet);
        }
        //判断二级域名表中是否存在该地址
        private bool IsExist(string address)
        {
             return  db.realmNameInfo.Any(o => o.RealmAddress == address);
        }

        // POST: realmNameInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [NewAuthorize]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Json(new { errorMsg = "id不能为空" }, JsonRequestBehavior.AllowGet);
            }
            realmNameInfo re = await db.realmNameInfo.FindAsync(id);
            if (re==null)
            {
                return Json(new { errorMsg = "为找到该条数据" }, JsonRequestBehavior.AllowGet);
            }
            try
            {
                db.realmNameInfo.Remove(re);
                await db.SaveChangesAsync();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { errorMsg = ex.Message }, JsonRequestBehavior.AllowGet);
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
