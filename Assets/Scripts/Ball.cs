using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Queue<Vector3> pastFiveVelocity;
    public Vector3 velBeforePhysicsUpdate { get; private set;} = Vector3.zero;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
    }

    void FixedUpdate()
    {
        velBeforePhysicsUpdate = rb.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Destroyer")
        {
            Destroy(gameObject);
        }
    }

}
