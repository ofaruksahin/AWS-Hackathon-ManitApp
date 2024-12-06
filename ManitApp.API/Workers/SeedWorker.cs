
using ManitApp.API.Domain.Entities;
using ManitApp.API.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ManitApp.API.Workers
{
    public class SeedWorker : BackgroundService
    {
        private IServiceProvider _serviceProvider;

        public SeedWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ManitAppDbContext>();
            var orders = context.Order.ToList();

            foreach (var order in orders)
            {
                var orderVector = new OrderVector
                {
                    OrderId = order.Id,
                    Vector = VectorizeOrder(order)
                };

                context.OrderVector.Add(orderVector);
            }

            await context.SaveChangesAsync();
        }

        private Pgvector.Vector VectorizeOrder(Order order)
        {
            var genderEncoding = order.Gender ? 1.0f : 0.0f;
            var originEncoding = order.Origin.GetHashCode();
            var destinationEncoding = order.Destination.GetHashCode();

            var vector = new float[]
            {
                genderEncoding,
                originEncoding,
                destinationEncoding
            };

            return new Pgvector.Vector(vector);
        }
    }
}
