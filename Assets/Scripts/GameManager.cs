using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int totalCoins;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
    }
}
