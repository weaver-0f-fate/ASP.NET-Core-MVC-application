using AutoMapper;
using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Services.ModelsDTO;
using Services.Presentations;
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

            services.AddSingleton<IService<CourseDTO>, CourseService>(
                x => new CourseService(
                    x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>(), 
                    x.GetRequiredService<IMapper>()));

            services.AddSingleton<IService<GroupDTO>, GroupService>(
                x => new GroupService(
                    x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>(), 
                    x.GetRequiredService<IMapper>()));

            services.AddSingleton<IService<StudentDTO>, StudentService>(
                x => new StudentService(
                    x.CreateScope().ServiceProvider.GetRequiredService<Task9Context>(), 
                    x.GetRequiredService<IMapper>()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Task9Context context) {

            
            context.Database.Migrate();
            

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandlerMiddleware();
                app.UseHsts();
            }

            //app.UseExceptionHandlerMiddleware();

            app.UseHttpsRedirection(); //TODO What is it

            app.UseStaticFiles(); //TODO What is it 

            app.UseRouting(); //TODO what is it

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Courses}/{action=Index}/{id?}");
            });
        }
    }
}
