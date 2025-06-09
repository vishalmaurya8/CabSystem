namespace CabSystem.Repositories
{
    public interface IRideFareService
    {
        decimal CalculateFare(string pickupLocation, string dropoffLocation);
    }
}
