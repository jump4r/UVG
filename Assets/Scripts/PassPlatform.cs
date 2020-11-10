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
            EnablePlatform();
        }

        else
        {
            DisablePlatform();
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
            
            /* Todo: Fix this, I'm not sure why the Y rotation angle isn't being tracked properly */
            // Probably something to do with localPosition vs. global world space
            Vector3 currentForwardRotation = rigTransform.rotation * Vector3.forward;
            Vector3 angleComparePointY = new Vector3(leftHandTransform.position.x, rigTransform.position.y, leftHandTransform.position.z);
            float platformYAngle = Vector3.Angle((currentForwardRotation - rigTransform.position), (angleComparePointY - rigTransform.position));
            
            // Debug.DrawRay(rigTransform.position, currentForwardRotation * 5f, Color.yellow, .2f);
            // Debug.Log("Current Hand Position: " + leftHandTransform.position.x);
            // Debug.DrawRay(rigTransform.position, angleComparePointY, Color.magenta, .2f);
            /* **** */

            float platformZAngle = (rightHandTransform.rotation.eulerAngles.z + leftHandTransform.rotation.eulerAngles.z) / 2f;
            transform.rotation = Quaternion.Euler(platformXAngle, rigTransform.rotation.eulerAngles.y, platformZAngle);
        }
    }

    public void EnablePlatform()
    {
        platform.enabled = true;
    }

    public void DisablePlatform()
    {
        platform.enabled = false;
    }
}
