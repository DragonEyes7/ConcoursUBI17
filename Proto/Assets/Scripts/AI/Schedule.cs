using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Schedule
    {
        private List<ScheduleItem> NPCSchedule;
        public Schedule()
        {
            NPCSchedule = new List<ScheduleItem>();
        }

        public Schedule(List<ScheduleItem> NewNPCSchedule)
        {
            NPCSchedule = NewNPCSchedule;
        }

        public Transform NextDestination(int Time)
        {
            bool smaller = true;
            int i = 0;
            while (smaller)
            {
                //(NPCSchedule[i].ScheduledTime < Time) ? i++ : smaller = false;

                if ( i < NPCSchedule.Count && NPCSchedule[i].ScheduledTime < Time)
                {
                    i++;
                }
                else
                {
                    smaller = false;
                    i--;
                }
            }
            if (i < 0) i = 0;
            return NPCSchedule[i].ScheduledLocation;
        }

        public void AddItem(int Time, Transform Location)
        {
            NPCSchedule.Add(new ScheduleItem(Time, Location));
        }

        public void OrderSchedule()
        {
            NPCSchedule.OrderBy(x => x.ScheduledTime);
        }
    }
}
