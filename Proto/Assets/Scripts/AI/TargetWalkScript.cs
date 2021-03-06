﻿using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetWalkScript : NPCWalkScript {

    private ScheduleTarget NPCSchedule;

    public void setSchedule(ScheduleTarget Schedule)
    {
        NPCSchedule = Schedule;
    }

    public override void OnTimeChange(int Time)
    {
        Transform NextPosition = NPCSchedule.NextDestination(Time, this.gameObject.transform);
        if (NextPosition != Location)
        {
            Destination = NextPosition;
            Location = Destination;
            OnDestinationChange();
        }
    }
}
