using System;
using AutoMapper;
using Data;
using Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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

            services.AddScoped<IService<CourseDto>, CourseService>(GetCoursesService);
            services.AddScoped<IService<GroupDto>, GroupService>(GetGroupsService);
            services.AddScoped<IService<StudentDto>, StudentService>(GetStudentsService);
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

        private static CourseService GetCoursesService(IServiceProvider serviceProvider) {
            var context = serviceProvider.GetRequiredService<Task9Context>();
            var repo = new CourseRepository(context, context.Courses);
            return new CourseService(repo, serviceProvider.GetRequiredService<IMapper>());
        }

        private static GroupService GetGroupsService(IServiceProvider serviceProvider) {
            var context = serviceProvider.GetRequiredService<Task9Context>();
            var repo = new GroupRepository(context, context.Groups);
            return new GroupService(repo, serviceProvider.GetRequiredService<IMapper>());
        }
        private static StudentService GetStudentsService(IServiceProvider serviceProvider) {
            var context = serviceProvider.GetRequiredService<Task9Context>();
            var repo = new StudentRepository(context, context.Students);
            return new StudentService(repo, serviceProvider.GetRequiredService<IMapper>());
        }
    }
}
