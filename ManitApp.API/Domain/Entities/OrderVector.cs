namespace ManitApp.API.Domain.Entities
{
    public class OrderVector
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public double[] Vector { get; set; } = Array.Empty<double>();
    }
}
