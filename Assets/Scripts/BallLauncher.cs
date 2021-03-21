using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    [SerializeField]
    private float launchSpeed = 10f;
    
    [SerializeField]
    private float launchFrequency = 5f;
    void Start()
    {
        InvokeRepeating("LaunchBall", launchFrequency, launchFrequency);
    }

    private void LaunchBall()
    {
        GameObject instance = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        Ball volleyball = instance.GetComponent<Ball>();
        rb.velocity = transform.rotation * Vector3.forward * launchSpeed;
    }
}
