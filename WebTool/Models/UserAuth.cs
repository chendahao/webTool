using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace WebTool.Models
{
    public sealed class UserAuth
    {
        public static void getAuths(AppUser user, ref List<Auth> auths, ref List<Module> modules, ref List<string> moduleIds)
        {
            List<string> rids = user.Roles.Select(o => o.RoleId).ToList();
            //foreach (IdentityUserRole ur in user.Roles)
            //{
            //    rids.Add(ur.RoleId);
            //}
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                auths = db.Auth.Where(o => rids.Contains(o.RoleId)).ToList();
                moduleIds = auths.Where(o => o.Acl.Contains("Index")).Select(o => o.ModuleId).ToList();
                modules = db.Module.Include(o => o.Children).Where(o => o.ParentId == null).OrderBy(o => o.ParentId).ThenBy(o => o.Sort).ToList();
                foreach (var m in modules)
                {
                    if (m.Children.Count > 0)
                    {
                        foreach (var mm in m.Children)
                        {
                            if (mm.Children.Count > 0)
                            {
                                //为了加载数据
                            }
                        }
                    }
                }
                //moduleIds = modules.Select(o => o.ModuleId).ToList();                
            }
        }

        public static AppUser FindUser(string userName, string userPass)
        {
            if (userName == null || userPass == null)
            {
                return null;
            }
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var user = db.Users.Include(o => o.Roles).Where(o => o.UserName == userName).FirstOrDefault();
                if (user != null)
                {
                    PasswordHasher ph = new PasswordHasher();
                    if (ph.VerifyHashedPassword(user.PasswordHash, userPass) == PasswordVerificationResult.Success)
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        //public static string GetParameter(string param)
        //{
        //    using (AppDbContext db = new AppDbContext())
        //    {
        //        var pm = db.Parameter.FirstOrDefault(o => o.ParameterName == param);
        //        if (pm == null) return "";
        //        else return pm.ParameterValue;
        //    }
        //}
    }
}