using CW_10_s27864.Data;
using CW_10_s27864.Models;


namespace CW_10_s27864.Services;

public interface IDbService
{
   public Task<ICollection<Trip>> GetTripsAsync();
}
class DbService(AppDbContext dbContext) : IDbService
{
   public async Task<ICollection<Trip>> GetTripsAsync()
   {

      return null;
   }
}
