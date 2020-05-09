using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointProgressTracker : MonoBehaviour
{
    private List<CheckpointProgressTracker> _opponentRaceCars;

    public Transform Racecars;

    public Transform Checkpoints;
    private List<Checkpoint> _checkpointNodes;
    private int _currentNode = 0;

    public string RacePosition;
    public string LapTime = "00:00:000";
    public string LastLapTime = "00:00:000";
    public string BestLapTime = "00:00:000";

    private float bestLapTimeFloat = float.MaxValue;
    private float startTime;

    public int laps;
    public int TotalLaps;
    public int Laps;
    public int TargetCheckpoint;
    public float DistanceToTargetCheckpoint;
    private bool firstLap = true;

    // Start is called before the first frame update
    void Start()
    {
        GetCheckpointProgressTrackers();
        GetCheckpoints();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateElapsedTime();
        UpdateDistanceToTargetCheckpoint();
        UpdateRacePosition();
    }
    private void UpdateElapsedTime()
    {
        float timeElapsedFloat = Time.time - startTime;
        string lapTimerString = secondsToTime(timeElapsedFloat);
        LapTime = lapTimerString;
    }
    private void UpdateDistanceToTargetCheckpoint()
    {
        var distanceToTargetCheckpoint = Vector3.Distance(transform.position, _checkpointNodes[_currentNode].transform.position);
        DistanceToTargetCheckpoint = distanceToTargetCheckpoint;
    }

    private void UpdateRacePosition()
    {
        int opponentsInfront = GetOpponentsInFront(_opponentRaceCars);
        int racePosition = opponentsInfront + 1;

        RacePosition = string.Format("{0} / {1}", racePosition, (_opponentRaceCars.Count + 1));
    }

    private int GetOpponentsInFront(List<CheckpointProgressTracker> opponentRaceCars)
    {
        int opponentsInFront = 0;
        for (int i = 0; i < opponentRaceCars.Count; i++)
        {
            CheckpointProgressTracker thisRaceCarTracker = this;
            if (isOpponentInFront(opponentRaceCars[i], thisRaceCarTracker)) {
                opponentsInFront++;
            }
        }

        return opponentsInFront;
    }

    private bool isOpponentInFront(CheckpointProgressTracker opponentCar, CheckpointProgressTracker thisCar)
    {
        bool opponentIsInFront = opponentCar.Laps > thisCar.Laps;
        if (opponentIsInFront)
        {
            return true;
        }
        else
        {
            opponentIsInFront = opponentCar.Laps == Laps && opponentCar.TargetCheckpoint > thisCar.TargetCheckpoint;
            if (opponentIsInFront)
            {
                return true;
            }
            else
            {

                opponentIsInFront = opponentCar.TargetCheckpoint == TargetCheckpoint && opponentCar.DistanceToTargetCheckpoint < thisCar.DistanceToTargetCheckpoint;
                if (opponentIsInFront)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private string secondsToTime(float time) 
    {
        //var h=Mathf.Floor(t/360); // no need for hours (unless this is one HUGE lap!)
        int m  = (int)Mathf.Floor(time / 60);// minutes
        int s  = (int)Mathf.Floor(time - m*60); // seconds
        int ss = (int)((time - (m*60) - s)*1000); // leaves subseconds (milliseconds)
        return(m.ToString("00") + ":" + s.ToString("00") + ":" + ss.ToString("000"));
    }

    private void GetCheckpointProgressTrackers()
    {
        CheckpointProgressTracker[] opponentRaceCarTrackers = Racecars.GetComponentsInChildren<CheckpointProgressTracker>();
        _opponentRaceCars = new List<CheckpointProgressTracker>();

        for (int i = 0; i < opponentRaceCarTrackers.Length; i++)
        {
            if (opponentRaceCarTrackers[i] != this)
            {
                _opponentRaceCars.Add(opponentRaceCarTrackers[i]);
            }
        }
    }

    private void GetCheckpoints()
    {
        Checkpoint[] checkpointTransforms = Checkpoints.GetComponentsInChildren<Checkpoint>();
        _checkpointNodes = new List<Checkpoint>();

        for (int i = 0; i < checkpointTransforms.Length; i++)
        {
            if (checkpointTransforms[i] != Checkpoints.transform)
            {
                _checkpointNodes.Add(checkpointTransforms[i]);
            }
        }

        TargetCheckpoint = _checkpointNodes[_checkpointNodes.Count - 1].index;
        _currentNode = _checkpointNodes.Count - 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        Checkpoint cp = other.GetComponent<Checkpoint>();

        bool timeToChangeNode = cp == _checkpointNodes[_currentNode];
        if (timeToChangeNode)
        {

            if (_currentNode == _checkpointNodes.Count - 1)
            {
                if (!firstLap)
                {
                    UpdateLastLap();
                    UpdateBestLap();
                    IncrementLaps();
                    ResetLapTimer();
                }
                else
                {
                    firstLap = false;
                    ResetLapTimer();
                }
            }

            bool lastNode = _currentNode == _checkpointNodes.Count - 1;
            if (lastNode)
            {
                _currentNode = 0;
            }
            else
            {
                _currentNode++;
            }

            TargetCheckpoint = _checkpointNodes[_currentNode].index;
        }
    }

    private void UpdateLastLap()
    {
        LastLapTime = LapTime;
    }

    private void UpdateBestLap()
    {
        float thisLapTime = Time.time - startTime;
        bool thisLapWasFastest = thisLapTime < bestLapTimeFloat;
        if (thisLapWasFastest) {
            LastLapTime = LapTime;
            BestLapTime = LapTime;
        }
    }
    private void IncrementLaps()
    {
        Laps++;
    }

    private void ResetLapTimer()
    {
        startTime = Time.time;
    }


}
