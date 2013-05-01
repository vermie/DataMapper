using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theoretical.Business
{
    public class CalendarEvent
    {
        public DateTime EventDate { get; set; }
        public string Title { get; set; }
    }

    public class CalendarEventForm
    {
        public DateTime EventDate { get; set; }
        public int EventHour { get; set; }
        public int EventMinute { get; set; }
        public string Title { get; set; }
    }
    class ScratchPad
    {


        public void Yo()
        {
            // Model
            var calendarEvent = new CalendarEvent
            {
                EventDate = new DateTime(2008, 12, 15, 20, 30, 0),
                Title = "Company Holiday Party"
            };

            // Configure AutoMapper
            //Mapper.CreateMap<CalendarEvent, CalendarEventForm>()
            //    .ForMember(dest => dest.EventDate, opt => opt.MapFrom(src => src.EventDate.Date))
            //    .ForMember(dest => dest.EventHour, opt => opt.MapFrom(src => src.EventDate.Hour))
            //    .ForMember(dest => dest.EventMinute, opt => opt.MapFrom(src => src.EventDate.Minute));

            //// Perform mapping
            //CalendarEventForm form = Mapper.Map<CalendarEvent, CalendarEventForm>(calendarEvent);

            //form.EventDate.ShouldEqual(new DateTime(2008, 12, 15));
            //form.EventHour.ShouldEqual(20);
            //form.EventMinute.ShouldEqual(30);
            //form.Title.ShouldEqual("Company Holiday Party");

            //someType.ItsProperty
        }


    }
}
