using CalendarT1.Models.EventTypesModels;
using Newtonsoft.Json;

namespace CalendarT1.Models.EventModels
{
    public abstract class AbstractEventModel : IGeneralEventModel
    {
        private TimeSpan _defaulteventremindertime = TimeSpan.FromHours(24);
		private const int _alphaColorDivisor= 20;
		public Guid Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool WasShown { get; set; }
        public virtual bool IsCompleted { get; set; }
        public ISubEventTypeModel EventType { get; set; }
        public List<DateTime> PostponeHistory { get; set; }
        public TimeSpan DefaultPostponeTime { get; set; }
        public TimeSpan ReminderTime { get; set; }
        public QuantityModel QuantityAmount { get; set; }
		public List<MicroTaskModel> MicroTasksList { get; set; }

		[JsonIgnore]
        public Color EventVisibleColor
        {
            get
            {
                Color color = EventType.EventTypeColor;

                // Apply the completed color adjustment if necessary
                if (IsCompleted)
                {
                    color = IsCompleteColorAdapt(color);
                }
                return color;
            }
        }

		// TO Consider postpone time and maybe some other extra options for advanced event adding mode??
		public AbstractEventModel(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel eventType, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false, QuantityModel quantityAmount = null, List<MicroTaskModel> multiTasksList = null)
		{
			//... rest of the code

			if (postponeTime.HasValue)
				ReminderTime = postponeTime.Value;
			else
				ReminderTime = _defaulteventremindertime;
			Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StartDateTime = startTime;
            EndDateTime = endTime;
            EventType = eventType;
            IsCompleted = isCompleted;
            WasShown = wasShown;
			QuantityAmount = quantityAmount;
			MicroTasksList = multiTasksList;
			PostponeHistory = new List<DateTime>(); // default new list 
        }
        private Color IsCompleteColorAdapt(Color color)
        {
            return Color.FromRgba(color.Red, color.Green, color.Blue, color.Alpha / _alphaColorDivisor);
        }

    }
}
