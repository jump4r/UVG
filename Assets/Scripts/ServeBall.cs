using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ServeBall : XRGrabInteractable
{
    public XRNode inputSource;
    private Vector3 handVelocity;
    private bool serveReady = false;
    private Rigidbody rb;
    public float tossMultiplier = 2f;

    // Disable script in game,
    // Enable only when servering
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    void Update()
    {
        if (serveReady)
        {
            InputDevice device = InputDevices.GetDeviceAtXRNode(inputSource);
            device.TryGetFeatureValue(CommonUsages.deviceVelocity, out handVelocity);
        }
    }

    protected override void OnSelectEnter(XRBaseInteractor interactor)
    {

    }

    protected override void OnSelectExit(XRBaseInteractor interactor)
    {
        
    }

    public void GrabBall()
    {
        serveReady = true;
    }

    public void TossBall()
    {
        serveReady = false;
        rb.AddForce(handVelocity * tossMultiplier);
    }
}
