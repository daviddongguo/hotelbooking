# hotelbooking

A hotel reservation application that uses React on the frontend and .net core apis on the backend

Front lives on [Azure](https://proud-stone-0f1f1d00f.azurestaticapps.net/)

frontend: <https://github.com/daviddongguo/hotelbooking.reactweb>

backend swagger lives on [Azure](https://davidwuhotelbooking.azurewebsites.net/swagger/index.html)

backend: <https://github.com/daviddongguo/hotelbooking>

## feature

- [Role-Based Access Control](https://github.com/daviddongguo/hotelbooking/tree/feature-api/david.hotelbooking.domain/Entities/RBAC)

- [Booking Services](https://github.com/daviddongguo/hotelbooking/blob/feature-api/david.hotelbooking.domain/Services/BookingService.cs) && [Tests](https://github.com/daviddongguo/hotelbooking/blob/feature-api/david.hotelbooking.UnitTests/Services/LocalDbBookingServiceTest.cs)

- [Local InMemory Dbcontext](https://github.com/daviddongguo/hotelbooking/blob/feature-api/david.hotelbooking.UnitTests/Services/LocalInMemoryDbContextFactory.cs) for testing

- [BookingsController](https://github.com/daviddongguo/hotelbooking/blob/feature-api/david.hotelbooking.api/Controllers/BookingsController.cs) && [Tests](https://github.com/daviddongguo/hotelbooking/blob/feature-api/david.hotelbooking.UnitTests/Controllers/BookingControllerTests.cs) by using **Moq**

- Use [RestSharp](https://github.com/daviddongguo/hotelbooking/blob/feature-api/david.hotelbooking.localapitests/LocalApiRemoteDbTests.cs) to test apis
