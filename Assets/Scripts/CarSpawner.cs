using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] carPrefabs;

    private void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager bulunamadý! Ana menüden baþlat.");
            return;
        }

        int selectedCar = GameManager.instance.selectedCarIndex;
        Instantiate(carPrefabs[selectedCar], spawnPoint.position, spawnPoint.rotation);
    }
}
