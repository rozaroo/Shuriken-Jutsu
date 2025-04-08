using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrunkSpawner : MonoBehaviour
{
    public GameObject trunkPrefab;
    private float heightRange = 1.5f;
    private float maxTime = 3.5f;
    private float timer;
    
    void Start() 
    {
        SpawnTrunk();
    }
    void Update() 
    {
        timer += Time.deltaTime;
        if (timer > maxTime) 
        {
            SpawnTrunk();
            timer = 0;
        }
    }
    public void SpawnTrunk() 
    {
        Vector3 spawnPosition = transform.position + new Vector3(0, Random.Range(-heightRange,heightRange));
        GameObject newTrunk;
        newTrunk = Instantiate(trunkPrefab, spawnPosition, Quaternion.identity);
        Destroy(newTrunk, 15f);
    }



}

