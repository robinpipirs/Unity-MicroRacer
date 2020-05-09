using Oculus.Platform.Models;
public class RaceStats
{
    public readonly int Laps;
    public readonly int TargetCheckpoint;
    public readonly int TotalCheckpoints;
    public readonly float DistanceToTargetCheckpoint;

    public RaceStats(int laps, int targetCheckPoint, int totalCheckpoints, float distanceToTargetCheckpoint) 
    {
        Laps = laps;
        TargetCheckpoint = targetCheckPoint;
        TotalCheckpoints = totalCheckpoints;
        DistanceToTargetCheckpoint = distanceToTargetCheckpoint;
    }
}