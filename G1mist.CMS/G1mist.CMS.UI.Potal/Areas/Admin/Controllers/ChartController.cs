using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G1mist.CMS.UI.Potal.Models;
using Newtonsoft.Json;
using G1mist.CMS.Modal;
using G1mist.CMS.IRepository;
using System.Data.SqlClient;
using System.Data;
using G1mist.CMS.DAL;
using System.Data.Objects;

namespace G1mist.CMS.UI.Potal.Areas.Admin.Controllers
{
    public class ChartController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public IRepository<tb_sales_install> InstallService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRepository<tb_sales_user> UserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRepository<alliance> AllianceService { get; set; }

        [HttpGet]
        public ActionResult GetInstallChart()
        {
            var list = new List<ChartInstall>();
            var context = ContextFactory.GetCurrentDbContext();

            var user = User;

            if (user == null)
            {
                throw new Exception("会话过期");
            }

            var aid = UserService.GetModal(a => a.username.Equals(user.Identity.Name)).alliance.ToString();

            var sql = @"select DATE_FORMAT(t.install_time,'%Y-%m') date,COUNT(*) count from tb_sales_install t where allian_id=" + aid + " and t.install_time>=DATE_SUB(CURRENT_DATE,INTERVAL 5 month) GROUP BY date;";

            var result = context.ExecuteStoreQuery<InstallChartModel>(sql).ToList();

            result.ForEach((a) =>
            {
                list.Add(new ChartInstall { color = "#9f7961", name = a.date, value = a.count });
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAdminInstallChart(string limit, string offset, string order, string search)
        {
            var context = ContextFactory.GetCurrentDbContext();

            var sql = @"select t.allian_id allian,DATE_FORMAT(t.install_time,'%Y-%m') date,COUNT(*) count from tb_sales_install t  GROUP BY date,t.allian_id ORDER BY date desc";

            var result = context.ExecuteStoreQuery<AdminInstallViewModel>(sql).ToList();

            var resultSet = new List<AdminInstallViewModel>();

            foreach (var item in result)
            {
                var alliance = AllianceService.GetModal(a => a.twocodeid.ToString().Equals(item.allian));

                if (alliance == null)
                {
                    throw new Exception("经销商不存在");
                }

                resultSet.Add(new AdminInstallViewModel
                {
                    allian = alliance.c_name,
                    count = item.count,
                    date = item.date
                });
            }

            var json = GetJson(resultSet.Count, resultSet);

            return Content(json);
        }

        [HttpGet]
        public ActionResult GetFocusChart()
        {
            var list = new List<ChartInstall>();
            var context = ContextFactory.GetCurrentDbContext();

            var user = User;

            if (user == null)
            {
                throw new Exception("会话过期");
            }

            var location = UserService.GetModal(a => a.username.Equals(user.Identity.Name)).alliance.ToString();

            var sql = @"select DATE_FORMAT(t.addtime,'%Y-%m') date,COUNT(*) count from tb_user t where location='" + location + "' and t.addtime>=DATE_SUB(CURRENT_DATE,INTERVAL 5 month) GROUP BY date;";

            var result = context.ExecuteStoreQuery<InstallChartModel>(sql).ToList();

            result.ForEach((a) =>
            {
                list.Add(new ChartInstall { color = "#a5c2d5", name = a.date, value = a.count });
            });

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetAdminFocusChart(string limit, string offset, string order, string search)
        {
            var context = ContextFactory.GetCurrentDbContext();

            var sql = @"select t.location allian,DATE_FORMAT(t.addtime,'%Y-%m') date,COUNT(*) count from tb_user t  GROUP BY date,allian ORDER BY date desc ";

            //所有得结果
            var allResult = context.ExecuteStoreQuery<AdminInstallViewModel>(sql).ToList();
            //分页
            var result = allResult.Skip(int.Parse(offset)).Take(int.Parse(limit)).ToList();

            var resultSet = new List<AdminInstallViewModel>();

            foreach (var item in result)
            {
                var alliance = AllianceService.GetModal(a => a.twocodeid.ToString().Equals(item.allian));

                if (alliance == null)
                {
                    throw new Exception("经销商不存在");
                }

                resultSet.Add(new AdminInstallViewModel
                {
                    allian = alliance.c_name,
                    count = item.count,
                    date = item.date
                });
            }

            var json = GetJson(allResult.Count, resultSet);

            return Content(json);
        }

        [HttpGet]
        public ActionResult IsAdmin()
        {
            var user = User;
            var msg = new Message();

            if (user == null)
            {
                throw new Exception("会话过期,请重新登录");
            }
            else
            {
                var userModel = UserService.GetModal(a => a.username.Equals(user.Identity.Name));

                if (userModel == null)
                {
                    throw new Exception("用户不存在");
                }
                else
                {
                    if (userModel.type == 1)
                    {
                        msg.code = 1;
                    }
                    else
                    {
                        msg.code = 0;
                    }
                }
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 根据数据生成JSON字串
        /// </summary>
        /// <param name="total"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [NonAction]
        private static string GetJson(int total, List<AdminInstallViewModel> list)
        {
            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            var result = new
            {
                total,
                rows = list
            };

            var json = JsonConvert.SerializeObject(result, settings);
            return json;
        }
    }
}
