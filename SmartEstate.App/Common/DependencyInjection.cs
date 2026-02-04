using Microsoft.Extensions.DependencyInjection;
using SmartEstate.App.Features.Auth;
using SmartEstate.App.Features.BrokerTakeover;
using SmartEstate.App.Features.Favorites;
using SmartEstate.App.Features.Listings;
using SmartEstate.App.Features.Messages;
using SmartEstate.App.Features.Search;

namespace SmartEstate.App;

public static class DependencyInjection
{
    public static IServiceCollection AddSmartEstateApp(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<ListingService>();
        services.AddScoped<FavoritesService>();
        services.AddScoped<SearchService>();
        services.AddScoped<MessageService>();
        services.AddScoped<TakeoverService>();
        return services;
    }
}
