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

    void Start()
    {
       platform = GetComponent<BoxCollider>(); 
    }

    void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - 0.3f, headTransform.position.z);
        if (Vector3.Distance(rightHandTransform.position, leftHandTransform.position) < 0.2f && !platform.enabled)
        {
            platform.enabled = true;
        }

        else
        {
            platform.enabled = false;
        }

        if (platform.enabled)
        {
            Vector3 averageHandPosition = new Vector3(
                (rightHandTransform.position.x + leftHandTransform.position.x) / 2f,
                (rightHandTransform.position.y + leftHandTransform.position.y) / 2f,
                (rightHandTransform.position.z + leftHandTransform.position.z) / 2f
            );
            Vector3 angleComparePointX = new Vector3(
                averageHandPosition.x, transform.position.y, averageHandPosition.z
            );
            float platformXAngle = Vector3.Angle((angleComparePointX - transform.position), (averageHandPosition - transform.position));
            
            Vector3 angleComparePointY = new Vector3(
                averageHandPosition.x, transform.position.y, averageHandPosition.z
            );

            Vector3 yDirection = (angleComparePointY - transform.position).normalized;
            Vector3 globalForward = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f);

            Debug.DrawLine(transform.position, angleComparePointY, Color.yellow, .2f);
            Debug.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f), Color.magenta, .2f);
            
            float platformYAngle = Vector3.Angle(Vector3.forward, yDirection);
            Debug.Log("Y Axis Rotation: " + platformYAngle);

            float platformZAngle = (rightHandTransform.rotation.eulerAngles.z + leftHandTransform.rotation.eulerAngles.z) / 2f;
            transform.rotation = Quaternion.Euler(platformXAngle, platformYAngle, platformZAngle);
        }
    }
}
