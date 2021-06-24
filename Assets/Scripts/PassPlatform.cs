using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    public Transform headTransform;
    public Transform rigTransform;

    private BoxCollider platform;
    [SerializeField]
    private HandGestures handGestures;

    private float energyLost = 0.75f;
    private bool readyToPass = true;
    private const float PASS_TIMEOUT = 0.1f;

    public delegate void OnHitAction(Ball volleyball);
    public static OnHitAction OnBallHit;
    
    private VolleyballPlayer vp;

    void Start()
    {
       platform = GetComponent<BoxCollider>();
       vp = GameObject.FindGameObjectWithTag("Player").GetComponent<VolleyballPlayer>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            if (readyToPass)
            {
                Ball volleyball = col.gameObject.GetComponent<Ball>();
            
                ContactPoint firstContactPoint = col.contacts[0];
                // Vector3 newBallDirection = ((firstContactPoint.normal * -1f) + Vector3.up).normalized;
                Vector3 newBallDirection = (firstContactPoint.normal * -1f).normalized;
                Vector3 newBallVelocity = (newBallDirection * energyLost * volleyball.velBeforePhysicsUpdate.magnitude * platformMovementMultiplication());

                volleyball.SetVelocity(newBallVelocity);
                volleyball.CalculatePath();

                Debug.Log("Ball passed by player, call it up");

                // Update Game State
                VolleyballGameManager.instance.HandleInteraction(vp);
                VolleyballGameManager.instance.IncrementHitAmount();


                // Call Bot Listeners
                OnBallHit(volleyball);

                readyToPass = false;
                Invoke("SetReadyToPass", PASS_TIMEOUT);
            }
            
        }
    }

    private void SetReadyToPass()
    {
        readyToPass = true;
    }

    void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - 0.3f, headTransform.position.z);
        
        if (handGestures.currentGesture == HandGesture.Pass)
        {
            platform.enabled = true;
            // DisableHandColliders();
        }

        else
        {
            platform.enabled = false;
            // EnableHandColliders();
        }

        if (platform.enabled)
        {
           setPlatformAngle();
        }
    }

    private void setPlatformAngle()
    {
         // X Position & Angle
            Vector3 averageHandPosition = new Vector3(
                (rightHandTransform.position.x + leftHandTransform.position.x) / 2f,
                (rightHandTransform.position.y + leftHandTransform.position.y) / 2f,
                (rightHandTransform.position.z + leftHandTransform.position.z) / 2f
            );
            Vector3 angleComparePointX = new Vector3(
                averageHandPosition.x, transform.position.y, averageHandPosition.z
            );
            float platformXAngle = Vector3.SignedAngle((angleComparePointX - transform.position), (averageHandPosition - transform.position), rigTransform.rotation * Vector3.right);
            
            // Y Position & Angle
            Vector3 angleComparePointY = new Vector3(
                averageHandPosition.x, transform.position.y, averageHandPosition.z
            );

            Vector3 yDirection = (angleComparePointY - transform.position).normalized;
            Vector3 globalForward = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            float platformYAngle = Vector3.SignedAngle(Vector3.forward, yDirection, Vector3.up);

            // Z Position & Angle
            float platformZAngle = (rightHandTransform.rotation.eulerAngles.z + leftHandTransform.rotation.eulerAngles.z) / 2f;
            transform.rotation = Quaternion.Euler(platformXAngle, platformYAngle, platformZAngle);
    }
    private float platformMovementMultiplication()
    {
        Vector3 leftHandVelocity = leftHandTransform.gameObject.GetComponent<Hand>().GetHandVelocity();
        Vector3 rightHandVelocity = rightHandTransform.gameObject.GetComponent<Hand>().GetHandVelocity();
    
        Vector3 averageHandVelocity = (leftHandVelocity + rightHandVelocity) / 2f;
        float platformMovementAngle = Vector3.Angle(Vector3.up, averageHandVelocity);
        float platformSpeedClamp = Mathf.Max(1f, Mathf.Log(averageHandVelocity.magnitude, 2f));
        float platformAdjustment = platformMovementAngle < 90f ? platformSpeedClamp : 1f / platformSpeedClamp;

        return platformAdjustment;
    }

    private void DisableHandColliders()
    {
        leftHandTransform.gameObject.GetComponent<BoxCollider>().enabled = false;
        rightHandTransform.gameObject.GetComponent<BoxCollider>().enabled = false;
    }

    private void EnableHandColliders()
    {
        leftHandTransform.gameObject.GetComponent<BoxCollider>().enabled = true;
        rightHandTransform.gameObject.GetComponent<BoxCollider>().enabled = true;
    }
}
