using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private Queue<Vector3> pastFiveVelocity;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
    }


    void Update()
    {
        if (pastFiveVelocity.Count < 5)
        {
            pastFiveVelocity.Enqueue(rb.velocity);
        }

        else {
            pastFiveVelocity.Dequeue();
            pastFiveVelocity.Enqueue(rb.velocity);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Untagged")
        {
            Debug.Log("Current Velocity: " + rb.velocity);
            ContactPoint firstContactPoint = col.contacts[0];

            Debug.Log("Reflect Over Normal: " + Vector3.Reflect(rb.velocity, firstContactPoint.normal));
            rb.velocity = Vector3.Reflect(rb.velocity, firstContactPoint.normal);
        }
    }
}
