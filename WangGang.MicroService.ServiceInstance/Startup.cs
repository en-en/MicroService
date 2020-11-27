using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Server1;
using WangGang.MicroService.IService;
using WangGang.MicroService.Service;
using WangGang.MicroService.ServiceInstance.Controllers;

namespace WangGang.MicroService.ServiceInstance
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddControllers();
            services.AddTransient<IUserService, UserService>();

            services.AddAuthentication("Bearer")

             .AddIdentityServerAuthentication(options =>
             {
                 options.Authority = "http://localhost:5001";//identifyServer�����ַ
                 options.RequireHttpsMetadata = false;//�Ƿ�ʹ��https
                 options.ApiName = "api1";//���������֤��API��Դ������
             });

            //services.AddAuthentication("Bearer")
            //       .AddJwtBearer("Bearer",options =>
            //       {
            //           options.Authority = "http://localhost:5001";//identifyServer�����ַ
            //            options.RequireHttpsMetadata = false;//�Ƿ�ʹ��https
            //            options.Audience = "serviceA";//���������֤��API��Դ������
            //        });

            //ע��swagger����
            services.AddSwaggerGen((s) =>
            {
                //Ψһ��ʶ�ĵ���URI�Ѻ�����
                s.SwaggerDoc("swaggerName", new OpenApiInfo()
                {
                    Title = "swagger�������ò���",//���������ı��⡣
                    Version = "5.3.1",//������汾��(����ֱ��д����Swashbuckle.AspNetCore���İ汾��,(��д v1 ��))
                    Description = "�û�������Ϣ",//��Ӧ�ó���ļ��������
                    Contact = new OpenApiContact()//����API����ϵ��Ϣ
                    {
                        Email = "123456789@qq.com",
                        Name = "����",
                        Extensions = null,
                        Url = null
                    },
                    License = new OpenApiLicense()//����API�������Ϣ
                    {
                        Name = "����",
                        Extensions = null,
                        Url = null
                    }
                });

                //�������ע�� 
                //ƴ�����ɵ�XML�ļ�·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                //HomeControllerΪ��ǰ�����µ�һ���ࣨ���Զ���һ����ǰӦ�ó����µ�һ���ࣩ[���ڻ�ȡ��������]
                var commentsFileName = typeof(HealthController).Assembly.GetName().Name + ".xml";
                var xmlPath = Path.Combine(basePath, commentsFileName);
                s.IncludeXmlComments(xmlPath);
                s.DocInclusionPredicate((docName, description) => true);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,IHostApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            //app.UseAuthentication();
            //app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI((s) =>
            {
                //ע�⣺/swagger/Ψһ��ʶ�ĵ���URI�Ѻ�����/swagger.josn   
                s.SwaggerEndpoint("/swagger/swaggerName/swagger.json", "UserService");

            });
            ServiceEntity serviceEntity = new ServiceEntity
            {
                IP = Configuration["Service:IP"],
                Port = Convert.ToInt32(Configuration["Service:Port"]),
                ServiceName = Configuration["Service:Name"],
                ConsulIP = Configuration["Consul:IP"],
                ConsulPort = Convert.ToInt32(Configuration["Consul:Port"])

            };
            Console.WriteLine($"consul��ʼע��{JsonConvert.SerializeObject(serviceEntity)}");
            app.RegisterConsul(lifetime, serviceEntity);
        }
    }
}
