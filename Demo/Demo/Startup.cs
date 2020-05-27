using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();// 配置session 表示该怎么用

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IHostApplicationLifetime appLifttime)
        {
            app.UseSession(); //使用session

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            #region 服务注册
            string ip = Configuration["ip"];
            string port = Configuration["port"];

            string serviceName = "MsgService";
            string serviceid = serviceName + Guid.NewGuid();//唯一
            using (var conSulClient = new ConsulClient(ConsulConfig))
            {
                //ServiceRegister 异步方法 需要等待 执行完才释放 

                AgentServiceRegistration service = new AgentServiceRegistration();
                service.Address = ip;   //正式环境为服务的ip
                service.Port =Convert.ToInt32( port);
                service.ID = serviceid;
                service.Name = serviceName;

                service.Check = new AgentServiceCheck() {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{ip}:{port}/api/Health",
                    Interval = TimeSpan.FromSeconds(10), //心跳检查
                    Timeout = TimeSpan.FromSeconds(5)   //超时
                };
                conSulClient.Agent.ServiceRegister(service).Wait();

                //应用退出时  注销
                appLifttime.ApplicationStopped.Register(() => {
                    using (var conSulClient = new ConsulClient(ConsulConfig))
                    {
                        Console.WriteLine("应用退出！");
                        conSulClient.Agent.ServiceDeregister(serviceid).Wait();
                    }

                });
            }
            #endregion

        }

        private void ConsulConfig(ConsulClientConfiguration c)
        {
            c.Address = new Uri("http://127.0.0.1:8500");
            c.Datacenter = "dc1";
        }
    }
}
