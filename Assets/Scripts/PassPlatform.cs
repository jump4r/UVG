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

    private float energyLost = 0.58f;
    private bool readyToPass = true;
    private const float PASS_TIMEOUT = 0.1f;

    public delegate void OnHitAction(Ball volleyball);
    public static OnHitAction OnBallHit;
    
    private VolleyballPlayer vp;

    void Start()
    {
       platform = GetComponent<BoxCollider>();
       vp = GameObject.FindGameObjectWithTag("Player").GetComponent<VolleyballPlayer>();

       HandGestures.OnGestureChanged += OnGestureChanged;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball" && readyToPass)
        {

            Ball volleyball = col.gameObject.GetComponent<Ball>();
        
            ContactPoint firstContactPoint = col.contacts[0];
            // Vector3 newBallDirection = ((firstContactPoint.normal * -1f) + Vector3.up).normalized;
            Vector3 newBallDirection = (firstContactPoint.normal * -1f).normalized;
            Vector3 newBallVelocity = (newBallDirection * energyLost * volleyball.velBeforePhysicsUpdate.magnitude * PlatformMovementMultiplication());

            volleyball.SetVelocity(newBallVelocity);
            volleyball.CalculatePath();
            volleyball.SetLastTouchedBy(vp);

            // Update Game State
            VolleyballGameManager.instance.HandleInteraction(vp);
            VolleyballGameManager.instance.IncrementHitAmount();

            // Call Bot Listeners
            OnBallHit(volleyball);

            readyToPass = false;
            Invoke("SetReadyToPass", PASS_TIMEOUT);
        }
    }

    private void OnGestureChanged(HandGesture formerGesture, HandGesture currentGesture)
    {
        if (currentGesture == HandGesture.Pass)
        {
            platform.enabled = true;
            DisableHandColliders();
        }

        else if (formerGesture == HandGesture.Pass)
        {
            platform.enabled = false;
            EnableHandColliders();
        }
    }

    private void SetReadyToPass()
    {
        readyToPass = true;
    }

    void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - 0.3f, headTransform.position.z);

        if (platform.enabled)
        {
            transform.rotation = PlatformVectorUtils.PlatformAngle(leftHandTransform, rightHandTransform, rigTransform, transform);
        }
    }

    private float PlatformMovementMultiplication()
    {
        Vector3 leftHandVelocity = leftHandTransform.gameObject.GetComponent<Hand>().GetHandVelocity();
        Vector3 rightHandVelocity = rightHandTransform.gameObject.GetComponent<Hand>().GetHandVelocity();

        return PlatformVectorUtils.CalculatePlaformMovementHitMultiplier(leftHandVelocity, rightHandVelocity);
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
