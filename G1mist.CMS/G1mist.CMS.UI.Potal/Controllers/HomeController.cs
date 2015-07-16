using System.Web.Mvc;
using G1mist.CMS.Modal;
using G1mist.CMS.Repository;
using G1mist.CMS.Common;
using System;

namespace G1mist.CMS.UI.Potal.Controllers
{
    public class HomeController : Controller
    {
        public IRepository.IRepository<tb_sales_user> UserService { get; set; }

        public bool DoAdd(tb_sales_user user)
        {
            //生成6位加密盐
            var salt = Security.GetPwdSalt();

            //初始密码为666666
            //将密码与加密盐混淆,做MD5运算
            var pass = Security.SetMD5("666666", salt);

            //创建时间
            user.createtime = DateTime.Now;
            //密码
            user.password = pass;
            //加密盐
            user.salt = salt;

            //执行插入操作
            return UserService.Insert(user);
        }
        //
        // GET: /Home/
        public ActionResult Index()
        {
            var user = new tb_sales_user { username = "gaoning", type = 1, isenable = 1 };
            var result = DoAdd(user);

            return Content(result.ToString());
            //return Content("Home");
        }
    }
}
