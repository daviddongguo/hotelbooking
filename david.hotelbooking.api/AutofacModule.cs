using Autofac;
using david.hotelbooking.domain.Abstract;
using david.hotelbooking.domain.Concretes;
using david.hotelbooking.domain.Services;
using Microsoft.Extensions.Logging;

namespace david.hotelbooking.api
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // The generic ILogger<TCategoryName> service was added to the ServiceCollection by ASP.NET Core.
            // It was then registered with Autofac using the Populate method. All of this starts
            // with the services.AddAutofac() that happens in Program and registers Autofac
            // as the service provider.
            builder.RegisterType<UserService>()
                .As<IUserService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>()
                .As<IUserRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<BookingService>()
                .As<IBookingService>()
                .InstancePerLifetimeScope();
        }
    }
}
