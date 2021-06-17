using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    private float initialYVelocity = 0f;
    private Vector3 initialPos = Vector3.zero;

    public Vector3 velBeforePhysicsUpdate { get; private set;} = Vector3.zero;
    public Vector3 estimatedLandingPos { get; private set; } = Vector3.zero;
    
    private int arcPoints = 50;
    private Vector3[] projectedPath;

    // Debug Vars for testing
    public BallLauncher launcher;
    private bool toBeDestroyed = false;

    // Out Of Play Delegate
    public delegate void OutOfPlayAction();
    public static OutOfPlayAction OnOutOfPlay;
    
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

            // Remove when we actually start serving
            if (launcher)
            {
                OnOutOfPlay();
            }
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
        initialYVelocity = newVel.y;
        initialPos = transform.position;
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

            if (i > 0 && projectedPath[i].y < 0 && projectedPath[i-1].y > 0)
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

    public List<float> GetTimeToYPosition(float yPos)
    {
        List<float> times = new List<float>();

        // Use Quadratic Forumla
        // ( v0 +/- Sqrt(v0^2 + 2g(y - y0)) )  / g
        // y0 = Initial Height
        // y = Ending Position
        // g = Gravity
        // v0 = Initial Y Velocity

        float sqrtValue = Mathf.Pow(initialYVelocity, 2) + (2 * Physics.gravity.magnitude * (initialPos.y - yPos));
        
        // If value under sqrt is negative, y pos won't be reached
        if (sqrtValue < 0) { return times; }

        times.Add((initialYVelocity + Mathf.Sqrt(sqrtValue)) / Physics.gravity.magnitude);
        times.Add((initialYVelocity - Mathf.Sqrt(sqrtValue)) / Physics.gravity.magnitude);

        return times;
    }

    public Vector3 GetJumpPoint(Vector3 hitPoint, float timeToMaxJumpHeight)
    {
        // Get Time From Start to Hit Point
        List<float> hitTimes = GetTimeToYPosition(hitPoint.y);
        float tA = 0;

        switch (hitTimes.Count)
        {
            case 0:
                return Vector3.zero;
            case 1:
                tA = hitTimes[0];
                break;
            case 2:
                tA = Mathf.Max(hitTimes[0], hitTimes[1]);
                break;
        }


        float t = tA - timeToMaxJumpHeight;
        float y = (-0.5f * Physics.gravity.magnitude * Mathf.Pow(t, 2)) + (initialYVelocity * t) + initialPos.y;
        float x = rb.velocity.x * t + initialPos.x;
        float z = rb.velocity.z * t + initialPos.z;

        return new Vector3(x, y, z);
    }
}
