using System;
using AutoMapper;
using Core.Models;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Services.ModelsDTO;
using Services.Services;
using ServicesInterfaces;
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

            AddCustomServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Task9Context context) {

            
            context.Database.Migrate();
            

            //if (env.IsDevelopment()) {
            //    app.UseDeveloperExceptionPage();
            //}
            //else {
            //    app.UseExceptionHandlerMiddleware();
            //    app.UseHsts();
            //}
            app.UseExceptionHandlerMiddleware();
            app.UseHsts();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Courses}/{action=Index}/{id?}");
            });
        }

        private static void AddCustomServices(IServiceCollection services) {
            services.AddScoped<IService<CourseDTO>, CourseService>(
                x => new CourseService(
                    new CourseRepository(
                        x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>(),
                        x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>().Courses),
                    x.GetRequiredService<IMapper>()));

            services.AddScoped<IService<GroupDTO>, GroupService>(
                x => new GroupService(
                    new GroupRepository(
                        x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>(),
                        x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>().Groups),
                    x.GetRequiredService<IMapper>()));

            services.AddScoped<IService<StudentDTO>, StudentService>(
                x => new StudentService(
                    new StudentRepository(
                        x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>(),
                        x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>().Students),
                    x.GetRequiredService<IMapper>()));
        }
    }
}
