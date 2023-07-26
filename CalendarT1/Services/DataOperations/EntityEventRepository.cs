using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using Microsoft.EntityFrameworkCore;




namespace CalendarT1.Services.DataOperations
{
	public class EntityEventRepository : IEventRepository
	{
		private readonly EventDbContext _context;

		public EntityEventRepository(EventDbContext context)
		{
			_context = context;
		}

		public async Task<List<AbstractEventModel>> GetEventsListAsync()
		{
			return await _context.Events.ToListAsync();
		}

		public async Task SaveEventsListAsync()
		{
			await _context.SaveChangesAsync();
		}

		public async Task DeleteFromEventsListAsync(AbstractEventModel eventToDelete)
		{
			_context.Events.Remove(eventToDelete);
			await _context.SaveChangesAsync();
		}

		public async Task AddEventAsync(AbstractEventModel eventToAdd)
		{
			await _context.Events.AddAsync(eventToAdd);
			await _context.SaveChangesAsync();
		}

		public async Task ClearEventsListAsync()
		{
			_context.Events.RemoveRange(_context.Events);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateEventAsync(AbstractEventModel eventToUpdate)
		{
			_context.Events.Update(eventToUpdate);
			await _context.SaveChangesAsync();
		}
		public async Task<AbstractEventModel> GetEventByIdAsync(Guid eventId)
		{
			var selectedEvent = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
			return selectedEvent;
		}

	}
}
