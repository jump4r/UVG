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
    private float passMultiplier = 5f;

    void Start()
    {
       platform = GetComponent<BoxCollider>(); 
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Ball volleyball = col.gameObject.GetComponent<Ball>();
            
            ContactPoint firstContactPoint = col.contacts[0];

            // Repace new velocity to be affected by ball speed.
            Vector3 newBallVelocity = firstContactPoint.normal * passMultiplier;

            volleyball.SetVelocity(newBallVelocity * -1f);
        }
    }

    void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - 0.3f, headTransform.position.z);
        if (Vector3.Distance(rightHandTransform.position, leftHandTransform.position) < 0.2f)
        {
            platform.enabled = true;
            DisableHandColliders();
        }

        else
        {
            platform.enabled = false;
            EnableHandColliders();
        }

        if (platform.enabled)
        {
            // X Position & Angle
            ////////////////////
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
            ////////////////////
            Vector3 angleComparePointY = new Vector3(
                averageHandPosition.x, transform.position.y, averageHandPosition.z
            );

            Vector3 yDirection = (angleComparePointY - transform.position).normalized;
            Vector3 globalForward = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);
            float platformYAngle = Vector3.SignedAngle(Vector3.forward, yDirection, Vector3.up);

            // Z Position & Angle
            /////////////////////
            float platformZAngle = (rightHandTransform.rotation.eulerAngles.z + leftHandTransform.rotation.eulerAngles.z) / 2f;
            transform.rotation = Quaternion.Euler(platformXAngle, platformYAngle, platformZAngle);
        }
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
