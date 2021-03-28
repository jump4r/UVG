using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Hand : MonoBehaviour
{
    public float hitMultiplier = 3f;

    [SerializeField]
    private XRNode handInputDevice;


    private Vector3 deviceVelocity;

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Ball")
        {
            Ball volleyball = col.gameObject.GetComponent<Ball>();
            ContactPoint firstContactPoint = col.contacts[0];
            InputDevices.GetDeviceAtXRNode(handInputDevice).TryGetFeatureValue(CommonUsages.deviceVelocity, out deviceVelocity);

            Vector3 newBallVelocity = firstContactPoint.normal * hitMultiplier * deviceVelocity.magnitude;
            volleyball.SetVelocity(newBallVelocity * -1f);
        }
    }

    public Vector3 GetHandVelocity()
    {
        Vector3 outVelocity;
        InputDevices.GetDeviceAtXRNode(handInputDevice).TryGetFeatureValue(CommonUsages.deviceVelocity, out outVelocity);
        return outVelocity;
    }
}
