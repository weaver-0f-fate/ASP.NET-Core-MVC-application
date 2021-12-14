using AutoMapper;
using Core.Models;
using Data;
using Data.Repositories;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Services;
using Services.ModelsDTO;
using Services.Services;
using Task9.Exceptions;


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

            services.AddScoped<IRepository<Course>, CourseRepository>(
                x => new CourseRepository(
                    x.GetRequiredService<Task9Context>()));

            services.AddScoped<IRepository<Group>, GroupRepository>(
                x => new GroupRepository(
                    x.GetRequiredService<Task9Context>()));

            services.AddScoped<IRepository<Student>, StudentRepository>(
                x => new StudentRepository(
                    x.GetRequiredService<Task9Context>()));


            services.AddScoped<IService<CourseDto>, CourseService>(
                x => new CourseService(
                x.GetRequiredService<IRepository<Course>>(), 
                   x.GetRequiredService<IMapper>()));

            services.AddScoped<IService<GroupDto>, GroupService>(
                x => new GroupService(
                    x.GetRequiredService<IRepository<Group>>(),
                    x.GetRequiredService<IMapper>()));

            services.AddScoped<IService<StudentDto>, StudentService>(
                x => new StudentService(
                    x.GetRequiredService<IRepository<Student>>(),
                    x.GetRequiredService<IMapper>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Task9Context context) {

            
            context.Database.Migrate();

            //env.EnvironmentName = "Production";

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandlerMiddleware();
                app.UseHsts();
            }

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
