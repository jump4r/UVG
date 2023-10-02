using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HandGesture { None, Pass, Set };
public class HandGestures : MonoBehaviour
{
    public Transform rightHandTransform;
    public Transform leftHandTransform;
    public Transform headTransform;

    [SerializeField]
    private Hand rightHandController;
    [SerializeField]
    private Hand leftHandController;

    public HandGesture currentGesture { get; private set; }
    private float handDistanceThreshold = 0.2f;
    public delegate void OnGestureAction(HandGesture formerGesture, HandGesture currentGesture);
    public static OnGestureAction OnGestureChanged;

    void Start()
    {
        currentGesture = HandGesture.None;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(headTransform.position.x, headTransform.position.y - 0.3f, headTransform.position.z);
        if (
            CheckSettingTriggersActive() // &&
            // CheckHandDistanceThreshold() &&
            // CheckHandsAboveHead()
        )
        {
            TryUpdateHandGesture(HandGesture.Set);
        }

        else if (CheckHandDistanceThreshold())
        {
            TryUpdateHandGesture(HandGesture.Pass);
        }

        else {
            TryUpdateHandGesture(HandGesture.None);
        }
    }

    private void TryUpdateHandGesture(HandGesture gesture)
    {
        if (currentGesture != gesture)
        {
            HandGesture formerGesture = currentGesture;
            currentGesture = gesture;
            OnGestureChanged(formerGesture, currentGesture);
        }
    }

    private bool CheckHandDistanceThreshold() 
    {
        return Vector3.Distance(rightHandTransform.position, leftHandTransform.position) < handDistanceThreshold;
    }

    private bool CheckHandsAboveHead()
    {
        return 
            rightHandTransform.position.y > headTransform.position.y &&
            leftHandTransform.position.y > headTransform.position.y;
    }

    private bool CheckSettingTriggersActive()
    {
        return rightHandController.GetTriggerPressed() && leftHandController.GetTriggerPressed();
    }
}
