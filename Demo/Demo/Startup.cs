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
            services.AddSession();// ����session ��ʾ����ô��

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.Extensions.Hosting.IHostApplicationLifetime appLifttime)
        {
            app.UseSession(); //ʹ��session

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

            #region ����ע��
            string ip = Configuration["ip"];
            string port = Configuration["port"];

            string serviceName = "MsgService";
            string serviceid = serviceName + Guid.NewGuid();//Ψһ
            using (var conSulClient = new ConsulClient(ConsulConfig))
            {
                //ServiceRegister �첽���� ��Ҫ�ȴ� ִ������ͷ� 

                AgentServiceRegistration service = new AgentServiceRegistration();
                service.Address = ip;   //��ʽ����Ϊ�����ip
                service.Port =Convert.ToInt32( port);
                service.ID = serviceid;
                service.Name = serviceName;

                service.Check = new AgentServiceCheck() {
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
                    HTTP = $"http://{ip}:{port}/api/Health",
                    Interval = TimeSpan.FromSeconds(10), //�������
                    Timeout = TimeSpan.FromSeconds(5)   //��ʱ
                };
                conSulClient.Agent.ServiceRegister(service).Wait();

                //Ӧ���˳�ʱ  ע��
                appLifttime.ApplicationStopped.Register(() => {
                    using (var conSulClient = new ConsulClient(ConsulConfig))
                    {
                        Console.WriteLine("Ӧ���˳���");
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
