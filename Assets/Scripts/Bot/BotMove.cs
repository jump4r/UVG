using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    private BotPlayer botPlayer;

    // Movement
    [SerializeField]
    private float moveSpeed;
    private Vector3 destinationPoint;
    private Rigidbody rb;
    private CharacterController controller;

    // Gravity
    private float fallingSpeed = 0f;

    // --- Jump ---
    // When the ball is set to a bot, this is the position the ball will be when the bot will
    // jump to spike.
    private Vector3 jumpPoint = Vector3.zero;
    private float jumpForce = 4f;
    private float maxJumpHeight;
    private float timeToMaxJumpHeight;
    private Vector3 verticalVelocity = Vector3.zero;

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
        controller = GetComponent<CharacterController>();
        maxJumpHeight = GetMaxJumpHeight();
        timeToMaxJumpHeight = GetTimeToMaxJumpHeight();

         Debug.Log("\nMax Jump height: " + maxJumpHeight + " Time To Max Jump: " + timeToMaxJumpHeight);
    }
    
    private float GetMaxJumpHeight()
    {
        // Eq: hMax = V0^2 * sin(a)^2 / (2 * g), a = pi / 2, v0 = jumpForce, g = gravity
        return Mathf.Pow(jumpForce, 2) / (2 * Physics.gravity.magnitude);
    }

    private float GetTimeToMaxJumpHeight()
    {
        // Eq: t = 2 * V0 / g
        return 2 * jumpForce / Physics.gravity.magnitude;
    }

    void FixedUpdate()
    {
        Vector3 yLockedDestination = Vector3.zero;

        if (destinationPoint != Vector3.zero && !ArrivedAtDestination())
        {
            Vector3 normalizedMovementVector = (destinationPoint - transform.position);
            Vector3 direction = new Vector3(normalizedMovementVector.x, 0, normalizedMovementVector.z).normalized;
            controller.Move(direction * Time.fixedDeltaTime * moveSpeed);
        }

        // -- Vertical Movements --
        // Gravity Calculations
        CalcualteGravityEffect();

        // Jump if needed
        if (jumpPoint != Vector3.zero)
        {
            Ball currentBall = VolleyballGameManager.instance.currentBall;

            if (Vector3.Distance(currentBall.transform.position, jumpPoint) < 0.1f)
            {
                Jump();
                jumpPoint = Vector3.zero;
            }
           
        }

        controller.Move(verticalVelocity * Time.fixedDeltaTime);
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
            fallingSpeed = Physics.gravity.y * Time.fixedDeltaTime;
        }


        verticalVelocity += Vector3.up * fallingSpeed;
    }

    void Jump()
    {
        if (!CheckIfGrounded())
        {
            return;
        }
        
        verticalVelocity = Vector3.up * jumpForce;
    }

    public void CalculateAndMoveToDestinationPoint(Ball volleyball, int currentHit)
    {
        Team landingTeam = VolleyballGameManager.instance.FindTeamLandingZone();

        if (landingTeam != botPlayer.team)
        {
            return;
        }

        // Move to spike spike point, set jump point
        if (currentHit == 2)
        {
            destinationPoint = volleyball.FindNearestYPointOnPath(maxJumpHeight);
            jumpPoint = volleyball.GetJumpPoint(destinationPoint, timeToMaxJumpHeight);

            Debug.Log("Jump Point: " + jumpPoint);
        }

        else {
            destinationPoint = volleyball.FindNearestYPointOnPath(transform.position.y);
        }
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

    private bool ArrivedAtDestination()
    {
        return Vector3.Distance(transform.position, destinationPoint) < 0.2f;
    }
}
