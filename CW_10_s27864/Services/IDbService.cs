using CW_10_s27864.Data;
using CW_10_s27864.DTO_s;
using CW_10_s27864.Exceptions;
using CW_10_s27864.Models;
using Microsoft.EntityFrameworkCore;


namespace CW_10_s27864.Services;

public interface IDbService
{
   public Task<GetTripsPageDto> GetTripsAsync(int page = 1, int pageSize = 10);
   public Task RemoveClientAsync(int id);
   public Task AddClientToTripAsync(AddClientToTripDto dto);
}
class DbService(ApbdContext dbContext) : IDbService
{
   public async Task<GetTripsPageDto> GetTripsAsync(int page = 1, int pageSize = 10)
   {
      
      var totalTrips = await dbContext.Trips.CountAsync();

      
      var totalPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

      
      var trips = await dbContext.Trips
         .OrderByDescending(t => t.DateFrom)
         .Skip((page - 1) * pageSize)
         .Take(pageSize)
         .Select(tr => new GetTripsGetDTO
         {
            IdTrip = tr.IdTrip,
            Name = tr.Name,
            Description = tr.Description,
            DateFrom = tr.DateFrom,
            DateTo = tr.DateTo,
            MaxPeople = tr.MaxPeople,
            Country = tr.IdCountries.Select(c => new GetCountryDTO
            {
               Name = c.Name
            }).ToList(),
            Client = tr.ClientTrips.Select(c => new GetClientDTO
            {
               FirstName = c.IdClientNavigation.FirstName,
               LastName = c.IdClientNavigation.LastName
            }).ToList()
         })
         .ToListAsync();

      return new GetTripsPageDto
      {
         PageNum = page,
         PageSize = pageSize,
         AllPages = totalPages,
         Trips = trips
      };
   }
   public async Task RemoveClientAsync(int id)
   {
      var client=await dbContext.Clients.FirstOrDefaultAsync(c => c.IdClient == id);
      if (client is null)
      {
         throw new NotFoundException("Client not found!");
      }
      var clientHaveTrips=await dbContext.ClientTrips.FirstOrDefaultAsync(c => c.IdClient == id);
      if (clientHaveTrips is not null)
      {
         throw new Exception("Client have trips!");
      }
      dbContext.Clients.Remove(client);
      await dbContext.SaveChangesAsync();
   }

   public async Task AddClientToTripAsync(AddClientToTripDto dto)
   {
      var clientPesel = await dbContext.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
      if (clientPesel is not null)
      {
      }var alreadyAssigned = await dbContext.ClientTrips.AnyAsync(ct =>
         ct.IdClient == clientPesel.IdClient && ct.IdTrip == dto.IdTrip);
      if (alreadyAssigned)
      {
         throw new Exception("Client already assigned to this trip!");
      }
      
      var trip = await dbContext.Trips.FirstOrDefaultAsync(t => t.IdTrip == dto.IdTrip);
      if (trip is null)
      {
         throw new NotFoundException("Trip not found!");
      }
      
      if (trip.DateFrom < DateTime.Now)
      {
         throw new Exception("Trip is over!");
      }

      var transaction = await dbContext.Database.BeginTransactionAsync();
      try
      {
         
         
            var client = new Client()
            {
               FirstName = dto.FirstName,
               LastName = dto.LastName,
               Email = dto.Email,
               Telephone = dto.Telephone,
               Pesel = dto.Pesel
            };
            await dbContext.Clients.AddAsync(client);
            await dbContext.SaveChangesAsync();

            var clientTrip = new ClientTrip()
            {
               IdClient = client.IdClient,
               IdTrip = dto.IdTrip,
               RegisteredAt = DateTime.Now,
               PaymentDate = dto.PaymentDate
            };
            await dbContext.ClientTrips.AddAsync(clientTrip);
            await dbContext.SaveChangesAsync();
         

         await transaction.CommitAsync();

      }catch 
      {
         await transaction.RollbackAsync();
         throw;
      }

}
}
