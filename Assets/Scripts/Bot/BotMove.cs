using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    private Vector3 destinationPoint;

    private Ball currentBall;

    void Start()
    {
        BallLauncher.OnLaunch += CalculateDestinationPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (destinationPoint != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationPoint, moveSpeed * Time.deltaTime);
        }
    }

    void CalculateDestinationPoint(Ball volleyball)
    {
        Debug.Log("Calculate Destination Point");
        currentBall = volleyball;
        destinationPoint = currentBall.FindNearestYPointOnPath(transform.position.y);
    }
}
