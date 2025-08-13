using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject[] playerCarPrefabs;
    private Transform spawnPoint;

    private GameObject currentPlayerCar;

    public int totalCoins;
    public int selectedCarIndex;

    public GameObject VehiclePrefab;
    public GameObject ZombieDeadEffect;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
            selectedCarIndex = PlayerPrefs.GetInt("SelectedCar", 0);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            spawnPoint = GameObject.FindWithTag("SpawnPoint")?.transform;

            if (spawnPoint != null)
            {
                SpawnCar(selectedCarIndex);
            }
            else
            {
                Debug.LogError("SpawnPoint bulunamadý! Lütfen Main sahnesinde 'SpawnPoint' tag'li bir obje oluþtur.");
            }
        }
    }

    public void SpawnCar(int index)
    {
        // Eski arabalarý sahneden kaldýr
        foreach (var car in FindObjectsOfType<Player>())
        {
            Destroy(car.gameObject);
        }

        if (index >= 0 && index < playerCarPrefabs.Length && spawnPoint != null)
        {
            currentPlayerCar = Instantiate(playerCarPrefabs[index], spawnPoint.position, spawnPoint.rotation);
            currentPlayerCar.tag = "PlayerCar";

            
            Player playerScript = currentPlayerCar.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.ZombieText = GameObject.Find("CoinCount").GetComponent<Text>();
                playerScript.ZombieSensivity = GameObject.Find("Health").GetComponent<Image>();
                playerScript.CarHealth = GameObject.Find("Health").GetComponent<Image>();
                playerScript.TimeCount = GameObject.Find("TimeCount").GetComponent<Text>();
                playerScript.Vehicle = VehiclePrefab;
                playerScript.ZombieDeadEffect = ZombieDeadEffect;
            }

           
            CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
            if (vcam != null)
            {
                vcam.Follow = currentPlayerCar.transform;
                vcam.LookAt = currentPlayerCar.transform;
            }

        }
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        SaveCoins();
    }

    public void SpendCoin(int amount)
    {
        totalCoins -= amount;
        if (totalCoins < 0) totalCoins = 0;
        SaveCoins();
    }

    public void SaveCoins()
    {
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
    }

    public void SelectCar(int index)
    {
        selectedCarIndex = index;
        PlayerPrefs.SetInt("SelectedCar", selectedCarIndex);
        PlayerPrefs.Save();
    }
}
