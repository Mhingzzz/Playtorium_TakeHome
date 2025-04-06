using Application.ContractRepo;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Infrastructure
{

    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDiscountCampaignRepository, DiscountCampaignRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            var DataConnectionString = configuration.GetConnectionString("ptr_db");
            services.AddDbContext<DataContext>(option =>
            {
                option.UseNpgsql(DataConnectionString);
            });

            return services;
        }
    }
}

