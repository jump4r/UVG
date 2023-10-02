using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public delegate void LaunchAction(Ball volleyball);
    public static event LaunchAction OnServe;
    public GameObject ballPrefab;
    [SerializeField]
    private float launchSpeed = 10f;

    [SerializeField]
    private float launchNoise = 0.5f;
    
    [SerializeField]
    private float launchFrequency = 5f;
    [SerializeField]
    private Team startingTeam;
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

        Vector3 noiseVector = VectorUtils.NoiseVector(launchNoise);
        
        rb.velocity = (transform.rotation * Vector3.forward * launchSpeed) + noiseVector;

        // Trigger Launch Event, Calcualte Trajectory beforehand since we need to do it this frame 
        volleyball.CalculatePath();
        volleyball.launcher = this;

        // Update Vball Game Manager State
        VolleyballGameManager.instance.amountOfHits = 0;
        VolleyballGameManager.instance.currentPossesion = startingTeam;
        VolleyballGameManager.instance.currentBall = volleyball;

        // Call On Launch Delegates
        // OnServe(volleyball);
    }
}
