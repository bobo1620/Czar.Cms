using Consul;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

//服务消费者
namespace XFZConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var consulClient = new ConsulClient(s => { s.Address = new Uri("http://127.0.0.1:8500"); })) 
            {

                var services = consulClient.Agent.Services().Result.Response.Values
                    .Where(c =>c.Service.Equals("MsgService",StringComparison.Ordinal));
                //遍历所有服务的信息
                /*
                foreach (var s in services.Values)
                {
                    Console.WriteLine($"id={ s.ID },port={s.Port},name={s.Service},addr={s.Address}");
                }
                */
                //客户端负载均衡
                Random random = new Random();
                int index = random.Next(services.Count());//[0,services.Coun)
                var sers = services.ElementAt(index);  //获取服务

                //请求服务
                using (HttpClient hclient = new HttpClient())
                using (var httpcontent = new StringContent("{\"phoneNum\":\"111\",\"msg\":\"aaa\"}", Encoding.UTF8, "application/json"))
                {
                    var surl = $"http://{sers.Address}:{sers.Port}/api/Sms/Send_LX";
                    var result = hclient.PostAsync($"http://{sers.Address}:{sers.Port}/api/Sms/Send_LX", httpcontent).Result;
                    Console.WriteLine("返回结果："+result.StatusCode+ surl);//状态码
                }

            }

                Console.WriteLine("Hello World!");
        }
    }
}
