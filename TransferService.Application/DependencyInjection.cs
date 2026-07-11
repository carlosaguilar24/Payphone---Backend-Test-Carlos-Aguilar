using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Application.Interfaces;
using TransferService.Application.Wallets;
using TransferService.Application.Wallets.Validators;
using FluentValidation;

namespace TransferService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IWalletService, WalletService>();
            services.AddValidatorsFromAssemblyContaining<CreateWalletRequestValidator>();
            return services;
        }
    }
}
