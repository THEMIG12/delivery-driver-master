using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;
    public List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    void Awake()
    {
        //Get all Car lap counters in the scene. 
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        //Store the lap counters in a list
        carLapCounters = carLapCounterArray.ToList<CarLapCounter>();

        //Hook up the passed checkpoint event 
        foreach (CarLapCounter lapCounters in carLapCounters) lapCounters.OnPassCheckpoint += OnPassCheckpoint;

        //Get the leaderboard UI handler
        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
    }

    void Start()
    {
        leaderboardUIHandler.UpdateList(carLapCounters);
    }

    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        //Sort the cars position first based on how many checkpoints they have passed, more is always better. Then sort on time where shorter time is better 
        carLapCounters = carLapCounters.OrderByDescending(s => s.GetNumberOfCheckpointsPassed()).ThenBy(s => s.GetTimeAtLastCheckPoint()).ToList();

        //Get the cars position
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        //Tell the lap counter which position the car has 
        carLapCounter.SetCarPosition(carPosition);

        leaderboardUIHandler.UpdateList(carLapCounters);
    }
}