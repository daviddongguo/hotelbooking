using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Concretes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace david.hotelbooking.api
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
            // services.AddDbContext<EFDbContext>( x => x.UseSqlite(Configuration.GetConnectionString("SqliteConnection"),
            //     b => b.MigrationsAssembly("david.hotelbooking.api")));
            services.AddDbContext<EFDbContext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
               b => b.MigrationsAssembly("david.hotelbooking.api")));
            services.AddControllers();
            services.AddScoped<IUserRepository, EFUserRepository>();
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
        }
    }
}
