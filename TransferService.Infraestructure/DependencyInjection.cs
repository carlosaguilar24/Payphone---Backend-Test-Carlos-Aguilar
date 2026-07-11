using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;
using TransferService.Infraestructure.Persistence;

namespace TransferService.Infraestructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TransferDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("PayphoneConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure()));

            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IMovementRepository, MovementRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
