using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectUICarHandler : MonoBehaviour
{
    public GameObject carPrefab;
    public Transform spawnOnTransform;
    CarUIHandler carUIHandler = null;
    bool isChangingCar = false;
    public CarData[] carDatas;

    int selectedCarIndex = 0;

    void Start()
    {
        carDatas = Resources.LoadAll<CarData>("CarData/");

        StartCoroutine(SpawnCarCO(true));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            OnNextCar();
        }

        else if (Input.GetKey(KeyCode.RightArrow))
        {
            OnPreviousCar();
        }

        else if (Input.GetKey(KeyCode.Space))
        {
            OnSelectedCar();
        }
    }

    public void OnPreviousCar()
    {
        if (isChangingCar) return;
        selectedCarIndex--;
        if (selectedCarIndex < 0) selectedCarIndex = carDatas.Length - 1;

        StartCoroutine(SpawnCarCO(false));
    }
    public void OnNextCar()
    {
        if (isChangingCar) return;
        selectedCarIndex++;
        if (selectedCarIndex > carDatas.Length - 1) selectedCarIndex = 0;

        StartCoroutine(SpawnCarCO(true));
    }
    public void OnSelectedCar()
    {
        PlayerPrefs.SetInt("P1SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P2SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P3SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);
        PlayerPrefs.SetInt("P4SelectedCarID", carDatas[selectedCarIndex].CarUniqueID);

        PlayerPrefs.Save();
        SceneManager.LoadScene("TopDown Controller Last");
    }

    IEnumerator SpawnCarCO(bool isCarAppearingOnTheRightSide)
    {
        isChangingCar = true;

        if (carUIHandler != null)
            carUIHandler.StartCarExitAnimation(!isCarAppearingOnTheRightSide);

        GameObject instantiatedCar = Instantiate(carPrefab, spawnOnTransform);
        carUIHandler = instantiatedCar.GetComponent<CarUIHandler>();
        carUIHandler.SetupCar(carDatas[selectedCarIndex]);
        carUIHandler.StartCarEnterAnimation(isCarAppearingOnTheRightSide);

        yield return new WaitForSeconds(0.4f);
        isChangingCar = false;
    }
}
