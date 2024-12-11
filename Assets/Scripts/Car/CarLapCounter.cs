using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CarLapCounter : MonoBehaviour
{
    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;
    int numberOfPassedCheckpoints = 0;
    int lapsCompleted = 0;
    int lapsToComplete = 3;
    int carPosition = 0;
    bool isHideRoutineRunning = false;
    float hideUIDelayTime;
    bool isRaceCompleted = false;
    public TextMeshProUGUI carPositionText;

    //Events
    public event Action<CarLapCounter> OnPassCheckpoint;

    public void SetCarPosition(int Cposition)
    {
        carPosition = Cposition;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }

    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassedCheckPoint;
    }

    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime += delayUntilHidePosition;

        carPositionText.text = carPosition.ToString();

        carPositionText.gameObject.SetActive(true);
        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;

            yield return new WaitForSeconds(delayUntilHidePosition);
            carPositionText.gameObject.SetActive(false);

            isHideRoutineRunning = false;
        }
    }


    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("CheckPoint"))
        {

            if (isRaceCompleted) return;

            CheckPoint checkPoint = collider2D.GetComponent<CheckPoint>();

            //Make sure that the car is passing the checkpoints in the correct order. The correct checkpoint must have exactly 1 higher value than the passed checkpoint 
            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;

                numberOfPassedCheckpoints++;

                //Store the time at the checkpoint
                timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                    }
                }

                //Invoke the passed checkpoint event
                OnPassCheckpoint?.Invoke(this);

                //Now show the cars position as it has been calculated

                if (isRaceCompleted)
                    StartCoroutine(ShowPositionCO(100));
                else StartCoroutine(ShowPositionCO(1.5f));
            }
        }
    }
}