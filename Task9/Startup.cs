using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Services.Services;
using Task9.Exceptions;
using Interfaces;
using Core.Models;
using Services;
using Services.ModelsDTO;

namespace Task9 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {


            services.AddAutoMapper(typeof(Startup));


            services.AddControllersWithViews();

            services.AddDbContext<Task9Context>(
                options =>
                 options.UseSqlServer(
                     Configuration.GetConnectionString("Task9Context"),
                     x => x.MigrationsAssembly("Data")));

            services.AddTransient <IRepository<Course>, CourseRepository>();
            services.AddTransient<IRepository<Group>, GroupRepository>();
            services.AddTransient<IRepository<Student>, StudentRepository>();

            services.AddTransient<IService<Course, CourseDto>, CourseService>();
            services.AddTransient<IService<Group, GroupDto>, GroupService>();
            services.AddTransient<IService<Student, StudentDto>, StudentService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Task9Context context) {
            
            env.EnvironmentName = "Production";
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandlerMiddleware();
                app.UseHsts();
            }

            context.Database.Migrate();

            

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Courses}/{action=Index}/{id?}");
            });
        }
    }
}
