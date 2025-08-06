using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    public static MarketManager instance;

    [SerializeField] private TextMeshProUGUI coinCounts;

    [Header("Satýn Alma Butonlarý")]
    [SerializeField] private Button BuyButton1;
    [SerializeField] private Button BuyButton2;
    [SerializeField] private Button BuyButton3;
    [SerializeField] private Button BuyButton4;

    [Header("Seçme Butonlarý")]
    [SerializeField] private Button SelectButton1;
    [SerializeField] private Button SelectButton2;
    [SerializeField] private Button SelectButton3;
    [SerializeField] private Button SelectButton4;

    [Header("Arabalarýn Fiyatlarý")]
    [SerializeField] private int car1Price = 0;
    [SerializeField] private int car2Price = 150;
    [SerializeField] private int car3Price = 200;
    [SerializeField] private int car4Price = 250;

    private int totalCoins;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        coinCounts.text = totalCoins.ToString();

        // Butonlara listener atama
        BuyButton1.onClick.AddListener(() => BuyCar(1, car1Price));
        BuyButton2.onClick.AddListener(() => BuyCar(2, car2Price));
        BuyButton3.onClick.AddListener(() => BuyCar(3, car3Price));
        BuyButton4.onClick.AddListener(() => BuyCar(4, car4Price));

        SelectButton1.onClick.AddListener(() => SelectCar(1));
        SelectButton2.onClick.AddListener(() => SelectCar(2));
        SelectButton3.onClick.AddListener(() => SelectCar(3));
        SelectButton4.onClick.AddListener(() => SelectCar(4));

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        // 1. araba hep sahipli, satýn alma kapalý seçim açýk
        BuyButton1.interactable = false;
        SelectButton1.interactable = true;

        BuyButton2.interactable = totalCoins >= car2Price && !IsCarOwned(2);
        SelectButton2.interactable = IsCarOwned(2);

        BuyButton3.interactable = totalCoins >= car3Price && !IsCarOwned(3);
        SelectButton3.interactable = IsCarOwned(3);

        BuyButton4.interactable = totalCoins >= car4Price && !IsCarOwned(4);
        SelectButton4.interactable = IsCarOwned(4);
    }

    private void BuyCar(int carIndex, int price)
    {
        if (IsCarOwned(carIndex))
        {
            Debug.Log("Zaten satýn alýndý.");
            return;
        }

        if (totalCoins >= price)
        {
            totalCoins -= price;
            PlayerPrefs.SetInt("TotalCoins", totalCoins);
            PlayerPrefs.SetInt("CarOwned_" + carIndex, 1);
            PlayerPrefs.SetInt("SelectedCar", carIndex);

            coinCounts.text = totalCoins.ToString();
            UpdateButtons();

            Debug.Log($"Araba {carIndex} satýn alýndý ve seçildi.");
            LoadMainScene();
        }
        else
        {
            Debug.Log("Yetersiz coin.");
        }
    }

    private void SelectCar(int carIndex)
    {
        if (IsCarOwned(carIndex))
        {
            PlayerPrefs.SetInt("SelectedCar", carIndex);
            Debug.Log($"Araba {carIndex} seçildi.");
            LoadMainScene();
        }
        else
        {
            Debug.Log("Bu araba satýn alýnmamýþ.");
        }
    }

    private bool IsCarOwned(int carIndex)
    {
        if (carIndex == 1) return true; // 1. araba varsayýlan olarak sahipli
        return PlayerPrefs.GetInt("CarOwned_" + carIndex, 0) == 1;
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
