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
    public delegate void OnHitAction(Ball volleyball);
    public static OnHitAction OnBallHit;


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

    private Vector3 FindPassTarget()
    {
        int currentHit = VolleyballGameManager.instance.amountOfHits;
        Vector3 newPassTarget = Vector3.zero;

        switch (currentHit)
        {
            case 0:
                newPassTarget = passTargets.passTarget;
                break;
            case 1:
                newPassTarget = passTargets.setTargets[Role.OUTSIDE];
                break;
            case 2:
                // Hit!
                break;
            default:
                break;
        }

        return newPassTarget;
    }

    // Pass or set ball to a teammate
    private void PassBall(Ball volleyball)
    {
        // Find the appropriate pass target
        passTarget = FindPassTarget();

        // Handle initial, Change possesion & add to hit count if needed
        VolleyballGameManager.instance.HandleInteraction(GetComponent<BotPlayer>());

        float gravity = Physics.gravity.magnitude;

        // Firing angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(passTarget.x, 0, passTarget.z);
        Vector3 planarPosition = new Vector3(volleyball.transform.position.x, 0, volleyball.transform.position.z);

        // Planar distance between self & target
        float distance = Vector3.Distance(planarTarget, planarPosition);
        // Distance along the y axis between objects
        float yOffset = volleyball.transform.position.y - passTarget.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.SignedAngle(Vector3.forward, planarTarget - planarPosition, Vector3.up);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        volleyball.SetVelocity(finalVelocity);
        volleyball.CalculatePath();

        // Lastly, Add 1 to total hits to represent player touching the ball
        VolleyballGameManager.instance.IncrementHitAmount();

        // Call any listeners to OnPass()
        OnBallHit(volleyball);
    }

    // Hit ball over net
    private void HitBall(Ball volleyball)
    {
        
    }
}
