using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Models;

namespace Demo.Controllers
{
    public class FirstController : Controller
    {
        private readonly ILogger<FirstController> _logger;
        public FirstController(ILogger<FirstController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            this._logger.LogWarning("12345667789");

            #region ViewData
            base.ViewData["User1"] = new CurrentUser()
            {
                Id = 7,
                Name = "Y",
                Account = " ╰つ Ｈ ♥. 花心胡萝卜",
                Email = "莲花未开时",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            };
            base.ViewData["Something"] = 12345;
            #endregion

            #region ViewBag
            base.ViewBag.Name = "Eleven";
            base.ViewBag.Description = "Teacher";
            base.ViewBag.User = new CurrentUser()
            {
                Id = 7,
                Name = "IOC",
                Account = "限量版",
                Email = "莲花未开时",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            };
            #endregion

            #region TempData
            base.TempData["User"] = new CurrentUser()
            {
                Id = 7,
                Name = "CSS",
                Account = "季雨林",
                Email = "KOKE",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            };//3.0preview默认不能复杂类型
            #endregion

            #region Session：服务器内存的一段内容，在HttpContext
            if (string.IsNullOrWhiteSpace(this.HttpContext.Session.GetString("CurrentUserSession")))
            {
                //SessionExtensions.SetString(this.HttpContext.Session, "","");

                //好像给这个实例增加了一个实例方法，就像扩展了这个类---所以我们能在抽象设计时，最小化--后续的便捷功能可以通过扩展方法来提供
                base.HttpContext.Session.SetString("CurrentUserSession", Newtonsoft.Json.JsonConvert.SerializeObject(new CurrentUser()
                {
                    Id = 7,
                    Name = "CSS",
                    Account = "季雨林",
                    Email = "KOKE",
                    Password = "落单的候鸟",
                    LoginTime = DateTime.Now
                }));
            }
            #endregion

            #region Model
            return View(new CurrentUser()
            {
                Id = 7,
                Name = "一点半",
                Account = "季雨林",
                Email = "KOKE",
                Password = "落单的候鸟",
                LoginTime = DateTime.Now
            });
            #endregion
            
        }
    }
}