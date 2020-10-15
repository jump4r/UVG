using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class Climber : MonoBehaviour
{
    public CharacterController character;
    public static XRController climbingHand;
    private ContinuousMovement continuousMovement;
    void Start()
    {
        character = GetComponent<CharacterController>();
        continuousMovement = GetComponent<ContinuousMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (climbingHand)
        {
            continuousMovement.enabled = false;
            Climb();
        }

        else
        {
            continuousMovement.enabled = true;
        }
    }

    void Climb()
    {
        InputDevices.GetDeviceAtXRNode(climbingHand.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 velocity);
        character.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }
}
