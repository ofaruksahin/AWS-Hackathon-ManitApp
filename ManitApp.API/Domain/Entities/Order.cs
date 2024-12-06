namespace ManitApp.API.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public TimeSpan Time { get; set; }
    }
}
