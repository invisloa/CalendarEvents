using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace CalendarT1.ViewModels.EventsViewModels
{
    public class WeeklyEventsViewModel : BaseViewModel
    {
        // All other properties and methods stay the same as DailyEventsViewModel, except for BindDataToScheduleList

        public void BindDataToScheduleList()
        {
            var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();
            var startOfWeek = _currentSelectedDate.AddDays(-(int)_currentSelectedDate.DayOfWeek);
            var endOfWeek = startOfWeek.AddDays(7);

            var filteredScheduleList = _allEventsList
                .Where(x => x.StartDateTime.Date >= startOfWeek.Date &&
                             x.EndDateTime.Date < endOfWeek.Date &&
                             selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
                .ToList();

            EventsToShowList = new ObservableCollection<EventModel>(filteredScheduleList);
        }
    }
}
*/