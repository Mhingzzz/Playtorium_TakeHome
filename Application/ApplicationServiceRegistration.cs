using Application.Interfaces;
using Application.Service;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{

   public static class ApplicationServiceRegistration
   {
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<IDiscountCampaignService, DiscountCampaignService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IItemService, ItemService>();
        // services.AddScoped<ICartService, CartService>();
        return services;
    }

   }
}