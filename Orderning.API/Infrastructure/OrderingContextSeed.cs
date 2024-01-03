namespace Microsoft.eShopOnContainers.Services.Ordering.API.Infrastructure
{
    using global::Ordering.Domain.SeedWork;
    using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;
    using Microsoft.eShopOnContainers.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
    using Ordering.Infrastructure;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OrderingContextSeed
    {
        public async Task SeedAsync(OrderingContext context)
        {

            if (!context.CardTypes.Any())
            {
                context.CardTypes.AddRange(GetPredefinedCardTypes());

                await context.SaveChangesAsync();
            }

            if (!context.OrderStatus.Any())
            {
                context.OrderStatus.AddRange(GetPredefinedOrderStatus());
            }

            if (!context.Buyers.Any())
            {
                context.Buyers.AddRange(GetDummyBuyer());
            }

            await context.SaveChangesAsync();
        }

        private static IEnumerable<CardType> GetPredefinedCardTypes()
        {
            return Enumeration.GetAll<CardType>();
        }

        private static List<OrderStatus> GetPredefinedOrderStatus()
        {
            return new List<OrderStatus>()
            {
                OrderStatus.Submitted,
                OrderStatus.AwaitingValidation,
                OrderStatus.StockConfirmed,
                OrderStatus.Paid,
                OrderStatus.Shipped,
                OrderStatus.Cancelled
            };
        }

        private static Buyer GetDummyBuyer()
        {
            return new Buyer(Guid.NewGuid().ToString(), "Paco Buyo");
        }
    }
}
