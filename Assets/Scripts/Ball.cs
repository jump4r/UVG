using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public float hitMultiplier = 5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Hand")
        {
            ContactPoint firstContactPoint = col.contacts[0];
            rb.velocity = firstContactPoint.normal * hitMultiplier;
            Debug.DrawRay(transform.position, firstContactPoint.normal, Color.red, 5f);
        }
    }
}
