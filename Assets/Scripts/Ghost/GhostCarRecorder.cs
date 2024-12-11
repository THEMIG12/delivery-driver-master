using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCarRecorder : MonoBehaviour
{
    public Transform carSpriteObj;
    public GameObject ghostCarPlaybackPrefab;

    GhostCarData ghostCarData = new GhostCarData();

    public bool isRecording = true;

    Rigidbody2D carRB;
    CarInputHandler carInputHandler;
    void Awake()
    {
        carRB = GetComponent<Rigidbody2D>();
        carInputHandler = GetComponent<CarInputHandler>();
    }

    void Start()
    {
        GameObject ghostCar = Instantiate(ghostCarPlaybackPrefab);

        ghostCar.GetComponent<GhostCarPlayBack>().LoadData(carInputHandler.PlayerNumber);

        StartCoroutine(RecordCarPositionCO());
        StartCoroutine(SaveCarPositionCO());
    }

    IEnumerator RecordCarPositionCO()
    {
        while (isRecording)
        {
            if (carSpriteObj != null)
                ghostCarData.AddDataItem(new GhostCarDataItem(carRB.position, carRB.rotation, carSpriteObj.localScale, Time.timeSinceLevelLoad));
            yield return new WaitForSeconds(0.15f);
        }
    }

    IEnumerator SaveCarPositionCO()
    {
        yield return new WaitForSeconds(5);

        SaveData();
    }

    void SaveData()
    {
        string jsonEncodedData = JsonUtility.ToJson(ghostCarData);
        Debug.Log($"Saved ghost data{jsonEncodedData}");

        if (carInputHandler != null)
        {
            PlayerPrefs.SetString($"{SceneManager.GetActiveScene().name}_{carInputHandler.PlayerNumber}_ghost", jsonEncodedData);
            PlayerPrefs.Save();
        }

        isRecording = false;
    }

}