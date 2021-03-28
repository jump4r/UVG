using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeBallSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject BallPrefab;
    private GameObject spawnedBall;


    private 
    // Start is called before the first frame update
    void Start()
    {
        SpawnBall();
    }
    
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == spawnedBall.name)
        {
            SpawnBall();
        }
    }

    void SpawnBall() {
        spawnedBall = Instantiate(BallPrefab, spawnPoint.position, spawnPoint.rotation);
        spawnedBall.name = "Ball-" + Random.Range(0, 10000);
    }
}
