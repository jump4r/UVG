using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSetter : MonoBehaviour
{
    [SerializeField]
    private Hand leftHand;

    [SerializeField]
    private Hand rightHand;
    private Ball volleyball;
    private GameObject player;
    private HandGestures handGestures;
    private float setSpeedMultiplier = 2.5f;

    private VolleyballPlayer vp;

    public delegate void OnSetAction(Ball volleyball);
    public static OnSetAction OnBallSet;
    void Start()
    {
        handGestures = GameObject.FindGameObjectWithTag("Gestures").GetComponent<HandGestures>();
        player = GameObject.FindGameObjectWithTag("Player");
        vp = player.GetComponent<VolleyballPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (handGestures.currentGesture == HandGesture.Set)
        {
            transform.position = GetCenter();
            GetComponent<SphereCollider>().enabled = true;

            if (volleyball != null)
            {
                volleyball.GetComponent<Rigidbody>().useGravity = false;

                volleyball.transform.position = GetCenter();
            }
        }
    
        else {
            if (volleyball != null)
            {
                rightHand.ResetHandCollider();
                leftHand.ResetHandCollider();

                Rigidbody ballRb = volleyball.GetComponent<Rigidbody>();
                ballRb.useGravity = true;

                Vector3 newBallVel = player.transform.rotation * (((leftHand.GetHandVelocity() + rightHand.GetHandVelocity()) / 2f) * setSpeedMultiplier);
                ballRb.velocity = newBallVel;

                // Update Game State
                VolleyballGameManager.instance.HandleInteraction(vp);
                VolleyballGameManager.instance.IncrementHitAmount();
                
                // Call bot listeners to bot set
                OnBallSet(volleyball);

                volleyball = null;
                GetComponent<SphereCollider>().enabled = false;
            }
        }
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Ball")
        {
            volleyball = collider.gameObject.GetComponent<Ball>();
        }
    }

    private Vector3 GetCenter()
    {
        return (leftHand.gameObject.transform.position + rightHand.gameObject.transform.position) / 2f;
    }
}
