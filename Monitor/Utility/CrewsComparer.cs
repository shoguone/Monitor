using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Monitor.Utility
{
    public class CrewsComparer : IComparer<MonitorCrew>
    {
        public int Compare(MonitorCrew item1, MonitorCrew item2)
        {
            if (item1.DepartureDate == item2.DepartureDate)
            {
                return string.Compare(item1.CrewNumber, item2.CrewNumber);
            }
            else if (item1.DepartureDate.HasValue && item2.DepartureDate.HasValue)
            {
                return DateTime.Compare(item1.DepartureDate.Value, item2.DepartureDate.Value);
            }
            else if (!item1.DepartureDate.HasValue && !item2.DepartureDate.HasValue)
            {
                return 0;
            }
            else if (!item1.DepartureDate.HasValue)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }


    }
}
