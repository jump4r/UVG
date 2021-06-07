using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    private BotPlayer botPlayer;

    [SerializeField]
    private float moveSpeed;
    private Vector3 destinationPoint;
    [SerializeField]
    private float jumpForce;
    private float maxJumpHeight;
    private Rigidbody rb;

    // Gravity
    private float fallingSpeed = 0f;
    private Vector3 verticalVelocity = Vector3.zero;

    private Ball currentBall;
    private float botHeight;
    private float botRadius;
    [SerializeField]
    private LayerMask groundLayer;

    void Start()
    {
        botPlayer = GetComponent<BotPlayer>();
        botHeight = GetComponent<CapsuleCollider>().height;
        botRadius = GetComponent<CapsuleCollider>().radius;
        rb = GetComponent<Rigidbody>();
        maxJumpHeight = GetMaxJumpHeight();
    }

    void Update()
    {
        Vector3 yLockedDestination = Vector3.zero;

        if (destinationPoint != Vector3.zero)
        {
            yLockedDestination = new Vector3(destinationPoint.x, transform.position.y, destinationPoint.z);
            transform.position = Vector3.MoveTowards(transform.position, yLockedDestination, moveSpeed * Time.deltaTime);
        }

        // Gravity
        CalcualteGravityEffect();
        transform.position += (verticalVelocity * Time.fixedDeltaTime);
    }

    void Jump()
    {
        if (!CheckIfGrounded())
        {
            return;
        }

        Debug.Log("Add Velocity for jump");
        rb.velocity = (Vector3.up * jumpForce);
    }

    private float GetMaxJumpHeight()
    {
        // Eq: hMax = V0^2 * sin(a)^2 / (2 * g), a = pi / 2, v0 = jumpForce, g = gravity
        return Mathf.Pow(jumpForce, 2) / (2 * Physics.gravity.magnitude);
    }

    void CalcualteGravityEffect()
    {
        if (CheckIfGrounded())
        {
            fallingSpeed = 0;
            verticalVelocity = Vector3.zero;
        }

        else
        {
            fallingSpeed += Physics.gravity.y * Time.fixedDeltaTime;
        }

        verticalVelocity += Vector3.up * fallingSpeed * Time.fixedDeltaTime;
    }

    public void CalculateAndMoveToDestinationPoint(Ball volleyball)
    {
        Team landingTeam = VolleyballGameManager.instance.FindTeamLandingZone();

        if (landingTeam != botPlayer.team)
        {
            return;
        }

        currentBall = volleyball;
        destinationPoint = currentBall.FindNearestYPointOnPath(transform.position.y);
    }

    public void MoveToTarget(Vector3 target) {
        destinationPoint = target;
    }

    bool CheckIfGrounded()
    {
        float rayLength = (botHeight / 2f) - botRadius + 0.01f;
        bool hasHit = Physics.SphereCast(transform.position, botRadius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
