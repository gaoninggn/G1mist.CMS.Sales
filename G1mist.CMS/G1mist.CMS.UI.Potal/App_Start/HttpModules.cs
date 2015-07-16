using System;
using System.IO;
using System.Linq;
using System.Web;

namespace G1mist.CMS.UI.Potal
{
    public class HttpModules : IHttpModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
            context.AuthorizeRequest += context_AuthorizeRequest;
        }

        /// <summary>
        /// 在这个事件中验证用户是否得到授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void context_AuthorizeRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            //获取访问URL
            var path = context.Request.Path;
            //处理对后台管理页面的访问
            HandleAdminBackStage(application, path, context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void context_BeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            //获取访问URL
            var path = context.Request.Path;
            //获取访问文件的拓展名
            var extention = Path.GetExtension(path);

            //处理对模板页的访问
            HandleAccessTemplates(extention, context);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }

        #region 辅助方法
        /// <summary>
        /// 处理对后台管理页面的访问
        /// </summary>
        /// <param name="path"></param>
        /// <param name="context"></param>
        private void HandleAdminBackStage(HttpApplication app, string path, HttpContext context)
        {
            path = path.ToLower();
            if (path.Contains("admin") && !path.Contains("admin/login") && !path.Contains("admin/user/login"))
            {
                //如果访问的是后台管理系统,则判断是否登录
                var result = CheckAuth(context);

                //如果用户未经授权,则跳转至登录页面
                if (!result)
                {
                    context.Response.Redirect("/areas/admin/login.html", true);
                    context.Response.End();
                }
            }
        }

        /// <summary>
        /// 检查用户是否授权
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private bool CheckAuth(HttpContext context)
        {
            if (context.User == null)
            {
                return false;
            }

            return context.User.Identity.IsAuthenticated;
        }

        /// <summary>
        /// 访问模板页面时的处理
        /// </summary>
        /// <param name="extention">访问的文件拓展名(.htm)</param>
        /// <param name="context">HttpContext</param>
        private static void HandleAccessTemplates(string extention, HttpContext context)
        {
            if (extention.Equals(".htm"))
            {
                context.Response.Redirect("/static/error.html");
                context.Response.End();
            }
        }
        #endregion
    }
}