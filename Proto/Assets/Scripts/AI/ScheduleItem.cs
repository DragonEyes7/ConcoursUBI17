using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class ScheduleItem
    {
        public int ScheduledTime;
        public Transform ScheduledLocation;

        public ScheduleItem(int Time, Transform Location)
        {

            ScheduledTime = Time;
            ScheduledLocation = Location;
        }

    }
}
