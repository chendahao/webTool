using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTool.Models
{
    public class AuthorizeKey
    {
        private static List<AuthOne> Keys = new List<AuthOne>();
        private static Dictionary<string, string> keyNames = new Dictionary<string, string>();

        public static Dictionary<string, string> KeyNames
        {
            get
            {
                if (keyNames == null)
                {
                    keyNames = new Dictionary<string, string>();
                }
                if (keyNames.Count < 1)
                {
                    keyNames.Add("InsertKey", "Create");
                    keyNames.Add("EditKey", "Edit");
                    keyNames.Add("DeleteKey", "Delete");
                    keyNames.Add("ListKey", "Index");
                    keyNames.Add("ViewKey", "Details");
                    keyNames.Add("AuditKey", "Audit");
                    keyNames.Add("InvalidKey", "Invalid");
                }
                return AuthorizeKey.keyNames;
            }
        }
        public static List<AuthOne> GetKeys()
        {
            if (Keys.Count < 1)
            {
                Keys.Add(new AuthOne() { key = "InsertKey", name = "Create", cname = "新增" });
                Keys.Add(new AuthOne() { key = "EditKey", name = "Edit", cname = "编辑" });
                Keys.Add(new AuthOne() { key = "DeleteKey", name = "Delete", cname = "删除" });
                Keys.Add(new AuthOne() { key = "ListKey", name = "Index", cname = "查看" });
                Keys.Add(new AuthOne() { key = "ViewKey", name = "Details", cname = "明细" });
                Keys.Add(new AuthOne() { key = "AuditKey", name = "Audit", cname = "审核" });
                Keys.Add(new AuthOne() { key = "InvalidKey", name = "Invalid", cname = "激活" });
            }
            return Keys;
        }


        #region 常规权限判断
        /// <summary>
        /// 判断是否具有插入权限
        /// </summary>
        public bool CanInsert { get; set; }

        /// <summary>
        /// 判断是否具有更新权限
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// 判断是否具有删除权限
        /// </summary>
        public bool CanDelete { get; set; }

        /// <summary>
        /// 判断是否具有列表权限
        /// </summary>
        public bool CanList { get; set; }

        /// <summary>
        /// 判断是否具有查看权限
        /// </summary>
        public bool CanView { get; set; }

        /// <summary>
        /// 判断是否具有审核权限
        /// </summary>
        public bool CanAudit { get; set; }

        /// <summary>
        /// 判断是否具有作废权限
        /// </summary>
        public bool CanInvalid { get; set; }

        #endregion

        public AuthorizeKey()
        {
            this.CanInsert = false;
            this.CanEdit = false;
            this.CanDelete = false;
            this.CanList = false;
            this.CanView = false;
            this.CanAudit = false;
            this.CanInvalid = false;
        }
        /// <summary>
        /// 常用构造函数
        /// </summary>
        public AuthorizeKey(string keys)
        {
            this.CanInsert = keys.Contains("Create");
            this.CanEdit = keys.Contains("Edit");
            this.CanDelete = keys.Contains("Delete");
            this.CanList = keys.Contains("Index");
            this.CanView = keys.Contains("Details");
            this.CanAudit = keys.Contains("Audit");
            this.CanInvalid = keys.Contains("Invalid");
        }
    }
}