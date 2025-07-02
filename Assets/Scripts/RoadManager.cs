using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoadManager : MonoBehaviour
{
    [SerializeField] private GameObject RoadPrefab;
    [SerializeField] private List<GameObject> RoadList=new List<GameObject> ();
    [SerializeField] private Transform Player;
    private float SpawnZ = 80.1323f;
    private float RoadLenght = 35f;
    private int MaxRoadCount = 5;

    private void Start()
    {
       
        for (int i = 0; i < MaxRoadCount; i++)
        {
            SpawnRoad();
        }
    }

    private void Update()
    {
        if (Player.position.z > SpawnZ - (MaxRoadCount * RoadLenght))
        {
            SpawnRoad();
            DeleteOldRoad();
        }
    }

    void SpawnRoad()
    {
        Vector3 spawnPos = new Vector3(0f, 0f, RoadPrefab.transform.position.z + SpawnZ);
        GameObject Road = Instantiate(RoadPrefab, spawnPos, Quaternion.identity);

        RoadList.Add(Road);
        SpawnZ += RoadLenght;
    }

    void DeleteOldRoad()
    {
        if (RoadList.Count > MaxRoadCount)
        {
            Destroy(RoadList[0]);
            RoadList.RemoveAt(0);
        }
    }
}
