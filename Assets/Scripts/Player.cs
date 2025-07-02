using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float smoothTime = 0.15f;

    private float leftClamp;
    private float rightClamp;
    private float targetX;
    private float velocityX = 0f;

    private Rigidbody rb;

    [Header("Zombie Settings")]
    [SerializeField] private Text ZombieText;
    [SerializeField] private Image ZombieSensivity;
    private float currentSensivity = 1f;
    [SerializeField] private float decreaseRate = 0.05f;

    [Header("Health Settings")]
    [SerializeField] private Image CarHealth;
    [SerializeField] private float maxHealth = 10f; 
    private float currentHealth;

    [Header("Vehicle Prefab")]
    public GameObject Vehicle;

    private  bool isDestroyed = false;


  
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHealth = maxHealth;

        ZombieText.text = GameManager.instance.totalCoins.ToString();
        targetX = transform.position.x;

        
        float worldHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        leftClamp = -worldHalfWidth+0.88f;
        rightClamp = worldHalfWidth-0.88f;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {

        if (isDestroyed) return;

        UpdateZombieSensivity();
        UpdateCoinText();
        UpdateCarHealth();
    }

    void MovePlayer()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            float middleScreen = Screen.width / 2;

            if (touch.phase == TouchPhase.Began)
            {
                if (touch.position.x < middleScreen)
                {
                    targetX = leftClamp;
                }
                else
                {
                    targetX = rightClamp;
                }
            }
        }

        float smoothX = Mathf.SmoothDamp(rb.position.x, targetX, ref velocityX, smoothTime);

        Vector3 nextPos = new Vector3(smoothX, rb.position.y, rb.position.z + speed * Time.fixedDeltaTime);

        rb.MovePosition(nextPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine(VehicleTime(0.02f));
            currentHealth = 0;
        }

        if (other.CompareTag("Zombie"))
        {
            GameManager.instance.AddCoin(1);
            Destroy(other.gameObject);

            currentSensivity = Mathf.Clamp01(currentSensivity + 0.1f);
            ZombieSensivity.fillAmount = currentSensivity;
        }
    }

    IEnumerator VehicleTime(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject newVehicle = Instantiate(Vehicle, transform.position, Quaternion.Euler(0, -159.21f, 0));

        isDestroyed = true;
        Destroy(this.gameObject);

        ParticleSystem ps = newVehicle.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
            yield return new WaitForSeconds(3f);
            ps.Stop();
           
        }
       
    }

    void UpdateZombieSensivity()
    {
        if (currentSensivity > 0)
        {
            currentSensivity -= decreaseRate * Time.deltaTime;
            ZombieSensivity.fillAmount = currentSensivity;
            ZombieSensivity.transform.localScale = new Vector3(currentSensivity, ZombieSensivity.transform.localScale.y, ZombieSensivity.transform.localScale.z);
           
        }
        else
        {
            StartCoroutine(VehicleTime(0));
        }
    }

    void UpdateCoinText()
    {
        ZombieText.text = GameManager.instance.totalCoins.ToString();
    }

    void UpdateCarHealth()
    {
        float healthRatio = Mathf.Clamp01(currentHealth / maxHealth);

        CarHealth.transform.localScale = new Vector3(healthRatio, CarHealth.transform.localScale.y, CarHealth.transform.localScale.z);

        if (currentHealth <= 0f)
        {
            StartCoroutine(VehicleTime(0f));
        }
    }
}
