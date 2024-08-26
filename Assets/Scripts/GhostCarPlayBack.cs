using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GhostCarPlayBack : MonoBehaviour
{
    GhostCarData ghostCarData = new GhostCarData();
    List<GhostCarDataItem> ghostCarDataItems = new List<GhostCarDataItem>();

    int currentPlaybackIndex = 0;

    float lastStoredTime = 0.1f;
    Vector2 lastStoredPosition = Vector2.zero;
    float lastStoredRotation = 0.1f;
    Vector2 lastStoredLocalScale = Vector3.zero;

    float duration = 0.1f;

    void Update()
    {
        if (ghostCarDataItems.Count == 0) return;

        if (Time.timeSinceLevelLoad >= ghostCarDataItems[currentPlaybackIndex].timeSinceLevelLoaded)
        {
            lastStoredTime = ghostCarDataItems[currentPlaybackIndex].timeSinceLevelLoaded;
            lastStoredPosition = ghostCarDataItems[currentPlaybackIndex].position;
            lastStoredRotation = ghostCarDataItems[currentPlaybackIndex].rotationZ;
            lastStoredLocalScale = ghostCarDataItems[currentPlaybackIndex].localScale;

            if (currentPlaybackIndex < ghostCarDataItems.Count - 1) currentPlaybackIndex++;

            duration = ghostCarDataItems[currentPlaybackIndex].timeSinceLevelLoaded - lastStoredTime;
        }

        float timePassed = Time.timeSinceLevelLoad - lastStoredTime;
        float lerpPercentage = timePassed / duration;

        transform.position = Vector2.Lerp(lastStoredPosition, ghostCarDataItems[currentPlaybackIndex].position, lerpPercentage);
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, lastStoredRotation), Quaternion.Euler(0, 0, ghostCarDataItems[currentPlaybackIndex].rotationZ), lerpPercentage);
        transform.localScale = Vector3.Lerp(lastStoredLocalScale, ghostCarDataItems[currentPlaybackIndex].localScale, lerpPercentage);
    }

    public void LoadData(int PlayerNumber)
    {
        if (!PlayerPrefs.HasKey($"{SceneManager.GetActiveScene().name}_{PlayerNumber}_ghost"))
            Destroy(gameObject);
        else
        {
            string jsonEncodedData = PlayerPrefs.GetString($"{SceneManager.GetActiveScene().name}_{PlayerNumber}_ghost");

            ghostCarData = JsonUtility.FromJson<GhostCarData>(jsonEncodedData);
            ghostCarDataItems = ghostCarData.GetDataItem();

        }
    }
}
