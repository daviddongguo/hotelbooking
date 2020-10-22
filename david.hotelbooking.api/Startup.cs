using Autofac;
using david.hotelbooking.domain.Concretes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace david.hotelbooking.api
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Add any Autofac modules or registrations.
            // This is called AFTER ConfigureServices so things you
            // register here OVERRIDE things registered in ConfigureServices.
            //
            // You must have the call to `UseServiceProviderFactory(new AutofacServiceProviderFactory())`
            // when building the host or this won't be called.
            builder.RegisterModule(new AutofacModule());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:3000",
                                                          "https://proud-stone-0f1f1d00f.azurestaticapps.net");
                                  });
            });
            services.AddDbContext<UserDbContext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
               b => b.MigrationsAssembly("david.hotelbooking.api")));
            services.AddDbContext<BookingDbContext>(x => x.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
               b => b.MigrationsAssembly("david.hotelbooking.api")));

            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true);
            //services.AddControllers()
            //    .AddNewtonsoftJson(options =>
            //    {
            //        options.SerializerSettings.ReferenceLoopHandling
            //            = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //    });
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IUserService, UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/echo",
                    context => context.Response.WriteAsync("echo"))
                    .RequireCors(MyAllowSpecificOrigins);

                endpoints.MapControllers().RequireCors(MyAllowSpecificOrigins);
            });
        }
    }
}
