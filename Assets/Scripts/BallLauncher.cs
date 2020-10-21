using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    void Start()
    {
        InvokeRepeating("LaunchBall", 5f, 5f);
    }

    private void LaunchBall()
    {
        GameObject instance = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        Ball volleyball = instance.GetComponent<Ball>();
        rb.velocity = transform.rotation * Vector3.forward * 10f;
        volleyball.hitMultiplier = 10f;
    }
}
