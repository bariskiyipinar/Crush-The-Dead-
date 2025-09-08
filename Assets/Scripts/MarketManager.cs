using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinCounts;
    [SerializeField] private Button[] carButtons;
    [SerializeField] private int[] carPrices;

    private int selectedCarIndex;

    private void Start()
    {
        if (GameManager.instance == null)
        {
            Debug.LogWarning("GameManager instance bulunamadý, coinler PlayerPrefs'ten alýnacak.");
        }
        UpdateCoinUI();

        selectedCarIndex = PlayerPrefs.GetInt("SelectedCar", 0);

        for (int i = 0; i < carButtons.Length; i++)
        {
            int index = i;
            carButtons[i].onClick.AddListener(() => OnCarButtonClicked(index));

        }

        UpdateAllCarUI();
    }

    private void OnCarButtonClicked(int carIndex)
    {
        if (IsCarUnlocked(carIndex))
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SelectCar(carIndex);
            }
            else
            {
                PlayerPrefs.SetInt("SelectedCar", carIndex);
                PlayerPrefs.Save();
            }

            selectedCarIndex = carIndex;
            UpdateAllCarUI();
            SoundManager.instance.Music.Stop();
            SoundManager.instance.carMusic.Play();
            SceneManager.LoadScene("Main");
        }
        else
        {
            TryBuyCar(carIndex);
        }
    }

    private void TryBuyCar(int carIndex)
    {
        int price = carPrices[carIndex];
        int currentCoins = GameManager.instance != null ? GameManager.instance.totalCoins : PlayerPrefs.GetInt("TotalCoins", 0);

        if (currentCoins >= price)
        {
            if (GameManager.instance != null)
            {
                GameManager.instance.SpendCoin(price);
                GameManager.instance.SelectCar(carIndex);
            }
            else
            {
                PlayerPrefs.SetInt("TotalCoins", currentCoins - price);
                PlayerPrefs.SetInt("SelectedCar", carIndex);
                PlayerPrefs.Save();
            }

            UnlockCar(carIndex);

            UpdateCoinUI();
            UpdateAllCarUI();

            Debug.Log("Araba satýn alýndý ve seçildi: " + carIndex);
        }
        else
        {
            Debug.Log("Yeterli coin yok!");
        }
    }

    private bool IsCarUnlocked(int carIndex)
    {
        return PlayerPrefs.GetInt("CarUnlocked_" + carIndex, carIndex == 0 ? 1 : 0) == 1;
    }

    private void UnlockCar(int carIndex)
    {
        PlayerPrefs.SetInt("CarUnlocked_" + carIndex, 1);
    }

    private void UpdateAllCarUI()
    {
        for (int i = 0; i < carButtons.Length; i++)
        {
            bool unlocked = IsCarUnlocked(i);
            int price = carPrices[i];
            int coins = GameManager.instance != null ? GameManager.instance.totalCoins : PlayerPrefs.GetInt("TotalCoins", 0);

            carButtons[i].interactable = unlocked || coins >= price;
            carButtons[i].GetComponent<Image>().color = (i == selectedCarIndex) ? Color.green : Color.white;
        }
    }

    private void UpdateCoinUI()
    {
        int coins = GameManager.instance != null ? GameManager.instance.totalCoins : PlayerPrefs.GetInt("TotalCoins", 0);
        coinCounts.text = coins.ToString();
    }
}
