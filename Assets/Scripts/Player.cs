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
    [SerializeField] private Text ZombieText;
    [SerializeField] private Image ZombieSensivity;
    [SerializeField] private Image CarHealth;

    private float currentSensivity = 1f;
    private float currentHealth;
    [SerializeField] private float maxHealth = 10f;
    [SerializeField] private float decreaseRate = 0.05f;

    [Header("Explosion Prefab")]
    public GameObject Vehicle;

    [Header("Car Prefabs")]
    public GameObject Araba1Prefab;
    public GameObject Araba2Prefab;
    public GameObject Araba3Prefab;
    public GameObject Araba4Prefab;  

    private GameObject currentVehicle;
    private bool isDestroyed = false;

    private bool hasSpawnedVehicle = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        targetX = transform.position.x;

        float halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        leftClamp = -halfWidth + 0.88f;
        rightClamp = halfWidth - 0.88f;

        ZombieText.text = GameManager.instance.totalCoins.ToString();

     if(GameManager.instance.totalCoins > 100 )
        {
            SpawnSelectedVehicle();
        }
            
     
        hasSpawnedVehicle = true;
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

    void SpawnSelectedVehicle()
    {
        int selectedCar = PlayerPrefs.GetInt("SelectedCar", 1);

        GameObject prefabToSpawn = selectedCar switch
        {
            1 => Araba1Prefab,
            2 => Araba2Prefab,
            3 => Araba3Prefab,
            4 => Araba4Prefab,
            _ => Araba1Prefab
        };

        if (currentVehicle != null)
        {
            Destroy(currentVehicle);
        }

        currentVehicle = Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
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

        Instantiate(Vehicle, transform.position, Quaternion.Euler(0, -159.21f, 0));
        isDestroyed = true;

        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);

        SceneManager.LoadScene("Market");
    }
}
