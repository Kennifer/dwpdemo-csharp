namespace DWP.Demo.Api.Types
{
    public record User
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Email { get; init; }
        public string IpAddress { get; init; }
        public decimal Latitude { get; init; }
        public decimal Longitude { get; init; }
    }
}