using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Clone : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private GameObject ZombiePrefab;
    [SerializeField] private GameObject ObstaclePrefab;
    [SerializeField] private GameObject RockPrefab;
    [SerializeField] private List<GameObject> RoadList = new List<GameObject>();
    [SerializeField] private List<GameObject> ZombieList= new List<GameObject>();
    [SerializeField] private List<GameObject> ObstacleList = new List<GameObject>();
    [SerializeField] private List<GameObject> RockList = new List<GameObject>();
    [SerializeField] private Transform Player;
    private float SpawnZ = 115.1f;
    private float RoadLenght = 80f;
    private int MaxRoadCount = 5;


    private float nextZombieSpawnZ = 40f; 
    private float zombieSpawnInterval = 20f;

    private float nextObstacleSpawnZ = 40f;
    private float ObstacleSpawnInterval = 20f;

    private float nextRockSpawnZ = 27f;
    private float RockSpawnInterval = 30f;



    private void Start()
    {
        
        for (int i = 0; i < MaxRoadCount; i++)
        {
            SpawnRoad();
        }


        if (Player == null)
        {
            GameObject playerCar = GameObject.FindGameObjectWithTag("PlayerCar");
            if (playerCar != null)
            {
                Player = playerCar.transform;
            }
        }
    }

    private void Update()
    {
        if (Player == null) return;
        if (Player.position.z > SpawnZ - (MaxRoadCount * RoadLenght))
        {
            SpawnRoad();
        }

       
        DeleteOldRoad();


        
        if (Player.position.z > nextZombieSpawnZ)
        {
            SpawnZombie();
            nextZombieSpawnZ += zombieSpawnInterval;

        }
        if(Player.position.z >nextObstacleSpawnZ)
        {
            SpawnObstacle();
            nextObstacleSpawnZ += ObstacleSpawnInterval;
        }
        if (Player.position.z > nextRockSpawnZ)
        {
            SpawnRock();
            nextRockSpawnZ += RockSpawnInterval;
        }

        DeleteOldZombies();
        DeleteOldObstacles();
        DeleteOldRocks();
    }


    void SpawnRoad()
    {
        
        float yPosition = RoadPrefab.transform.position.y;

        Vector3 spawnPos = new Vector3(0f, yPosition, SpawnZ);
        GameObject Road = Instantiate(RoadPrefab, spawnPos, Quaternion.identity);

        RoadList.Add(Road);
        SpawnZ += RoadLenght;
    }


    void DeleteOldRoad()
    {
        if (RoadList.Count > MaxRoadCount)
        {
            GameObject oldRoad = RoadList[0];

            
            if (Player.position.z - oldRoad.transform.position.z > RoadLenght * 2)
            {
                RoadList.RemoveAt(0);
                Destroy(oldRoad);
            }
        }
    }



    void SpawnZombie()
    {
        float randomX = Random.value > 0.5f ? -2f : 2f;
        float randomZ = Player.position.z + 30f;

        Vector3 spawnPos = new Vector3(randomX, 0, randomZ);
        Quaternion lookRotation = Quaternion.LookRotation(Player.position - spawnPos);

        GameObject zombie = Instantiate(ZombiePrefab, spawnPos, lookRotation);
        ZombieList.Add(zombie);
    }

    void DeleteOldZombies()
    {
        
        List<GameObject> zombiesToRemove = new List<GameObject>();

        foreach (GameObject zombie in ZombieList)
        {
            if (zombie != null && Player.position.z - zombie.transform.position.z > 40f)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        foreach (GameObject zombie in zombiesToRemove)
        {
            ZombieList.Remove(zombie);
            Destroy(zombie);
        }
    }
    void DeleteOldObstacles()
    {
       List<GameObject> ObstaclesToRemove =new List<GameObject>();

        foreach(GameObject obstacle in ObstacleList)
        {
            if(obstacle !=null && Player.position.z -obstacle.transform.position.z > 40f)
            {
                ObstaclesToRemove.Add(obstacle);
            }
        }

        foreach(GameObject obstacle in ObstaclesToRemove)
        {
            ObstacleList.Remove(obstacle);
            Destroy(obstacle);
        }
    }

    void SpawnObstacle()
    {
        float randomX = Random.value > 0.5f ? -2f : 2f;
        float randomZ = Player.position.z + 40f;

        Vector3 spawnPos = new Vector3(randomX, 0, randomZ);


        GameObject Obstacle = Instantiate(ObstaclePrefab, spawnPos,Quaternion.identity);
        ObstacleList.Add(Obstacle);
    }

    void SpawnRock()
    {
        float randomX = Random.value > 0.5f ? -2f : 2f;
        float randomZ = Player.position.z + 35f;

        Vector3 spawnPos = new Vector3(randomX, 0, randomZ);


        GameObject Obstacle = Instantiate(RockPrefab, spawnPos, Quaternion.identity);
        RockList.Add(Obstacle);
    }

    void DeleteOldRocks()
    {
        List<GameObject> RockToRemove = new List<GameObject>();

        foreach (GameObject Rock in RockList)
        {
            if (Rock != null && Player.position.z - Rock.transform.position.z > 40f)
            {
                RockToRemove.Add(Rock);
            }
        }

        foreach (GameObject Rock in RockToRemove)
        {
            RockList.Remove(Rock);
            Destroy(Rock);
        }
    }

}
