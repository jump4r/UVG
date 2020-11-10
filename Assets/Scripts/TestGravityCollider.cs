using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGravityCollider : MonoBehaviour
{

    public float graivty = -9.81f;
    public float force = 10f;

    void Start()
    {
        
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        force += graivty * Time.fixedDeltaTime;
        transform.Translate(new Vector3(0, force * Time.fixedDeltaTime, 0));
        Debug.Log("Force : " + force);
    }
}
