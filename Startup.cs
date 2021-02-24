using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCore.ApiPhones.Interfaces;
using WebCore.ApiPhones.Services;
using System.Reflection;
using System.IO;

namespace WebCore.ApiPhones
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
            services.AddControllers();

            services.AddSingleton<ConnectionFactory>(provider =>
                new ConnectionFactory(Configuration["Data:OracleConnectionString"]));

            services.AddSingleton<OracleService>();

            // �������� � ����������
            services.AddSingleton<IPhoneBook, OracleService>();

            // �������� � ����������
            services.AddSingleton<IUnit, OracleService>();

            // �������� � ����������
            services.AddSingleton<IPhoto, OracleService>();

            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "API ����������� �����������",
                        Version = "v1",
                        Description =
                        "����������� ������ � ���������� ������������",
                        //                        TermsOfService = new Uri("https://api.rb.asu.ru/tos"),
                    });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });

            // CORS � �����-�������� �������
            //services.AddCors(); // ��������� ������� CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "PhoneBook API v1");
            });

            app.UseCors("CorsPolicy");
            //app.UseCors(builder => builder.AllowAnyOrigin());
        }
    }
}
