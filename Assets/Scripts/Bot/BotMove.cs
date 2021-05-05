using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Vector3 destinationPoint;

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
        BallLauncher.OnLaunch += CalculateDestinationPoint;
        botHeight = GetComponent<CapsuleCollider>().height;
        botRadius = GetComponent<CapsuleCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (destinationPoint != Vector3.zero)
        {
            Vector3 yLockedDestination = new Vector3(destinationPoint.x, transform.position.y, destinationPoint.z);
            transform.position = Vector3.MoveTowards(transform.position, yLockedDestination, moveSpeed * Time.deltaTime);
        }

        // Gravity
        CalcualteGravityEffect();
        transform.position += (verticalVelocity * Time.fixedDeltaTime);
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

    void CalculateDestinationPoint(Ball volleyball)
    {
        currentBall = volleyball;
        destinationPoint = currentBall.FindNearestYPointOnPath(transform.position.y);
    }

    bool CheckIfGrounded()
    {
        float rayLength = (botHeight / 2f) - botRadius + 0.01f;
        bool hasHit = Physics.SphereCast(transform.position, botRadius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
        return hasHit;
    }
}
