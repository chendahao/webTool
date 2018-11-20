using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebTool.Models
{
    /// <summary>
    /// 二级域名表
    /// </summary>
    [Table("realmNameInfo")]
    public class realmNameInfo : BaseModel
    {
        //序号     ID
        //域名名称 realmName  
        //域名地址 realmAddress 
        //是否可用 isUsing
        /// <summary>
        /// id
        /// </summary>
        private string id;
        [Key]
        [StringLength(128)]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        /// <summary>
        /// 域名名称
        /// </summary>
        private string realmName;
        [Required]
        [StringLength(100)]
        public string RealmName
        {
            get { return realmName; }
            set { realmName = value; }
        }
        /// <summary>
        /// 域名地址
        /// </summary>
        private string realmAddress;
        [Required]
        [StringLength(200)]
        public string RealmAddress
        {
            get { return realmAddress; }
            set { realmAddress = value; }
        }
        /// <summary>
        /// 是否可用 1可 0不可
        /// </summary>
        private int isUsing;

        public int IsUsing
        {
            get { return isUsing; }
            set { isUsing = value; }
        }
    }
}