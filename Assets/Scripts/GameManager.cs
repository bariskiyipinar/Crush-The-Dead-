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

    private void Awake()
    {
        if (instance != null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
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
}
