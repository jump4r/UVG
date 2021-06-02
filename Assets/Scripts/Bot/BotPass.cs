 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPass : MonoBehaviour
{
    [SerializeField]
    private Vector3 passTarget;

    [SerializeField]
    private float initialAngle;

    private BotPassTargets passTargets;

    void Start()
    {
        passTargets = GameObject.FindGameObjectWithTag("BlueTeamManager").GetComponentInChildren<BotPassTargets>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            PassBall(col.gameObject.GetComponent<Ball>());
        }
    }

    void FindPassTarget()
    {
        int currentHit = VolleyballGameManager.instance.amountOfHits;

        switch (currentHit)
        {
            case 0:
                passTarget = passTargets.setTarget;
        }
    }

    void PassBall(Ball volleyball)
    {
        // Handle initial, Change possesion & add to hit count if needed
        VolleyballGameManager.instance.HandleInteraction(GetComponent<BotPlayer>());

        // Find the appropriate pass target
        FindPassTarget();

        Vector3 p = passTarget;
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
