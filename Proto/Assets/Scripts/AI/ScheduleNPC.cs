using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class ScheduleNPC
    {
        protected List<int> TimeIntervalls;
        protected List<Transform> Locations;

        public ScheduleNPC()
        {
            TimeIntervalls = new List<int>();
            Locations = new List<Transform>();
        }

        public ScheduleNPC(List<int> NewTimeIntervalls, List<Transform> NewLocations)
        {
            TimeIntervalls = NewTimeIntervalls;
            Locations = NewLocations;
        }

        public virtual Transform NextDestination(int Time, Transform CurrentLocation)
        {
            bool smaller = true;
            int i = 0;
            while (smaller)
            {
                //(NPCSchedule[i].ScheduledTime < Time) ? i++ : smaller = false;

                if ( i < TimeIntervalls.Count() && TimeIntervalls[i] < Time)
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
            if (1 > TimeIntervalls.Count()) i = TimeIntervalls.Count();
            //Transform Closest = ClosestLocation(CurrentLocation);
            //Locations.Remove(Closest);
            return Locations[i];
        }

        public virtual void AddItem(int Time, Transform Location)
        {
            TimeIntervalls.Add(Time);
            Locations.Add(Location);
            OrderSchedule();
        }

        public void RemoveLocation(Transform Location)
        {
            Locations.Remove(Location);
        }

        public virtual void OrderSchedule()
        {
            TimeIntervalls.OrderBy(x => x);
        }
    }
}
