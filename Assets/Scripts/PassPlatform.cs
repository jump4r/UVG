using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform rightHandTransform;
    public Transform leftHandTransform;

    public Transform headTransform;

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

            Debug.Log("Angle: " + Vector3.Angle(Vector3.forward, (averageHandPosition - transform.position)));
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
