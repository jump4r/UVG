using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTurn : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty rightHandSnapTurnAction;
    [SerializeField]
    private float turnAmount = 45;

    public float debounceTime = 0.5f;
    private float timeStarted = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeStarted > 0f && (timeStarted + debounceTime) < Time.time)
        {
            timeStarted = 0f;
            return;
        }

        Vector2 rightHandValue = rightHandSnapTurnAction.action?.ReadValue<Vector2>() ?? Vector2.zero;
        
        if (Mathf.Abs(rightHandValue.x) > 0.7f)
        {
            RotatePlayer(rightHandValue.x > 0f ? 1f : -1f);
        }
    }

    void RotatePlayer(float direction)
    {
        if (timeStarted > 0f)
        {
            return;
        }

        transform.Rotate(new Vector3(0, turnAmount * direction, 0));
        timeStarted = Time.time;
    }
}
