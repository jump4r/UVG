using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ServeBall : XRGrabInteractable
{
    private ActionBasedController controller;
    private Vector3 handVelocity;
    private bool serveReady = false;
    private Rigidbody rb;
    public float tossMultiplier = 2f;
    public GameObject Ball;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        if (interactor is XRDirectInteractor)
        {
            controller = interactor.GetComponent<ActionBasedController>();
        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        base.OnSelectExited(interactor);
        if (interactor is XRDirectInteractor)
        {
            if (controller)
            {
                StartCoroutine(TossBall());
            }
        }
    }

    IEnumerator TossBall()
    {
        // InputDevices.GetDeviceAtXRNode(controller.controllerNode).TryGetFeatureValue(CommonUsages.deviceVelocity, out handVelocity);
        handVelocity = controller.gameObject.GetComponent<Hand>().GetHandVelocity();
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
