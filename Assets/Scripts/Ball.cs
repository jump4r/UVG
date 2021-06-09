using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    public Vector3 velBeforePhysicsUpdate { get; private set;} = Vector3.zero;
    public Vector3 estimatedLandingPos { get; private set; } = Vector3.zero;
    private const float groundYPosition = -0.5f; // Todo: Not hard set this? need to just get the pos of the ground
    
    private int arcPoints = 50;
    private Vector3[] projectedPath;

    // Debug Vars for testing
    public BallLauncher launcher;
    private bool toBeDestroyed = false;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        projectedPath = new Vector3[arcPoints + 1];
    }

    void Start()
    {
        CalculatePath();
    }

    void FixedUpdate()
    {
        velBeforePhysicsUpdate = rb.velocity;
    }

    void OnCollisionEnter(Collision col)
    {
        if (
            (col.gameObject.layer ==  9 || col.gameObject.tag == "Destroyer") &&
            !toBeDestroyed
        )
        {
            toBeDestroyed = true;
            Invoke("DestroyAndRelaunch", 1f);
        }
    }

    void DestroyAndRelaunch()
    {
        launcher.InvokeLaunch(1f);
        Destroy(this.gameObject);
    }


    public void SetVelocity(Vector3 newVel)
    {
        rb.velocity = newVel;
    }

    public void CalculatePath()
    {
        float g = Physics.gravity.y;
        Vector3 velocity = rb.velocity;
        Vector3 initialPoint = transform.position;

        for(int i = 0; i < projectedPath.Length; i++)
        {
            float t = i * (Time.fixedDeltaTime * 5);
            float x = initialPoint.x + (velocity.x * t);
            float z = initialPoint.z + (velocity.z * t);
            float y = initialPoint.y + (velocity.y * t) + (g * Mathf.Pow(t, 2) / 2);

            Vector3 newPoint = new Vector3(x, y, z);
            projectedPath[i] = new Vector3(x, y, z);

            if (i > 0 && projectedPath[i].y < groundYPosition && projectedPath[i-1].y > groundYPosition)
            {
                estimatedLandingPos = projectedPath[i-1];
            }
        }

        for (int i = 0; i < projectedPath.Length - 1; i++) {
            Debug.DrawLine(projectedPath[i], projectedPath[i+1], Color.red, 3f);
        }
    }

    // Gets point on the balls trajectory closest to the yOrigin value on the y-axis
    public Vector3 FindNearestYPointOnPath(float yOrigin)
    {
        for (int i = 0; i < projectedPath.Length; i++)
        {
            if (i > 0 && projectedPath[i].y < yOrigin && projectedPath[i-1].y > yOrigin)
            {
                return projectedPath[i-1];
            }
        }   

        return Vector3.zero;
    }

    // This should be replaced with like actual math
    public Vector3 FindNearestXPointOnPath(float xOrigin)
    {
        for (int i = 0; i < projectedPath.Length; i++)
        {
            if (i > 0 && projectedPath[i].x < xOrigin && projectedPath[i-1].x > xOrigin)
            {
                return projectedPath[i-1];
            }
        }   

        return Vector3.zero;
    }

    public Vector3 GetJumpPoint(Vector3 hitPoint, float timeToMaxJumpHeight)
    { 
        Vector3 yLockedVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        Vector3 jumpXZPoint = hitPoint - (yLockedVelocity * timeToMaxJumpHeight);

        Debug.Log("Hit Point: " + hitPoint);
        Debug.Log("Velocity Delta: " + (yLockedVelocity * timeToMaxJumpHeight));

        // This is so bad omg, replace this with an anti-grav equation and we can get the
        // Actual point we should jump at.
        return FindNearestXPointOnPath(jumpXZPoint.x);
    }
}
