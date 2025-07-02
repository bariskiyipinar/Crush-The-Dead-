using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;  
    [SerializeField] private int score = 0;   
    public static GameManager instance;

    private float scoreTimer = 0f; 
    private float scoreInterval = 1f;
    public int totalCoins = 0;

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

        totalCoins = PlayerPrefs.GetInt("TotalCoins", 0);
    }

    private void Start()
    {
        scoreText.text = score.ToString(); 
    }

    private void Update()
    {
        
        scoreTimer += Time.deltaTime;

   
        if (scoreTimer >= scoreInterval)
        {
            score += 1; 
            scoreTimer = 0f; 
            scoreText.text = score.ToString(); 
        }

         
    }

    public void AddCoin(int amount)
    {
        totalCoins += amount;
        PlayerPrefs.SetInt("TotalCoins", totalCoins);
        PlayerPrefs.Save();
    }


}
