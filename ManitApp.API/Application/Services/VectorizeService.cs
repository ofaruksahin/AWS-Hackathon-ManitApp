using ManitApp.API.Application.RequestModels;
using ManitApp.API.Application.ResponseModels;
using ManitApp.API.Application.Services.Contracts;
using ManitApp.API.Domain.Entities;
using ManitApp.API.Infrastructure;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.EntityFrameworkCore;

namespace ManitApp.API.Application.Services
{
    public class VectorizeService : IVectorizeService
    {
        private readonly ManitAppDbContext _context;

        public VectorizeService(ManitAppDbContext context)
        {
            _context = context;
        }

        public async Task VectorizeOrderForUser(int userId)
        {
            var orders = _context.Order.Where(o => o.UserId == userId).ToList();

            foreach (var order in orders)
            {
                var orderVector = new OrderVector
                {
                    OrderId = order.Id,
                    Vector = VectorizeOrder(order)
                };

                _context.OrderVector.Add(orderVector);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<SuggestionResponseModel>> GetSuggestions(SuggestionRequestModel requestModel)
        {
            var requestVector = VectorizeRequest(requestModel);

            var orderVectors = _context.OrderVector.ToList();

            var closestVectors = orderVectors
                .Select(ov => new
                {
                    OrderId = ov.OrderId,
                    Distance = CalculateEuclideanDistance(requestVector, ov.Vector)
                })
                .OrderBy(x => x.Distance) 
                .Take(5)
                .Select(ov => ov.OrderId)
                .ToList();

            var orders = await _context.Order.Where(o => closestVectors.Any(cv => o.Id == cv)).ToListAsync();

            return orders.Select(x => new SuggestionResponseModel
            {
                Name = x.Name,
                Gender = x.Gender,
                Origin = x.Origin,
                Destination = x.Destination,
            }).ToList();
        }

        private double[] VectorizeOrder(Order order)
        {
            var genderEncoding = order.Gender ? 1.0 : 0.0;
            var originEncoding = order.Origin.GetHashCode();
            var destinationEncoding = order.Destination.GetHashCode();
            var timeEncoding = order.Time.ToString().GetHashCode();

            var vector = new double[]
            {
                order.UserId,
                genderEncoding,
                originEncoding,
                destinationEncoding,
                timeEncoding
            };

            return vector;
        }

        private double[] VectorizeRequest(SuggestionRequestModel requestModel)
        {
            var genderEncoding = requestModel.Gender ? 1.0 : 0.0;
            var originEncoding = requestModel.Origin.GetHashCode();
            var destinationEncoding = requestModel.Destination.GetHashCode();

            return new double[]
            {
                genderEncoding,
                originEncoding,
                destinationEncoding
            };
        }

        private static double CalculateEuclideanDistance(double[] vector1, double[] vector2)
        {
            var v1 = Vector<double>.Build.DenseOfArray(vector1);
            var v2 = Vector<double>.Build.DenseOfArray(vector2);
            return (v1 - v2).L2Norm(); // L2 normu: Euclidean mesafesi
        }
    }
}
