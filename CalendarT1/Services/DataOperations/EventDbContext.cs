using CalendarT1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.DataOperations
{
	public class EventDbContext : DbContext
	{
		public DbSet<EventModel> Events { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=testerDB;Trusted_Connection=True;");
		}
	}
}
