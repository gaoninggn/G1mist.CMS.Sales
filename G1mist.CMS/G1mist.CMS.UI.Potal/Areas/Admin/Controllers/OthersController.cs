using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using G1mist.CMS.Modal;
using G1mist.CMS.IRepository;
using G1mist.CMS.UI.Potal.Models;
using Newtonsoft.Json;

namespace G1mist.CMS.UI.Potal.Areas.Admin.Controllers
{
    public class OthersController : Controller
    {
        //费用Service
        public IRepository<tb_sales_fee> FeeService { get; set; }

        //用户Service
        public IRepository<tb_sales_user> UserService { get; set; }

        //订单Service
        public IRepository<shopform> OrderService { get; set; }

        //关注Service
        public IRepository<tb_user> FocusService { get; set; }

        //分销商Service
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
            List<shopform> list;

            if (string.IsNullOrEmpty(search))
            {
                list = OrderService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.id > 0, a => a.real_time,
               a => new shopform
               {
                   order_status = a.order_status,
                   order_total_price = a.order_total_price,
                   order_create_time = a.order_create_time,
                   receiver_name = a.receiver_name,
                   buyer_nick = a.buyer_nick,
                   car = a.car,
                   product_name = a.product_name,
                   product_count = a.product_count,
                   real_time = a.real_time
               }, out total, false).ToList();
            }
            else
            {
                list = OrderService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.product_name.Contains(search), a => a.real_time,
              a => new shopform
              {
                  order_status = a.order_status,
                  order_total_price = a.order_total_price,
                  order_create_time = a.order_create_time,
                  receiver_name = a.receiver_name,
                  buyer_nick = a.buyer_nick,
                  car = a.car,
                  product_name = a.product_name,
                  product_count = a.product_count,
                  real_time = a.real_time
              }, out total, false).ToList();
            }

            //根据数据生成json
            var json = GetJson(total, list);

            return Content(json);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="order"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetBook(string limit, string offset, string order, string search)
        {
            int total;
            List<shopform> list;

            if (string.IsNullOrEmpty(search))
            {
                list = OrderService.GetListByPage(int.Parse(offset), int.Parse(limit), a => !string.IsNullOrEmpty(a.pre_time) && a.is_installed == 0, a => a.real_time,
               a => new shopform
               {
                   order_status = a.order_status,
                   order_total_price = a.order_total_price,
                   order_create_time = a.order_create_time,
                   receiver_name = a.receiver_name,
                   buyer_nick = a.buyer_nick,
                   car = a.car,
                   product_name = a.product_name,
                   product_count = a.product_count,
                   real_time = a.real_time,
                   pre_time = a.pre_time
               }, out total, false).ToList();
            }
            else
            {
                list = OrderService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.product_name.Contains(search) && !string.IsNullOrEmpty(a.pre_time) && a.is_installed == 0, a => a.real_time,
              a => new shopform
              {
                  order_status = a.order_status,
                  order_total_price = a.order_total_price,
                  order_create_time = a.order_create_time,
                  receiver_name = a.receiver_name,
                  buyer_nick = a.buyer_nick,
                  car = a.car,
                  product_name = a.product_name,
                  product_count = a.product_count,
                  real_time = a.real_time,
                  pre_time = a.pre_time
              }, out total, false).ToList();
            }

            //根据数据生成json
            var json = GetJson(total, list);

            return Content(json);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fee"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFee(tb_sales_fee fee)
        {
            var msg = new Message();

            if (UserService.GetModal(a => a.username.Equals(User.Identity.Name)).type == 0)
            {
                msg.code = 0;
                msg.body = "您的权限不足";

                return Json(msg);
            }

            if (fee.installfee <= 0 || fee.recommendfee <= 0)
            {
                fee.installfee = 0;
                fee.recommendfee = 0;
            }
            else
            {
                var fees = FeeService.GetList(a => a.id > 0);
                if (fees != null)
                {
                    if (fees.Count >= 1)
                    {
                        fees.ToList().ForEach((f) => { FeeService.Delete(f); });
                    }
                }

                var result = FeeService.Insert(fee);

                if (result)
                {
                    msg.code = 1;
                    msg.body = "设置成功";
                }
                else
                {
                    msg.code = 0;
                    msg.body = "设置失败";
                }
            }
            return Json(msg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFee()
        {
            var fee = FeeService.GetModal(a => a.id > 0);

            if (fee != null)
            {
                return Json(new { installfee = fee.installfee, recommendfee = fee.recommendfee }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Content("");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="order"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetFocus(string limit, string offset, string order, string search)
        {
            int total;
            List<tb_user> list;

            if (IsAdmin())
            {
                GetAllFocusData(limit, offset, search, out total, out list);
            }
            else
            {
                GetFocusDataByAlliance(limit, offset, search, out total, out list);
            }

            //根据数据构建关注查询的ViewModel
            var viewListModel = BuildFocusViewModel(list);

            //根据数据生成json
            var json = GetFocusJson(total, viewListModel);

            return Content(json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <param name="total"></param>
        /// <param name="list"></param>
        [NonAction]
        private void GetAllFocusData(string limit, string offset, string search, out int total, out List<tb_user> list)
        {
            if (string.IsNullOrEmpty(search))
            {
                list = FocusService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.id > 0, a => a.addtime,
               a => new tb_user
               {
                   name = a.name,
                   addtime = a.addtime,
                   location = a.location
               }, out total, false).ToList();
            }
            else
            {
                list = FocusService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.name.Contains(search), a => a.addtime,
              a => new tb_user
              {
                  name = a.name,
                  addtime = a.addtime,
                  location = a.location
              }, out total, false).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="search"></param>
        /// <param name="total"></param>
        /// <param name="list"></param>
        [NonAction]
        private void GetFocusDataByAlliance(string limit, string offset, string search, out int total, out List<tb_user> list)
        {
            var allianceId = GetAllianceIdByCurrentUser();

            if (string.IsNullOrEmpty(search))
            {
                list = FocusService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.location.Equals(allianceId), a => a.addtime,
               a => new tb_user
               {
                   name = a.name,
                   addtime = a.addtime,
                   location = a.location
               }, out total, false).ToList();
            }
            else
            {
                list = FocusService.GetListByPage(int.Parse(offset), int.Parse(limit), a => a.name.Contains(search) && a.location.Equals(allianceId), a => a.addtime,
              a => new tb_user
              {
                  name = a.name,
                  addtime = a.addtime,
                  location = a.location
              }, out total, false).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        [NonAction]
        private List<FocusViewModel> BuildFocusViewModel(List<tb_user> list)
        {
            var viewListModel = new List<FocusViewModel>();
            list.ForEach((a) =>
            {
                viewListModel.Add(new FocusViewModel
                {
                    name = a.name,
                    focustime = ((DateTime)a.addtime).ToString("yyyy-MM-dd hh:mm:ss"),
                    alliance = AllianceService.GetModal(b => b.twocodeid.ToString().Equals(a.location)).c_name
                });
            });
            return viewListModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [NonAction]
        private static string GetJson(int total, List<shopform> list)
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
        /// 
        /// </summary>
        /// <param name="total"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        [NonAction]
        private static string GetFocusJson(int total, List<FocusViewModel> list)
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
    }
}
