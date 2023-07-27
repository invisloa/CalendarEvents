using Microsoft.EntityFrameworkCore;

namespace CalendarT1.Services.DataOperations
{
    public class EventDbContext : DbContext
	{
		public DbSet<AbstractEventModel> Events { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=testerDB;Trusted_Connection=True;");
		}
	}
}
