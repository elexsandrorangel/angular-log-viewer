using AutoMapper;
using LogViewer.Business.Implementations;
using LogViewer.Business.Interfaces;
using LogViewer.Business.Mappers;
using LogViewer.Infrastructure;
using LogViewer.Middleware;
using LogViewer.Repository.Contexts;
using LogViewer.Repository.Implementations;
using LogViewer.Repository.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LogViewer
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
            // https://stackoverflow.com/questions/57626878/the-json-value-could-not-be-converted-to-system-int32
            services.AddControllers().AddNewtonsoftJson();
            
            services.AddDbContext<LogViewerContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("LogViewerDbConnection"), pgsql =>
                {
                    pgsql.MigrationsHistoryTable(tableName: "__migration_history", schema: Constants.DefaultSchema);
                });
            });

            //// https://stackoverflow.com/questions/59199593/net-core-3-0-possible-object-cycle-was-detected-which-is-not-supported
            //services.AddControllersWithViews().AddNewtonsoftJson(options =>
            //    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Log Viewer API", Version = "v1" });
            });

            // Automapper profiles: 
            services.AddAutoMapper(typeof(MappingProfiles));

            #region IoC
            services.AddTransient(typeof(IAccessLogBusiness), typeof(AccessLogBusiness));
            services.AddTransient(typeof(IAccessLogRepository), typeof(AccessLogRepository));
            #endregion

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();


            services.AddCors(option =>
            {
                option.AddPolicy("CorsDefaultPolicy", p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyMethod();
                    p.AllowAnyHeader();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Log Viewer API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<LogViewerExceptionMiddleware>();
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();


            app.UseCors("CorsDefaultPolicy");


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
