using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace WebTool.Models
{
    public class BaseModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged 成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string PropertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        #endregion
    }

    public class AppUser : IdentityUser
    {
        
        public AppUser()
            : base()
        {
            this.RegDate = DateTime.Now;
            this.EndDate = DateTime.Now;
        }

        private string cname;
        [Required]
        [StringLength(20)]
        [Display(Name = "登录名")]
        public string Cname
        {
            get { return cname; }
            set { cname = value; }
        }
        //private string userName;
        //[Required]
        //[StringLength(20)]
        //[Display(Name = "联系人")]
        //public string UName
        //{
        //    get { return userName; }
        //    set { userName = value; }
        //}

        private string email;
        [StringLength(100)]
        [Display(Name = "邮箱")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string mobile;
        [StringLength(20)]
        [Display(Name = "手机")]
        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }
        private string qQNO;
        [StringLength(20)]
        [Display(Name = "qq")]
        public string QQNO
        {
            get { return qQNO; }
            set { qQNO = value; }
        }

        private string campanyName;
        [StringLength(50)]
        [Display(Name = "公司名称")]
        public string CampanyName
        {
            get { return campanyName; }
            set { campanyName = value; }
        }

        private int stat;
        /// <summary>
        /// 锁定状态，锁定=0，可登陆=1
        /// 这里在增加一个状态2 封
        /// </summary>
        [Required]
        [Display(Name = "锁定状态")]
        public int Stat
        {
            get { return stat; }
            set { stat = value; }
        }

        private int lvl;
        /// <summary>
        /// 默认0  
        /// </summary>
        [Required]
        [Display(Name = "账号级别")]
        public int Lvl
        {
            get { return lvl; }
            set { lvl = value; }
        }

        private int canSend;
        /// <summary>
        /// 默认0    可发调试与账号级别对应，当调整账号级别的时候应该增加可发条数
        /// 最好是可调整的数值input
        /// </summary>
        [Required]
        [Display(Name = "可发送条数")]
        public int CanSend
        {
            get { return canSend; }
            set { canSend = value; }
        }

        private DateTime regDate;
        [Display(Name = "日期")]
        public DateTime RegDate
        {
            get { return regDate; }
            set { regDate = value; }
        }
        /// <summary>
        /// 注册时为空 ？
        /// 会员到期时间，默认激活后设置
        /// </summary>
        private DateTime? endDate;
        [Display(Name = "会员到期时间")]
        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        private string regIP;
        [Display(Name = "注册ip")]
        public string RegIP
        {
            get { return regIP; }
            set { regIP = value; }
        }
    }

    [Table("Module")]
    public class Module : BaseModel
    {
        public Module()
        {
            this.ModuleId = Guid.NewGuid().ToString();
        }

        private string moduleId;
        [Key]
        [StringLength(128)]
        public string ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; OnPropertyChanged(); }
        }

        private string name;
        /// <summary>
        /// 模块名称
        /// </summary>
        [StringLength(100)]
        [Required]
        [Display(Name = "模块名称")]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private string controlName;
        /// <summary>
        /// 控制器名称
        /// </summary>
        [StringLength(50)]
        [Required]
        [Display(Name = "控制器名称")]
        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; OnPropertyChanged(); }
        }

        private string actionName;
        /// <summary>
        /// 动作名称
        /// </summary>
        [StringLength(50)]
        [Required]
        [Display(Name = "动作名称")]
        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; OnPropertyChanged(); }
        }

        private int type;
        /// <summary>
        /// 是否是菜单类型，this=0表示为不是菜单显示，this=1表示为菜单显示
        /// </summary>
        [Display(Name = "菜单类型")]
        public int Type
        {
            get { return type; }
            set { type = value; OnPropertyChanged(); }
        }

        private int stat;
        /// <summary>
        /// 有效状态，=1 表示有效，=0表示无效
        /// </summary>
        [Display(Name = "有效状态")]
        public int Stat
        {
            get { return stat; }
            set { stat = value; OnPropertyChanged(); }
        }

        private int sort;
        /// <summary>
        /// 排序号，用于排序
        /// </summary>
        [Required]
        [Display(Name = "排序号")]
        public int Sort
        {
            get { return sort; }
            set { sort = value; OnPropertyChanged(); }
        }

        private string icon;
        /// <summary>
        /// 图标名称
        /// </summary>
        [StringLength(50)]
        [Display(Name = "图标名称")]
        public string Icon
        {
            get { return icon; }
            set { icon = value; OnPropertyChanged(); }
        }

        private string parentId;
        /// <summary>
        /// 自关联，上级模块
        /// </summary>
        [Display(Name = "上级模块")]
        public string ParentId
        {
            get { return parentId; }
            set { parentId = value; OnPropertyChanged(); }
        }
        public virtual Module ParentModule { get; set; }

        /// <summary>
        /// 自关联，下级模块
        /// </summary>
        [ForeignKey("ParentId")]
        public virtual List<Module> Children { get; set; }
    }

    [Table("Auth")]
    public class Auth : BaseModel
    {
        public Auth()
        {
        }

        private string roleId;
        [Key, Column(Order = 0)]
        public string RoleId
        {
            get { return roleId; }
            set { roleId = value; OnPropertyChanged(); }
        }

        private string moduleId;
        [Key, Column(Order = 1)]
        public string ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; OnPropertyChanged(); }
        }

        private string controlName;
        /// <summary>
        /// 控制器名称
        /// </summary>
        [StringLength(50)]
        [Required]
        [Display(Name = "控制器名称")]
        public string ControlName
        {
            get { return controlName; }
            set { controlName = value; OnPropertyChanged(); }
        }

        private string acl;
        /// <summary>
        /// 模块名称
        /// </summary>
        [StringLength(500)]
        [Required]
        [Display(Name = "授权码")]
        public string Acl
        {
            get { return acl; }
            set { acl = value; OnPropertyChanged(); }
        }

    }

    /// <summary>
    /// 参数表
    /// </summary>
    [Table("Parameter")]
    public class Parameter : BaseModel
    {
        public Parameter()
            : base()
        {
            this.ParameterId = Guid.NewGuid().ToString();
        }
        private string parameterId;
        [Key]
        [StringLength(128)]
        public string ParameterId
        {
            get { return parameterId; }
            set { parameterId = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 参数名称
        /// </summary>
        private string parameterName;
        [StringLength(50)]
        [Required]
        [Index(IsUnique = true)]
        public string ParameterName
        {
            get { return parameterName; }
            set { parameterName = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 参数描述
        /// </summary>
        private string parameterDescribe;
        [StringLength(50)]
        [Required]
        public string ParameterDescribe
        {
            get { return parameterDescribe; }
            set { parameterDescribe = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 参数值
        /// </summary>
        private string parameterValue;
        [Required]
        public string ParameterValue
        {
            get { return parameterValue; }
            set { parameterValue = value; OnPropertyChanged(); }
        }
    }

    /// <summary>
    /// 字典表
    /// </summary>
    [Table("Incode")]
    public class Incode : BaseModel
    {
        public Incode()
            : base()
        {
            this.IncodeId = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// ID
        /// </summary>
        private string incodeId;
        [Key]
        [StringLength(128)]
        public string IncodeId
        {
            get { return incodeId; }
            set { incodeId = value; OnPropertyChanged(); }
        }
        /// <summary>
        /// 名称
        /// </summary>
        private string name;
        [Required]
        [Display(Name = "名称")]
        [StringLength(100)]
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 分类
        /// </summary>
        private string category;
        [Required]
        [Display(Name = "分类")]
        [StringLength(100)]
        public string Category
        {
            get { return category; }
            set { category = value; OnPropertyChanged(); }
        }

        /// <summary>
        /// 排序
        /// </summary>
        private int sort;

        [Display(Name = "排序")]

        public int Sort
        {
            get { return sort; }
            set { sort = value; OnPropertyChanged(); }
        }

    }
}