 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPass : MonoBehaviour
{
    [SerializeField]
    private Transform passTarget;

    [SerializeField]
    private float initialAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Collision Detected");
        if (col.gameObject.tag == "Ball")
        {
            PassBall(col.gameObject.GetComponent<Ball>());
        }
    }

    void PassBall(Ball volleyball)
    {
        Vector3 p = passTarget.position;
        float gravity = Physics.gravity.magnitude;

        // Firing angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPosition = new Vector3(volleyball.transform.position.x, 0, volleyball.transform.position.z);

        // Planar distance between self & target
        float distance = Vector3.Distance(planarTarget, planarPosition);
        // Distance along the y axis between objects
        float yOffset = volleyball.transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.SignedAngle(Vector3.forward, planarTarget - planarPosition, Vector3.up);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        volleyball.SetVelocity(finalVelocity);
        volleyball.CalculatePath();
    }
}
