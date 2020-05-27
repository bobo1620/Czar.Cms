using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        [HttpPost(nameof(Send_LX))] //nameof(Send_LX))得到字符串的Send_LX
        public void Send_LX(SendMsgRequest model) 
        {
            Console.WriteLine($"短信号码{model.phoneNum}短信内容{model.msg}");
        }

        public class SendMsgRequest 
        {
            public string phoneNum { get; set; }
            public string msg { get; set; }
        }
    }
}