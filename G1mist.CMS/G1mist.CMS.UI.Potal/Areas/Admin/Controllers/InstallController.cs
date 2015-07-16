using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G1mist.CMS.UI.Potal.Models;
using G1mist.CMS.Modal;
using G1mist.CMS.IRepository;
using Newtonsoft.Json;

namespace G1mist.CMS.UI.Potal.Areas.Admin.Controllers
{
    public class InstallController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        public IRepository<tb_sales_install> InstallService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRepository<shopform> OrderService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRepository<tb_sales_user> UserService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IRepository<alliance> AllianceService { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="order"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get(string limit, string offset, string order, string search)
        {
            int total;
            List<tb_sales_install> list;

            if (IsAdmin())
            {
                //1.总部身份
                //1.1.获取所有的安装信息
                GetAllInstallData(limit, offset, search, out total, out list);
            }
            else
            {
                //2.分销商身份
                //2.1按照登录的身份获取相应分销商的安装信息
                GetInstallDataByAlliance(limit, offset, search, out total, out list);
            }

            //根据获取到的数据构建ViewModel
            var viewListModel = BuildViewModel(list);

            //根据数据生成json
            var json = GetJson(total, viewListModel);

            return Content(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Verify(string goodid)
        {
            var msg = new Message();

            if (string.IsNullOrEmpty(goodid))
            {
                msg.code = 0;
                msg.body = "产品编号为空";
            }
            else
            {
                //TODO: Verify
                var result = DoVerify(goodid);

                if (result == 1)
                {
                    msg.code = 1;
                    msg.body = "验证成功";
                }
                else if (result == -1)
                {
                    msg.code = 0;
                    msg.body = "此订单已经安装";
                }
                else if (result == 0)
                {
                    msg.code = 0;
                    msg.body = "订单不存在";
                }
                else
                {
                    msg.code = 0;
                    msg.body = "未知错误";
                }
            }
            return Json(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Install(string goodid)
        {
            var msg = new Message();
            var user = User;

            if (user == null)
            {
                msg.code = 0;
                msg.body = "会话时间到,请重新登陆";

                return Json(msg);
            }

            var allian_id = UserService.GetModal(a => a.username.Equals(user.Identity.Name)).alliance;

            var install = new tb_sales_install
            {
                install_time = DateTime.Now,
                order_id = goodid,
                allian_id = allian_id.ToString(),
                user_id = UserService.GetModal(b => b.username.Equals(user.Identity.Name)).id.ToString()
            };

            //1.在安装表中插入条目
            var result1 = InstallService.Insert(install);

            //2.修改订单中的条目,将订单标识为已安装
            var order = OrderService.GetModal(a => a.order_id.Equals(goodid));
            order.is_installed = 1;
            var result2 = OrderService.Update(order);

            if (result1 && result2)
            {
                msg.code = 1;
                msg.body = "安装成功";
            }
            else
            {
                msg.code = 0;
                msg.body = "安装失败";
            }

            return Json(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [NonAction]
        private List<InstallViewModel> BuildViewModel(List<tb_sales_install> list)
        {
            var viewListModel = new List<InstallViewModel>();

            foreach (var a in list)
            {
                var model = new InstallViewModel();
                model.order_id = a.order_id;
                model.install_time = ((DateTime)a.install_time).ToString("yyyy-MM-dd hh:mm:ss");
                model.install_user = UserService.GetModal(b => b.id.ToString().Equals(a.user_id)).username;
                model.alliance_name = AllianceService.GetModal(b => b.twocodeid.ToString().Equals(a.allian_id)).c_name;

                viewListModel.Add(model);
            }
            return viewListModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="goodid"></param>
        /// <returns></returns>
        [NonAction]
        private int DoVerify(string goodid)
        {
            var model = OrderService.GetModal(a => a.order_id.Equals(goodid));

            if (model == null)
            {
                return 0;
            }
            else if (model != null && model.is_installed == 1)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [NonAction]
        private static string GetJson(int total, List<InstallViewModel> list)
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

        /// <summary>
        /// 判断用户是否为管理员权限
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private bool IsAdmin()
        {
            var user = User;

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
                    return userModel.type == 1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        private string GetAllianceIdByCurrentUser()
        {
            var user = User;

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
                    return userModel.alliance.ToString();
                }
            }
        }

        /// <summary>
        /// 获取所有安装数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <param name="total"></param>
        /// <param name="list"></param>
        [NonAction]
        private void GetAllInstallData(string limit, string offset, string search, out int total, out List<tb_sales_install> list)
        {
            if (string.IsNullOrEmpty(search))
            {
                list = InstallService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.id > 0, a => a.install_time,
               a => new tb_sales_install
               {
                   id = a.id,
                   order_id = a.order_id,
                   install_time = a.install_time,
                   allian_id = a.allian_id,
                   user_id = a.user_id
               }, out total, false).ToList();
            }
            else
            {
                list = InstallService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.order_id.Contains(search), a => a.install_time,
              a => new tb_sales_install
              {
                  id = a.id,
                  order_id = a.order_id,
                  install_time = a.install_time,
                  allian_id = a.allian_id,
                  user_id = a.user_id
              }, out total, false).ToList();
            }
        }

        /// <summary>
        /// 获取与登录身份相关的分销商安装数据
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <param name="total"></param>
        /// <param name="list"></param>
        [NonAction]
        private void GetInstallDataByAlliance(string limit, string offset, string search, out int total, out List<tb_sales_install> list)
        {
            var allianceId = GetAllianceIdByCurrentUser();

            if (string.IsNullOrEmpty(search))
            {
                list = InstallService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.allian_id.Equals(allianceId), a => a.install_time,
               a => new tb_sales_install
               {
                   id = a.id,
                   order_id = a.order_id,
                   install_time = a.install_time,
                   allian_id = a.allian_id,
                   user_id = a.user_id
               }, out total, false).ToList();
            }
            else
            {
                list = InstallService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.order_id.Contains(search) && a.allian_id.Equals(allianceId), a => a.install_time,
              a => new tb_sales_install
              {
                  id = a.id,
                  order_id = a.order_id,
                  install_time = a.install_time,
                  allian_id = a.allian_id,
                  user_id = a.user_id
              }, out total, false).ToList();
            }
        }
    }
}
