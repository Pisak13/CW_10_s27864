
using CW_10_s27864.Models;
using Microsoft.EntityFrameworkCore;

namespace CW_10_s27864.Data;

public class AppDbContext: DbContext
{
   
   public AppDbContext(DbContextOptions options) : base(options)
   {
      
   }
}