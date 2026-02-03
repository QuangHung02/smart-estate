using SmartEstate.App.Common.Abstractions;
using SmartEstate.Domain.Entities;
using SmartEstate.Domain.Enums;
using SmartEstate.Infrastructure.Persistence;
using SmartEstate.Shared.Time;


namespace SmartEstate.Api.Data;

public static class SeedData
{
    public static async Task EnsureSeedDataAsync(this Microsoft.AspNetCore.Builder.WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var logger = services.GetService<ILoggerFactory>()?.CreateLogger("SeedData");
        try
        {
            var ctx = services.GetRequiredService<SmartEstateDbContext>();
            var hasher = services.GetRequiredService<IPasswordHasher>();
            var clock = services.GetRequiredService<IClock>();

            // if there are already users, assume seeded
            if (ctx.Users.Any())
            {
                logger?.LogInformation("Database already contains data - skipping seeding.");
                return;
            }

            logger?.LogInformation("Seeding initial data...");

            // Users
            var admin = User.Create("admin@local", "Admin", UserRole.Admin);
            admin.SetPasswordHash(hasher.Hash("Admin123!"));

            var seller = User.Create("seller@local", "Seller One", UserRole.Seller);
            seller.SetPasswordHash(hasher.Hash("Seller123!"));

            var broker = User.Create("broker@local", "Broker Joe", UserRole.Broker);
            broker.SetPasswordHash(hasher.Hash("Broker123!"));

            ctx.Users.AddRange(admin, seller, broker);
            await ctx.SaveChangesAsync();

            // Broker profile
            var bp = new BrokerProfile
            {
                UserId = broker.Id,
                CompanyName = "Acme Realty",
                LicenseNo = "LIC-001",
                Bio = "Experienced broker serving local market.",
                RatingAvg = 4.7m,
                RatingCount = 42
            };
            ctx.BrokerProfiles.Add(bp);
            await ctx.SaveChangesAsync();

            // Listing
            var listingUpdate = new ListingUpdate(
                Title: "Cozy 2BR apartment near city center",
                Description: "Bright, newly renovated 2-bedroom apartment. Close to transport and shops.",
                PropertyType: PropertyType.Apartment,
                PriceAmount: 1200000000m,
                PriceCurrency: "VND",
                AreaM2: 65.0,
                Bedrooms: 2,
                Bathrooms: 2,
                FullAddress: "123 Nguyen Trai, Ward 5, District 3",
                City: "Hanoi",
                District: "District 3",
                Ward: "Ward 5",
                Street: "Nguyen Trai",
                Lat: 21.0285,
                Lng: 105.8542,
                VirtualTourUrl: null
            );

            var listing = Listing.Create(seller.Id, listingUpdate);
            ctx.Listings.Add(listing);
            await ctx.SaveChangesAsync();

            // Images
            ctx.ListingImages.Add(new ListingImage
            {
                ListingId = listing.Id,
                Url = "/uploads/sample1.jpg",
                SortOrder = 1,
                Caption = "Living room"
            });
            ctx.ListingImages.Add(new ListingImage
            {
                ListingId = listing.Id,
                Url = "/uploads/sample2.jpg",
                SortOrder = 2,
                Caption = "Kitchen"
            });
            await ctx.SaveChangesAsync();

            // Takeover request + payment (sample flow)
            var takeover = TakeoverRequest.Create(
                listingId: listing.Id,
                sellerUserId: seller.Id,
                brokerUserId: broker.Id,
                payer: TakeoverPayer.Seller,
                feeAmount: 5000000m,
                feeCurrency: "VND",
                note: "Please handle viewings and negotiations."
            );

            ctx.TakeoverRequests.Add(takeover);
            await ctx.SaveChangesAsync();

            // Accept + create payment + complete
            takeover.Accept(clock.UtcNow);

            var payment = Payment.CreateTakeoverFee(
                payerUserId: takeover.GetPayerUserId(),
                listingId: listing.Id,
                takeoverRequestId: takeover.Id,
                amount: 5000000m,
                currency: "VND",
                provider: "dummy",
                providerRef: null,
                payUrl: null
            );

            ctx.Payments.Add(payment);
            await ctx.SaveChangesAsync();

            takeover.AttachPayment(payment.Id);
            takeover.Complete(clock.UtcNow);

            payment.MarkPaid();

            await ctx.SaveChangesAsync();

            logger?.LogInformation("Seeding completed.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}