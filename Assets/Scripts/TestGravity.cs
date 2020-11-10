using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGravity : MonoBehaviour
{

    public float graivty = -9.81f;
    public float startingUpwardForce = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, startingUpwardForce, 0);
    }

    void FixedUpdate()
    {
        rb.velocity += new Vector3(0, graivty * Time.fixedDeltaTime, 0);
    }
}
