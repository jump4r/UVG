using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public delegate void LaunchAction(Ball volleyball);
    public static event LaunchAction OnLaunch;
    public GameObject ballPrefab;
    [SerializeField]
    private float launchSpeed = 10f;
    
    [SerializeField]
    private float launchFrequency = 5f;
    void Start()
    {
        InvokeLaunch(launchFrequency);
    }


    public void InvokeLaunch(float delay)
    {
        Invoke("LaunchBall", delay);
    }

    public void LaunchBall()
    {
        GameObject instance = Instantiate(ballPrefab, transform.position, Quaternion.identity);
        Ball volleyball = instance.GetComponent<Ball>();
        Rigidbody rb = instance.GetComponent<Rigidbody>();
        rb.velocity = transform.rotation * Vector3.forward * launchSpeed;

        // Trigger Launch Event, Calcualte Trajectory beforehand since we need to do it this frame 
        volleyball.CalculatePath();
        volleyball.launcher = this;

        // Update Vball Game Manager State
        VolleyballGameManager.instance.amountOfHits = 0;
        VolleyballGameManager.instance.currentPossesion = Team.BLUE;
        VolleyballGameManager.instance.currentBall = volleyball;

        // Call On Launch Delegates
        OnLaunch(volleyball);
    }
}
