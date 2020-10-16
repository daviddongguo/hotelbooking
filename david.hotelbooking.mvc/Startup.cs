using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace david.hotelbooking.mvc
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
            //services.AddDbContext<EFDbContext>(x => x.UseMySql(Configuration.GetConnectionString("SqliteConnection"),
            //   b => b.MigrationsAssembly("david.hotelbooking.api")));
            services.AddDbContext<UserDbContext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                b => b.MigrationsAssembly("david.hotelbooking.mvc")));
            services.AddDbContext<BookingDbcontext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                b => b.MigrationsAssembly("david.hotelbooking.mvc")));
            services.AddScoped<IUserService, UserService>();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
        }
    }
}
