using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    private BotPlayer botPlayer;
    private BotDestinations destinations;

    // Movement
    [SerializeField]
    private float moveSpeed;
    public Vector3 destinationPoint;
    private CharacterController controller;

    // Gravity
    private float fallingSpeed = 0f;

    // --- Jump ---
    // When the ball is set to a bot, this is the position the ball will be when the bot will
    // jump to spike.
    private Vector3 jumpPoint = Vector3.zero;
    private float jumpForce = 5f;
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
        botHeight = GetComponent<CharacterController>().height;
        botRadius = GetComponent<CharacterController>().radius;
        controller = GetComponent<CharacterController>();
        maxJumpHeight = GetMaxJumpHeight();
        timeToMaxJumpHeight = GetTimeToMaxJumpHeight();

        destinations = GameObject.FindGameObjectWithTag(botPlayer.managerTag).GetComponentInChildren<BotDestinations>();

        StartCoroutine(LateStart(1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Ball.OnOutOfPlay += MoveToInitialPosition;
        Debug.Log("Late Start Called");
    }
    
    private float GetMaxJumpHeight()
    {
        // Eq: hMax = V0^2 * sin(a)^2 / (2 * g), a = pi / 2, v0 = jumpForce, g = gravity
        return Mathf.Pow(jumpForce, 2) / (2 * Physics.gravity.magnitude);
    }

    private float GetTimeToMaxJumpHeight()
    {
        // Eq: t = V0 / g
        return jumpForce / Physics.gravity.magnitude;
    }

    void FixedUpdate()
    {
        if (destinationPoint != Vector3.zero)
        {
            Vector3 movementVector = (destinationPoint - transform.position);
            Vector3 normalizedDirection = new Vector3(movementVector.x, 0, movementVector.z).normalized;

            if (!ArrivedAtDestination())
            {
                controller.Move(normalizedDirection * Time.fixedDeltaTime * moveSpeed);
                if (gameObject.name == "Bot Player (1)" || gameObject.name == "Blue Team Server")
                {
                    Debug.DrawLine(destinationPoint, transform.position, Color.white);
                }
            }

            else if (IsGrounded())
            {
                transform.position = destinationPoint;
            }
        }

        // -- Vertical Movements --
        // Gravity Calculations
        CalcualteGravityEffect();

        // Jump if needed
        if (jumpPoint != Vector3.zero)
        {
            Ball currentBall = VolleyballGameManager.instance.currentBall;

            if (currentBall != null && Vector3.Distance(currentBall.transform.position, jumpPoint) < 0.1f)
            {
                Jump();
                jumpPoint = Vector3.zero;
            }
           
        }

        controller.Move(verticalVelocity * Time.fixedDeltaTime);
    }

    void CalcualteGravityEffect()
    {
        // Entire game is flat so this is probably fine
        // Looks like The Gravity Calculations are being done async, sometimes the bot will have a vertical velocity but still be grounded
        // If the bot has a vertical velocity, assume they are still mid jump, only calculate grounded state if they are falling or zero
        if (verticalVelocity.y > 0)
        {
            fallingSpeed = Physics.gravity.y * Time.fixedDeltaTime;
            verticalVelocity += Vector3.up * fallingSpeed;
            return;
        }

        if (IsGrounded())
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
        if (!IsGrounded())
        {
            return;
        }
        
        verticalVelocity = Vector3.up * jumpForce;
    }

    public void CalculateAndMoveToDestinationPoint(Ball volleyball)
    {
        Team landingTeam = VolleyballGameManager.instance.FindTeamLandingZone();
        int currentHit = VolleyballGameManager.instance.amountOfHits;

        if (landingTeam != botPlayer.team)
        {
            return;
        }

        // Move to spike spike point, set jump point
        if (currentHit == 2)
        {
            destinationPoint = volleyball.FindNearestYPointOnPath(maxJumpHeight + botHeight / 2);
            jumpPoint = volleyball.GetJumpPoint(destinationPoint, timeToMaxJumpHeight);
        }

        else {
            destinationPoint = volleyball.FindNearestYPointOnPath(botHeight / 2);
            destinationPoint.y = botHeight / 2;
        }
    }

    public void MoveToTarget(Vector3 target)
    {
        destinationPoint = target;
        destinationPoint.y = botHeight / 2f;
    }

    public void MoveToBlockPoint(Ball volleyball)
    {
        Vector3 oppSpikePoint = volleyball.FindNearestYPointOnPath(maxJumpHeight + botHeight / 2);
        Vector3 netPosition = GameObject.FindGameObjectWithTag("Net").transform.position;

        float updatedZPos = oppSpikePoint.z < 0 ? 0.5f : -0.5f;
        float updatedXPos = oppSpikePoint.x;

        destinationPoint = new Vector3(updatedXPos, oppSpikePoint.y, updatedZPos);
        jumpPoint = volleyball.GetJumpPoint(destinationPoint, timeToMaxJumpHeight);

        Debug.DrawLine(transform.position, destinationPoint, Color.green, 5f);
    }

    public void MoveToInitialPosition()
    {
        destinationPoint = ServerManager.instance.currentServer == botPlayer ?
            destinations.GetServerPosition() :
            destinations.GetServeReceivePosition(botPlayer.role);

        destinationPoint.y = botHeight / 2f;
    }

    bool IsGrounded()
    {
        float rayLength = (controller.height / 2f) - controller.radius + 0.01f;
        return Physics.SphereCast(transform.position, botRadius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    }

    public bool ArrivedAtDestination()
    {
        if (Vector3.Distance(transform.position, destinationPoint) < .3f)
        {
            transform.position = destinationPoint;
            return true;
        }
        
        return false;
    }
}
