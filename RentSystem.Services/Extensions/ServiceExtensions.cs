﻿using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentSystem.Core.Contracts.Service;
using RentSystem.Core.DTOs;
using RentSystem.Services.Services;
using RentSystem.Services.Validations;

namespace RentSystem.Services.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IAdvertService, AdvertService>();

            services.AddScoped<IValidator<AdvertDTO>, AdvertValidator>();
            services.AddScoped<IValidator<ItemDTO>, ItemValidator>();

            services.AddAutoMapper(typeof(MappingProfiles.ItemMappingProfile));

            return services;
        }
    }
}
