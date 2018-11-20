using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTool.Models
{
    public static class IdentityManager
    {
        // 判断角色是否已在存在
        public static bool RoleExists(string name)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.RoleExists(name);
        }
        // 获取角色
        public static IdentityRole GetRole(string id)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            return rm.FindById(id);
        }
        /// <summary>
        /// 获取用户的所有角色名称
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public static List<string> GetRoles(string userid)
        {
            var um = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
            return um.GetRoles(userid).ToList();
        }
        // 新增角色
        public static bool CreateRole(string name)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var idResult = rm.Create(new IdentityRole(name));
            return idResult.Succeeded;
        }
        // 新增用户
        public static bool CreateUser(AppUser user, string password)
        {
            var um = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
            var idResult = um.Create(user, password);
            return idResult.Succeeded;
        }
        //修改密码
        public static bool ChangePwd(string userId, string oldpassword, string password)
        {
            var cp = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
            var idResult = cp.ChangePassword(userId, oldpassword, password);
            return idResult.Succeeded;
        }
        // 将使用者加入角色中
        public static bool AddUserToRole(string userId, string roleName)
        {
            var um = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
            var idResult = um.AddToRole(userId, roleName);
            return idResult.Succeeded;
        }
        // 清除使用者的角色设定
        public static void ClearUserRoles(string userId)
        {
            var um = new UserManager<AppUser>(new UserStore<AppUser>(new ApplicationDbContext()));
            var user = um.FindById(userId);
            var currentRoles = new List<IdentityUserRole>();
            currentRoles.AddRange(user.Roles);
            foreach (var role in currentRoles)
            {
                um.RemoveFromRole(userId, role.Role.Name);
            }
        }
    }
}