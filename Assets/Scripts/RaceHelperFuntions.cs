using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RaceHelperFuntions
{
    public static bool IsOpponentInFront(RaceStats opponentCar, RaceStats thisCar)
    {
        bool opponentIsInFrontOnDifferentLap = opponentCar.Laps > thisCar.Laps;
        bool opponentHasHigherCheckpointTarget = opponentCar.TargetCheckpoint > thisCar.TargetCheckpoint;
        bool opponentIsCloserToCheckpointTarget = opponentCar.DistanceToTargetCheckpoint < thisCar.DistanceToTargetCheckpoint;

        bool opponentIsOnTheSameLap = opponentCar.Laps == thisCar.Laps;
        bool opponentHasTheSameCheckpointTarget = opponentCar.Laps == thisCar.Laps;

        if (opponentIsInFrontOnDifferentLap)
        {
            return true;
        }

        if (opponentIsOnTheSameLap) {

            if (opponentHasHigherCheckpointTarget) 
            {
                return true;
            }

            if (opponentHasTheSameCheckpointTarget) 
            {
                if (opponentIsCloserToCheckpointTarget) 
                {
                    return true;
                }
            }
        }

        return false;
    }
}
