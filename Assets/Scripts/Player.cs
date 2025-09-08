using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float smoothTime = 0.15f;

    private float leftClamp, rightClamp, targetX, velocityX = 0f;
    private Rigidbody rb;



    [Header("UI")]
     public Text ZombieText;
     public Image ZombieSensivity;
     public Image CarHealth;
     public Text TimeCount;

    private float elapsedTime = 0f;  
    private int displayedTime = 0; 

    private float currentSensivity = 1f;
    private float currentHealth;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float decreaseRate = 0.05f;

    [Header("Explosion Prefab")]
    public GameObject Vehicle;

    [Header("Effect")]
    public GameObject ZombieDeadEffect;


    private bool isDestroyed = false;

   

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        targetX = transform.position.x;

        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        leftClamp = -2f;
        rightClamp = 2f;

        ZombieText.text = GameManager.instance.totalCoins.ToString();

     
    }

    void FixedUpdate()
    {
        if (!isDestroyed)
            MovePlayer();
    }

    void Update()
    {
        if (isDestroyed) return;

        UpdateZombieSensivity();
        UpdateCoinText();
        UpdateCarHealth();


        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 1f)
        {
            displayedTime++;
            elapsedTime = 0f;
            UpdateTimeCount();
        }
    }

    void MovePlayer()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            targetX = touch.position.x < Screen.width / 2 ? leftClamp : rightClamp;
        }

        float smoothX = Mathf.SmoothDamp(rb.position.x, targetX, ref velocityX, smoothTime);
        Vector3 nextPos = new Vector3(smoothX, rb.position.y, rb.position.z + speed * Time.fixedDeltaTime);
        rb.MovePosition(nextPos);
    }

   

    void UpdateZombieSensivity()
    {
        if (currentSensivity > 0)
        {
            currentSensivity -= decreaseRate * Time.deltaTime;
            ZombieSensivity.fillAmount = currentSensivity;
         
        }
        else
        {
            StartCoroutine(VehicleTime(0));
        }
    }


    void UpdateTimeCount()
    {
        if (TimeCount != null)
        {
            TimeCount.text = displayedTime.ToString();
        }
    }

    void UpdateCoinText()
    {
        ZombieText.text = GameManager.instance.totalCoins.ToString();
    }

    void UpdateCarHealth()
    {
        float ratio = Mathf.Clamp01(currentHealth / maxHealth);
        CarHealth.transform.localScale = new Vector3(ratio, CarHealth.transform.localScale.y, CarHealth.transform.localScale.z);
        if (currentHealth <= 0f)
            StartCoroutine(VehicleTime(0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            currentHealth = 0;
            StartCoroutine(VehicleTime(0.02f));
        }

        if (other.CompareTag("Rock"))
        {
            currentHealth -= 1f;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Zombie"))
        {
            GameManager.instance.AddCoin(1);
            currentHealth += 1f;

            GameObject effect = Instantiate(ZombieDeadEffect,other.transform.position + new Vector3(0f, 2f, 0f), other.transform.rotation);
            Destroy(effect, 1f);


            Destroy(other.gameObject);

            currentSensivity = Mathf.Clamp01(currentSensivity + 0.1f);
            ZombieSensivity.fillAmount = currentSensivity;
        }
    }

    IEnumerator VehicleTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(Vehicle, transform.position, Quaternion.Euler(0, -159.21f, 0));
        isDestroyed = true;
        Transform obje=this.gameObject.transform.GetChild(0);
        obje.gameObject.SetActive(false);

        PlayerPrefs.SetInt("TotalCoins", GameManager.instance.totalCoins);
        PlayerPrefs.Save();
        SoundManager.instance.carMusic.Stop();
        yield return new WaitForSeconds(2.5f);
        SoundManager.instance.Music.Play();
       
    
        SceneManager.LoadScene("Market");
      
    }
}
