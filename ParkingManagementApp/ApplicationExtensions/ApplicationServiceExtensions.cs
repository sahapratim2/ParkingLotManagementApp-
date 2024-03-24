using ParkingManagementApp.Core.Interfaces;
using ParkingManagementApp.Core.Respositories;
using ParkingManagementApp.Data;

namespace ParkingManagementApp.ApplicationExtensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IDatabaseManager, DatabaseManager>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}