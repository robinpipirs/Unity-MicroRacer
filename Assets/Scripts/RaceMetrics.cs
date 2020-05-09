using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceMetrics : MonoBehaviour
{
    public Text LapText;
    public Text RacePosition;
    public Text LapTime;
    public Text LastLapTime;
    public Text BestLapTime;

    public CheckpointProgressTracker CarToTrack;

    // Update is called once per frame
    void FixedUpdate()
    {
        var currentLap = CarToTrack.RaceStats.Laps + 1;
        LapText.text = string.Format("Lap: {0} / {1}", currentLap, CarToTrack.TotalLaps);
        RacePosition.text = string.Format("Pos: {0}", CarToTrack.RacePosition);
        LapTime.text = string.Format("Current: {0}", CarToTrack.LapTime);
        LastLapTime.text = string.Format("Last: {0}", CarToTrack.LastLapTime);
        BestLapTime.text = string.Format("Best: {0}", CarToTrack.BestLapTime);
    }
}
