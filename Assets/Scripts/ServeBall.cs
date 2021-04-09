using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ServeBall : XRGrabInteractable
{
    private XRController controller;
    private Vector3 handVelocity;
    private bool serveReady = false;
    private Rigidbody rb;
    public float tossMultiplier = 2f;
    public GameObject Ball;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    // Todo: Replace with Action-Based Equivilents;
    // protected override void OnSelectEnter(XRBaseInteractor interactor)
    // {
    //     base.OnSelectEnter(interactor);
    //     if (interactor is XRDirectInteractor)
    //     {
    //         controller = interactor.GetComponent<XRController>();
    //     }
    // }

    // protected override void OnSelectExit(XRBaseInteractor interactor)
    // {
    //     base.OnSelectExit(interactor);
    //     if (interactor is XRDirectInteractor)
    //     {
    //         if (controller)
    //         {
    //             StartCoroutine(TossBall());
    //         }
    //     }
    // }

    IEnumerator TossBall()
    {
        InputDevices.GetDeviceAtXRNode(controller.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out handVelocity);
        controller = null;
        yield return 0;

        rb.velocity = Vector3.up * (1 + handVelocity.magnitude) * tossMultiplier;
        Invoke("TransitionToRegularBall", 0.25f);
    }

    void TransitionToRegularBall()
    {
        GameObject b = Instantiate(Ball, transform.position, transform.rotation);
        b.GetComponent<Rigidbody>().velocity = rb.velocity;
        Destroy(gameObject);
    }
}
