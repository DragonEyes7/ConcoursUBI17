using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class ScheduleTarget : ScheduleNPC
    {
        public ScheduleTarget()
        {
            TimeIntervalls = new List<int>();
            Locations = new List<Transform>();
        }

        public ScheduleTarget(List<int> NewTimeIntervalls, List<Transform> NewLocations)
        {
            TimeIntervalls = NewTimeIntervalls;
            Locations = NewLocations;
        }

        public override Transform NextDestination(int Time, Transform CurrentLocation)
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

        public override void AddItem(int Time, Transform Location)
        {
            TimeIntervalls.Add(Time);
            Locations.Add(Location);
            OrderSchedule();
        }

        public Transform ClosestLocation(Transform CurrentLocation, List<Transform> Locations)
        {
            float SmallestDistance = float.PositiveInfinity;
            Transform Closest = null;
            foreach (Transform t in Locations)
            {
                float distance = 
                    Math.Abs(t.position.x - CurrentLocation.position.x) +
                    Math.Abs(t.position.y - CurrentLocation.position.y) +
                    Math.Abs(t.position.z - CurrentLocation.position.z);
                if (distance < SmallestDistance)
                {
                    SmallestDistance = distance;
                    Closest = t;
                }
            }
            return Closest;
        }

        public override void OrderSchedule()
        {
            if (Locations.Count > 1) {
                List<Transform> LocationsCopy = new List<Transform>();
                LocationsCopy = Locations.ToList();
                Transform currentLocation = LocationsCopy[0];
                LocationsCopy.Remove(currentLocation);
                Locations[0] = currentLocation;
                Transform closest;
                int i = 1;
                while (LocationsCopy.Count > 0)
                {
                    closest = ClosestLocation(currentLocation, LocationsCopy);
                    Locations[i] = closest;
                    i++;
                    LocationsCopy.Remove(closest);
                    currentLocation = closest;
                }
            }
        }
    }
}
