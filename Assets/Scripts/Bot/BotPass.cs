 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotPass : MonoBehaviour
{
    private BotPlayer botPlayer;
    private bool readyToPass = true;
    private const float PASS_TIMEOUT = 0.1f;
    [SerializeField]
    private Vector3 passTarget;

    [SerializeField]
    private float initialAngle;

    private BotPassTargets passTargets;
    public delegate void OnHitAction(Ball volleyball);
    public static OnHitAction OnBallHit;


    void Start()
    {
        botPlayer = GetComponent<BotPlayer>();
        passTargets = GameObject.FindGameObjectWithTag(botPlayer.managerTag).GetComponentInChildren<BotPassTargets>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            if (readyToPass)
            {
                PassBall(col.gameObject.GetComponent<Ball>());
                readyToPass = false;
                Invoke("SetReadyToPass", PASS_TIMEOUT);
            }
        }
    }

    void SetReadyToPass()
    {
        readyToPass = true;
    }

    private Vector3 GetInitialHitVelocity(Ball volleyball)
    {
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
        
        return Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
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
                newPassTarget = passTargets.GetSetTargetFromAvailablePlayers();
                break;
            case 2:
                // Choose a hit target
                newPassTarget = passTargets.GetHitTarget();
                break;
            default:
                break;
        }

        return newPassTarget;
    }

    // Pass or set ball to a teammate
    private void PassBall(Ball volleyball)
    {
        // Handle initial, Change possesion & add to hit count if needed
        VolleyballGameManager.instance.HandleInteraction(GetComponent<BotPlayer>());

        // Find the appropriate pass target
        passTarget = FindPassTarget();

        // Set initial pass angle
        initialAngle = GetInitialAngleFromHitType(volleyball);

        Vector3 initialVelocity = GetInitialHitVelocity(volleyball);

        volleyball.SetVelocity(initialVelocity);
        volleyball.CalculatePath();

        // Lastly, Add 1 to total hits to represent player touching the ball
        VolleyballGameManager.instance.IncrementHitAmount();

        // Call any listeners to OnPass()
        OnBallHit(volleyball);
    }

    private float GetInitialAngleFromHitType(Ball volleyball)
    {
        int currentHit = VolleyballGameManager.instance.amountOfHits;
        float angle = 65f;

        if (currentHit == 2) 
        {
            float netAngle = AngleToNet(volleyball);
            angle = netAngle < 0 ? netAngle + 20f : angle;
        }

        return angle;
    }

    // Hit ball over net
    private void HitBall(Ball volleyball)
    {
        float netAngle = AngleToNet(volleyball);

        // Find the appropriate pass target
        passTarget = FindPassTarget();

        // Set initial pass angle, spike if we're above the net, otherwise freeball
        initialAngle = netAngle < 0 ? netAngle + 10f : 65f;
    }

    private float AngleToNet(Ball volleyball)
    {
        Vector3 netTopPos = VolleyballGameManager.instance.topOfNet;
        
        Vector3 ballPos = volleyball.transform.position; 
        Vector3 netComparePos = new Vector3(ballPos.x, netTopPos.y, netTopPos.z); // Top of the net at the ball's xPosition
        Vector3 ballComparePos = new Vector3(ballPos.x, netTopPos.y, ballPos.z); // On top of net plane, at Ball's z position
        
        float hitAngle = Vector3.SignedAngle((ballComparePos - netComparePos), (ballPos - netComparePos), Vector3.right);

        Debug.DrawLine(ballComparePos, netComparePos, Color.green, 3f);
        Debug.DrawLine(ballPos, netComparePos, Color.cyan, 3f);

        hitAngle = botPlayer.team == Team.RED ? hitAngle * -1 : hitAngle;

        return hitAngle;
    }
}
