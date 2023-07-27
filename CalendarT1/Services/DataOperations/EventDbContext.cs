using CalendarT1.Models.EventModels;
using Microsoft.EntityFrameworkCore;

namespace CalendarT1.Services.DataOperations
{
    public class EventDbContext : DbContext
	{
		public DbSet<IGeneralEventModel> Events { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=testerDB;Trusted_Connection=True;");
		}
	}
}
